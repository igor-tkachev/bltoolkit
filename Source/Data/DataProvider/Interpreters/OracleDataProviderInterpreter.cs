using System;
using System.Data;

namespace BLToolkit.Data.DataProvider.Interpreters
{
    public class OracleDataProviderInterpreter : DataProviderInterpreterBase
    {
        public override void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (value is TimeSpan)
            {
                parameter.Value = ((TimeSpan)value).ToString();
            }
            else
                base.SetParameterValue(parameter, value);
        }

        public override string GetSequenceQuery(string sequenceName)
        {
            return string.Format("SELECT {0}.NEXTVAL FROM DUAL", sequenceName);
        }

        public override string NextSequenceQuery(string sequenceName)
        {
            return string.Format("{0}.NEXTVAL", sequenceName);
        }

        public override string GetReturningInto(string columnName)
        {
            return string.Format("returning {0} into :IDENTITY_PARAMETER", columnName);
        }
    }
}