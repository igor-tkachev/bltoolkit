using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class AsQueryableParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("AsQueryable");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return parser.ParseSequence(methodCall.Arguments[0], sqlQuery);
		}
	}
}
