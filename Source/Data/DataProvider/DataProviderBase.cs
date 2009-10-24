using System;
using System.Data;
using System.Data.Common;

using BLToolkit.Common;
using BLToolkit.Mapping;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	/// <summary>
	/// The <b>DataProviderBase</b> is a class that provides specific data provider information
	/// for the <see cref="DbManager"/> class. 
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public abstract class DataProviderBase
	{
		#region Abstract Properties

		/// <summary>
		/// Returns an actual type of the connection object used by this instance of the <see cref="DbManager"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public abstract Type ConnectionType { get; }

		/// <summary>
		/// Returns the data manager name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <value>The data manager name.</value>
		public abstract string Name { get; }

		private  string _uniqueName;
		/// <summary>
		/// Same as <see cref="Name"/>, but may be overridden to add two or more providers of same type.
		/// </summary>
		public string  UniqueName
		{
			get { return _uniqueName ?? Name; }
			internal set { _uniqueName = value; }
		}

		#endregion

		#region Abstract Methods

		/// <summary>
		/// Creates a new instance of the <see cref="IDbConnection"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <returns>The <see cref="IDbConnection"/> object.</returns>
		public abstract IDbConnection CreateConnectionObject();

		/// <summary>
		/// Creates a new connection object with same connection string.
		/// </summary>
		/// <param name="connection">A connection object used as prototype.</param>
		/// <returns>New connection instance.</returns>
		public virtual IDbConnection CloneConnection(IDbConnection connection)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");

			ICloneable cloneable = connection as ICloneable;

			if (cloneable != null)
				return (IDbConnection)cloneable.Clone();

			IDbConnection newConnection = CreateConnectionObject();

			// This is definitelly not enought when PersistSecurityInfo set to false.
			//
			newConnection.ConnectionString = connection.ConnectionString;

			return newConnection;
		}

		/// <summary>
		/// Creates an instance of the <see cref="DbDataAdapter"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <returns>The <see cref="DbDataAdapter"/> object.</returns>
		public abstract DbDataAdapter CreateDataAdapterObject();

		/// <summary>
		/// Populates the specified <see cref="IDbCommand"/> object's Parameters collection with 
		/// parameter information for the stored procedure specified in the <see cref="IDbCommand"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <param name="command">The <see cref="IDbCommand"/> referencing the stored procedure 
		/// for which the parameter information is to be derived.
		/// The derived parameters will be populated into the Parameters of this command.</param>
		/// <returns>true - parameters can be derive.</returns>
		public abstract bool DeriveParameters(IDbCommand command);

		#endregion

		#region Factory methods

		public virtual IDbCommand CreateCommandObject(IDbConnection connection)
		{
			return connection.CreateCommand();
		}

		public virtual IDbDataParameter CreateParameterObject(IDbCommand command)
		{
			return command.CreateParameter();
		}

		#endregion

		#region IDbDataParameter methods

		public virtual IDbDataParameter GetParameter(
			IDbCommand           command,
			NameOrIndexParameter nameOrIndex)
		{
			return (IDbDataParameter)(nameOrIndex.ByName?
				command.Parameters[nameOrIndex.Name]: command.Parameters[nameOrIndex.Index]);
		}

		public virtual void AttachParameter(
			IDbCommand       command,
			IDbDataParameter parameter)
		{
			command.Parameters.Add(parameter);
		}

		public virtual void SetUserDefinedType(IDbDataParameter parameter, string typeName)
		{
			throw new NotSupportedException(Name + " data provider does not support UDT.");
		}

		public virtual bool IsValueParameter(IDbDataParameter parameter)
		{
			return parameter.Direction != ParameterDirection.ReturnValue;
		}

		public virtual IDbDataParameter CloneParameter(IDbDataParameter parameter)
		{
			return (IDbDataParameter)((ICloneable)parameter).Clone();
		}

		#endregion

		#region Virtual Members

		public virtual object Convert(object value, ConvertType convertType)
		{
			return value;
		}

		public virtual void InitDbManager(DbManager dbManager)
		{
			MappingSchema schema = MappingSchema;

			if (schema != null)
				dbManager.MappingSchema = schema;
		}

		/// <summary>
		/// One time initialization from a configuration file.
		/// </summary>
		/// <param name="attributes">Provider specific attributes.</param>
		public virtual void Configure(System.Collections.Specialized.NameValueCollection attributes)
		{
		}

		private        MappingSchema _mappingSchema;
		public virtual MappingSchema  MappingSchema
		{
			get { return _mappingSchema;  }
			set { _mappingSchema = value; }
		}

		public virtual void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
		}

		public virtual ISqlProvider CreateSqlProvider()
		{
			return new BasicSqlProvider(this);
		}

		public virtual IDataReader GetDataReader(MappingSchema schema, IDataReader dataReader)
		{
			return dataReader;
		}

		public virtual string ProviderName           { get { return ConnectionType.Namespace; } }
		public virtual int    MaxParameters          { get { return 100;   } }
		public virtual int    MaxBatchSize           { get { return 65536; } }
		public virtual string EndOfSql               { get { return ";";   } }
		public virtual string DatabaseOwnerDelimiter { get { return ".";   } }
		public virtual string DatabaseTableDelimiter { get { return ".";   } }
		public virtual string OwnerTableDelimiter    { get { return ".";   } }

		#endregion

		protected abstract class DataReaderEx<T> : IDataReader, IDataReaderEx where T: IDataReader
		{
			protected readonly T _rd;

			protected DataReaderEx(T rd)
			{
				_rd = rd;
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				_rd.Dispose();
			}

			#endregion

			#region Implementation of IDataRecord

			public string GetName(int i) { return _rd.GetName(i); }
			public string GetDataTypeName(int i) { return _rd.GetDataTypeName(i); }
			public Type GetFieldType(int i) { return _rd.GetFieldType(i); }
			public object GetValue(int i) { return _rd.GetValue(i); }
			public int GetValues(object[] values) { return _rd.GetValues(values); }
			public int GetOrdinal(string name) { return _rd.GetOrdinal(name); }
			public bool GetBoolean(int i) { return _rd.GetBoolean(i); }
			public byte GetByte(int i) { return _rd.GetByte(i); }
			public char GetChar(int i) { return _rd.GetChar(i); }
			public Guid GetGuid(int i) { return _rd.GetGuid(i); }
			public short GetInt16(int i) { return _rd.GetInt16(i); }
			public int GetInt32(int i) { return _rd.GetInt32(i); }
			public long GetInt64(int i) { return _rd.GetInt64(i); }
			public float GetFloat(int i) { return _rd.GetFloat(i); }
			public double GetDouble(int i) { return _rd.GetDouble(i); }
			public string GetString(int i) { return _rd.GetString(i); }
			public decimal GetDecimal(int i) { return _rd.GetDecimal(i); }
			public DateTime GetDateTime(int i) { return _rd.GetDateTime(i); }
			public IDataReader GetData(int i) { return _rd.GetData(i); }
			public bool IsDBNull(int i) { return _rd.IsDBNull(i); }

			public int FieldCount { get { return _rd.FieldCount; } }

			object IDataRecord.this[int i] { get { return _rd[i]; } }
			object IDataRecord.this[string name] { get { return _rd[name]; } }

			public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
			{
				return _rd.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
			}

			public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
			{
				return _rd.GetChars(i, fieldoffset, buffer, bufferoffset, length);
			}

			#endregion

			#region Implementation of IDataReader

			public void Close() { _rd.Close(); }
			public DataTable GetSchemaTable() { return _rd.GetSchemaTable(); }
			public bool NextResult() { return _rd.NextResult(); }
			public bool Read() { return _rd.Read(); }
			public int Depth { get { return _rd.Depth; } }
			public bool IsClosed { get { return _rd.IsClosed; } }
			public int RecordsAffected { get { return _rd.RecordsAffected; } }

			#endregion

			#region Implementation of IDataReaderEx

			public abstract DateTimeOffset GetDateTimeOffset(int i);

			#endregion
		}
	}
}
