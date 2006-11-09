using System;
using System.Collections.Generic;

using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	public class SprocQuery<T> : DataAccessBase
	{
		#region Constructors

		public SprocQuery()
		{
		}

		public SprocQuery(DbManager dbManager)
			: base(dbManager)
		{
		}

		public SprocQuery(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		#endregion

		#region SelectByKey

		public virtual T SelectByKey(DbManager db, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectByKey"), key)
				.ExecuteObject<T>();
		}

		public virtual T SelectByKey(params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey(db, key);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region SelectAll

		public virtual List<T> SelectAll(DbManager db)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectAll"))
				.ExecuteList<T>();
		}

		public virtual List<T> SelectAll()
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll(db);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region Insert

		public virtual void Insert(DbManager db, T obj)
		{
			db
			  .SetSpCommand(
					GetSpName(obj.GetType(), "Insert"),
					db.CreateParameters(obj))
			  .ExecuteNonQuery();
		}

		public virtual void Insert(T obj)
		{
			DbManager db = GetDbManager();

			try
			{
				Insert(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region Update

		public virtual int Update(DbManager db, T obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Update"),
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public virtual int Update(T obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Update(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region DeleteByKey

		public virtual int DeleteByKey(DbManager db, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "Delete"), key)
				.ExecuteNonQuery();
		}

		public virtual int DeleteByKey(params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteByKey(db, key);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region Delete

		public virtual int Delete(DbManager db, T obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Delete"), 
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public virtual int Delete(T obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Delete(db, obj);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion
	}
}
