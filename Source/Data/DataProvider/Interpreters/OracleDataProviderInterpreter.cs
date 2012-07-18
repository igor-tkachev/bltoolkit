using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;

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

        public override List<string> GetInsertBatchSqlList<T>(
            string         insertText,
			IEnumerable<T> collection, 
            MemberMapper[] members,
			int            maxBatchSize)
        {
            var sb = new StringBuilder();
            var sp = new OracleSqlProvider();
            var n = 0;
            var sqlList = new List<string>();

            foreach (var item in collection)
            {
                if (sb.Length == 0)
                    sb.AppendLine("INSERT ALL");

                string strItem = "\t" + insertText
                                            .Replace("INSERT INTO", "INTO")
                                            .Replace("\r", "")
                                            .Replace("\n", "")
                                            .Replace("\t", " ")
                                            .Replace("( ", "(");

                var values = new List<object>();
                foreach (var member in members)
                {
                    var sbItem = new StringBuilder();

                    var value = member.GetValue(item);

                    if (value is DateTime?)
                        value = ((DateTime?)value).Value;

                    sp.BuildValue(sbItem, value);

                    values.Add(sbItem.ToString());
                }

                sb.AppendFormat(strItem, values.ToArray());
                sb.AppendLine();

                n++;

                if (n >= maxBatchSize)
                {
                    sb.AppendLine("SELECT * FROM dual");

                    var sql = sb.ToString();
                    sqlList.Add(sql);

                    n = 0;
                    sb.Length = 0;
                }
            }

            if (n > 0)
            {
                sb.AppendLine("SELECT * FROM dual");

                var sql = sb.ToString();
                sqlList.Add(sql);
            }

            return sqlList;
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