using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

using BLToolkit.Common;
using BLToolkit.Mapping;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace BLToolkit.Data.DataProvider
{
	/// <summary>
	/// Implements access to the Data Provider for Oracle.
	/// </summary>
	/// <remarks>
	/// See the <see cref="DbManager.AddDataProvider(DataProviderBase)"/> method to find an example.
	/// </remarks>
	/// <seealso cref="DbManager.AddDataProvider(DataProviderBase)">AddDataManager Method</seealso>
	public class OdpDataProvider : DataProviderBase
	{
		public OdpDataProvider()
		{
			MappingSchema = new OdpMappingSchema();
		}

		static OdpDataProvider()
		{
			// Fix Oracle bug #1: Array types are not handled.
			//
			Type OraDb_DbTypeTableType = typeof(OracleParameter).Assembly.GetType("Oracle.DataAccess.Client.OraDb_DbTypeTable");

			if (null != OraDb_DbTypeTableType)
			{
				Hashtable typeTable = (Hashtable)OraDb_DbTypeTableType.InvokeMember(
					"s_table", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField,
					null, null, Type.EmptyTypes);

				if (null != typeTable)
				{
					typeTable.Add(typeof(DateTime[]),           OracleDbType.TimeStamp);
					typeTable.Add(typeof(Int16[]),              OracleDbType.Int16);
					typeTable.Add(typeof(Int32[]),              OracleDbType.Int32);
					typeTable.Add(typeof(Int64[]),              OracleDbType.Int64);
					typeTable.Add(typeof(Single[]),             OracleDbType.Single);
					typeTable.Add(typeof(Double[]),             OracleDbType.Double);
					typeTable.Add(typeof(Decimal[]),            OracleDbType.Decimal);
					typeTable.Add(typeof(String[]),             OracleDbType.Varchar2);
					typeTable.Add(typeof(TimeSpan[]),           OracleDbType.IntervalDS);
					typeTable.Add(typeof(OracleBFile[]),        OracleDbType.BFile);
					typeTable.Add(typeof(OracleBinary[]),       OracleDbType.Raw);
					typeTable.Add(typeof(OracleBlob[]),         OracleDbType.Blob);
					typeTable.Add(typeof(OracleClob[]),         OracleDbType.Clob);
					typeTable.Add(typeof(OracleDate[]),         OracleDbType.Date);
					typeTable.Add(typeof(OracleDecimal[]),      OracleDbType.Decimal);
					typeTable.Add(typeof(OracleIntervalDS[]),   OracleDbType.IntervalDS);
					typeTable.Add(typeof(OracleIntervalYM[]),   OracleDbType.IntervalYM);
					typeTable.Add(typeof(OracleRefCursor[]),    OracleDbType.RefCursor);
					typeTable.Add(typeof(OracleString[]),       OracleDbType.Varchar2);
					typeTable.Add(typeof(OracleTimeStamp[]),    OracleDbType.TimeStamp);
					typeTable.Add(typeof(OracleTimeStampLTZ[]), OracleDbType.TimeStampLTZ);
					typeTable.Add(typeof(OracleTimeStampTZ[]),  OracleDbType.TimeStampTZ);
					typeTable.Add(typeof(OracleXmlType[]),      OracleDbType.XmlType);

					typeTable.Add(typeof(Boolean),              OracleDbType.Byte);
					typeTable.Add(typeof(Guid),                 OracleDbType.Raw);
					typeTable.Add(typeof(SByte),                OracleDbType.Decimal);
					typeTable.Add(typeof(UInt16),               OracleDbType.Decimal);
					typeTable.Add(typeof(UInt32),               OracleDbType.Decimal);
					typeTable.Add(typeof(UInt64),               OracleDbType.Decimal);

					typeTable.Add(typeof(Boolean[]),            OracleDbType.Byte);
					typeTable.Add(typeof(Guid[]),               OracleDbType.Raw);
					typeTable.Add(typeof(SByte[]),              OracleDbType.Decimal);
					typeTable.Add(typeof(UInt16[]),             OracleDbType.Decimal);
					typeTable.Add(typeof(UInt32[]),             OracleDbType.Decimal);
					typeTable.Add(typeof(UInt64[]),             OracleDbType.Decimal);
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
			OracleConnection oraConnection = connection as OracleConnection;
			if (null != oraConnection)
			{
				OracleCommand oraCommand = oraConnection.CreateCommand();

				// Fix Oracle bug #2: Empty arrays can not be sent to the server.
				//
				oraCommand.BindByName = true;

				return oraCommand;
			}

			return base.CreateCommandObject(connection);
		}

		public override IDbDataParameter CloneParameter(IDbDataParameter parameter)
		{
			OracleParameter oraParameter = parameter as OracleParameter;

			if (null != oraParameter)
			{
				OracleParameter oraParameterClone = (OracleParameter)oraParameter.Clone();

				// Fix Oracle bug #3: CollectionType property is not cloned.
				//
				oraParameterClone.CollectionType = oraParameter.CollectionType;

				return oraParameterClone;
			}

			return base.CloneParameter(parameter);
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
		/// <param name="command">The IDbCommand referencing the stored procedure for which the parameter information is to be derived. The derived parameters will be populated into the Parameters of this command.</param>
		public override bool DeriveParameters(IDbCommand command)
		{
			OracleCommand oraCommand = command as OracleCommand;
			if (null != oraCommand)
			{
				OracleCommandBuilder.DeriveParameters(oraCommand);
				return true;
			}

			return false;
		}

		public override object Convert(object value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
					return String.Concat(":", (string)value);

				case ConvertType.NameToParameter:
					return String.Concat("p", (string)value);

				case ConvertType.ParameterToName:
					Debug.Assert(value is string, "OraDirectDataProvider.Convert: value is not a string???");
					Debug.Assert(Char.ToLower(((string)value).ToLower()[0]) == 'p', "OraDirectDataProvider.Convert: prefix 'p' not set?\n\n" + value.ToString());
					return ((string)value).Substring(1);
				
				default:
					return base.Convert(value, convertType);
			}
		}

		public override void AttachParameter(IDbCommand command, IDbDataParameter parameter)
		{
			OracleParameter oraParameter = parameter as OracleParameter;

			if (null != oraParameter)
			{
				if (oraParameter.CollectionType == OracleCollectionType.PLSQLAssociativeArray)
				{
					if (oraParameter.Direction == ParameterDirection.Input || oraParameter.Direction == ParameterDirection.InputOutput)
					{
						Array ar = oraParameter.Value as Array;
						if (null != ar && !(ar is byte[] || ar is char[]))
						{
							oraParameter.Size = ar.Length;
						}

						if (oraParameter.Size == 0)
						{
							// Skip this parameter.
							// Fix Oracle bug #2: Empty arrays can not be sent to the server.
							//
							return;
						}
					}
					else if (oraParameter.Direction == ParameterDirection.Output)
					{
						// Fix Oracle bug #4: ArrayBindSize must be explicitly specified.
						//
						if (oraParameter.DbType == DbType.String)
						{
							oraParameter.Size = 1024;
							int[] arrayBindSize = new int[oraParameter.Size];
							for (int i = 0; i < oraParameter.Size; ++i)
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
			}
			
			base.AttachParameter(command, parameter);
		}

		public override bool IsValueParameter(IDbDataParameter parameter)
		{
			OracleParameter oraParameter = parameter as OracleParameter;
			if (null != oraParameter)
			{
				if (oraParameter.OracleDbType == OracleDbType.RefCursor && oraParameter.Direction == ParameterDirection.Output)
				{
					// Ignore out ref cursors, while out parameters of other types are o.k.
					return false;
				}
			}

			return base.IsValueParameter(parameter);
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
			get { return "ODP"; }
		}

		public class OdpMappingSchema : MappingSchema
		{

#if FW2

			protected override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
			{
				return new OracleDataReaderMapper(this, dataReader);
			}

			protected override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader, NameOrIndexParameter nip)
			{
				return new SqlScalarDataReaderMapper(this, dataReader, nip);
			}

#endif

			#region Convert

			#region Primitive Types

			public override SByte ConvertToSByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultSByteNullValue : (SByte)oraDecimal.Value;
				}

				return base.ConvertToSByte(value);
			}

			public override Int16 ConvertToInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultInt16NullValue : oraDecimal.ToInt16();
				}

				return base.ConvertToInt16(value);
			}

			public override Int32 ConvertToInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultInt32NullValue : oraDecimal.ToInt32();
				}

				return base.ConvertToInt32(value);
			}

			public override Int64 ConvertToInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultInt64NullValue : oraDecimal.ToInt64();
				}

				return base.ConvertToInt64(value);
			}

			public override Byte ConvertToByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultByteNullValue : oraDecimal.ToByte();
				}

				return base.ConvertToByte(value);
			}

			public override UInt16 ConvertToUInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultUInt16NullValue : (UInt16)oraDecimal.Value;
				}

				return base.ConvertToUInt16(value);
			}

			public override UInt32 ConvertToUInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultUInt32NullValue : (UInt32)oraDecimal.Value;
				}

				return base.ConvertToUInt32(value);
			}

			public override UInt64 ConvertToUInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultUInt64NullValue : (UInt64)oraDecimal.Value;
				}

				return base.ConvertToUInt64(value);
			}

			public override Single ConvertToSingle(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultSingleNullValue : oraDecimal.ToSingle();
				}

				return base.ConvertToSingle(value);
			}

			public override Double ConvertToDouble(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultDoubleNullValue : oraDecimal.ToDouble();
				}

				return base.ConvertToDouble(value);
			}

			public override Boolean ConvertToBoolean(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultBooleanNullValue : (oraDecimal.Value != 0);
				}

				return base.ConvertToBoolean(value);
			}

			public override DateTime ConvertToDateTime(object value)
			{
				if (value is OracleDate)
				{
					OracleDate oraDate = (OracleDate)value;
					return oraDate.IsNull ? DefaultDateTimeNullValue : oraDate.Value;
				}

				return base.ConvertToDateTime(value);
			}

			public override Decimal ConvertToDecimal(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? DefaultDecimalNullValue : oraDecimal.Value;
				}

				return base.ConvertToDecimal(value);
			}

			public override Guid ConvertToGuid(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull ? DefaultGuidNullValue : new Guid(oraString.Value);
				}

				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull ? DefaultGuidNullValue : new Guid(oraBlob.Value);
				}

				return base.ConvertToGuid(value);
			}

			public override String ConvertToString(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull ? DefaultStringNullValue : oraString.Value;
				}

				if (value is OracleClob)
				{
					OracleClob oraClob = (OracleClob)value;
					return oraClob.IsNull ? DefaultStringNullValue : oraClob.Value;
				}

				return base.ConvertToString(value);
			}


			public override Stream ConvertToStream(object value)
			{
				if (value is OracleXmlType)
				{
					OracleXmlType oraXml = (OracleXmlType)value;
					return oraXml.IsNull ? DefaultStreamNullValue : oraXml.GetStream();
				}

				return base.ConvertToStream(value);
			}

			public override XmlReader ConvertToXmlReader(object value)
			{
				if (value is OracleXmlType)
				{
					OracleXmlType oraXml = (OracleXmlType)value;
					return oraXml.IsNull ? DefaultXmlReaderNullValue : oraXml.GetXmlReader();
				}

				return base.ConvertToXmlReader(value);
			}

			public override Byte[] ConvertToByteArray(object value)
			{
				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull ? null : oraBlob.Value;
				}

				if (value is OracleBinary)
				{
					OracleBinary oraBinary = (OracleBinary)value;
					return oraBinary.IsNull ? null : oraBinary.Value;
				}
				
				if (value is OracleBFile)
				{
					OracleBFile oraBFile = (OracleBFile)value;
					return oraBFile.IsNull ? null : oraBFile.Value;
				}

				return base.ConvertToByteArray(value);
			}

			public override Char[] ConvertToCharArray(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull ? null : oraString.Value.ToCharArray();
				}

				if (value is OracleClob)
				{
					OracleClob oraClob = (OracleClob)value;
					return oraClob.IsNull ? null : oraClob.Value.ToCharArray();
				}

				return base.ConvertToCharArray(value);
			}

			#endregion

#if FW2

			#region Nullable Types

			public override SByte? ConvertToNullableSByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (SByte?)oraDecimal.Value;
				}

				return base.ConvertToNullableSByte(value);
			}

			public override Int16? ConvertToNullableInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Int16?)oraDecimal.ToInt16();
				}

				return base.ConvertToNullableInt16(value);
			}

			public override Int32? ConvertToNullableInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Int32?)oraDecimal.ToInt32();
				}

				return base.ConvertToNullableInt32(value);
			}

			public override Int64? ConvertToNullableInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Int64?)oraDecimal.ToInt64();
				}

				return base.ConvertToNullableInt64(value);
			}

			public override Byte? ConvertToNullableByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Byte?)oraDecimal.ToByte();
				}

				return base.ConvertToNullableByte(value);
			}

			public override UInt16? ConvertToNullableUInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (UInt16?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt16(value);
			}

			public override UInt32? ConvertToNullableUInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (UInt32?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt32(value);
			}

			public override UInt64? ConvertToNullableUInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (UInt64?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt64(value);
			}

			public override Single? ConvertToNullableSingle(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Single?)oraDecimal.ToSingle();
				}

				return base.ConvertToNullableSingle(value);
			}

			public override Double? ConvertToNullableDouble(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Double?)oraDecimal.ToDouble();
				}

				return base.ConvertToNullableDouble(value);
			}

			public override Boolean? ConvertToNullableBoolean(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Boolean?)(oraDecimal.Value != 0);
				}

				return base.ConvertToNullableBoolean(value);
			}

			public override DateTime? ConvertToNullableDateTime(object value)
			{
				if (value is OracleDate)
				{
					OracleDate oraDate = (OracleDate)value;
					return oraDate.IsNull ? null : (DateTime?)oraDate.Value;
				}

				return base.ConvertToNullableDateTime(value);
			}

			public override Decimal? ConvertToNullableDecimal(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull ? null : (Decimal?)oraDecimal.Value;
				}

				return base.ConvertToNullableDecimal(value);
			}

			public override Guid? ConvertToNullableGuid(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull ? null : (Guid?)new Guid(oraString.Value);
				}

				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull ? null : (Guid?)new Guid(oraBlob.Value);
				}

				return base.ConvertToNullableGuid(value);
			}

			#endregion
#endif

			#endregion
		}

#if FW2

		public class OracleDataReaderMapper : DataReaderMapper
		{
			public OracleDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader)
				: base(mappingSchema, dataReader)
			{
				_dataReader = (OracleDataReader)dataReader;
			}

			private OracleDataReader _dataReader;

			public override Type GetFieldType(int index)
			{
				Type fieldType = _dataReader.GetProviderSpecificFieldType(index);

				if (fieldType == typeof(SqlXml))    return typeof(SqlXml);
				if (fieldType == typeof(SqlBinary)) return typeof(SqlBytes);

				return _dataReader.GetFieldType(index);
			}

			public override object GetValue(object o, int index)
			{
				Type fieldType = _dataReader.GetProviderSpecificFieldType(index);

				if (fieldType == typeof(OracleXmlType))
				{
					OracleXmlType xml = _dataReader.GetOracleXmlType(index);
					return xml.IsNull ? null : xml;
				}
				else if (fieldType == typeof(OracleBlob))
				{
					OracleBlob blob = _dataReader.GetOracleBlob(index);
					return blob.IsNull ? null : blob;
				}
				else
				{
					object value = _dataReader.GetValue(index);
					return value is DBNull ? null : value;
				}
			}
		}

		public class SqlScalarDataReaderMapper : ScalarDataReaderMapper
		{
			public SqlScalarDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader, NameOrIndexParameter nip)
				: base(mappingSchema, dataReader, nip)
			{
				_dataReader = (OracleDataReader)dataReader;
				_fieldType = _dataReader.GetProviderSpecificFieldType(Index);

				if (_fieldType == typeof(OracleXmlType))
					_fieldType = typeof(OracleXmlType);
				else if (_fieldType == typeof(OracleBlob))
					_fieldType = typeof(OracleBlob);
				else
					_fieldType = _dataReader.GetFieldType(Index);
			}

			private OracleDataReader _dataReader;
			private Type             _fieldType;

			public override Type GetFieldType(int index)
			{
				return _fieldType;
			}

			public override object GetValue(object o, int index)
			{
				if (_fieldType == typeof(OracleXmlType))
				{
					OracleXmlType xml = _dataReader.GetOracleXmlType(Index);
					return xml.IsNull ? null : xml;
				}
				else if (_fieldType == typeof(OracleBlob))
				{
					OracleBlob blob = _dataReader.GetOracleBlob(Index);
					return blob.IsNull ? null : blob;
				}
				else
				{
					object value = _dataReader.GetValue(index);
					return value is DBNull ? null : value;
				}
			}
		}

#endif

	}
}
