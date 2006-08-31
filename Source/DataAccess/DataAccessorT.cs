using System;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.Aspects;

namespace BLToolkit.DataAccess
{
	public /*abstract*/ class DataAccessor<T> : DataAccessor
	{
		#region Constructors

		public /*protected*/ DataAccessor()
		{
		}

		public /*protected*/ DataAccessor(DbManager dbManager)
			: base(dbManager)
		{
		}

		#endregion

		#region Obsolete

		#region CRUDL (SP)

			#region SelectByKey

		[NoInterception, Obsolete("Use SprocQuery<T>.SelectByKey instead.")]
		public virtual T SelectByKey(DbManager db, params object[] key)
		{
			return new SprocQuery<T>(DbManager).SelectByKey(db, key);
		}

		[NoInterception, Obsolete("Use SprocQuery<T>.SelectByKey instead.")]
		public virtual T SelectByKey(params object[] key)
		{
			return new SprocQuery<T>(DbManager).SelectByKey(key);
		}

			#endregion

			#region SelectAll

		[NoInterception, Obsolete("Use SprocQuery<T>.SelectAll instead.")]
		public virtual List<T> SelectAll(DbManager db)
		{
			return new SprocQuery<T>(DbManager).SelectAll(db);
		}

		[NoInterception, Obsolete("Use SprocQuery<T>.SelectAll instead.")]
		public virtual List<T> SelectAll()
		{
			return new SprocQuery<T>(DbManager).SelectAll();
		}

			#endregion

			#region Insert

		[NoInterception, Obsolete("Use SprocQuery.Insert instead.")]
		public virtual void Insert(DbManager db, T obj)
		{
			new SprocQuery(DbManager).Insert(db, obj);
		}

		[NoInterception, Obsolete("Use SprocQuery.Insert instead.")]
		public virtual void Insert(T obj)
		{
			new SprocQuery(DbManager).Insert(obj);
		}

			#endregion

			#region Update

		[NoInterception, Obsolete("Use SprocQuery.Update instead.")]
		public virtual int Update(DbManager db, T obj)
		{
			return new SprocQuery<T>(DbManager).Update(db, obj);
		}

		[NoInterception, Obsolete("Use SprocQuery.Update instead.")]
		public virtual int Update(T obj)
		{
			return new SprocQuery<T>(DbManager).Update(obj);
		}

			#endregion

			#region DeleteByKey

		[NoInterception, Obsolete("Use SprocQuery<T>.DeleteByKey instead.")]
		public virtual int DeleteByKey(DbManager db, params object[] key)
		{
			return new SprocQuery<T>(DbManager).DeleteByKey(db, key);
		}

		[NoInterception, Obsolete("Use SprocQuery<T>.DeleteByKey instead.")]
		public virtual int DeleteByKey(params object[] key)
		{
			return new SprocQuery<T>(DbManager).DeleteByKey(key);
		}

			#endregion

			#region Delete

		[NoInterception, Obsolete("Use SprocQuery.Delete instead.")]
		public virtual int Delete(DbManager db, T obj)
		{
			return new SprocQuery(DbManager).Delete(db, obj);
		}

		[NoInterception, Obsolete("Use SprocQuery.Delete instead.")]
		public virtual int Delete(T obj)
		{
			return new SprocQuery(DbManager).Delete(obj);
		}

			#endregion

		#endregion

		#region CRUDL (SQL)

			#region SelectByKey

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectByKey instead.")]
		public virtual T SelectByKeySql(DbManager db, params object[] keys)
		{
			return new SqlQuery<T>(DbManager).SelectByKey(db, keys);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectByKey instead.")]
		public virtual T SelectByKeySql(params object[] keys)
		{
			return new SqlQuery<T>(DbManager).SelectByKey(keys);
		}

			#endregion

			#region SelectAll

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectAll instead.")]
		public virtual List<T> SelectAllSql(DbManager db)
		{
			return new SqlQuery<T>(DbManager).SelectAll(db);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectAll instead.")]
		public virtual L SelectAllSql<L>(DbManager db, L list)
			where L : IList<T>
		{
			return new SqlQuery<T>(DbManager).SelectAll(db, list);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectAll instead.")]
		public virtual List<T> SelectAllSql()
		{
			return new SqlQuery<T>(DbManager).SelectAll();
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.SelectAll instead.")]
		public virtual L SelectAllSql<L>(L list)
			where L : IList<T>
		{
			return new SqlQuery<T>(DbManager).SelectAll(list);
		}

			#endregion

			#region Insert

		[NoInterception, Obsolete("Use SqlQuery<T>.Insert instead.")]
		public virtual void InsertSql(DbManager db, T obj)
		{
			new SqlQuery<T>(DbManager).Insert(db, obj);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.Insert instead.")]
		public virtual void InsertSql(T obj)
		{
			new SqlQuery<T>(DbManager).Insert(obj);
		}

			#endregion

			#region Update

		[NoInterception, Obsolete("Use SqlQuery<T>.Update instead.")]
		public virtual int UpdateSql(DbManager db, T obj)
		{
			return base.UpdateSql(db, obj);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.Update instead.")]
		public virtual int UpdateSql(T obj)
		{
			return base.UpdateSql(obj);
		}

			#endregion

			#region DeleteByKey

		[NoInterception, Obsolete("Use SqlQuery<T>.DeleteByKey instead.")]
		public virtual int DeleteByKeySql(DbManager db, params object[] key)
		{
			return base.DeleteByKeySql<T>(db, key);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.DeleteByKey instead.")]
		public virtual int DeleteByKeySql(params object[] key)
		{
			return base.DeleteByKeySql<T>(key);
		}

			#endregion

			#region Delete

		[NoInterception, Obsolete("Use SqlQuery<T>.Delete instead.")]
		public virtual int DeleteSql(DbManager db, T obj)
		{
			return base.DeleteSql(db, obj);
		}

		[NoInterception, Obsolete("Use SqlQuery<T>.Delete instead.")]
		public virtual int DeleteSql(T obj)
		{
			return base.DeleteSql(obj);
		}

			#endregion

		#endregion

		#endregion
	}
}
