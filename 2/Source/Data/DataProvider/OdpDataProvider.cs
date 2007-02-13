// Odp.Net Data Provider.
// http://www.oracle.com/technology/tech/windows/odpnet/index.html
//
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;

using BLToolkit.Aspects;
using BLToolkit.Common;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

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
					typeTable[typeof(DateTime[])]          = OracleDbType.TimeStamp;
					typeTable[typeof(Int16[])]             = OracleDbType.Int16;
					typeTable[typeof(Int32[])]             = OracleDbType.Int32;
					typeTable[typeof(Int64[])]             = OracleDbType.Int64;
					typeTable[typeof(Single[])]            = OracleDbType.Single;
					typeTable[typeof(Double[])]            = OracleDbType.Double;
					typeTable[typeof(Decimal[])]           = OracleDbType.Decimal;
					typeTable[typeof(String[])]            = OracleDbType.Varchar2;
					typeTable[typeof(TimeSpan[])]          = OracleDbType.IntervalDS;
					typeTable[typeof(OracleBFile[])]       = OracleDbType.BFile;
					typeTable[typeof(OracleBinary[])]      = OracleDbType.Raw;
					typeTable[typeof(OracleBlob[])]        = OracleDbType.Blob;
					typeTable[typeof(OracleClob[])]        = OracleDbType.Clob;
					typeTable[typeof(OracleDate[])]        = OracleDbType.Date;
					typeTable[typeof(OracleDecimal[])]     = OracleDbType.Decimal;
					typeTable[typeof(OracleIntervalDS[])]  = OracleDbType.IntervalDS;
					typeTable[typeof(OracleIntervalYM[])]  = OracleDbType.IntervalYM;
					typeTable[typeof(OracleRefCursor[])]   = OracleDbType.RefCursor;
					typeTable[typeof(OracleString[])]      = OracleDbType.Varchar2;
					typeTable[typeof(OracleTimeStamp[])]   = OracleDbType.TimeStamp;
					typeTable[typeof(OracleTimeStampLTZ[])]= OracleDbType.TimeStampLTZ;
					typeTable[typeof(OracleTimeStampTZ[])] = OracleDbType.TimeStampTZ;
					typeTable[typeof(OracleXmlType[])]     = OracleDbType.XmlType;

					typeTable[typeof(Boolean)]             = OracleDbType.Byte;
					typeTable[typeof(Guid)]                = OracleDbType.Raw;
					typeTable[typeof(SByte)]               = OracleDbType.Decimal;
					typeTable[typeof(UInt16)]              = OracleDbType.Decimal;
					typeTable[typeof(UInt32)]              = OracleDbType.Decimal;
					typeTable[typeof(UInt64)]              = OracleDbType.Decimal;

					typeTable[typeof(Boolean[])]           = OracleDbType.Byte;
					typeTable[typeof(Guid[])]              = OracleDbType.Raw;
					typeTable[typeof(SByte[])]             = OracleDbType.Decimal;
					typeTable[typeof(UInt16[])]            = OracleDbType.Decimal;
					typeTable[typeof(UInt32[])]            = OracleDbType.Decimal;
					typeTable[typeof(UInt64[])]            = OracleDbType.Decimal;
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
			OracleParameter oraParameter = (parameter is OracleParameterWrap)?
				(parameter as OracleParameterWrap).OracleParameter: parameter as OracleParameter;

			if (null != oraParameter)
			{
				OracleParameter oraParameterClone = (OracleParameter)oraParameter.Clone();

				// Fix Oracle bug #3: CollectionType property is not cloned.
				//
				oraParameterClone.CollectionType = oraParameter.CollectionType;

				return OracleParameterWrap.CreateInstance(oraParameterClone);
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
			OracleParameter oraParameter = (parameter is OracleParameterWrap)?
				(parameter as OracleParameterWrap).OracleParameter: parameter as OracleParameter;

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

				parameter = oraParameter;
			}
			
			base.AttachParameter(command, parameter);
		}

		public override bool IsValueParameter(IDbDataParameter parameter)
		{
			OracleParameter oraParameter = (parameter is OracleParameterWrap)?
				(parameter as OracleParameterWrap).OracleParameter: parameter as OracleParameter;

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

		public override IDbDataParameter CreateParameterObject(IDbCommand command)
		{
			IDbDataParameter parameter = base.CreateParameterObject(command);

			if (parameter is OracleParameter)
				parameter = OracleParameterWrap.CreateInstance(parameter as OracleParameter);

			return parameter;
		}

		public override IDbDataParameter GetParameter(IDbCommand command, NameOrIndexParameter nameOrIndex)
		{
			IDbDataParameter parameter = base.GetParameter(command, nameOrIndex);

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
			get { return "ODP"; }
		}

		#region Inner types

		public class OdpMappingSchema : MappingSchema
		{
			public override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader)
			{
				return new OracleDataReaderMapper(this, dataReader);
			}

			public override DataReaderMapper CreateDataReaderMapper(IDataReader dataReader, NameOrIndexParameter nip)
			{
				return new OracleScalarDataReaderMapper(this, dataReader, nip);
			}

			#region Convert

			#region Primitive Types

			[CLSCompliant(false)]
			public override SByte ConvertToSByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultSByteNullValue: (SByte)oraDecimal.Value;
				}

				return base.ConvertToSByte(value);
			}

			public override Int16 ConvertToInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultInt16NullValue: oraDecimal.ToInt16();
				}

				return base.ConvertToInt16(value);
			}

			public override Int32 ConvertToInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultInt32NullValue: oraDecimal.ToInt32();
				}

				return base.ConvertToInt32(value);
			}

			public override Int64 ConvertToInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultInt64NullValue: oraDecimal.ToInt64();
				}

				return base.ConvertToInt64(value);
			}

			public override Byte ConvertToByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultByteNullValue: oraDecimal.ToByte();
				}

				return base.ConvertToByte(value);
			}

			[CLSCompliant(false)]
			public override UInt16 ConvertToUInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultUInt16NullValue: (UInt16)oraDecimal.Value;
				}

				return base.ConvertToUInt16(value);
			}

			[CLSCompliant(false)]
			public override UInt32 ConvertToUInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultUInt32NullValue: (UInt32)oraDecimal.Value;
				}

				return base.ConvertToUInt32(value);
			}

			[CLSCompliant(false)]
			public override UInt64 ConvertToUInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultUInt64NullValue: (UInt64)oraDecimal.Value;
				}

				return base.ConvertToUInt64(value);
			}

			public override Single ConvertToSingle(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultSingleNullValue: oraDecimal.ToSingle();
				}

				return base.ConvertToSingle(value);
			}

			public override Double ConvertToDouble(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultDoubleNullValue: oraDecimal.ToDouble();
				}

				return base.ConvertToDouble(value);
			}

			public override Boolean ConvertToBoolean(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultBooleanNullValue: (oraDecimal.Value != 0);
				}

				return base.ConvertToBoolean(value);
			}

			public override DateTime ConvertToDateTime(object value)
			{
				if (value is OracleDate)
				{
					OracleDate oraDate = (OracleDate)value;
					return oraDate.IsNull? DefaultDateTimeNullValue: oraDate.Value;
				}

				return base.ConvertToDateTime(value);
			}

			public override Decimal ConvertToDecimal(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? DefaultDecimalNullValue: oraDecimal.Value;
				}

				return base.ConvertToDecimal(value);
			}

			public override Guid ConvertToGuid(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull? DefaultGuidNullValue: new Guid(oraString.Value);
				}

				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull? DefaultGuidNullValue: new Guid(oraBlob.Value);
				}

				return base.ConvertToGuid(value);
			}

			public override String ConvertToString(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull? DefaultStringNullValue: oraString.Value;
				}

				if (value is OracleXmlType)
				{
					OracleXmlType oraXmlType = (OracleXmlType)value;
					return oraXmlType.IsNull ? DefaultStringNullValue : oraXmlType.Value;
				}

				if (value is OracleClob)
				{
					OracleClob oraClob = (OracleClob)value;
					return oraClob.IsNull? DefaultStringNullValue: oraClob.Value;
				}

				return base.ConvertToString(value);
			}


			public override Stream ConvertToStream(object value)
			{
				if (value is OracleXmlType)
				{
					OracleXmlType oraXml = (OracleXmlType)value;
					return oraXml.IsNull? DefaultStreamNullValue: oraXml.GetStream();
				}

				return base.ConvertToStream(value);
			}

			public override XmlReader ConvertToXmlReader(object value)
			{
				if (value is OracleXmlType)
				{
					OracleXmlType oraXml = (OracleXmlType)value;
					return oraXml.IsNull? DefaultXmlReaderNullValue: oraXml.GetXmlReader();
				}

				return base.ConvertToXmlReader(value);
			}

			public override Byte[] ConvertToByteArray(object value)
			{
				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull? null: oraBlob.Value;
				}

				if (value is OracleBinary)
				{
					OracleBinary oraBinary = (OracleBinary)value;
					return oraBinary.IsNull? null: oraBinary.Value;
				}
				
				if (value is OracleBFile)
				{
					OracleBFile oraBFile = (OracleBFile)value;
					return oraBFile.IsNull? null: oraBFile.Value;
				}

				return base.ConvertToByteArray(value);
			}

			public override Char[] ConvertToCharArray(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull? null: oraString.Value.ToCharArray();
				}

				if (value is OracleClob)
				{
					OracleClob oraClob = (OracleClob)value;
					return oraClob.IsNull? null: oraClob.Value.ToCharArray();
				}

				return base.ConvertToCharArray(value);
			}

			#endregion

#if FW2
			#region Nullable Types

			[CLSCompliant(false)]
			public override SByte? ConvertToNullableSByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (SByte?)oraDecimal.Value;
				}

				return base.ConvertToNullableSByte(value);
			}

			public override Int16? ConvertToNullableInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Int16?)oraDecimal.ToInt16();
				}

				return base.ConvertToNullableInt16(value);
			}

			public override Int32? ConvertToNullableInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Int32?)oraDecimal.ToInt32();
				}

				return base.ConvertToNullableInt32(value);
			}

			public override Int64? ConvertToNullableInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Int64?)oraDecimal.ToInt64();
				}

				return base.ConvertToNullableInt64(value);
			}

			public override Byte? ConvertToNullableByte(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Byte?)oraDecimal.ToByte();
				}

				return base.ConvertToNullableByte(value);
			}

			[CLSCompliant(false)]
			public override UInt16? ConvertToNullableUInt16(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (UInt16?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt16(value);
			}

			[CLSCompliant(false)]
			public override UInt32? ConvertToNullableUInt32(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (UInt32?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt32(value);
			}

			[CLSCompliant(false)]
			public override UInt64? ConvertToNullableUInt64(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (UInt64?)oraDecimal.Value;
				}

				return base.ConvertToNullableUInt64(value);
			}

			public override Single? ConvertToNullableSingle(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Single?)oraDecimal.ToSingle();
				}

				return base.ConvertToNullableSingle(value);
			}

			public override Double? ConvertToNullableDouble(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Double?)oraDecimal.ToDouble();
				}

				return base.ConvertToNullableDouble(value);
			}

			public override Boolean? ConvertToNullableBoolean(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Boolean?)(oraDecimal.Value != 0);
				}

				return base.ConvertToNullableBoolean(value);
			}

			public override DateTime? ConvertToNullableDateTime(object value)
			{
				if (value is OracleDate)
				{
					OracleDate oraDate = (OracleDate)value;
					return oraDate.IsNull? null: (DateTime?)oraDate.Value;
				}

				return base.ConvertToNullableDateTime(value);
			}

			public override Decimal? ConvertToNullableDecimal(object value)
			{
				if (value is OracleDecimal)
				{
					OracleDecimal oraDecimal = (OracleDecimal)value;
					return oraDecimal.IsNull? null: (Decimal?)oraDecimal.Value;
				}

				return base.ConvertToNullableDecimal(value);
			}

			public override Guid? ConvertToNullableGuid(object value)
			{
				if (value is OracleString)
				{
					OracleString oraString = (OracleString)value;
					return oraString.IsNull? null: (Guid?)new Guid(oraString.Value);
				}

				if (value is OracleBlob)
				{
					OracleBlob oraBlob = (OracleBlob)value;
					return oraBlob.IsNull? null: (Guid?)new Guid(oraBlob.Value);
				}

				return base.ConvertToNullableGuid(value);
			}

			#endregion
#endif

			#endregion
		}

		public class OracleDataReaderMapper : DataReaderMapper
		{
			public OracleDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader)
				: base(mappingSchema, dataReader)
			{
				_dataReader = (OracleDataReader)dataReader;
			}

			private OracleDataReader _dataReader;

#if FW2
			public override Type GetFieldType(int index)
			{
				Type fieldType = _dataReader.GetProviderSpecificFieldType(index);

				if (fieldType == typeof(OracleXmlType)) return typeof(OracleXmlType);
				if (fieldType == typeof(OracleBlob))    return typeof(OracleBlob);

				return _dataReader.GetFieldType(index);
			}

			public override object GetValue(object o, int index)
			{
				Type fieldType = _dataReader.GetProviderSpecificFieldType(index);

				if (fieldType == typeof(OracleXmlType))
				{
					OracleXmlType xml = _dataReader.GetOracleXmlType(index);
					return xml.IsNull? null: xml;
				}
				else if (fieldType == typeof(OracleBlob))
				{
					OracleBlob blob = _dataReader.GetOracleBlob(index);
					return blob.IsNull? null: blob;
				}
				else
				{
					object value = _dataReader.GetValue(index);
					return value is DBNull? null: value;
				}
			}
#endif
			public override Boolean  GetBoolean(object o, int index) { return MappingSchema.ConvertToBoolean(GetValue(o, index)); }
			public override Char     GetChar   (object o, int index) { return MappingSchema.ConvertToChar   (GetValue(o, index)); }
			public override Guid     GetGuid   (object o, int index) { return MappingSchema.ConvertToGuid   (GetValue(o, index)); }

			[CLSCompliant(false)]
			public override SByte    GetSByte  (object o, int index) { return  (SByte)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt16   GetUInt16 (object o, int index) { return (UInt16)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt32   GetUInt32 (object o, int index) { return (UInt32)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt64   GetUInt64 (object o, int index) { return (UInt64)_dataReader.GetDecimal(index); }

			public override Decimal  GetDecimal(object o, int index) { return OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(index), 28).Value; }

#if FW2
			public override Boolean? GetNullableBoolean(object o, int index) { return MappingSchema.ConvertToNullableBoolean(GetValue(o, index)); }
			public override Char?    GetNullableChar   (object o, int index) { return MappingSchema.ConvertToNullableChar   (GetValue(o, index)); }
			public override Guid?    GetNullableGuid   (object o, int index) { return MappingSchema.ConvertToNullableGuid   (GetValue(o, index)); }

			[CLSCompliant(false)]
			public override SByte?   GetNullableSByte  (object o, int index) { return _dataReader.IsDBNull(index)? null:  (SByte?)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt16?  GetNullableUInt16 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt16?)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt32?  GetNullableUInt32 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt32?)_dataReader.GetDecimal(index); }
			[CLSCompliant(false)]
			public override UInt64?  GetNullableUInt64 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt64?)_dataReader.GetDecimal(index); }

			public override Decimal? GetNullableDecimal(object o, int index) { return _dataReader.IsDBNull(index)? (decimal?)null: OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(index), 28).Value; }
#endif
		}

		public class OracleScalarDataReaderMapper : ScalarDataReaderMapper
		{
			private OracleDataReader _dataReader;

			public OracleScalarDataReaderMapper(MappingSchema mappingSchema, IDataReader dataReader, NameOrIndexParameter nip)
				: base(mappingSchema, dataReader, nip)
			{
				_dataReader = (OracleDataReader)dataReader;
#if FW2
				_fieldType = _dataReader.GetProviderSpecificFieldType(Index);

				if (_fieldType == typeof(OracleXmlType))
					_fieldType = typeof(OracleXmlType);
				else if (_fieldType == typeof(OracleBlob))
					_fieldType = typeof(OracleBlob);
				else
					_fieldType = _dataReader.GetFieldType(Index);
#endif
			}

#if FW2
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
					return xml.IsNull? null: xml;
				}
				else if (_fieldType == typeof(OracleBlob))
				{
					OracleBlob blob = _dataReader.GetOracleBlob(Index);
					return blob.IsNull? null: blob;
				}
				else
				{
					object value = _dataReader.GetValue(index);
					return value is DBNull? null: value;
				}
			}
#endif
			public override Boolean  GetBoolean(object o, int index) { return MappingSchema.ConvertToBoolean(GetValue(o, Index)); }
			public override Char     GetChar   (object o, int index) { return MappingSchema.ConvertToChar   (GetValue(o, Index)); }
			public override Guid     GetGuid   (object o, int index) { return MappingSchema.ConvertToGuid   (GetValue(o, Index)); }

			[CLSCompliant(false)]
			public override SByte    GetSByte  (object o, int index) { return  (SByte)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt16   GetUInt16 (object o, int index) { return (UInt16)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt32   GetUInt32 (object o, int index) { return (UInt32)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt64   GetUInt64 (object o, int index) { return (UInt64)_dataReader.GetDecimal(Index); }

			public override Decimal  GetDecimal(object o, int index) { return OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(Index), 28).Value; }

#if FW2
			public override Boolean? GetNullableBoolean(object o, int index) { return MappingSchema.ConvertToNullableBoolean(GetValue(o, Index)); }
			public override Char?    GetNullableChar   (object o, int index) { return MappingSchema.ConvertToNullableChar   (GetValue(o, Index)); }
			public override Guid?    GetNullableGuid   (object o, int index) { return MappingSchema.ConvertToNullableGuid   (GetValue(o, Index)); }

			[CLSCompliant(false)]
			public override SByte?   GetNullableSByte  (object o, int index) { return _dataReader.IsDBNull(index)? null:  (SByte?)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt16?  GetNullableUInt16 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt16?)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt32?  GetNullableUInt32 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt32?)_dataReader.GetDecimal(Index); }
			[CLSCompliant(false)]
			public override UInt64?  GetNullableUInt64 (object o, int index) { return _dataReader.IsDBNull(index)? null: (UInt64?)_dataReader.GetDecimal(Index); }

			public override Decimal? GetNullableDecimal(object o, int index) { return _dataReader.IsDBNull(index)? (decimal?)null: OracleDecimal.SetPrecision(_dataReader.GetOracleDecimal(Index), 28).Value; }
#endif
		}

		[Mixin(typeof(IDbDataParameter), "_oracleParameter")]
		[Mixin(typeof(IDataParameter),   "_oracleParameter")]
		[Mixin(typeof(IDisposable),      "_oracleParameter")]
		[Mixin(typeof(ICloneable),       "_oracleParameter")]
		[CLSCompliant(false)]
		public abstract class OracleParameterWrap
		{
			protected OracleParameter _oracleParameter;
			public    OracleParameter  OracleParameter
			{
				get { return _oracleParameter; }
			}

			public static IDbDataParameter CreateInstance(OracleParameter oraParameter)
			{
				OracleParameterWrap wrap = (OracleParameterWrap)TypeAccessor.CreateInstance(typeof (OracleParameterWrap));

				wrap._oracleParameter = oraParameter;

				return (IDbDataParameter)wrap;
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
				[MixinOverride]
				set
				{
					if (null != value)
					{
						if (value is Guid)
						{
							// Fix Oracle bug #6: guid type is not handled
							//
							value = ((Guid)value).ToByteArray();
						}
						else if (value is IConvertible)
						{
							IConvertible convertible = (IConvertible)value;
							TypeCode        typeCode = convertible.GetTypeCode();

							switch (typeCode)
							{
								case TypeCode.Boolean:
									// Fix Oracle bug #7: bool type is handled wrong
									//
									value = convertible.ToByte(null);
									break;

								case TypeCode.SByte:
								case TypeCode.UInt16:
								case TypeCode.UInt32:
								case TypeCode.UInt64:
									// Fix Oracle bug #8: some integer types are handled wrong
									//
									value = convertible.ToDecimal(null);
									break;

								default:
									// Fix Oracle bug #5: Enum type is not handled
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

					_oracleParameter.Value = value;
				}
			}
		}

		#endregion
	}
}
