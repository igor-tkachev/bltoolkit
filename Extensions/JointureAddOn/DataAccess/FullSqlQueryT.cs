using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
    public class FullSqlQueryT<T> : FullSqlQuery, ISqlQueryT<T>
    {
        #region Constructors

        public FullSqlQueryT(DbManager dbManager, bool ignoreLazyLoad = false)
            : base(dbManager, ignoreLazyLoad)
        {
        }

        #endregion

        #region Implementation of ISqlQueryT<T>

        public T SelectByKey(DbManager db, params object[] keys)
        {
            return (T) base.SelectByKey(db, typeof(T), keys);
        }

        public T SelectByKey(params object[] keys)
        {
            return (T)base.SelectByKey(typeof(T), keys);
        }

        public List<T> SelectAll(DbManager db)
        {
            return base.SelectAll(db, typeof(T)).Cast<T>().ToList();
        }

        public TL SelectAll<TL>(DbManager db, TL list) where TL : IList<T>
        {
            return (TL)base.SelectAll(db, (IList)list, typeof(T));
        }

        public TL SelectAll<TL>(DbManager db) where TL : IList<T>, new()
        {
            return SelectAll(db, new TL());
        }

        public List<T> SelectAll()
        {
            return base.SelectAll(typeof(T)).Cast<T>().ToList();
        }

        public TL SelectAll<TL>(TL list) where TL : IList<T>
        {
            return SelectAll(DbManager, list);
        }

        public TL SelectAll<TL>() where TL : IList<T>, new()
        {
            return SelectAll<TL>(DbManager);
        }

        public int Insert(DbManager db, T obj)
        {
            return base.Insert(db, obj);
        }

        public int Insert(T obj)
        {
            return base.Insert(obj);
        }

        public int Insert(DbManager db, int maxBatchSize, IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public int Insert(int maxBatchSize, IEnumerable<T> list)
        {
            return Insert(DbManager, maxBatchSize, list);
        }

        public int Insert(DbManager db, IEnumerable<T> list)
        {
            return Insert(db, int.MaxValue, list);
        }

        public int Insert(IEnumerable<T> list)
        {
            return Insert(DbManager, list);
        }

        public int Update(DbManager db, T obj)
        {
            return base.Update(db, obj);
        }

        public int Update(T obj)
        {
            return base.Update(obj);
        }

        public int Update(DbManager db, int maxBatchSize, IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public int Update(int maxBatchSize, IEnumerable<T> list)
        {
            return Update(DbManager, maxBatchSize, list);
        }

        public int Update(DbManager db, IEnumerable<T> list)
        {
            return Update(db, int.MaxValue, list);
        }

        public int Update(IEnumerable<T> list)
        {
            return Update(DbManager, list);
        }

        public int DeleteByKey(DbManager db, params object[] key)
        {
            return base.DeleteByKey(db, typeof(T), key);
        }

        public int DeleteByKey(params object[] key)
        {
            return base.DeleteByKey(typeof(T), key);
        }

        public int Delete(DbManager db, T obj)
        {
            return base.Delete(db, obj);
        }

        public int Delete(T obj)
        {
            return base.Delete(obj);
        }

        public int Delete(DbManager db, int maxBatchSize, IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public int Delete(int maxBatchSize, IEnumerable<T> list)
        {
            return Delete(DbManager, maxBatchSize, list);
        }

        public int Delete(DbManager db, IEnumerable<T> list)
        {
            return Delete(int.MaxValue, list);
        }

        public int Delete(IEnumerable<T> list)
        {
            return Delete(DbManager, list);
        }

        #endregion
    }
}