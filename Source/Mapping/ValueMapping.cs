using System;
using System.Data.SqlTypes;

#if FW2
using System.Collections.Generic;
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
		static
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
			AddSameType(typeof(SByte),    new SByteToSByte());
			AddSameType(typeof(Int16),    new Int16ToInt16());
			AddSameType(typeof(Int32),    new Int32ToInt32());
			AddSameType(typeof(Int64),    new Int64ToInt64());
			AddSameType(typeof(Byte),     new ByteToByte());
			AddSameType(typeof(UInt16),   new UInt16ToUInt16());
			AddSameType(typeof(UInt32),   new UInt32ToUInt32());
			AddSameType(typeof(UInt64),   new UInt64ToUInt64());
			AddSameType(typeof(Boolean),  new BooleanToBoolean());
			AddSameType(typeof(Char),     new CharToChar());
			AddSameType(typeof(Single),   new SingleToSingle());
			AddSameType(typeof(Double),   new DoubleToDouble());
			AddSameType(typeof(Decimal),  new DecimalToDecimal());
			AddSameType(typeof(Guid),     new GuidToGuid());
			AddSameType(typeof(DateTime), new DateTimeToDateTime());

#if FW2
			AddSameType(typeof(SByte?),    new NullableSByteToNullableSByte());
			AddSameType(typeof(Int16?),    new NullableInt16ToNullableInt16());
			AddSameType(typeof(Int32?),    new NullableInt32ToNullableInt32());
			AddSameType(typeof(Int64?),    new NullableInt64ToNullableInt64());
			AddSameType(typeof(Byte?),     new NullableByteToNullableByte());
			AddSameType(typeof(UInt16?),   new NullableUInt16ToNullableUInt16());
			AddSameType(typeof(UInt32?),   new NullableUInt32ToNullableUInt32());
			AddSameType(typeof(UInt64?),   new NullableUInt64ToNullableUInt64());
			AddSameType(typeof(Boolean?),  new NullableBooleanToNullableBoolean());
			AddSameType(typeof(Char?),     new NullableCharToNullableChar());
			AddSameType(typeof(Single?),   new NullableSingleToNullableSingle());
			AddSameType(typeof(Double?),   new NullableDoubleToNullableDouble());
			AddSameType(typeof(Decimal?),  new NullableDecimalToNullableDecimal());
			AddSameType(typeof(Guid?),     new NullableGuidToNullableGuid());
			AddSameType(typeof(DateTime?), new NullableDateTimeToNullableDateTime());
#endif

			AddSameType(typeof(SqlByte),     new SqlByteToSqlByte());
			AddSameType(typeof(SqlInt16),    new SqlInt16ToSqlInt16());
			AddSameType(typeof(SqlInt32),    new SqlInt32ToSqlInt32());
			AddSameType(typeof(SqlInt64),    new SqlInt64ToSqlInt64());
			AddSameType(typeof(SqlSingle),   new SqlSingleToSqlSingle());
			AddSameType(typeof(SqlBoolean),  new SqlBooleanToSqlBoolean());
			AddSameType(typeof(SqlDouble),   new SqlDoubleToSqlDouble());
			AddSameType(typeof(SqlDateTime), new SqlDateTimeToSqlDateTime());
			AddSameType(typeof(SqlDecimal),  new SqlDecimalToSqlDecimal());
			AddSameType(typeof(SqlMoney),    new SqlMoneyToSqlMoney());
			AddSameType(typeof(SqlGuid),     new SqlGuidToSqlGuid());
			AddSameType(typeof(SqlString),   new SqlStringToSqlString());

			_mappers.Add(new KeyValue(typeof(Int32),    typeof(Double)),  new Int32ToDouble());
			_mappers.Add(new KeyValue(typeof(Int32),    typeof(Decimal)), new Int32ToDecimal());
			_mappers.Add(new KeyValue(typeof(Double),   typeof(Int32)),   new DoubleToInt32());
			_mappers.Add(new KeyValue(typeof(Decimal),  typeof(Int32)),   new DecimalToInt32());

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

		#region Simple Types

		class SByteToSByte : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSByte(destObject, destIndex, source.GetSByte(sourceObject, sourceIndex));
			}
		}

		class Int16ToInt16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt16(destObject, destIndex, source.GetInt16(sourceObject, sourceIndex));
			}
		}

		class Int32ToInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt32(destObject, destIndex, source.GetInt32(sourceObject, sourceIndex));
			}
		}

		class Int64ToInt64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetInt64(destObject, destIndex, source.GetInt64(sourceObject, sourceIndex));
			}
		}

		class ByteToByte : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetByte(destObject, destIndex, source.GetByte(sourceObject, sourceIndex));
			}
		}

		class UInt16ToUInt16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt16(destObject, destIndex, source.GetUInt16(sourceObject, sourceIndex));
			}
		}

		class UInt32ToUInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt32(destObject, destIndex, source.GetUInt32(sourceObject, sourceIndex));
			}
		}

		class UInt64ToUInt64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetUInt64(destObject, destIndex, source.GetUInt64(sourceObject, sourceIndex));
			}
		}

		class BooleanToBoolean : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetBoolean(destObject, destIndex, source.GetBoolean(sourceObject, sourceIndex));
			}
		}

		class CharToChar : IValueMapper
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

		class SingleToSingle : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSingle(destObject, destIndex, source.GetSingle(sourceObject, sourceIndex));
			}
		}

		class DoubleToDouble : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDouble(destObject, destIndex, source.GetDouble(sourceObject, sourceIndex));
			}
		}

		class DecimalToDecimal : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDecimal(destObject, destIndex, source.GetDecimal(sourceObject, sourceIndex));
			}
		}

		class GuidToGuid : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetGuid(destObject, destIndex, source.GetGuid(sourceObject, sourceIndex));
			}
		}

		class DateTimeToDateTime : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetDateTime(destObject, destIndex, source.GetDateTime(sourceObject, sourceIndex));
			}
		}

		#endregion

#if FW2
		#region Simple Types

		class NullableSByteToNullableSByte : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableSByte(destObject, destIndex, source.GetNullableSByte(sourceObject, sourceIndex));
			}
		}

		class NullableInt16ToNullableInt16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt16(destObject, destIndex, source.GetNullableInt16(sourceObject, sourceIndex));
			}
		}

		class NullableInt32ToNullableInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt32(destObject, destIndex, source.GetNullableInt32(sourceObject, sourceIndex));
			}
		}

		class NullableInt64ToNullableInt64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableInt64(destObject, destIndex, source.GetNullableInt64(sourceObject, sourceIndex));
			}
		}

		class NullableByteToNullableByte : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableByte(destObject, destIndex, source.GetNullableByte(sourceObject, sourceIndex));
			}
		}

		class NullableUInt16ToNullableUInt16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt16(destObject, destIndex, source.GetNullableUInt16(sourceObject, sourceIndex));
			}
		}

		class NullableUInt32ToNullableUInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt32(destObject, destIndex, source.GetNullableUInt32(sourceObject, sourceIndex));
			}
		}

		class NullableUInt64ToNullableUInt64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableUInt64(destObject, destIndex, source.GetNullableUInt64(sourceObject, sourceIndex));
			}
		}

		class NullableBooleanToNullableBoolean : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableBoolean(destObject, destIndex, source.GetNullableBoolean(sourceObject, sourceIndex));
			}
		}

		class NullableCharToNullableChar : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableChar(destObject, destIndex, source.GetNullableChar(sourceObject, sourceIndex));
			}
		}

		class NullableSingleToNullableSingle : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableSingle(destObject, destIndex, source.GetNullableSingle(sourceObject, sourceIndex));
			}
		}

		class NullableDoubleToNullableDouble : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDouble(destObject, destIndex, source.GetNullableDouble(sourceObject, sourceIndex));
			}
		}

		class NullableDecimalToNullableDecimal : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDecimal(destObject, destIndex, source.GetNullableDecimal(sourceObject, sourceIndex));
			}
		}

		class NullableGuidToNullableGuid : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableGuid(destObject, destIndex, source.GetNullableGuid(sourceObject, sourceIndex));
			}
		}

		class NullableDateTimeToNullableDateTime : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetNullableDateTime(destObject, destIndex, source.GetNullableDateTime(sourceObject, sourceIndex));
			}
		}

		#endregion
#endif

		#region SQL Types

		class SqlByteToSqlByte : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlByte(destObject, destIndex, source.GetSqlByte(sourceObject, sourceIndex));
			}
		}

		class SqlInt16ToSqlInt16 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt16(destObject, destIndex, source.GetSqlInt16(sourceObject, sourceIndex));
			}
		}

		class SqlInt32ToSqlInt32 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt32(destObject, destIndex, source.GetSqlInt32(sourceObject, sourceIndex));
			}
		}

		class SqlInt64ToSqlInt64 : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlInt64(destObject, destIndex, source.GetSqlInt64(sourceObject, sourceIndex));
			}
		}

		class SqlSingleToSqlSingle : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlSingle(destObject, destIndex, source.GetSqlSingle(sourceObject, sourceIndex));
			}
		}

		class SqlBooleanToSqlBoolean : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlBoolean(destObject, destIndex, source.GetSqlBoolean(sourceObject, sourceIndex));
			}
		}

		class SqlDoubleToSqlDouble : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDouble(destObject, destIndex, source.GetSqlDouble(sourceObject, sourceIndex));
			}
		}

		class SqlDateTimeToSqlDateTime : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDateTime(destObject, destIndex, source.GetSqlDateTime(sourceObject, sourceIndex));
			}
		}

		class SqlDecimalToSqlDecimal : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlDecimal(destObject, destIndex, source.GetSqlDecimal(sourceObject, sourceIndex));
			}
		}

		class SqlMoneyToSqlMoney : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlMoney(destObject, destIndex, source.GetSqlMoney(sourceObject, sourceIndex));
			}
		}

		class SqlGuidToSqlGuid : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlGuid(destObject, destIndex, source.GetSqlGuid(sourceObject, sourceIndex));
			}
		}

		class SqlStringToSqlString : IValueMapper
		{
			public void Map(
				IMapDataSource      source, object sourceObject, int sourceIndex,
				IMapDataDestination dest,   object destObject,   int destIndex)
			{
				if (source.IsNull(sourceObject, sourceIndex))
					dest.SetNull(destObject, destIndex);
				else
					dest.SetSqlString(destObject, destIndex, source.GetSqlString(sourceObject, sourceIndex));
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
					dest.SetDouble(destObject, destIndex, Convert.ToDouble(source.GetInt32(sourceObject, sourceIndex)));
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
					dest.SetDecimal(destObject, destIndex, Convert.ToDecimal(source.GetInt32(sourceObject, sourceIndex)));
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
					dest.SetInt32(destObject, destIndex, Convert.ToInt32(source.GetDouble(sourceObject, sourceIndex)));
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
					dest.SetInt32(destObject, destIndex, Convert.ToInt32(source.GetDecimal(sourceObject, sourceIndex)));
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

		#region Generic Mappers

		static class GetData<T>
		{
			public delegate T GetMethod(IMapDataSource s, object o, int index);

			private static GetMethod Conv<T1>(GetData<T1>.GetMethod op)
			{
				return (GetMethod)(object)op;
			}

			public  static GetMethod Get = GetGetter();
			private static GetMethod GetGetter()
			{
				Type t = typeof(T);

				if (t == typeof(SByte))       return Conv<SByte>      (delegate(IMapDataSource s, object o, int i) { return s.GetSByte      (o, i); });
				if (t == typeof(Int16))       return Conv<Int16>      (delegate(IMapDataSource s, object o, int i) { return s.GetInt16      (o, i); });
				if (t == typeof(Int32))       return Conv<Int32>      (delegate(IMapDataSource s, object o, int i) { return s.GetInt32      (o, i); });
				if (t == typeof(Int64))       return Conv<Int64>      (delegate(IMapDataSource s, object o, int i) { return s.GetInt64      (o, i); });
				if (t == typeof(Byte))        return Conv<Byte>       (delegate(IMapDataSource s, object o, int i) { return s.GetByte       (o, i); });
				if (t == typeof(UInt16))      return Conv<UInt16>     (delegate(IMapDataSource s, object o, int i) { return s.GetUInt16     (o, i); });
				if (t == typeof(UInt32))      return Conv<UInt32>     (delegate(IMapDataSource s, object o, int i) { return s.GetUInt32     (o, i); });
				if (t == typeof(UInt64))      return Conv<UInt64>     (delegate(IMapDataSource s, object o, int i) { return s.GetUInt64     (o, i); });

				if (t == typeof(Boolean))     return Conv<Boolean>    (delegate(IMapDataSource s, object o, int i) { return s.GetBoolean    (o, i); });
				if (t == typeof(Char))        return Conv<Char>       (delegate(IMapDataSource s, object o, int i) { return s.GetChar       (o, i); });
				if (t == typeof(Single))      return Conv<Single>     (delegate(IMapDataSource s, object o, int i) { return s.GetSingle     (o, i); });
				if (t == typeof(Double))      return Conv<Double>     (delegate(IMapDataSource s, object o, int i) { return s.GetDouble     (o, i); });
				if (t == typeof(Decimal))     return Conv<Decimal>    (delegate(IMapDataSource s, object o, int i) { return s.GetDecimal    (o, i); });
				if (t == typeof(Guid))        return Conv<Guid>       (delegate(IMapDataSource s, object o, int i) { return s.GetGuid       (o, i); });
				if (t == typeof(DateTime))    return Conv<DateTime>   (delegate(IMapDataSource s, object o, int i) { return s.GetDateTime   (o, i); });

#if FW2
				if (t == typeof(SByte?))      return Conv<SByte?>     (delegate(IMapDataSource s, object o, int i) { return s.GetNullableSByte   (o, i); });
				if (t == typeof(Int16?))      return Conv<Int16?>     (delegate(IMapDataSource s, object o, int i) { return s.GetNullableInt16   (o, i); });
				if (t == typeof(Int32?))      return Conv<Int32?>     (delegate(IMapDataSource s, object o, int i) { return s.GetNullableInt32   (o, i); });
				if (t == typeof(Int64?))      return Conv<Int64?>     (delegate(IMapDataSource s, object o, int i) { return s.GetNullableInt64   (o, i); });
				if (t == typeof(Byte?))       return Conv<Byte?>      (delegate(IMapDataSource s, object o, int i) { return s.GetNullableByte    (o, i); });
				if (t == typeof(UInt16?))     return Conv<UInt16?>    (delegate(IMapDataSource s, object o, int i) { return s.GetNullableUInt16  (o, i); });
				if (t == typeof(UInt32?))     return Conv<UInt32?>    (delegate(IMapDataSource s, object o, int i) { return s.GetNullableUInt32  (o, i); });
				if (t == typeof(UInt64?))     return Conv<UInt64?>    (delegate(IMapDataSource s, object o, int i) { return s.GetNullableUInt64  (o, i); });

				if (t == typeof(Boolean?))    return Conv<Boolean?>   (delegate(IMapDataSource s, object o, int i) { return s.GetNullableBoolean (o, i); });
				if (t == typeof(Char?))       return Conv<Char?>      (delegate(IMapDataSource s, object o, int i) { return s.GetNullableChar    (o, i); });
				if (t == typeof(Single?))     return Conv<Single?>    (delegate(IMapDataSource s, object o, int i) { return s.GetNullableSingle  (o, i); });
				if (t == typeof(Double?))     return Conv<Double?>    (delegate(IMapDataSource s, object o, int i) { return s.GetNullableDouble  (o, i); });
				if (t == typeof(Decimal?))    return Conv<Decimal?>   (delegate(IMapDataSource s, object o, int i) { return s.GetNullableDecimal (o, i); });
				if (t == typeof(Guid?))       return Conv<Guid?>      (delegate(IMapDataSource s, object o, int i) { return s.GetNullableGuid    (o, i); });
				if (t == typeof(DateTime?))   return Conv<DateTime?>  (delegate(IMapDataSource s, object o, int i) { return s.GetNullableDateTime(o, i); });
#endif

				if (t == typeof(SqlByte))     return Conv<SqlByte>    (delegate(IMapDataSource s, object o, int i) { return s.GetSqlByte    (o, i); });
				if (t == typeof(SqlInt16))    return Conv<SqlInt16>   (delegate(IMapDataSource s, object o, int i) { return s.GetSqlInt16   (o, i); });
				if (t == typeof(SqlInt32))    return Conv<SqlInt32>   (delegate(IMapDataSource s, object o, int i) { return s.GetSqlInt32   (o, i); });
				if (t == typeof(SqlInt64))    return Conv<SqlInt64>   (delegate(IMapDataSource s, object o, int i) { return s.GetSqlInt64   (o, i); });
				if (t == typeof(SqlSingle))   return Conv<SqlSingle>  (delegate(IMapDataSource s, object o, int i) { return s.GetSqlSingle  (o, i); });
				if (t == typeof(SqlBoolean))  return Conv<SqlBoolean> (delegate(IMapDataSource s, object o, int i) { return s.GetSqlBoolean (o, i); });
				if (t == typeof(SqlDouble))   return Conv<SqlDouble>  (delegate(IMapDataSource s, object o, int i) { return s.GetSqlDouble  (o, i); });
				if (t == typeof(SqlDateTime)) return Conv<SqlDateTime>(delegate(IMapDataSource s, object o, int i) { return s.GetSqlDateTime(o, i); });
				if (t == typeof(SqlDecimal))  return Conv<SqlDecimal> (delegate(IMapDataSource s, object o, int i) { return s.GetSqlDecimal (o, i); });
				if (t == typeof(SqlMoney))    return Conv<SqlMoney>   (delegate(IMapDataSource s, object o, int i) { return s.GetSqlMoney   (o, i); });
				if (t == typeof(SqlGuid))     return Conv<SqlGuid>    (delegate(IMapDataSource s, object o, int i) { return s.GetSqlGuid    (o, i); });
				if (t == typeof(SqlString))   return Conv<SqlString>  (delegate(IMapDataSource s, object o, int i) { return s.GetSqlString  (o, i); });

				return null;
			}
		}

		static class SetData<T>
		{
			public delegate void SetMethod(IMapDataDestination d, object o, int index, T value);

			private static SetMethod Conv<T1>(SetData<T1>.SetMethod op)
			{
				return (SetMethod)(object)op;
			}

			public  static SetMethod Set = GetSetter();
			private static SetMethod GetSetter()
			{
				Type t = typeof(T);

				if (t == typeof(SByte))       return Conv<SByte>      (delegate(IMapDataDestination d, object o, int i, SByte       v) { d.SetSByte      (o, i, v); });
				if (t == typeof(Int16))       return Conv<Int16>      (delegate(IMapDataDestination d, object o, int i, Int16       v) { d.SetInt16      (o, i, v); });
				if (t == typeof(Int32))       return Conv<Int32>      (delegate(IMapDataDestination d, object o, int i, Int32       v) { d.SetInt32      (o, i, v); });
				if (t == typeof(Int64))       return Conv<Int64>      (delegate(IMapDataDestination d, object o, int i, Int64       v) { d.SetInt64      (o, i, v); });
				if (t == typeof(Byte))        return Conv<Byte>       (delegate(IMapDataDestination d, object o, int i, Byte        v) { d.SetByte       (o, i, v); });
				if (t == typeof(UInt16))      return Conv<UInt16>     (delegate(IMapDataDestination d, object o, int i, UInt16      v) { d.SetUInt16     (o, i, v); });
				if (t == typeof(UInt32))      return Conv<UInt32>     (delegate(IMapDataDestination d, object o, int i, UInt32      v) { d.SetUInt32     (o, i, v); });
				if (t == typeof(UInt64))      return Conv<UInt64>     (delegate(IMapDataDestination d, object o, int i, UInt64      v) { d.SetUInt64     (o, i, v); });

				if (t == typeof(Boolean))     return Conv<Boolean>    (delegate(IMapDataDestination d, object o, int i, Boolean     v) { d.SetBoolean    (o, i, v); });
				if (t == typeof(Char))        return Conv<Char>       (delegate(IMapDataDestination d, object o, int i, Char        v) { d.SetChar       (o, i, v); });
				if (t == typeof(Single))      return Conv<Single>     (delegate(IMapDataDestination d, object o, int i, Single      v) { d.SetSingle     (o, i, v); });
				if (t == typeof(Double))      return Conv<Double>     (delegate(IMapDataDestination d, object o, int i, Double      v) { d.SetDouble     (o, i, v); });
				if (t == typeof(Decimal))     return Conv<Decimal>    (delegate(IMapDataDestination d, object o, int i, Decimal     v) { d.SetDecimal    (o, i, v); });
				if (t == typeof(Guid))        return Conv<Guid>       (delegate(IMapDataDestination d, object o, int i, Guid        v) { d.SetGuid       (o, i, v); });
				if (t == typeof(DateTime))    return Conv<DateTime>   (delegate(IMapDataDestination d, object o, int i, DateTime    v) { d.SetDateTime   (o, i, v); });

#if FW2
				if (t == typeof(SByte?))      return Conv<SByte?>     (delegate(IMapDataDestination d, object o, int i, SByte?      v) { d.SetNullableSByte   (o, i, v); });
				if (t == typeof(Int16?))      return Conv<Int16?>     (delegate(IMapDataDestination d, object o, int i, Int16?      v) { d.SetNullableInt16   (o, i, v); });
				if (t == typeof(Int32?))      return Conv<Int32?>     (delegate(IMapDataDestination d, object o, int i, Int32?      v) { d.SetNullableInt32   (o, i, v); });
				if (t == typeof(Int64?))      return Conv<Int64?>     (delegate(IMapDataDestination d, object o, int i, Int64?      v) { d.SetNullableInt64   (o, i, v); });
				if (t == typeof(Byte?))       return Conv<Byte?>      (delegate(IMapDataDestination d, object o, int i, Byte?       v) { d.SetNullableByte    (o, i, v); });
				if (t == typeof(UInt16?))     return Conv<UInt16?>    (delegate(IMapDataDestination d, object o, int i, UInt16?     v) { d.SetNullableUInt16  (o, i, v); });
				if (t == typeof(UInt32?))     return Conv<UInt32?>    (delegate(IMapDataDestination d, object o, int i, UInt32?     v) { d.SetNullableUInt32  (o, i, v); });
				if (t == typeof(UInt64?))     return Conv<UInt64?>    (delegate(IMapDataDestination d, object o, int i, UInt64?     v) { d.SetNullableUInt64  (o, i, v); });

				if (t == typeof(Boolean?))    return Conv<Boolean?>   (delegate(IMapDataDestination d, object o, int i, Boolean?    v) { d.SetNullableBoolean (o, i, v); });
				if (t == typeof(Char?))       return Conv<Char?>      (delegate(IMapDataDestination d, object o, int i, Char?       v) { d.SetNullableChar    (o, i, v); });
				if (t == typeof(Single?))     return Conv<Single?>    (delegate(IMapDataDestination d, object o, int i, Single?     v) { d.SetNullableSingle  (o, i, v); });
				if (t == typeof(Double?))     return Conv<Double?>    (delegate(IMapDataDestination d, object o, int i, Double?     v) { d.SetNullableDouble  (o, i, v); });
				if (t == typeof(Decimal?))    return Conv<Decimal?>   (delegate(IMapDataDestination d, object o, int i, Decimal?    v) { d.SetNullableDecimal (o, i, v); });
				if (t == typeof(Guid?))       return Conv<Guid?>      (delegate(IMapDataDestination d, object o, int i, Guid?       v) { d.SetNullableGuid    (o, i, v); });
				if (t == typeof(DateTime?))   return Conv<DateTime?>  (delegate(IMapDataDestination d, object o, int i, DateTime?   v) { d.SetNullableDateTime(o, i, v); });
#endif

				if (t == typeof(SqlByte))     return Conv<SqlByte>    (delegate(IMapDataDestination d, object o, int i, SqlByte     v) { d.SetSqlByte    (o, i, v); });
				if (t == typeof(SqlInt16))    return Conv<SqlInt16>   (delegate(IMapDataDestination d, object o, int i, SqlInt16    v) { d.SetSqlInt16   (o, i, v); });
				if (t == typeof(SqlInt32))    return Conv<SqlInt32>   (delegate(IMapDataDestination d, object o, int i, SqlInt32    v) { d.SetSqlInt32   (o, i, v); });
				if (t == typeof(SqlInt64))    return Conv<SqlInt64>   (delegate(IMapDataDestination d, object o, int i, SqlInt64    v) { d.SetSqlInt64   (o, i, v); });
				if (t == typeof(SqlSingle))   return Conv<SqlSingle>  (delegate(IMapDataDestination d, object o, int i, SqlSingle   v) { d.SetSqlSingle  (o, i, v); });
				if (t == typeof(SqlBoolean))  return Conv<SqlBoolean> (delegate(IMapDataDestination d, object o, int i, SqlBoolean  v) { d.SetSqlBoolean (o, i, v); });
				if (t == typeof(SqlDouble))   return Conv<SqlDouble>  (delegate(IMapDataDestination d, object o, int i, SqlDouble   v) { d.SetSqlDouble  (o, i, v); });
				if (t == typeof(SqlDateTime)) return Conv<SqlDateTime>(delegate(IMapDataDestination d, object o, int i, SqlDateTime v) { d.SetSqlDateTime(o, i, v); });
				if (t == typeof(SqlDecimal))  return Conv<SqlDecimal> (delegate(IMapDataDestination d, object o, int i, SqlDecimal  v) { d.SetSqlDecimal (o, i, v); });
				if (t == typeof(SqlMoney))    return Conv<SqlMoney>   (delegate(IMapDataDestination d, object o, int i, SqlMoney    v) { d.SetSqlMoney   (o, i, v); });
				if (t == typeof(SqlGuid))     return Conv<SqlGuid>    (delegate(IMapDataDestination d, object o, int i, SqlGuid     v) { d.SetSqlGuid    (o, i, v); });
				if (t == typeof(SqlString))   return Conv<SqlString>  (delegate(IMapDataDestination d, object o, int i, SqlString   v) { d.SetSqlString  (o, i, v); });

				return null;
			}
		}

		interface IGetSetDataChecker
		{
			bool Check();
		}

		class GetSetDataChecker<S,D> : IGetSetDataChecker
		{
			public bool Check()
			{
				return
					GetData<S>.Get != null && SetData<S>.Set != null &&
					GetData<D>.Get != null && SetData<D>.Set != null;
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
					SetData<T>.SetMethod set = SetData<T>.Set;
					GetData<T>.GetMethod get = GetData<T>.Get;

					set(dest, destObject, destIndex, get(source, sourceObject, sourceIndex));

					//SetData<T>.Set(
					//	dest, destObject, destIndex,
					//	GetData<T>.Get(source, sourceObject, sourceIndex));
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
					SetData<D>.SetMethod       set  = SetData<D>.Set;
					GetData<S>.GetMethod       get  = GetData<S>.Get;
					Convert<D,S>.ConvertMethod conv = Convert<D, S>.From;

					set(dest, destObject, destIndex, conv(get(source, sourceObject, sourceIndex)));

					//SetData<D>.Set(
					//	dest, destObject, destIndex,
					//	ConvertTo<D>.From(GetData<S>.Get(source, sourceObject, sourceIndex)));
				}
			}
		}

		#endregion

#endif
	}
}
