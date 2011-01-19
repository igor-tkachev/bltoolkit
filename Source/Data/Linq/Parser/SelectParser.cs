using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class SelectParser : MethodCallParser
	{
		protected override IParseInfo ParseMethodCall(IParseInfo parseInfo, MethodCallExpression expression)
		{
			if (!IsQueryable("Select", expression.Method))
				return null;

			var selector = (LambdaExpression)expression.Arguments[1].Unwrap();

			if (selector.Parameters.Count != 1)
				return null;

			var info      = new SelectInfo(parseInfo, expression);
			var sequence = parseInfo.Parser.ParseSequence(info, expression.Arguments[0]);

			var body = selector.Body.Unwrap();

			switch (body.NodeType)
			{
				case ExpressionType.New :
					break;

				case ExpressionType.MemberInit :
					break;

				default :
					break;
			}

			sequence.SetAlias(selector.Parameters[0].Name);

			if (body == selector.Parameters[0])
				return sequence;

			//if (parser.SqlQuery.Select.IsDistinct)
			//	baseQuery = new SubQueryInfo(baseQuery);

			switch (body.NodeType)
			{
				case ExpressionType.New:
					break;

				case ExpressionType.MemberInit:
					break;

				default:
					break;
			}

			return null;
		}

		class SelectInfo : ParseInfoBase
		{
			public SelectInfo(IParseInfo parseInfo, Expression expression) : base(parseInfo, expression)
			{
			}

			public override Expression BuildExpression(IParseInfo rootParse, Expression expression, int level)
			{
				throw new NotImplementedException();
			}

			public override IEnumerable<ISqlExpression> ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override IEnumerable<int> ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}
		}
	}
}
