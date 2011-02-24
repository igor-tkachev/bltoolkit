using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class WhereParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Where");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence  = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);
			var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();
			var result    = parser.ParseWhere(parent, sequence, condition, true);

			result.SetAlias(condition.Parameters[0].Name);

			return result;
		}
	}
}
