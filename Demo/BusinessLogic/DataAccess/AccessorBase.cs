using System;

using BLToolkit.DataAccess;

using BLToolkit.Demo.ObjectModel;

namespace BLToolkit.Demo.BusinessLogic.DataAccess
{
	public abstract class AccessorBase<T> : DataAccessor<T>
		where T : BizEntity
	{
		public new abstract int Insert(T obj);
	}
}
