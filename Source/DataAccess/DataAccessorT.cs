using System;
using System.Collections;
using System.Collections.Generic;

using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
	public class DataAccessor<T> : DataAccessor
	{
		#region Constructors

		public DataAccessor()
		{
		}

		public DataAccessor(DbManager dbManager)
			: base(dbManager)
		{
		}

		#endregion

		#region Overrides

		protected override string GetSpName(Type type, string actionName)
		{
			if (type == null)
				type = typeof(T);

			return base.GetSpName(type, actionName);
		}

		protected override SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
		{
			if (type == null)
				type = typeof(T);

			return base.GetSqlQueryInfo(db, type, actionName);
		}

		#endregion

		#region CRUDL (SP)

			#region SelectByKey

		public T SelectByKey(DbManager db, params object[] key)
		{
			return base.SelectByKey<T>(db, key);
		}

		public T SelectByKey(params object[] key)
		{
			return base.SelectByKey<T>(key);
		}

			#endregion

			#region SelectAll

		public List<T> SelectAll(DbManager db)
		{
			return base.SelectAll<T>(db);
		}

		public List<T> SelectAll()
		{
			return base.SelectAll<T>();
		}

			#endregion

			#region Insert

		public void Insert(DbManager db, T obj)
		{
			base.Insert(db, obj);
		}

		public void Insert(T obj)
		{
			base.Insert(obj);
		}

			#endregion

			#region Update

		public int Update(DbManager db, T obj)
		{
			return base.Update(db, obj);
		}

		public int Update(T obj)
		{
			return base.Update(obj);
		}

			#endregion

			#region DeleteByKey

		public int DeleteByKey(DbManager db, params object[] key)
		{
			return base.DeleteByKey<T>(db, key);
		}

		public int DeleteByKey(params object[] key)
		{
			return base.DeleteByKey<T>(key);
		}

			#endregion

			#region Delete

		public int Delete(DbManager db, T obj)
		{
			return base.Delete(db, obj);
		}

		public int Delete(T obj)
		{
			return base.Delete(obj);
		}

			#endregion

		#endregion

		#region CRUDL (SQL)

			#region SelectByKey

		public T SelectByKeySql(DbManager db, params object[] keys)
		{
			return base.SelectByKeySql<T>(db, keys);
		}

		public T SelectByKeySql(params object[] keys)
		{
			return base.SelectByKeySql<T>(keys);
		}

			#endregion

			#region SelectAll

		public List<T> SelectAllSql(DbManager db)
		{
			return base.SelectAllSql<T>(db);
		}

		public L SelectAllSql<L>(DbManager db, L list)
			where L : IList
		{
			return base.SelectAllSql<L,T>(db, list);
		}

		public List<T> SelectAllSql()
		{
			return base.SelectAllSql<T>();
		}

		public L SelectAllSql<L>(L list)
			where L : IList
		{
			return base.SelectAllSql<L,T>(list);
		}

			#endregion

			#region Insert

		public void InsertSql(DbManager db, T obj)
		{
			base.InsertSql(db, obj);
		}

		public void InsertSql(T obj)
		{
			base.InsertSql(obj);
		}

			#endregion

			#region Update

		public int UpdateSql(DbManager db, T obj)
		{
			return base.UpdateSql(db, obj);
		}

		public int UpdateSql(T obj)
		{
			return base.UpdateSql(obj);
		}

			#endregion

			#region DeleteByKey

		public int DeleteByKeySql(DbManager db, params object[] key)
		{
			return base.DeleteByKeySql<T>(db, key);
		}

		public int DeleteByKeySql(params object[] key)
		{
			return base.DeleteByKeySql<T>(key);
		}

			#endregion

			#region Delete

		public int DeleteSql(DbManager db, T obj)
		{
			return base.DeleteSql(db, obj);
		}

		public int DeleteSql(T obj)
		{
			return base.DeleteSql(obj);
		}

			#endregion

		#endregion
	}
}
