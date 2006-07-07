using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using BLToolkit.Mapping;

namespace BLToolkit.Data.DataProvider
{
#if EXPERIMENTAL
	static class SqlDataReaderAdapter<T>
	{
		public static DataReader<T>.GetMethod Conv<T1>(DataReader<T1>.GetMethod m)
		{
			return (DataReader<T>.GetMethod)(object)m;
		}

		public static DataReader<T>.GetMethod Get = SelectMethod(typeof(T));
		public static DataReader<T>.GetMethod SelectMethod(Type type)
		{
			// Value types
			//
			if (type == typeof(SqlBinary))
					return Conv<SqlBinary>    (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlBinary(index);   });
			
			if (type == typeof(SqlBoolean))
					return Conv<SqlBoolean>   (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlBoolean(index);  });

			if (type == typeof(SqlByte))
					return Conv<SqlByte>      (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlByte(index);     });

			if (type == typeof(SqlDateTime))
					return Conv<SqlDateTime>  (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlDateTime(index); });

			if (type == typeof(SqlDecimal))
					return Conv<SqlDecimal>   (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlDecimal(index);  });

			if (type == typeof(SqlDouble))
					return Conv<SqlDouble>    (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlDouble(index);   });

			if (type == typeof(SqlGuid))
					return Conv<SqlGuid>      (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlGuid(index);     });

			if (type == typeof(SqlInt16))
					return Conv<SqlInt16>     (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlInt16(index);    });

			if (type == typeof(SqlInt32))
					return Conv<SqlInt32>     (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlInt32(index);    });

			if (type == typeof(SqlInt64))
					return Conv<SqlInt64>     (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlInt64(index);    });

			if (type == typeof(SqlMoney))
					return Conv<SqlMoney>     (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlMoney(index);    });

			if (type == typeof(SqlSingle))
					return Conv<SqlSingle>    (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlSingle(index);   });

			if (type == typeof(SqlString))
					return Conv<SqlString>    (delegate(IDataReader dr, int index) { return ((SqlDataReader)dr).GetSqlString(index);   });

			// Nullable types and classes
			//
			if (type == typeof(SqlBinary?))
					return Conv<SqlBinary?>   (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlBinary?)null   : ((SqlDataReader)dr).GetSqlBinary(index);   });

			if (type == typeof(SqlBoolean?))
					return Conv<SqlBoolean?>  (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlBoolean?)null  : ((SqlDataReader)dr).GetSqlBoolean(index);  });

			if (type == typeof(SqlByte?))
					return Conv<SqlByte?>     (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlByte?)null     : ((SqlDataReader)dr).GetSqlByte(index);     });

			if (type == typeof(SqlBytes))
					return Conv<SqlBytes>     (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlBytes)null     : ((SqlDataReader)dr).GetSqlBytes(index);    });

			if (type == typeof(SqlChars))
					return Conv<SqlChars>     (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlChars)null     : ((SqlDataReader)dr).GetSqlChars(index);    });

			if (type == typeof(SqlDateTime?))
					return Conv<SqlDateTime?> (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlDateTime?)null : ((SqlDataReader)dr).GetSqlDateTime(index); });

			if (type == typeof(SqlDecimal?))
					return Conv<SqlDecimal?>  (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlDecimal?)null  : ((SqlDataReader)dr).GetSqlDecimal(index);  });

			if (type == typeof(SqlDouble?))
					return Conv<SqlDouble?>   (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlDouble?)null   : ((SqlDataReader)dr).GetSqlDouble(index);   });

			if (type == typeof(SqlGuid?))
					return Conv<SqlGuid?>     (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlGuid?)null     : ((SqlDataReader)dr).GetSqlGuid(index);     });

			if (type == typeof(SqlInt16?))
					return Conv<SqlInt16?>    (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlInt16?)null    : ((SqlDataReader)dr).GetSqlInt16(index);    });

			if (type == typeof(SqlInt32?))
					return Conv<SqlInt32?>    (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlInt32?)null    : ((SqlDataReader)dr).GetSqlInt32(index);    });

			if (type == typeof(SqlInt64?))
					return Conv<SqlInt64?>    (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlInt64?)null    : ((SqlDataReader)dr).GetSqlInt64(index);    });

			if (type == typeof(SqlMoney?))
					return Conv<SqlMoney?>    (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlMoney?)null    : ((SqlDataReader)dr).GetSqlMoney(index);    });

			if (type == typeof(SqlSingle?))
					return Conv<SqlSingle?>   (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlSingle?)null   : ((SqlDataReader)dr).GetSqlSingle(index);   });

			if (type == typeof(SqlString?))
					return Conv<SqlString?>   (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlString?)null   : ((SqlDataReader)dr).GetSqlString(index);   });

			if (type == typeof(SqlXml))
					return Conv<SqlXml>       (delegate(IDataReader dr, int index)
						{ return dr.IsDBNull(index) ? (SqlXml)null       : ((SqlDataReader)dr).GetSqlXml(index);      });

			return DataReader<T>.DefaultMethod;
		}
	}
#endif

	/// <summary>
	/// Implements access to the Data Provider for SQL Server.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
	public sealed class SqlDataProvider: DataProviderBase
	{
		public SqlDataProvider()
		{
			//MappingSchema = new SqlMappingSchema();
		}

		/// <summary>
		/// Creates the database connection object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>The database connection object.</returns>
		public override IDbConnection CreateConnectionObject()
		{
			return new SqlConnection();
		}

		/// <summary>
		/// Creates the data adapter object.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <returns>A data adapter object.</returns>
		public override DbDataAdapter CreateDataAdapterObject()
		{
			return new SqlDataAdapter();
		}

		/// <summary>
		/// Populates the specified IDbCommand object's Parameters collection with 
		/// parameter information for the stored procedure specified in the IDbCommand.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <param name="command">The IDbCommand referencing the stored procedure for which the parameter information is to be derived. The derived parameters will be populated into the Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			SqlCommandBuilder.DeriveParameters((SqlCommand)command);
			return true;
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
			}

			return value;
		}

#if EXPERIMENTAL

		#region ReadXXXX

		private Char ReadChar(IDataReader dr, int index)
		{
			return dr.GetString(index)[0];
		}

		private Char? ReadNullableChar(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (Char?)null  : dr.GetString(index)[0];
		}

		private Byte[] ReadSqlBytesAsByteArray(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (Byte[])null : ((SqlDataReader)dr).GetSqlBytes(index).Value;
		}

		private Stream ReadSqlBytesAsStream(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (Stream)null : ((SqlDataReader)dr).GetSqlBytes(index).Stream;
		}

		private SqlGuid ReadSqlBinaryAsSqlGuid(IDataReader dr, int index)
		{
			return ((SqlDataReader)dr).GetSqlBinary(index).ToSqlGuid();
		}

		private SqlGuid? ReadSqlBinaryAsNullableSqlGuid(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (SqlGuid?)null : ((SqlDataReader)dr).GetSqlBinary(index).ToSqlGuid();
		}

		private XmlReader ReadSqlXmlAsXmlReader(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (XmlReader)null : ((SqlDataReader)dr).GetSqlXml(index).CreateReader();
		}

		private Char[] ReadSqlCharsAsCharArray(IDataReader dr, int index)
		{
			return dr.IsDBNull(index) ? (Char[])null : ((SqlDataReader)dr).GetSqlChars(index).Value;
		}

		#endregion

		public override DataReader<T>.GetMethod SelectDataReaderGetMethod<T>(IDataReader dr, int index)
		{
			// SqlDataReader does not implement Char data type.
			//
			if (typeof(T) == typeof(Char))
				return DataReader<T>.Conv<Char> (ReadChar);
			if (typeof(T) == typeof(Char?))
				return DataReader<T>.Conv<Char?>(ReadNullableChar);

			// Check int, string and other base types first.
			//
			if (DataReader<T>.Get != DataReader<T>.DefaultMethod)
				return DataReader<T>.Get;

			Type fieldType = ((SqlDataReader)dr).GetProviderSpecificFieldType(index);

			// Provider specific data types.
			//
			if ((fieldType == typeof(T) || fieldType == Nullable.GetUnderlyingType(typeof(T)))
					&& SqlDataReaderAdapter<T>.Get != DataReader<T>.DefaultMethod)
				return SqlDataReaderAdapter<T>.Get;

			// BLToolkit extensions.
			//
			if (fieldType == typeof(SqlBytes) || fieldType == typeof(SqlBinary))
			{
				// SqlBytes and SqlBinary represents the same SQL type.

				if (typeof(T) == typeof(Byte[]))    return DataReader<T>.Conv<Byte[]>    (ReadSqlBytesAsByteArray);
				if (typeof(T) == typeof(Stream))    return DataReader<T>.Conv<Stream>    (ReadSqlBytesAsStream);

				if (typeof(T) == typeof(SqlGuid))   return DataReader<T>.Conv<SqlGuid>   (ReadSqlBinaryAsSqlGuid);
				if (typeof(T) == typeof(SqlGuid?))  return DataReader<T>.Conv<SqlGuid?>  (ReadSqlBinaryAsNullableSqlGuid);

				if (typeof(T) == typeof(SqlBinary)) return DataReader<T>.Conv<SqlBinary> (SqlDataReaderAdapter<SqlBinary>.Get);
				if (typeof(T) == typeof(SqlBytes))  return DataReader<T>.Conv<SqlBytes>  (SqlDataReaderAdapter<SqlBytes>.Get);
			}
			else if (fieldType == typeof(SqlXml))
			{
				if (typeof(T) == typeof(XmlReader)) return DataReader<T>.Conv<XmlReader> (ReadSqlXmlAsXmlReader);
			}
			else if (fieldType == typeof(SqlString) || fieldType == typeof(SqlChars))
			{
				// SqlChars and SqlString represents the same SQL type.

				if (typeof(T) == typeof(Char[]))    return DataReader<T>.Conv<Char[]>    (ReadSqlCharsAsCharArray);

				if (typeof(T) == typeof(SqlChars))  return DataReader<T>.Conv<SqlChars>  (SqlDataReaderAdapter<SqlChars>.Get);
				if (typeof(T) == typeof(SqlString)) return DataReader<T>.Conv<SqlString> (SqlDataReaderAdapter<SqlString>.Get);
			}

			// Fall back to Convert<T>.From(IDataReader.GetValue(index));
			return SqlDataReaderAdapter<T>.Get;
		}
#endif

		/// <summary>
		/// Returns connection type.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataManager Method</seealso>
		/// <value>An instance of the <see cref="Type"/> class.</value>
		public override Type ConnectionType
		{
			get { return typeof(SqlConnection); }
		}

		/// <summary>
		/// Returns the data provider name.
		/// </summary>
		/// <remarks>
		/// See the <see cref="DbManager.AddDataProvider"/> method to find an example.
		/// </remarks>
		/// <seealso cref="DbManager.AddDataProvider">AddDataProvider Method</seealso>
		/// <value>Data provider name.</value>
		public override string Name
		{
			get { return "Sql"; }
		}

		public class SqlMappingSchema : MappingSchema
		{
			protected override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
			{
				return new SqlDataReaderMapper(this, dataReader);
			}
		}

		public class SqlDataReaderMapper : DataReaderMapper
		{
			public SqlDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader)
				: base(mappingSchema, dataReader)
			{
				_dataReader = (SqlDataReader)dataReader;
			}

			private SqlDataReader _dataReader;

#if FW2
			public override Type GetFieldType(int index)
			{
				return _dataReader.GetProviderSpecificFieldType(index);
			}

			public override object GetValue(object o, int index)
			{
				return _dataReader.GetProviderSpecificValue(index);
			}
#endif

			public override SqlBoolean  GetSqlBoolean (object o, int index) { return _dataReader.GetSqlBoolean (index); }
			public override SqlByte     GetSqlByte    (object o, int index) { return _dataReader.GetSqlByte    (index); }
			public override SqlDateTime GetSqlDateTime(object o, int index) { return _dataReader.GetSqlDateTime(index); }
			public override SqlDecimal  GetSqlDecimal (object o, int index) { return _dataReader.GetSqlDecimal (index); }
			public override SqlDouble   GetSqlDouble  (object o, int index) { return _dataReader.GetSqlDouble  (index); }
			public override SqlGuid     GetSqlGuid    (object o, int index) { return _dataReader.GetSqlGuid    (index); }
			public override SqlInt16    GetSqlInt16   (object o, int index) { return _dataReader.GetSqlInt16   (index); }
			public override SqlInt32    GetSqlInt32   (object o, int index) { return _dataReader.GetSqlInt32   (index); }
			public override SqlInt64    GetSqlInt64   (object o, int index) { return _dataReader.GetSqlInt64   (index); }
			public override SqlMoney    GetSqlMoney   (object o, int index) { return _dataReader.GetSqlMoney   (index); }
			public override SqlSingle   GetSqlSingle  (object o, int index) { return _dataReader.GetSqlSingle  (index); }
			public override SqlString   GetSqlString  (object o, int index) { return _dataReader.GetSqlString  (index); }
		}
	}
}
