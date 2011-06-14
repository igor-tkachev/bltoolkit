using System;
using System.Linq;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class JoinBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			if (!methodCall.IsQueryable("Join", "GroupJoin") || methodCall.Arguments.Count != 5)
				return false;

			var body = ((LambdaExpression)methodCall.Arguments[2].Unwrap()).Body.Unwrap();

			if (body.NodeType == ExpressionType	.MemberInit)
			{
				var mi = (MemberInitExpression)body;
				bool throwExpr;

				if (mi.NewExpression.Arguments.Count > 0 || mi.Bindings.Count == 0)
					throwExpr = true;
				else
					throwExpr = mi.Bindings.Any(b => b.BindingType != MemberBindingType.Assignment);

				if (throwExpr)
					throw new NotSupportedException(string.Format("Explicit construction of entity type '{0}' in join is not allowed.", body.Type));
			}

			return true;
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var isGroup      = methodCall.Method.Name == "GroupJoin";
			var outerContext = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0], buildInfo.SqlQuery));
			var innerContext = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[1], new SqlQuery()));
			var countContext = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[1], new SqlQuery()));

			var context  = new SubQueryContext(outerContext);
			innerContext = isGroup ? new GroupJoinSubQueryContext(innerContext, methodCall) : new SubQueryContext(innerContext);
			countContext = new SubQueryContext(countContext);

			var join = innerContext.SqlQuery.InnerJoin();
			var sql  = context.SqlQuery;

			sql.From.Tables[0].Joins.Add(join.JoinedTable);

			var selector = (LambdaExpression)methodCall.Arguments[4].Unwrap();

			context.SetAlias(selector.Parameters[0].Name);
			innerContext.SetAlias(selector.Parameters[1].Name);

			var outerKeyLambda = ((LambdaExpression)methodCall.Arguments[2].Unwrap());
			var innerKeyLambda = ((LambdaExpression)methodCall.Arguments[3].Unwrap());

			var outerKeySelector = outerKeyLambda.Body.Unwrap();
			var innerKeySelector = innerKeyLambda.Body.Unwrap();

			var outerParent = context.     Parent;
			var innerParent = innerContext.Parent;
			var countParent = countContext.Parent;

			var outerKeyContext = new ExpressionContext(buildInfo.Parent, context,      outerKeyLambda);
			var innerKeyContext = new ExpressionContext(buildInfo.Parent, innerContext, innerKeyLambda);
			var countKeyContext = new ExpressionContext(buildInfo.Parent, countContext, innerKeyLambda);

			// Process counter.
			//
			var counterSql = ((SubQueryContext)countContext).SqlQuery;

			// Make join and where for the counter.
			//
			if (outerKeySelector.NodeType == ExpressionType.New)
			{
				var new1 = (NewExpression)outerKeySelector;
				var new2 = (NewExpression)innerKeySelector;

				for (var i = 0; i < new1.Arguments.Count; i++)
				{
					var arg1 = new1.Arguments[i];
					var arg2 = new2.Arguments[i];

					BuildJoin(builder, join, outerKeyContext, arg1, innerKeyContext, arg2, countKeyContext, counterSql);
				}
			}
			else if (outerKeySelector.NodeType == ExpressionType.MemberInit)
			{
				var mi1 = (MemberInitExpression)outerKeySelector;
				var mi2 = (MemberInitExpression)innerKeySelector;

				for (var i = 0; i < mi1.Bindings.Count; i++)
				{
					if (mi1.Bindings[i].Member != mi2.Bindings[i].Member)
						throw new LinqException(string.Format("List of member inits does not match for entity type '{0}'.", outerKeySelector.Type));

					var arg1 = ((MemberAssignment)mi1.Bindings[i]).Expression;
					var arg2 = ((MemberAssignment)mi2.Bindings[i]).Expression;

					BuildJoin(builder, join, outerKeyContext, arg1, innerKeyContext, arg2, countKeyContext, counterSql);
				}
			}
			else
			{
				BuildJoin(builder, join, outerKeyContext, outerKeySelector, innerKeyContext, innerKeySelector, countKeyContext, counterSql);
			}

			context.     Parent = outerParent;
			innerContext.Parent = innerParent;
			countContext.Parent = countParent;

			if (isGroup)
			{
				counterSql.ParentSql = sql;
				counterSql.Select.Columns.Clear();

				var inner = (GroupJoinSubQueryContext)innerContext;

				inner.Join       = join.JoinedTable;
				inner.CounterSql = counterSql;
				return new GroupJoinContext(buildInfo.Parent, selector, context, inner);
			}

			return new JoinContext(buildInfo.Parent, selector, context, innerContext)
#if DEBUG
			{
				MethodCall = methodCall
			}
#endif
				;
		}

		static void BuildJoin(
			ExpressionBuilder        builder,
			SqlQuery.FromClause.Join join,
			ExpressionContext outerKeyContext, Expression outerKeySelector,
			ExpressionContext innerKeyContext, Expression innerKeySelector,
			ExpressionContext countKeyContext, SqlQuery countSql)
		{
			var predicate = builder.ConvertObjectComparison(
				ExpressionType.Equal,
				outerKeyContext, outerKeySelector,
				innerKeyContext, innerKeySelector);

			if (predicate != null)
				join.JoinedTable.Condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			else
				join
					.Expr(builder.ConvertToSql(outerKeyContext, outerKeySelector)).Equal
					.Expr(builder.ConvertToSql(innerKeyContext, innerKeySelector));

			predicate = builder.ConvertObjectComparison(
				ExpressionType.Equal,
				outerKeyContext, outerKeySelector,
				countKeyContext, innerKeySelector);

			if (predicate != null)
				countSql.Where.SearchCondition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			else
				countSql.Where
					.Expr(builder.ConvertToSql(outerKeyContext, outerKeySelector)).Equal
					.Expr(builder.ConvertToSql(countKeyContext, innerKeySelector));
		}

		internal class JoinContext : SelectContext
		{
			public JoinContext(IBuildContext parent, LambdaExpression lambda, IBuildContext outerContext, IBuildContext innerContext)
				: base(parent, lambda, outerContext, innerContext)
			{
			}
		}

		internal class GroupJoinContext : JoinContext
		{
			public GroupJoinContext(
				IBuildContext            parent,
				LambdaExpression         lambda,
				IBuildContext            outerContext,
				GroupJoinSubQueryContext innerContext)
				: base(parent, lambda, outerContext, innerContext)
			{
				innerContext.GroupJoin = this;
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				if (expression == Lambda.Parameters[1])
				{
					return Expression.Constant(null, Lambda.Parameters[1].Type);
				}

				return base.BuildExpression(expression, level);
			}
		}

		internal class GroupJoinSubQueryContext : SubQueryContext
		{
			readonly MethodCallExpression _methodCall;

			public SqlQuery.JoinedTable Join;
			public SqlQuery             CounterSql;
			public GroupJoinContext     GroupJoin;

			public GroupJoinSubQueryContext(IBuildContext subQuery, MethodCallExpression methodCall)
				: base(subQuery)
			{
				_methodCall = methodCall;
			}

			public override IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				if (expression == null)
					return this;

				return base.GetContext(expression, level, buildInfo);
			}

			Expression _counterExpression;
			SqlInfo[]  _counterInfo;

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				if (expression != null && expression == _counterExpression)
					return _counterInfo ?? (_counterInfo = new[]
					{
						new SqlInfo
						{
							Query = CounterSql.ParentSql,
							Index = CounterSql.ParentSql.Select.Add(CounterSql),
							Sql   = CounterSql
						}
					});

				return base.ConvertToIndex(expression, level, flags);
			}

			public SqlQuery GetCounter(Expression expr)
			{
				Join.IsWeak = true;

				_counterExpression = expr;

				return CounterSql;
			}
		}
	}
}
