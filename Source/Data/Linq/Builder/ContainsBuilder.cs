using System;
using System.Linq.Expressions;
using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class ContainsBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Contains") && methodCall.Arguments.Count == 2;
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			var context = new ContainsContext(buildInfo, methodCall, sequence);

			return context;
		}

		class ContainsContext : SequenceContextBase
		{
			readonly BuildInfo            _buildInfo;
			readonly MethodCallExpression _methodCall;

			public ContainsContext(BuildInfo buildInfo, MethodCallExpression methodCall, IBuildContext sequence)
				: base(buildInfo.Parent, sequence, null)
			{
				_buildInfo  = buildInfo;
				_methodCall = methodCall;
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				var idx = ConvertToIndex(expression, level, ConvertFlags.Field);
				return Builder.BuildSql(typeof(bool), idx[0].Index);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				if (expression == null)
				{
					var args      = _methodCall.Method.GetGenericArguments();
					var param     = Expression.Parameter(args[0], "param");
					var expr      = _methodCall.Arguments[1];
					var condition = Expression.Lambda(Expression.Equal(param, expr), param);

					IBuildContext ctx = new ExpressionContext(Parent, Sequence, condition);

					ctx = Builder.GetContext(ctx, expr) ?? ctx;

					Sequence.Parent = this;

					SqlQuery.Condition cond;

					if (ctx.IsExpression(expr, 0, RequestFor.Field) || ctx.IsExpression(expr, 0, RequestFor.Expression))
					{
						Sequence.ConvertToIndex(null, 0, ConvertFlags.All);
						var ex = Builder.ConvertToSql(ctx, _methodCall.Arguments[1]);
						cond = new SqlQuery.Condition(false, new SqlQuery.Predicate.InSubQuery(ex, false, SqlQuery));
					}
					else
					{
						var sequence = Builder.BuildWhere(Parent, Sequence, condition, true);
						cond = new SqlQuery.Condition(false, new SqlQuery.Predicate.FuncLike(SqlFunction.CreateExists(sequence.SqlQuery)));
					}

					var sql   = new SqlQuery.SearchCondition(cond);
					var query = SqlQuery;

					if (_buildInfo.Parent != null)
						query = _buildInfo.Parent.SqlQuery;

					return new[] { new SqlInfo { Query = query, Sql = sql } };
				}

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				var sql = ConvertToSql(expression, level, flags);

				if (sql[0].Index < 0)
					sql[0].Index = sql[0].Query.Select.Add(sql[0].Sql);

				return sql;
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				if (expression == null)
				{
					switch (requestFlag)
					{
						case RequestFor.Expression :
						case RequestFor.Field      : return false;
					}
				}

				throw new NotImplementedException();
			}

			public override IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				throw new NotImplementedException();
			}
		}
	}
}
