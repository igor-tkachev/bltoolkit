using System.Collections.Generic;
using BLToolkit.Data;

namespace BLToolkit.DataAccess
{
    public interface ISqlQueryT<T>
    {
        T SelectByKey(DbManager db, params object[] keys);
        T SelectByKey(params object[] keys);
        List<T> SelectAll(DbManager db);

        TL SelectAll<TL>(DbManager db, TL list)
            where TL : IList<T>;

        TL SelectAll<TL>(DbManager db)
            where TL : IList<T>, new();

        List<T> SelectAll();

        TL SelectAll<TL>(TL list)
            where TL : IList<T>;

        TL SelectAll<TL>()
            where TL : IList<T>, new();

        int Insert(DbManager db, T obj);
        int Insert(T obj);
        int Insert(DbManager db, int maxBatchSize, IEnumerable<T> list);
        int Insert(int maxBatchSize, IEnumerable<T> list);
        int Insert(DbManager db, IEnumerable<T> list);
        int Insert(IEnumerable<T> list);
        int Update(DbManager db, T obj);
        int Update(T obj);
        int Update(DbManager db, int maxBatchSize, IEnumerable<T> list);
        int Update(int maxBatchSize, IEnumerable<T> list);
        int Update(DbManager db, IEnumerable<T> list);
        int Update(IEnumerable<T> list);
        int DeleteByKey(DbManager db, params object[] key);
        int DeleteByKey(params object[] key);
        int Delete(DbManager db, T obj);
        int Delete(T obj);
        int Delete(DbManager db, int maxBatchSize, IEnumerable<T> list);
        int Delete(int maxBatchSize, IEnumerable<T> list);
        int Delete(DbManager db, IEnumerable<T> list);
        int Delete(IEnumerable<T> list);
    }
}