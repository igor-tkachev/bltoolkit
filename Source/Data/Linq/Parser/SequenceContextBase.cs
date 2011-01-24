using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using Data.Sql;

	abstract class SequenceContextBase : IParseContext
	{
		protected SequenceContextBase(IParseContext sequence, LambdaExpression lambda)
		{
			Sequence = sequence;
			Parser   = sequence.Parser;
			Lambda   = lambda;
			SqlQuery = sequence.SqlQuery;
		}

		public IParseContext    Sequence { get; set; }
		public ExpressionParser Parser   { get; set; }
		public LambdaExpression Lambda   { get; set; }
		public SqlQuery         SqlQuery { get; set; }
		public IParseContext    Root
		{
			get { return Sequence.Root;  }
			set { Sequence.Root = value; }
		}

		Expression IParseContext.Expression { get { return Lambda; } }

		public abstract Expression       BuildQuery     ();
		public abstract Expression       BuildExpression(Expression expression, int level);
		public abstract ISqlExpression[] ConvertToSql   (Expression expression, int level, ConvertFlags flags);
		public abstract int[]            ConvertToIndex (Expression expression, int level, ConvertFlags flags);
		public abstract bool             IsExpression   (Expression expression, int level, RequestFor requestFlag);
		public abstract IParseContext    GetContext     (Expression expression, int level, SqlQuery currentSql);

		public virtual void SetAlias(string alias)
		{
		}
	}
}
