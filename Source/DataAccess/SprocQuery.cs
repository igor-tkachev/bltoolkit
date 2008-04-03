using System;
using System.Collections;

using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	public class SprocQuery : DataAccessBase
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

		public virtual object SelectByKey(DbManager db, Type type, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectByKey"), key)
				.ExecuteObject(type);
		}

		public virtual object SelectByKey(Type type, params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey(db, type, key);
			}
			finally
			{
				Dispose(db);
			}
		}

		public virtual T SelectByKey<T>(DbManager db, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectByKey"), key)
				.ExecuteObject<T>();
		}

		public virtual T SelectByKey<T>(params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey<T>(db, key);
			}
			finally
			{
				Dispose(db);
			}
		}

		#endregion

		#region SelectAll

		public virtual ArrayList SelectAll(DbManager db, Type type)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectAll"))
				.ExecuteList(type);
		}

		public virtual IList SelectAll(DbManager db, IList list, Type type)
		{
			return db
				.SetSpCommand(GetSpName(type, "SelectAll"))
				.ExecuteList(list, type);
		}

		public virtual ArrayList SelectAll(Type type)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll(db, type);
			}
			finally
			{
				Dispose(db);
			}
		}

		public virtual IList SelectAll(IList list, Type type)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll(db, list, type);
			}
			finally
			{
				Dispose(db);
			}
		}

		public virtual System.Collections.Generic.List<T> SelectAll<T>(DbManager db)
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectAll"))
				.ExecuteList<T>();
		}

		public virtual L SelectAll<L,T>(DbManager db, L list)
			where L : System.Collections.Generic.IList<T>
		{
			return db
				.SetSpCommand(GetSpName(typeof(T), "SelectAll"))
				.ExecuteList<L,T>(list);
		}

		public virtual L SelectAll<L, T>(DbManager db)
			where L : System.Collections.Generic.IList<T>, new()
		{
			return SelectAll<L, T>(db, new L());
		}

		public virtual System.Collections.Generic.List<T> SelectAll<T>()
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll<T>(db);
			}
			finally
			{
				Dispose(db);
			}
		}

		public virtual L SelectAll<L,T>(L list)
			where L : System.Collections.Generic.IList<T>
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll<L,T>(db, list);
			}
			finally
			{
				Dispose(db);
			}
		}

		public virtual L SelectAll<L,T>()
			where L : System.Collections.Generic.IList<T>, new()
		{
			return SelectAll<L,T>(new L());
		}

		#endregion

		#region Insert

		public virtual void Insert(DbManager db, object obj)
		{
			db
			  .SetSpCommand(
					GetSpName(obj.GetType(), "Insert"),
					db.CreateParameters(obj))
			  .ExecuteNonQuery();
		}

		public virtual void Insert(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				Insert(db, obj);
			}
			finally
			{
				Dispose(db);
			}
		}

		#endregion

		#region Update

		public virtual int Update(DbManager db, object obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Update"),
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public virtual int Update(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Update(db, obj);
			}
			finally
			{
				Dispose(db);
			}
		}

		#endregion

		#region DeleteByKey

		public virtual int DeleteByKey(DbManager db, Type type, params object[] key)
		{
			return db
				.SetSpCommand(GetSpName(type, "Delete"), key)
				.ExecuteNonQuery();
		}

		public virtual int DeleteByKey(Type type, params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteByKey(db, type, key);
			}
			finally
			{
				Dispose(db);
			}
		}

		#endregion

		#region Delete

		public virtual int Delete(DbManager db, object obj)
		{
			return db
				.SetSpCommand(
					GetSpName(obj.GetType(), "Delete"), 
					db.CreateParameters(obj))
				.ExecuteNonQuery();
		}

		public virtual int Delete(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Delete(db, obj);
			}
			finally
			{
				Dispose(db);
			}
		}

		#endregion
	}
}
