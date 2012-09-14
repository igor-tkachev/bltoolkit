using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	public class Table<T> : ExpressionQuery<T>, ITable
	{
		public Table(IDataContextInfo dataContextInfo, Expression expression)
		{
			Init(dataContextInfo, expression);
		}

		public Table(IDataContextInfo dataContextInfo)
		{
			Init(dataContextInfo, null);
		}

#if !SILVERLIGHT

		public Table()
		{
			Init(null, null);
		}

		public Table(Expression expression)
		{
			Init(null, expression);
		}
		
#endif

        public Table(IDataContext dataContext)
            : this(CtorFix(dataContext), null)
        {
        }

        public Table(IDataContext dataContext, Expression expression)
            : this(CtorFix(dataContext), expression)
        {
        }

        //fixes .NET 4.5 CLR bug mentioned in this blog post
        //http://elegantcode.com/2012/08/23/net-4-5-operation-could-destabilize-the-runtime-yikes/
        //can be reverted if/once the runtime bug is fixed
        private static IDataContextInfo CtorFix(IDataContext dataContext)
        {
            return dataContext == null ? null : new DataContextInfo(dataContext);
        }

		#region Overrides

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return "Table(" + typeof (T).Name + ")";
		}

#endif

		#endregion
	}
}
