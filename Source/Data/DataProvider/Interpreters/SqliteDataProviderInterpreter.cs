using System.Collections.Generic;
using System.Data;
using BLToolkit.Mapping;

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

        public override void SetCollectionIds<T>(DbManager db, IEnumerable<MemberMapper> members, IEnumerable<T> collection)
        {
            throw new System.NotImplementedException();
        }
    }
}