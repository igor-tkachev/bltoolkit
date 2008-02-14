using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Reflection.Extension;

namespace BLToolkit.DataAccess
{
	public class SqlQuery<T> : SqlQueryBase
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

		public SqlQueryInfo GetSqlQueryInfo(DbManager db, string actionName)
		{
			return base.GetSqlQueryInfo(db, typeof(T), actionName);
		}

		#endregion

		#region SelectByKey

		public virtual T SelectByKey(DbManager db, params object[] keys)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectByKey");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, keys))
				.ExecuteObject<T>();
		}

		public virtual T SelectByKey(params object[] keys)
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectByKey(db, keys);
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
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList<T>();
		}

		public virtual L SelectAll<L>(DbManager db, L list)
			where L : IList<T>
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(query.QueryText)
				.ExecuteList<L,T>(list);
		}

		public virtual L SelectAll<L>(DbManager db)
			where L : IList<T>, new()
		{
			return SelectAll<L>(db, new L());
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

		public virtual L SelectAll<L>(L list)
			where L : IList<T>
		{
			DbManager db = GetDbManager();

			try
			{
				return SelectAll(db, list);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public virtual L SelectAll<L>()
			where L : IList<T>, new()
		{
			return SelectAll<L>(new L());
		}

		#endregion

		#region Insert

		public virtual int Insert(DbManager db, T obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Insert");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
				.ExecuteNonQuery();
		}

		public virtual int Insert(T obj)
		{
			DbManager db = GetDbManager();

			try
			{
				return Insert(db, obj);
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
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Update");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
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
			SqlQueryInfo query = GetSqlQueryInfo(db, typeof(T), "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, key))
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
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Delete");

			return db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
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
