using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	class SelectParser : MethodCallParser
	{
		protected override ParseInfo ParseMethodCall(ExpressionParser parser, MethodCallExpression expression)
		{
			if (!IsQueryable("Select", expression.Method))
				return null;

			var selector = (LambdaExpression)expression.Arguments[1].Unwrap();

			if (selector.Parameters.Count != 1)
				return null;

			var body      = selector.Body.Unwrap();
			var baseQuery = parser.ParseSequence(expression.Arguments[0]);

			baseQuery.SetAlias(selector.Parameters[0].Name);

			if (body == selector.Parameters[0])
				return baseQuery;

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
	}
}
