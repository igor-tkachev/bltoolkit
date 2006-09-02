using System;

using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;

using BLToolkit.Demo.ObjectModel;
using BLToolkit.Demo.BusinessLogic.DataAccess;

namespace BLToolkit.Demo.BusinessLogic
{
	public abstract class ManagerBase<T,A>
		where T : BizEntity
		where A : AccessorBase<T,A>
	{
		#region Insert, Update, Delete

		public void Insert(T obj)
		{
			obj.Validate();

			obj.ID = Accessor.Insert(obj);

			obj.AcceptChanges();
		}

		public void Update(T obj)
		{
			obj.Validate();
			
			Query.Update(obj);

			obj.AcceptChanges();
		}

		public void Delete(T obj)
		{
			Query.Delete(obj);
		}

		public void Delete(int id)
		{
			Query.DeleteByKey(id);
		}

		#endregion

		#region Select

		public EditableList<T> SelectAll()
		{
			EditableList<T> list = new EditableList<T>();

			return Query.SelectAll(list);
		}

		#endregion

		#region Protected Members

		protected A Accessor
		{
			get { return AccessorBase<T,A>.CreateInstance(); }
		}

		private            SqlQuery<T>   _query;
		protected virtual  SqlQuery<T>    Query
		{
			get
			{
				if (null == _query)
					_query = new SqlQuery<T>();

				return _query;
			}
		}

		#endregion
	}
}
