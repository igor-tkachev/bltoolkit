using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BLToolkit.Data.DataProvider
{
	using Sql.SqlProvider;

	/// <summary>
	/// Implements access to the Data Provider for SQL Server.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public sealed class SqlDataProvider : DataProviderBase
	{
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
			return new SqlConnection();
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
			return new SqlDataAdapter();
		}

		/// <summary>
		/// Populates the specified <see cref="IDbCommand"/> object's Parameters collection with 
		/// parameter information for the stored procedure specified in the <see cref="IDbCommand"/>.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
		/// <param name="command">The <see cref="IDbCommand"/> referencing the stored procedure for which the parameter
		/// information is to be derived. The derived parameters will be populated into the 
		/// Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			SqlCommandBuilder.DeriveParameters((SqlCommand)command);

			foreach (SqlParameter p in command.Parameters)
			{
				// We have to clear UDT type names.
				// Otherwise it will fail with error
				// "Database name is not allowed with a table-valued parameter"
				// but this is exactly the way how they are discovered.
				//
				if (p.SqlDbType == SqlDbType.Structured)
				{
					int lastDot = p.TypeName.LastIndexOf('.');
					if (lastDot >= 0)
						p.TypeName = p.TypeName.Substring(lastDot + 1);
				}
			}
			return true;
		}

		public override void PrepareCommand(ref CommandType commandType, ref string commandText, ref IDbDataParameter[] commandParameters)
		{
			if (commandParameters == null)
				return;

			foreach (IDbDataParameter p in commandParameters)
			{
				object val = p.Value;
				if (val == null || !val.GetType().IsArray || val is byte[] || val is char[])
					continue;

				DataTable dt = new DataTable();
				dt.Columns.Add("column_value", val.GetType().GetElementType());

				dt.BeginLoadData();
				foreach (object o in (Array)val)
				{
					DataRow row = dt.NewRow();
					row[0] = o;
					dt.Rows.Add(row);
				}
				dt.EndLoadData();

				p.Value = dt;
			}
		}

		public override void SetUserDefinedType(IDbDataParameter parameter, string typeName)
		{
			if (!(parameter is SqlParameter))
				throw new ArgumentException("SqlParameter expected.", "parameter");

			((SqlParameter)parameter).TypeName = typeName;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
				case ConvertType.NameToParameter:
					return "@" + value;

				case ConvertType.NameToQueryField:
					{
						string name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;
					}

					return "[" + value + "]";

				case ConvertType.NameToDatabase:
				case ConvertType.NameToOwner:
				case ConvertType.NameToQueryTable:
					{
						string name = value.ToString();

						if (name.Length > 0 && name[0] == '[')
							return value;

						if (name.IndexOf('.') > 0)
							value = string.Join("].[", name.Split('.'));
					}

					return "[" + value + "]";

				case ConvertType.ParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						return str.Length > 0 && str[0] == '@'? str.Substring(1): str;
					}
					break;

				case ConvertType.ExceptionToErrorNumber:
					if (value is SqlException)
						return ((SqlException)value).Number;
					break;
			}

			return value;
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
			get { return typeof(SqlConnection); }
		}

		public const string NameString = DataProvider.ProviderName.MsSql;

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

		public override ISqlProvider CreateSqlProvider()
		{
			return new MSSqlSqlProvider(this);
		}

		public override int MaxParameters
		{
			get { return 2100 - 20; }
		}

		public override int MaxBatchSize
		{
			get { return 65536; }
		}

		public override string DatabaseTableDelimiter
		{
			get { return ".."; }
		}

		#region GetDataReader

		public override IDataReader GetDataReader(Mapping.MappingSchema schema, IDataReader dataReader)
		{
			return new DataReaderEx((SqlDataReader)dataReader);
			//return base.GetDataReader(schema, dataReader);
		}

		class DataReaderEx : IDataReader, IDataReaderEx
		{
			readonly SqlDataReader _rd;

			public DataReaderEx(SqlDataReader rd)
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

			public string      GetName        (int i)           { return _rd.GetName        (i);      }
			public string      GetDataTypeName(int i)           { return _rd.GetDataTypeName(i);      }
			public Type        GetFieldType   (int i)           { return _rd.GetFieldType   (i);      }
			public object      GetValue       (int i)           { return _rd.GetValue       (i);      }
			public int         GetValues      (object[] values) { return _rd.GetValues      (values); }
			public int         GetOrdinal     (string name)     { return _rd.GetOrdinal     (name);   }
			public bool        GetBoolean     (int i)           { return _rd.GetBoolean     (i);      }
			public byte        GetByte        (int i)           { return _rd.GetByte        (i);      }
			public char        GetChar        (int i)           { return _rd.GetChar        (i);      }
			public Guid        GetGuid        (int i)           { return _rd.GetGuid        (i);      }
			public short       GetInt16       (int i)           { return _rd.GetInt16       (i);      }
			public int         GetInt32       (int i)           { return _rd.GetInt32       (i);      }
			public long        GetInt64       (int i)           { return _rd.GetInt64       (i);      }
			public float       GetFloat       (int i)           { return _rd.GetFloat       (i);      }
			public double      GetDouble      (int i)           { return _rd.GetDouble      (i);      }
			public string      GetString      (int i)           { return _rd.GetString      (i);      }
			public decimal     GetDecimal     (int i)           { return _rd.GetDecimal     (i);      }
			public DateTime    GetDateTime    (int i)           { return _rd.GetDateTime    (i);      }
			public IDataReader GetData        (int i)           { return _rd.GetData        (i);      }
			public bool        IsDBNull       (int i)           { return _rd.IsDBNull       (i);      }

			public int FieldCount                { get { return _rd.FieldCount; } }

			object IDataRecord.this[int i]       { get { return _rd[i];    } }
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

			public void      Close         ()      {        _rd.Close         (); }
			public DataTable GetSchemaTable()      { return _rd.GetSchemaTable(); }
			public bool      NextResult    ()      { return _rd.NextResult    (); }
			public bool      Read          ()      { return _rd.Read          (); }
			public int       Depth           { get { return _rd.Depth           ; } }
			public bool      IsClosed        { get { return _rd.IsClosed        ; } }
			public int       RecordsAffected { get { return _rd.RecordsAffected ; } }

			#endregion

			#region Implementation of IDataReaderEx

			public DateTimeOffset GetDateTimeOffset(int i)
			{
				return _rd.GetDateTimeOffset(i);
			}

			#endregion
		}

		#endregion
	}
}
