using System;
using System.Collections;

using BLToolkit.Data;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public class SqlQuery : SqlQueryBase
	{
		#region Constructors

		public SqlQuery()
		{
		}

		public SqlQuery(DbManager dbManager)
			: base(dbManager)
		{
		}

		public SqlQuery(DbManager dbManager, bool dispose)
			: base(dbManager, dispose)
		{
		}

		public SqlQuery(ExtensionList extensions)
		{
			Extensions = extensions;
		}

		#endregion

		#region Overrides

		public SqlQueryInfo GetSqlQueryInfo<T>(DbManager db, string actionName)
		{
			return base.GetSqlQueryInfo(db, typeof(T), actionName);
		}

		#endregion

		#region SelectByKey

		public virtual object SelectByKey(DbManager db, Type type, params object[] keys)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectByKey");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, keys))
				.ExecuteObject(type);
		}

		public virtual object SelectByKey(Type type, params object[] keys)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey(db, type, keys);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public virtual T SelectByKey<T>(DbManager db, params object[] keys)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectByKey");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, keys))
				.ExecuteObject<T>();
		}

		public virtual T SelectByKey<T>(params object[] keys)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey<T>(db, keys);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region SelectAll

		public virtual ArrayList SelectAll(DbManager db, Type type)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList(type);
		}

		public virtual IList SelectAll(DbManager db, IList list, Type type)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "SelectAll");

			return db
				.SetCommand(query.QueryText)
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
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList<T>();
		}

		public virtual L SelectAll<L,T>(DbManager db, L list)
			where L : System.Collections.Generic.IList<T>
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList<L,T>(list);
		}

		public virtual L SelectAll<L,T>(DbManager db)
			where L : System.Collections.Generic.IList<T>, new()
		{
			return SelectAll<L,T>(db, new L());
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

		public virtual int Insert(DbManager db, object obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Insert");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
				.ExecuteNonQuery();
		}

		public virtual int Insert(object obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Insert(db, obj);
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
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Update");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
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
			SqlQueryInfo query = GetSqlQueryInfo(db, type, "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, key))
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

		public virtual int DeleteByKey<T>(DbManager db, params object[] key)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, key))
				.ExecuteNonQuery();
		}

		public virtual int DeleteByKey<T>(params object[] key)
		{
			DbManager db = GetDbManager();

			try
			{
				return DeleteByKey<T>(db, key);
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
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
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
