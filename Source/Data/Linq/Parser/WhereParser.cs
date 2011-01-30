using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	class WhereParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Where");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence  = parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
			var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var result    = parser.ParseWhere(sequence, condition);

			result.SetAlias(condition.Parameters[0].Name);

			return result;
		}
	}
}
