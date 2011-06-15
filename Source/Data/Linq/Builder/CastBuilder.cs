using System;
using System.Linq.Expressions;
using BLToolkit.Data.Sql;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;

	class CastBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Cast");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return new CastContext(builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0])));
		}

		class CastContext : IBuildContext
		{
			readonly IBuildContext _context;

			public CastContext(IBuildContext context)
			{
				_context = context;
			}

			public string            _sqlQueryText { get { return _context._sqlQueryText; } }
			public ExpressionBuilder Builder       { get { return _context.Builder;       } }
			public Expression        Expression    { get { return _context.Expression;    } }

			public SqlQuery SqlQuery
			{
				get { return _context.SqlQuery;  }
				set { _context.SqlQuery = value; }
			}

			public IBuildContext Parent
			{
				get { return _context.Parent;  }
				set { _context.Parent = value; }
			}

			public void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				_context.BuildQuery(query, queryParameter);
			}

			public Expression BuildExpression(Expression expression, int level)
			{
				if (expression == null)
				{
					
				}

				return _context.BuildExpression(expression, level);
			}

			public SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				return _context.ConvertToSql(expression, level, flags);
			}

			public SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				return _context.ConvertToIndex(expression, level, flags);
			}

			public bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				return _context.IsExpression(expression, level, requestFlag);
			}

			public IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				return _context.GetContext(expression, level, buildInfo);
			}

			public int ConvertToParentIndex(int index, IBuildContext context)
			{
				return _context.ConvertToParentIndex(index, context);
			}

			public void SetAlias(string alias)
			{
				_context.SetAlias(alias);
			}

			public ISqlExpression GetSubQuery(IBuildContext context)
			{
				return _context.GetSubQuery(context);
			}
		}
	}
}
