using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;

	class DistinctParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			return methodCall.IsQueryable("Distinct");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, ParseInfo parseInfo)
		{
			var sequence = parser.ParseSequence(new ParseInfo(parseInfo, methodCall.Arguments[0]));
			var sql      = sequence.SqlQuery;

			if (sql.Select.TakeValue != null || sql.Select.SkipValue != null)
				sequence = new SubQueryContext(sequence);

			sequence.SqlQuery.Select.IsDistinct = true;

			return sequence;
		}
	}
}
