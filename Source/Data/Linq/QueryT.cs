using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class Query<T> : Table<T>
	{
		public Query(IDataContextInfo dataContext, Expression expression)
			: base(dataContext, expression)
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
