using System.Data;

namespace BLToolkit.Data.DataProvider
{
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