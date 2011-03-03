using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	public abstract class SequenceContextBase : IParseContext
	{
		protected SequenceContextBase(IParseContext parent, IParseContext sequence, LambdaExpression lambda)
		{
			Parent   = parent;
			Sequence = sequence;
			Parser   = sequence.Parser;
			Lambda   = lambda;
			SqlQuery = sequence.SqlQuery;

			Sequence.Parent = this;
		}

#if DEBUG
		[CLSCompliant(false)]
		public string _sqlQueryText { get { return SqlQuery == null ? "" : SqlQuery.SqlText; } }
#endif

		public IParseContext    Parent   { get; set; }
		public IParseContext    Sequence { get; set; }
		public ExpressionParser Parser   { get; set; }
		public LambdaExpression Lambda   { get; set; }
		public SqlQuery         SqlQuery { get; set; }

		Expression IParseContext.Expression { get { return Lambda; } }

		public virtual void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
		{
			var expr = BuildExpression(null, 0);

			var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],T>>(
				expr, new []
				{
					ExpressionParser.ContextParam,
					ExpressionParser.DataContextParam,
					ExpressionParser.DataReaderParam,
					ExpressionParser.ExpressionParam,
					ExpressionParser.ParametersParam,
				});

			query.SetQuery(mapper.Compile());
		}

		public abstract Expression    BuildExpression(Expression expression, int level);
		public abstract SqlInfo[]     ConvertToSql   (Expression expression, int level, ConvertFlags flags);
		public abstract SqlInfo[]     ConvertToIndex (Expression expression, int level, ConvertFlags flags);
		public abstract bool          IsExpression   (Expression expression, int level, RequestFor requestFlag);
		public abstract IParseContext GetContext     (Expression expression, int level, SqlQuery currentSql);

		public virtual int ConvertToParentIndex(int index, IParseContext context)
		{
			return Parent == null ? index : Parent.ConvertToParentIndex(index, context);
		}

		public virtual void SetAlias(string alias)
		{
		}

		protected bool IsSubQuery()
		{
			for (var p = Parent; p != null; p = p.Parent)
				if (p.IsExpression(null, 0, RequestFor.SubQuery))
					return true;
			return false;
		}
	}
}
