using System;
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

		static SqlQueryInfo _selectByKeyQuery;

		public virtual T SelectByKey(DbManager db, params object[] keys)
		{
			if (_selectByKeyQuery == null)
				_selectByKeyQuery = GetSqlQueryInfo(db, typeof(T), "SelectByKey");

			return db
				.SetCommand(_selectByKeyQuery.QueryText, _selectByKeyQuery.GetParameters(db, keys))
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

		static SqlQueryInfo _selectAllQuery;

		public virtual List<T> SelectAll(DbManager db)
		{
			if (_selectAllQuery == null)
				_selectAllQuery = GetSqlQueryInfo(db, typeof(T), "SelectAll");

			return db
				.SetCommand(_selectAllQuery.QueryText)
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

		static SqlQueryInfo _insertQuery;

		public virtual int Insert(DbManager db, T obj)
		{
			if (_insertQuery == null)
				_insertQuery = GetSqlQueryInfo(db, obj.GetType(), "Insert");

			return db
				.SetCommand(_insertQuery.QueryText, _insertQuery.GetParameters(db, obj))
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

		static SqlQueryInfo _insertBatchQuery;

		public virtual int Insert(DbManager db, int maxBatchSize, IEnumerable<T> list)
		{
			if (_insertBatchQuery == null)
				_insertBatchQuery = GetSqlQueryInfo(db, typeof(T), "InsertBatch");

			return db
				.SetCommand(_insertBatchQuery.QueryText)
				.ExecuteForEach(
					list,
					_insertBatchQuery.GetMemberMappers(),
					maxBatchSize,
					delegate(T obj) { return _insertBatchQuery.GetParameters(db, obj); });
		}

		public virtual int Insert(int maxBatchSize, IEnumerable<T> list)
		{
			DbManager db = GetDbManager();

			try
			{
				return Insert(db, maxBatchSize, list);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public virtual int Insert(DbManager db, IEnumerable<T> list)
		{
			return Insert(db, int.MaxValue, list);
		}

		public virtual int Insert(IEnumerable<T> list)
		{
			return Insert(int.MaxValue, list);
		}

		#endregion

		#region Update

		static SqlQueryInfo _updateQuery;

		public virtual int Update(DbManager db, T obj)
		{
			if (_updateQuery == null)
				_updateQuery = GetSqlQueryInfo(db, obj.GetType(), "Update");

			return db
				.SetCommand(_updateQuery.QueryText, _updateQuery.GetParameters(db, obj))
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

		static SqlQueryInfo _updateBatchQuery;

		public virtual int Update(DbManager db, int maxBatchSize, IEnumerable<T> list)
		{
			if (_updateBatchQuery == null)
				_updateBatchQuery = GetSqlQueryInfo(db, typeof(T), "UpdateBatch");

			return db
				.SetCommand(_updateBatchQuery.QueryText)
				.ExecuteForEach(
					list,
					_updateBatchQuery.GetMemberMappers(),
					maxBatchSize,
					delegate(T obj) { return _updateBatchQuery.GetParameters(db, obj); });
		}

		public virtual int Update(int maxBatchSize, IEnumerable<T> list)
		{
			DbManager db = GetDbManager();

			try
			{
				return Update(db, maxBatchSize, list);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public virtual int Update(DbManager db, IEnumerable<T> list)
		{
			return Update(db, int.MaxValue, list);
		}

		public virtual int Update(IEnumerable<T> list)
		{
			return Update(int.MaxValue, list);
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

		static SqlQueryInfo _deleteQuery;

		public virtual int Delete(DbManager db, T obj)
		{
			if (_deleteQuery == null)
				_deleteQuery = GetSqlQueryInfo(db, obj.GetType(), "Delete");

			return db
				.SetCommand(_deleteQuery.QueryText, _deleteQuery.GetParameters(db, obj))
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

		static SqlQueryInfo _deleteBatchQuery;

		public virtual int Delete(DbManager db, int maxBatchSize, IEnumerable<T> list)
		{
			if (_deleteBatchQuery == null)
				_deleteBatchQuery = GetSqlQueryInfo(db, typeof(T), "DeleteBatch");

			return db
				.SetCommand(_deleteBatchQuery.QueryText)
				.ExecuteForEach(
					list,
					_deleteBatchQuery.GetMemberMappers(),
					maxBatchSize,
					delegate(T obj) { return _deleteBatchQuery.GetParameters(db, obj); });
		}

		public virtual int Delete(int maxBatchSize, IEnumerable<T> list)
		{
			DbManager db = GetDbManager();

			try
			{
				return Delete(db, list);
			}
			finally
			{
				if (DisposeDbManager)
					db.Dispose();
			}
		}

		public virtual int Delete(DbManager db, IEnumerable<T> list)
		{
			return Delete(db, int.MaxValue, list);
		}

		public virtual int Delete(IEnumerable<T> list)
		{
			return Delete(int.MaxValue, list);
		}


		#endregion
	}
}
