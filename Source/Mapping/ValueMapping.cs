using System;
using System.Data.SqlTypes;

#if FW2
using KeyValue = System.Collections.Generic.KeyValuePair<System.Type, System.Type>;
using Table    = System.Collections.Generic.Dictionary<System.Collections.Generic.KeyValuePair<System.Type, System.Type>, BLToolkit.Mapping.IValueMapper>;
#else
using KeyValue = BLToolkit.Common.CompoundValue;
using Table    = System.Collections.Hashtable;
#endif

using BLToolkit.Common;

namespace BLToolkit.Mapping
{
	public
#if FW2
		static partial
#endif
		class ValueMapping
	{
		#region Init

		private static Table _mappers = new Table();

		private static void AddSameType(Type type, IValueMapper mapper)
		{
			_mappers.Add(new KeyValue(type, type), mapper);
		}

		static ValueMapping()
		{

			// Scalar Types.
			//
			AddSameType(typeof(SByte),       new I8ToI8());
			AddSameType(typeof(Int16),       new I16ToI16());
			AddSameType(typeof(Int32),       new I32ToI32());
			AddSameType(typeof(Int64),       new I64ToI64());

			AddSameType(typeof(Byte),        new U8ToU8());
			AddSameType(typeof(UInt16),      new U16ToU16());
			AddSameType(typeof(UInt32),      new U32ToU32());
			AddSameType(typeof(UInt64),      new U64ToU64());

			AddSameType(typeof(Single),      new R4ToR4());
			AddSameType(typeof(Double),      new R8ToR8());

			AddSameType(typeof(Boolean),     new BToB());
			AddSameType(typeof(Decimal),     new DToD());

			AddSameType(typeof(Char),        new CToC());
			AddSameType(typeof(Guid),        new GToG());
			AddSameType(typeof(DateTime),    new DTToDT());

#if FW2
			// Nullable Types.
			//
			AddSameType(typeof(SByte?),      new NI8ToNI8());
			AddSameType(typeof(Int16?),      new NI16ToNI16());
			AddSameType(typeof(Int32?),      new NI32ToNI32());
			AddSameType(typeof(Int64?),      new NI64ToNI64());

			AddSameType(typeof(Byte?),       new NU8ToNU8());
			AddSameType(typeof(UInt16?),     new NU16ToNU16());
			AddSameType(typeof(UInt32?),     new NU32ToNU32());
			AddSameType(typeof(UInt64?),     new NU64ToNU64());

			AddSameType(typeof(Single?),     new NR4ToNR4());
			AddSameType(typeof(Double?),     new NR8ToNR8());

			AddSameType(typeof(Boolean?),    new NBToNB());
			AddSameType(typeof(Decimal?),    new NDToND());

			AddSameType(typeof(Char?),       new NCToNC());
			AddSameType(typeof(Guid?),       new NGToNG());
			AddSameType(typeof(DateTime?),   new NDTToNDT());
#endif

			// SqlTypes.
			//
			AddSameType(typeof(SqlString),   new dbSTodbS());

			AddSameType(typeof(SqlByte),     new dbU8TodbU8());
			AddSameType(typeof(SqlInt16),    new dbI16TodbI16());
			AddSameType(typeof(SqlInt32),    new dbI32TodbI32());
			AddSameType(typeof(SqlInt64),    new dbI64TodbI64());

			AddSameType(typeof(SqlSingle),   new dbR4TodbR4());
			AddSameType(typeof(SqlDouble),   new dbR8TodbR8());
			AddSameType(typeof(SqlDecimal),  new dbDTodbD());
			AddSameType(typeof(SqlMoney),    new dbMTodbM());

			AddSameType(typeof(SqlBoolean),  new dbBTodbB());
			AddSameType(typeof(SqlGuid),     new dbGTodbG());
			AddSameType(typeof(SqlDateTime), new dbDTTodbDT());


			// Different Types.
			//
			_mappers.Add(new KeyValue(typeof(Int32),      typeof(Double)),  new Int32ToDouble());
			_mappers.Add(new KeyValue(typeof(Int32),      typeof(Decimal)), new Int32ToDecimal());
			_mappers.Add(new KeyValue(typeof(Double),     typeof(Int32)),   new DoubleToInt32());
			_mappers.Add(new KeyValue(typeof(Decimal),    typeof(Int32)),   new DecimalToInt32());

			_mappers.Add(new KeyValue(typeof(SqlInt32),   typeof(Int32)),   new SqlInt32ToInt32());
			_mappers.Add(new KeyValue(typeof(SqlDecimal), typeof(Decimal)), new SqlDecimalToDecimal());

			_mappers.Add(new KeyValue(typeof(SqlString),  typeof(Int32)),   new SqlStringToInt32());
			_mappers.Add(new KeyValue(typeof(SqlString),  typeof(Boolean)), new SqlStringToBoolean());
		}

		#endregion

		#region Default Mapper

		class DefaultValueMapper : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				dest.SetValue(destObject, destIndex, source.GetValue(sourceObject, sourceIndex));

				//object o = source.GetValue(sourceObject, sourceIndex);

				//if (o == null) dest.SetNull (destObject, destIndex);
				//else           dest.SetValue(destObject, destIndex, o);
			}
		}

		private static IValueMapper _defaultMapper = new DefaultValueMapper();
		[CLSCompliant(false)]
		public  static IValueMapper  DefaultMapper
		{
			get { return _defaultMapper;  }
			set { _defaultMapper = value; }
		}

		#endregion

		#region GetMapper

		private static object _sync = new object();

		[CLSCompliant(false)]
		public static IValueMapper GetMapper(Type t1, Type t2)
		{
			lock (_sync)
			{
				if (t1 == null) t1 = typeof(object);
				if (t2 == null) t2 = typeof(object);

				if (t1.IsEnum) t1 = Enum.GetUnderlyingType(t1);
				if (t2.IsEnum) t2 = Enum.GetUnderlyingType(t2);

				KeyValue key = new KeyValue(t1, t2);

				IValueMapper t;

#if FW2
				if (_mappers.TryGetValue(key, out t))
					return t;

				Type type = typeof(GetSetDataChecker<,>).MakeGenericType(t1, t2);

				if (((IGetSetDataChecker)Activator.CreateInstance(type)).Check() == false)
				{
					t = _defaultMapper;
				}
				else
				{
					type = t1 == t2?
						typeof(ValueMapper<>). MakeGenericType(t1):
						typeof(ValueMapper<,>).MakeGenericType(t1, t2);

					t = (IValueMapper)Activator.CreateInstance(type);
				}
#else
				t = (IValueMapper)_mappers[key];

				if (t != null)
					return t;

				t = _defaultMapper;
#endif
				_mappers.Add(key, t);

				return t;
			}
		}

		#endregion

		#region Same Types

		#region Scalar Types

		sealed class I8ToI8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSByte(destObject, destIndex,
						source.GetSByte(sourceObject, sourceIndex));
			}
		}

		sealed class I16ToI16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt16(destObject, destIndex,
						source.GetInt16(sourceObject, sourceIndex));
			}
		}

		sealed class I32ToI32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex,
						source.GetInt32(sourceObject, sourceIndex));
			}
		}

		sealed class I64ToI64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt64(destObject, destIndex,
						source.GetInt64(sourceObject, sourceIndex));
			}
		}


		sealed class U8ToU8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetByte(destObject, destIndex,
						source.GetByte(sourceObject, sourceIndex));
			}
		}

		sealed class U16ToU16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt16(destObject, destIndex,
						source.GetUInt16(sourceObject, sourceIndex));
			}
		}

		sealed class U32ToU32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt32(destObject, destIndex,
						source.GetUInt32(sourceObject, sourceIndex));
			}
		}

		sealed class U64ToU64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt64(destObject, destIndex,
						source.GetUInt64(sourceObject, sourceIndex));
			}
		}


		sealed class R4ToR4 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSingle(destObject, destIndex,
						source.GetSingle(sourceObject, sourceIndex));
			}
		}

		sealed class R8ToR8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDouble(destObject, destIndex,
						source.GetDouble(sourceObject, sourceIndex));
			}
		}


		sealed class BToB : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetBoolean(destObject, destIndex,
						source.GetBoolean(sourceObject, sourceIndex));
			}
		}

		sealed class DToD : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDecimal(destObject, destIndex,
						source.GetDecimal(sourceObject, sourceIndex));
			}
		}


		sealed class CToC : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetChar(destObject, destIndex, source.GetChar(sourceObject, sourceIndex));
			}
		}

		sealed class GToG : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetGuid(destObject, destIndex,
						source.GetGuid(sourceObject, sourceIndex));
			}
		}

		sealed class DTToDT : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDateTime(destObject, destIndex,
						source.GetDateTime(sourceObject, sourceIndex));
			}
		}

		#endregion

		#region SqlTypes

		sealed class dbSTodbS : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlString(destObject, destIndex,
						source.GetSqlString(sourceObject, sourceIndex));
			}
		}


		sealed class dbU8TodbU8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlByte(destObject, destIndex,
						source.GetSqlByte(sourceObject, sourceIndex));
			}
		}

		sealed class dbI16TodbI16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt16(destObject, destIndex,
						source.GetSqlInt16(sourceObject, sourceIndex));
			}
		}

		sealed class dbI32TodbI32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt32(destObject, destIndex,
						source.GetSqlInt32(sourceObject, sourceIndex));
			}
		}

		sealed class dbI64TodbI64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt64(destObject, destIndex,
						source.GetSqlInt64(sourceObject, sourceIndex));
			}
		}


		sealed class dbR4TodbR4 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlSingle(destObject, destIndex,
						source.GetSqlSingle(sourceObject, sourceIndex));
			}
		}

		sealed class dbR8TodbR8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDouble(destObject, destIndex,
						source.GetSqlDouble(sourceObject, sourceIndex));
			}
		}

		sealed class dbDTodbD : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDecimal(destObject, destIndex,
						source.GetSqlDecimal(sourceObject, sourceIndex));
			}
		}

		sealed class dbMTodbM : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlMoney(destObject, destIndex,
						source.GetSqlMoney(sourceObject, sourceIndex));
			}
		}


		sealed class dbBTodbB : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlBoolean(destObject, destIndex,
						source.GetSqlBoolean(sourceObject, sourceIndex));
			}
		}

		sealed class dbGTodbG : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlGuid(destObject, destIndex,
						source.GetSqlGuid(sourceObject, sourceIndex));
			}
		}

		sealed class dbDTTodbDT : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDateTime(destObject, destIndex,
						source.GetSqlDateTime(sourceObject, sourceIndex));
			}
		}

		#endregion

		#endregion

		#region Different Types

		class Int32ToDouble : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDouble(destObject, destIndex,
						Convert.ToDouble(
							source.GetInt32(sourceObject, sourceIndex)));
			}
		}

		class Int32ToDecimal : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDecimal(destObject, destIndex,
						Convert.ToDecimal(
							source.GetInt32(sourceObject, sourceIndex)));
			}
		}

		class DoubleToInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex,
						Convert.ToInt32(
							source.GetDouble(sourceObject, sourceIndex)));
			}
		}

		class DecimalToInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex,
						Convert.ToInt32(
							source.GetDecimal(sourceObject, sourceIndex)));
			}
		}

		class SqlInt32ToInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				SqlInt32 value = source.GetSqlInt32(sourceObject, sourceIndex);

				if (value.IsNull) dest.SetNull (destObject, destIndex);
				else              dest.SetInt32(destObject, destIndex, value.Value);
			}
		}

		class SqlDecimalToDecimal : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				SqlDecimal value = source.GetSqlDecimal(sourceObject, sourceIndex);

				if (value.IsNull) dest.SetNull   (destObject, destIndex);
				else              dest.SetDecimal(destObject, destIndex, value.Value);
			}
		}

		class SqlStringToInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				SqlString value = source.GetSqlString(sourceObject, sourceIndex);

				if (value.IsNull) dest.SetNull(destObject, destIndex);
				else dest.SetInt32(destObject, destIndex, value.ToSqlInt32().Value);
			}
		}

		class SqlStringToBoolean : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				SqlString value = source.GetSqlString(sourceObject, sourceIndex);

				if (value.IsNull) dest.SetNull(destObject, destIndex);
				else dest.SetBoolean(destObject, destIndex, value.ToSqlBoolean().Value);
			}
		}

		#endregion

#if FW2

		#region Nullable Types

		sealed class NI8ToNI8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableSByte(destObject, destIndex,
						source.GetNullableSByte(sourceObject, sourceIndex));
			}
		}

		sealed class NI16ToNI16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt16(destObject, destIndex,
						source.GetNullableInt16(sourceObject, sourceIndex));
			}
		}

		sealed class NI32ToNI32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt32(destObject, destIndex,
						source.GetNullableInt32(sourceObject, sourceIndex));
			}
		}

		sealed class NI64ToNI64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt64(destObject, destIndex,
						source.GetNullableInt64(sourceObject, sourceIndex));
			}
		}


		sealed class NU8ToNU8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableByte(destObject, destIndex,
						source.GetNullableByte(sourceObject, sourceIndex));
			}
		}

		sealed class NU16ToNU16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt16(destObject, destIndex,
						source.GetNullableUInt16(sourceObject, sourceIndex));
			}
		}

		sealed class NU32ToNU32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt32(destObject, destIndex,
						source.GetNullableUInt32(sourceObject, sourceIndex));
			}
		}

		sealed class NU64ToNU64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt64(destObject, destIndex,
						source.GetNullableUInt64(sourceObject, sourceIndex));
			}
		}


		sealed class NR4ToNR4 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableSingle(destObject, destIndex,
						source.GetNullableSingle(sourceObject, sourceIndex));
			}
		}

		sealed class NR8ToNR8 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDouble(destObject, destIndex,
						source.GetNullableDouble(sourceObject, sourceIndex));
			}
		}


		sealed class NBToNB : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableBoolean(destObject, destIndex,
						source.GetNullableBoolean(sourceObject, sourceIndex));
			}
		}

		sealed class NDToND : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDecimal(destObject, destIndex,
						source.GetNullableDecimal(sourceObject, sourceIndex));
			}
		}


		sealed class NCToNC : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableChar(destObject, destIndex,
						source.GetNullableChar(sourceObject, sourceIndex));
			}
		}

		sealed class NGToNG : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableGuid(destObject, destIndex,
						source.GetNullableGuid(sourceObject, sourceIndex));
			}
		}

		sealed class NDTToNDT : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDateTime(destObject, destIndex,
						source.GetNullableDateTime(sourceObject, sourceIndex));
			}
		}

		#endregion

		#region Generic Mappers

		interface IGetSetDataChecker
		{
			bool Check();
		}

		class GetSetDataChecker<S,D> : IGetSetDataChecker
		{
			public bool Check()
			{
				return
					GetData<S>.I != null && SetData<S>.I != null &&
					GetData<D>.I != null && SetData<D>.I != null;
			}
		}

		class ValueMapper<T> : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
				{
					SetData<T>.MB<T> setter = SetData<T>.I;
					GetData<T>.MB<T> getter = GetData<T>.I;

					setter.Set(dest, destObject, destIndex,
						getter.Get(source, sourceObject, sourceIndex));
				}
			}
		}

		class ValueMapper<S,D> : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
				{
					SetData<D>.MB<D>       setter    = SetData<D>.I;
					GetData<S>.MB<S>       getter    = GetData<S>.I;
					Convert<D, S>.CB<D, S> converter = Convert<D, S>.I;

					setter.Set(dest, destObject, destIndex,
						converter.C(
							getter.Get(source, sourceObject, sourceIndex)));
				}
			}
		}

		#endregion

#endif
	}
}
