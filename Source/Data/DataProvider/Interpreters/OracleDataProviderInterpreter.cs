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
        public override DbType GetParameterDbType(DbType dbType)
        {
            /*
             * In the OracleParameter class, DbType, OracleDbType, and Value properties are linked. Specifying the value of any of these properties infers the value of one or more of the other properties.
             */

            /*See how the DbType values map to OracleDbType
             * In the OracleParameter class, specifying the value of DbType infers the value of OracleDbType
AnsiString => Varchar2
Binary => Raw
Byte => Byte
Boolean => Value does not fall within the expected range.
Currency => Value does not fall within the expected range.
Date => Date
DateTime => TimeStamp
Decimal => Decimal
Double => Double
Guid => Value does not fall within the expected range.
Int16 => Int16
Int32 => Int32
Int64 => Int64
Object => Object
SByte => Value does not fall within the expected range.
Single => Single
String => Varchar2
Time => TimeStamp
UInt16 => Value does not fall within the expected range.
UInt32 => Value does not fall within the expected range.
UInt64 => Value does not fall within the expected range.
VarNumeric => Value does not fall within the expected range.
AnsiStringFixedLength => Char
StringFixedLength => Char
Xml => Specified argument was out of the range of valid values.
DateTime2 => Specified argument was out of the range of valid values.
DateTimeOffset => Specified argument was out of the range of valid values.
             * */

            /*See how the OracleDbType values map to DbType
             * In the OracleParameter class, specifying the value of OracleDbType infers the value of DbType 
BFile => Object
Blob => Object
Byte => Byte
Char => StringFixedLength
Clob => Object
Date => Date
Decimal => Decimal
Double => Double
Long => String
LongRaw => Binary
Int16 => Int16
Int32 => Int32
Int64 => Int64
IntervalDS => Object
IntervalYM => Int64
NClob => Object
NChar => StringFixedLength
NVarchar2 => String
Raw => Binary
RefCursor => Object
Single => Single
TimeStamp => DateTime
TimeStampLTZ => DateTime
TimeStampTZ => DateTime
Varchar2 => String
XmlType => String
Array => Object
Object => Object
Ref => Object
BinaryDouble => Double
BinaryFloat => Single
             */
            switch (dbType)
            {
                case DbType.UInt16:
                    return DbType.Int16;
                case DbType.DateTime2:
                    return DbType.DateTime;
                
                // TODO Add the other cases ...

                default:
                    return base.GetParameterDbType(dbType);
            }
        }

        public override void SetParameterValue(IDbDataParameter parameter, object value)
        {
            if (null != value)
            {
                if (value is Guid)
                {
                    // Fix Oracle.Net bug #6: guid type is not handled
                    //
                    value = ((Guid) value).ToByteArray();
                }
                else if (parameter.DbType == DbType.Date && value is DateTime)
                {
                    var dt = (DateTime)value;
                    value = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, 0, dt.Kind);
                }
                else if (parameter.DbType == DbType.DateTime && value is DateTime)
                {
                    var dt = (DateTime)value;
                    value = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);
                }
                    //// This case is treated in OracleParameterWrap set method of Value property
                    //else if (value is Array && !(value is byte[] || value is char[]))
                    //{
                    //    _oracleParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                    //}
                else if (value is IConvertible)
                {
                    var convertible = (IConvertible) value;
                    var typeCode = convertible.GetTypeCode();

                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            // Fix Oracle.Net bug #7: bool type is handled wrong
                            //
                            value = convertible.ToByte(null);
                            break;

                        case TypeCode.SByte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            // Fix Oracle.Net bug #8: some integer types are handled wrong
                            //
                            value = convertible.ToDecimal(null);
                            break;

                            // Fix Oracle.Net bug #10: zero-length string can not be converted to
                            // ORAXML type, but null value can be.
                            //
                        case TypeCode.String:
                            if (((string) value).Length == 0)
                                value = null;
                            break;

                        default:
                            // Fix Oracle.Net bug #5: Enum type is not handled
                            //
                            if (value is Enum)
                            {
                                // Convert a Enum value to it's underlying type.
                                //
                                value = System.Convert.ChangeType(value, typeCode);
                            }
                            break;
                    }
                }
            }

            if (value is TimeSpan)
            {
                parameter.Value = ((TimeSpan) value).ToString();
            }
            else
                base.SetParameterValue(parameter, value);
        }

        public override List<string> GetInsertBatchSqlList<T>(
            string                  insertText, 
            IEnumerable<T>          collection, 
            MemberMapper[]          members, 
            int                     maxBatchSize, 
            bool                    withIdentity, 
            DbManager               db,
            List<IDbDataParameter>  parameters)
        {
            //const InsertBatchMethod method = InsertBatchMethod.InsertAllWithPks;
            const InsertBatchMethod method = InsertBatchMethod.UnionAll;
            switch (method)
            {
                case InsertBatchMethod.InsertAllWithPks:
                    SetCollectionIds(db, members, collection);
                    return GetInsertBatchSqlListWithInsertAllWithPks(insertText, collection, members, maxBatchSize, db, parameters);
                    break;
                case InsertBatchMethod.InsertAll:
                    // Note : Can this work?
                    return GetInsertBatchSqlListWithInsertAll(insertText, collection, members, maxBatchSize, db.MappingSchema);
                    break;
                case InsertBatchMethod.UnionAll:
                default:
                    return GetInsertBatchSqlListUnionAll(insertText, collection, members, maxBatchSize, withIdentity, db.MappingSchema);
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
            //var sql2 = new StringBuilder("SELECT level," + sequenceName + " Id from DUAL connect by level <= " + count);

            //db.SetCommand(sql2.ToString());

            //return db.ExecuteScalarList<long>();

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
            var valuesWithoutSequence = withIdentity ? valuesQuery.Substring(valuesQuery.IndexOf(",") + 1) : valuesQuery;

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

                    //if (value is string && ((string)value).Length >= 2000)
                    //{
                    //    var par = db.Parameter("p" + ++pn, value);
                    //    parameters.Add(par);
                    //    sb.Append(":" + par.ParameterName);
                    //}

                    sp.BuildValue(sbItem, value);

                    values.Add(sbItem + " " + member.Name);
                }

                sb.AppendFormat(valuesWithoutSequence, values.ToArray());
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
            DbManager db,
            List<IDbDataParameter> parameters)
        {
            MappingSchema mappingSchema = db.MappingSchema;

            var queries = new List<string>();

            var sb = new StringBuilder();
            var sp = new OracleSqlProvider();
            var pn = 0;
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
                    else if (value is string && ((string)value).Length >= 2000)
                    {
                        var par = db.Parameter("p" + ++pn, value);
                        parameters.Add(par);
                        sb.Append(":" + par.ParameterName);
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

                    if (DbManager.TraceSwitch.TraceInfo)
                        DbManager.WriteTraceLine("\n" + sql.Replace("\r", ""), DbManager.TraceSwitch.DisplayName);

                    queries.Add(sql);

                    parameters.Clear();
                    pn = 0;
                    n = 0;
                    sb.Length = 0;
                }
            }

            if (n > 0)
            {
                sb.AppendLine("SELECT * FROM dual");

                var sql = sb.ToString();

                if (DbManager.TraceSwitch.TraceInfo)
                    DbManager.WriteTraceLine("\n" + sql.Replace("\r", ""), DbManager.TraceSwitch.DisplayName);

                queries.Add(sql);
            }

            return queries;
        }

        /// <summary>
        /// Note : Can this work?
        /// </summary>
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