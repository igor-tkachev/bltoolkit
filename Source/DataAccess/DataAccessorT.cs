using System;
using System.Collections;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Aspects;

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

		[NoInterception]
		protected override string GetSpName(Type type, string actionName)
		{
			if (type == null)
				type = typeof(T);

			return base.GetSpName(type, actionName);
		}

		[NoInterception]
		public override SqlQueryInfo GetSqlQueryInfo(DbManager db, Type type, string actionName)
		{
			if (type == null)
				type = typeof(T);

			return base.GetSqlQueryInfo(db, type, actionName);
		}

		public SqlQueryInfo GetSqlQueryInfo(DbManager db, string actionName)
		{
			return base.GetSqlQueryInfo(db, typeof(T), actionName);
		}

		#endregion

		#region CRUDL (SP)

			#region SelectByKey

		[NoInterception]
		public virtual T SelectByKey(DbManager db, params object[] key)
		{
			return base.SelectByKey<T>(db, key);
		}

		[NoInterception]
		public virtual T SelectByKey(params object[] key)
		{
			return base.SelectByKey<T>(key);
		}

			#endregion

			#region SelectAll

		[NoInterception]
		public virtual List<T> SelectAll(DbManager db)
		{
			return base.SelectAll<T>(db);
		}

		[NoInterception]
		public virtual List<T> SelectAll()
		{
			return base.SelectAll<T>();
		}

			#endregion

			#region Insert

		[NoInterception]
		public virtual void Insert(DbManager db, T obj)
		{
			base.Insert(db, obj);
		}

		[NoInterception]
		public virtual void Insert(T obj)
		{
			base.Insert(obj);
		}

			#endregion

			#region Update

		[NoInterception]
		public virtual int Update(DbManager db, T obj)
		{
			return base.Update(db, obj);
		}

		[NoInterception]
		public virtual int Update(T obj)
		{
			return base.Update(obj);
		}

			#endregion

			#region DeleteByKey

		[NoInterception]
		public virtual int DeleteByKey(DbManager db, params object[] key)
		{
			return base.DeleteByKey<T>(db, key);
		}

		[NoInterception]
		public virtual int DeleteByKey(params object[] key)
		{
			return base.DeleteByKey<T>(key);
		}

			#endregion

			#region Delete

		[NoInterception]
		public virtual int Delete(DbManager db, T obj)
		{
			return base.Delete(db, obj);
		}

		[NoInterception]
		public virtual int Delete(T obj)
		{
			return base.Delete(obj);
		}

			#endregion

		#endregion

		#region CRUDL (SQL)

			#region SelectByKey

		[NoInterception]
		public virtual T SelectByKeySql(DbManager db, params object[] keys)
		{
			return base.SelectByKeySql<T>(db, keys);
		}

		[NoInterception]
		public virtual T SelectByKeySql(params object[] keys)
		{
			return base.SelectByKeySql<T>(keys);
		}

			#endregion

			#region SelectAll

		[NoInterception]
		public virtual List<T> SelectAllSql(DbManager db)
		{
			return base.SelectAllSql<T>(db);
		}

		[NoInterception]
		public virtual L SelectAllSql<L>(DbManager db, L list)
			where L : IList
		{
			return base.SelectAllSql<L,T>(db, list);
		}

		[NoInterception]
		public virtual List<T> SelectAllSql()
		{
			return base.SelectAllSql<T>();
		}

		[NoInterception]
		public virtual L SelectAllSql<L>(L list)
			where L : IList
		{
			return base.SelectAllSql<L,T>(list);
		}

			#endregion

			#region Insert

		[NoInterception]
		public virtual void InsertSql(DbManager db, T obj)
		{
			base.InsertSql(db, obj);
		}

		[NoInterception]
		public virtual void InsertSql(T obj)
		{
			base.InsertSql(obj);
		}

			#endregion

			#region Update

		[NoInterception]
		public virtual int UpdateSql(DbManager db, T obj)
		{
			return base.UpdateSql(db, obj);
		}

		[NoInterception]
		public virtual int UpdateSql(T obj)
		{
			return base.UpdateSql(obj);
		}

			#endregion

			#region DeleteByKey

		[NoInterception]
		public virtual int DeleteByKeySql(DbManager db, params object[] key)
		{
			return base.DeleteByKeySql<T>(db, key);
		}

		[NoInterception]
		public virtual int DeleteByKeySql(params object[] key)
		{
			return base.DeleteByKeySql<T>(key);
		}

			#endregion

			#region Delete

		[NoInterception]
		public virtual int DeleteSql(DbManager db, T obj)
		{
			return base.DeleteSql(db, obj);
		}

		[NoInterception]
		public virtual int DeleteSql(T obj)
		{
			return base.DeleteSql(obj);
		}

			#endregion

		#endregion
	}
}
