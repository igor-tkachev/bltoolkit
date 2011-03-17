using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class TakeSkipParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("Skip", "Take");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));

			var arg = methodCall.Arguments[1].Unwrap();

			if (arg.NodeType == ExpressionType.Lambda)
				arg = ((LambdaExpression)arg).Body.Unwrap();

			var expr = parser.ParseExpression(sequence, arg);

			if (methodCall.Method.Name == "Take")
			{
				ParseTake(parser, sequence, expr);
			}
			else
			{
				ParseSkip(parser, sequence, sequence.SqlQuery.Select.SkipValue, expr);
			}

			return sequence;
		}

		static void ParseTake(ExpressionParser parser, IParseContext sequence, ISqlExpression expr)
		{
			var sql = sequence.SqlQuery;

			parser.SqlProvider.SqlQuery = sql;

			sql.Select.Take(expr);

			if (sql.Select.SkipValue != null && parser.SqlProvider.IsTakeSupported && !parser.SqlProvider.IsSkipSupported)
			{
				if (sql.Select.SkipValue is SqlParameter && sql.Select.TakeValue is SqlValue)
				{
					var skip = (SqlParameter)sql.Select.SkipValue;
					var parm = (SqlParameter)sql.Select.SkipValue.Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);

					parm.SetTakeConverter((int)((SqlValue)sql.Select.TakeValue).Value);

					sql.Select.Take(parm);

					var ep = (from pm in parser.CurrentSqlParameters where pm.SqlParameter == skip select pm).First();

					ep = new ParameterAccessor
					{
						Expression   = ep.Expression,
						Accessor     = ep.Accessor,
						SqlParameter = parm
					};

					parser.CurrentSqlParameters.Add(ep);
				}
				else
					sql.Select.Take(parser.Convert(
						sequence,
						new SqlBinaryExpression(typeof(int), sql.Select.SkipValue, "+", sql.Select.TakeValue, Precedence.Additive)));
			}

			if (!parser.SqlProvider.TakeAcceptsParameter)
			{
				var p = sql.Select.TakeValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}

		static void ParseSkip(ExpressionParser parser, IParseContext sequence, ISqlExpression prevSkipValue, ISqlExpression expr)
		{
			var sql = sequence.SqlQuery;

			parser.SqlProvider.SqlQuery = sql;

			sql.Select.Skip(expr);

			parser.SqlProvider.SqlQuery = sql;

			if (sql.Select.TakeValue != null)
			{
				if (parser.SqlProvider.IsSkipSupported || !parser.SqlProvider.IsTakeSupported)
					sql.Select.Take(parser.Convert(
						sequence,
						new SqlBinaryExpression(typeof(int), sql.Select.TakeValue, "-", sql.Select.SkipValue, Precedence.Additive)));

				if (prevSkipValue != null)
					sql.Select.Skip(parser.Convert(
						sequence,
						new SqlBinaryExpression(typeof(int), prevSkipValue, "+", sql.Select.SkipValue, Precedence.Additive)));
			}

			if (!parser.SqlProvider.TakeAcceptsParameter)
			{
				var p = sql.Select.SkipValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}
	}
}
