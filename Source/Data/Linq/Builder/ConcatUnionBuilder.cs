using System;
using System.Linq.Expressions;
using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class ConcatUnionBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.Arguments.Count == 2 && methodCall.IsQueryable("Concat", "Union");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence1 = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
			var sequence2 = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[1], new SqlQuery()));
			var union     = new SqlQuery.Union(sequence2.SqlQuery, methodCall.Method.Name == "Concat");

			var sq = sequence1 as SubQueryContext;

			if (sq == null || sq.Union != null || !sq.SqlQuery.IsSimple)
				sq = new SubQueryContext(sequence1);

			sq.SubQuery.SqlQuery.Unions.Add(union);
			sq.Union = sequence2;

			return sq;
		}
	}
}
