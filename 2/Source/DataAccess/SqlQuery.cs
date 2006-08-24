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

		public SqlQuery(ExtensionList extensions)
		{
			Extensions = extensions;
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
				if (DisposeDbManager)
					db.Dispose();
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
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion

		#region Insert

		public virtual void Insert(DbManager db, object obj)
		{
			SqlQueryInfo query = GetSqlQueryInfo(db, obj.GetType(), "Insert");

			db
				.SetCommand(query.QueryText, query.GetParameters(db, obj))
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
				if (DisposeDbManager)
					db.Dispose();
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
				if (DisposeDbManager)
					db.Dispose();
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
				if (DisposeDbManager)
					db.Dispose();
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
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		#endregion
	}
}
