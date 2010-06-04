using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class Query<T> : Table<T>
	{
		public Query()
		{
		}

		public Query(DbManager dbManager)
			: base(dbManager)
		{
		}

		public Query(Expression expression)
			: base(expression)
		{
		}

		public Query(DbManager dbManager, Expression expression)
			: base(dbManager, expression)
		{
		}

		public new string SqlText
		{
			get { return base.SqlText; }
		}

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return base.SqlText;
		}

#endif
	}
}
