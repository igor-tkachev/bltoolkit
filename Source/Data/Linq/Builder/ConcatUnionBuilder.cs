using System;
using System.Data;
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

			return new UnionContext(sq);
		}

		protected override SequenceConvertInfo Convert(
			ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression param)
		{
			return null;
		}

		class UnionContext : PassThroughContext
		{
			public UnionContext(IBuildContext context) : base(context)
			{
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = BuildExpression(null, 0);

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
					expr, new []
					{
						ExpressionBuilder.ContextParam,
						ExpressionBuilder.DataContextParam,
						ExpressionBuilder.DataReaderParam,
						ExpressionBuilder.ExpressionParam,
						ExpressionBuilder.ParametersParam,
					});

				query.SetQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
				{
					var sql = Context.ConvertToSql(null, level, ConvertFlags.All);
				}

				return base.BuildExpression(expression, level);
			}
		}
	}
}
