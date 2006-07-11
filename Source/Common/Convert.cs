using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using BLToolkit.Reflection;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		public abstract class CB<Q,V>
		{
			public abstract Q C(V p);
		}

		public static T From(P p)
		{
			return I.C(p);
		}

		sealed class Assignable<Q> : CB<Q,Q> { public override Q C(Q p) { return p; } }
		sealed class Default<Q,V>  : CB<Q,V> { public override Q C(V p) { return (Q)Convert.ChangeType(p, typeof(Q)); } }

		public static readonly CB<T,P> I = GetConverter();
		static CB<T,P> GetConverter()
		{
			Type t = typeof(T);

			// Convert to the same type.
			//
			if (TypeHelper.IsSameOrParent(t, typeof(P)))
				return (CB<T,P>)(object)(new Assignable<T>());

			// Scalar Types
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

			// Nullable Types
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

			// SqlTypes
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
			if (t == typeof(SqlBytes))    return GetSqlBytesConverter();
			if (t == typeof(SqlChars))    return GetSqlCharsConverter();
			if (t == typeof(SqlXml))      return GetSqlXmlConverter();

			// Other Types
			//
			if (t == typeof(Type))        return GetTypeConverter();
			if (t == typeof(Stream))      return GetStreamConverter();
			if (t == typeof(XmlReader))   return GetXmlReaderConverter();
			if (t == typeof(Byte[]))      return GetByteArrayConverter();
			if (t == typeof(Char[]))      return GetCharArrayConverter();

			return new Default<T,P>();
		}
	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T,P>.From(p);
		}
	}
}
