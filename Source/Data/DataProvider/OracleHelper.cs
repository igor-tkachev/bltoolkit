using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BLToolkit.Data.DataProvider
{
    public static class OracleHelper
    {
        #region Text

        /// <summary>
        /// If value is null or empty, return NULL or the value converted for Oracle SQL query
        /// </summary>
        /// <param name="value">Text</param>
        /// <returns>Text converted for oracle query</returns>
        public static string SqlConvertString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("'", "''");
                value = value.Replace("&", "' || '&' || '");

                return "'" + value + "'";
            }

            return string.IsNullOrWhiteSpace(value) ? "NULL" : value;
        }

        #endregion

        #region Date & Time

        /// <summary>
        /// Convert DateTime to TO_DATE('value','YYYYMMDD')
        /// </summary>
        /// <param name="value">Date</param>
        /// <returns>Date converted for oracle query</returns>
        public static string SqlConvertDate(DateTime value)
        {
            return string.Format("TO_DATE('{0}','YYYYMMDD')", value.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Convert DateTime to TO_DATE('value','YYYYMMDDHH24MISS')
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SqlConvertDateTime(DateTime value)
        {
            return string.Format("TO_DATE('{0}','YYYYMMDDHH24MISS')", value.ToString("yyyyMMddHHmmss"));
        }

        /// <summary>
        /// Convert DateTime to TO_TIMESTAMP('value','YYYYMMDDHH24MISSFF3')
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SqlConvertTimeStamp(DateTime value)
        {
            return string.Format("TO_TIMESTAMP('{0}','YYYYMMDDHH24MISSFF3')", value.ToString("yyyyMMddHHmmssfff"));
        }

        /// <summary>
        /// Convert DateTime to TO_CHAR(TO_DATE('value','YYYYMMDD')))
        /// </summary>
        /// <param name="value">DateTime</param>
        /// <returns>DateTime converted for oracle query</returns>
        public static string SqlConvertDateToChar(DateTime value)
        {
            return string.Format("TO_CHAR(TO_DATE('{0}','YYYYMMDD'))", value.ToString("yyyyMMdd"));
        }

        #endregion

        #region Connection string

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid, int port = 1521)
        {
            return
                string.Format(
                    "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};",
                    server, port, sid, userName, password);
        }
        
                /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;Pooling=False;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionStringWithoutPooling(string userName, string password, string server, string sid, int port = 1521)
        {
            return
                string.Format(
                    "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};Pooling=False;",
                    server, port, sid, userName, password);
        }

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;Connection Timeout=timeout;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid,
                                                     TimeSpan timeOut, int port = 1521)
        {
            return
                string.Format(
                    "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};Connection Timeout={5};",
                    server, port, sid, userName, password, (int)timeOut.TotalSeconds);
        }
        
                /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;Connection Timeout=timeout;Pooling=False;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionStringWithoutPooling(string userName, string password, string server, string sid,
                                                     TimeSpan timeOut, int port = 1521)
        {
            return
                string.Format(
                    "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SID = {2})));User Id={3};Password={4};Connection Timeout={5};Pooling=False;",
                    server, port, sid, userName, password, (int)timeOut.TotalSeconds);
        }

        /// <summary>
        /// Generate the minimum connection string. The connection string looks like
        /// Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = server)(PORT = port)))(CONNECT_DATA = (SID = sid)));User Id=username;Password=password;Connection Timeout=timeout;
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="password">Password</param>
        /// <param name="server">Server name</param>
        /// <param name="sid">Database SID</param>
        /// <param name="port">Port of the server. Default value is 1521</param>
        /// <returns>Default connection string</returns>
        public static string GetFullConnectionString(string userName, string password, string server, string sid, int timeOutInSecond, int port = 1521)
        {
            return GetFullConnectionString(userName, password, server, sid, TimeSpan.FromSeconds(timeOutInSecond), port);
        }

        #endregion

        public static string Interpret(IDbCommand poCommand)
        {
            if (poCommand.Parameters.Count == 0)
                return poCommand.CommandText;

            var oRegex = new Regex(@"(?<string>'[^']+')|(?<Parameters>:[a-zA-Z0-9_]+)");
            MatchCollection oMatchCollection = oRegex.Matches(poCommand.CommandText);

            string strQuery = poCommand.CommandText + " ";
            int matchCount = 0;

            for (int i = 0; i < oMatchCollection.Count; i++)
            {
                if (oMatchCollection[i].Groups["string"].Success)
                    continue;

                string strParameter = oMatchCollection[i].Groups["Parameters"].Captures[0].Value;

                var param = (IDbDataParameter)poCommand.Parameters[matchCount];
                if (param.Value is DateTime)
                {
                    var dt = (DateTime)param.Value;

                    strQuery = strQuery.Replace(strParameter + " ",
                                                dt.Date == dt
                                                    ? SqlConvertDate(dt) + " "
                                                    : SqlConvertDateTime(dt) + " ");
                }
                else if (param.Value is string)
                    strQuery = strQuery.Replace(strParameter, SqlConvertString(param.Value.ToString()) + " ");
                else if (param.Value is Int16)
                    strQuery = strQuery.Replace(strParameter, ((Int16)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is Int32)
                    strQuery = strQuery.Replace(strParameter, ((Int32)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is Int64)
                    strQuery = strQuery.Replace(strParameter, ((Int64)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is decimal)
                    strQuery = strQuery.Replace(strParameter, ((decimal)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is float)
                    strQuery = strQuery.Replace(strParameter, ((float)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is double)
                    strQuery = strQuery.Replace(strParameter, ((double)param.Value).ToString(CultureInfo.InvariantCulture) + " ");
                else if (param.Value is TimeSpan)
                    strQuery = strQuery.Replace(strParameter, "'" + ((TimeSpan)param.Value).ToString() + "' ");
                else
                    throw new NotImplementedException(param.Value.GetType() + " is not implemented yet.");

                matchCount++;
            }

            if (matchCount != poCommand.Parameters.Count)
            {
                // ReSharper disable InvocationIsSkipped
                Debug.WriteLine(
                    "Number of parameters in query is not equals to number of parameters set in the command object " +
                    poCommand.CommandText);
                // ReSharper restore InvocationIsSkipped
                var msg =
                    "Number of parameters in query is not equals to number of parameters set in the command object : " + poCommand.CommandText + "\r\n" +
                    "Query params :\r\n";

                foreach (Match match in oMatchCollection)
                {
                    msg += "\t" + match.Value + "\r\n";
                }

                msg += "\nCommand params :\r\n";

                foreach (IDataParameter param in poCommand.Parameters)
                {
                    msg += "\t" + param.ParameterName + " = " + Convert.ToString(param) + "\r\n";
                }

                throw new Exception(msg);
            }

            return strQuery;
        }

    }
}