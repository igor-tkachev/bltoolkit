using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class ScalarSelectBuilder : ISequenceBuilder
	{
		public int BuildCounter { get; set; }

		public bool CanBuild(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			return
				buildInfo.Expression.NodeType == ExpressionType.Lambda &&
				((LambdaExpression)buildInfo.Expression).Parameters.Count == 0;
		}

		public IBuildContext BuildSequence(ExpressionBuilder builder, BuildInfo buildInfo)
		{
			return new ScalarSelectContext
			{
				Builder     = builder,
				Parent     = buildInfo.Parent,
				Expression = buildInfo.Expression,
				SqlQuery   = buildInfo.SqlQuery
			};
		}

		class ScalarSelectContext : IBuildContext
		{
#if DEBUG
			public string _sqlQueryText { get { return SqlQuery == null ? "" : SqlQuery.SqlText; } }
#endif

			public ExpressionBuilder Builder     { get; set; }
			public Expression       Expression { get; set; }
			public SqlQuery         SqlQuery   { get; set; }
			public IBuildContext    Parent     { get; set; }

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
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

			public Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
					expression = ((LambdaExpression)Expression).Body.Unwrap();

				switch (expression.NodeType)
				{
					case ExpressionType.New:
					case ExpressionType.MemberInit:
						{
							var expr = Builder.BuildExpression(this, expression);

							if (SqlQuery.Select.Columns.Count == 0)
								SqlQuery.Select.Expr(new SqlValue(1));

							return expr;
						}

					default :
						{
							var expr = Builder.ConvertToSql(this, expression);
							var idx  = SqlQuery.Select.Add(expr);

							return Builder.BuildSql(expression.Type, idx);
						}
				}

			}

			public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression : return true;
					default                    : return false;
				}
			}

			public IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				throw new NotImplementedException();
			}

			public int ConvertToParentIndex(int index, IBuildContext context)
			{
				throw new NotImplementedException();
			}

			public void SetAlias(string alias)
			{
			}

			public ISqlExpression GetSubQuery()
			{
				return SqlQuery;
			}
		}
	}
}
