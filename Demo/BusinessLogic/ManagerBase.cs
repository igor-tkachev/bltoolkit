using System;

using BLToolkit.EditableObjects;

using BLToolkit.Demo.ObjectModel;
using BLToolkit.Demo.BusinessLogic.DataAccess;

namespace BLToolkit.Demo.BusinessLogic
{
	public abstract class ManagerBase<T>
		where T : BizEntity
	{
		#region Insert, Update, Delete

		public void Insert(T obj)
		{
			obj.Validate();

			obj.ID = DataAccessor.Insert(obj);

			obj.AcceptChanges();
		}

		public void Update(T obj)
		{
			obj.Validate();
			
			DataAccessor.UpdateSql(obj);

			obj.AcceptChanges();
		}

		public void Delete(T obj)
		{
			DataAccessor.DeleteSql(obj);
		}

		public void Delete(int id)
		{
			DataAccessor.DeleteByKeySql(id);
		}

		#endregion

		#region Select

		public EditableList<T> SelectAll()
		{
			EditableList<T> list = new EditableList<T>();

			return DataAccessor.SelectAllSql(list);
		}

		#endregion

		#region Protected Members

		protected abstract AccessorBase<T> DataAccessor { get; }

		#endregion
	}
}
