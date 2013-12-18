// Odp.Net Data Provider.
// http://www.oracle.com/technology/tech/windows/odpnet/index.html
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

using BLToolkit.Aspects;
using BLToolkit.Common;
using BLToolkit.Data.DataProvider.Interpreters;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

#if MANAGED
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
#else
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
#endif

namespace BLToolkit.Data.DataProvider
{
    using Sql.SqlProvider;

    /// <summary>
    /// Implements access to the Data Provider for Oracle.
    /// </summary>
    /// <remarks>
    /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
    /// </remarks>
    /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
#if !MANAGED
    public class OdpDataProvider : DataProviderBase
    {
        private readonly DataProviderInterpreterBase _interpreterBase;

        public OdpDataProvider()
        {
            MappingSchema = new OdpMappingSchema();
            _interpreterBase = new OracleDataProviderInterpreter();
        }

        public const string NameString = DataProvider.ProviderName.Oracle;

        private const string DbTypeTableName = "Oracle.DataAccess.Client.OraDb_DbTypeTable";

        static OdpDataProvider()
        {
#else
	public class OdpManagedDataProvider : DataProviderBase
	{
        private readonly DataProviderInterpreterBase _interpreterBase;

		public OdpManagedDataProvider()
		{
			MappingSchema = new OdpMappingSchema();
            _interpreterBase = new OracleDataProviderInterpreter();
		}

		public const string NameString = DataProvider.ProviderName.OracleManaged;

		private const string DbTypeTableName = "Oracle.ManagedDataAccess.Client.OraDb_DbTypeTable";

		static OdpManagedDataProvider()
		{
#endif
            // Fix Oracle.Net bug #1: Array types are not handled.
            //
            var oraDbDbTypeTableType = typeof(OracleParameter).Assembly.GetType(DbTypeTableName);

            if (null != oraDbDbTypeTableType)
            {
                var typeTable = (Hashtable)oraDbDbTypeTableType.InvokeMember(
                    "s_table", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField,
                    null, null, Type.EmptyTypes);

                if (null != typeTable)
                {
                    typeTable[typeof(DateTime[])] = OracleDbType.TimeStamp;
                    typeTable[typeof(Int16[])] = OracleDbType.Int16;
                    typeTable[typeof(Int32[])] = OracleDbType.Int32;
                    typeTable[typeof(Int64[])] = OracleDbType.Int64;
                    typeTable[typeof(Single[])] = OracleDbType.Single;
                    typeTable[typeof(Double[])] = OracleDbType.Double;
                    typeTable[typeof(Decimal[])] = OracleDbType.Decimal;
                    typeTable[typeof(TimeSpan[])] = OracleDbType.IntervalDS;
                    typeTable[typeof(String[])] = OracleDbType.Varchar2;
                    typeTable[typeof(OracleBFile[])] = OracleDbType.BFile;
                    typeTable[typeof(OracleBinary[])] = OracleDbType.Raw;
                    typeTable[typeof(OracleBlob[])] = OracleDbType.Blob;
                    typeTable[typeof(OracleClob[])] = OracleDbType.Clob;
                    typeTable[typeof(OracleDate[])] = OracleDbType.Date;
                    typeTable[typeof(OracleDecimal[])] = OracleDbType.Decimal;
                    typeTable[typeof(OracleIntervalDS[])] = OracleDbType.IntervalDS;
                    typeTable[typeof(OracleIntervalYM[])] = OracleDbType.IntervalYM;
                    typeTable[typeof(OracleRefCursor[])] = OracleDbType.RefCursor;
                    typeTable[typeof(OracleString[])] = OracleDbType.Varchar2;
                    typeTable[typeof(OracleTimeStamp[])] = OracleDbType.TimeStamp;
                    typeTable[typeof(OracleTimeStampLTZ[])] = OracleDbType.TimeStampLTZ;
                    typeTable[typeof(OracleTimeStampTZ[])] = OracleDbType.TimeStampTZ;
#if !MANAGED
                    typeTable[typeof(OracleXmlType[])] = OracleDbType.XmlType;
#endif

                    typeTable[typeof(Boolean)] = OracleDbType.Byte;
                    typeTable[typeof(Guid)] = OracleDbType.Raw;
                    typeTable[typeof(SByte)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt16)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt32)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt64)] = OracleDbType.Decimal;

                    typeTable[typeof(Boolean[])] = OracleDbType.Byte;
                    typeTable[typeof(Guid[])] = OracleDbType.Raw;
                    typeTable[typeof(SByte[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt16[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt32[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt64[])] = OracleDbType.Decimal;

                    typeTable[typeof(Boolean?)] = OracleDbType.Byte;
                    typeTable[typeof(Guid?)] = OracleDbType.Raw;
                    typeTable[typeof(SByte?)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt16?)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt32?)] = OracleDbType.Decimal;
                    typeTable[typeof(UInt64?)] = OracleDbType.Decimal;
                    typeTable[typeof(DateTime?[])] = OracleDbType.TimeStamp;
                    typeTable[typeof(Int16?[])] = OracleDbType.Int16;
                    typeTable[typeof(Int32?[])] = OracleDbType.Int32;
                    typeTable[typeof(Int64?[])] = OracleDbType.Int64;
                    typeTable[typeof(Single?[])] = OracleDbType.Single;
                    typeTable[typeof(Double?[])] = OracleDbType.Double;
                    typeTable[typeof(Decimal?[])] = OracleDbType.Decimal;
                    typeTable[typeof(TimeSpan?[])] = OracleDbType.IntervalDS;
                    typeTable[typeof(Boolean?[])] = OracleDbType.Byte;
                    typeTable[typeof(Guid?[])] = OracleDbType.Raw;
                    typeTable[typeof(SByte?[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt16?[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt32?[])] = OracleDbType.Decimal;
                    typeTable[typeof(UInt64?[])] = OracleDbType.Decimal;

                    typeTable[typeof(XmlReader)] = OracleDbType.XmlType;
                    typeTable[typeof(XmlDocument)] = OracleDbType.XmlType;
                    typeTable[typeof(MemoryStream)] = OracleDbType.Blob;
                    typeTable[typeof(XmlReader[])] = OracleDbType.XmlType;
                    typeTable[typeof(XmlDocument[])] = OracleDbType.XmlType;
                    typeTable[typeof(MemoryStream[])] = OracleDbType.Blob;
                }
            }
        }

        /// <summary>
        /// Creates the database connection object.
        /// </summary>
        /// <remarks>
        /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
        /// </remarks>
        /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
        /// <returns>The database connection object.</returns>
        public override IDbConnection CreateConnectionObject()
        {
            return new OracleConnection();
        }

        public override IDbCommand CreateCommandObject(IDbConnection connection)
        {
            var oraConnection = connection as OracleConnection;

            if (null != oraConnection)
            {
                var oraCommand = oraConnection.CreateCommand();

                // Fix Oracle.Net bug #2: Empty arrays can not be sent to the server.
                //
                oraCommand.BindByName = true;

                return oraCommand;
            }

            return base.CreateCommandObject(connection);
        }

		public override void SetParameterValue(IDbDataParameter parameter, object value)
		{
            // We need NVarChar2 in order to insert UTF8 string values. The default Odp VarChar2 dbtype doesnt work
            // with UTF8 values. Note : Microsoft oracle client uses NVarChar value by default.

// ReSharper disable once SuspiciousTypeConversion.Global
            var wrap = parameter as OracleParameterWrap;
            if (wrap != null && value is string)
                wrap.OracleParameter.OracleDbType = OracleDbType.NVarchar2;

            _interpreterBase.SetParameterValue(parameter, value);

			// strings and byte arrays larger than 4000 bytes may be handled improperly
		    if (wrap != null)
			{
				const int thresholdSize = 4000;
				if (value is string && Encoding.UTF8.GetBytes((string)value).Length > thresholdSize)
				{
					wrap.OracleParameter.OracleDbType = OracleDbType.Clob;
				}
				else if (value is byte[] && ((byte[])value).Length > thresholdSize)
				{
					wrap.OracleParameter.OracleDbType = OracleDbType.Blob;
				}
			}
		}

		public override IDbDataParameter CloneParameter(IDbDataParameter parameter)
		{
			var oraParameter = (parameter is OracleParameterWrap)?
				(parameter as OracleParameterWrap).OracleParameter: parameter as OracleParameter;

            if (null != oraParameter)
            {
                var oraParameterClone = (OracleParameter)oraParameter.Clone();

                // Fix Oracle.Net bug #3: CollectionType property is not cloned.
                //
                oraParameterClone.CollectionType = oraParameter.CollectionType;

                // Fix Oracle.Net bug #8423178
                // See http://forums.oracle.com/forums/thread.jspa?threadID=975902&tstart=0
                //
                if (oraParameterClone.OracleDbType == OracleDbType.RefCursor)
                {
                    // Set OracleDbType to itself to reset m_bSetDbType and m_bOracleDbTypeExSet
                    //
                    oraParameterClone.OracleDbType = OracleDbType.RefCursor;
                }

                return OracleParameterWrap.CreateInstance(oraParameterClone);
            }

            return base.CloneParameter(parameter);
        }

        public override void SetUserDefinedType(IDbDataParameter parameter, string typeName)
        {
            var oraParameter = (parameter is OracleParameterWrap) ?
                (parameter as OracleParameterWrap).OracleParameter : parameter as OracleParameter;

            if (oraParameter == null)
                throw new ArgumentException("OracleParameter expected.", "parameter");

            oraParameter.UdtTypeName = typeName;
        }

        /// <summary>
        /// Creates the data adapter object.
        /// </summary>
        /// <remarks>
        /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
        /// </remarks>
        /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
        /// <returns>A data adapter object.</returns>
        public override DbDataAdapter CreateDataAdapterObject()
        {
            return new OracleDataAdapter();
        }

        /// <summary>
        /// Populates the specified IDbCommand object's Parameters collection with 
        /// parameter information for the stored procedure specified in the IDbCommand.
        /// </summary>
        /// <remarks>
        /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
        /// </remarks>
        /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
        /// <param name="command">The IDbCommand referencing the stored procedure for which the parameter
        /// information is to be derived. The derived parameters will be populated into
        /// the Parameters of this command.</param>
        public override bool DeriveParameters(IDbCommand command)
        {
            var oraCommand = command as OracleCommand;

            if (null != oraCommand)
            {
                try
                {
                    OracleCommandBuilder.DeriveParameters(oraCommand);
                }
                catch (Exception ex)
                {
                    // Make Oracle less laconic.
                    //
                    throw new DataException(string.Format("{0}\nCommandText: {1}", ex.Message, oraCommand.CommandText), ex);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Open an <see cref="IDataReader"/> into the given <see cref="OracleRefCursor"/> object
        /// </summary>
        /// <param name="refCursor">an <see cref="OracleRefCursor"/> to perform GetDataReader() on</param>
        /// <returns>The <see cref="IDataReader"/> into the returned by GetDataReader()</returns>
        public override IDataReader GetRefCursorDataReader(object refCursor)
        {
            OracleRefCursor oracleRefCursor = refCursor as OracleRefCursor;
            if (oracleRefCursor == null)
            {
                throw new ArgumentException("Argument must be of type 'OracleRefCursor'", "refCursor");
            }
            return oracleRefCursor.GetDataReader();
        }

        public override object Convert(object value, ConvertType convertType)
        {
            switch (convertType)
            {
                case ConvertType.NameToQueryParameter:
                    var qname = (string)value;

                    //
                    // Avoid "ORA-00972: identifier is too long" error
                    // Cause error : You tried to reference a table, cluster, view, index, synonym, tablespace, or username with a value that was longer than 30 characters.
                    // Resolution : Names for tables, clusters, views, indexes, synonyms, tablespaces, and usernames must be 30 characters or less. 
                    // You must shorten the name to no more than 30 characters for these objects.
                    //
                    if (qname.Length > 30)
                    {
                        qname = qname.Substring(0, 30);
                        return SqlProvider.Convert(qname, convertType);
                    }
                    return SqlProvider.Convert(value, convertType);

                case ConvertType.NameToCommandParameter:
                case ConvertType.NameToSprocParameter:
                    return ParameterPrefix == null ? value : ParameterPrefix + value;

                case ConvertType.SprocParameterToName:
                    var name = (string)value;

                    if (name.Length > 0)
                    {
                        if (name[0] == ':')
                            return name.Substring(1);

                        if (ParameterPrefix != null &&
                            name.ToUpper(CultureInfo.InvariantCulture).StartsWith(ParameterPrefix))
                        {
                            return name.Substring(ParameterPrefix.Length);
                        }
                    }

                    break;

                case ConvertType.ExceptionToErrorNumber:
                    if (value is OracleException)
                        return ((OracleException)value).Number;
                    break;
            }

            return SqlProvider.Convert(value, convertType);
        }

        public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
        {
            base.PrepareCommand(ref commandType, ref commandText, ref commandParameters);

            if (commandType == CommandType.Text)
            {
                // Fix Oracle bug #11 '\r' is not a valid character!
                //
                commandText = commandText.Replace('\r', ' ');
            }
        }

        public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
        {
            var oraParameter = (parameter is OracleParameterWrap) ?
                (parameter as OracleParameterWrap).OracleParameter : parameter as OracleParameter;

            if (null != oraParameter)
            {
                if (oraParameter.CollectionType == OracleCollectionType.PLSQLAssociativeArray)
                {
                    if (oraParameter.Direction == ParameterDirection.Input
                        || oraParameter.Direction == ParameterDirection.InputOutput)
                    {
                        var ar = oraParameter.Value as Array;

                        if (null != ar && !(ar is byte[] || ar is char[]))
                        {
                            oraParameter.Size = ar.Length;

                            if (oraParameter.DbType == DbType.String
                                && oraParameter.Direction == ParameterDirection.InputOutput)
                            {
                                var arrayBindSize = new int[oraParameter.Size];

                                for (var i = 0; i < oraParameter.Size; ++i)
                                {
                                    arrayBindSize[i] = 1024;
                                }

                                oraParameter.ArrayBindSize = arrayBindSize;
                            }
                        }

                        if (oraParameter.Size == 0)
                        {
                            // Skip this parameter.
                            // Fix Oracle.Net bug #2: Empty arrays can not be sent to the server.
                            //
                            return;
                        }

                        if (oraParameter.Value is Stream[])
                        {
                            var streams = (Stream[])oraParameter.Value;

                            for (var i = 0; i < oraParameter.Size; ++i)
                            {
                                if (streams[i] is OracleBFile || streams[i] is OracleBlob || streams[i] is OracleClob
#if !MANAGED
                                    || streams[i] is OracleXmlStream
#endif
                                    )
                                {
                                    // Known Oracle type.
                                    //
                                    continue;
                                }

                                streams[i] = CopyStream(streams[i], (OracleCommand)command);
                            }
                        }
                        else if (oraParameter.Value is XmlDocument[])
                        {
                            var xmlDocuments = (XmlDocument[])oraParameter.Value;
                            var values = new object[oraParameter.Size];

                            switch (oraParameter.OracleDbType)
                            {
                                case OracleDbType.XmlType:
#if !MANAGED
                                    for (var i = 0; i < oraParameter.Size; ++i)
                                    {
                                        values[i] = xmlDocuments[i].DocumentElement == null ?
                                            (object)DBNull.Value :
                                            new OracleXmlType((OracleConnection)command.Connection, xmlDocuments[i]);
                                    }

                                    oraParameter.Value = values;

                                    break;

#else
									throw new NotSupportedException();
#endif

                                // Fix Oracle.Net bug #9: XmlDocument.ToString() returns System.Xml.XmlDocument,
                                // so m_value.ToString() is not enought.
                                //
                                case OracleDbType.Clob:
                                case OracleDbType.NClob:
                                case OracleDbType.Varchar2:
                                case OracleDbType.NVarchar2:
                                case OracleDbType.Char:
                                case OracleDbType.NChar:
                                    for (var i = 0; i < oraParameter.Size; ++i)
                                    {
                                        values[i] = xmlDocuments[i].DocumentElement == null ?
                                            (object)DBNull.Value :
                                            xmlDocuments[i].InnerXml;
                                    }

                                    oraParameter.Value = values;

                                    break;

                                // Or convert to bytes if need.
                                //
                                case OracleDbType.Blob:
                                case OracleDbType.BFile:
                                case OracleDbType.Raw:
                                case OracleDbType.Long:
                                case OracleDbType.LongRaw:
                                    for (var i = 0; i < oraParameter.Size; ++i)
                                    {
                                        if (xmlDocuments[i].DocumentElement == null)
                                            values[i] = DBNull.Value;
                                        else
                                            using (var s = new MemoryStream())
                                            {
                                                xmlDocuments[i].Save(s);
                                                values[i] = s.GetBuffer();
                                            }
                                    }

                                    oraParameter.Value = values;

                                    break;
                            }
                        }
                    }
                    else if (oraParameter.Direction == ParameterDirection.Output)
                    {
                        // Fix Oracle.Net bug #4: ArrayBindSize must be explicitly specified.
                        //
                        if (oraParameter.DbType == DbType.String)
                        {
                            oraParameter.Size = 1024;
                            var arrayBindSize = new int[oraParameter.Size];
                            for (var i = 0; i < oraParameter.Size; ++i)
                            {
                                arrayBindSize[i] = 1024;
                            }

                            oraParameter.ArrayBindSize = arrayBindSize;
                        }
                        else
                        {
                            oraParameter.Size = 32767;
                        }
                    }
                }
                else if (oraParameter.Value is Stream)
                {
                    var stream = (Stream)oraParameter.Value;

                    if (!(stream is OracleBFile) && !(stream is OracleBlob) &&
                        !(stream is OracleClob)
#if !MANAGED
                        && !(stream is OracleXmlStream)
#endif
                        )
                    {
                        oraParameter.Value = CopyStream(stream, (OracleCommand)command);
                    }
                }
                else if (oraParameter.Value is Byte[])
                {
                    var bytes = (Byte[])oraParameter.Value;

                    if (bytes.Length > 32000)
                    {
                        oraParameter.Value = CopyStream(bytes, (OracleCommand)command);
                    }
                }
                else if (oraParameter.Value is XmlDocument)
                {
                    var xmlDocument = (XmlDocument)oraParameter.Value;
                    if (xmlDocument.DocumentElement == null)
                        oraParameter.Value = DBNull.Value;
                    else
                    {

                        switch (oraParameter.OracleDbType)
                        {
                            case OracleDbType.XmlType:
#if !MANAGED
								oraParameter.Value = new OracleXmlType((OracleConnection)command.Connection, xmlDocument);
								break;
#else
								throw new NotSupportedException();
#endif

                            // Fix Oracle.Net bug #9: XmlDocument.ToString() returns System.Xml.XmlDocument,
                            // so m_value.ToString() is not enought.
                            //
                            case OracleDbType.Clob:
                            case OracleDbType.NClob:
                            case OracleDbType.Varchar2:
                            case OracleDbType.NVarchar2:
                            case OracleDbType.Char:
                            case OracleDbType.NChar:
                                using (TextWriter w = new StringWriter())
                                {
                                    xmlDocument.Save(w);
                                    oraParameter.Value = w.ToString();
                                }
                                break;

                            // Or convert to bytes if need.
                            //
                            case OracleDbType.Blob:
                            case OracleDbType.BFile:
                            case OracleDbType.Raw:
                            case OracleDbType.Long:
                            case OracleDbType.LongRaw:
                                using (var s = new MemoryStream())
                                {
                                    xmlDocument.Save(s);
                                    oraParameter.Value = s.GetBuffer();
                                }
                                break;
                        }
                    }
                }

                parameter = oraParameter;
            }

            base.AttachParameter(command, parameter);
        }

        public override DbType GetParameterDbType(DbType dbType)
        {
            return _interpreterBase.GetParameterDbType(dbType);
        }

        private static Stream CopyStream(Stream stream, OracleCommand cmd)
        {
            return CopyStream(Common.Convert.ToByteArray(stream), cmd);
        }

        private static Stream CopyStream(Byte[] bytes, OracleCommand cmd)
        {
            var ret = new OracleBlob(cmd.Connection);
            ret.Write(bytes, 0, bytes.Length);
            return ret;
        }

        public override bool IsValueParameter(IDbDataParameter parameter)
        {
            var oraParameter = (parameter is OracleParameterWrap) ?
                (parameter as OracleParameterWrap).OracleParameter : parameter as OracleParameter;

            if (null != oraParameter)
            {
                if (oraParameter.OracleDbType == OracleDbType.RefCursor
                    && oraParameter.Direction == ParameterDirection.Output)
                {
                    // Ignore out ref cursors, while out parameters of other types are o.k.
                    return false;
                }
            }

            return base.IsValueParameter(parameter);
        }

        public override string GetSequenceQuery(string sequenceName)
        {
            return _interpreterBase.GetSequenceQuery(sequenceName);
        }

        public override string NextSequenceQuery(string sequenceName)
        {
            return _interpreterBase.NextSequenceQuery(sequenceName);
        }

        public override string GetReturningInto(string columnName)
        {
            return _interpreterBase.GetReturningInto(columnName);
        }

        public override IDbDataParameter CreateParameterObject(IDbCommand command)
        {
            var parameter = base.CreateParameterObject(command);

            if (parameter is OracleParameter)
                parameter = OracleParameterWrap.CreateInstance(parameter as OracleParameter);

            return parameter;
        }

        public override IDbDataParameter GetParameter(IDbCommand command, NameOrIndexParameter nameOrIndex)
        {
            var parameter = base.GetParameter(command, nameOrIndex);

            if (parameter is OracleParameter)
                parameter = OracleParameterWrap.CreateInstance(parameter as OracleParameter);

            return parameter;
        }

        /// <summary>
        /// Returns connection type.
        /// </summary>
        /// <remarks>
        /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
        /// </remarks>
        /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
        /// <value>An instance of the <see cref="Type"/> class.</value>
        public override Type ConnectionType
        {
            get { return typeof(OracleConnection); }
        }

        /// <summary>
        /// Returns the data provider name.
        /// </summary>
        /// <remarks>
        /// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
        /// </remarks>
        /// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataProvider Method</seealso>
        /// <value>Data provider name.</value>
        public override string Name
        {
            get { return NameString; }
        }

        public override int MaxBatchSize
        {
            get { return 0; }
        }

        public override ISqlProvider CreateSqlProvider()
        {
            return new OracleSqlProvider();
        }

        public override int ExecuteArray(IDbCommand command, int iterations)
		{
			var cmd = (OracleCommand)command;
			var oracleParameters = cmd.Parameters.OfType<OracleParameter>().ToArray();
			var oldCollectionTypes = oracleParameters.Select(p => p.CollectionType).ToArray();

			try
			{
				foreach (var p in oracleParameters)
				{
					p.CollectionType = OracleCollectionType.None;
				}

				cmd.ArrayBindCount = iterations;
				return cmd.ExecuteNonQuery();
			}
			finally
			{
				foreach (var p in oracleParameters.Zip(oldCollectionTypes, (p, t) => new { Param = p, CollectionType = t }))
				{
					p.Param.CollectionType = p.CollectionType;
				}

				cmd.ArrayBindCount = 0;
			}
		}

        public override IDataReader GetDataReader(MappingSchema schema, IDataReader dataReader)
        {
            return dataReader is OracleDataReader ?
                new OracleDataReaderEx((OracleDataReader)dataReader) :
                base.GetDataReader(schema, dataReader);
        }

        class OracleDataReaderEx : DataReaderEx<OracleDataReader>
        {
            public OracleDataReaderEx(OracleDataReader rd)
                : base(rd)
            {
            }

            public override object GetValue(int i)
            {
                string dataTypeName = GetDataTypeName(i);
                if (dataTypeName == "Clob")
                {
                    OracleClob clob = DataReader.GetOracleClob(i);
                    if (!clob.IsNull)
                    {
                        return clob;
                        //byte[] b = new byte[clob.Length];
                        ////Read data from database
                        //clob.Read(b, 0, (int)clob.Length);

                        //return b;
                        //return clob.Value;
                    }
                    else
                        return null;
                }
                else
                    return base.GetValue(i);
            }

            public override DateTimeOffset GetDateTimeOffset(int i)
            {
                var ts = DataReader.GetOracleTimeStampTZ(i);
                return new DateTimeOffset(ts.Value, ts.GetTimeZoneOffset());
            }
        }

        private string _parameterPrefix = "P";
        public string ParameterPrefix
        {
            get { return _parameterPrefix; }
            set
            {
                _parameterPrefix = string.IsNullOrEmpty(value) ? null :
                    value.ToUpper(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// One time initialization from a configuration file.
        /// </summary>
        /// <param name="attributes">Provider specific attributes.</param>
        public override void Configure(System.Collections.Specialized.NameValueCollection attributes)
        {
            var val = attributes["ParameterPrefix"];
            if (val != null)
                ParameterPrefix = val;

            base.Configure(attributes);
        }

        #region Inner types

        public class OdpMappingSchema : MappingSchema
        {
            public override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
            {
                return new OracleDataReaderMapper(this, dataReader);
            }

            public override DataReaderMapper CreateDataReaderMapper(
                IDataReader dataReader,
                NameOrIndexParameter nip)
            {
                return new OracleScalarDataReaderMapper(this, dataReader, nip);
            }

            public override Reflection.Extension.ExtensionList Extensions
            {
                get { return Map.DefaultSchema.Extensions; }
                set { Map.DefaultSchema.Extensions = value; }
            }

            #region Convert

            #region Primitive Types

            [CLSCompliant(false)]
            public override SByte ConvertToSByte(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultSByteNullValue : (SByte)oraDecimal.Value;
                }

                return base.ConvertToSByte(value);
            }

            public override Int16 ConvertToInt16(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultInt16NullValue : oraDecimal.ToInt16();
                }

                return base.ConvertToInt16(value);
            }

            public override Int32 ConvertToInt32(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultInt32NullValue : oraDecimal.ToInt32();
                }

                return base.ConvertToInt32(value);
            }

            public override Int64 ConvertToInt64(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultInt64NullValue : oraDecimal.ToInt64();
                }

                return base.ConvertToInt64(value);
            }

            public override Byte ConvertToByte(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultByteNullValue : oraDecimal.ToByte();
                }

                return base.ConvertToByte(value);
            }

            [CLSCompliant(false)]
            public override UInt16 ConvertToUInt16(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultUInt16NullValue : (UInt16)oraDecimal.Value;
                }

                return base.ConvertToUInt16(value);
            }

            [CLSCompliant(false)]
            public override UInt32 ConvertToUInt32(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultUInt32NullValue : (UInt32)oraDecimal.Value;
                }

                return base.ConvertToUInt32(value);
            }

            [CLSCompliant(false)]
            public override UInt64 ConvertToUInt64(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultUInt64NullValue : (UInt64)oraDecimal.Value;
                }

                return base.ConvertToUInt64(value);
            }

            public override Single ConvertToSingle(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultSingleNullValue : oraDecimal.ToSingle();
                }

                return base.ConvertToSingle(value);
            }

            public override Double ConvertToDouble(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultDoubleNullValue : oraDecimal.ToDouble();
                }

                return base.ConvertToDouble(value);
            }

            public override Boolean ConvertToBoolean(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultBooleanNullValue : (oraDecimal.Value != 0);
                }

                return base.ConvertToBoolean(value);
            }

            public override DateTime ConvertToDateTime(object value)
            {
                if (value is OracleDate)
                {
                    var oraDate = (OracleDate)value;
                    return oraDate.IsNull ? DefaultDateTimeNullValue : oraDate.Value;
                }

                return base.ConvertToDateTime(value);
            }

            public override Decimal ConvertToDecimal(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? DefaultDecimalNullValue : oraDecimal.Value;
                }

                return base.ConvertToDecimal(value);
            }

            public override Guid ConvertToGuid(object value)
            {
                if (value is OracleString)
                {
                    var oraString = (OracleString)value;
                    return oraString.IsNull ? DefaultGuidNullValue : new Guid(oraString.Value);
                }

                if (value is OracleBlob)
                {
                    using (var oraBlob = (OracleBlob) value)
                        return oraBlob.IsNull ? DefaultGuidNullValue : new Guid(oraBlob.Value);
                }

                return base.ConvertToGuid(value);
            }

            public override String ConvertToString(object value)
            {
                if (value is OracleString)
                {
                    var oraString = (OracleString)value;
                    return oraString.IsNull ? DefaultStringNullValue : oraString.Value;
                }
#if !MANAGED
                if (value is OracleXmlType)
                {
                    var oraXmlType = (OracleXmlType)value;
                    return oraXmlType.IsNull ? DefaultStringNullValue : oraXmlType.Value;
                }
#endif
                if (value is OracleClob)
                {
                    using (var oraClob = (OracleClob) value)
                        return oraClob.IsNull ? DefaultStringNullValue : oraClob.Value;
                }

                return base.ConvertToString(value);
            }

#if !MANAGED
            public override Stream ConvertToStream(object value)
            {
                if (value is OracleXmlType)
                {
                    var oraXml = (OracleXmlType)value;
                    return oraXml.IsNull ? DefaultStreamNullValue : oraXml.GetStream();
                }

                return base.ConvertToStream(value);
            }

            public override XmlReader ConvertToXmlReader(object value)
            {
                if (value is OracleXmlType)
                {
                    var oraXml = (OracleXmlType)value;
                    return oraXml.IsNull ? DefaultXmlReaderNullValue : oraXml.GetXmlReader();
                }

                return base.ConvertToXmlReader(value);
            }

            public override XmlDocument ConvertToXmlDocument(object value)
            {
                if (value is OracleXmlType)
                {
                    var oraXml = (OracleXmlType)value;
                    return oraXml.IsNull ? DefaultXmlDocumentNullValue : oraXml.GetXmlDocument();
                }

                return base.ConvertToXmlDocument(value);
            }
#endif

            public override Byte[] ConvertToByteArray(object value)
            {
                if (value is OracleBlob)
                {
                    using (var oraBlob = (OracleBlob) value)
                        return oraBlob.IsNull ? null : oraBlob.Value;
                }

                if (value is OracleBinary)
                {
                    var oraBinary = (OracleBinary)value;
                    return oraBinary.IsNull ? null : oraBinary.Value;
                }

                if (value is OracleBFile)
                {
                    var oraBFile = (OracleBFile)value;
                    return oraBFile.IsNull ? null : oraBFile.Value;
                }

                return base.ConvertToByteArray(value);
            }

            public override Char[] ConvertToCharArray(object value)
            {
                if (value is OracleString)
                {
                    var oraString = (OracleString)value;
                    return oraString.IsNull ? null : oraString.Value.ToCharArray();
                }

                if (value is OracleClob)
                {
                    using (var oraClob = (OracleClob) value)
                        return oraClob.IsNull ? null : oraClob.Value.ToCharArray();
                }

                return base.ConvertToCharArray(value);
            }

            #endregion

            #region Nullable Types

            [CLSCompliant(false)]
            public override SByte? ConvertToNullableSByte(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (SByte?)oraDecimal.Value;
                }

                return base.ConvertToNullableSByte(value);
            }

            public override Int16? ConvertToNullableInt16(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Int16?)oraDecimal.ToInt16();
                }

                return base.ConvertToNullableInt16(value);
            }

            public override Int32? ConvertToNullableInt32(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Int32?)oraDecimal.ToInt32();
                }

                return base.ConvertToNullableInt32(value);
            }

            public override Int64? ConvertToNullableInt64(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Int64?)oraDecimal.ToInt64();
                }

                return base.ConvertToNullableInt64(value);
            }

            public override Byte? ConvertToNullableByte(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Byte?)oraDecimal.ToByte();
                }

                return base.ConvertToNullableByte(value);
            }

            [CLSCompliant(false)]
            public override UInt16? ConvertToNullableUInt16(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (UInt16?)oraDecimal.Value;
                }

                return base.ConvertToNullableUInt16(value);
            }

            [CLSCompliant(false)]
            public override UInt32? ConvertToNullableUInt32(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (UInt32?)oraDecimal.Value;
                }

                return base.ConvertToNullableUInt32(value);
            }

            [CLSCompliant(false)]
            public override UInt64? ConvertToNullableUInt64(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (UInt64?)oraDecimal.Value;
                }

                return base.ConvertToNullableUInt64(value);
            }

            public override Single? ConvertToNullableSingle(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Single?)oraDecimal.ToSingle();
                }

                return base.ConvertToNullableSingle(value);
            }

            public override Double? ConvertToNullableDouble(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Double?)oraDecimal.ToDouble();
                }

                return base.ConvertToNullableDouble(value);
            }

            public override Boolean? ConvertToNullableBoolean(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Boolean?)(oraDecimal.Value != 0);
                }

                return base.ConvertToNullableBoolean(value);
            }

            public override DateTime? ConvertToNullableDateTime(object value)
            {
                if (value is OracleDate)
                {
                    var oraDate = (OracleDate)value;
                    return oraDate.IsNull ? null : (DateTime?)oraDate.Value;
                }

                return base.ConvertToNullableDateTime(value);
            }

            public override Decimal? ConvertToNullableDecimal(object value)
            {
                if (value is OracleDecimal)
                {
                    var oraDecimal = (OracleDecimal)value;
                    return oraDecimal.IsNull ? null : (Decimal?)oraDecimal.Value;
                }

                return base.ConvertToNullableDecimal(value);
            }

            public override Guid? ConvertToNullableGuid(object value)
            {
                if (value is OracleString)
                {
                    var oraString = (OracleString)value;
                    return oraString.IsNull ? null : (Guid?)new Guid(oraString.Value);
                }

                if (value is OracleBlob)
                {
                    var oraBlob = (OracleBlob)value;
                    return oraBlob.IsNull ? null : (Guid?)new Guid(oraBlob.Value);
                }

                return base.ConvertToNullableGuid(value);
            }

            #endregion

            #endregion

            public override object MapValueToEnum(object value, Type type)
            {
                if (value is OracleString)
                {
                    var oracleValue = (OracleString)value;
                    value = oracleValue.IsNull ? null : oracleValue.Value;
                }
                else if (value is OracleDecimal)
                {
                    var oracleValue = (OracleDecimal)value;
                    if (oracleValue.IsNull)
                        value = null;
                    else
                        value = oracleValue.Value;
                }

                return base.MapValueToEnum(value, type);
            }

            public override object ConvertChangeType(object value, Type conversionType)
            {
                // Handle OracleDecimal with IsNull == true case
                //
                return base.ConvertChangeType(IsNull(value) ? null : value, conversionType);
            }

            public override bool IsNull(object value)
            {
                // ODP 10 does not expose this interface to public.
                //
                // return value is INullable && ((INullable)value).IsNull;

                return
                    value is OracleDecimal ? ((OracleDecimal)value).IsNull :
                    value is OracleString ? ((OracleString)value).IsNull :
                    value is OracleDate ? ((OracleDate)value).IsNull :
                    value is OracleTimeStamp ? ((OracleTimeStamp)value).IsNull :
                    value is OracleTimeStampTZ ? ((OracleTimeStampTZ)value).IsNull :
                    value is OracleTimeStampLTZ ? ((OracleTimeStampLTZ)value).IsNull :
#if !MANAGED
                    value is OracleXmlType ? ((OracleXmlType)value).IsNull :
#endif
                    value is OracleBlob ? ((OracleBlob)value).IsNull :
                    value is OracleClob ? ((OracleClob)value).IsNull :
                    value is OracleBFile ? ((OracleBFile)value).IsNull :
                    value is OracleBinary ? ((OracleBinary)value).IsNull :
                    value is OracleIntervalDS ? ((OracleIntervalDS)value).IsNull :
                    value is OracleIntervalYM ? ((OracleIntervalYM)value).IsNull :
                        base.IsNull(value);
            }
        }

        // TODO: implement via IDataReaderEx / DataReaderEx
        //
        public class OracleDataReaderMapper : DataReaderMapper
        {
            public OracleDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader)
                : base(mappingSchema, dataReader)
            {
                _dataReader = dataReader is OracleDataReaderEx ?
                    ((OracleDataReaderEx)dataReader).DataReader :
                    (OracleDataReader)dataReader;
            }

            private readonly OracleDataReader _dataReader;

            public override Type GetFieldType(int index)
            {
                var fieldType = _dataReader.GetProviderSpecificFieldType(index);

                if (fieldType != typeof(OracleBlob)
#if !MANAGED
                    && fieldType != typeof(OracleXmlType)
#endif
                    )
                    fieldType = _dataReader.GetFieldType(index);

                return fieldType;
            }

            public override object GetValue(object o, int index)
            {
                var fieldType = _dataReader.GetProviderSpecificFieldType(index);

#if !MANAGED
                if (fieldType == typeof(OracleXmlType))
                {
                    var xml = _dataReader.GetOracleXmlType(index);
                    return MappingSchema.ConvertToXmlDocument(xml);
                }
#endif
                if (fieldType == typeof(OracleBlob))
                {
                    var blob = _dataReader.GetOracleBlob(index);
                    return MappingSchema.ConvertToStream(blob);
                }

                return _dataReader.IsDBNull(index) ? null :
                    _dataReader.GetValue(index);
            }

            public override Boolean GetBoolean(object o, int index) { return MappingSchema.ConvertToBoolean(GetValue(o, index)); }
            public override Char GetChar(object o, int index) { return MappingSchema.ConvertToChar(GetValue(o, index)); }
            public override Guid GetGuid(object o, int index) { return MappingSchema.ConvertToGuid(GetValue(o, index)); }

            [CLSCompliant(false)]
            public override SByte GetSByte(object o, int index) { return (SByte)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt16 GetUInt16(object o, int index) { return (UInt16)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt32 GetUInt32(object o, int index) { return (UInt32)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt64 GetUInt64(object o, int index) { return (UInt64)_dataReader.GetDecimal(index); }

            public override Decimal GetDecimal(object o, int index) { return OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(index), 28).Value; }

            public override Boolean? GetNullableBoolean(object o, int index) { return MappingSchema.ConvertToNullableBoolean(GetValue(o, index)); }
            public override Char? GetNullableChar(object o, int index) { return MappingSchema.ConvertToNullableChar(GetValue(o, index)); }
            public override Guid? GetNullableGuid(object o, int index) { return MappingSchema.ConvertToNullableGuid(GetValue(o, index)); }

            [CLSCompliant(false)]
            public override SByte? GetNullableSByte(object o, int index) { return _dataReader.IsDBNull(index) ? null : (SByte?)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt16? GetNullableUInt16(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt16?)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt32? GetNullableUInt32(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt32?)_dataReader.GetDecimal(index); }
            [CLSCompliant(false)]
            public override UInt64? GetNullableUInt64(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt64?)_dataReader.GetDecimal(index); }

            public override Decimal? GetNullableDecimal(object o, int index) { return _dataReader.IsDBNull(index) ? (decimal?)null : OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(index), 28).Value; }
        }

        public class OracleScalarDataReaderMapper : ScalarDataReaderMapper
        {
            private readonly OracleDataReader _dataReader;

            public OracleScalarDataReaderMapper(
                MappingSchema mappingSchema,
                IDataReader dataReader,
                NameOrIndexParameter nameOrIndex)
                : base(mappingSchema, dataReader, nameOrIndex)
            {
                _dataReader = dataReader is OracleDataReaderEx ?
                    ((OracleDataReaderEx)dataReader).DataReader :
                    (OracleDataReader)dataReader;

                _fieldType = _dataReader.GetProviderSpecificFieldType(Index);

                if (_fieldType != typeof(OracleBlob)
#if !MANAGED
                    && _fieldType != typeof(OracleXmlType)
#endif
                    )
                    _fieldType = _dataReader.GetFieldType(Index);
            }

            private readonly Type _fieldType;

            public override Type GetFieldType(int index)
            {
                return _fieldType;
            }

            public override object GetValue(object o, int index)
            {
#if !MANAGED
                if (_fieldType == typeof(OracleXmlType))
                {
                    var xml = _dataReader.GetOracleXmlType(Index);
                    return MappingSchema.ConvertToXmlDocument(xml);
                }
#endif
                if (_fieldType == typeof(OracleBlob))
                {
                    var blob = _dataReader.GetOracleBlob(Index);
                    return MappingSchema.ConvertToStream(blob);
                }

                return _dataReader.IsDBNull(index) ? null :
                    _dataReader.GetValue(Index);
            }

            public override Boolean GetBoolean(object o, int index) { return MappingSchema.ConvertToBoolean(GetValue(o, Index)); }
            public override Char GetChar(object o, int index) { return MappingSchema.ConvertToChar(GetValue(o, Index)); }
            public override Guid GetGuid(object o, int index) { return MappingSchema.ConvertToGuid(GetValue(o, Index)); }

            [CLSCompliant(false)]
            public override SByte GetSByte(object o, int index) { return (SByte)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt16 GetUInt16(object o, int index) { return (UInt16)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt32 GetUInt32(object o, int index) { return (UInt32)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt64 GetUInt64(object o, int index) { return (UInt64)_dataReader.GetDecimal(Index); }

            public override Decimal GetDecimal(object o, int index) { return OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(Index), 28).Value; }

            public override Boolean? GetNullableBoolean(object o, int index) { return MappingSchema.ConvertToNullableBoolean(GetValue(o, Index)); }
            public override Char? GetNullableChar(object o, int index) { return MappingSchema.ConvertToNullableChar(GetValue(o, Index)); }
            public override Guid? GetNullableGuid(object o, int index) { return MappingSchema.ConvertToNullableGuid(GetValue(o, Index)); }

            [CLSCompliant(false)]
            public override SByte? GetNullableSByte(object o, int index) { return _dataReader.IsDBNull(index) ? null : (SByte?)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt16? GetNullableUInt16(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt16?)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt32? GetNullableUInt32(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt32?)_dataReader.GetDecimal(Index); }
            [CLSCompliant(false)]
            public override UInt64? GetNullableUInt64(object o, int index) { return _dataReader.IsDBNull(index) ? null : (UInt64?)_dataReader.GetDecimal(Index); }

            public override Decimal? GetNullableDecimal(object o, int index) { return _dataReader.IsDBNull(index) ? (decimal?)null : OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(Index), 28).Value; }
        }

        [Mixin(typeof(IDbDataParameter), "_oracleParameter")]
        [Mixin(typeof(IDataParameter), "_oracleParameter")]
        [Mixin(typeof(IDisposable), "_oracleParameter")]
        [Mixin(typeof(ICloneable), "_oracleParameter")]
        [CLSCompliant(false)]
        public abstract class OracleParameterWrap
        {
            protected OracleParameter _oracleParameter;
            public OracleParameter OracleParameter
            {
                get { return _oracleParameter; }
            }

            public static IDbDataParameter CreateInstance(OracleParameter oraParameter)
            {
                var wrap = TypeAccessor<OracleParameterWrap>.CreateInstanceEx();

                wrap._oracleParameter = oraParameter;

                return (IDbDataParameter)wrap;
            }

            public override string ToString()
            {
                return _oracleParameter.ToString();
            }

            ///<summary>
            ///Gets or sets the value of the parameter.
            ///</summary>
            ///<returns>
            ///An <see cref="T:System.Object"/> that is the value of the parameter.
            ///The default value is null.
            ///</returns>
            protected object Value
            {
#if CONVERTORACLETYPES
				[MixinOverride]
				get
				{
					object value = _oracleParameter.Value;
					if (value is OracleBinary)
					{
						OracleBinary oracleValue = (OracleBinary)value;
						return oracleValue.IsNull? null: oracleValue.Value;
					}
					if (value is OracleDate)
					{
						OracleDate oracleValue = (OracleDate)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleDecimal)
					{
						OracleDecimal oracleValue = (OracleDecimal)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleIntervalDS)
					{
						OracleIntervalDS oracleValue = (OracleIntervalDS)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleIntervalYM)
					{
						OracleIntervalYM oracleValue = (OracleIntervalYM)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleString)
					{
						OracleString oracleValue = (OracleString)value;
						return oracleValue.IsNull? null: oracleValue.Value;
					}
					if (value is OracleTimeStamp)
					{
						OracleTimeStamp oracleValue = (OracleTimeStamp)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleTimeStampLTZ)
					{
						OracleTimeStampLTZ oracleValue = (OracleTimeStampLTZ)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleTimeStampTZ)
					{
						OracleTimeStampTZ oracleValue = (OracleTimeStampTZ)value;
						if (oracleValue.IsNull)
							return null;
						return oracleValue.Value;
					}
					if (value is OracleXmlType)
					{
						OracleXmlType oracleValue = (OracleXmlType)value;
						return oracleValue.IsNull? null: oracleValue.Value;
					}

					return value;
				}
#endif
                [MixinOverride]
                set
                {
                    if (null != value)
                    {
                        if (value is Array && !(value is byte[] || value is char[]))
                        {
                            _oracleParameter.CollectionType = OracleCollectionType.PLSQLAssociativeArray;
                        }
                    }

                    _oracleParameter.Value = value;
                }
            }
        }

        #endregion

        #region InsertBatch

        public override int InsertBatch<T>(
            DbManager                       db,
            string                          insertText,
            IEnumerable<T>                  collection,
            MemberMapper[]                  members,
            int                             maxBatchSize,
            DbManager.ParameterProvider<T>  getParameters)
        {
            if (db.UseQueryText)
            {
                // Called if UseQuery false or out of a transaction scope
                _interpreterBase.SetCollectionIds(db, members, collection);

                var parameters = new List<IDbDataParameter>();
                List<string> sqlList = _interpreterBase.GetInsertBatchSqlList(insertText, collection, members, maxBatchSize, false, db, parameters);
                return ExecuteSqlList(db, sqlList, parameters);
            }

#if !MANAGED
            return OracleInsertBulk(db, insertText, collection, members, 1000);
#else
            throw new NotImplementedException("OracleInsertBulk not supported on OdpManagedDataProvider!");
#endif
        }

        public override int InsertBatchWithIdentity<T>(
            DbManager                       db,
            string                          insertText,
            IEnumerable<T>                  collection,
            MemberMapper[]                  members,
            int                             maxBatchSize,
            DbManager.ParameterProvider<T>  getParameters)
        {
            /*
             * ﻿OracleBulkCopy doesn't support transaction for all the records, it only support transaction for batches if UseInternalTransaction is specified.
             * ﻿If BatchSize > 0 and the UseInternalTransaction bulk copy option is specified, each batch of the bulk copy operation occurs within a transaction.
             * If the connection used to perform the bulk copy operation is already part of a transaction, an InvalidOperationException exception is raised.
             * If BatchSize > 0 and the UseInternalTransaction option is not specified, rows are sent to the database in batches of size BatchSize, but no transaction-related action is taken.
             */

            if (db.UseQueryText || db.Transaction != null)
            {
                var parameters = new List<IDbDataParameter>();
                List<string> sqlList = _interpreterBase.GetInsertBatchSqlList(insertText, collection, members, 100, true, db, parameters);
                return ExecuteSqlList(db, sqlList, parameters);
            }

#if !MANAGED            
            // Called if UseQuery false or out of a transaction scope
            _interpreterBase.SetCollectionIds(db, members, collection);

            return OracleInsertBulk(db, insertText, collection, members, 1000);
#else
            throw new NotImplementedException("OracleInsertBulk not supported on OdpManagedDataProvider!");
#endif
        }

#if !MANAGED

        private int OracleInsertBulk<T>(
            DbManager db,
            string insertText,
            IEnumerable<T> collection,
            MemberMapper[] members,
            int maxBatchSize)
        {

            var idx = insertText.IndexOf('\n');
            var tbl = insertText.Substring(0, idx).Substring("INSERT INTO ".Length).TrimEnd('\r');
            var rd = new BulkCopyReader(members, collection);
            var bc = new OracleBulkCopy((OracleConnection)db.Connection)
            {
                BatchSize = maxBatchSize,
                DestinationTableName = tbl,
            };

            foreach (var memberMapper in members)
                bc.ColumnMappings.Add(new OracleBulkCopyColumnMapping(memberMapper.Ordinal, memberMapper.Name));

            bc.WriteToServer(rd);

            return rd.Count;
        }
#endif

        #endregion

        class BulkCopyReader : IDataReader
        {
            readonly MemberMapper[] _members;
            readonly IEnumerable _collection;
            readonly IEnumerator _enumerator;

            public int Count;

            public BulkCopyReader(MemberMapper[] members, IEnumerable collection)
            {
                _members = members;
                _collection = collection;
                _enumerator = _collection.GetEnumerator();
            }

            #region Implementation of IDisposable

            public void Dispose()
            {
            }

            #endregion

            #region Implementation of IDataRecord

            public string GetName(int i)
            {
                return _members[i].Name;
            }

            public Type GetFieldType(int i)
            {
                if (_members[i].Type.IsGenericType && _members[i].Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return Nullable.GetUnderlyingType(_members[i].Type);
                else
                    return _members[i].Type;
            }

            public object GetValue(int i)
            {
                var value = _members[i].GetValue(_enumerator.Current);

                return value ?? DBNull.Value;
            }

            public int FieldCount
            {
                get { return _members.Length; }
            }

            public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
            {
                throw new NotImplementedException();
            }

            public string GetDataTypeName(int i) { throw new NotImplementedException(); }
            public int GetValues(object[] values) { throw new NotImplementedException(); }
            public int GetOrdinal(string name) { throw new NotImplementedException(); }
            public bool GetBoolean(int i) { throw new NotImplementedException(); }
            public byte GetByte(int i) { throw new NotImplementedException(); }
            public char GetChar(int i) { throw new NotImplementedException(); }
            public Guid GetGuid(int i) { throw new NotImplementedException(); }
            public short GetInt16(int i) { throw new NotImplementedException(); }
            public int GetInt32(int i) { throw new NotImplementedException(); }
            public long GetInt64(int i) { throw new NotImplementedException(); }
            public float GetFloat(int i) { throw new NotImplementedException(); }
            public double GetDouble(int i) { throw new NotImplementedException(); }
            public string GetString(int i) { throw new NotImplementedException(); }
            public decimal GetDecimal(int i) { throw new NotImplementedException(); }
            public DateTime GetDateTime(int i) { throw new NotImplementedException(); }
            public IDataReader GetData(int i) { throw new NotImplementedException(); }
            public bool IsDBNull(int i) { throw new NotImplementedException(); }

            object IDataRecord.this[int i]
            {
                get { throw new NotImplementedException(); }
            }

            object IDataRecord.this[string name]
            {
                get { throw new NotImplementedException(); }
            }

            #endregion

            #region Implementation of IDataReader

            public void Close()
            {
                throw new NotImplementedException();
            }

            public DataTable GetSchemaTable()
            {
                throw new NotImplementedException();
            }

            public bool NextResult()
            {
                throw new NotImplementedException();
            }

            public bool Read()
            {
                var b = _enumerator.MoveNext();

                if (b)
                    Count++;

                return b;
            }

            public int Depth
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsClosed
            {
                get { throw new NotImplementedException(); }
            }

            public int RecordsAffected
            {
                get { throw new NotImplementedException(); }
            }

            #endregion
        }
    }

}
