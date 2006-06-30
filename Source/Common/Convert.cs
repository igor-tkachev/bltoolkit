using System;
using System.Data.SqlTypes;
using System.IO;

namespace BLToolkit.Common
{
	public static class Convert<T,P>
	{
		public delegate T ConvertMethod(P p);

		public static readonly ConvertMethod From = GetConverter();
		public static ConvertMethod GetConverter()
		{
			Type t = typeof(T);

			// Convert to the same type.
			//
			if (t.IsAssignableFrom(typeof(P)))
				return (ConvertMethod)(object)(Convert<P,P>.ConvertMethod)(delegate(P p) { return p; });

			// Scalar Types.
			//
			if (t == typeof(String))      return GetStringConverter();

			if (t == typeof(SByte))       return GetSByteConverter();
			if (t == typeof(Int16))       return GetInt16Converter();
			if (t == typeof(Int32))       return GetInt32Converter();
			if (t == typeof(Int64))       return GetInt64Converter();

			if (t == typeof(Byte))        return GetByteConverter();
			if (t == typeof(UInt16))      return GetUInt16Converter();
			if (t == typeof(UInt32))      return GetUInt32Converter();
			if (t == typeof(UInt64))      return GetUInt64Converter();

			if (t == typeof(Char))        return GetCharConverter();
			if (t == typeof(Single))      return GetSingleConverter();
			if (t == typeof(Double))      return GetDoubleConverter();

			if (t == typeof(Boolean))     return GetBooleanConverter();
			if (t == typeof(Decimal))     return GetDecimalConverter();
			if (t == typeof(DateTime))    return GetDateTimeConverter();
			if (t == typeof(TimeSpan))    return GetTimeSpanConverter();
			if (t == typeof(Guid))        return GetGuidConverter();
			if (t == typeof(Stream))      return GetStreamConverter();

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return GetNullableSByteConverter();
			if (t == typeof(Int16?))      return GetNullableInt16Converter();
			if (t == typeof(Int32?))      return GetNullableInt32Converter();
			if (t == typeof(Int64?))      return GetNullableInt64Converter();

			if (t == typeof(Byte?))       return GetNullableByteConverter();
			if (t == typeof(UInt16?))     return GetNullableUInt16Converter();
			if (t == typeof(UInt32?))     return GetNullableUInt32Converter();
			if (t == typeof(UInt64?))     return GetNullableUInt64Converter();

			if (t == typeof(Char?))       return GetNullableCharConverter();
			if (t == typeof(Single?))     return GetNullableSingleConverter();
			if (t == typeof(Double?))     return GetNullableDoubleConverter();

			if (t == typeof(Boolean?))    return GetNullableBooleanConverter();
			if (t == typeof(Decimal?))    return GetNullableDecimalConverter();
			if (t == typeof(DateTime?))   return GetNullableDateTimeConverter();
			if (t == typeof(TimeSpan?))   return GetNullableTimeSpanConverter();
			if (t == typeof(Guid?))       return GetNullableGuidConverter();

			// Sql Types.
			//
			if (t == typeof(SqlString))   return GetSqlStringConverter();

			if (t == typeof(SqlByte))     return GetSqlByteConverter();
			if (t == typeof(SqlInt16))    return GetSqlInt16Converter();
			if (t == typeof(SqlInt32))    return GetSqlInt32Converter();
			if (t == typeof(SqlInt64))    return GetSqlInt64Converter();

			if (t == typeof(SqlSingle))   return GetSqlSingleConverter();
			if (t == typeof(SqlDouble))   return GetSqlDoubleConverter();
			if (t == typeof(SqlDecimal))  return GetSqlDecimalConverter();
			if (t == typeof(SqlMoney))    return GetSqlMoneyConverter();

			if (t == typeof(SqlBoolean))  return GetSqlBooleanConverter();
			if (t == typeof(SqlDateTime)) return GetSqlDateTimeConverter();
			if (t == typeof(SqlGuid))     return GetSqlGuidConverter();
			if (t == typeof(SqlBinary))   return GetSqlBinaryConverter();

			return delegate(P o) { return (T)Convert.ChangeType(o, typeof(T)); };
		}

		#region Scalar Types

		#region String

		private static ConvertMethod GetStringConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<String,SByte>.ConvertMethod)      (delegate(SByte       p) { return p.ToString(); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<String,Int16>.ConvertMethod)      (delegate(Int16       p) { return p.ToString(); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<String,Int32>.ConvertMethod)      (delegate(Int32       p) { return p.ToString(); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<String,Int64>.ConvertMethod)      (delegate(Int64       p) { return p.ToString(); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<String,Byte>.ConvertMethod)       (delegate(Byte        p) { return p.ToString(); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<String,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return p.ToString(); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<String,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return p.ToString(); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<String,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return p.ToString(); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<String,Char>.ConvertMethod)       (delegate(Char        p) { return p.ToString(); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<String,Single>.ConvertMethod)     (delegate(Single      p) { return p.ToString(); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<String,Double>.ConvertMethod)     (delegate(Double      p) { return p.ToString(); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<String,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return p.ToString(); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<String,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return p.ToString(); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<String,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return p.ToString(); });
			if (t == typeof(TimeSpan))    return (ConvertMethod)(object)(Convert<String,TimeSpan>.ConvertMethod)   (delegate(TimeSpan    p) { return p.ToString(); });
			if (t == typeof(Guid))        return (ConvertMethod)(object)(Convert<String,Guid>.ConvertMethod)       (delegate(Guid        p) { return p.ToString(); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<String,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.ToString(); });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<String,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.ToString(); });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<String,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.ToString(); });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<String,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.ToString(); });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<String,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.ToString(); });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<String,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.ToString(); });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<String,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.ToString(); });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<String,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.ToString(); });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<String,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.ToString(); });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<String,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.ToString(); });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<String,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.ToString(); });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<String,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.ToString(); });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<String,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.ToString(); });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<String,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.ToString(); });
			if (t == typeof(TimeSpan?))   return (ConvertMethod)(object)(Convert<String,TimeSpan?>.ConvertMethod)  (delegate(TimeSpan?   p) { return p.ToString(); });
			if (t == typeof(Guid?))       return (ConvertMethod)(object)(Convert<String,Guid?>.ConvertMethod)      (delegate(Guid?       p) { return p.ToString(); });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<String,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToString(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<String,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToString(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<String,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToString(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<String,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToString(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<String,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToString(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<String,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToString(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<String,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToString(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<String,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToString(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<String,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToString(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<String,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToString(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<String,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.ToString(); });
			if (t == typeof(SqlGuid))     return (ConvertMethod)(object)(Convert<String,SqlGuid>.ConvertMethod)    (delegate(SqlGuid     p) { return p.ToString(); });
			if (t == typeof(SqlBinary))   return (ConvertMethod)(object)(Convert<String,SqlBinary>.ConvertMethod)  (delegate(SqlBinary   p) { return p.ToString(); });

			return (ConvertMethod)(object)(Convert<String,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToString(p);
			});
		}

		#endregion

		#region SByte

		private static ConvertMethod GetSByteConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SByte,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToSByte(p); });

			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SByte,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToSByte(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SByte,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToSByte(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SByte,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToSByte(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SByte,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SByte,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SByte,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SByte,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToSByte(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SByte,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToSByte(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SByte,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToSByte(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SByte,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToSByte(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SByte,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToSByte(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SByte,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToSByte(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SByte,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToSByte(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SByte,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue?                 p.Value  : (SByte)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SByte,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SByte,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SByte,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SByte,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SByte,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SByte,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SByte,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SByte,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SByte,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SByte,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SByte,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SByte,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SByte,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SByte,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SByte,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SByte,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SByte,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SByte,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SByte,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SByte,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SByte,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SByte,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SByte,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SByte,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); });

			return (ConvertMethod)(object)(Convert<SByte,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToSByte(p);
			});
		}

		#endregion

		#region Int16

		private static ConvertMethod GetInt16Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int16,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt16(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int16,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Int16,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Int16,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt16(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int16,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int16,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int16,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int16,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt16(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int16,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt16(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int16,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt16(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int16,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt16(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int16,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt16(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int16,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt16(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int16,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt16(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int16,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Int16,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue?                 p.Value  : (Int16)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Int16,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Int16,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int16,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int16,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Int16,UInt32?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int16,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int16,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int16,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int16,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int16,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int16,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int16,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int16,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int16,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int16,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Int16)0:                 p.Value;  });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int16,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int16,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int16,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int16,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int16,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int16,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int16,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int16,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); });

			return (ConvertMethod)(object)(Convert<Int16,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt16(p);
			});
		}

		#endregion

		#region Int32

		private static ConvertMethod GetInt32Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int32,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt32(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int32,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Int32,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Int32,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt32(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int32,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int32,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int32,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int32,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt32(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int32,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt32(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int32,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt32(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int32,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt32(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int32,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt32(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int32,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt32(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int32,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt32(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int32,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Int32,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Int32,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue?                 p.Value  : (Int32)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Int32,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int32,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int32,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Int32,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int32,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int32,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int32,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int32,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int32,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int32,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int32,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int32,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int32,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int32,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int32,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Int32)0:                 p.Value;  });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int32,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int32,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int32,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int32,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int32,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int32,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int32,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); });

			return (ConvertMethod)(object)(Convert<Int32,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt32(p);
			});
		}

		#endregion

		#region Int64

		private static ConvertMethod GetInt64Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int64,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt64(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int64,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Int64,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Int64,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt64(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int64,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int64,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int64,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int64,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt64(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int64,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt64(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int64,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt64(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int64,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt64(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int64,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt64(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int64,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt64(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int64,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt64(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int64,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Int64,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Int64,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Int64,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue?                 p.Value  : (Int64)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int64,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int64,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Int64,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int64,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int64,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int64,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int64,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int64,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int64,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int64,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int64,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int64,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int64,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int64,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int64,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Int64)0:                 p.Value;  });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int64,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int64,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int64,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int64,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int64,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int64,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); });

			return (ConvertMethod)(object)(Convert<Int64,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt64(p);
			});
		}

		#endregion

		#region Byte

		private static ConvertMethod GetByteConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Byte,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToByte(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Byte,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToByte(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Byte,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToByte(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Byte,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToByte(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Byte,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToByte(p); });

			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Byte,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Byte,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Byte,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToByte(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Byte,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToByte(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Byte,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToByte(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Byte,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToByte(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Byte,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToByte(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Byte,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToByte(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Byte,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToByte(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Byte,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Byte,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Byte,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Byte,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Byte,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue?                p.Value  : (Byte)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Byte,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Byte,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Byte,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Byte,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Byte,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Byte,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Byte,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Byte,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Byte,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Byte,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Byte,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Byte)0:                p.Value;  });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Byte,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Byte,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Byte,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Byte,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Byte,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Byte,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Byte,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Byte,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Byte,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); });

			return (ConvertMethod)(object)(Convert<Byte,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToByte(p);
			});
		}

		#endregion

		#region UInt16

		private static ConvertMethod GetUInt16Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt16,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToUInt16(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt16,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt16,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt16,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt16,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt16(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt16,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt16(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<UInt16,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToUInt16(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<UInt16,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToUInt16(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt16,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt16(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt16,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt16(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt16,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt16(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt16,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt16(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt16,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt16(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt16,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt16(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt16,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt16,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt16,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt16,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt16,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<UInt16,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue?                  p.Value  : (UInt16)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<UInt16,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<UInt16,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt16,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt16,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt16,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt16,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt16,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt16,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt16,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt16,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt16,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt16,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt16,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt16,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt16,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt16,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt16,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt16,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt16,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt16,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt16(p);
			});
		}

		#endregion

		#region UInt32

		private static ConvertMethod GetUInt32Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt32,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToUInt32(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt32,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt32,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt32,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt32,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt32(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt32,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt32(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<UInt32,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToUInt32(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<UInt32,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToUInt32(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt32,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt32(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt32,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt32(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt32,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt32(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt32,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt32(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt32,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt32(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt32,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt32(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt32,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt32,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt32,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt32,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt32,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<UInt32,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<UInt32,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue?                  p.Value  : (UInt32)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<UInt32,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt32,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt32,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt32,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt32,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt32,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt32,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt32,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt32,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt32,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt32,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt32,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt32,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt32,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt32,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt32,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt32,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt32,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt32,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt32(p);
			});
		}

		#endregion

		#region UInt64

		private static ConvertMethod GetUInt64Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt64,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToUInt64(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt64,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt64,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt64,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt64,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt64(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt64,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt64(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<UInt64,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToUInt64(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<UInt64,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToUInt64(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt64,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt64(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt64,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt64(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt64,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt64(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt64,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt64(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt64,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt64(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt64,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt64(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt64,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt64,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt64,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt64,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt64,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<UInt64,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<UInt64,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<UInt64,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue?                  p.Value  : (UInt64)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt64,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt64,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt64,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt64,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt64,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt64,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt64,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt64,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt64,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt64,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt64,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt64,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt64,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt64,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt64,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt64,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt64,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt64,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt64(p);
			});
		}

		#endregion

		#region Char

		private static ConvertMethod GetCharConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Char,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToChar(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Char,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToChar(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Char,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToChar(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Char,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToChar(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Char,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToChar(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Char,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToChar(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Char,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToChar(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Char,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToChar(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Char,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToChar(p); });

			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Char,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToChar(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Char,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToChar(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Char,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToChar(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Char,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToChar(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Char,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToChar(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Char,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Char,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Char,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Char,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Char,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Char,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Char,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Char,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Char,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue?                p.Value  : (Char)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Char,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Char,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Char,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Char,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Char,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToChar(p.Value) : (Char)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Char,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Char,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Char,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Char,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Char,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Char,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Char,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Char,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Char,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Char,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Char,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Char)0: Convert.ToChar(p.Value); });

			return (ConvertMethod)(object)(Convert<Char,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToChar(p);
			});
		}

		#endregion

		#region Single

		private static ConvertMethod GetSingleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Single,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToSingle(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Single,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Single,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Single,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Single,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToSingle(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Single,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Single,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Single,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Single,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToSingle(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Single,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToSingle(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Single,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToSingle(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Single,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToSingle(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Single,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToSingle(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Single,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToSingle(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Single,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Single,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Single,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Single,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Single,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Single,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Single,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Single,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Single,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Single,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue?                  p.Value  : (Single)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Single,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Single,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Single,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Single,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToSingle(p.Value) : (Single)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Single,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Single,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Single,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Single,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Single,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Single,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Single)0:                  p.Value;  });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Single,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Single,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Single,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Single,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Single,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Single)0: Convert.ToSingle(p.Value); });

			return (ConvertMethod)(object)(Convert<Single,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToSingle(p);
			});
		}

		#endregion

		#region Double

		private static ConvertMethod GetDoubleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Double,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDouble(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Double,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Double,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Double,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Double,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDouble(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Double,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Double,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Double,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Double,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDouble(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Double,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDouble(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Double,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDouble(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Double,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDouble(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Double,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDouble(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Double,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDouble(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Double,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Double,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Double,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Double,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Double,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Double,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Double,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Double,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Double,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Double,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Double,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue?                  p.Value  : (Double)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Double,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Double,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Double,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToDouble(p.Value) : (Double)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Double,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Double,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Double,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Double,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Double,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Double,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Double,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Double)0:                  p.Value;  });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Double,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Double,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Double,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Double,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Double)0: Convert.ToDouble(p.Value); });

			return (ConvertMethod)(object)(Convert<Double,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDouble(p);
			});
		}

		#endregion

		#region Boolean

		private static ConvertMethod GetBooleanConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Boolean,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToBoolean(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Boolean,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Boolean,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Boolean,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Boolean,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToBoolean(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Boolean,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Boolean,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Boolean,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Boolean,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Boolean,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToBoolean(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Boolean,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToBoolean(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Boolean,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Boolean,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToBoolean(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Boolean,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToBoolean(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Boolean,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Boolean,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Boolean,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Boolean,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Boolean,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Boolean,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Boolean,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Boolean,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Boolean,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Boolean,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Boolean,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Boolean,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue?                   p.Value  : false; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Boolean,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Boolean,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToBoolean(p.Value) : false; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Boolean,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Boolean,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Boolean,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Boolean,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Boolean,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Boolean,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Boolean,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Boolean,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Boolean,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Boolean,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? false:                   p.Value;  });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Boolean,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? false: Convert.ToBoolean(p.Value); });

			return (ConvertMethod)(object)(Convert<Boolean,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToBoolean(p);
			});
		}

		#endregion

		#region Decimal

		private static ConvertMethod GetDecimalConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Decimal,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDecimal(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Decimal,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Decimal,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Decimal,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Decimal,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDecimal(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Decimal,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Decimal,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Decimal,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Decimal,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Decimal,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDecimal(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Decimal,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDecimal(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Decimal,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Decimal,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDecimal(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Decimal,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDecimal(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Decimal,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Decimal,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Decimal,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Decimal,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Decimal,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Decimal,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Decimal,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Decimal,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Decimal,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Decimal,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Decimal,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Decimal,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Decimal,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue?                   p.Value  : (Decimal)0; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Decimal,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToDecimal(p.Value) : (Decimal)0; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Decimal,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Decimal,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Decimal,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Decimal,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Decimal,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Decimal,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Decimal,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Decimal,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? (Decimal)0:                   p.Value;  });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Decimal,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? (Decimal)0:                   p.Value;  });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Decimal,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Decimal,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? (Decimal)0: Convert.ToDecimal(p.Value); });

			return (ConvertMethod)(object)(Convert<Decimal,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDecimal(p);
			});
		}

		#endregion

		#region DateTime

		private static ConvertMethod GetDateTimeConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<DateTime,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDateTime(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<DateTime,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<DateTime,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<DateTime,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<DateTime,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDateTime(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<DateTime,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<DateTime,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<DateTime,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<DateTime,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<DateTime,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDateTime(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<DateTime,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDateTime(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<DateTime,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<DateTime,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDateTime(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<DateTime,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDateTime(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<DateTime,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<DateTime,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<DateTime,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<DateTime,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<DateTime,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<DateTime,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<DateTime,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<DateTime,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<DateTime,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<DateTime,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<DateTime,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<DateTime,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<DateTime,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToDateTime(p.Value) : DateTime.MinValue; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<DateTime,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue?                    p.Value  : DateTime.MinValue; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<DateTime,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<DateTime,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<DateTime,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<DateTime,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<DateTime,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<DateTime,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<DateTime,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<DateTime,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<DateTime,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<DateTime,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<DateTime,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? DateTime.MinValue:                    p.Value;  });

			return (ConvertMethod)(object)(Convert<DateTime,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDateTime(p);
			});
		}

		#endregion

		#region TimeSpan

		private static ConvertMethod GetTimeSpanConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<TimeSpan,String>.ConvertMethod)     (delegate(String      p) { return p == null? TimeSpan.MinValue: TimeSpan.Parse(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<TimeSpan,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return p - DateTime.MinValue; });

			// Nullable Types.
			//
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<TimeSpan,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? p.Value - DateTime.MinValue : TimeSpan.MinValue; });
			if (t == typeof(TimeSpan?))   return (ConvertMethod)(object)(Convert<TimeSpan,TimeSpan?>.ConvertMethod)  (delegate(TimeSpan?   p) { return p.HasValue? p.Value                     : TimeSpan.MinValue; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<TimeSpan,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? TimeSpan.MinValue: TimeSpan.Parse(p.Value);     });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<TimeSpan,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? TimeSpan.MinValue: p.Value - DateTime.MinValue; });

			return (ConvertMethod)(object)(Convert<TimeSpan,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return TimeSpan.MinValue;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#region Guid

		private static ConvertMethod GetGuidConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Guid,String>.ConvertMethod)     (delegate(String      p) { return p == null ? Guid.Empty : new Guid(p); });

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (ConvertMethod)(object)(Convert<Guid,Guid?>.ConvertMethod)      (delegate(Guid?       p) { return p.HasValue? p.Value : Guid.Empty; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Guid,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? Guid.Empty: new Guid(p.Value); });
			if (t == typeof(SqlGuid))     return (ConvertMethod)(object)(Convert<Guid,SqlGuid>.ConvertMethod)    (delegate(SqlGuid     p) { return p.IsNull? Guid.Empty: p.Value; });
			if (t == typeof(SqlBinary))   return (ConvertMethod)(object)(Convert<Guid,SqlBinary>.ConvertMethod)  (delegate(SqlBinary   p) { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; });

			return (ConvertMethod)(object)(Convert<Guid,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return Guid.Empty;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#region Stream

		private static ConvertMethod GetStreamConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(byte[]))    return (ConvertMethod)(object)(Convert<Stream, byte[]>.ConvertMethod)(delegate(byte[] p) { return p == null ? Stream.Null : new MemoryStream(p); });

			// SqlTypes.
			//
			if (t == typeof(SqlBinary)) return (ConvertMethod)(object)(Convert<Stream, SqlBinary>.ConvertMethod)(delegate(SqlBinary p) { return p.IsNull ? Stream.Null : new MemoryStream(p.Value); });
			if (t == typeof(SqlBytes))  return (ConvertMethod)(object)(Convert<Stream, SqlBytes>.ConvertMethod)(delegate(SqlBytes p) { return p.IsNull ? Stream.Null : p.Stream; });

			return (ConvertMethod)(object)(Convert<Stream, P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return Stream.Null;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#endregion

		#region Nullable Types

		#region SByte?

		private static ConvertMethod GetNullableSByteConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SByte?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (SByte?)Convert.ToSByte(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SByte?,SByte>.ConvertMethod)      (delegate(SByte       p) { return                 p; });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SByte?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToSByte(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SByte?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToSByte(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SByte?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToSByte(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SByte?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SByte?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SByte?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToSByte(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SByte?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToSByte(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SByte?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToSByte(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SByte?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToSByte(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SByte?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToSByte(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SByte?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToSByte(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SByte?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToSByte(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SByte?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToSByte(p); });

			// Nullable Types.
			//
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SByte?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SByte?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SByte?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SByte?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SByte?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SByte?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SByte?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SByte?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SByte?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SByte?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SByte?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SByte?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SByte?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SByte?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SByte?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SByte?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SByte?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SByte?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SByte?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SByte?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SByte?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SByte?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SByte?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SByte?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); });

			return (ConvertMethod)(object)(Convert<SByte?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToSByte(p);
			});
		}

		#endregion

		#region Int16?

		private static ConvertMethod GetNullableInt16Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int16?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Int16?)Convert.ToInt16(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int16?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Int16?,Int16>.ConvertMethod)      (delegate(Int16       p) { return                 p;  });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Int16?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Int16?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt16(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int16?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int16?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int16?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int16?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt16(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int16?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt16(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int16?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt16(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int16?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt16(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int16?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt16(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int16?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt16(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int16?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt16(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int16?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Int16?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Int16?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int16?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int16?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int16?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int16?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int16?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int16?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int16?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int16?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int16?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int16?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int16?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int16?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int16?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Int16?)                p.Value;  });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int16?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int16?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int16?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int16?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int16?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int16?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int16?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int16?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); });

			return (ConvertMethod)(object)(Convert<Int16?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt16(p);
			});
		}

		#endregion

		#region Int32?

		private static ConvertMethod GetNullableInt32Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int32?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Int32?)Convert.ToInt32(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int32?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Int32?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Int32?,Int32>.ConvertMethod)      (delegate(Int32       p) { return                 p;  });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Int32?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt32(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int32?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int32?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int32?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int32?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt32(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int32?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt32(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int32?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt32(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int32?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt32(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int32?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt32(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int32?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt32(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int32?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt32(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int32?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Int32?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Int32?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int32?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int32?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Int32?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int32?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int32?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int32?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int32?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int32?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int32?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int32?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int32?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int32?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int32?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int32?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Int32?)                p.Value;  });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int32?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int32?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int32?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int32?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int32?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int32?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int32?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); });

			return (ConvertMethod)(object)(Convert<Int32?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt32(p);
			});
		}

		#endregion

		#region Int64?

		private static ConvertMethod GetNullableInt64Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Int64?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Int64?)Convert.ToInt64(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Int64?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Int64?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Int64?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Int64?,Int64>.ConvertMethod)      (delegate(Int64       p) { return                 p;  });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Int64?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Int64?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Int64?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Int64?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt64(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Int64?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt64(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Int64?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt64(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Int64?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt64(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Int64?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt64(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Int64?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt64(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Int64?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt64(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Int64?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Int64?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Int64?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Int64?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Int64?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Int64?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Int64?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Int64?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Int64?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Int64?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Int64?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Int64?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Int64?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Int64?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Int64?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Int64?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Int64?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Int64?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Int64?)                p.Value;  });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Int64?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Int64?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Int64?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Int64?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Int64?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Int64?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); });

			return (ConvertMethod)(object)(Convert<Int64?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt64(p);
			});
		}

		#endregion

		#region Byte?

		private static ConvertMethod GetNullableByteConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Byte?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Byte?)Convert.ToByte(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Byte?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToByte(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Byte?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToByte(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Byte?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToByte(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Byte?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToByte(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Byte?,Byte>.ConvertMethod)       (delegate(Byte        p) { return                p;  });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Byte?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Byte?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Byte?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToByte(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Byte?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToByte(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Byte?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToByte(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Byte?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToByte(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Byte?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToByte(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Byte?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToByte(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Byte?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToByte(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Byte?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Byte?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Byte?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Byte?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });

			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Byte?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Byte?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Byte?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Byte?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Byte?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Byte?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Byte?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Byte?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Byte?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Byte?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Byte?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Byte?)               p.Value;  });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Byte?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Byte?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Byte?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Byte?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Byte?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Byte?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Byte?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Byte?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Byte?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); });

			return (ConvertMethod)(object)(Convert<Byte?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToByte(p);
			});
		}

		#endregion

		#region UInt16?

		private static ConvertMethod GetNullableUInt16Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt16?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (UInt16?)Convert.ToUInt16(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt16?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt16?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt16?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt16(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt16?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt16(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt16?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt16(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<UInt16?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToUInt16(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<UInt16?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return                  p; });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<UInt16?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToUInt16(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt16?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt16(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt16?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt16(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt16?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt16(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt16?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt16(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt16?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt16(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt16?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt16(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt16?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt16?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt16?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt16?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt16?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<UInt16?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<UInt16?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt16?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt16?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt16?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt16?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt16?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt16?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt16?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt16?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt16?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt16?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt16?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt16?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt16?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt16?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt16?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt16?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt16?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt16?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt16(p);
			});
		}

		#endregion

		#region UInt32?

		private static ConvertMethod GetNullableUInt32Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt32?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (UInt32?)Convert.ToUInt32(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt32?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt32?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt32?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt32(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt32?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt32(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt32?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt32(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<UInt32?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToUInt32(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<UInt32?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return                  p;  });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<UInt32?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToUInt32(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt32?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt32(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt32?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt32(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt32?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt32(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt32?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt32(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt32?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt32(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt32?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt32(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt32?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt32?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt32?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt32?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt32?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<UInt32?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<UInt32?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt32?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt32?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt32?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt32?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt32?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt32?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt32?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt32?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt32?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt32?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt32?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt32?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt32?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt32?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt32?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt32?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt32?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt32?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt32(p);
			});
		}

		#endregion

		#region UInt64?

		private static ConvertMethod GetNullableUInt64Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<UInt64?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (UInt64?)Convert.ToUInt64(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<UInt64?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<UInt64?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<UInt64?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToUInt64(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<UInt64?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToUInt64(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<UInt64?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToUInt64(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<UInt64?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToUInt64(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<UInt64?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToUInt64(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<UInt64?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return                  p;  });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<UInt64?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToUInt64(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<UInt64?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToUInt64(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<UInt64?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToUInt64(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<UInt64?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToUInt64(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<UInt64?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToUInt64(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<UInt64?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToUInt64(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<UInt64?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<UInt64?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<UInt64?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<UInt64?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<UInt64?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<UInt64?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<UInt64?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<UInt64?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<UInt64?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<UInt64?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<UInt64?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<UInt64?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<UInt64?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<UInt64?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<UInt64?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<UInt64?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<UInt64?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<UInt64?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<UInt64?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<UInt64?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<UInt64?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<UInt64?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<UInt64?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<UInt64?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); });

			return (ConvertMethod)(object)(Convert<UInt64?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToUInt64(p);
			});
		}

		#endregion

		#region Char?

		private static ConvertMethod GetNullableCharConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Char?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Char?)Convert.ToChar(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Char?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToChar(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Char?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToChar(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Char?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToChar(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Char?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToChar(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Char?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToChar(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Char?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToChar(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Char?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToChar(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Char?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToChar(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Char?,Char>.ConvertMethod)       (delegate(Char        p) { return                p;  });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Char?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToChar(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Char?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToChar(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Char?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToChar(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Char?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToChar(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Char?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToChar(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Char?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Char?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Char?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Char?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Char?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Char?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Char?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Char?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });

			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Char?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Char?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Char?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Char?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Char?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Char?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Char?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Char?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Char?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Char?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Char?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Char?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Char?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Char?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Char?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Char?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); });

			return (ConvertMethod)(object)(Convert<Char?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToChar(p);
			});
		}

		#endregion

		#region Single?

		private static ConvertMethod GetNullableSingleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Single?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Single?)Convert.ToSingle(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Single?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Single?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Single?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Single?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToSingle(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Single?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Single?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Single?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Single?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToSingle(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Single?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToSingle(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Single?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToSingle(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Single?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToSingle(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Single?,Single>.ConvertMethod)     (delegate(Single      p) { return                  p;  });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Single?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToSingle(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Single?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToSingle(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Single?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Single?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Single?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Single?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Single?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Single?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Single?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Single?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Single?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Single?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Single?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Single?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Single?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Single?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Single?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Single?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Single?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Single?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Single?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Single?)                 p.Value;  });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Single?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Single?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Single?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Single?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Single?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); });

			return (ConvertMethod)(object)(Convert<Single?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToSingle(p);
			});
		}

		#endregion

		#region Double?

		private static ConvertMethod GetNullableDoubleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Double?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Double?)Convert.ToDouble(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Double?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Double?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Double?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Double?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDouble(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Double?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Double?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Double?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Double?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDouble(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Double?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDouble(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Double?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDouble(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Double?,Double>.ConvertMethod)     (delegate(Double      p) { return                  p;  });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Double?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDouble(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Double?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDouble(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Double?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDouble(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Double?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Double?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Double?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Double?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Double?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Double?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Double?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Double?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Double?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Double?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Double?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Double?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Double?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Double?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Double?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Double?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Double?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Double?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Double?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Double?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Double?)                 p.Value;  });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Double?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Double?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Double?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Double?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); });

			return (ConvertMethod)(object)(Convert<Double?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDouble(p);
			});
		}

		#endregion

		#region Boolean?

		private static ConvertMethod GetNullableBooleanConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Boolean?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Boolean?)Convert.ToBoolean(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Boolean?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Boolean?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Boolean?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Boolean?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToBoolean(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Boolean?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Boolean?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Boolean?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Boolean?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Boolean?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToBoolean(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Boolean?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToBoolean(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Boolean?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Boolean?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return                   p;  });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<Boolean?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToBoolean(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Boolean?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToBoolean(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Boolean?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Boolean?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Boolean?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Boolean?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Boolean?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Boolean?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Boolean?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Boolean?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Boolean?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Boolean?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Boolean?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });

			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Boolean?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Boolean?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Boolean?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Boolean?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Boolean?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Boolean?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Boolean?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Boolean?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Boolean?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Boolean?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Boolean?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Boolean?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Boolean?)                  p.Value;  });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Boolean?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); });

			return (ConvertMethod)(object)(Convert<Boolean?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToBoolean(p);
			});
		}

		#endregion

		#region Decimal?

		private static ConvertMethod GetNullableDecimalConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Decimal?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (Decimal?)Convert.ToDecimal(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<Decimal?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<Decimal?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<Decimal?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<Decimal?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDecimal(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<Decimal?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<Decimal?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<Decimal?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<Decimal?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<Decimal?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDecimal(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<Decimal?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDecimal(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<Decimal?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<Decimal?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDecimal(p); });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<Decimal?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return                   p;  });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<Decimal?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDecimal(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<Decimal?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<Decimal?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<Decimal?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<Decimal?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<Decimal?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<Decimal?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<Decimal?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<Decimal?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<Decimal?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<Decimal?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<Decimal?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<Decimal?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<Decimal?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Decimal?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<Decimal?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<Decimal?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<Decimal?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<Decimal?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<Decimal?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<Decimal?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<Decimal?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (Decimal?)                  p.Value;  });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<Decimal?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (Decimal?)                  p.Value;  });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<Decimal?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<Decimal?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); });

			return (ConvertMethod)(object)(Convert<Decimal?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDecimal(p);
			});
		}

		#endregion

		#region DateTime?

		private static ConvertMethod GetNullableDateTimeConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<DateTime?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (DateTime?)Convert.ToDateTime(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<DateTime?,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<DateTime?,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<DateTime?,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<DateTime?,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDateTime(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<DateTime?,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<DateTime?,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<DateTime?,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<DateTime?,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<DateTime?,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDateTime(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<DateTime?,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDateTime(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<DateTime?,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<DateTime?,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDateTime(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<DateTime?,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDateTime(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<DateTime?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return                    p;  });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<DateTime?,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<DateTime?,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<DateTime?,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<DateTime?,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<DateTime?,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<DateTime?,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<DateTime?,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<DateTime?,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<DateTime?,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<DateTime?,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<DateTime?,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<DateTime?,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<DateTime?,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<DateTime?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<DateTime?,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<DateTime?,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<DateTime?,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<DateTime?,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<DateTime?,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<DateTime?,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<DateTime?,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<DateTime?,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<DateTime?,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<DateTime?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (DateTime?)                   p.Value;  });

			return (ConvertMethod)(object)(Convert<DateTime?,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDateTime(p);
			});
		}

		#endregion

		#region TimeSpan?

		private static ConvertMethod GetNullableTimeSpanConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<TimeSpan?,String>.ConvertMethod)     (delegate(String      p) { return p == null? null: (TimeSpan?)TimeSpan.Parse(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<TimeSpan?,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return p - DateTime.MinValue; });
			if (t == typeof(TimeSpan))    return (ConvertMethod)(object)(Convert<TimeSpan?,TimeSpan>.ConvertMethod)   (delegate(TimeSpan    p) { return p; });

			// Nullable Types.
			//
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<TimeSpan?,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (TimeSpan?)(p.Value - DateTime.MinValue) : null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<TimeSpan?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (TimeSpan?)TimeSpan.Parse(p.Value);       });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<TimeSpan?,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? null: (TimeSpan?)(p.Value - DateTime.MinValue); });

			return (ConvertMethod)(object)(Convert<TimeSpan?,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return null;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#region Guid?

		private static ConvertMethod GetNullableGuidConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<Guid?,String>.ConvertMethod)     (delegate(String      p) { return p == null ? null : (Guid?)new Guid(p); });
			if (t == typeof(Guid))        return (ConvertMethod)(object)(Convert<Guid?,Guid>.ConvertMethod)       (delegate(Guid        p) { return p; });

			// Nullable Types.
			//

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<Guid?,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.IsNull? null: (Guid?)new Guid(p.Value); });
			if (t == typeof(SqlGuid))     return (ConvertMethod)(object)(Convert<Guid?,SqlGuid>.ConvertMethod)    (delegate(SqlGuid     p) { return p.IsNull? null: (Guid?)p.Value; });
			if (t == typeof(SqlBinary))   return (ConvertMethod)(object)(Convert<Guid?,SqlBinary>.ConvertMethod)  (delegate(SqlBinary   p) { return p.IsNull? null: (Guid?)p.ToSqlGuid().Value; });

			return (ConvertMethod)(object)(Convert<Guid?,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return null;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#endregion

		#region SQL Types

		#region SqlString

		private static ConvertMethod GetSqlStringConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlString,String>.ConvertMethod)     (delegate(String      p) { return p == null? SqlString.Null: (SqlString)p.ToString(); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlString,SByte>.ConvertMethod)      (delegate(SByte       p) { return p.ToString(); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlString,Int16>.ConvertMethod)      (delegate(Int16       p) { return p.ToString(); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlString,Int32>.ConvertMethod)      (delegate(Int32       p) { return p.ToString(); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlString,Int64>.ConvertMethod)      (delegate(Int64       p) { return p.ToString(); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlString,Byte>.ConvertMethod)       (delegate(Byte        p) { return p.ToString(); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlString,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return p.ToString(); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlString,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return p.ToString(); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlString,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return p.ToString(); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlString,Char>.ConvertMethod)       (delegate(Char        p) { return p.ToString(); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlString,Single>.ConvertMethod)     (delegate(Single      p) { return p.ToString(); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlString,Double>.ConvertMethod)     (delegate(Double      p) { return p.ToString(); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlString,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return p.ToString(); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlString,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return p.ToString(); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlString,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return p.ToString(); });
			if (t == typeof(TimeSpan))    return (ConvertMethod)(object)(Convert<SqlString,TimeSpan>.ConvertMethod)   (delegate(TimeSpan    p) { return p.ToString(); });
			if (t == typeof(Guid))        return (ConvertMethod)(object)(Convert<SqlString,Guid>.ConvertMethod)       (delegate(Guid        p) { return p.ToString(); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlString,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlString,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlString,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlString,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlString,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlString,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlString,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlString,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlString,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlString,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlString,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlString,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlString,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlString,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(TimeSpan?))   return (ConvertMethod)(object)(Convert<SqlString,TimeSpan?>.ConvertMethod)  (delegate(TimeSpan?   p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });
			if (t == typeof(Guid?))       return (ConvertMethod)(object)(Convert<SqlString,Guid?>.ConvertMethod)      (delegate(Guid?       p) { return p.HasValue? (SqlString)p.ToString(): SqlString.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlString,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlString(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlString,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlString(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlString,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlString(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlString,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlString(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlString,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlString(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlString,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlString(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlString,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlString(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlString,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlString(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlString,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlString(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlString,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.ToSqlString(); });
			if (t == typeof(SqlGuid))     return (ConvertMethod)(object)(Convert<SqlString,SqlGuid>.ConvertMethod)    (delegate(SqlGuid     p) { return p.ToSqlString(); });
			if (t == typeof(SqlBinary))   return (ConvertMethod)(object)(Convert<SqlString,SqlBinary>.ConvertMethod)  (delegate(SqlBinary   p) { return p.IsNull? SqlString.Null: (SqlString)p.ToString(); });

			return (ConvertMethod)(object)(Convert<SqlString,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToString(p);
			});
		}

		#endregion

		#region SqlByte

		private static ConvertMethod GetSqlByteConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlByte,String>.ConvertMethod)     (delegate(String      p) { return SqlByte.Parse(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlByte,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToByte(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlByte,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToByte(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlByte,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToByte(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlByte,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToByte(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlByte,Byte>.ConvertMethod)       (delegate(Byte        p) { return                p; });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlByte,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlByte,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToByte(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlByte,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToByte(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlByte,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToByte(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlByte,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToByte(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlByte,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToByte(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlByte,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToByte(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlByte,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToByte(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlByte,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToByte(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlByte,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlByte,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlByte,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlByte,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlByte,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue?                p.Value  : SqlByte.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlByte,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlByte,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlByte,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlByte,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlByte,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlByte,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlByte,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlByte,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlByte,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlByte,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlByte(); });

			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlByte,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlByte(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlByte,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlByte(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlByte,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlByte(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlByte,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlByte(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlByte,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlByte(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlByte,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlByte(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlByte,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlByte(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlByte,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlByte(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlByte,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlByte.Null: Convert.ToByte(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlByte,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToByte(p);
			});
		}

		#endregion

		#region SqlInt16

		private static ConvertMethod GetSqlInt16Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlInt16,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt16(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlInt16,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlInt16,Int16>.ConvertMethod)      (delegate(Int16       p) { return                 p; });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlInt16,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt16(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlInt16,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt16(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlInt16,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlInt16,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlInt16,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt16(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlInt16,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt16(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlInt16,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt16(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlInt16,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt16(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlInt16,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt16(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlInt16,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt16(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlInt16,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt16(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlInt16,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt16(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlInt16,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlInt16,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue?                 p.Value  : SqlInt16.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlInt16,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlInt16,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlInt16,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlInt16,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlInt16,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlInt16,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlInt16,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlInt16,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlInt16,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlInt16,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlInt16,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlInt16,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlInt16,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlInt16(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlInt16,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlInt16,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlInt16,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlInt16(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlInt16,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlInt16,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlInt16,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlInt16,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlInt16(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlInt16,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlInt16(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlInt16,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlInt16.Null: Convert.ToInt16(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlInt16,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt16(p);
			});
		}

		#endregion

		#region SqlInt32

		private static ConvertMethod GetSqlInt32Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlInt32,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt32(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlInt32,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlInt32,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt32(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlInt32,Int32>.ConvertMethod)      (delegate(Int32       p) { return                 p;  });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlInt32,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToInt32(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlInt32,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlInt32,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlInt32,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlInt32,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt32(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlInt32,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt32(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlInt32,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt32(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlInt32,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt32(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlInt32,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt32(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlInt32,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt32(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlInt32,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt32(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlInt32,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlInt32,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlInt32,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue?                 p.Value  : SqlInt32.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlInt32,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlInt32,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlInt32,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlInt32,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlInt32,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlInt32,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlInt32,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlInt32,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlInt32,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlInt32,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlInt32,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlInt32,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlInt32(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlInt32,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlInt32,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlInt32,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlInt32(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlInt32,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlInt32,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlInt32,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlInt32,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlInt32(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlInt32,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlInt32(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlInt32,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlInt32.Null: Convert.ToInt32(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlInt32,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt32(p);
			});
		}

		#endregion

		#region SqlInt64

		private static ConvertMethod GetSqlInt64Converter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlInt64,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToInt64(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlInt64,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlInt64,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlInt64,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToInt64(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlInt64,Int64>.ConvertMethod)      (delegate(Int64       p) { return                 p;  });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlInt64,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlInt64,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlInt64,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToInt64(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlInt64,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToInt64(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlInt64,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToInt64(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlInt64,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToInt64(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlInt64,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToInt64(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlInt64,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToInt64(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlInt64,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToInt64(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlInt64,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToInt64(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlInt64,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlInt64,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlInt64,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlInt64,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue?                 p.Value  : SqlInt64.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlInt64,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlInt64,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlInt64,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlInt64,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlInt64,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlInt64,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlInt64,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlInt64,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlInt64,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlInt64,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlInt64,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlInt64(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlInt64,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlInt64,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlInt64,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlInt64(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlInt64,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlInt64,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlInt64,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlInt64,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlInt64(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlInt64,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlInt64(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlInt64,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlInt64.Null: Convert.ToInt64(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlInt64,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToInt64(p);
			});
		}

		#endregion

		#region SqlSingle

		private static ConvertMethod GetSqlSingleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlSingle,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToSingle(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlSingle,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlSingle,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlSingle,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToSingle(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlSingle,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToSingle(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlSingle,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlSingle,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlSingle,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToSingle(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlSingle,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToSingle(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlSingle,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToSingle(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlSingle,Single>.ConvertMethod)     (delegate(Single      p) { return                  p;  });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlSingle,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToSingle(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlSingle,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToSingle(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlSingle,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToSingle(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlSingle,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToSingle(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlSingle,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlSingle,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlSingle,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlSingle,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlSingle,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlSingle,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlSingle,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlSingle,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlSingle,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlSingle,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue?                  p.Value  : SqlSingle.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlSingle,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlSingle,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlSingle,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlSingle,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlSingle,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlSingle(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlSingle,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlSingle,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlSingle,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlSingle,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlSingle(); });

			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlSingle,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlSingle,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlSingle,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlSingle(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlSingle,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlSingle(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlSingle,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlSingle.Null: Convert.ToSingle(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlSingle,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToSingle(p);
			});
		}

		#endregion

		#region SqlDouble

		private static ConvertMethod GetSqlDoubleConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlDouble,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDouble(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlDouble,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlDouble,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlDouble,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDouble(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlDouble,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDouble(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlDouble,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlDouble,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlDouble,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDouble(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlDouble,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDouble(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlDouble,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDouble(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlDouble,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDouble(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlDouble,Double>.ConvertMethod)     (delegate(Double      p) { return                  p;  });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlDouble,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDouble(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlDouble,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDouble(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlDouble,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDouble(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlDouble,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlDouble,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlDouble,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlDouble,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlDouble,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlDouble,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlDouble,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlDouble,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlDouble,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlDouble,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlDouble,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue?                  p.Value  : SqlDouble.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlDouble,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlDouble,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlDouble,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlDouble,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlDouble(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlDouble,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlDouble,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlDouble,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlDouble,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlDouble(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlDouble,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlDouble,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlDouble,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlDouble(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlDouble,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlDouble(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlDouble,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlDouble.Null: Convert.ToDouble(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlDouble,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDouble(p);
			});
		}

		#endregion

		#region SqlDecimal

		private static ConvertMethod GetSqlDecimalConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlDecimal,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDecimal(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlDecimal,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlDecimal,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlDecimal,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlDecimal,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDecimal(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlDecimal,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlDecimal,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlDecimal,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlDecimal,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlDecimal,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDecimal(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlDecimal,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDecimal(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlDecimal,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlDecimal,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDecimal(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlDecimal,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return                   p;  });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlDecimal,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDecimal(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlDecimal,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlDecimal,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlDecimal,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlDecimal,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlDecimal,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlDecimal,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlDecimal,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlDecimal,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlDecimal,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlDecimal,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlDecimal,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlDecimal,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlDecimal,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue?                   p.Value  : SqlDecimal.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlDecimal,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlDecimal,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlDecimal(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlDecimal,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlDecimal,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlDecimal,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlDecimal,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlDecimal(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlDecimal,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlDecimal,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlDecimal,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlDecimal(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlDecimal,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlDecimal(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlDecimal,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlDecimal.Null: Convert.ToDecimal(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlDecimal,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDecimal(p);
			});
		}

		#endregion

		#region SqlMoney

		private static ConvertMethod GetSqlMoneyConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlMoney,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDecimal(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlMoney,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlMoney,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlMoney,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDecimal(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlMoney,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDecimal(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlMoney,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlMoney,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlMoney,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDecimal(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlMoney,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlMoney,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDecimal(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlMoney,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDecimal(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlMoney,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDecimal(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlMoney,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDecimal(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlMoney,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return                   p;  });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlMoney,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToDecimal(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlMoney,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlMoney,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlMoney,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlMoney,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlMoney,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlMoney,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlMoney,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlMoney,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlMoney,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlMoney,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlMoney,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlMoney,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlMoney,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue?                   p.Value  : SqlMoney.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlMoney,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlMoney,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlMoney(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlMoney,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlMoney,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlMoney,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlMoney,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlMoney(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlMoney,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlMoney,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlMoney,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlMoney(); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlMoney,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.ToSqlMoney(); });
			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlMoney,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlMoney.Null: Convert.ToDecimal(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlMoney,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDecimal(p);
			});
		}

		#endregion

		#region SqlBoolean

		private static ConvertMethod GetSqlBooleanConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlBoolean,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToBoolean(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlBoolean,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlBoolean,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlBoolean,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToBoolean(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlBoolean,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToBoolean(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlBoolean,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlBoolean,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlBoolean,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToBoolean(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlBoolean,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlBoolean,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToBoolean(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlBoolean,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToBoolean(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlBoolean,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToBoolean(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlBoolean,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return                   p;  });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlBoolean,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToBoolean(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlBoolean,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return Convert.ToBoolean(p); });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlBoolean,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlBoolean,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlBoolean,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlBoolean,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlBoolean,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlBoolean,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlBoolean,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlBoolean,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlBoolean,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlBoolean,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlBoolean,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlBoolean,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue?                   p.Value  : SqlBoolean.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlBoolean,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlBoolean,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlBoolean,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlBoolean(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlBoolean,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlBoolean,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlBoolean,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlBoolean,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.ToSqlBoolean(); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlBoolean,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlBoolean,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlBoolean,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.ToSqlBoolean(); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlBoolean,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.ToSqlBoolean(); });

			if (t == typeof(SqlDateTime)) return (ConvertMethod)(object)(Convert<SqlBoolean,SqlDateTime>.ConvertMethod)(delegate(SqlDateTime p) { return p.IsNull? SqlBoolean.Null: Convert.ToBoolean(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlBoolean,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToBoolean(p);
			});
		}

		#endregion

		#region SqlDateTime

		private static ConvertMethod GetSqlDateTimeConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlDateTime,String>.ConvertMethod)     (delegate(String      p) { return Convert.ToDateTime(p); });

			if (t == typeof(SByte))       return (ConvertMethod)(object)(Convert<SqlDateTime,SByte>.ConvertMethod)      (delegate(SByte       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int16))       return (ConvertMethod)(object)(Convert<SqlDateTime,Int16>.ConvertMethod)      (delegate(Int16       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int32))       return (ConvertMethod)(object)(Convert<SqlDateTime,Int32>.ConvertMethod)      (delegate(Int32       p) { return Convert.ToDateTime(p); });
			if (t == typeof(Int64))       return (ConvertMethod)(object)(Convert<SqlDateTime,Int64>.ConvertMethod)      (delegate(Int64       p) { return Convert.ToDateTime(p); });

			if (t == typeof(Byte))        return (ConvertMethod)(object)(Convert<SqlDateTime,Byte>.ConvertMethod)       (delegate(Byte        p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt16))      return (ConvertMethod)(object)(Convert<SqlDateTime,UInt16>.ConvertMethod)     (delegate(UInt16      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt32))      return (ConvertMethod)(object)(Convert<SqlDateTime,UInt32>.ConvertMethod)     (delegate(UInt32      p) { return Convert.ToDateTime(p); });
			if (t == typeof(UInt64))      return (ConvertMethod)(object)(Convert<SqlDateTime,UInt64>.ConvertMethod)     (delegate(UInt64      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Char))        return (ConvertMethod)(object)(Convert<SqlDateTime,Char>.ConvertMethod)       (delegate(Char        p) { return Convert.ToDateTime(p); });
			if (t == typeof(Single))      return (ConvertMethod)(object)(Convert<SqlDateTime,Single>.ConvertMethod)     (delegate(Single      p) { return Convert.ToDateTime(p); });
			if (t == typeof(Double))      return (ConvertMethod)(object)(Convert<SqlDateTime,Double>.ConvertMethod)     (delegate(Double      p) { return Convert.ToDateTime(p); });

			if (t == typeof(Boolean))     return (ConvertMethod)(object)(Convert<SqlDateTime,Boolean>.ConvertMethod)    (delegate(Boolean     p) { return Convert.ToDateTime(p); });
			if (t == typeof(Decimal))     return (ConvertMethod)(object)(Convert<SqlDateTime,Decimal>.ConvertMethod)    (delegate(Decimal     p) { return Convert.ToDateTime(p); });
			if (t == typeof(DateTime))    return (ConvertMethod)(object)(Convert<SqlDateTime,DateTime>.ConvertMethod)   (delegate(DateTime    p) { return                    p;  });

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (ConvertMethod)(object)(Convert<SqlDateTime,SByte?>.ConvertMethod)     (delegate(SByte?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Int16?))      return (ConvertMethod)(object)(Convert<SqlDateTime,Int16?>.ConvertMethod)     (delegate(Int16?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Int32?))      return (ConvertMethod)(object)(Convert<SqlDateTime,Int32?>.ConvertMethod)     (delegate(Int32?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Int64?))      return (ConvertMethod)(object)(Convert<SqlDateTime,Int64?>.ConvertMethod)     (delegate(Int64?      p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });

			if (t == typeof(Byte?))       return (ConvertMethod)(object)(Convert<SqlDateTime,Byte?>.ConvertMethod)      (delegate(Byte?       p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(UInt16?))     return (ConvertMethod)(object)(Convert<SqlDateTime,UInt16?>.ConvertMethod)    (delegate(UInt16?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(UInt32?))     return (ConvertMethod)(object)(Convert<SqlDateTime,UInt32?>.ConvertMethod)    (delegate(UInt32?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(UInt64?))     return (ConvertMethod)(object)(Convert<SqlDateTime,UInt64?>.ConvertMethod)    (delegate(UInt64?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });

			if (t == typeof(Char?))       return (ConvertMethod)(object)(Convert<SqlDateTime,Char?>.ConvertMethod)      (delegate(Char?       p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Single?))     return (ConvertMethod)(object)(Convert<SqlDateTime,Single?>.ConvertMethod)    (delegate(Single?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Double?))     return (ConvertMethod)(object)(Convert<SqlDateTime,Double?>.ConvertMethod)    (delegate(Double?     p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });

			if (t == typeof(Boolean?))    return (ConvertMethod)(object)(Convert<SqlDateTime,Boolean?>.ConvertMethod)   (delegate(Boolean?    p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(Decimal?))    return (ConvertMethod)(object)(Convert<SqlDateTime,Decimal?>.ConvertMethod)   (delegate(Decimal?    p) { return p.HasValue? Convert.ToDateTime(p.Value) : SqlDateTime.Null; });
			if (t == typeof(DateTime?))   return (ConvertMethod)(object)(Convert<SqlDateTime,DateTime?>.ConvertMethod)  (delegate(DateTime?   p) { return p.HasValue?                    p.Value  : SqlDateTime.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlDateTime,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlDateTime(); });

			if (t == typeof(SqlByte))     return (ConvertMethod)(object)(Convert<SqlDateTime,SqlByte>.ConvertMethod)    (delegate(SqlByte     p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt16))    return (ConvertMethod)(object)(Convert<SqlDateTime,SqlInt16>.ConvertMethod)   (delegate(SqlInt16    p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt32))    return (ConvertMethod)(object)(Convert<SqlDateTime,SqlInt32>.ConvertMethod)   (delegate(SqlInt32    p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlInt64))    return (ConvertMethod)(object)(Convert<SqlDateTime,SqlInt64>.ConvertMethod)   (delegate(SqlInt64    p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlSingle))   return (ConvertMethod)(object)(Convert<SqlDateTime,SqlSingle>.ConvertMethod)  (delegate(SqlSingle   p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDouble))   return (ConvertMethod)(object)(Convert<SqlDateTime,SqlDouble>.ConvertMethod)  (delegate(SqlDouble   p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlDecimal))  return (ConvertMethod)(object)(Convert<SqlDateTime,SqlDecimal>.ConvertMethod) (delegate(SqlDecimal  p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });
			if (t == typeof(SqlMoney))    return (ConvertMethod)(object)(Convert<SqlDateTime,SqlMoney>.ConvertMethod)   (delegate(SqlMoney    p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });

			if (t == typeof(SqlBoolean))  return (ConvertMethod)(object)(Convert<SqlDateTime,SqlBoolean>.ConvertMethod) (delegate(SqlBoolean  p) { return p.IsNull? SqlDateTime.Null: Convert.ToDateTime(p.Value); });

			return (ConvertMethod)(object)(Convert<SqlDateTime,P>.ConvertMethod)(delegate(P p)
			{
				return Convert.ToDateTime(p);
			});
		}

		#endregion

		#region SqlGuid

		private static ConvertMethod GetSqlGuidConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(String))      return (ConvertMethod)(object)(Convert<SqlGuid,String>.ConvertMethod)     (delegate(String      p) { return new SqlGuid(p); });
			if (t == typeof(Guid))        return (ConvertMethod)(object)(Convert<SqlGuid,Guid>.ConvertMethod)       (delegate(Guid        p) { return p; });

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (ConvertMethod)(object)(Convert<SqlGuid,Guid?>.ConvertMethod)      (delegate(Guid?       p) { return p.HasValue? p.Value : SqlGuid.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (ConvertMethod)(object)(Convert<SqlGuid,SqlString>.ConvertMethod)  (delegate(SqlString   p) { return p.ToSqlGuid(); });
			if (t == typeof(SqlBinary))   return (ConvertMethod)(object)(Convert<SqlGuid,SqlBinary>.ConvertMethod)  (delegate(SqlBinary   p) { return p.ToSqlGuid(); });

			return (ConvertMethod)(object)(Convert<SqlGuid,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return SqlGuid.Null;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#region SqlBinary

		private static ConvertMethod GetSqlBinaryConverter()
		{
			Type t = typeof(P);

			// Scalar Types.
			//
			if (t == typeof(Guid))        return (ConvertMethod)(object)(Convert<SqlBinary,Guid>.ConvertMethod)       (delegate(Guid        p) { return new SqlGuid(p).ToSqlBinary(); });

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (ConvertMethod)(object)(Convert<SqlBinary,Guid?>.ConvertMethod)      (delegate(Guid?       p) { return p.HasValue? new SqlGuid(p.Value).ToSqlBinary() : SqlBinary.Null; });

			// SqlTypes.
			//
			if (t == typeof(SqlGuid))     return (ConvertMethod)(object)(Convert<SqlBinary,SqlGuid>.ConvertMethod)    (delegate(SqlGuid     p) { return p.ToSqlBinary(); });

			return (ConvertMethod)(object)(Convert<SqlBinary,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return SqlBinary.Null;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion

		#endregion
	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T,P>.From(p);
		}
	}
}
