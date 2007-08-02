using System;

using BLToolkit.DataAccess;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.BusinessLogic.DataAccess
{
	public abstract class AccessorBase<T,A> : DataAccessor<T,A>
		where T : BizEntity
		where A : DataAccessor<T,A>
	{
		public abstract int Insert(T obj);
	}
}
