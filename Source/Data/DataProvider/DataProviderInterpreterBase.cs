#region

using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using BLToolkit.Mapping;

#endregion

namespace BLToolkit.Data.DataProvider
{
    /// <summary>
    ///     BasicSqlProvider equivalent for the non-linq DAL
    /// </summary>
    public abstract class DataProviderInterpreterBase
    {
        public virtual void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (value is Binary)
            {
                var arr = ((Binary) value).ToArray();

                parameter.Value = arr;
                parameter.DbType = DbType.Binary;
                parameter.Size = arr.Length;
            }
            else
                parameter.Value = value;
        }

        public virtual List<string> GetInsertBatchSqlList<T>(
            string insertText,
            IEnumerable<T> collection,
            MemberMapper[] members,
            int maxBatchSize,
            bool withIdentity,
            DbManager db,
            List<IDbDataParameter> parameters)
        {
            return new List<string>();
        }

        public virtual string GetSequenceQuery(string sequenceName)
        {
            return null;
        }

        public virtual DbType GetParameterDbType(DbType dbType)
        {
            return dbType;
        }

        public virtual string NextSequenceQuery(string sequenceName)
        {
            return null;
        }

        public virtual string GetReturningInto(string columnName)
        {
            return null;
        }

        public abstract void SetCollectionIds<T>(
            DbManager db,
            IEnumerable<MemberMapper> members,
            IEnumerable<T> collection);
    }
}