using System.Data;

namespace BLToolkit.Data.DataProvider.Interpreters
{
    public class SqliteDataProviderInterpreter : DataProviderInterpreterBase
    {
        public override void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (parameter.DbType == DbType.DateTime2)
                parameter.DbType = DbType.DateTime;

            base.SetParameterValue(parameter, value);
        }
    }
}