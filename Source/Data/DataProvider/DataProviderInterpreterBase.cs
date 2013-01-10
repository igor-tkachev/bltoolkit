using System.Collections.Generic;
using System.Data;
using BLToolkit.Mapping;

namespace BLToolkit.Data.DataProvider
{
    /// <summary>
    /// BasicSqlProvider equivalent for the non-linq DAL
    /// </summary>
    public abstract class DataProviderInterpreterBase
    {
        public virtual void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (value is System.Data.Linq.Binary)
            {
                var arr = ((System.Data.Linq.Binary)value).ToArray();

                parameter.Value = arr;
                parameter.DbType = DbType.Binary;
                parameter.Size = arr.Length;
            }
            else
                parameter.Value = value;
        }

        public virtual List<string> GetInsertBatchSqlList<T>(
            string              insertText, 
            IEnumerable<T>      collection, 
            MemberMapper[]      members, 
            int                 maxBatchSize, 
            bool                withIdentity)
        {
            return new List<string>();
        }

        public virtual string GetSequenceQuery(string sequenceName)
        {
            return null;
        }

        public virtual string NextSequenceQuery(string sequenceName)
        {
            return null;
        }

        public virtual string GetReturningInto(string columnName)
        {
            return null;
        }
    }
}