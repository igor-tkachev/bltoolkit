using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class DefaultIfEmptyParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("DefaultIfEmpty");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence     = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
			var defaultValue = methodCall.Arguments.Count == 1 ? null : methodCall.Arguments[1].Unwrap();

			return new DefaultIfEmptyContext(sequence, defaultValue);
		}

		public class DefaultIfEmptyContext : SequenceContextBase
		{
			public DefaultIfEmptyContext(IParseContext sequence, Expression defaultValue) 
				: base(sequence, null)
			{
				_defaultValue = defaultValue;
			}

			private readonly Expression _defaultValue;

			public override Expression BuildExpression(Expression expression, int level)
			{
				var expr = Sequence.BuildExpression(expression, level);

				if (expression == null)
				{
					var idx = Sequence.ConvertToIndex(null, 0, ConvertFlags.Key);

					Expression cond = null;

					foreach (var info in idx)
					{
						var n = ConvertToParentIndex(info.Index, this);

						var e = Expression.Call(
							ExpressionParser.DataReaderParam,
							ReflectionHelper.DataReader.IsDBNull,
							Expression.Constant(n)) as Expression;

						cond = cond == null ? e : Expression.AndAlso(cond, e);
					}

					var defaultValue = _defaultValue ?? Expression.Constant(null, expr.Type);

					expr = Expression.Condition(cond, defaultValue, expr);
				}

				return expr;
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToSql(expression, level, flags);
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				return Sequence.ConvertToIndex(expression, level, flags);
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				return Sequence.IsExpression(expression, level, requestFlag);
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				return Sequence.GetContext(expression, level, currentSql);
			}
		}
	}
}
