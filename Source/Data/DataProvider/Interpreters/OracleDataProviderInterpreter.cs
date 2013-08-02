#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

#endregion

namespace BLToolkit.Data.DataProvider.Interpreters
{
    public class OracleDataProviderInterpreter : DataProviderInterpreterBase
    {
        public override void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (value is TimeSpan)
            {
                parameter.Value = ((TimeSpan) value).ToString();
            }
            else
                base.SetParameterValue(parameter, value);
        }

        public override List<string> GetInsertBatchSqlList<T>(string insertText, IEnumerable<T> collection, MemberMapper[] members, int maxBatchSize, bool withIdentity, DbManager db)
        {
            const InsertBatchMethod method = InsertBatchMethod.UnionAll;
            switch (method)
            {
                case InsertBatchMethod.InsertAllWithPks:
                    SetCollectionIds(db, members, collection);
                    return GetInsertBatchSqlListWithInsertAllWithPks(insertText, collection, members, maxBatchSize, db.MappingSchema);
                    break;
                case InsertBatchMethod.InsertAll:
                    return GetInsertBatchSqlListWithInsertAll(insertText, collection, members, maxBatchSize, db.MappingSchema);
                    break;
                case InsertBatchMethod.UnionAll:

                    return GetInsertBatchSqlListUnionAll(insertText, collection, members, maxBatchSize, withIdentity, db.MappingSchema);
                    break;
                default:

                    return GetInsertBatchSqlListUnionAll(insertText, collection, members, maxBatchSize, withIdentity, db.MappingSchema);
                    break;
            }
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

        public override void SetCollectionIds<T>(
            DbManager db,
            IEnumerable<MemberMapper> members,
            IEnumerable<T> collection)
        {
            MemberMapper primaryKeyMapper = null;
            SequenceKeyGenerator keyGenerator = null;
            foreach (var mapper in members)
            {
                keyGenerator = mapper.MapMemberInfo.KeyGenerator as SequenceKeyGenerator;
                if (keyGenerator != null)
                {
                    primaryKeyMapper = mapper;
                    break;
                }
            }

            if (primaryKeyMapper == null)
                throw new Exception("The class mapping should contain a pk column!");

            //int j = 0;
            //foreach (var element in collection)
            //{
            //    primaryKeyMapper.SetInt64(element, j);
            //    j++;
            //}

            var rowCount = collection.Count();
            var sequenceIds = ReserveSequenceValues(db, rowCount, NextSequenceQuery(keyGenerator.Sequence));

            int i = 0;
            foreach (var element in collection)
            {
                primaryKeyMapper.SetInt64(element, sequenceIds[i]);
                i++;
            }
        }

        #region Private methods

        private List<Int64> ReserveSequenceValues(DbManager db, int count, string sequenceName)
        {
            var results = new List<long>();

            foreach (var page in Enumerable.Range(1, count).ToPages(1000))
            {
                var sql = new StringBuilder("SELECT " + sequenceName + " FROM (");
                for (int i = 1; i < page.Count(); i++)
                    sql.Append("SELECT 0 FROM DUAL UNION ALL ");

                sql.Append("SELECT 0 FROM DUAL)");

                db.SetCommand(sql.ToString());

                var result = db.ExecuteScalarList<long>();
                results.AddRange(result);
            }

            return results;
        }

        private List<string> GetInsertBatchSqlListUnionAll<T>(
            string insertText,
            IEnumerable<T> collection,
            MemberMapper[] members,
            int maxBatchSize,
            bool withIdentity,
            MappingSchema mappingSchema)
        {
            var sp = new OracleSqlProvider();
            var n = 0;
            var sqlList = new List<string>();

            var indexValuesWord = insertText.IndexOf(" VALUES (", StringComparison.Ordinal);
            var initQuery = insertText.Substring(0, indexValuesWord) + Environment.NewLine;
            var valuesQuery = insertText.Substring(indexValuesWord + 9);
            var indexEndValuesQuery = valuesQuery.IndexOf(")");
            valuesQuery = valuesQuery.Substring(0, indexEndValuesQuery)
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", "");

            // 1 = Number of primary keys generated by sequence
            var valuesWihtoutSequence = withIdentity ? valuesQuery.Substring(valuesQuery.IndexOf(",") + 1) : valuesQuery;

            var sb = new StringBuilder(initQuery);
            sb.Append(" SELECT ");
            sb.AppendFormat(valuesQuery, members.Select(m => m.Name).ToArray());
            sb.AppendLine(" FROM (");

            initQuery = sb.ToString();

            sb = new StringBuilder(initQuery);
            bool isFirstValues = true;

            foreach (var item in collection)
            {
                if (!isFirstValues)
                    sb.AppendLine(" UNION ALL ");

                sb.Append("SELECT ");

                var values = new List<object>();
                foreach (var member in members)
                {
                    var sbItem = new StringBuilder();

                    var value = member.GetValue(item);

                    if (value != null && value.GetType().IsEnum)
                        value = mappingSchema.MapEnumToValue(value, true);

                    if (value is DateTime?)
                        value = ((DateTime?) value).Value;

                    sp.BuildValue(sbItem, value);

                    values.Add(sbItem + " " + member.Name);
                }

                sb.AppendFormat(valuesWihtoutSequence, values.ToArray());
                sb.Append(" FROM DUAL");

                isFirstValues = false;

                n++;
                if (n > maxBatchSize)
                {
                    sb.Append(")");
                    sqlList.Add(sb.ToString());
                    sb = new StringBuilder(initQuery);
                    isFirstValues = true;
                    n = 0;
                }
            }

            if (n > 0)
            {
                sb.Append(")");
                sqlList.Add(sb.ToString());
            }
            return sqlList;
        }

        private List<string> GetInsertBatchSqlListWithInsertAllWithPks<T>(
            string insertText,
            IEnumerable<T> collection,
            MemberMapper[] members,
            int maxBatchSize,
            MappingSchema mappingSchema)
        {
            var queries = new List<string>();

            var sb = new StringBuilder();
            var sp = new OracleSqlProvider();
            var n = 0;
            var cnt = 0;
            var str = "\t" + insertText
                .Substring(0, insertText.IndexOf(") VALUES ("))
                .Substring(7)
                .Replace("\r", "")
                .Replace("\n", "")
                .Replace("\t", " ")
                .Replace("( ", "(")
                //.Replace("  ", " ")
                      + ") VALUES (";

            foreach (var item in collection)
            {
                if (sb.Length == 0)
                    sb.AppendLine("INSERT ALL");

                sb.Append(str);

                foreach (var member in members)
                {
                    var value = member.GetValue(item);

                    if (value != null && value.GetType().IsEnum)
                        value = mappingSchema.MapEnumToValue(value, true);

                    if (value is Nullable<DateTime>)
                        value = ((DateTime?) value).Value;

                    if (value is DateTime)
                    {
                        var dt = (DateTime) value;
                        sb.Append(string.Format("to_timestamp('{0:dd.MM.yyyy HH:mm:ss.ffffff}', 'DD.MM.YYYY HH24:MI:SS.FF6')", dt));
                    }
                    else
                        sp.BuildValue(sb, value);

                    sb.Append(", ");
                }

                sb.Length -= 2;
                sb.AppendLine(")");

                n++;

                if (n >= maxBatchSize)
                {
                    sb.AppendLine("SELECT * FROM dual");

                    var sql = sb.ToString();

                    queries.Add(sql);

                    n = 0;
                }
            }

            if (n > 0)
            {
                sb.AppendLine("SELECT * FROM dual");

                var sql = sb.ToString();

                queries.Add(sql);
            }

            return queries;
        }

        private List<string> GetInsertBatchSqlListWithInsertAll<T>(
            string insertText,
            IEnumerable<T> collection,
            MemberMapper[] members,
            int maxBatchSize,
            MappingSchema mappingSchema)
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

                    var keyGenerator = member.MapMemberInfo.KeyGenerator as SequenceKeyGenerator;
                    if (keyGenerator != null)
                    {
                        values.Add(NextSequenceQuery(keyGenerator.Sequence));
                    }
                    else
                    {
                        var value = member.GetValue(item);

                        if (value != null && value.GetType().IsEnum)
                            value = mappingSchema.MapEnumToValue(value, true);

                        if (value is DateTime?)
                            value = ((DateTime?) value).Value;

                        if (value is DateTime)
                        {
                            var dt = (DateTime) value;
                            //sb.Append(string.Format("to_timestamp('{0:dd.MM.yyyy HH:mm:ss.ffffff}', 'DD.MM.YYYY HH24:MI:SS.FF6')", dt));
                            sp.BuildValue(sbItem, string.Format("to_timestamp('{0:dd.MM.yyyy HH:mm:ss.ffffff}', 'DD.MM.YYYY HH24:MI:SS.FF6')", dt));
                        }
                        else
                            sp.BuildValue(sbItem, value);

                        values.Add(sbItem.ToString());
                    }
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

        #endregion

        private enum InsertBatchMethod
        {
            UnionAll,
            InsertAll,
            InsertAllWithPks,
        }
    }
}