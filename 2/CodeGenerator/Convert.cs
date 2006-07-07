using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	public abstract class Convert<T,P>
	{
		public abstract T From(P p);

		sealed class Assignable<Q> : Convert<Q, Q> { public override Q From(Q p) { return p; } }
		sealed class Default<Q,V>  : Convert<Q, V> { public override Q From(V p) { return (Q)Convert.ChangeType(p, typeof(Q)); } }

		public static readonly Convert<T, P> Instance = GetConverter();
		static Convert<T, P> GetConverter()
		{
			Type t = typeof(T);

			// Convert to the same type.
			//
			if (t.IsAssignableFrom(typeof(P)))
				return (Convert<T, P>)(object)(new Assignable<T>());

			if (t == typeof(Type))        return GetTypeConverter();

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
			if (t == typeof(Stream))      return GetStreamConverter();

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

			return new Default<T,P>();
		}

		#region Type


		// Scalar Types.
		//
		sealed class TypeFromString     : Convert<Type,String>     { public override Type From(String p)      { return p == null      ? null: Type.GetType(p);                   } }
		sealed class TypeFromCharArray  : Convert<Type,Char[]>     { public override Type From(Char[] p)      { return p == null      ? null: Type.GetType(new string(p));       } }
		sealed class TypeFromGuid       : Convert<Type,Guid>       { public override Type From(Guid p)        { return p == Guid.Empty? null: Type.GetTypeFromCLSID(p);          } }

		// Nullable Types.
		//
		sealed class TypeFromNullableGuid       : Convert<Type,Guid?>       { public override Type From(Guid? p)        { return p.HasValue? Type.GetTypeFromCLSID(p): null; } }

		// SqlTypes.
		//
		sealed class TypeFromSqlString  : Convert<Type,SqlString>  { public override Type From(SqlString p)   { return p.IsNull       ? null: Type.GetType(p.Value);             } }
		sealed class TypeFromSqlChars   : Convert<Type,SqlChars>   { public override Type From(SqlChars p)    { return p.IsNull       ? null: Type.GetType(new string(p.Value)); } }
		sealed class TypeFromSQlGuid    : Convert<Type,SQlGuid>    { public override Type From(SQlGuid p)     { return p.IsNull       ? null: Type.GetTypeFromCLSID(p.value);    } }

		sealed class TypeDefault<Q>     : Convert<Type,Q>          { public override Type From(Q p)           { return Convert<Type,object>.Instance.From(p); } }
		sealed class TypeFromObject     : Convert<Type,object>     { public override Type From(object p)      {  } }

		static Convert<T, P> GetTypeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new TypeFromString());
			if (t == typeof(Char[]))      return (Convert<T, P>)(object)(new TypeFromCharArray());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new TypeFromGuid());

			// Nullable Types.
			//
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new TypeFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new TypeFromSqlString());
			if (t == typeof(SqlChars))    return (Convert<T, P>)(object)(new TypeFromSqlChars());
			if (t == typeof(SQlGuid))     return (Convert<T, P>)(object)(new TypeFromSQlGuid());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new TypeFromObject());

			return (Convert<T, P>)(object)(new TypeDefault<P>());
		}

		#endregion

		#region Scalar Types

		#region String

		sealed class StringFromType       : Convert<String,Type>       { public override String From(Type p)        { return p == null? null: p.FullName; } }

		// Scalar Types.
		//
		sealed class StringFromSByte      : Convert<String,SByte>      { public override String From(SByte p)       { return p.ToString(); } }
		sealed class StringFromInt16      : Convert<String,Int16>      { public override String From(Int16 p)       { return p.ToString(); } }
		sealed class StringFromInt32      : Convert<String,Int32>      { public override String From(Int32 p)       { return p.ToString(); } }
		sealed class StringFromInt64      : Convert<String,Int64>      { public override String From(Int64 p)       { return p.ToString(); } }

		sealed class StringFromByte       : Convert<String,Byte>       { public override String From(Byte p)        { return p.ToString(); } }
		sealed class StringFromUInt16     : Convert<String,UInt16>     { public override String From(UInt16 p)      { return p.ToString(); } }
		sealed class StringFromUInt32     : Convert<String,UInt32>     { public override String From(UInt32 p)      { return p.ToString(); } }
		sealed class StringFromUInt64     : Convert<String,UInt64>     { public override String From(UInt64 p)      { return p.ToString(); } }

		sealed class StringFromSingle     : Convert<String,Single>     { public override String From(Single p)      { return p.ToString(); } }
		sealed class StringFromDouble     : Convert<String,Double>     { public override String From(Double p)      { return p.ToString(); } }

		sealed class StringFromBoolean    : Convert<String,Boolean>    { public override String From(Boolean p)     { return p.ToString(); } }
		sealed class StringFromDecimal    : Convert<String,Decimal>    { public override String From(Decimal p)     { return p.ToString(); } }
		sealed class StringFromChar       : Convert<String,Char>       { public override String From(Char p)        { return p.ToString(); } }
		sealed class StringFromTimeSpan   : Convert<String,TimeSpan>   { public override String From(TimeSpan p)    { return p.ToString(); } }
		sealed class StringFromDateTime   : Convert<String,DateTime>   { public override String From(DateTime p)    { return p.ToString(); } }
		sealed class StringFromGuid       : Convert<String,Guid>       { public override String From(Guid p)        { return p.ToString(); } }

		// Nullable Types.
		//
		sealed class StringFromNullableSByte      : Convert<String,SByte?>      { public override String From(SByte? p)       { return p.ToString(); } }
		sealed class StringFromNullableInt16      : Convert<String,Int16?>      { public override String From(Int16? p)       { return p.ToString(); } }
		sealed class StringFromNullableInt32      : Convert<String,Int32?>      { public override String From(Int32? p)       { return p.ToString(); } }
		sealed class StringFromNullableInt64      : Convert<String,Int64?>      { public override String From(Int64? p)       { return p.ToString(); } }

		sealed class StringFromNullableByte       : Convert<String,Byte?>       { public override String From(Byte? p)        { return p.ToString(); } }
		sealed class StringFromNullableUInt16     : Convert<String,UInt16?>     { public override String From(UInt16? p)      { return p.ToString(); } }
		sealed class StringFromNullableUInt32     : Convert<String,UInt32?>     { public override String From(UInt32? p)      { return p.ToString(); } }
		sealed class StringFromNullableUInt64     : Convert<String,UInt64?>     { public override String From(UInt64? p)      { return p.ToString(); } }

		sealed class StringFromNullableSingle     : Convert<String,Single?>     { public override String From(Single? p)      { return p.ToString(); } }
		sealed class StringFromNullableDouble     : Convert<String,Double?>     { public override String From(Double? p)      { return p.ToString(); } }

		sealed class StringFromNullableBoolean    : Convert<String,Boolean?>    { public override String From(Boolean? p)     { return p.ToString(); } }
		sealed class StringFromNullableDecimal    : Convert<String,Decimal?>    { public override String From(Decimal? p)     { return p.ToString(); } }
		sealed class StringFromNullableChar       : Convert<String,Char?>       { public override String From(Char? p)        { return p.ToString(); } }
		sealed class StringFromNullableTimeSpan   : Convert<String,TimeSpan?>   { public override String From(TimeSpan? p)    { return p.ToString(); } }
		sealed class StringFromNullableDateTime   : Convert<String,DateTime?>   { public override String From(DateTime? p)    { return p.ToString(); } }
		sealed class StringFromNullableGuid       : Convert<String,Guid?>       { public override String From(Guid? p)        { return p.ToString(); } }

		// SqlTypes.
		//
		sealed class StringFromSqlString  : Convert<String,SqlString>  { public override String From(SqlString p)   { return p.ToString(); } }

		sealed class StringFromSqlByte    : Convert<String,SqlByte>    { public override String From(SqlByte p)     { return p.ToString(); } }
		sealed class StringFromSqlInt16   : Convert<String,SqlInt16>   { public override String From(SqlInt16 p)    { return p.ToString(); } }
		sealed class StringFromSqlInt32   : Convert<String,SqlInt32>   { public override String From(SqlInt32 p)    { return p.ToString(); } }
		sealed class StringFromSqlInt64   : Convert<String,SqlInt64>   { public override String From(SqlInt64 p)    { return p.ToString(); } }

		sealed class StringFromSqlSingle  : Convert<String,SqlSingle>  { public override String From(SqlSingle p)   { return p.ToString(); } }
		sealed class StringFromSqlDouble  : Convert<String,SqlDouble>  { public override String From(SqlDouble p)   { return p.ToString(); } }
		sealed class StringFromSqlDecimal : Convert<String,SqlDecimal> { public override String From(SqlDecimal p)  { return p.ToString(); } }
		sealed class StringFromSqlMoney   : Convert<String,SqlMoney>   { public override String From(SqlMoney p)    { return p.ToString(); } }

		sealed class StringFromSqlBoolean : Convert<String,SqlBoolean> { public override String From(SqlBoolean p)  { return p.ToString(); } }
		sealed class StringFromSqlGuid    : Convert<String,SqlGuid>    { public override String From(SqlGuid p)     { return p.ToString(); } }
		sealed class StringFromSqlBinary  : Convert<String,SqlBinary>  { public override String From(SqlBinary p)   { return p.ToString(); } }

		sealed class StringDefault<Q>     : Convert<String,Q>          { public override String From(Q p)           { return Convert<String,object>.Instance.From(p); } }
		sealed class StringFromObject     : Convert<String,object>     { public override String From(object p)      { return Convert.ToString(p); } }

		static Convert<T, P> GetStringConverter()
		{
			Type t = typeof(P);

			if (t == typeof(Type))        return (Convert<T, P>)(object)(new StringFromType());

			// Scalar Types.
			//
			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new StringFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new StringFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new StringFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new StringFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new StringFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new StringFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new StringFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new StringFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new StringFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new StringFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new StringFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new StringFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new StringFromChar());
			if (t == typeof(TimeSpan))    return (Convert<T, P>)(object)(new StringFromTimeSpan());
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new StringFromDateTime());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new StringFromGuid());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new StringFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new StringFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new StringFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new StringFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new StringFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new StringFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new StringFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new StringFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new StringFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new StringFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new StringFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new StringFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new StringFromNullableChar());
			if (t == typeof(TimeSpan?))    return (Convert<T, P>)(object)(new StringFromNullableTimeSpan());
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new StringFromNullableDateTime());
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new StringFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new StringFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new StringFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new StringFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new StringFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new StringFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new StringFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new StringFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new StringFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new StringFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new StringFromSqlBoolean());
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new StringFromSqlGuid());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new StringFromSqlBinary());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new StringFromObject());

			return (Convert<T, P>)(object)(new StringDefault<P>());
		}

		#endregion

		#region SByte


		// Scalar Types.
		//
		sealed class SByteFromString     : Convert<SByte,String>     { public override SByte From(String p)      { return Convert.ToSByte(p); } }

		sealed class SByteFromInt16      : Convert<SByte,Int16>      { public override SByte From(Int16 p)       { return Convert.ToSByte(p); } }
		sealed class SByteFromInt32      : Convert<SByte,Int32>      { public override SByte From(Int32 p)       { return Convert.ToSByte(p); } }
		sealed class SByteFromInt64      : Convert<SByte,Int64>      { public override SByte From(Int64 p)       { return Convert.ToSByte(p); } }

		sealed class SByteFromByte       : Convert<SByte,Byte>       { public override SByte From(Byte p)        { return Convert.ToSByte(p); } }
		sealed class SByteFromUInt16     : Convert<SByte,UInt16>     { public override SByte From(UInt16 p)      { return Convert.ToSByte(p); } }
		sealed class SByteFromUInt32     : Convert<SByte,UInt32>     { public override SByte From(UInt32 p)      { return Convert.ToSByte(p); } }
		sealed class SByteFromUInt64     : Convert<SByte,UInt64>     { public override SByte From(UInt64 p)      { return Convert.ToSByte(p); } }

		sealed class SByteFromSingle     : Convert<SByte,Single>     { public override SByte From(Single p)      { return Convert.ToSByte(p); } }
		sealed class SByteFromDouble     : Convert<SByte,Double>     { public override SByte From(Double p)      { return Convert.ToSByte(p); } }

		sealed class SByteFromBoolean    : Convert<SByte,Boolean>    { public override SByte From(Boolean p)     { return Convert.ToSByte(p); } }
		sealed class SByteFromDecimal    : Convert<SByte,Decimal>    { public override SByte From(Decimal p)     { return Convert.ToSByte(p); } }
		sealed class SByteFromChar       : Convert<SByte,Char>       { public override SByte From(Char p)        { return Convert.ToSByte(p); } }

		// Nullable Types.
		//
		sealed class SByteFromNullableSByte      : Convert<SByte,SByte?>      { public override SByte From(SByte? p)       { return p.HasValue?                 p.Value : (SByte)0; } }

		sealed class SByteFromNullableInt16      : Convert<SByte,Int16?>      { public override SByte From(Int16? p)       { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableInt32      : Convert<SByte,Int32?>      { public override SByte From(Int32? p)       { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableInt64      : Convert<SByte,Int64?>      { public override SByte From(Int64? p)       { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class SByteFromNullableByte       : Convert<SByte,Byte?>       { public override SByte From(Byte? p)        { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableUInt16     : Convert<SByte,UInt16?>     { public override SByte From(UInt16? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableUInt32     : Convert<SByte,UInt32?>     { public override SByte From(UInt32? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableUInt64     : Convert<SByte,UInt64?>     { public override SByte From(UInt64? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class SByteFromNullableSingle     : Convert<SByte,Single?>     { public override SByte From(Single? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableDouble     : Convert<SByte,Double?>     { public override SByte From(Double? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class SByteFromNullableBoolean    : Convert<SByte,Boolean?>    { public override SByte From(Boolean? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableDecimal    : Convert<SByte,Decimal?>    { public override SByte From(Decimal? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class SByteFromNullableChar       : Convert<SByte,Char?>       { public override SByte From(Char? p)        { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		// SqlTypes.
		//
		sealed class SByteFromSqlString  : Convert<SByte,SqlString>  { public override SByte From(SqlString p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class SByteFromSqlByte    : Convert<SByte,SqlByte>    { public override SByte From(SqlByte p)     { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlInt16   : Convert<SByte,SqlInt16>   { public override SByte From(SqlInt16 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlInt32   : Convert<SByte,SqlInt32>   { public override SByte From(SqlInt32 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlInt64   : Convert<SByte,SqlInt64>   { public override SByte From(SqlInt64 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class SByteFromSqlSingle  : Convert<SByte,SqlSingle>  { public override SByte From(SqlSingle p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlDouble  : Convert<SByte,SqlDouble>  { public override SByte From(SqlDouble p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlDecimal : Convert<SByte,SqlDecimal> { public override SByte From(SqlDecimal p)  { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class SByteFromSqlMoney   : Convert<SByte,SqlMoney>   { public override SByte From(SqlMoney p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class SByteFromSqlBoolean : Convert<SByte,SqlBoolean> { public override SByte From(SqlBoolean p)  { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class SByteDefault<Q>     : Convert<SByte,Q>          { public override SByte From(Q p)           { return Convert<SByte,object>.Instance.From(p); } }
		sealed class SByteFromObject     : Convert<SByte,object>     { public override SByte From(object p)      { return Convert.ToSByte(p); } }

		static Convert<T, P> GetSByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SByteFromString());

			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SByteFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SByteFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SByteFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SByteFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SByteFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SByteFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SByteFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SByteFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SByteFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SByteFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SByteFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SByteFromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SByteFromNullableSByte());

			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SByteFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SByteFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SByteFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SByteFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SByteFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SByteFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SByteFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SByteFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SByteFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SByteFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SByteFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SByteFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SByteFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SByteFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SByteFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SByteFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SByteFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SByteFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SByteFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SByteFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SByteFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SByteFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SByteFromObject());

			return (Convert<T, P>)(object)(new SByteDefault<P>());
		}

		#endregion

		#region Int16


		// Scalar Types.
		//
		sealed class Int16FromString     : Convert<Int16,String>     { public override Int16 From(String p)      { return Convert.ToInt16(p); } }

		sealed class Int16FromSByte      : Convert<Int16,SByte>      { public override Int16 From(SByte p)       { return Convert.ToInt16(p); } }
		sealed class Int16FromInt32      : Convert<Int16,Int32>      { public override Int16 From(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class Int16FromInt64      : Convert<Int16,Int64>      { public override Int16 From(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class Int16FromByte       : Convert<Int16,Byte>       { public override Int16 From(Byte p)        { return Convert.ToInt16(p); } }
		sealed class Int16FromUInt16     : Convert<Int16,UInt16>     { public override Int16 From(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class Int16FromUInt32     : Convert<Int16,UInt32>     { public override Int16 From(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class Int16FromUInt64     : Convert<Int16,UInt64>     { public override Int16 From(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class Int16FromSingle     : Convert<Int16,Single>     { public override Int16 From(Single p)      { return Convert.ToInt16(p); } }
		sealed class Int16FromDouble     : Convert<Int16,Double>     { public override Int16 From(Double p)      { return Convert.ToInt16(p); } }

		sealed class Int16FromBoolean    : Convert<Int16,Boolean>    { public override Int16 From(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class Int16FromDecimal    : Convert<Int16,Decimal>    { public override Int16 From(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class Int16FromChar       : Convert<Int16,Char>       { public override Int16 From(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class Int16FromNullableInt16      : Convert<Int16,Int16?>      { public override Int16 From(Int16? p)       { return p.HasValue?                 p.Value : (Int16)0; } }

		sealed class Int16FromNullableSByte      : Convert<Int16,SByte?>      { public override Int16 From(SByte? p)       { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableInt32      : Convert<Int16,Int32?>      { public override Int16 From(Int32? p)       { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableInt64      : Convert<Int16,Int64?>      { public override Int16 From(Int64? p)       { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class Int16FromNullableByte       : Convert<Int16,Byte?>       { public override Int16 From(Byte? p)        { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableUInt16     : Convert<Int16,UInt16?>     { public override Int16 From(UInt16? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableUInt32     : Convert<Int16,UInt32?>     { public override Int16 From(UInt32? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableUInt64     : Convert<Int16,UInt64?>     { public override Int16 From(UInt64? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class Int16FromNullableSingle     : Convert<Int16,Single?>     { public override Int16 From(Single? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableDouble     : Convert<Int16,Double?>     { public override Int16 From(Double? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class Int16FromNullableBoolean    : Convert<Int16,Boolean?>    { public override Int16 From(Boolean? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableDecimal    : Convert<Int16,Decimal?>    { public override Int16 From(Decimal? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class Int16FromNullableChar       : Convert<Int16,Char?>       { public override Int16 From(Char? p)        { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		// SqlTypes.
		//
		sealed class Int16FromSqlInt16   : Convert<Int16,SqlInt16>   { public override Int16 From(SqlInt16 p)    { return p.IsNull? (Int16)0:                 p.Value;  } }
		sealed class Int16FromSqlString  : Convert<Int16,SqlString>  { public override Int16 From(SqlString p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class Int16FromSqlByte    : Convert<Int16,SqlByte>    { public override Int16 From(SqlByte p)     { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class Int16FromSqlInt32   : Convert<Int16,SqlInt32>   { public override Int16 From(SqlInt32 p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class Int16FromSqlInt64   : Convert<Int16,SqlInt64>   { public override Int16 From(SqlInt64 p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class Int16FromSqlSingle  : Convert<Int16,SqlSingle>  { public override Int16 From(SqlSingle p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class Int16FromSqlDouble  : Convert<Int16,SqlDouble>  { public override Int16 From(SqlDouble p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class Int16FromSqlDecimal : Convert<Int16,SqlDecimal> { public override Int16 From(SqlDecimal p)  { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class Int16FromSqlMoney   : Convert<Int16,SqlMoney>   { public override Int16 From(SqlMoney p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class Int16FromSqlBoolean : Convert<Int16,SqlBoolean> { public override Int16 From(SqlBoolean p)  { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class Int16Default<Q>     : Convert<Int16,Q>          { public override Int16 From(Q p)           { return Convert<Int16,object>.Instance.From(p); } }
		sealed class Int16FromObject     : Convert<Int16,object>     { public override Int16 From(object p)      { return Convert.ToInt16(p); } }

		static Convert<T, P> GetInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new Int16FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new Int16FromSByte());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new Int16FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new Int16FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new Int16FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new Int16FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new Int16FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new Int16FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new Int16FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new Int16FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new Int16FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new Int16FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new Int16FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new Int16FromNullableInt16());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new Int16FromNullableSByte());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new Int16FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new Int16FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new Int16FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new Int16FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new Int16FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new Int16FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new Int16FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new Int16FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new Int16FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new Int16FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new Int16FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new Int16FromSqlInt16());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new Int16FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new Int16FromSqlByte());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new Int16FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new Int16FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new Int16FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new Int16FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new Int16FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new Int16FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new Int16FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new Int16FromObject());

			return (Convert<T, P>)(object)(new Int16Default<P>());
		}

		#endregion

		#region Int32


		// Scalar Types.
		//
		sealed class Int32FromString     : Convert<Int32,String>     { public override Int32 From(String p)      { return Convert.ToInt32(p); } }

		sealed class Int32FromSByte      : Convert<Int32,SByte>      { public override Int32 From(SByte p)       { return Convert.ToInt32(p); } }
		sealed class Int32FromInt16      : Convert<Int32,Int16>      { public override Int32 From(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class Int32FromInt64      : Convert<Int32,Int64>      { public override Int32 From(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class Int32FromByte       : Convert<Int32,Byte>       { public override Int32 From(Byte p)        { return Convert.ToInt32(p); } }
		sealed class Int32FromUInt16     : Convert<Int32,UInt16>     { public override Int32 From(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class Int32FromUInt32     : Convert<Int32,UInt32>     { public override Int32 From(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class Int32FromUInt64     : Convert<Int32,UInt64>     { public override Int32 From(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class Int32FromSingle     : Convert<Int32,Single>     { public override Int32 From(Single p)      { return Convert.ToInt32(p); } }
		sealed class Int32FromDouble     : Convert<Int32,Double>     { public override Int32 From(Double p)      { return Convert.ToInt32(p); } }

		sealed class Int32FromBoolean    : Convert<Int32,Boolean>    { public override Int32 From(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class Int32FromDecimal    : Convert<Int32,Decimal>    { public override Int32 From(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class Int32FromChar       : Convert<Int32,Char>       { public override Int32 From(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class Int32FromNullableInt32      : Convert<Int32,Int32?>      { public override Int32 From(Int32? p)       { return p.HasValue?                 p.Value : 0; } }

		sealed class Int32FromNullableSByte      : Convert<Int32,SByte?>      { public override Int32 From(SByte? p)       { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableInt16      : Convert<Int32,Int16?>      { public override Int32 From(Int16? p)       { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableInt64      : Convert<Int32,Int64?>      { public override Int32 From(Int64? p)       { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class Int32FromNullableByte       : Convert<Int32,Byte?>       { public override Int32 From(Byte? p)        { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableUInt16     : Convert<Int32,UInt16?>     { public override Int32 From(UInt16? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableUInt32     : Convert<Int32,UInt32?>     { public override Int32 From(UInt32? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableUInt64     : Convert<Int32,UInt64?>     { public override Int32 From(UInt64? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class Int32FromNullableSingle     : Convert<Int32,Single?>     { public override Int32 From(Single? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableDouble     : Convert<Int32,Double?>     { public override Int32 From(Double? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class Int32FromNullableBoolean    : Convert<Int32,Boolean?>    { public override Int32 From(Boolean? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableDecimal    : Convert<Int32,Decimal?>    { public override Int32 From(Decimal? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class Int32FromNullableChar       : Convert<Int32,Char?>       { public override Int32 From(Char? p)        { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class Int32FromSqlInt32   : Convert<Int32,SqlInt32>   { public override Int32 From(SqlInt32 p)    { return p.IsNull? 0:                 p.Value;  } }
		sealed class Int32FromSqlString  : Convert<Int32,SqlString>  { public override Int32 From(SqlString p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class Int32FromSqlByte    : Convert<Int32,SqlByte>    { public override Int32 From(SqlByte p)     { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class Int32FromSqlInt16   : Convert<Int32,SqlInt16>   { public override Int32 From(SqlInt16 p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class Int32FromSqlInt64   : Convert<Int32,SqlInt64>   { public override Int32 From(SqlInt64 p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class Int32FromSqlSingle  : Convert<Int32,SqlSingle>  { public override Int32 From(SqlSingle p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class Int32FromSqlDouble  : Convert<Int32,SqlDouble>  { public override Int32 From(SqlDouble p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class Int32FromSqlDecimal : Convert<Int32,SqlDecimal> { public override Int32 From(SqlDecimal p)  { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class Int32FromSqlMoney   : Convert<Int32,SqlMoney>   { public override Int32 From(SqlMoney p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class Int32FromSqlBoolean : Convert<Int32,SqlBoolean> { public override Int32 From(SqlBoolean p)  { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class Int32Default<Q>     : Convert<Int32,Q>          { public override Int32 From(Q p)           { return Convert<Int32,object>.Instance.From(p); } }
		sealed class Int32FromObject     : Convert<Int32,object>     { public override Int32 From(object p)      { return Convert.ToInt32(p); } }

		static Convert<T, P> GetInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new Int32FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new Int32FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new Int32FromInt16());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new Int32FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new Int32FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new Int32FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new Int32FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new Int32FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new Int32FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new Int32FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new Int32FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new Int32FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new Int32FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new Int32FromNullableInt32());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new Int32FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new Int32FromNullableInt16());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new Int32FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new Int32FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new Int32FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new Int32FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new Int32FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new Int32FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new Int32FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new Int32FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new Int32FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new Int32FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new Int32FromSqlInt32());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new Int32FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new Int32FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new Int32FromSqlInt16());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new Int32FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new Int32FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new Int32FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new Int32FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new Int32FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new Int32FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new Int32FromObject());

			return (Convert<T, P>)(object)(new Int32Default<P>());
		}

		#endregion

		#region Int64


		// Scalar Types.
		//
		sealed class Int64FromString     : Convert<Int64,String>     { public override Int64 From(String p)      { return Convert.ToInt64(p); } }

		sealed class Int64FromSByte      : Convert<Int64,SByte>      { public override Int64 From(SByte p)       { return Convert.ToInt64(p); } }
		sealed class Int64FromInt16      : Convert<Int64,Int16>      { public override Int64 From(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class Int64FromInt32      : Convert<Int64,Int32>      { public override Int64 From(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class Int64FromByte       : Convert<Int64,Byte>       { public override Int64 From(Byte p)        { return Convert.ToInt64(p); } }
		sealed class Int64FromUInt16     : Convert<Int64,UInt16>     { public override Int64 From(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class Int64FromUInt32     : Convert<Int64,UInt32>     { public override Int64 From(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class Int64FromUInt64     : Convert<Int64,UInt64>     { public override Int64 From(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class Int64FromSingle     : Convert<Int64,Single>     { public override Int64 From(Single p)      { return Convert.ToInt64(p); } }
		sealed class Int64FromDouble     : Convert<Int64,Double>     { public override Int64 From(Double p)      { return Convert.ToInt64(p); } }

		sealed class Int64FromBoolean    : Convert<Int64,Boolean>    { public override Int64 From(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class Int64FromDecimal    : Convert<Int64,Decimal>    { public override Int64 From(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class Int64FromChar       : Convert<Int64,Char>       { public override Int64 From(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class Int64FromNullableInt64      : Convert<Int64,Int64?>      { public override Int64 From(Int64? p)       { return p.HasValue?                 p.Value : 0; } }

		sealed class Int64FromNullableSByte      : Convert<Int64,SByte?>      { public override Int64 From(SByte? p)       { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableInt16      : Convert<Int64,Int16?>      { public override Int64 From(Int16? p)       { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableInt32      : Convert<Int64,Int32?>      { public override Int64 From(Int32? p)       { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class Int64FromNullableByte       : Convert<Int64,Byte?>       { public override Int64 From(Byte? p)        { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableUInt16     : Convert<Int64,UInt16?>     { public override Int64 From(UInt16? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableUInt32     : Convert<Int64,UInt32?>     { public override Int64 From(UInt32? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableUInt64     : Convert<Int64,UInt64?>     { public override Int64 From(UInt64? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class Int64FromNullableSingle     : Convert<Int64,Single?>     { public override Int64 From(Single? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableDouble     : Convert<Int64,Double?>     { public override Int64 From(Double? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class Int64FromNullableBoolean    : Convert<Int64,Boolean?>    { public override Int64 From(Boolean? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableDecimal    : Convert<Int64,Decimal?>    { public override Int64 From(Decimal? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class Int64FromNullableChar       : Convert<Int64,Char?>       { public override Int64 From(Char? p)        { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class Int64FromSqlInt64   : Convert<Int64,SqlInt64>   { public override Int64 From(SqlInt64 p)    { return p.IsNull? 0:                 p.Value;  } }
		sealed class Int64FromSqlString  : Convert<Int64,SqlString>  { public override Int64 From(SqlString p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class Int64FromSqlByte    : Convert<Int64,SqlByte>    { public override Int64 From(SqlByte p)     { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class Int64FromSqlInt16   : Convert<Int64,SqlInt16>   { public override Int64 From(SqlInt16 p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class Int64FromSqlInt32   : Convert<Int64,SqlInt32>   { public override Int64 From(SqlInt32 p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class Int64FromSqlSingle  : Convert<Int64,SqlSingle>  { public override Int64 From(SqlSingle p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class Int64FromSqlDouble  : Convert<Int64,SqlDouble>  { public override Int64 From(SqlDouble p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class Int64FromSqlDecimal : Convert<Int64,SqlDecimal> { public override Int64 From(SqlDecimal p)  { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class Int64FromSqlMoney   : Convert<Int64,SqlMoney>   { public override Int64 From(SqlMoney p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class Int64FromSqlBoolean : Convert<Int64,SqlBoolean> { public override Int64 From(SqlBoolean p)  { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class Int64Default<Q>     : Convert<Int64,Q>          { public override Int64 From(Q p)           { return Convert<Int64,object>.Instance.From(p); } }
		sealed class Int64FromObject     : Convert<Int64,object>     { public override Int64 From(object p)      { return Convert.ToInt64(p); } }

		static Convert<T, P> GetInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new Int64FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new Int64FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new Int64FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new Int64FromInt32());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new Int64FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new Int64FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new Int64FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new Int64FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new Int64FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new Int64FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new Int64FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new Int64FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new Int64FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new Int64FromNullableInt64());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new Int64FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new Int64FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new Int64FromNullableInt32());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new Int64FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new Int64FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new Int64FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new Int64FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new Int64FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new Int64FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new Int64FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new Int64FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new Int64FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new Int64FromSqlInt64());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new Int64FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new Int64FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new Int64FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new Int64FromSqlInt32());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new Int64FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new Int64FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new Int64FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new Int64FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new Int64FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new Int64FromObject());

			return (Convert<T, P>)(object)(new Int64Default<P>());
		}

		#endregion

		#region Byte


		// Scalar Types.
		//
		sealed class ByteFromString     : Convert<Byte,String>     { public override Byte From(String p)      { return Convert.ToByte(p); } }

		sealed class ByteFromSByte      : Convert<Byte,SByte>      { public override Byte From(SByte p)       { return Convert.ToByte(p); } }
		sealed class ByteFromInt16      : Convert<Byte,Int16>      { public override Byte From(Int16 p)       { return Convert.ToByte(p); } }
		sealed class ByteFromInt32      : Convert<Byte,Int32>      { public override Byte From(Int32 p)       { return Convert.ToByte(p); } }
		sealed class ByteFromInt64      : Convert<Byte,Int64>      { public override Byte From(Int64 p)       { return Convert.ToByte(p); } }

		sealed class ByteFromUInt16     : Convert<Byte,UInt16>     { public override Byte From(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class ByteFromUInt32     : Convert<Byte,UInt32>     { public override Byte From(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class ByteFromUInt64     : Convert<Byte,UInt64>     { public override Byte From(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class ByteFromSingle     : Convert<Byte,Single>     { public override Byte From(Single p)      { return Convert.ToByte(p); } }
		sealed class ByteFromDouble     : Convert<Byte,Double>     { public override Byte From(Double p)      { return Convert.ToByte(p); } }

		sealed class ByteFromBoolean    : Convert<Byte,Boolean>    { public override Byte From(Boolean p)     { return Convert.ToByte(p); } }
		sealed class ByteFromDecimal    : Convert<Byte,Decimal>    { public override Byte From(Decimal p)     { return Convert.ToByte(p); } }
		sealed class ByteFromChar       : Convert<Byte,Char>       { public override Byte From(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class ByteFromNullableByte       : Convert<Byte,Byte?>       { public override Byte From(Byte? p)        { return p.HasValue?                p.Value : (Byte)0; } }

		sealed class ByteFromNullableSByte      : Convert<Byte,SByte?>      { public override Byte From(SByte? p)       { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableInt16      : Convert<Byte,Int16?>      { public override Byte From(Int16? p)       { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableInt32      : Convert<Byte,Int32?>      { public override Byte From(Int32? p)       { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableInt64      : Convert<Byte,Int64?>      { public override Byte From(Int64? p)       { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class ByteFromNullableUInt16     : Convert<Byte,UInt16?>     { public override Byte From(UInt16? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableUInt32     : Convert<Byte,UInt32?>     { public override Byte From(UInt32? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableUInt64     : Convert<Byte,UInt64?>     { public override Byte From(UInt64? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class ByteFromNullableSingle     : Convert<Byte,Single?>     { public override Byte From(Single? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableDouble     : Convert<Byte,Double?>     { public override Byte From(Double? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class ByteFromNullableBoolean    : Convert<Byte,Boolean?>    { public override Byte From(Boolean? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableDecimal    : Convert<Byte,Decimal?>    { public override Byte From(Decimal? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class ByteFromNullableChar       : Convert<Byte,Char?>       { public override Byte From(Char? p)        { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		// SqlTypes.
		//
		sealed class ByteFromSqlByte    : Convert<Byte,SqlByte>    { public override Byte From(SqlByte p)     { return p.IsNull? (Byte)0:                p.Value;  } }
		sealed class ByteFromSqlString  : Convert<Byte,SqlString>  { public override Byte From(SqlString p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class ByteFromSqlInt16   : Convert<Byte,SqlInt16>   { public override Byte From(SqlInt16 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class ByteFromSqlInt32   : Convert<Byte,SqlInt32>   { public override Byte From(SqlInt32 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class ByteFromSqlInt64   : Convert<Byte,SqlInt64>   { public override Byte From(SqlInt64 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class ByteFromSqlSingle  : Convert<Byte,SqlSingle>  { public override Byte From(SqlSingle p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class ByteFromSqlDouble  : Convert<Byte,SqlDouble>  { public override Byte From(SqlDouble p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class ByteFromSqlDecimal : Convert<Byte,SqlDecimal> { public override Byte From(SqlDecimal p)  { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class ByteFromSqlMoney   : Convert<Byte,SqlMoney>   { public override Byte From(SqlMoney p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class ByteFromSqlBoolean : Convert<Byte,SqlBoolean> { public override Byte From(SqlBoolean p)  { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class ByteDefault<Q>     : Convert<Byte,Q>          { public override Byte From(Q p)           { return Convert<Byte,object>.Instance.From(p); } }
		sealed class ByteFromObject     : Convert<Byte,object>     { public override Byte From(object p)      { return Convert.ToByte(p); } }

		static Convert<T, P> GetByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new ByteFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new ByteFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new ByteFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new ByteFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new ByteFromInt64());

			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new ByteFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new ByteFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new ByteFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new ByteFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new ByteFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new ByteFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new ByteFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new ByteFromChar());

			// Nullable Types.
			//
			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new ByteFromNullableByte());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new ByteFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new ByteFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new ByteFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new ByteFromNullableInt64());

			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new ByteFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new ByteFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new ByteFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new ByteFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new ByteFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new ByteFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new ByteFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new ByteFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new ByteFromSqlByte());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new ByteFromSqlString());

			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new ByteFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new ByteFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new ByteFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new ByteFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new ByteFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new ByteFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new ByteFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new ByteFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new ByteFromObject());

			return (Convert<T, P>)(object)(new ByteDefault<P>());
		}

		#endregion

		#region UInt16


		// Scalar Types.
		//
		sealed class UInt16FromString     : Convert<UInt16,String>     { public override UInt16 From(String p)      { return Convert.ToUInt16(p); } }

		sealed class UInt16FromSByte      : Convert<UInt16,SByte>      { public override UInt16 From(SByte p)       { return Convert.ToUInt16(p); } }
		sealed class UInt16FromInt16      : Convert<UInt16,Int16>      { public override UInt16 From(Int16 p)       { return Convert.ToUInt16(p); } }
		sealed class UInt16FromInt32      : Convert<UInt16,Int32>      { public override UInt16 From(Int32 p)       { return Convert.ToUInt16(p); } }
		sealed class UInt16FromInt64      : Convert<UInt16,Int64>      { public override UInt16 From(Int64 p)       { return Convert.ToUInt16(p); } }

		sealed class UInt16FromByte       : Convert<UInt16,Byte>       { public override UInt16 From(Byte p)        { return Convert.ToUInt16(p); } }
		sealed class UInt16FromUInt32     : Convert<UInt16,UInt32>     { public override UInt16 From(UInt32 p)      { return Convert.ToUInt16(p); } }
		sealed class UInt16FromUInt64     : Convert<UInt16,UInt64>     { public override UInt16 From(UInt64 p)      { return Convert.ToUInt16(p); } }

		sealed class UInt16FromSingle     : Convert<UInt16,Single>     { public override UInt16 From(Single p)      { return Convert.ToUInt16(p); } }
		sealed class UInt16FromDouble     : Convert<UInt16,Double>     { public override UInt16 From(Double p)      { return Convert.ToUInt16(p); } }

		sealed class UInt16FromBoolean    : Convert<UInt16,Boolean>    { public override UInt16 From(Boolean p)     { return Convert.ToUInt16(p); } }
		sealed class UInt16FromDecimal    : Convert<UInt16,Decimal>    { public override UInt16 From(Decimal p)     { return Convert.ToUInt16(p); } }
		sealed class UInt16FromChar       : Convert<UInt16,Char>       { public override UInt16 From(Char p)        { return Convert.ToUInt16(p); } }

		// Nullable Types.
		//
		sealed class UInt16FromNullableUInt16     : Convert<UInt16,UInt16?>     { public override UInt16 From(UInt16? p)      { return p.HasValue?                  p.Value : (UInt16)0; } }

		sealed class UInt16FromNullableSByte      : Convert<UInt16,SByte?>      { public override UInt16 From(SByte? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableInt16      : Convert<UInt16,Int16?>      { public override UInt16 From(Int16? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableInt32      : Convert<UInt16,Int32?>      { public override UInt16 From(Int32? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableInt64      : Convert<UInt16,Int64?>      { public override UInt16 From(Int64? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class UInt16FromNullableByte       : Convert<UInt16,Byte?>       { public override UInt16 From(Byte? p)        { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableUInt32     : Convert<UInt16,UInt32?>     { public override UInt16 From(UInt32? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableUInt64     : Convert<UInt16,UInt64?>     { public override UInt16 From(UInt64? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class UInt16FromNullableSingle     : Convert<UInt16,Single?>     { public override UInt16 From(Single? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableDouble     : Convert<UInt16,Double?>     { public override UInt16 From(Double? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class UInt16FromNullableBoolean    : Convert<UInt16,Boolean?>    { public override UInt16 From(Boolean? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableDecimal    : Convert<UInt16,Decimal?>    { public override UInt16 From(Decimal? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class UInt16FromNullableChar       : Convert<UInt16,Char?>       { public override UInt16 From(Char? p)        { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		// SqlTypes.
		//
		sealed class UInt16FromSqlString  : Convert<UInt16,SqlString>  { public override UInt16 From(SqlString p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class UInt16FromSqlByte    : Convert<UInt16,SqlByte>    { public override UInt16 From(SqlByte p)     { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlInt16   : Convert<UInt16,SqlInt16>   { public override UInt16 From(SqlInt16 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlInt32   : Convert<UInt16,SqlInt32>   { public override UInt16 From(SqlInt32 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlInt64   : Convert<UInt16,SqlInt64>   { public override UInt16 From(SqlInt64 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class UInt16FromSqlSingle  : Convert<UInt16,SqlSingle>  { public override UInt16 From(SqlSingle p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlDouble  : Convert<UInt16,SqlDouble>  { public override UInt16 From(SqlDouble p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlDecimal : Convert<UInt16,SqlDecimal> { public override UInt16 From(SqlDecimal p)  { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class UInt16FromSqlMoney   : Convert<UInt16,SqlMoney>   { public override UInt16 From(SqlMoney p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class UInt16FromSqlBoolean : Convert<UInt16,SqlBoolean> { public override UInt16 From(SqlBoolean p)  { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class UInt16Default<Q>     : Convert<UInt16,Q>          { public override UInt16 From(Q p)           { return Convert<UInt16,object>.Instance.From(p); } }
		sealed class UInt16FromObject     : Convert<UInt16,object>     { public override UInt16 From(object p)      { return Convert.ToUInt16(p); } }

		static Convert<T, P> GetUInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new UInt16FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new UInt16FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new UInt16FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new UInt16FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new UInt16FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new UInt16FromByte());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new UInt16FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new UInt16FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new UInt16FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new UInt16FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new UInt16FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new UInt16FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new UInt16FromChar());

			// Nullable Types.
			//
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new UInt16FromNullableUInt16());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new UInt16FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new UInt16FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new UInt16FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new UInt16FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new UInt16FromNullableByte());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new UInt16FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new UInt16FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new UInt16FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new UInt16FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new UInt16FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new UInt16FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new UInt16FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new UInt16FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new UInt16FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new UInt16FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new UInt16FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new UInt16FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new UInt16FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new UInt16FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new UInt16FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new UInt16FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new UInt16FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new UInt16FromObject());

			return (Convert<T, P>)(object)(new UInt16Default<P>());
		}

		#endregion

		#region UInt32


		// Scalar Types.
		//
		sealed class UInt32FromString     : Convert<UInt32,String>     { public override UInt32 From(String p)      { return Convert.ToUInt32(p); } }

		sealed class UInt32FromSByte      : Convert<UInt32,SByte>      { public override UInt32 From(SByte p)       { return Convert.ToUInt32(p); } }
		sealed class UInt32FromInt16      : Convert<UInt32,Int16>      { public override UInt32 From(Int16 p)       { return Convert.ToUInt32(p); } }
		sealed class UInt32FromInt32      : Convert<UInt32,Int32>      { public override UInt32 From(Int32 p)       { return Convert.ToUInt32(p); } }
		sealed class UInt32FromInt64      : Convert<UInt32,Int64>      { public override UInt32 From(Int64 p)       { return Convert.ToUInt32(p); } }

		sealed class UInt32FromByte       : Convert<UInt32,Byte>       { public override UInt32 From(Byte p)        { return Convert.ToUInt32(p); } }
		sealed class UInt32FromUInt16     : Convert<UInt32,UInt16>     { public override UInt32 From(UInt16 p)      { return Convert.ToUInt32(p); } }
		sealed class UInt32FromUInt64     : Convert<UInt32,UInt64>     { public override UInt32 From(UInt64 p)      { return Convert.ToUInt32(p); } }

		sealed class UInt32FromSingle     : Convert<UInt32,Single>     { public override UInt32 From(Single p)      { return Convert.ToUInt32(p); } }
		sealed class UInt32FromDouble     : Convert<UInt32,Double>     { public override UInt32 From(Double p)      { return Convert.ToUInt32(p); } }

		sealed class UInt32FromBoolean    : Convert<UInt32,Boolean>    { public override UInt32 From(Boolean p)     { return Convert.ToUInt32(p); } }
		sealed class UInt32FromDecimal    : Convert<UInt32,Decimal>    { public override UInt32 From(Decimal p)     { return Convert.ToUInt32(p); } }
		sealed class UInt32FromChar       : Convert<UInt32,Char>       { public override UInt32 From(Char p)        { return Convert.ToUInt32(p); } }

		// Nullable Types.
		//
		sealed class UInt32FromNullableUInt32     : Convert<UInt32,UInt32?>     { public override UInt32 From(UInt32? p)      { return p.HasValue?                  p.Value : (UInt32)0; } }

		sealed class UInt32FromNullableSByte      : Convert<UInt32,SByte?>      { public override UInt32 From(SByte? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableInt16      : Convert<UInt32,Int16?>      { public override UInt32 From(Int16? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableInt32      : Convert<UInt32,Int32?>      { public override UInt32 From(Int32? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableInt64      : Convert<UInt32,Int64?>      { public override UInt32 From(Int64? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class UInt32FromNullableByte       : Convert<UInt32,Byte?>       { public override UInt32 From(Byte? p)        { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableUInt16     : Convert<UInt32,UInt16?>     { public override UInt32 From(UInt16? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableUInt64     : Convert<UInt32,UInt64?>     { public override UInt32 From(UInt64? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class UInt32FromNullableSingle     : Convert<UInt32,Single?>     { public override UInt32 From(Single? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableDouble     : Convert<UInt32,Double?>     { public override UInt32 From(Double? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class UInt32FromNullableBoolean    : Convert<UInt32,Boolean?>    { public override UInt32 From(Boolean? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableDecimal    : Convert<UInt32,Decimal?>    { public override UInt32 From(Decimal? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class UInt32FromNullableChar       : Convert<UInt32,Char?>       { public override UInt32 From(Char? p)        { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		// SqlTypes.
		//
		sealed class UInt32FromSqlString  : Convert<UInt32,SqlString>  { public override UInt32 From(SqlString p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class UInt32FromSqlByte    : Convert<UInt32,SqlByte>    { public override UInt32 From(SqlByte p)     { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlInt16   : Convert<UInt32,SqlInt16>   { public override UInt32 From(SqlInt16 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlInt32   : Convert<UInt32,SqlInt32>   { public override UInt32 From(SqlInt32 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlInt64   : Convert<UInt32,SqlInt64>   { public override UInt32 From(SqlInt64 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class UInt32FromSqlSingle  : Convert<UInt32,SqlSingle>  { public override UInt32 From(SqlSingle p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlDouble  : Convert<UInt32,SqlDouble>  { public override UInt32 From(SqlDouble p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlDecimal : Convert<UInt32,SqlDecimal> { public override UInt32 From(SqlDecimal p)  { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class UInt32FromSqlMoney   : Convert<UInt32,SqlMoney>   { public override UInt32 From(SqlMoney p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class UInt32FromSqlBoolean : Convert<UInt32,SqlBoolean> { public override UInt32 From(SqlBoolean p)  { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class UInt32Default<Q>     : Convert<UInt32,Q>          { public override UInt32 From(Q p)           { return Convert<UInt32,object>.Instance.From(p); } }
		sealed class UInt32FromObject     : Convert<UInt32,object>     { public override UInt32 From(object p)      { return Convert.ToUInt32(p); } }

		static Convert<T, P> GetUInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new UInt32FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new UInt32FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new UInt32FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new UInt32FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new UInt32FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new UInt32FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new UInt32FromUInt16());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new UInt32FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new UInt32FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new UInt32FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new UInt32FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new UInt32FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new UInt32FromChar());

			// Nullable Types.
			//
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new UInt32FromNullableUInt32());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new UInt32FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new UInt32FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new UInt32FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new UInt32FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new UInt32FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new UInt32FromNullableUInt16());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new UInt32FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new UInt32FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new UInt32FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new UInt32FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new UInt32FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new UInt32FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new UInt32FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new UInt32FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new UInt32FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new UInt32FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new UInt32FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new UInt32FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new UInt32FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new UInt32FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new UInt32FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new UInt32FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new UInt32FromObject());

			return (Convert<T, P>)(object)(new UInt32Default<P>());
		}

		#endregion

		#region UInt64


		// Scalar Types.
		//
		sealed class UInt64FromString     : Convert<UInt64,String>     { public override UInt64 From(String p)      { return Convert.ToUInt64(p); } }

		sealed class UInt64FromSByte      : Convert<UInt64,SByte>      { public override UInt64 From(SByte p)       { return Convert.ToUInt64(p); } }
		sealed class UInt64FromInt16      : Convert<UInt64,Int16>      { public override UInt64 From(Int16 p)       { return Convert.ToUInt64(p); } }
		sealed class UInt64FromInt32      : Convert<UInt64,Int32>      { public override UInt64 From(Int32 p)       { return Convert.ToUInt64(p); } }
		sealed class UInt64FromInt64      : Convert<UInt64,Int64>      { public override UInt64 From(Int64 p)       { return Convert.ToUInt64(p); } }

		sealed class UInt64FromByte       : Convert<UInt64,Byte>       { public override UInt64 From(Byte p)        { return Convert.ToUInt64(p); } }
		sealed class UInt64FromUInt16     : Convert<UInt64,UInt16>     { public override UInt64 From(UInt16 p)      { return Convert.ToUInt64(p); } }
		sealed class UInt64FromUInt32     : Convert<UInt64,UInt32>     { public override UInt64 From(UInt32 p)      { return Convert.ToUInt64(p); } }

		sealed class UInt64FromSingle     : Convert<UInt64,Single>     { public override UInt64 From(Single p)      { return Convert.ToUInt64(p); } }
		sealed class UInt64FromDouble     : Convert<UInt64,Double>     { public override UInt64 From(Double p)      { return Convert.ToUInt64(p); } }

		sealed class UInt64FromBoolean    : Convert<UInt64,Boolean>    { public override UInt64 From(Boolean p)     { return Convert.ToUInt64(p); } }
		sealed class UInt64FromDecimal    : Convert<UInt64,Decimal>    { public override UInt64 From(Decimal p)     { return Convert.ToUInt64(p); } }
		sealed class UInt64FromChar       : Convert<UInt64,Char>       { public override UInt64 From(Char p)        { return Convert.ToUInt64(p); } }

		// Nullable Types.
		//
		sealed class UInt64FromNullableUInt64     : Convert<UInt64,UInt64?>     { public override UInt64 From(UInt64? p)      { return p.HasValue?                  p.Value : (UInt64)0; } }

		sealed class UInt64FromNullableSByte      : Convert<UInt64,SByte?>      { public override UInt64 From(SByte? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableInt16      : Convert<UInt64,Int16?>      { public override UInt64 From(Int16? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableInt32      : Convert<UInt64,Int32?>      { public override UInt64 From(Int32? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableInt64      : Convert<UInt64,Int64?>      { public override UInt64 From(Int64? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class UInt64FromNullableByte       : Convert<UInt64,Byte?>       { public override UInt64 From(Byte? p)        { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableUInt16     : Convert<UInt64,UInt16?>     { public override UInt64 From(UInt16? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableUInt32     : Convert<UInt64,UInt32?>     { public override UInt64 From(UInt32? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class UInt64FromNullableSingle     : Convert<UInt64,Single?>     { public override UInt64 From(Single? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableDouble     : Convert<UInt64,Double?>     { public override UInt64 From(Double? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class UInt64FromNullableBoolean    : Convert<UInt64,Boolean?>    { public override UInt64 From(Boolean? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableDecimal    : Convert<UInt64,Decimal?>    { public override UInt64 From(Decimal? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class UInt64FromNullableChar       : Convert<UInt64,Char?>       { public override UInt64 From(Char? p)        { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		// SqlTypes.
		//
		sealed class UInt64FromSqlString  : Convert<UInt64,SqlString>  { public override UInt64 From(SqlString p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class UInt64FromSqlByte    : Convert<UInt64,SqlByte>    { public override UInt64 From(SqlByte p)     { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlInt16   : Convert<UInt64,SqlInt16>   { public override UInt64 From(SqlInt16 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlInt32   : Convert<UInt64,SqlInt32>   { public override UInt64 From(SqlInt32 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlInt64   : Convert<UInt64,SqlInt64>   { public override UInt64 From(SqlInt64 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class UInt64FromSqlSingle  : Convert<UInt64,SqlSingle>  { public override UInt64 From(SqlSingle p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlDouble  : Convert<UInt64,SqlDouble>  { public override UInt64 From(SqlDouble p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlDecimal : Convert<UInt64,SqlDecimal> { public override UInt64 From(SqlDecimal p)  { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class UInt64FromSqlMoney   : Convert<UInt64,SqlMoney>   { public override UInt64 From(SqlMoney p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class UInt64FromSqlBoolean : Convert<UInt64,SqlBoolean> { public override UInt64 From(SqlBoolean p)  { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class UInt64Default<Q>     : Convert<UInt64,Q>          { public override UInt64 From(Q p)           { return Convert<UInt64,object>.Instance.From(p); } }
		sealed class UInt64FromObject     : Convert<UInt64,object>     { public override UInt64 From(object p)      { return Convert.ToUInt64(p); } }

		static Convert<T, P> GetUInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new UInt64FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new UInt64FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new UInt64FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new UInt64FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new UInt64FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new UInt64FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new UInt64FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new UInt64FromUInt32());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new UInt64FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new UInt64FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new UInt64FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new UInt64FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new UInt64FromChar());

			// Nullable Types.
			//
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new UInt64FromNullableUInt64());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new UInt64FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new UInt64FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new UInt64FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new UInt64FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new UInt64FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new UInt64FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new UInt64FromNullableUInt32());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new UInt64FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new UInt64FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new UInt64FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new UInt64FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new UInt64FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new UInt64FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new UInt64FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new UInt64FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new UInt64FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new UInt64FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new UInt64FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new UInt64FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new UInt64FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new UInt64FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new UInt64FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new UInt64FromObject());

			return (Convert<T, P>)(object)(new UInt64Default<P>());
		}

		#endregion

		#region Char


		// Scalar Types.
		//
		sealed class CharFromString     : Convert<Char,String>     { public override Char From(String p)      { return Convert.ToChar(p); } }

		sealed class CharFromSByte      : Convert<Char,SByte>      { public override Char From(SByte p)       { return Convert.ToChar(p); } }
		sealed class CharFromInt16      : Convert<Char,Int16>      { public override Char From(Int16 p)       { return Convert.ToChar(p); } }
		sealed class CharFromInt32      : Convert<Char,Int32>      { public override Char From(Int32 p)       { return Convert.ToChar(p); } }
		sealed class CharFromInt64      : Convert<Char,Int64>      { public override Char From(Int64 p)       { return Convert.ToChar(p); } }

		sealed class CharFromByte       : Convert<Char,Byte>       { public override Char From(Byte p)        { return Convert.ToChar(p); } }
		sealed class CharFromUInt16     : Convert<Char,UInt16>     { public override Char From(UInt16 p)      { return Convert.ToChar(p); } }
		sealed class CharFromUInt32     : Convert<Char,UInt32>     { public override Char From(UInt32 p)      { return Convert.ToChar(p); } }
		sealed class CharFromUInt64     : Convert<Char,UInt64>     { public override Char From(UInt64 p)      { return Convert.ToChar(p); } }
		sealed class CharFromBoolean    : Convert<Char,Boolean>    { public override Char From(Boolean p)     { return p? '1':'0'; } }

		// Nullable Types.
		//
		sealed class CharFromNullableChar       : Convert<Char,Char?>       { public override Char From(Char? p)        { return p.HasValue?                p.Value : (Char)0; } }

		sealed class CharFromNullableSByte      : Convert<Char,SByte?>      { public override Char From(SByte? p)       { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableInt16      : Convert<Char,Int16?>      { public override Char From(Int16? p)       { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableInt32      : Convert<Char,Int32?>      { public override Char From(Int32? p)       { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableInt64      : Convert<Char,Int64?>      { public override Char From(Int64? p)       { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }

		sealed class CharFromNullableByte       : Convert<Char,Byte?>       { public override Char From(Byte? p)        { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableUInt16     : Convert<Char,UInt16?>     { public override Char From(UInt16? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableUInt32     : Convert<Char,UInt32?>     { public override Char From(UInt32? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableUInt64     : Convert<Char,UInt64?>     { public override Char From(UInt64? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class CharFromNullableBoolean    : Convert<Char,Boolean?>    { public override Char From(Boolean? p)     { return p.HasValue? p.Value? '1':'0'       : (Char)0; } }

		// SqlTypes.
		//
		sealed class CharFromSqlString  : Convert<Char,SqlString>  { public override Char From(SqlString p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class CharFromSqlByte    : Convert<Char,SqlByte>    { public override Char From(SqlByte p)     { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlInt16   : Convert<Char,SqlInt16>   { public override Char From(SqlInt16 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlInt32   : Convert<Char,SqlInt32>   { public override Char From(SqlInt32 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlInt64   : Convert<Char,SqlInt64>   { public override Char From(SqlInt64 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class CharFromSqlSingle  : Convert<Char,SqlSingle>  { public override Char From(SqlSingle p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlDouble  : Convert<Char,SqlDouble>  { public override Char From(SqlDouble p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlDecimal : Convert<Char,SqlDecimal> { public override Char From(SqlDecimal p)  { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class CharFromSqlMoney   : Convert<Char,SqlMoney>   { public override Char From(SqlMoney p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class CharFromSqlBoolean : Convert<Char,SqlBoolean> { public override Char From(SqlBoolean p)  { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class CharDefault<Q>     : Convert<Char,Q>          { public override Char From(Q p)           { return Convert<Char,object>.Instance.From(p); } }
		sealed class CharFromObject     : Convert<Char,object>     { public override Char From(object p)      { return Convert.ToChar(p); } }

		static Convert<T, P> GetCharConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new CharFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new CharFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new CharFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new CharFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new CharFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new CharFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new CharFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new CharFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new CharFromUInt64());
			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new CharFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new CharFromNullableChar());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new CharFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new CharFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new CharFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new CharFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new CharFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new CharFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new CharFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new CharFromNullableUInt64());
			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new CharFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new CharFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new CharFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new CharFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new CharFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new CharFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new CharFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new CharFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new CharFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new CharFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new CharFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new CharFromObject());

			return (Convert<T, P>)(object)(new CharDefault<P>());
		}

		#endregion

		#region Single


		// Scalar Types.
		//
		sealed class SingleFromString     : Convert<Single,String>     { public override Single From(String p)      { return Convert.ToSingle(p); } }

		sealed class SingleFromSByte      : Convert<Single,SByte>      { public override Single From(SByte p)       { return Convert.ToSingle(p); } }
		sealed class SingleFromInt16      : Convert<Single,Int16>      { public override Single From(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class SingleFromInt32      : Convert<Single,Int32>      { public override Single From(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class SingleFromInt64      : Convert<Single,Int64>      { public override Single From(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class SingleFromByte       : Convert<Single,Byte>       { public override Single From(Byte p)        { return Convert.ToSingle(p); } }
		sealed class SingleFromUInt16     : Convert<Single,UInt16>     { public override Single From(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class SingleFromUInt32     : Convert<Single,UInt32>     { public override Single From(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class SingleFromUInt64     : Convert<Single,UInt64>     { public override Single From(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class SingleFromDouble     : Convert<Single,Double>     { public override Single From(Double p)      { return Convert.ToSingle(p); } }

		sealed class SingleFromBoolean    : Convert<Single,Boolean>    { public override Single From(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class SingleFromDecimal    : Convert<Single,Decimal>    { public override Single From(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class SingleFromNullableSingle     : Convert<Single,Single?>     { public override Single From(Single? p)      { return p.HasValue?                  p.Value : 0; } }

		sealed class SingleFromNullableSByte      : Convert<Single,SByte?>      { public override Single From(SByte? p)       { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableInt16      : Convert<Single,Int16?>      { public override Single From(Int16? p)       { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableInt32      : Convert<Single,Int32?>      { public override Single From(Int32? p)       { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableInt64      : Convert<Single,Int64?>      { public override Single From(Int64? p)       { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class SingleFromNullableByte       : Convert<Single,Byte?>       { public override Single From(Byte? p)        { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableUInt16     : Convert<Single,UInt16?>     { public override Single From(UInt16? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableUInt32     : Convert<Single,UInt32?>     { public override Single From(UInt32? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableUInt64     : Convert<Single,UInt64?>     { public override Single From(UInt64? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class SingleFromNullableDouble     : Convert<Single,Double?>     { public override Single From(Double? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class SingleFromNullableBoolean    : Convert<Single,Boolean?>    { public override Single From(Boolean? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class SingleFromNullableDecimal    : Convert<Single,Decimal?>    { public override Single From(Decimal? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class SingleFromSqlSingle  : Convert<Single,SqlSingle>  { public override Single From(SqlSingle p)   { return p.IsNull? 0:                  p.Value;  } }
		sealed class SingleFromSqlString  : Convert<Single,SqlString>  { public override Single From(SqlString p)   { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class SingleFromSqlByte    : Convert<Single,SqlByte>    { public override Single From(SqlByte p)     { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class SingleFromSqlInt16   : Convert<Single,SqlInt16>   { public override Single From(SqlInt16 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class SingleFromSqlInt32   : Convert<Single,SqlInt32>   { public override Single From(SqlInt32 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class SingleFromSqlInt64   : Convert<Single,SqlInt64>   { public override Single From(SqlInt64 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class SingleFromSqlDouble  : Convert<Single,SqlDouble>  { public override Single From(SqlDouble p)   { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class SingleFromSqlDecimal : Convert<Single,SqlDecimal> { public override Single From(SqlDecimal p)  { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class SingleFromSqlMoney   : Convert<Single,SqlMoney>   { public override Single From(SqlMoney p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class SingleFromSqlBoolean : Convert<Single,SqlBoolean> { public override Single From(SqlBoolean p)  { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class SingleDefault<Q>     : Convert<Single,Q>          { public override Single From(Q p)           { return Convert<Single,object>.Instance.From(p); } }
		sealed class SingleFromObject     : Convert<Single,object>     { public override Single From(object p)      { return Convert.ToSingle(p); } }

		static Convert<T, P> GetSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SingleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SingleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SingleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SingleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SingleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SingleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SingleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SingleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SingleFromUInt64());

			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SingleFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SingleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SingleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SingleFromNullableSingle());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SingleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SingleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SingleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SingleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SingleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SingleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SingleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SingleFromNullableUInt64());

			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SingleFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SingleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SingleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SingleFromSqlSingle());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SingleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SingleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SingleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SingleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SingleFromSqlInt64());

			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SingleFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SingleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SingleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SingleFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SingleFromObject());

			return (Convert<T, P>)(object)(new SingleDefault<P>());
		}

		#endregion

		#region Double


		// Scalar Types.
		//
		sealed class DoubleFromString     : Convert<Double,String>     { public override Double From(String p)      { return Convert.ToDouble(p); } }

		sealed class DoubleFromSByte      : Convert<Double,SByte>      { public override Double From(SByte p)       { return Convert.ToDouble(p); } }
		sealed class DoubleFromInt16      : Convert<Double,Int16>      { public override Double From(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class DoubleFromInt32      : Convert<Double,Int32>      { public override Double From(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class DoubleFromInt64      : Convert<Double,Int64>      { public override Double From(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class DoubleFromByte       : Convert<Double,Byte>       { public override Double From(Byte p)        { return Convert.ToDouble(p); } }
		sealed class DoubleFromUInt16     : Convert<Double,UInt16>     { public override Double From(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class DoubleFromUInt32     : Convert<Double,UInt32>     { public override Double From(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class DoubleFromUInt64     : Convert<Double,UInt64>     { public override Double From(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class DoubleFromSingle     : Convert<Double,Single>     { public override Double From(Single p)      { return Convert.ToDouble(p); } }

		sealed class DoubleFromBoolean    : Convert<Double,Boolean>    { public override Double From(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class DoubleFromDecimal    : Convert<Double,Decimal>    { public override Double From(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class DoubleFromNullableDouble     : Convert<Double,Double?>     { public override Double From(Double? p)      { return p.HasValue?                  p.Value : 0; } }

		sealed class DoubleFromNullableSByte      : Convert<Double,SByte?>      { public override Double From(SByte? p)       { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableInt16      : Convert<Double,Int16?>      { public override Double From(Int16? p)       { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableInt32      : Convert<Double,Int32?>      { public override Double From(Int32? p)       { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableInt64      : Convert<Double,Int64?>      { public override Double From(Int64? p)       { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class DoubleFromNullableByte       : Convert<Double,Byte?>       { public override Double From(Byte? p)        { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableUInt16     : Convert<Double,UInt16?>     { public override Double From(UInt16? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableUInt32     : Convert<Double,UInt32?>     { public override Double From(UInt32? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableUInt64     : Convert<Double,UInt64?>     { public override Double From(UInt64? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class DoubleFromNullableSingle     : Convert<Double,Single?>     { public override Double From(Single? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class DoubleFromNullableBoolean    : Convert<Double,Boolean?>    { public override Double From(Boolean? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class DoubleFromNullableDecimal    : Convert<Double,Decimal?>    { public override Double From(Decimal? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class DoubleFromSqlDouble  : Convert<Double,SqlDouble>  { public override Double From(SqlDouble p)   { return p.IsNull? 0:                  p.Value;  } }
		sealed class DoubleFromSqlString  : Convert<Double,SqlString>  { public override Double From(SqlString p)   { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class DoubleFromSqlByte    : Convert<Double,SqlByte>    { public override Double From(SqlByte p)     { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class DoubleFromSqlInt16   : Convert<Double,SqlInt16>   { public override Double From(SqlInt16 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class DoubleFromSqlInt32   : Convert<Double,SqlInt32>   { public override Double From(SqlInt32 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class DoubleFromSqlInt64   : Convert<Double,SqlInt64>   { public override Double From(SqlInt64 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class DoubleFromSqlSingle  : Convert<Double,SqlSingle>  { public override Double From(SqlSingle p)   { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class DoubleFromSqlDecimal : Convert<Double,SqlDecimal> { public override Double From(SqlDecimal p)  { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class DoubleFromSqlMoney   : Convert<Double,SqlMoney>   { public override Double From(SqlMoney p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class DoubleFromSqlBoolean : Convert<Double,SqlBoolean> { public override Double From(SqlBoolean p)  { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class DoubleDefault<Q>     : Convert<Double,Q>          { public override Double From(Q p)           { return Convert<Double,object>.Instance.From(p); } }
		sealed class DoubleFromObject     : Convert<Double,object>     { public override Double From(object p)      { return Convert.ToDouble(p); } }

		static Convert<T, P> GetDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new DoubleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new DoubleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new DoubleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new DoubleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new DoubleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new DoubleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new DoubleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new DoubleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new DoubleFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new DoubleFromSingle());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new DoubleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new DoubleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new DoubleFromNullableDouble());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new DoubleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new DoubleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new DoubleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new DoubleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new DoubleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new DoubleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new DoubleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new DoubleFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new DoubleFromNullableSingle());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new DoubleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new DoubleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new DoubleFromSqlDouble());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new DoubleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new DoubleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new DoubleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new DoubleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new DoubleFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new DoubleFromSqlSingle());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new DoubleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new DoubleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new DoubleFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new DoubleFromObject());

			return (Convert<T, P>)(object)(new DoubleDefault<P>());
		}

		#endregion

		#region Boolean


		// Scalar Types.
		//
		sealed class BooleanFromString     : Convert<Boolean,String>     { public override Boolean From(String p)      { return Convert.ToBoolean(p); } }

		sealed class BooleanFromSByte      : Convert<Boolean,SByte>      { public override Boolean From(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class BooleanFromInt16      : Convert<Boolean,Int16>      { public override Boolean From(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class BooleanFromInt32      : Convert<Boolean,Int32>      { public override Boolean From(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class BooleanFromInt64      : Convert<Boolean,Int64>      { public override Boolean From(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class BooleanFromByte       : Convert<Boolean,Byte>       { public override Boolean From(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class BooleanFromUInt16     : Convert<Boolean,UInt16>     { public override Boolean From(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class BooleanFromUInt32     : Convert<Boolean,UInt32>     { public override Boolean From(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class BooleanFromUInt64     : Convert<Boolean,UInt64>     { public override Boolean From(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class BooleanFromSingle     : Convert<Boolean,Single>     { public override Boolean From(Single p)      { return Convert.ToBoolean(p); } }
		sealed class BooleanFromDouble     : Convert<Boolean,Double>     { public override Boolean From(Double p)      { return Convert.ToBoolean(p); } }

		sealed class BooleanFromDecimal    : Convert<Boolean,Decimal>    { public override Boolean From(Decimal p)     { return Convert.ToBoolean(p); } }

		sealed class BooleanFromChar       : Convert<Boolean,Char>       { public override Boolean From(Char p)       
			{
					switch (p)
					{
					case (Char)0: return false; // Allow int <=> Char <=> Boolean
					case     '0': return false;
					case     'n': return false;
					case     'N': return false;
					case     'f': return false;
					case     'F': return false;

					case (Char)1: return true; // Allow int <=> Char <=> Boolean
					case     '1': return true;
					case     'y': return true;
					case     'Y': return true;
					case     't': return true;
					case     'T': return true;
					}

					// Throw an InvalidCastException
					//
					return Convert.ToBoolean(p);
				
			} }

		// Nullable Types.
		//
		sealed class BooleanFromNullableBoolean    : Convert<Boolean,Boolean?>    { public override Boolean From(Boolean? p)     { return p.HasValue? p.Value                   : false; } }

		sealed class BooleanFromNullableSByte      : Convert<Boolean,SByte?>      { public override Boolean From(SByte? p)       { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableInt16      : Convert<Boolean,Int16?>      { public override Boolean From(Int16? p)       { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableInt32      : Convert<Boolean,Int32?>      { public override Boolean From(Int32? p)       { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableInt64      : Convert<Boolean,Int64?>      { public override Boolean From(Int64? p)       { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class BooleanFromNullableByte       : Convert<Boolean,Byte?>       { public override Boolean From(Byte? p)        { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableUInt16     : Convert<Boolean,UInt16?>     { public override Boolean From(UInt16? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableUInt32     : Convert<Boolean,UInt32?>     { public override Boolean From(UInt32? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableUInt64     : Convert<Boolean,UInt64?>     { public override Boolean From(UInt64? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class BooleanFromNullableSingle     : Convert<Boolean,Single?>     { public override Boolean From(Single? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class BooleanFromNullableDouble     : Convert<Boolean,Double?>     { public override Boolean From(Double? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class BooleanFromNullableDecimal    : Convert<Boolean,Decimal?>    { public override Boolean From(Decimal? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class BooleanFromNullableChar       : Convert<Boolean,Char?>       { public override Boolean From(Char? p)        { return (p.HasValue)? Convert<Boolean,Char>.Instance.From(p.Value): false; } }

		// SqlTypes.
		//
		sealed class BooleanFromSqlBoolean : Convert<Boolean,SqlBoolean> { public override Boolean From(SqlBoolean p)  { return p.IsNull? false:                   p.Value;  } }
		sealed class BooleanFromSqlString  : Convert<Boolean,SqlString>  { public override Boolean From(SqlString p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }

		sealed class BooleanFromSqlByte    : Convert<Boolean,SqlByte>    { public override Boolean From(SqlByte p)     { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlInt16   : Convert<Boolean,SqlInt16>   { public override Boolean From(SqlInt16 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlInt32   : Convert<Boolean,SqlInt32>   { public override Boolean From(SqlInt32 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlInt64   : Convert<Boolean,SqlInt64>   { public override Boolean From(SqlInt64 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }

		sealed class BooleanFromSqlSingle  : Convert<Boolean,SqlSingle>  { public override Boolean From(SqlSingle p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlDouble  : Convert<Boolean,SqlDouble>  { public override Boolean From(SqlDouble p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlDecimal : Convert<Boolean,SqlDecimal> { public override Boolean From(SqlDecimal p)  { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class BooleanFromSqlMoney   : Convert<Boolean,SqlMoney>   { public override Boolean From(SqlMoney p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }


		sealed class BooleanDefault<Q>     : Convert<Boolean,Q>          { public override Boolean From(Q p)           { return Convert<Boolean,object>.Instance.From(p); } }
		sealed class BooleanFromObject     : Convert<Boolean,object>     { public override Boolean From(object p)      { return Convert.ToBoolean(p); } }

		static Convert<T, P> GetBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new BooleanFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new BooleanFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new BooleanFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new BooleanFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new BooleanFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new BooleanFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new BooleanFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new BooleanFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new BooleanFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new BooleanFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new BooleanFromDouble());

			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new BooleanFromDecimal());

			if (t == typeof(Char))        return (Convert<T, P>)(object)(new BooleanFromChar());

			// Nullable Types.
			//
			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new BooleanFromNullableBoolean());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new BooleanFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new BooleanFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new BooleanFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new BooleanFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new BooleanFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new BooleanFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new BooleanFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new BooleanFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new BooleanFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new BooleanFromNullableDouble());

			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new BooleanFromNullableDecimal());

			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new BooleanFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new BooleanFromSqlBoolean());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new BooleanFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new BooleanFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new BooleanFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new BooleanFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new BooleanFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new BooleanFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new BooleanFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new BooleanFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new BooleanFromSqlMoney());


			if (t == typeof(object))      return (Convert<T, P>)(object)(new BooleanFromObject());

			return (Convert<T, P>)(object)(new BooleanDefault<P>());
		}

		#endregion

		#region Decimal


		// Scalar Types.
		//
		sealed class DecimalFromString     : Convert<Decimal,String>     { public override Decimal From(String p)      { return Convert.ToDecimal(p); } }

		sealed class DecimalFromSByte      : Convert<Decimal,SByte>      { public override Decimal From(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class DecimalFromInt16      : Convert<Decimal,Int16>      { public override Decimal From(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class DecimalFromInt32      : Convert<Decimal,Int32>      { public override Decimal From(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class DecimalFromInt64      : Convert<Decimal,Int64>      { public override Decimal From(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class DecimalFromByte       : Convert<Decimal,Byte>       { public override Decimal From(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class DecimalFromUInt16     : Convert<Decimal,UInt16>     { public override Decimal From(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class DecimalFromUInt32     : Convert<Decimal,UInt32>     { public override Decimal From(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class DecimalFromUInt64     : Convert<Decimal,UInt64>     { public override Decimal From(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class DecimalFromSingle     : Convert<Decimal,Single>     { public override Decimal From(Single p)      { return Convert.ToDecimal(p); } }
		sealed class DecimalFromDouble     : Convert<Decimal,Double>     { public override Decimal From(Double p)      { return Convert.ToDecimal(p); } }

		sealed class DecimalFromBoolean    : Convert<Decimal,Boolean>    { public override Decimal From(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class DecimalFromNullableDecimal    : Convert<Decimal,Decimal?>    { public override Decimal From(Decimal? p)     { return p.HasValue?                   p.Value : 0; } }

		sealed class DecimalFromNullableSByte      : Convert<Decimal,SByte?>      { public override Decimal From(SByte? p)       { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableInt16      : Convert<Decimal,Int16?>      { public override Decimal From(Int16? p)       { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableInt32      : Convert<Decimal,Int32?>      { public override Decimal From(Int32? p)       { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableInt64      : Convert<Decimal,Int64?>      { public override Decimal From(Int64? p)       { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class DecimalFromNullableByte       : Convert<Decimal,Byte?>       { public override Decimal From(Byte? p)        { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableUInt16     : Convert<Decimal,UInt16?>     { public override Decimal From(UInt16? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableUInt32     : Convert<Decimal,UInt32?>     { public override Decimal From(UInt32? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableUInt64     : Convert<Decimal,UInt64?>     { public override Decimal From(UInt64? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class DecimalFromNullableSingle     : Convert<Decimal,Single?>     { public override Decimal From(Single? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class DecimalFromNullableDouble     : Convert<Decimal,Double?>     { public override Decimal From(Double? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class DecimalFromNullableBoolean    : Convert<Decimal,Boolean?>    { public override Decimal From(Boolean? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class DecimalFromSqlDecimal : Convert<Decimal,SqlDecimal> { public override Decimal From(SqlDecimal p)  { return p.IsNull? 0:                   p.Value;  } }
		sealed class DecimalFromSqlMoney   : Convert<Decimal,SqlMoney>   { public override Decimal From(SqlMoney p)    { return p.IsNull? 0:                   p.Value;  } }
		sealed class DecimalFromSqlString  : Convert<Decimal,SqlString>  { public override Decimal From(SqlString p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class DecimalFromSqlByte    : Convert<Decimal,SqlByte>    { public override Decimal From(SqlByte p)     { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class DecimalFromSqlInt16   : Convert<Decimal,SqlInt16>   { public override Decimal From(SqlInt16 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class DecimalFromSqlInt32   : Convert<Decimal,SqlInt32>   { public override Decimal From(SqlInt32 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class DecimalFromSqlInt64   : Convert<Decimal,SqlInt64>   { public override Decimal From(SqlInt64 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class DecimalFromSqlSingle  : Convert<Decimal,SqlSingle>  { public override Decimal From(SqlSingle p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class DecimalFromSqlDouble  : Convert<Decimal,SqlDouble>  { public override Decimal From(SqlDouble p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class DecimalFromSqlBoolean : Convert<Decimal,SqlBoolean> { public override Decimal From(SqlBoolean p)  { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class DecimalDefault<Q>     : Convert<Decimal,Q>          { public override Decimal From(Q p)           { return Convert<Decimal,object>.Instance.From(p); } }
		sealed class DecimalFromObject     : Convert<Decimal,object>     { public override Decimal From(object p)      { return Convert.ToDecimal(p); } }

		static Convert<T, P> GetDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new DecimalFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new DecimalFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new DecimalFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new DecimalFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new DecimalFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new DecimalFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new DecimalFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new DecimalFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new DecimalFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new DecimalFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new DecimalFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new DecimalFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new DecimalFromNullableDecimal());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new DecimalFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new DecimalFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new DecimalFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new DecimalFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new DecimalFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new DecimalFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new DecimalFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new DecimalFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new DecimalFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new DecimalFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new DecimalFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new DecimalFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new DecimalFromSqlMoney());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new DecimalFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new DecimalFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new DecimalFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new DecimalFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new DecimalFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new DecimalFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new DecimalFromSqlDouble());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new DecimalFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new DecimalFromObject());

			return (Convert<T, P>)(object)(new DecimalDefault<P>());
		}

		#endregion

		#region DateTime


		// Scalar Types.
		//
		sealed class DateTimeFromString     : Convert<DateTime,String>     { public override DateTime From(String p)      { return Convert.ToDateTime(p); } }

		sealed class DateTimeFromSByte      : Convert<DateTime,SByte>      { public override DateTime From(SByte p)       { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromInt16      : Convert<DateTime,Int16>      { public override DateTime From(Int16 p)       { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromInt32      : Convert<DateTime,Int32>      { public override DateTime From(Int32 p)       { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromInt64      : Convert<DateTime,Int64>      { public override DateTime From(Int64 p)       { return Convert.ToDateTime(p); } }

		sealed class DateTimeFromByte       : Convert<DateTime,Byte>       { public override DateTime From(Byte p)        { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromUInt16     : Convert<DateTime,UInt16>     { public override DateTime From(UInt16 p)      { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromUInt32     : Convert<DateTime,UInt32>     { public override DateTime From(UInt32 p)      { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromUInt64     : Convert<DateTime,UInt64>     { public override DateTime From(UInt64 p)      { return Convert.ToDateTime(p); } }

		sealed class DateTimeFromSingle     : Convert<DateTime,Single>     { public override DateTime From(Single p)      { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromDouble     : Convert<DateTime,Double>     { public override DateTime From(Double p)      { return Convert.ToDateTime(p); } }

		sealed class DateTimeFromBoolean    : Convert<DateTime,Boolean>    { public override DateTime From(Boolean p)     { return Convert.ToDateTime(p); } }
		sealed class DateTimeFromDecimal    : Convert<DateTime,Decimal>    { public override DateTime From(Decimal p)     { return Convert.ToDateTime(p); } }

		// Nullable Types.
		//
		sealed class DateTimeFromNullableDateTime   : Convert<DateTime,DateTime?>   { public override DateTime From(DateTime? p)    { return p.HasValue?                    p.Value : DateTime.MinValue; } }

		sealed class DateTimeFromNullableSByte      : Convert<DateTime,SByte?>      { public override DateTime From(SByte? p)       { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableInt16      : Convert<DateTime,Int16?>      { public override DateTime From(Int16? p)       { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableInt32      : Convert<DateTime,Int32?>      { public override DateTime From(Int32? p)       { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableInt64      : Convert<DateTime,Int64?>      { public override DateTime From(Int64? p)       { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DateTimeFromNullableByte       : Convert<DateTime,Byte?>       { public override DateTime From(Byte? p)        { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableUInt16     : Convert<DateTime,UInt16?>     { public override DateTime From(UInt16? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableUInt32     : Convert<DateTime,UInt32?>     { public override DateTime From(UInt32? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableUInt64     : Convert<DateTime,UInt64?>     { public override DateTime From(UInt64? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DateTimeFromNullableSingle     : Convert<DateTime,Single?>     { public override DateTime From(Single? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableDouble     : Convert<DateTime,Double?>     { public override DateTime From(Double? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DateTimeFromNullableBoolean    : Convert<DateTime,Boolean?>    { public override DateTime From(Boolean? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DateTimeFromNullableDecimal    : Convert<DateTime,Decimal?>    { public override DateTime From(Decimal? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		// SqlTypes.
		//
		sealed class DateTimeFromSqlDateTime: Convert<DateTime,SqlDateTime>{ public override DateTime From(SqlDateTime p) { return p.IsNull? DateTime.MinValue:                    p.Value;  } }
		sealed class DateTimeFromSqlString  : Convert<DateTime,SqlString>  { public override DateTime From(SqlString p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DateTimeFromSqlByte    : Convert<DateTime,SqlByte>    { public override DateTime From(SqlByte p)     { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlInt16   : Convert<DateTime,SqlInt16>   { public override DateTime From(SqlInt16 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlInt32   : Convert<DateTime,SqlInt32>   { public override DateTime From(SqlInt32 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlInt64   : Convert<DateTime,SqlInt64>   { public override DateTime From(SqlInt64 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DateTimeFromSqlSingle  : Convert<DateTime,SqlSingle>  { public override DateTime From(SqlSingle p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlDouble  : Convert<DateTime,SqlDouble>  { public override DateTime From(SqlDouble p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlDecimal : Convert<DateTime,SqlDecimal> { public override DateTime From(SqlDecimal p)  { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DateTimeFromSqlMoney   : Convert<DateTime,SqlMoney>   { public override DateTime From(SqlMoney p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DateTimeFromSqlBoolean : Convert<DateTime,SqlBoolean> { public override DateTime From(SqlBoolean p)  { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DateTimeDefault<Q>     : Convert<DateTime,Q>          { public override DateTime From(Q p)           { return Convert<DateTime,object>.Instance.From(p); } }
		sealed class DateTimeFromObject     : Convert<DateTime,object>     { public override DateTime From(object p)      { return Convert.ToDateTime(p); } }

		static Convert<T, P> GetDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new DateTimeFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new DateTimeFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new DateTimeFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new DateTimeFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new DateTimeFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new DateTimeFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new DateTimeFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new DateTimeFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new DateTimeFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new DateTimeFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new DateTimeFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new DateTimeFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new DateTimeFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new DateTimeFromNullableDateTime());

			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new DateTimeFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new DateTimeFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new DateTimeFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new DateTimeFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new DateTimeFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new DateTimeFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new DateTimeFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new DateTimeFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new DateTimeFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new DateTimeFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new DateTimeFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new DateTimeFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new DateTimeFromSqlDateTime());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new DateTimeFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new DateTimeFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new DateTimeFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new DateTimeFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new DateTimeFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new DateTimeFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new DateTimeFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new DateTimeFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new DateTimeFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new DateTimeFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new DateTimeFromObject());

			return (Convert<T, P>)(object)(new DateTimeDefault<P>());
		}

		#endregion

		#region TimeSpan


		// Scalar Types.
		//
		sealed class TimeSpanFromString     : Convert<TimeSpan,String>     { public override TimeSpan From(String p)      { return p == null? TimeSpan.MinValue: TimeSpan.Parse(p); } }
		sealed class TimeSpanFromDateTime   : Convert<TimeSpan,DateTime>   { public override TimeSpan From(DateTime p)    { return p - DateTime.MinValue; } }

		// Nullable Types.
		//
		sealed class TimeSpanFromNullableTimeSpan   : Convert<TimeSpan,TimeSpan?>   { public override TimeSpan From(TimeSpan? p)    { return p.HasValue? p.Value                    : TimeSpan.MinValue; } }
		sealed class TimeSpanFromNullableDateTime   : Convert<TimeSpan,DateTime?>   { public override TimeSpan From(DateTime? p)    { return p.HasValue? p.Value - DateTime.MinValue: TimeSpan.MinValue; } }

		// SqlTypes.
		//
		sealed class TimeSpanFromSqlString  : Convert<TimeSpan,SqlString>  { public override TimeSpan From(SqlString p)   { return p.IsNull? TimeSpan.MinValue: TimeSpan.Parse(p.Value);     } }
		sealed class TimeSpanFromSqlDateTime: Convert<TimeSpan,SqlDateTime>{ public override TimeSpan From(SqlDateTime p) { return p.IsNull? TimeSpan.MinValue: p.Value - DateTime.MinValue; } }

		sealed class TimeSpanDefault<Q>     : Convert<TimeSpan,Q>          { public override TimeSpan From(Q p)           { return Convert<TimeSpan,object>.Instance.From(p); } }
		sealed class TimeSpanFromObject     : Convert<TimeSpan,object>     { public override TimeSpan From(object p)     
			{
				if (p == null)
					return TimeSpan.MinValue;

				// Scalar Types.
				//
				if (p is String)      return Convert<TimeSpan,String>     .Instance.From((String)p);
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .Instance.From((DateTime)p);

				// Nullable Types.
				//
				if (p is TimeSpan)    return Convert<TimeSpan,TimeSpan>   .Instance.From((TimeSpan)p);
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .Instance.From((DateTime)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<TimeSpan,SqlString>  .Instance.From((SqlString)p);
				if (p is SqlDateTime) return Convert<TimeSpan,SqlDateTime>.Instance.From((SqlDateTime)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetTimeSpanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new TimeSpanFromString());
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new TimeSpanFromDateTime());

			// Nullable Types.
			//
			if (t == typeof(TimeSpan?))    return (Convert<T, P>)(object)(new TimeSpanFromNullableTimeSpan());
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new TimeSpanFromNullableDateTime());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new TimeSpanFromSqlString());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new TimeSpanFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new TimeSpanFromObject());

			return (Convert<T, P>)(object)(new TimeSpanDefault<P>());
		}

		#endregion

		#region Guid


		// Scalar Types.
		//
		sealed class GuidFromString     : Convert<Guid,String>     { public override Guid From(String p)      { return p == null? Guid.Empty: new Guid(p); } }

		// Nullable Types.
		//
		sealed class GuidFromNullableGuid       : Convert<Guid,Guid?>       { public override Guid From(Guid? p)        { return p.HasValue? p.Value : Guid.Empty; } }

		// SqlTypes.
		//
		sealed class GuidFromSqlGuid    : Convert<Guid,SqlGuid>    { public override Guid From(SqlGuid p)     { return p.IsNull? Guid.Empty: p.Value;             } }
		sealed class GuidFromSqlString  : Convert<Guid,SqlString>  { public override Guid From(SqlString p)   { return p.IsNull? Guid.Empty: new Guid(p.Value);   } }
		sealed class GuidFromSqlBinary  : Convert<Guid,SqlBinary>  { public override Guid From(SqlBinary p)   { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; } }
		sealed class GuidFromType       : Convert<Guid,Type>       { public override Guid From(Type p)        { return p == null? Guid.Empty: p.GUID; } }

		sealed class GuidDefault<Q>     : Convert<Guid,Q>          { public override Guid From(Q p)           { return Convert<Guid,object>.Instance.From(p); } }
		sealed class GuidFromObject     : Convert<Guid,object>     { public override Guid From(object p)     
			{
				if (p == null)
					return Guid.Empty;

				// Scalar Types.
				//
				if (p is String)      return Convert<Guid,String>     .Instance.From((String)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Guid,Guid>       .Instance.From((Guid)p);

				// SqlTypes.
				//
				if (p is SqlGuid)     return Convert<Guid,SqlGuid>    .Instance.From((SqlGuid)p);
				if (p is SqlString)   return Convert<Guid,SqlString>  .Instance.From((SqlString)p);
				if (p is SqlBinary)   return Convert<Guid,SqlBinary>  .Instance.From((SqlBinary)p);
				if (p is Type)        return Convert<Guid,Type>       .Instance.From((Type)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new GuidFromString());

			// Nullable Types.
			//
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new GuidFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new GuidFromSqlGuid());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new GuidFromSqlString());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new GuidFromSqlBinary());
			if (t == typeof(Type))        return (Convert<T, P>)(object)(new GuidFromType());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new GuidFromObject());

			return (Convert<T, P>)(object)(new GuidDefault<P>());
		}

		#endregion

		#region Stream


		// Scalar Types.
		//
		sealed class StreamFromByteArray  : Convert<Stream,Byte[]>     { public override Stream From(Byte[] p)      { return p == null? Stream.Null: new MemoryStream(p); } }

		// SqlTypes.
		//
		sealed class StreamFromSqlBytes   : Convert<Stream,SqlBytes>   { public override Stream From(SqlBytes p)    { return p.IsNull? Stream.Null: p.Stream;                  } }
		sealed class StreamFromSqlBinary  : Convert<Stream,SqlBinary>  { public override Stream From(SqlBinary p)   { return p.IsNull? Stream.Null: new MemoryStream(p.Value); } }

		sealed class StreamDefault<Q>     : Convert<Stream,Q>          { public override Stream From(Q p)           { return Convert<Stream,object>.Instance.From(p); } }
		sealed class StreamFromObject     : Convert<Stream,object>     { public override Stream From(object p)     
			{
				if (p == null)
					return Stream.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<Stream,Byte[]>     .Instance.From((Byte[])p);

				// SqlTypes.
				//
				if (p is SqlBytes)    return Convert<Stream,SqlBytes>   .Instance.From((SqlBytes)p);
				if (p is SqlBinary)   return Convert<Stream,SqlBinary>  .Instance.From((SqlBinary)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetStreamConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (Convert<T, P>)(object)(new StreamFromByteArray());

			// SqlTypes.
			//
			if (t == typeof(SqlBytes))    return (Convert<T, P>)(object)(new StreamFromSqlBytes());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new StreamFromSqlBinary());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new StreamFromObject());

			return (Convert<T, P>)(object)(new StreamDefault<P>());
		}

		#endregion

		#endregion

		#region Nullable Types

		#region SByte?


		// Scalar Types.
		//
		sealed class NullableSByteFromSByte      : Convert<SByte?,SByte>      { public override SByte? From(SByte p)       { return p; } }
		sealed class NullableSByteFromString     : Convert<SByte?,String>     { public override SByte? From(String p)      { return p == null? null: (SByte?)Convert.ToSByte(p); } }

		sealed class NullableSByteFromInt16      : Convert<SByte?,Int16>      { public override SByte? From(Int16 p)       { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromInt32      : Convert<SByte?,Int32>      { public override SByte? From(Int32 p)       { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromInt64      : Convert<SByte?,Int64>      { public override SByte? From(Int64 p)       { return Convert.ToSByte(p); } }

		sealed class NullableSByteFromByte       : Convert<SByte?,Byte>       { public override SByte? From(Byte p)        { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromUInt16     : Convert<SByte?,UInt16>     { public override SByte? From(UInt16 p)      { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromUInt32     : Convert<SByte?,UInt32>     { public override SByte? From(UInt32 p)      { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromUInt64     : Convert<SByte?,UInt64>     { public override SByte? From(UInt64 p)      { return Convert.ToSByte(p); } }

		sealed class NullableSByteFromSingle     : Convert<SByte?,Single>     { public override SByte? From(Single p)      { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromDouble     : Convert<SByte?,Double>     { public override SByte? From(Double p)      { return Convert.ToSByte(p); } }

		sealed class NullableSByteFromBoolean    : Convert<SByte?,Boolean>    { public override SByte? From(Boolean p)     { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromDecimal    : Convert<SByte?,Decimal>    { public override SByte? From(Decimal p)     { return Convert.ToSByte(p); } }
		sealed class NullableSByteFromChar       : Convert<SByte?,Char>       { public override SByte? From(Char p)        { return Convert.ToSByte(p); } }

		// Nullable Types.
		//
		sealed class NullableSByteFromNullableInt16      : Convert<SByte?,Int16?>      { public override SByte? From(Int16? p)       { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableInt32      : Convert<SByte?,Int32?>      { public override SByte? From(Int32? p)       { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableInt64      : Convert<SByte?,Int64?>      { public override SByte? From(Int64? p)       { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NullableSByteFromNullableByte       : Convert<SByte?,Byte?>       { public override SByte? From(Byte? p)        { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableUInt16     : Convert<SByte?,UInt16?>     { public override SByte? From(UInt16? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableUInt32     : Convert<SByte?,UInt32?>     { public override SByte? From(UInt32? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableUInt64     : Convert<SByte?,UInt64?>     { public override SByte? From(UInt64? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NullableSByteFromNullableSingle     : Convert<SByte?,Single?>     { public override SByte? From(Single? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableDouble     : Convert<SByte?,Double?>     { public override SByte? From(Double? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NullableSByteFromNullableBoolean    : Convert<SByte?,Boolean?>    { public override SByte? From(Boolean? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableDecimal    : Convert<SByte?,Decimal?>    { public override SByte? From(Decimal? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NullableSByteFromNullableChar       : Convert<SByte?,Char?>       { public override SByte? From(Char? p)        { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableSByteFromSqlString  : Convert<SByte?,SqlString>  { public override SByte? From(SqlString p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NullableSByteFromSqlByte    : Convert<SByte?,SqlByte>    { public override SByte? From(SqlByte p)     { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlInt16   : Convert<SByte?,SqlInt16>   { public override SByte? From(SqlInt16 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlInt32   : Convert<SByte?,SqlInt32>   { public override SByte? From(SqlInt32 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlInt64   : Convert<SByte?,SqlInt64>   { public override SByte? From(SqlInt64 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NullableSByteFromSqlSingle  : Convert<SByte?,SqlSingle>  { public override SByte? From(SqlSingle p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlDouble  : Convert<SByte?,SqlDouble>  { public override SByte? From(SqlDouble p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlDecimal : Convert<SByte?,SqlDecimal> { public override SByte? From(SqlDecimal p)  { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NullableSByteFromSqlMoney   : Convert<SByte?,SqlMoney>   { public override SByte? From(SqlMoney p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NullableSByteFromSqlBoolean : Convert<SByte?,SqlBoolean> { public override SByte? From(SqlBoolean p)  { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NullableSByteDefault<Q>     : Convert<SByte?,Q>          { public override SByte? From(Q p)           { return Convert<SByte?,object>.Instance.From(p); } }
		sealed class NullableSByteFromObject     : Convert<SByte?,object>     { public override SByte? From(object p)      { return Convert.ToSByte(p); } }

		static Convert<T, P> GetNullableSByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableSByteFromSByte());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableSByteFromString());

			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableSByteFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableSByteFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableSByteFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableSByteFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableSByteFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableSByteFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableSByteFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableSByteFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableSByteFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableSByteFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableSByteFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableSByteFromChar());

			// Nullable Types.
			//
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableSByteFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableSByteFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableSByteFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableSByteFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableSByteFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableSByteFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableSByteFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableSByteFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableSByteFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableSByteFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableSByteFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableSByteFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableSByteFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableSByteFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableSByteFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableSByteFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableSByteFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableSByteFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableSByteFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableSByteFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableSByteFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableSByteFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableSByteFromObject());

			return (Convert<T, P>)(object)(new NullableSByteDefault<P>());
		}

		#endregion

		#region Int16?


		// Scalar Types.
		//
		sealed class NullableInt16FromInt16      : Convert<Int16?,Int16>      { public override Int16? From(Int16 p)       { return p; } }
		sealed class NullableInt16FromString     : Convert<Int16?,String>     { public override Int16? From(String p)      { return p == null? null: (Int16?)Convert.ToInt16(p); } }

		sealed class NullableInt16FromSByte      : Convert<Int16?,SByte>      { public override Int16? From(SByte p)       { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromInt32      : Convert<Int16?,Int32>      { public override Int16? From(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromInt64      : Convert<Int16?,Int64>      { public override Int16? From(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class NullableInt16FromByte       : Convert<Int16?,Byte>       { public override Int16? From(Byte p)        { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromUInt16     : Convert<Int16?,UInt16>     { public override Int16? From(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromUInt32     : Convert<Int16?,UInt32>     { public override Int16? From(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromUInt64     : Convert<Int16?,UInt64>     { public override Int16? From(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class NullableInt16FromSingle     : Convert<Int16?,Single>     { public override Int16? From(Single p)      { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromDouble     : Convert<Int16?,Double>     { public override Int16? From(Double p)      { return Convert.ToInt16(p); } }

		sealed class NullableInt16FromBoolean    : Convert<Int16?,Boolean>    { public override Int16? From(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromDecimal    : Convert<Int16?,Decimal>    { public override Int16? From(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class NullableInt16FromChar       : Convert<Int16?,Char>       { public override Int16? From(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class NullableInt16FromNullableSByte      : Convert<Int16?,SByte?>      { public override Int16? From(SByte? p)       { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableInt32      : Convert<Int16?,Int32?>      { public override Int16? From(Int32? p)       { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableInt64      : Convert<Int16?,Int64?>      { public override Int16? From(Int64? p)       { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NullableInt16FromNullableByte       : Convert<Int16?,Byte?>       { public override Int16? From(Byte? p)        { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableUInt16     : Convert<Int16?,UInt16?>     { public override Int16? From(UInt16? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableUInt32     : Convert<Int16?,UInt32?>     { public override Int16? From(UInt32? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableUInt64     : Convert<Int16?,UInt64?>     { public override Int16? From(UInt64? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NullableInt16FromNullableSingle     : Convert<Int16?,Single?>     { public override Int16? From(Single? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableDouble     : Convert<Int16?,Double?>     { public override Int16? From(Double? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NullableInt16FromNullableBoolean    : Convert<Int16?,Boolean?>    { public override Int16? From(Boolean? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableDecimal    : Convert<Int16?,Decimal?>    { public override Int16? From(Decimal? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NullableInt16FromNullableChar       : Convert<Int16?,Char?>       { public override Int16? From(Char? p)        { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableInt16FromSqlInt16   : Convert<Int16?,SqlInt16>   { public override Int16? From(SqlInt16 p)    { return p.IsNull? null: (Int16?)                p.Value;  } }
		sealed class NullableInt16FromSqlString  : Convert<Int16?,SqlString>  { public override Int16? From(SqlString p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NullableInt16FromSqlByte    : Convert<Int16?,SqlByte>    { public override Int16? From(SqlByte p)     { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NullableInt16FromSqlInt32   : Convert<Int16?,SqlInt32>   { public override Int16? From(SqlInt32 p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NullableInt16FromSqlInt64   : Convert<Int16?,SqlInt64>   { public override Int16? From(SqlInt64 p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NullableInt16FromSqlSingle  : Convert<Int16?,SqlSingle>  { public override Int16? From(SqlSingle p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NullableInt16FromSqlDouble  : Convert<Int16?,SqlDouble>  { public override Int16? From(SqlDouble p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NullableInt16FromSqlDecimal : Convert<Int16?,SqlDecimal> { public override Int16? From(SqlDecimal p)  { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NullableInt16FromSqlMoney   : Convert<Int16?,SqlMoney>   { public override Int16? From(SqlMoney p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NullableInt16FromSqlBoolean : Convert<Int16?,SqlBoolean> { public override Int16? From(SqlBoolean p)  { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NullableInt16Default<Q>     : Convert<Int16?,Q>          { public override Int16? From(Q p)           { return Convert<Int16?,object>.Instance.From(p); } }
		sealed class NullableInt16FromObject     : Convert<Int16?,object>     { public override Int16? From(object p)      { return Convert.ToInt16(p); } }

		static Convert<T, P> GetNullableInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableInt16FromInt16());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableInt16FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableInt16FromSByte());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableInt16FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableInt16FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableInt16FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableInt16FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableInt16FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableInt16FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableInt16FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableInt16FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableInt16FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableInt16FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableInt16FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableInt16FromNullableSByte());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableInt16FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableInt16FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableInt16FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableInt16FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableInt16FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableInt16FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableInt16FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableInt16FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableInt16FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableInt16FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableInt16FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableInt16FromSqlInt16());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableInt16FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableInt16FromSqlByte());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableInt16FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableInt16FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableInt16FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableInt16FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableInt16FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableInt16FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableInt16FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableInt16FromObject());

			return (Convert<T, P>)(object)(new NullableInt16Default<P>());
		}

		#endregion

		#region Int32?


		// Scalar Types.
		//
		sealed class NullableInt32FromInt32      : Convert<Int32?,Int32>      { public override Int32? From(Int32 p)       { return p; } }
		sealed class NullableInt32FromString     : Convert<Int32?,String>     { public override Int32? From(String p)      { return p == null? null: (Int32?)Convert.ToInt32(p); } }

		sealed class NullableInt32FromSByte      : Convert<Int32?,SByte>      { public override Int32? From(SByte p)       { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromInt16      : Convert<Int32?,Int16>      { public override Int32? From(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromInt64      : Convert<Int32?,Int64>      { public override Int32? From(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class NullableInt32FromByte       : Convert<Int32?,Byte>       { public override Int32? From(Byte p)        { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromUInt16     : Convert<Int32?,UInt16>     { public override Int32? From(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromUInt32     : Convert<Int32?,UInt32>     { public override Int32? From(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromUInt64     : Convert<Int32?,UInt64>     { public override Int32? From(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class NullableInt32FromSingle     : Convert<Int32?,Single>     { public override Int32? From(Single p)      { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromDouble     : Convert<Int32?,Double>     { public override Int32? From(Double p)      { return Convert.ToInt32(p); } }

		sealed class NullableInt32FromBoolean    : Convert<Int32?,Boolean>    { public override Int32? From(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromDecimal    : Convert<Int32?,Decimal>    { public override Int32? From(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class NullableInt32FromChar       : Convert<Int32?,Char>       { public override Int32? From(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class NullableInt32FromNullableSByte      : Convert<Int32?,SByte?>      { public override Int32? From(SByte? p)       { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableInt16      : Convert<Int32?,Int16?>      { public override Int32? From(Int16? p)       { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableInt64      : Convert<Int32?,Int64?>      { public override Int32? From(Int64? p)       { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NullableInt32FromNullableByte       : Convert<Int32?,Byte?>       { public override Int32? From(Byte? p)        { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableUInt16     : Convert<Int32?,UInt16?>     { public override Int32? From(UInt16? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableUInt32     : Convert<Int32?,UInt32?>     { public override Int32? From(UInt32? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableUInt64     : Convert<Int32?,UInt64?>     { public override Int32? From(UInt64? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NullableInt32FromNullableSingle     : Convert<Int32?,Single?>     { public override Int32? From(Single? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableDouble     : Convert<Int32?,Double?>     { public override Int32? From(Double? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NullableInt32FromNullableBoolean    : Convert<Int32?,Boolean?>    { public override Int32? From(Boolean? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableDecimal    : Convert<Int32?,Decimal?>    { public override Int32? From(Decimal? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NullableInt32FromNullableChar       : Convert<Int32?,Char?>       { public override Int32? From(Char? p)        { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableInt32FromSqlInt32   : Convert<Int32?,SqlInt32>   { public override Int32? From(SqlInt32 p)    { return p.IsNull? null: (Int32?)                p.Value;  } }
		sealed class NullableInt32FromSqlString  : Convert<Int32?,SqlString>  { public override Int32? From(SqlString p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NullableInt32FromSqlByte    : Convert<Int32?,SqlByte>    { public override Int32? From(SqlByte p)     { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NullableInt32FromSqlInt16   : Convert<Int32?,SqlInt16>   { public override Int32? From(SqlInt16 p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NullableInt32FromSqlInt64   : Convert<Int32?,SqlInt64>   { public override Int32? From(SqlInt64 p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NullableInt32FromSqlSingle  : Convert<Int32?,SqlSingle>  { public override Int32? From(SqlSingle p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NullableInt32FromSqlDouble  : Convert<Int32?,SqlDouble>  { public override Int32? From(SqlDouble p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NullableInt32FromSqlDecimal : Convert<Int32?,SqlDecimal> { public override Int32? From(SqlDecimal p)  { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NullableInt32FromSqlMoney   : Convert<Int32?,SqlMoney>   { public override Int32? From(SqlMoney p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NullableInt32FromSqlBoolean : Convert<Int32?,SqlBoolean> { public override Int32? From(SqlBoolean p)  { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NullableInt32Default<Q>     : Convert<Int32?,Q>          { public override Int32? From(Q p)           { return Convert<Int32?,object>.Instance.From(p); } }
		sealed class NullableInt32FromObject     : Convert<Int32?,object>     { public override Int32? From(object p)      { return Convert.ToInt32(p); } }

		static Convert<T, P> GetNullableInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableInt32FromInt32());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableInt32FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableInt32FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableInt32FromInt16());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableInt32FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableInt32FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableInt32FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableInt32FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableInt32FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableInt32FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableInt32FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableInt32FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableInt32FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableInt32FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableInt32FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableInt32FromNullableInt16());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableInt32FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableInt32FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableInt32FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableInt32FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableInt32FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableInt32FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableInt32FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableInt32FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableInt32FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableInt32FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableInt32FromSqlInt32());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableInt32FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableInt32FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableInt32FromSqlInt16());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableInt32FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableInt32FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableInt32FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableInt32FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableInt32FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableInt32FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableInt32FromObject());

			return (Convert<T, P>)(object)(new NullableInt32Default<P>());
		}

		#endregion

		#region Int64?


		// Scalar Types.
		//
		sealed class NullableInt64FromInt64      : Convert<Int64?,Int64>      { public override Int64? From(Int64 p)       { return p; } }
		sealed class NullableInt64FromString     : Convert<Int64?,String>     { public override Int64? From(String p)      { return p == null? null: (Int64?)Convert.ToInt64(p); } }

		sealed class NullableInt64FromSByte      : Convert<Int64?,SByte>      { public override Int64? From(SByte p)       { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromInt16      : Convert<Int64?,Int16>      { public override Int64? From(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromInt32      : Convert<Int64?,Int32>      { public override Int64? From(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class NullableInt64FromByte       : Convert<Int64?,Byte>       { public override Int64? From(Byte p)        { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromUInt16     : Convert<Int64?,UInt16>     { public override Int64? From(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromUInt32     : Convert<Int64?,UInt32>     { public override Int64? From(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromUInt64     : Convert<Int64?,UInt64>     { public override Int64? From(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class NullableInt64FromSingle     : Convert<Int64?,Single>     { public override Int64? From(Single p)      { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromDouble     : Convert<Int64?,Double>     { public override Int64? From(Double p)      { return Convert.ToInt64(p); } }

		sealed class NullableInt64FromBoolean    : Convert<Int64?,Boolean>    { public override Int64? From(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromDecimal    : Convert<Int64?,Decimal>    { public override Int64? From(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class NullableInt64FromChar       : Convert<Int64?,Char>       { public override Int64? From(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class NullableInt64FromNullableSByte      : Convert<Int64?,SByte?>      { public override Int64? From(SByte? p)       { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableInt16      : Convert<Int64?,Int16?>      { public override Int64? From(Int16? p)       { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableInt32      : Convert<Int64?,Int32?>      { public override Int64? From(Int32? p)       { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NullableInt64FromNullableByte       : Convert<Int64?,Byte?>       { public override Int64? From(Byte? p)        { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableUInt16     : Convert<Int64?,UInt16?>     { public override Int64? From(UInt16? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableUInt32     : Convert<Int64?,UInt32?>     { public override Int64? From(UInt32? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableUInt64     : Convert<Int64?,UInt64?>     { public override Int64? From(UInt64? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NullableInt64FromNullableSingle     : Convert<Int64?,Single?>     { public override Int64? From(Single? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableDouble     : Convert<Int64?,Double?>     { public override Int64? From(Double? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NullableInt64FromNullableBoolean    : Convert<Int64?,Boolean?>    { public override Int64? From(Boolean? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableDecimal    : Convert<Int64?,Decimal?>    { public override Int64? From(Decimal? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NullableInt64FromNullableChar       : Convert<Int64?,Char?>       { public override Int64? From(Char? p)        { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableInt64FromSqlInt64   : Convert<Int64?,SqlInt64>   { public override Int64? From(SqlInt64 p)    { return p.IsNull? null: (Int64?)                p.Value;  } }
		sealed class NullableInt64FromSqlString  : Convert<Int64?,SqlString>  { public override Int64? From(SqlString p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NullableInt64FromSqlByte    : Convert<Int64?,SqlByte>    { public override Int64? From(SqlByte p)     { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NullableInt64FromSqlInt16   : Convert<Int64?,SqlInt16>   { public override Int64? From(SqlInt16 p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NullableInt64FromSqlInt32   : Convert<Int64?,SqlInt32>   { public override Int64? From(SqlInt32 p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NullableInt64FromSqlSingle  : Convert<Int64?,SqlSingle>  { public override Int64? From(SqlSingle p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NullableInt64FromSqlDouble  : Convert<Int64?,SqlDouble>  { public override Int64? From(SqlDouble p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NullableInt64FromSqlDecimal : Convert<Int64?,SqlDecimal> { public override Int64? From(SqlDecimal p)  { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NullableInt64FromSqlMoney   : Convert<Int64?,SqlMoney>   { public override Int64? From(SqlMoney p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NullableInt64FromSqlBoolean : Convert<Int64?,SqlBoolean> { public override Int64? From(SqlBoolean p)  { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NullableInt64Default<Q>     : Convert<Int64?,Q>          { public override Int64? From(Q p)           { return Convert<Int64?,object>.Instance.From(p); } }
		sealed class NullableInt64FromObject     : Convert<Int64?,object>     { public override Int64? From(object p)      { return Convert.ToInt64(p); } }

		static Convert<T, P> GetNullableInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableInt64FromInt64());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableInt64FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableInt64FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableInt64FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableInt64FromInt32());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableInt64FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableInt64FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableInt64FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableInt64FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableInt64FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableInt64FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableInt64FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableInt64FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableInt64FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableInt64FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableInt64FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableInt64FromNullableInt32());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableInt64FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableInt64FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableInt64FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableInt64FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableInt64FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableInt64FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableInt64FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableInt64FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableInt64FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableInt64FromSqlInt64());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableInt64FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableInt64FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableInt64FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableInt64FromSqlInt32());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableInt64FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableInt64FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableInt64FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableInt64FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableInt64FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableInt64FromObject());

			return (Convert<T, P>)(object)(new NullableInt64Default<P>());
		}

		#endregion

		#region Byte?


		// Scalar Types.
		//
		sealed class NullableByteFromByte       : Convert<Byte?,Byte>       { public override Byte? From(Byte p)        { return p; } }
		sealed class NullableByteFromString     : Convert<Byte?,String>     { public override Byte? From(String p)      { return p == null? null: (Byte?)Convert.ToByte(p); } }

		sealed class NullableByteFromSByte      : Convert<Byte?,SByte>      { public override Byte? From(SByte p)       { return Convert.ToByte(p); } }
		sealed class NullableByteFromInt16      : Convert<Byte?,Int16>      { public override Byte? From(Int16 p)       { return Convert.ToByte(p); } }
		sealed class NullableByteFromInt32      : Convert<Byte?,Int32>      { public override Byte? From(Int32 p)       { return Convert.ToByte(p); } }
		sealed class NullableByteFromInt64      : Convert<Byte?,Int64>      { public override Byte? From(Int64 p)       { return Convert.ToByte(p); } }

		sealed class NullableByteFromUInt16     : Convert<Byte?,UInt16>     { public override Byte? From(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class NullableByteFromUInt32     : Convert<Byte?,UInt32>     { public override Byte? From(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class NullableByteFromUInt64     : Convert<Byte?,UInt64>     { public override Byte? From(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class NullableByteFromSingle     : Convert<Byte?,Single>     { public override Byte? From(Single p)      { return Convert.ToByte(p); } }
		sealed class NullableByteFromDouble     : Convert<Byte?,Double>     { public override Byte? From(Double p)      { return Convert.ToByte(p); } }

		sealed class NullableByteFromBoolean    : Convert<Byte?,Boolean>    { public override Byte? From(Boolean p)     { return Convert.ToByte(p); } }
		sealed class NullableByteFromDecimal    : Convert<Byte?,Decimal>    { public override Byte? From(Decimal p)     { return Convert.ToByte(p); } }
		sealed class NullableByteFromChar       : Convert<Byte?,Char>       { public override Byte? From(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class NullableByteFromNullableSByte      : Convert<Byte?,SByte?>      { public override Byte? From(SByte? p)       { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableInt16      : Convert<Byte?,Int16?>      { public override Byte? From(Int16? p)       { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableInt32      : Convert<Byte?,Int32?>      { public override Byte? From(Int32? p)       { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableInt64      : Convert<Byte?,Int64?>      { public override Byte? From(Int64? p)       { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NullableByteFromNullableUInt16     : Convert<Byte?,UInt16?>     { public override Byte? From(UInt16? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableUInt32     : Convert<Byte?,UInt32?>     { public override Byte? From(UInt32? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableUInt64     : Convert<Byte?,UInt64?>     { public override Byte? From(UInt64? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NullableByteFromNullableSingle     : Convert<Byte?,Single?>     { public override Byte? From(Single? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableDouble     : Convert<Byte?,Double?>     { public override Byte? From(Double? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NullableByteFromNullableBoolean    : Convert<Byte?,Boolean?>    { public override Byte? From(Boolean? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableDecimal    : Convert<Byte?,Decimal?>    { public override Byte? From(Decimal? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NullableByteFromNullableChar       : Convert<Byte?,Char?>       { public override Byte? From(Char? p)        { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableByteFromSqlByte    : Convert<Byte?,SqlByte>    { public override Byte? From(SqlByte p)     { return p.IsNull? null: (Byte?)               p.Value;  } }
		sealed class NullableByteFromSqlString  : Convert<Byte?,SqlString>  { public override Byte? From(SqlString p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NullableByteFromSqlInt16   : Convert<Byte?,SqlInt16>   { public override Byte? From(SqlInt16 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NullableByteFromSqlInt32   : Convert<Byte?,SqlInt32>   { public override Byte? From(SqlInt32 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NullableByteFromSqlInt64   : Convert<Byte?,SqlInt64>   { public override Byte? From(SqlInt64 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NullableByteFromSqlSingle  : Convert<Byte?,SqlSingle>  { public override Byte? From(SqlSingle p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NullableByteFromSqlDouble  : Convert<Byte?,SqlDouble>  { public override Byte? From(SqlDouble p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NullableByteFromSqlDecimal : Convert<Byte?,SqlDecimal> { public override Byte? From(SqlDecimal p)  { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NullableByteFromSqlMoney   : Convert<Byte?,SqlMoney>   { public override Byte? From(SqlMoney p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NullableByteFromSqlBoolean : Convert<Byte?,SqlBoolean> { public override Byte? From(SqlBoolean p)  { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NullableByteDefault<Q>     : Convert<Byte?,Q>          { public override Byte? From(Q p)           { return Convert<Byte?,object>.Instance.From(p); } }
		sealed class NullableByteFromObject     : Convert<Byte?,object>     { public override Byte? From(object p)      { return Convert.ToByte(p); } }

		static Convert<T, P> GetNullableByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableByteFromByte());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableByteFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableByteFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableByteFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableByteFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableByteFromInt64());

			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableByteFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableByteFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableByteFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableByteFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableByteFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableByteFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableByteFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableByteFromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableByteFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableByteFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableByteFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableByteFromNullableInt64());

			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableByteFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableByteFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableByteFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableByteFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableByteFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableByteFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableByteFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableByteFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableByteFromSqlByte());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableByteFromSqlString());

			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableByteFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableByteFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableByteFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableByteFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableByteFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableByteFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableByteFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableByteFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableByteFromObject());

			return (Convert<T, P>)(object)(new NullableByteDefault<P>());
		}

		#endregion

		#region UInt16?


		// Scalar Types.
		//
		sealed class NullableUInt16FromUInt16     : Convert<UInt16?,UInt16>     { public override UInt16? From(UInt16 p)      { return p; } }
		sealed class NullableUInt16FromString     : Convert<UInt16?,String>     { public override UInt16? From(String p)      { return p == null? null: (UInt16?)Convert.ToUInt16(p); } }

		sealed class NullableUInt16FromSByte      : Convert<UInt16?,SByte>      { public override UInt16? From(SByte p)       { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromInt16      : Convert<UInt16?,Int16>      { public override UInt16? From(Int16 p)       { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromInt32      : Convert<UInt16?,Int32>      { public override UInt16? From(Int32 p)       { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromInt64      : Convert<UInt16?,Int64>      { public override UInt16? From(Int64 p)       { return Convert.ToUInt16(p); } }

		sealed class NullableUInt16FromByte       : Convert<UInt16?,Byte>       { public override UInt16? From(Byte p)        { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromUInt32     : Convert<UInt16?,UInt32>     { public override UInt16? From(UInt32 p)      { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromUInt64     : Convert<UInt16?,UInt64>     { public override UInt16? From(UInt64 p)      { return Convert.ToUInt16(p); } }

		sealed class NullableUInt16FromSingle     : Convert<UInt16?,Single>     { public override UInt16? From(Single p)      { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromDouble     : Convert<UInt16?,Double>     { public override UInt16? From(Double p)      { return Convert.ToUInt16(p); } }

		sealed class NullableUInt16FromBoolean    : Convert<UInt16?,Boolean>    { public override UInt16? From(Boolean p)     { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromDecimal    : Convert<UInt16?,Decimal>    { public override UInt16? From(Decimal p)     { return Convert.ToUInt16(p); } }
		sealed class NullableUInt16FromChar       : Convert<UInt16?,Char>       { public override UInt16? From(Char p)        { return Convert.ToUInt16(p); } }

		// Nullable Types.
		//
		sealed class NullableUInt16FromNullableSByte      : Convert<UInt16?,SByte?>      { public override UInt16? From(SByte? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableInt16      : Convert<UInt16?,Int16?>      { public override UInt16? From(Int16? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableInt32      : Convert<UInt16?,Int32?>      { public override UInt16? From(Int32? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableInt64      : Convert<UInt16?,Int64?>      { public override UInt16? From(Int64? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NullableUInt16FromNullableByte       : Convert<UInt16?,Byte?>       { public override UInt16? From(Byte? p)        { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableUInt32     : Convert<UInt16?,UInt32?>     { public override UInt16? From(UInt32? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableUInt64     : Convert<UInt16?,UInt64?>     { public override UInt16? From(UInt64? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NullableUInt16FromNullableSingle     : Convert<UInt16?,Single?>     { public override UInt16? From(Single? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableDouble     : Convert<UInt16?,Double?>     { public override UInt16? From(Double? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NullableUInt16FromNullableBoolean    : Convert<UInt16?,Boolean?>    { public override UInt16? From(Boolean? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableDecimal    : Convert<UInt16?,Decimal?>    { public override UInt16? From(Decimal? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NullableUInt16FromNullableChar       : Convert<UInt16?,Char?>       { public override UInt16? From(Char? p)        { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableUInt16FromSqlString  : Convert<UInt16?,SqlString>  { public override UInt16? From(SqlString p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NullableUInt16FromSqlByte    : Convert<UInt16?,SqlByte>    { public override UInt16? From(SqlByte p)     { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlInt16   : Convert<UInt16?,SqlInt16>   { public override UInt16? From(SqlInt16 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlInt32   : Convert<UInt16?,SqlInt32>   { public override UInt16? From(SqlInt32 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlInt64   : Convert<UInt16?,SqlInt64>   { public override UInt16? From(SqlInt64 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NullableUInt16FromSqlSingle  : Convert<UInt16?,SqlSingle>  { public override UInt16? From(SqlSingle p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlDouble  : Convert<UInt16?,SqlDouble>  { public override UInt16? From(SqlDouble p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlDecimal : Convert<UInt16?,SqlDecimal> { public override UInt16? From(SqlDecimal p)  { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NullableUInt16FromSqlMoney   : Convert<UInt16?,SqlMoney>   { public override UInt16? From(SqlMoney p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NullableUInt16FromSqlBoolean : Convert<UInt16?,SqlBoolean> { public override UInt16? From(SqlBoolean p)  { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NullableUInt16Default<Q>     : Convert<UInt16?,Q>          { public override UInt16? From(Q p)           { return Convert<UInt16?,object>.Instance.From(p); } }
		sealed class NullableUInt16FromObject     : Convert<UInt16?,object>     { public override UInt16? From(object p)      { return Convert.ToUInt16(p); } }

		static Convert<T, P> GetNullableUInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableUInt16FromUInt16());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableUInt16FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableUInt16FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableUInt16FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableUInt16FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableUInt16FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableUInt16FromByte());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableUInt16FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableUInt16FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableUInt16FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableUInt16FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableUInt16FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableUInt16FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableUInt16FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableUInt16FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableUInt16FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableUInt16FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableUInt16FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableUInt16FromNullableByte());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableUInt16FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableUInt16FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableUInt16FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableUInt16FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableUInt16FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableUInt16FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableUInt16FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableUInt16FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableUInt16FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableUInt16FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableUInt16FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableUInt16FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableUInt16FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableUInt16FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableUInt16FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableUInt16FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableUInt16FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableUInt16FromObject());

			return (Convert<T, P>)(object)(new NullableUInt16Default<P>());
		}

		#endregion

		#region UInt32?


		// Scalar Types.
		//
		sealed class NullableUInt32FromUInt32     : Convert<UInt32?,UInt32>     { public override UInt32? From(UInt32 p)      { return p; } }
		sealed class NullableUInt32FromString     : Convert<UInt32?,String>     { public override UInt32? From(String p)      { return p == null? null: (UInt32?)Convert.ToUInt32(p); } }

		sealed class NullableUInt32FromSByte      : Convert<UInt32?,SByte>      { public override UInt32? From(SByte p)       { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromInt16      : Convert<UInt32?,Int16>      { public override UInt32? From(Int16 p)       { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromInt32      : Convert<UInt32?,Int32>      { public override UInt32? From(Int32 p)       { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromInt64      : Convert<UInt32?,Int64>      { public override UInt32? From(Int64 p)       { return Convert.ToUInt32(p); } }

		sealed class NullableUInt32FromByte       : Convert<UInt32?,Byte>       { public override UInt32? From(Byte p)        { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromUInt16     : Convert<UInt32?,UInt16>     { public override UInt32? From(UInt16 p)      { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromUInt64     : Convert<UInt32?,UInt64>     { public override UInt32? From(UInt64 p)      { return Convert.ToUInt32(p); } }

		sealed class NullableUInt32FromSingle     : Convert<UInt32?,Single>     { public override UInt32? From(Single p)      { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromDouble     : Convert<UInt32?,Double>     { public override UInt32? From(Double p)      { return Convert.ToUInt32(p); } }

		sealed class NullableUInt32FromBoolean    : Convert<UInt32?,Boolean>    { public override UInt32? From(Boolean p)     { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromDecimal    : Convert<UInt32?,Decimal>    { public override UInt32? From(Decimal p)     { return Convert.ToUInt32(p); } }
		sealed class NullableUInt32FromChar       : Convert<UInt32?,Char>       { public override UInt32? From(Char p)        { return Convert.ToUInt32(p); } }

		// Nullable Types.
		//
		sealed class NullableUInt32FromNullableSByte      : Convert<UInt32?,SByte?>      { public override UInt32? From(SByte? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableInt16      : Convert<UInt32?,Int16?>      { public override UInt32? From(Int16? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableInt32      : Convert<UInt32?,Int32?>      { public override UInt32? From(Int32? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableInt64      : Convert<UInt32?,Int64?>      { public override UInt32? From(Int64? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NullableUInt32FromNullableByte       : Convert<UInt32?,Byte?>       { public override UInt32? From(Byte? p)        { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableUInt16     : Convert<UInt32?,UInt16?>     { public override UInt32? From(UInt16? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableUInt64     : Convert<UInt32?,UInt64?>     { public override UInt32? From(UInt64? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NullableUInt32FromNullableSingle     : Convert<UInt32?,Single?>     { public override UInt32? From(Single? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableDouble     : Convert<UInt32?,Double?>     { public override UInt32? From(Double? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NullableUInt32FromNullableBoolean    : Convert<UInt32?,Boolean?>    { public override UInt32? From(Boolean? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableDecimal    : Convert<UInt32?,Decimal?>    { public override UInt32? From(Decimal? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NullableUInt32FromNullableChar       : Convert<UInt32?,Char?>       { public override UInt32? From(Char? p)        { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableUInt32FromSqlString  : Convert<UInt32?,SqlString>  { public override UInt32? From(SqlString p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NullableUInt32FromSqlByte    : Convert<UInt32?,SqlByte>    { public override UInt32? From(SqlByte p)     { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlInt16   : Convert<UInt32?,SqlInt16>   { public override UInt32? From(SqlInt16 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlInt32   : Convert<UInt32?,SqlInt32>   { public override UInt32? From(SqlInt32 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlInt64   : Convert<UInt32?,SqlInt64>   { public override UInt32? From(SqlInt64 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NullableUInt32FromSqlSingle  : Convert<UInt32?,SqlSingle>  { public override UInt32? From(SqlSingle p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlDouble  : Convert<UInt32?,SqlDouble>  { public override UInt32? From(SqlDouble p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlDecimal : Convert<UInt32?,SqlDecimal> { public override UInt32? From(SqlDecimal p)  { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NullableUInt32FromSqlMoney   : Convert<UInt32?,SqlMoney>   { public override UInt32? From(SqlMoney p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NullableUInt32FromSqlBoolean : Convert<UInt32?,SqlBoolean> { public override UInt32? From(SqlBoolean p)  { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NullableUInt32Default<Q>     : Convert<UInt32?,Q>          { public override UInt32? From(Q p)           { return Convert<UInt32?,object>.Instance.From(p); } }
		sealed class NullableUInt32FromObject     : Convert<UInt32?,object>     { public override UInt32? From(object p)      { return Convert.ToUInt32(p); } }

		static Convert<T, P> GetNullableUInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableUInt32FromUInt32());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableUInt32FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableUInt32FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableUInt32FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableUInt32FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableUInt32FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableUInt32FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableUInt32FromUInt16());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableUInt32FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableUInt32FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableUInt32FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableUInt32FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableUInt32FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableUInt32FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableUInt32FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableUInt32FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableUInt32FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableUInt32FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableUInt32FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableUInt32FromNullableUInt16());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableUInt32FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableUInt32FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableUInt32FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableUInt32FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableUInt32FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableUInt32FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableUInt32FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableUInt32FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableUInt32FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableUInt32FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableUInt32FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableUInt32FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableUInt32FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableUInt32FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableUInt32FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableUInt32FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableUInt32FromObject());

			return (Convert<T, P>)(object)(new NullableUInt32Default<P>());
		}

		#endregion

		#region UInt64?


		// Scalar Types.
		//
		sealed class NullableUInt64FromUInt64     : Convert<UInt64?,UInt64>     { public override UInt64? From(UInt64 p)      { return p; } }
		sealed class NullableUInt64FromString     : Convert<UInt64?,String>     { public override UInt64? From(String p)      { return p == null? null: (UInt64?)Convert.ToUInt64(p); } }

		sealed class NullableUInt64FromSByte      : Convert<UInt64?,SByte>      { public override UInt64? From(SByte p)       { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromInt16      : Convert<UInt64?,Int16>      { public override UInt64? From(Int16 p)       { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromInt32      : Convert<UInt64?,Int32>      { public override UInt64? From(Int32 p)       { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromInt64      : Convert<UInt64?,Int64>      { public override UInt64? From(Int64 p)       { return Convert.ToUInt64(p); } }

		sealed class NullableUInt64FromByte       : Convert<UInt64?,Byte>       { public override UInt64? From(Byte p)        { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromUInt16     : Convert<UInt64?,UInt16>     { public override UInt64? From(UInt16 p)      { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromUInt32     : Convert<UInt64?,UInt32>     { public override UInt64? From(UInt32 p)      { return Convert.ToUInt64(p); } }

		sealed class NullableUInt64FromSingle     : Convert<UInt64?,Single>     { public override UInt64? From(Single p)      { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromDouble     : Convert<UInt64?,Double>     { public override UInt64? From(Double p)      { return Convert.ToUInt64(p); } }

		sealed class NullableUInt64FromBoolean    : Convert<UInt64?,Boolean>    { public override UInt64? From(Boolean p)     { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromDecimal    : Convert<UInt64?,Decimal>    { public override UInt64? From(Decimal p)     { return Convert.ToUInt64(p); } }
		sealed class NullableUInt64FromChar       : Convert<UInt64?,Char>       { public override UInt64? From(Char p)        { return Convert.ToUInt64(p); } }

		// Nullable Types.
		//
		sealed class NullableUInt64FromNullableSByte      : Convert<UInt64?,SByte?>      { public override UInt64? From(SByte? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableInt16      : Convert<UInt64?,Int16?>      { public override UInt64? From(Int16? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableInt32      : Convert<UInt64?,Int32?>      { public override UInt64? From(Int32? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableInt64      : Convert<UInt64?,Int64?>      { public override UInt64? From(Int64? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NullableUInt64FromNullableByte       : Convert<UInt64?,Byte?>       { public override UInt64? From(Byte? p)        { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableUInt16     : Convert<UInt64?,UInt16?>     { public override UInt64? From(UInt16? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableUInt32     : Convert<UInt64?,UInt32?>     { public override UInt64? From(UInt32? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NullableUInt64FromNullableSingle     : Convert<UInt64?,Single?>     { public override UInt64? From(Single? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableDouble     : Convert<UInt64?,Double?>     { public override UInt64? From(Double? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NullableUInt64FromNullableBoolean    : Convert<UInt64?,Boolean?>    { public override UInt64? From(Boolean? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableDecimal    : Convert<UInt64?,Decimal?>    { public override UInt64? From(Decimal? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NullableUInt64FromNullableChar       : Convert<UInt64?,Char?>       { public override UInt64? From(Char? p)        { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableUInt64FromSqlString  : Convert<UInt64?,SqlString>  { public override UInt64? From(SqlString p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NullableUInt64FromSqlByte    : Convert<UInt64?,SqlByte>    { public override UInt64? From(SqlByte p)     { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlInt16   : Convert<UInt64?,SqlInt16>   { public override UInt64? From(SqlInt16 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlInt32   : Convert<UInt64?,SqlInt32>   { public override UInt64? From(SqlInt32 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlInt64   : Convert<UInt64?,SqlInt64>   { public override UInt64? From(SqlInt64 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NullableUInt64FromSqlSingle  : Convert<UInt64?,SqlSingle>  { public override UInt64? From(SqlSingle p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlDouble  : Convert<UInt64?,SqlDouble>  { public override UInt64? From(SqlDouble p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlDecimal : Convert<UInt64?,SqlDecimal> { public override UInt64? From(SqlDecimal p)  { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NullableUInt64FromSqlMoney   : Convert<UInt64?,SqlMoney>   { public override UInt64? From(SqlMoney p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NullableUInt64FromSqlBoolean : Convert<UInt64?,SqlBoolean> { public override UInt64? From(SqlBoolean p)  { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NullableUInt64Default<Q>     : Convert<UInt64?,Q>          { public override UInt64? From(Q p)           { return Convert<UInt64?,object>.Instance.From(p); } }
		sealed class NullableUInt64FromObject     : Convert<UInt64?,object>     { public override UInt64? From(object p)      { return Convert.ToUInt64(p); } }

		static Convert<T, P> GetNullableUInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableUInt64FromUInt64());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableUInt64FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableUInt64FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableUInt64FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableUInt64FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableUInt64FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableUInt64FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableUInt64FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableUInt64FromUInt32());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableUInt64FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableUInt64FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableUInt64FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableUInt64FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableUInt64FromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableUInt64FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableUInt64FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableUInt64FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableUInt64FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableUInt64FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableUInt64FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableUInt64FromNullableUInt32());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableUInt64FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableUInt64FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableUInt64FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableUInt64FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableUInt64FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableUInt64FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableUInt64FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableUInt64FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableUInt64FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableUInt64FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableUInt64FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableUInt64FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableUInt64FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableUInt64FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableUInt64FromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableUInt64FromObject());

			return (Convert<T, P>)(object)(new NullableUInt64Default<P>());
		}

		#endregion

		#region Char?


		// Scalar Types.
		//
		sealed class NullableCharFromChar       : Convert<Char?,Char>       { public override Char? From(Char p)        { return p; } }
		sealed class NullableCharFromString     : Convert<Char?,String>     { public override Char? From(String p)      { return p == null? null: (Char?)Convert.ToChar(p); } }

		sealed class NullableCharFromSByte      : Convert<Char?,SByte>      { public override Char? From(SByte p)       { return Convert.ToChar(p); } }
		sealed class NullableCharFromInt16      : Convert<Char?,Int16>      { public override Char? From(Int16 p)       { return Convert.ToChar(p); } }
		sealed class NullableCharFromInt32      : Convert<Char?,Int32>      { public override Char? From(Int32 p)       { return Convert.ToChar(p); } }
		sealed class NullableCharFromInt64      : Convert<Char?,Int64>      { public override Char? From(Int64 p)       { return Convert.ToChar(p); } }

		sealed class NullableCharFromByte       : Convert<Char?,Byte>       { public override Char? From(Byte p)        { return Convert.ToChar(p); } }
		sealed class NullableCharFromUInt16     : Convert<Char?,UInt16>     { public override Char? From(UInt16 p)      { return Convert.ToChar(p); } }
		sealed class NullableCharFromUInt32     : Convert<Char?,UInt32>     { public override Char? From(UInt32 p)      { return Convert.ToChar(p); } }
		sealed class NullableCharFromUInt64     : Convert<Char?,UInt64>     { public override Char? From(UInt64 p)      { return Convert.ToChar(p); } }
		sealed class NullableCharFromBoolean    : Convert<Char?,Boolean>    { public override Char? From(Boolean p)     { return p? '1':'0'; } }

		// Nullable Types.
		//
		sealed class NullableCharFromNullableSByte      : Convert<Char?,SByte?>      { public override Char? From(SByte? p)       { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableInt16      : Convert<Char?,Int16?>      { public override Char? From(Int16? p)       { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableInt32      : Convert<Char?,Int32?>      { public override Char? From(Int32? p)       { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableInt64      : Convert<Char?,Int64?>      { public override Char? From(Int64? p)       { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }

		sealed class NullableCharFromNullableByte       : Convert<Char?,Byte?>       { public override Char? From(Byte? p)        { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableUInt16     : Convert<Char?,UInt16?>     { public override Char? From(UInt16? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableUInt32     : Convert<Char?,UInt32?>     { public override Char? From(UInt32? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableUInt64     : Convert<Char?,UInt64?>     { public override Char? From(UInt64? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NullableCharFromNullableBoolean    : Convert<Char?,Boolean?>    { public override Char? From(Boolean? p)     { return p.HasValue? (Char?)(p.Value? '1':'0')      : null; } }

		// SqlTypes.
		//
		sealed class NullableCharFromSqlByte    : Convert<Char?,SqlByte>    { public override Char? From(SqlByte p)     { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NullableCharFromSqlInt16   : Convert<Char?,SqlInt16>   { public override Char? From(SqlInt16 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NullableCharFromSqlInt32   : Convert<Char?,SqlInt32>   { public override Char? From(SqlInt32 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NullableCharFromSqlInt64   : Convert<Char?,SqlInt64>   { public override Char? From(SqlInt64 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NullableCharFromSqlBoolean : Convert<Char?,SqlBoolean> { public override Char? From(SqlBoolean p)  { return p.IsNull? null: (Char?)(p.Value? '1':'0'); } }

		sealed class NullableCharDefault<Q>     : Convert<Char?,Q>          { public override Char? From(Q p)           { return Convert<Char?,object>.Instance.From(p); } }
		sealed class NullableCharFromObject     : Convert<Char?,object>     { public override Char? From(object p)      { return Convert.ToChar(p); } }

		static Convert<T, P> GetNullableCharConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableCharFromChar());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableCharFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableCharFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableCharFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableCharFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableCharFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableCharFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableCharFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableCharFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableCharFromUInt64());
			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableCharFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableCharFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableCharFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableCharFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableCharFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableCharFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableCharFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableCharFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableCharFromNullableUInt64());
			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableCharFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableCharFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableCharFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableCharFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableCharFromSqlInt64());
			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableCharFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableCharFromObject());

			return (Convert<T, P>)(object)(new NullableCharDefault<P>());
		}

		#endregion

		#region Single?


		// Scalar Types.
		//
		sealed class NullableSingleFromSingle     : Convert<Single?,Single>     { public override Single? From(Single p)      { return p; } }
		sealed class NullableSingleFromString     : Convert<Single?,String>     { public override Single? From(String p)      { return p == null? null: (Single?)Convert.ToSingle(p); } }

		sealed class NullableSingleFromSByte      : Convert<Single?,SByte>      { public override Single? From(SByte p)       { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromInt16      : Convert<Single?,Int16>      { public override Single? From(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromInt32      : Convert<Single?,Int32>      { public override Single? From(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromInt64      : Convert<Single?,Int64>      { public override Single? From(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class NullableSingleFromByte       : Convert<Single?,Byte>       { public override Single? From(Byte p)        { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromUInt16     : Convert<Single?,UInt16>     { public override Single? From(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromUInt32     : Convert<Single?,UInt32>     { public override Single? From(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromUInt64     : Convert<Single?,UInt64>     { public override Single? From(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class NullableSingleFromDouble     : Convert<Single?,Double>     { public override Single? From(Double p)      { return Convert.ToSingle(p); } }

		sealed class NullableSingleFromBoolean    : Convert<Single?,Boolean>    { public override Single? From(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class NullableSingleFromDecimal    : Convert<Single?,Decimal>    { public override Single? From(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class NullableSingleFromNullableSByte      : Convert<Single?,SByte?>      { public override Single? From(SByte? p)       { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableInt16      : Convert<Single?,Int16?>      { public override Single? From(Int16? p)       { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableInt32      : Convert<Single?,Int32?>      { public override Single? From(Int32? p)       { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableInt64      : Convert<Single?,Int64?>      { public override Single? From(Int64? p)       { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NullableSingleFromNullableByte       : Convert<Single?,Byte?>       { public override Single? From(Byte? p)        { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableUInt16     : Convert<Single?,UInt16?>     { public override Single? From(UInt16? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableUInt32     : Convert<Single?,UInt32?>     { public override Single? From(UInt32? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableUInt64     : Convert<Single?,UInt64?>     { public override Single? From(UInt64? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NullableSingleFromNullableDouble     : Convert<Single?,Double?>     { public override Single? From(Double? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NullableSingleFromNullableBoolean    : Convert<Single?,Boolean?>    { public override Single? From(Boolean? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NullableSingleFromNullableDecimal    : Convert<Single?,Decimal?>    { public override Single? From(Decimal? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableSingleFromSqlSingle  : Convert<Single?,SqlSingle>  { public override Single? From(SqlSingle p)   { return p.IsNull? null: (Single?)                 p.Value;  } }
		sealed class NullableSingleFromSqlString  : Convert<Single?,SqlString>  { public override Single? From(SqlString p)   { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NullableSingleFromSqlByte    : Convert<Single?,SqlByte>    { public override Single? From(SqlByte p)     { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NullableSingleFromSqlInt16   : Convert<Single?,SqlInt16>   { public override Single? From(SqlInt16 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NullableSingleFromSqlInt32   : Convert<Single?,SqlInt32>   { public override Single? From(SqlInt32 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NullableSingleFromSqlInt64   : Convert<Single?,SqlInt64>   { public override Single? From(SqlInt64 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NullableSingleFromSqlDouble  : Convert<Single?,SqlDouble>  { public override Single? From(SqlDouble p)   { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NullableSingleFromSqlDecimal : Convert<Single?,SqlDecimal> { public override Single? From(SqlDecimal p)  { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NullableSingleFromSqlMoney   : Convert<Single?,SqlMoney>   { public override Single? From(SqlMoney p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NullableSingleFromSqlBoolean : Convert<Single?,SqlBoolean> { public override Single? From(SqlBoolean p)  { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NullableSingleDefault<Q>     : Convert<Single?,Q>          { public override Single? From(Q p)           { return Convert<Single?,object>.Instance.From(p); } }
		sealed class NullableSingleFromObject     : Convert<Single?,object>     { public override Single? From(object p)      { return Convert.ToSingle(p); } }

		static Convert<T, P> GetNullableSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableSingleFromSingle());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableSingleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableSingleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableSingleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableSingleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableSingleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableSingleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableSingleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableSingleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableSingleFromUInt64());

			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableSingleFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableSingleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableSingleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableSingleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableSingleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableSingleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableSingleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableSingleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableSingleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableSingleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableSingleFromNullableUInt64());

			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableSingleFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableSingleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableSingleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableSingleFromSqlSingle());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableSingleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableSingleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableSingleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableSingleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableSingleFromSqlInt64());

			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableSingleFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableSingleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableSingleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableSingleFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableSingleFromObject());

			return (Convert<T, P>)(object)(new NullableSingleDefault<P>());
		}

		#endregion

		#region Double?


		// Scalar Types.
		//
		sealed class NullableDoubleFromDouble     : Convert<Double?,Double>     { public override Double? From(Double p)      { return p; } }
		sealed class NullableDoubleFromString     : Convert<Double?,String>     { public override Double? From(String p)      { return p == null? null: (Double?)Convert.ToDouble(p); } }

		sealed class NullableDoubleFromSByte      : Convert<Double?,SByte>      { public override Double? From(SByte p)       { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromInt16      : Convert<Double?,Int16>      { public override Double? From(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromInt32      : Convert<Double?,Int32>      { public override Double? From(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromInt64      : Convert<Double?,Int64>      { public override Double? From(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class NullableDoubleFromByte       : Convert<Double?,Byte>       { public override Double? From(Byte p)        { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromUInt16     : Convert<Double?,UInt16>     { public override Double? From(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromUInt32     : Convert<Double?,UInt32>     { public override Double? From(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromUInt64     : Convert<Double?,UInt64>     { public override Double? From(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class NullableDoubleFromSingle     : Convert<Double?,Single>     { public override Double? From(Single p)      { return Convert.ToDouble(p); } }

		sealed class NullableDoubleFromBoolean    : Convert<Double?,Boolean>    { public override Double? From(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class NullableDoubleFromDecimal    : Convert<Double?,Decimal>    { public override Double? From(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class NullableDoubleFromNullableSByte      : Convert<Double?,SByte?>      { public override Double? From(SByte? p)       { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableInt16      : Convert<Double?,Int16?>      { public override Double? From(Int16? p)       { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableInt32      : Convert<Double?,Int32?>      { public override Double? From(Int32? p)       { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableInt64      : Convert<Double?,Int64?>      { public override Double? From(Int64? p)       { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NullableDoubleFromNullableByte       : Convert<Double?,Byte?>       { public override Double? From(Byte? p)        { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableUInt16     : Convert<Double?,UInt16?>     { public override Double? From(UInt16? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableUInt32     : Convert<Double?,UInt32?>     { public override Double? From(UInt32? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableUInt64     : Convert<Double?,UInt64?>     { public override Double? From(UInt64? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NullableDoubleFromNullableSingle     : Convert<Double?,Single?>     { public override Double? From(Single? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NullableDoubleFromNullableBoolean    : Convert<Double?,Boolean?>    { public override Double? From(Boolean? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NullableDoubleFromNullableDecimal    : Convert<Double?,Decimal?>    { public override Double? From(Decimal? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableDoubleFromSqlDouble  : Convert<Double?,SqlDouble>  { public override Double? From(SqlDouble p)   { return p.IsNull? null: (Double?)                 p.Value;  } }
		sealed class NullableDoubleFromSqlString  : Convert<Double?,SqlString>  { public override Double? From(SqlString p)   { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NullableDoubleFromSqlByte    : Convert<Double?,SqlByte>    { public override Double? From(SqlByte p)     { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NullableDoubleFromSqlInt16   : Convert<Double?,SqlInt16>   { public override Double? From(SqlInt16 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NullableDoubleFromSqlInt32   : Convert<Double?,SqlInt32>   { public override Double? From(SqlInt32 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NullableDoubleFromSqlInt64   : Convert<Double?,SqlInt64>   { public override Double? From(SqlInt64 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NullableDoubleFromSqlSingle  : Convert<Double?,SqlSingle>  { public override Double? From(SqlSingle p)   { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NullableDoubleFromSqlDecimal : Convert<Double?,SqlDecimal> { public override Double? From(SqlDecimal p)  { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NullableDoubleFromSqlMoney   : Convert<Double?,SqlMoney>   { public override Double? From(SqlMoney p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NullableDoubleFromSqlBoolean : Convert<Double?,SqlBoolean> { public override Double? From(SqlBoolean p)  { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NullableDoubleDefault<Q>     : Convert<Double?,Q>          { public override Double? From(Q p)           { return Convert<Double?,object>.Instance.From(p); } }
		sealed class NullableDoubleFromObject     : Convert<Double?,object>     { public override Double? From(object p)      { return Convert.ToDouble(p); } }

		static Convert<T, P> GetNullableDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableDoubleFromDouble());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableDoubleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableDoubleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableDoubleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableDoubleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableDoubleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableDoubleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableDoubleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableDoubleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableDoubleFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableDoubleFromSingle());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableDoubleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableDoubleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableDoubleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableDoubleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableDoubleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableDoubleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableDoubleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableDoubleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableDoubleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableDoubleFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableDoubleFromNullableSingle());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableDoubleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableDoubleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableDoubleFromSqlDouble());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableDoubleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableDoubleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableDoubleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableDoubleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableDoubleFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableDoubleFromSqlSingle());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableDoubleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableDoubleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableDoubleFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableDoubleFromObject());

			return (Convert<T, P>)(object)(new NullableDoubleDefault<P>());
		}

		#endregion

		#region Boolean?


		// Scalar Types.
		//
		sealed class NullableBooleanFromBoolean    : Convert<Boolean?,Boolean>    { public override Boolean? From(Boolean p)     { return p; } }
		sealed class NullableBooleanFromString     : Convert<Boolean?,String>     { public override Boolean? From(String p)      { return Convert.ToBoolean(p); } }

		sealed class NullableBooleanFromSByte      : Convert<Boolean?,SByte>      { public override Boolean? From(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromInt16      : Convert<Boolean?,Int16>      { public override Boolean? From(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromInt32      : Convert<Boolean?,Int32>      { public override Boolean? From(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromInt64      : Convert<Boolean?,Int64>      { public override Boolean? From(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class NullableBooleanFromByte       : Convert<Boolean?,Byte>       { public override Boolean? From(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromUInt16     : Convert<Boolean?,UInt16>     { public override Boolean? From(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromUInt32     : Convert<Boolean?,UInt32>     { public override Boolean? From(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromUInt64     : Convert<Boolean?,UInt64>     { public override Boolean? From(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class NullableBooleanFromSingle     : Convert<Boolean?,Single>     { public override Boolean? From(Single p)      { return Convert.ToBoolean(p); } }
		sealed class NullableBooleanFromDouble     : Convert<Boolean?,Double>     { public override Boolean? From(Double p)      { return Convert.ToBoolean(p); } }

		sealed class NullableBooleanFromDecimal    : Convert<Boolean?,Decimal>    { public override Boolean? From(Decimal p)     { return Convert.ToBoolean(p); } }

		sealed class NullableBooleanFromChar       : Convert<Boolean?,Char>       { public override Boolean? From(Char p)        { return Convert<Boolean,Char>.Instance.From(p); } }

		// Nullable Types.
		//
		sealed class NullableBooleanFromNullableSByte      : Convert<Boolean?,SByte?>      { public override Boolean? From(SByte? p)       { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableInt16      : Convert<Boolean?,Int16?>      { public override Boolean? From(Int16? p)       { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableInt32      : Convert<Boolean?,Int32?>      { public override Boolean? From(Int32? p)       { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableInt64      : Convert<Boolean?,Int64?>      { public override Boolean? From(Int64? p)       { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NullableBooleanFromNullableByte       : Convert<Boolean?,Byte?>       { public override Boolean? From(Byte? p)        { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableUInt16     : Convert<Boolean?,UInt16?>     { public override Boolean? From(UInt16? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableUInt32     : Convert<Boolean?,UInt32?>     { public override Boolean? From(UInt32? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableUInt64     : Convert<Boolean?,UInt64?>     { public override Boolean? From(UInt64? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NullableBooleanFromNullableSingle     : Convert<Boolean?,Single?>     { public override Boolean? From(Single? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NullableBooleanFromNullableDouble     : Convert<Boolean?,Double?>     { public override Boolean? From(Double? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NullableBooleanFromNullableDecimal    : Convert<Boolean?,Decimal?>    { public override Boolean? From(Decimal? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NullableBooleanFromNullableChar       : Convert<Boolean?,Char?>       { public override Boolean? From(Char? p)        { return p.HasValue? (Boolean?)Convert<Boolean,Char>.Instance.From(p.Value): null; } }

		// SqlTypes.
		//
		sealed class NullableBooleanFromSqlBoolean : Convert<Boolean?,SqlBoolean> { public override Boolean? From(SqlBoolean p)  { return p.IsNull? null: (Boolean?)                  p.Value;  } }
		sealed class NullableBooleanFromSqlString  : Convert<Boolean?,SqlString>  { public override Boolean? From(SqlString p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }

		sealed class NullableBooleanFromSqlByte    : Convert<Boolean?,SqlByte>    { public override Boolean? From(SqlByte p)     { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlInt16   : Convert<Boolean?,SqlInt16>   { public override Boolean? From(SqlInt16 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlInt32   : Convert<Boolean?,SqlInt32>   { public override Boolean? From(SqlInt32 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlInt64   : Convert<Boolean?,SqlInt64>   { public override Boolean? From(SqlInt64 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }

		sealed class NullableBooleanFromSqlSingle  : Convert<Boolean?,SqlSingle>  { public override Boolean? From(SqlSingle p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlDouble  : Convert<Boolean?,SqlDouble>  { public override Boolean? From(SqlDouble p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlDecimal : Convert<Boolean?,SqlDecimal> { public override Boolean? From(SqlDecimal p)  { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NullableBooleanFromSqlMoney   : Convert<Boolean?,SqlMoney>   { public override Boolean? From(SqlMoney p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }


		sealed class NullableBooleanDefault<Q>     : Convert<Boolean?,Q>          { public override Boolean? From(Q p)           { return Convert<Boolean?,object>.Instance.From(p); } }
		sealed class NullableBooleanFromObject     : Convert<Boolean?,object>     { public override Boolean? From(object p)      { return Convert.ToBoolean(p); } }

		static Convert<T, P> GetNullableBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableBooleanFromBoolean());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableBooleanFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableBooleanFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableBooleanFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableBooleanFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableBooleanFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableBooleanFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableBooleanFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableBooleanFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableBooleanFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableBooleanFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableBooleanFromDouble());

			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableBooleanFromDecimal());

			if (t == typeof(Char))        return (Convert<T, P>)(object)(new NullableBooleanFromChar());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableBooleanFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableBooleanFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableBooleanFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableBooleanFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableBooleanFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableBooleanFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableBooleanFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableBooleanFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableBooleanFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableBooleanFromNullableDouble());

			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableBooleanFromNullableDecimal());

			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new NullableBooleanFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableBooleanFromSqlBoolean());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableBooleanFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableBooleanFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableBooleanFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableBooleanFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableBooleanFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableBooleanFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableBooleanFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableBooleanFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableBooleanFromSqlMoney());


			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableBooleanFromObject());

			return (Convert<T, P>)(object)(new NullableBooleanDefault<P>());
		}

		#endregion

		#region Decimal?


		// Scalar Types.
		//
		sealed class NullableDecimalFromDecimal    : Convert<Decimal?,Decimal>    { public override Decimal? From(Decimal p)     { return p; } }
		sealed class NullableDecimalFromString     : Convert<Decimal?,String>     { public override Decimal? From(String p)      { return p == null? null: (Decimal?)Convert.ToDecimal(p); } }

		sealed class NullableDecimalFromSByte      : Convert<Decimal?,SByte>      { public override Decimal? From(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromInt16      : Convert<Decimal?,Int16>      { public override Decimal? From(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromInt32      : Convert<Decimal?,Int32>      { public override Decimal? From(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromInt64      : Convert<Decimal?,Int64>      { public override Decimal? From(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class NullableDecimalFromByte       : Convert<Decimal?,Byte>       { public override Decimal? From(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromUInt16     : Convert<Decimal?,UInt16>     { public override Decimal? From(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromUInt32     : Convert<Decimal?,UInt32>     { public override Decimal? From(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromUInt64     : Convert<Decimal?,UInt64>     { public override Decimal? From(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class NullableDecimalFromSingle     : Convert<Decimal?,Single>     { public override Decimal? From(Single p)      { return Convert.ToDecimal(p); } }
		sealed class NullableDecimalFromDouble     : Convert<Decimal?,Double>     { public override Decimal? From(Double p)      { return Convert.ToDecimal(p); } }

		sealed class NullableDecimalFromBoolean    : Convert<Decimal?,Boolean>    { public override Decimal? From(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class NullableDecimalFromNullableSByte      : Convert<Decimal?,SByte?>      { public override Decimal? From(SByte? p)       { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableInt16      : Convert<Decimal?,Int16?>      { public override Decimal? From(Int16? p)       { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableInt32      : Convert<Decimal?,Int32?>      { public override Decimal? From(Int32? p)       { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableInt64      : Convert<Decimal?,Int64?>      { public override Decimal? From(Int64? p)       { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class NullableDecimalFromNullableByte       : Convert<Decimal?,Byte?>       { public override Decimal? From(Byte? p)        { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableUInt16     : Convert<Decimal?,UInt16?>     { public override Decimal? From(UInt16? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableUInt32     : Convert<Decimal?,UInt32?>     { public override Decimal? From(UInt32? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableUInt64     : Convert<Decimal?,UInt64?>     { public override Decimal? From(UInt64? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class NullableDecimalFromNullableSingle     : Convert<Decimal?,Single?>     { public override Decimal? From(Single? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class NullableDecimalFromNullableDouble     : Convert<Decimal?,Double?>     { public override Decimal? From(Double? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class NullableDecimalFromNullableBoolean    : Convert<Decimal?,Boolean?>    { public override Decimal? From(Boolean? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableDecimalFromSqlDecimal : Convert<Decimal?,SqlDecimal> { public override Decimal? From(SqlDecimal p)  { return p.IsNull? null: (Decimal?)                  p.Value;  } }
		sealed class NullableDecimalFromSqlMoney   : Convert<Decimal?,SqlMoney>   { public override Decimal? From(SqlMoney p)    { return p.IsNull? null: (Decimal?)                  p.Value;  } }
		sealed class NullableDecimalFromSqlString  : Convert<Decimal?,SqlString>  { public override Decimal? From(SqlString p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class NullableDecimalFromSqlByte    : Convert<Decimal?,SqlByte>    { public override Decimal? From(SqlByte p)     { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class NullableDecimalFromSqlInt16   : Convert<Decimal?,SqlInt16>   { public override Decimal? From(SqlInt16 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class NullableDecimalFromSqlInt32   : Convert<Decimal?,SqlInt32>   { public override Decimal? From(SqlInt32 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class NullableDecimalFromSqlInt64   : Convert<Decimal?,SqlInt64>   { public override Decimal? From(SqlInt64 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class NullableDecimalFromSqlSingle  : Convert<Decimal?,SqlSingle>  { public override Decimal? From(SqlSingle p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class NullableDecimalFromSqlDouble  : Convert<Decimal?,SqlDouble>  { public override Decimal? From(SqlDouble p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class NullableDecimalFromSqlBoolean : Convert<Decimal?,SqlBoolean> { public override Decimal? From(SqlBoolean p)  { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class NullableDecimalDefault<Q>     : Convert<Decimal?,Q>          { public override Decimal? From(Q p)           { return Convert<Decimal?,object>.Instance.From(p); } }
		sealed class NullableDecimalFromObject     : Convert<Decimal?,object>     { public override Decimal? From(object p)      { return Convert.ToDecimal(p); } }

		static Convert<T, P> GetNullableDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableDecimalFromDecimal());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableDecimalFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableDecimalFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableDecimalFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableDecimalFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableDecimalFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableDecimalFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableDecimalFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableDecimalFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableDecimalFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableDecimalFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableDecimalFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableDecimalFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableDecimalFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableDecimalFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableDecimalFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableDecimalFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableDecimalFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableDecimalFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableDecimalFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableDecimalFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableDecimalFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableDecimalFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableDecimalFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableDecimalFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableDecimalFromSqlMoney());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableDecimalFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableDecimalFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableDecimalFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableDecimalFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableDecimalFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableDecimalFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableDecimalFromSqlDouble());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableDecimalFromSqlBoolean());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableDecimalFromObject());

			return (Convert<T, P>)(object)(new NullableDecimalDefault<P>());
		}

		#endregion

		#region DateTime?


		// Scalar Types.
		//
		sealed class NullableDateTimeFromDateTime   : Convert<DateTime?,DateTime>   { public override DateTime? From(DateTime p)    { return p; } }
		sealed class NullableDateTimeFromString     : Convert<DateTime?,String>     { public override DateTime? From(String p)      { return p == null? null: (DateTime?)Convert.ToDateTime(p); } }

		sealed class NullableDateTimeFromSByte      : Convert<DateTime?,SByte>      { public override DateTime? From(SByte p)       { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromInt16      : Convert<DateTime?,Int16>      { public override DateTime? From(Int16 p)       { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromInt32      : Convert<DateTime?,Int32>      { public override DateTime? From(Int32 p)       { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromInt64      : Convert<DateTime?,Int64>      { public override DateTime? From(Int64 p)       { return Convert.ToDateTime(p); } }

		sealed class NullableDateTimeFromByte       : Convert<DateTime?,Byte>       { public override DateTime? From(Byte p)        { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromUInt16     : Convert<DateTime?,UInt16>     { public override DateTime? From(UInt16 p)      { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromUInt32     : Convert<DateTime?,UInt32>     { public override DateTime? From(UInt32 p)      { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromUInt64     : Convert<DateTime?,UInt64>     { public override DateTime? From(UInt64 p)      { return Convert.ToDateTime(p); } }

		sealed class NullableDateTimeFromSingle     : Convert<DateTime?,Single>     { public override DateTime? From(Single p)      { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromDouble     : Convert<DateTime?,Double>     { public override DateTime? From(Double p)      { return Convert.ToDateTime(p); } }

		sealed class NullableDateTimeFromBoolean    : Convert<DateTime?,Boolean>    { public override DateTime? From(Boolean p)     { return Convert.ToDateTime(p); } }
		sealed class NullableDateTimeFromDecimal    : Convert<DateTime?,Decimal>    { public override DateTime? From(Decimal p)     { return Convert.ToDateTime(p); } }

		// Nullable Types.
		//
		sealed class NullableDateTimeFromNullableSByte      : Convert<DateTime?,SByte?>      { public override DateTime? From(SByte? p)       { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableInt16      : Convert<DateTime?,Int16?>      { public override DateTime? From(Int16? p)       { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableInt32      : Convert<DateTime?,Int32?>      { public override DateTime? From(Int32? p)       { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableInt64      : Convert<DateTime?,Int64?>      { public override DateTime? From(Int64? p)       { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NullableDateTimeFromNullableByte       : Convert<DateTime?,Byte?>       { public override DateTime? From(Byte? p)        { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableUInt16     : Convert<DateTime?,UInt16?>     { public override DateTime? From(UInt16? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableUInt32     : Convert<DateTime?,UInt32?>     { public override DateTime? From(UInt32? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableUInt64     : Convert<DateTime?,UInt64?>     { public override DateTime? From(UInt64? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NullableDateTimeFromNullableSingle     : Convert<DateTime?,Single?>     { public override DateTime? From(Single? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableDouble     : Convert<DateTime?,Double?>     { public override DateTime? From(Double? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NullableDateTimeFromNullableBoolean    : Convert<DateTime?,Boolean?>    { public override DateTime? From(Boolean? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NullableDateTimeFromNullableDecimal    : Convert<DateTime?,Decimal?>    { public override DateTime? From(Decimal? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NullableDateTimeFromSqlString  : Convert<DateTime?,SqlString>  { public override DateTime? From(SqlString p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NullableDateTimeFromSqlByte    : Convert<DateTime?,SqlByte>    { public override DateTime? From(SqlByte p)     { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlInt16   : Convert<DateTime?,SqlInt16>   { public override DateTime? From(SqlInt16 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlInt32   : Convert<DateTime?,SqlInt32>   { public override DateTime? From(SqlInt32 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlInt64   : Convert<DateTime?,SqlInt64>   { public override DateTime? From(SqlInt64 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NullableDateTimeFromSqlSingle  : Convert<DateTime?,SqlSingle>  { public override DateTime? From(SqlSingle p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlDouble  : Convert<DateTime?,SqlDouble>  { public override DateTime? From(SqlDouble p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlDecimal : Convert<DateTime?,SqlDecimal> { public override DateTime? From(SqlDecimal p)  { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlMoney   : Convert<DateTime?,SqlMoney>   { public override DateTime? From(SqlMoney p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NullableDateTimeFromSqlBoolean : Convert<DateTime?,SqlBoolean> { public override DateTime? From(SqlBoolean p)  { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NullableDateTimeFromSqlDateTime: Convert<DateTime?,SqlDateTime>{ public override DateTime? From(SqlDateTime p) { return p.IsNull? null: (DateTime?)                   p.Value;  } }

		sealed class NullableDateTimeDefault<Q>     : Convert<DateTime?,Q>          { public override DateTime? From(Q p)           { return Convert<DateTime?,object>.Instance.From(p); } }
		sealed class NullableDateTimeFromObject     : Convert<DateTime?,object>     { public override DateTime? From(object p)      { return Convert.ToDateTime(p); } }

		static Convert<T, P> GetNullableDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new NullableDateTimeFromDateTime());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableDateTimeFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new NullableDateTimeFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new NullableDateTimeFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new NullableDateTimeFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new NullableDateTimeFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new NullableDateTimeFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new NullableDateTimeFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new NullableDateTimeFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new NullableDateTimeFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new NullableDateTimeFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new NullableDateTimeFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new NullableDateTimeFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new NullableDateTimeFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new NullableDateTimeFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new NullableDateTimeFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new NullableDateTimeFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new NullableDateTimeFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new NullableDateTimeFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new NullableDateTimeFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new NullableDateTimeFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new NullableDateTimeFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new NullableDateTimeFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new NullableDateTimeFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new NullableDateTimeFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new NullableDateTimeFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableDateTimeFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new NullableDateTimeFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new NullableDateTimeFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new NullableDateTimeFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new NullableDateTimeFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new NullableDateTimeFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new NullableDateTimeFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new NullableDateTimeFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new NullableDateTimeFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new NullableDateTimeFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new NullableDateTimeFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableDateTimeFromObject());

			return (Convert<T, P>)(object)(new NullableDateTimeDefault<P>());
		}

		#endregion

		#region TimeSpan?


		// Scalar Types.
		//
		sealed class NullableTimeSpanFromTimeSpan   : Convert<TimeSpan?,TimeSpan>   { public override TimeSpan? From(TimeSpan p)    { return p; } }
		sealed class NullableTimeSpanFromString     : Convert<TimeSpan?,String>     { public override TimeSpan? From(String p)      { return p == null? null: (TimeSpan?)TimeSpan.Parse(p); } }
		sealed class NullableTimeSpanFromDateTime   : Convert<TimeSpan?,DateTime>   { public override TimeSpan? From(DateTime p)    { return p - DateTime.MinValue; } }

		// Nullable Types.
		//
		sealed class NullableTimeSpanFromNullableDateTime   : Convert<TimeSpan?,DateTime?>   { public override TimeSpan? From(DateTime? p)    { return p.HasValue? (TimeSpan?)(p.Value - DateTime.MinValue) : null; } }

		// SqlTypes.
		//
		sealed class NullableTimeSpanFromSqlString  : Convert<TimeSpan?,SqlString>  { public override TimeSpan? From(SqlString p)   { return p.IsNull? null: (TimeSpan?)TimeSpan.Parse(p.Value);       } }
		sealed class NullableTimeSpanFromSqlDateTime: Convert<TimeSpan?,SqlDateTime>{ public override TimeSpan? From(SqlDateTime p) { return p.IsNull? null: (TimeSpan?)(p.Value - DateTime.MinValue); } }

		sealed class NullableTimeSpanDefault<Q>     : Convert<TimeSpan?,Q>          { public override TimeSpan? From(Q p)           { return Convert<TimeSpan?,object>.Instance.From(p); } }
		sealed class NullableTimeSpanFromObject     : Convert<TimeSpan?,object>     { public override TimeSpan? From(object p)     
			{
				if (p == null)
					return null;

				// Scalar Types.
				//
				if (p is TimeSpan)    return Convert<TimeSpan,TimeSpan>   .Instance.From((TimeSpan)p);
				if (p is String)      return Convert<TimeSpan,String>     .Instance.From((String)p);
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .Instance.From((DateTime)p);

				// Nullable Types.
				//
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .Instance.From((DateTime)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<TimeSpan,SqlString>  .Instance.From((SqlString)p);
				if (p is SqlDateTime) return Convert<TimeSpan,SqlDateTime>.Instance.From((SqlDateTime)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetNullableTimeSpanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(TimeSpan))    return (Convert<T, P>)(object)(new NullableTimeSpanFromTimeSpan());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableTimeSpanFromString());
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new NullableTimeSpanFromDateTime());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new NullableTimeSpanFromNullableDateTime());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableTimeSpanFromSqlString());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new NullableTimeSpanFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableTimeSpanFromObject());

			return (Convert<T, P>)(object)(new NullableTimeSpanDefault<P>());
		}

		#endregion

		#region Guid?


		// Scalar Types.
		//
		sealed class NullableGuidFromGuid       : Convert<Guid?,Guid>       { public override Guid? From(Guid p)        { return p; } }
		sealed class NullableGuidFromString     : Convert<Guid?,String>     { public override Guid? From(String p)      { return p == null? null: (Guid?)new Guid(p); } }

		// Nullable Types.
		//

		// SqlTypes.
		//
		sealed class NullableGuidFromSqlGuid    : Convert<Guid?,SqlGuid>    { public override Guid? From(SqlGuid p)     { return p.IsNull? null: (Guid?)p.Value;             } }
		sealed class NullableGuidFromSqlString  : Convert<Guid?,SqlString>  { public override Guid? From(SqlString p)   { return p.IsNull? null: (Guid?)new Guid(p.Value);   } }
		sealed class NullableGuidFromSqlBinary  : Convert<Guid?,SqlBinary>  { public override Guid? From(SqlBinary p)   { return p.IsNull? null: (Guid?)p.ToSqlGuid().Value; } }
		sealed class NullableGuidFromType       : Convert<Guid?,Type>       { public override Guid? From(Type p)        { return p == null? null: p.GUID; } }

		sealed class NullableGuidDefault<Q>     : Convert<Guid?,Q>          { public override Guid? From(Q p)           { return Convert<Guid?,object>.Instance.From(p); } }
		sealed class NullableGuidFromObject     : Convert<Guid?,object>     { public override Guid? From(object p)     
			{
				if (p == null)
					return null;

				// Scalar Types.
				//
				if (p is Guid)        return Convert<Guid,Guid>       .Instance.From((Guid)p);
				if (p is String)      return Convert<Guid,String>     .Instance.From((String)p);

				// Nullable Types.
				//

				// SqlTypes.
				//
				if (p is SqlGuid)     return Convert<Guid,SqlGuid>    .Instance.From((SqlGuid)p);
				if (p is SqlString)   return Convert<Guid,SqlString>  .Instance.From((SqlString)p);
				if (p is SqlBinary)   return Convert<Guid,SqlBinary>  .Instance.From((SqlBinary)p);
				if (p is Type)        return Convert<Guid,Type>       .Instance.From((Type)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetNullableGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new NullableGuidFromGuid());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new NullableGuidFromString());

			// Nullable Types.
			//

			// SqlTypes.
			//
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new NullableGuidFromSqlGuid());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new NullableGuidFromSqlString());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new NullableGuidFromSqlBinary());
			if (t == typeof(Type))        return (Convert<T, P>)(object)(new NullableGuidFromType());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new NullableGuidFromObject());

			return (Convert<T, P>)(object)(new NullableGuidDefault<P>());
		}

		#endregion

		#endregion

		#region SqlTypes

		#region SqlString


		// Scalar Types.
		//
		sealed class SqlStringFromString     : Convert<SqlString,String>     { public override SqlString From(String p)      { return p == null? SqlString.Null: p; } }

		sealed class SqlStringFromSByte      : Convert<SqlString,SByte>      { public override SqlString From(SByte p)       { return p.ToString(); } }
		sealed class SqlStringFromInt16      : Convert<SqlString,Int16>      { public override SqlString From(Int16 p)       { return p.ToString(); } }
		sealed class SqlStringFromInt32      : Convert<SqlString,Int32>      { public override SqlString From(Int32 p)       { return p.ToString(); } }
		sealed class SqlStringFromInt64      : Convert<SqlString,Int64>      { public override SqlString From(Int64 p)       { return p.ToString(); } }

		sealed class SqlStringFromByte       : Convert<SqlString,Byte>       { public override SqlString From(Byte p)        { return p.ToString(); } }
		sealed class SqlStringFromUInt16     : Convert<SqlString,UInt16>     { public override SqlString From(UInt16 p)      { return p.ToString(); } }
		sealed class SqlStringFromUInt32     : Convert<SqlString,UInt32>     { public override SqlString From(UInt32 p)      { return p.ToString(); } }
		sealed class SqlStringFromUInt64     : Convert<SqlString,UInt64>     { public override SqlString From(UInt64 p)      { return p.ToString(); } }

		sealed class SqlStringFromSingle     : Convert<SqlString,Single>     { public override SqlString From(Single p)      { return p.ToString(); } }
		sealed class SqlStringFromDouble     : Convert<SqlString,Double>     { public override SqlString From(Double p)      { return p.ToString(); } }

		sealed class SqlStringFromBoolean    : Convert<SqlString,Boolean>    { public override SqlString From(Boolean p)     { return p.ToString(); } }
		sealed class SqlStringFromDecimal    : Convert<SqlString,Decimal>    { public override SqlString From(Decimal p)     { return p.ToString(); } }
		sealed class SqlStringFromChar       : Convert<SqlString,Char>       { public override SqlString From(Char p)        { return p.ToString(); } }
		sealed class SqlStringFromTimeSpan   : Convert<SqlString,TimeSpan>   { public override SqlString From(TimeSpan p)    { return p.ToString(); } }
		sealed class SqlStringFromDateTime   : Convert<SqlString,DateTime>   { public override SqlString From(DateTime p)    { return p.ToString(); } }
		sealed class SqlStringFromGuid       : Convert<SqlString,Guid>       { public override SqlString From(Guid p)        { return p.ToString(); } }
		sealed class SqlStringFromCharArray  : Convert<SqlString,Char[]>     { public override SqlString From(Char[] p)      { return new String(p); } }

		// Nullable Types.
		//
		sealed class SqlStringFromNullableSByte      : Convert<SqlString,SByte?>      { public override SqlString From(SByte? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableInt16      : Convert<SqlString,Int16?>      { public override SqlString From(Int16? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableInt32      : Convert<SqlString,Int32?>      { public override SqlString From(Int32? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableInt64      : Convert<SqlString,Int64?>      { public override SqlString From(Int64? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class SqlStringFromNullableByte       : Convert<SqlString,Byte?>       { public override SqlString From(Byte? p)        { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableUInt16     : Convert<SqlString,UInt16?>     { public override SqlString From(UInt16? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableUInt32     : Convert<SqlString,UInt32?>     { public override SqlString From(UInt32? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableUInt64     : Convert<SqlString,UInt64?>     { public override SqlString From(UInt64? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class SqlStringFromNullableSingle     : Convert<SqlString,Single?>     { public override SqlString From(Single? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableDouble     : Convert<SqlString,Double?>     { public override SqlString From(Double? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class SqlStringFromNullableBoolean    : Convert<SqlString,Boolean?>    { public override SqlString From(Boolean? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableDecimal    : Convert<SqlString,Decimal?>    { public override SqlString From(Decimal? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableChar       : Convert<SqlString,Char?>       { public override SqlString From(Char? p)        { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableTimeSpan   : Convert<SqlString,TimeSpan?>   { public override SqlString From(TimeSpan? p)    { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableDateTime   : Convert<SqlString,DateTime?>   { public override SqlString From(DateTime? p)    { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class SqlStringFromNullableGuid       : Convert<SqlString,Guid?>       { public override SqlString From(Guid? p)        { return p.HasValue? p.ToString(): SqlString.Null; } }

		// SqlTypes.
		//

		sealed class SqlStringFromSqlByte    : Convert<SqlString,SqlByte>    { public override SqlString From(SqlByte p)     { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlInt16   : Convert<SqlString,SqlInt16>   { public override SqlString From(SqlInt16 p)    { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlInt32   : Convert<SqlString,SqlInt32>   { public override SqlString From(SqlInt32 p)    { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlInt64   : Convert<SqlString,SqlInt64>   { public override SqlString From(SqlInt64 p)    { return p.ToSqlString(); } }

		sealed class SqlStringFromSqlSingle  : Convert<SqlString,SqlSingle>  { public override SqlString From(SqlSingle p)   { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlDouble  : Convert<SqlString,SqlDouble>  { public override SqlString From(SqlDouble p)   { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlDecimal : Convert<SqlString,SqlDecimal> { public override SqlString From(SqlDecimal p)  { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlMoney   : Convert<SqlString,SqlMoney>   { public override SqlString From(SqlMoney p)    { return p.ToSqlString(); } }

		sealed class SqlStringFromSqlBoolean : Convert<SqlString,SqlBoolean> { public override SqlString From(SqlBoolean p)  { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlChars   : Convert<SqlString,SqlChars>   { public override SqlString From(SqlChars p)    { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlGuid    : Convert<SqlString,SqlGuid>    { public override SqlString From(SqlGuid p)     { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlDateTime: Convert<SqlString,SqlDateTime>{ public override SqlString From(SqlDateTime p) { return p.ToSqlString(); } }
		sealed class SqlStringFromSqlBinary  : Convert<SqlString,SqlBinary>  { public override SqlString From(SqlBinary p)   { return p.IsNull? SqlString.Null: p.ToString(); } }
		sealed class SqlStringFromType       : Convert<SqlString,Type>       { public override SqlString From(Type p)        { return p == null? SqlString.Null: p.FullName; } }

		sealed class SqlStringDefault<Q>     : Convert<SqlString,Q>          { public override SqlString From(Q p)           { return Convert<SqlString,object>.Instance.From(p); } }
		sealed class SqlStringFromObject     : Convert<SqlString,object>     { public override SqlString From(object p)      { return Convert.ToString(p); } }

		static Convert<T, P> GetSqlStringConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlStringFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlStringFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlStringFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlStringFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlStringFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlStringFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlStringFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlStringFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlStringFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlStringFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlStringFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlStringFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlStringFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlStringFromChar());
			if (t == typeof(TimeSpan))    return (Convert<T, P>)(object)(new SqlStringFromTimeSpan());
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new SqlStringFromDateTime());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new SqlStringFromGuid());
			if (t == typeof(Char[]))      return (Convert<T, P>)(object)(new SqlStringFromCharArray());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlStringFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlStringFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlStringFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlStringFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlStringFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlStringFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlStringFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlStringFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlStringFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlStringFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlStringFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlStringFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlStringFromNullableChar());
			if (t == typeof(TimeSpan?))    return (Convert<T, P>)(object)(new SqlStringFromNullableTimeSpan());
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new SqlStringFromNullableDateTime());
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new SqlStringFromNullableGuid());

			// SqlTypes.
			//

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlStringFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlStringFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlStringFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlStringFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlStringFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlStringFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlStringFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlStringFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlStringFromSqlBoolean());
			if (t == typeof(SqlChars))    return (Convert<T, P>)(object)(new SqlStringFromSqlChars());
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new SqlStringFromSqlGuid());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlStringFromSqlDateTime());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new SqlStringFromSqlBinary());
			if (t == typeof(Type))        return (Convert<T, P>)(object)(new SqlStringFromType());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlStringFromObject());

			return (Convert<T, P>)(object)(new SqlStringDefault<P>());
		}

		#endregion

		#region SqlByte


		// Scalar Types.
		//
		sealed class SqlByteFromByte       : Convert<SqlByte,Byte>       { public override SqlByte From(Byte p)        { return p; } }
		sealed class SqlByteFromString     : Convert<SqlByte,String>     { public override SqlByte From(String p)      { return p == null? SqlByte.Null: SqlByte.Parse(p); } }

		sealed class SqlByteFromSByte      : Convert<SqlByte,SByte>      { public override SqlByte From(SByte p)       { return Convert.ToByte(p); } }
		sealed class SqlByteFromInt16      : Convert<SqlByte,Int16>      { public override SqlByte From(Int16 p)       { return Convert.ToByte(p); } }
		sealed class SqlByteFromInt32      : Convert<SqlByte,Int32>      { public override SqlByte From(Int32 p)       { return Convert.ToByte(p); } }
		sealed class SqlByteFromInt64      : Convert<SqlByte,Int64>      { public override SqlByte From(Int64 p)       { return Convert.ToByte(p); } }

		sealed class SqlByteFromUInt16     : Convert<SqlByte,UInt16>     { public override SqlByte From(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class SqlByteFromUInt32     : Convert<SqlByte,UInt32>     { public override SqlByte From(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class SqlByteFromUInt64     : Convert<SqlByte,UInt64>     { public override SqlByte From(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class SqlByteFromSingle     : Convert<SqlByte,Single>     { public override SqlByte From(Single p)      { return Convert.ToByte(p); } }
		sealed class SqlByteFromDouble     : Convert<SqlByte,Double>     { public override SqlByte From(Double p)      { return Convert.ToByte(p); } }

		sealed class SqlByteFromBoolean    : Convert<SqlByte,Boolean>    { public override SqlByte From(Boolean p)     { return Convert.ToByte(p); } }
		sealed class SqlByteFromDecimal    : Convert<SqlByte,Decimal>    { public override SqlByte From(Decimal p)     { return Convert.ToByte(p); } }
		sealed class SqlByteFromChar       : Convert<SqlByte,Char>       { public override SqlByte From(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class SqlByteFromNullableByte       : Convert<SqlByte,Byte?>       { public override SqlByte From(Byte? p)        { return p.HasValue?                p.Value  : SqlByte.Null; } }
		sealed class SqlByteFromNullableSByte      : Convert<SqlByte,SByte?>      { public override SqlByte From(SByte? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableInt16      : Convert<SqlByte,Int16?>      { public override SqlByte From(Int16? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableInt32      : Convert<SqlByte,Int32?>      { public override SqlByte From(Int32? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableInt64      : Convert<SqlByte,Int64?>      { public override SqlByte From(Int64? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class SqlByteFromNullableUInt16     : Convert<SqlByte,UInt16?>     { public override SqlByte From(UInt16? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableUInt32     : Convert<SqlByte,UInt32?>     { public override SqlByte From(UInt32? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableUInt64     : Convert<SqlByte,UInt64?>     { public override SqlByte From(UInt64? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class SqlByteFromNullableSingle     : Convert<SqlByte,Single?>     { public override SqlByte From(Single? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableDouble     : Convert<SqlByte,Double?>     { public override SqlByte From(Double? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class SqlByteFromNullableBoolean    : Convert<SqlByte,Boolean?>    { public override SqlByte From(Boolean? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableDecimal    : Convert<SqlByte,Decimal?>    { public override SqlByte From(Decimal? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class SqlByteFromNullableChar       : Convert<SqlByte,Char?>       { public override SqlByte From(Char? p)        { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		// SqlTypes.
		//
		sealed class SqlByteFromSqlString  : Convert<SqlByte,SqlString>  { public override SqlByte From(SqlString p)   { return p.ToSqlByte(); } }

		sealed class SqlByteFromSqlInt16   : Convert<SqlByte,SqlInt16>   { public override SqlByte From(SqlInt16 p)    { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlInt32   : Convert<SqlByte,SqlInt32>   { public override SqlByte From(SqlInt32 p)    { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlInt64   : Convert<SqlByte,SqlInt64>   { public override SqlByte From(SqlInt64 p)    { return p.ToSqlByte(); } }

		sealed class SqlByteFromSqlSingle  : Convert<SqlByte,SqlSingle>  { public override SqlByte From(SqlSingle p)   { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlDouble  : Convert<SqlByte,SqlDouble>  { public override SqlByte From(SqlDouble p)   { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlDecimal : Convert<SqlByte,SqlDecimal> { public override SqlByte From(SqlDecimal p)  { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlMoney   : Convert<SqlByte,SqlMoney>   { public override SqlByte From(SqlMoney p)    { return p.ToSqlByte(); } }

		sealed class SqlByteFromSqlBoolean : Convert<SqlByte,SqlBoolean> { public override SqlByte From(SqlBoolean p)  { return p.ToSqlByte(); } }
		sealed class SqlByteFromSqlDateTime: Convert<SqlByte,SqlDateTime>{ public override SqlByte From(SqlDateTime p) { return p.IsNull? SqlByte.Null: Convert.ToByte(p.Value); } }

		sealed class SqlByteDefault<Q>     : Convert<SqlByte,Q>          { public override SqlByte From(Q p)           { return Convert<SqlByte,object>.Instance.From(p); } }
		sealed class SqlByteFromObject     : Convert<SqlByte,object>     { public override SqlByte From(object p)      { return Convert.ToByte(p); } }

		static Convert<T, P> GetSqlByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlByteFromByte());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlByteFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlByteFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlByteFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlByteFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlByteFromInt64());

			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlByteFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlByteFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlByteFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlByteFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlByteFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlByteFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlByteFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlByteFromChar());

			// Nullable Types.
			//
			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlByteFromNullableByte());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlByteFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlByteFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlByteFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlByteFromNullableInt64());

			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlByteFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlByteFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlByteFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlByteFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlByteFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlByteFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlByteFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlByteFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlByteFromSqlString());

			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlByteFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlByteFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlByteFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlByteFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlByteFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlByteFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlByteFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlByteFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlByteFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlByteFromObject());

			return (Convert<T, P>)(object)(new SqlByteDefault<P>());
		}

		#endregion

		#region SqlInt16


		// Scalar Types.
		//
		sealed class SqlInt16FromInt16      : Convert<SqlInt16,Int16>      { public override SqlInt16 From(Int16 p)       { return p; } }
		sealed class SqlInt16FromString     : Convert<SqlInt16,String>     { public override SqlInt16 From(String p)      { return p == null? SqlInt16.Null: SqlInt16.Parse(p); } }

		sealed class SqlInt16FromSByte      : Convert<SqlInt16,SByte>      { public override SqlInt16 From(SByte p)       { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromInt32      : Convert<SqlInt16,Int32>      { public override SqlInt16 From(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromInt64      : Convert<SqlInt16,Int64>      { public override SqlInt16 From(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class SqlInt16FromByte       : Convert<SqlInt16,Byte>       { public override SqlInt16 From(Byte p)        { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromUInt16     : Convert<SqlInt16,UInt16>     { public override SqlInt16 From(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromUInt32     : Convert<SqlInt16,UInt32>     { public override SqlInt16 From(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromUInt64     : Convert<SqlInt16,UInt64>     { public override SqlInt16 From(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class SqlInt16FromSingle     : Convert<SqlInt16,Single>     { public override SqlInt16 From(Single p)      { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromDouble     : Convert<SqlInt16,Double>     { public override SqlInt16 From(Double p)      { return Convert.ToInt16(p); } }

		sealed class SqlInt16FromBoolean    : Convert<SqlInt16,Boolean>    { public override SqlInt16 From(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromDecimal    : Convert<SqlInt16,Decimal>    { public override SqlInt16 From(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class SqlInt16FromChar       : Convert<SqlInt16,Char>       { public override SqlInt16 From(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class SqlInt16FromNullableInt16      : Convert<SqlInt16,Int16?>      { public override SqlInt16 From(Int16? p)       { return p.HasValue?                 p.Value  : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableSByte      : Convert<SqlInt16,SByte?>      { public override SqlInt16 From(SByte? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableInt32      : Convert<SqlInt16,Int32?>      { public override SqlInt16 From(Int32? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableInt64      : Convert<SqlInt16,Int64?>      { public override SqlInt16 From(Int64? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class SqlInt16FromNullableByte       : Convert<SqlInt16,Byte?>       { public override SqlInt16 From(Byte? p)        { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableUInt16     : Convert<SqlInt16,UInt16?>     { public override SqlInt16 From(UInt16? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableUInt32     : Convert<SqlInt16,UInt32?>     { public override SqlInt16 From(UInt32? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableUInt64     : Convert<SqlInt16,UInt64?>     { public override SqlInt16 From(UInt64? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class SqlInt16FromNullableSingle     : Convert<SqlInt16,Single?>     { public override SqlInt16 From(Single? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableDouble     : Convert<SqlInt16,Double?>     { public override SqlInt16 From(Double? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class SqlInt16FromNullableBoolean    : Convert<SqlInt16,Boolean?>    { public override SqlInt16 From(Boolean? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableDecimal    : Convert<SqlInt16,Decimal?>    { public override SqlInt16 From(Decimal? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class SqlInt16FromNullableChar       : Convert<SqlInt16,Char?>       { public override SqlInt16 From(Char? p)        { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		// SqlTypes.
		//
		sealed class SqlInt16FromSqlString  : Convert<SqlInt16,SqlString>  { public override SqlInt16 From(SqlString p)   { return p.ToSqlInt16(); } }

		sealed class SqlInt16FromSqlByte    : Convert<SqlInt16,SqlByte>    { public override SqlInt16 From(SqlByte p)     { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlInt32   : Convert<SqlInt16,SqlInt32>   { public override SqlInt16 From(SqlInt32 p)    { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlInt64   : Convert<SqlInt16,SqlInt64>   { public override SqlInt16 From(SqlInt64 p)    { return p.ToSqlInt16(); } }

		sealed class SqlInt16FromSqlSingle  : Convert<SqlInt16,SqlSingle>  { public override SqlInt16 From(SqlSingle p)   { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlDouble  : Convert<SqlInt16,SqlDouble>  { public override SqlInt16 From(SqlDouble p)   { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlDecimal : Convert<SqlInt16,SqlDecimal> { public override SqlInt16 From(SqlDecimal p)  { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlMoney   : Convert<SqlInt16,SqlMoney>   { public override SqlInt16 From(SqlMoney p)    { return p.ToSqlInt16(); } }

		sealed class SqlInt16FromSqlBoolean : Convert<SqlInt16,SqlBoolean> { public override SqlInt16 From(SqlBoolean p)  { return p.ToSqlInt16(); } }
		sealed class SqlInt16FromSqlDateTime: Convert<SqlInt16,SqlDateTime>{ public override SqlInt16 From(SqlDateTime p) { return p.IsNull? SqlInt16.Null: Convert.ToInt16(p.Value); } }

		sealed class SqlInt16Default<Q>     : Convert<SqlInt16,Q>          { public override SqlInt16 From(Q p)           { return Convert<SqlInt16,object>.Instance.From(p); } }
		sealed class SqlInt16FromObject     : Convert<SqlInt16,object>     { public override SqlInt16 From(object p)      { return Convert.ToInt16(p); } }

		static Convert<T, P> GetSqlInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlInt16FromInt16());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlInt16FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlInt16FromSByte());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlInt16FromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlInt16FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlInt16FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlInt16FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlInt16FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlInt16FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlInt16FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlInt16FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlInt16FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlInt16FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlInt16FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlInt16FromNullableInt16());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlInt16FromNullableSByte());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlInt16FromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlInt16FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlInt16FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlInt16FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlInt16FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlInt16FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlInt16FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlInt16FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlInt16FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlInt16FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlInt16FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlInt16FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlInt16FromSqlByte());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlInt16FromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlInt16FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlInt16FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlInt16FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlInt16FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlInt16FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlInt16FromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlInt16FromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlInt16FromObject());

			return (Convert<T, P>)(object)(new SqlInt16Default<P>());
		}

		#endregion

		#region SqlInt32


		// Scalar Types.
		//
		sealed class SqlInt32FromInt32      : Convert<SqlInt32,Int32>      { public override SqlInt32 From(Int32 p)       { return p; } }
		sealed class SqlInt32FromString     : Convert<SqlInt32,String>     { public override SqlInt32 From(String p)      { return p == null? SqlInt32.Null: SqlInt32.Parse(p); } }

		sealed class SqlInt32FromSByte      : Convert<SqlInt32,SByte>      { public override SqlInt32 From(SByte p)       { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromInt16      : Convert<SqlInt32,Int16>      { public override SqlInt32 From(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromInt64      : Convert<SqlInt32,Int64>      { public override SqlInt32 From(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class SqlInt32FromByte       : Convert<SqlInt32,Byte>       { public override SqlInt32 From(Byte p)        { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromUInt16     : Convert<SqlInt32,UInt16>     { public override SqlInt32 From(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromUInt32     : Convert<SqlInt32,UInt32>     { public override SqlInt32 From(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromUInt64     : Convert<SqlInt32,UInt64>     { public override SqlInt32 From(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class SqlInt32FromSingle     : Convert<SqlInt32,Single>     { public override SqlInt32 From(Single p)      { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromDouble     : Convert<SqlInt32,Double>     { public override SqlInt32 From(Double p)      { return Convert.ToInt32(p); } }

		sealed class SqlInt32FromBoolean    : Convert<SqlInt32,Boolean>    { public override SqlInt32 From(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromDecimal    : Convert<SqlInt32,Decimal>    { public override SqlInt32 From(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class SqlInt32FromChar       : Convert<SqlInt32,Char>       { public override SqlInt32 From(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class SqlInt32FromNullableInt32      : Convert<SqlInt32,Int32?>      { public override SqlInt32 From(Int32? p)       { return p.HasValue?                 p.Value  : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableSByte      : Convert<SqlInt32,SByte?>      { public override SqlInt32 From(SByte? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableInt16      : Convert<SqlInt32,Int16?>      { public override SqlInt32 From(Int16? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableInt64      : Convert<SqlInt32,Int64?>      { public override SqlInt32 From(Int64? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class SqlInt32FromNullableByte       : Convert<SqlInt32,Byte?>       { public override SqlInt32 From(Byte? p)        { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableUInt16     : Convert<SqlInt32,UInt16?>     { public override SqlInt32 From(UInt16? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableUInt32     : Convert<SqlInt32,UInt32?>     { public override SqlInt32 From(UInt32? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableUInt64     : Convert<SqlInt32,UInt64?>     { public override SqlInt32 From(UInt64? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class SqlInt32FromNullableSingle     : Convert<SqlInt32,Single?>     { public override SqlInt32 From(Single? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableDouble     : Convert<SqlInt32,Double?>     { public override SqlInt32 From(Double? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class SqlInt32FromNullableBoolean    : Convert<SqlInt32,Boolean?>    { public override SqlInt32 From(Boolean? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableDecimal    : Convert<SqlInt32,Decimal?>    { public override SqlInt32 From(Decimal? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class SqlInt32FromNullableChar       : Convert<SqlInt32,Char?>       { public override SqlInt32 From(Char? p)        { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		// SqlTypes.
		//
		sealed class SqlInt32FromSqlString  : Convert<SqlInt32,SqlString>  { public override SqlInt32 From(SqlString p)   { return p.ToSqlInt32(); } }

		sealed class SqlInt32FromSqlByte    : Convert<SqlInt32,SqlByte>    { public override SqlInt32 From(SqlByte p)     { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlInt16   : Convert<SqlInt32,SqlInt16>   { public override SqlInt32 From(SqlInt16 p)    { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlInt64   : Convert<SqlInt32,SqlInt64>   { public override SqlInt32 From(SqlInt64 p)    { return p.ToSqlInt32(); } }

		sealed class SqlInt32FromSqlSingle  : Convert<SqlInt32,SqlSingle>  { public override SqlInt32 From(SqlSingle p)   { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlDouble  : Convert<SqlInt32,SqlDouble>  { public override SqlInt32 From(SqlDouble p)   { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlDecimal : Convert<SqlInt32,SqlDecimal> { public override SqlInt32 From(SqlDecimal p)  { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlMoney   : Convert<SqlInt32,SqlMoney>   { public override SqlInt32 From(SqlMoney p)    { return p.ToSqlInt32(); } }

		sealed class SqlInt32FromSqlBoolean : Convert<SqlInt32,SqlBoolean> { public override SqlInt32 From(SqlBoolean p)  { return p.ToSqlInt32(); } }
		sealed class SqlInt32FromSqlDateTime: Convert<SqlInt32,SqlDateTime>{ public override SqlInt32 From(SqlDateTime p) { return p.IsNull? SqlInt32.Null: Convert.ToInt32(p.Value); } }

		sealed class SqlInt32Default<Q>     : Convert<SqlInt32,Q>          { public override SqlInt32 From(Q p)           { return Convert<SqlInt32,object>.Instance.From(p); } }
		sealed class SqlInt32FromObject     : Convert<SqlInt32,object>     { public override SqlInt32 From(object p)      { return Convert.ToInt32(p); } }

		static Convert<T, P> GetSqlInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlInt32FromInt32());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlInt32FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlInt32FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlInt32FromInt16());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlInt32FromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlInt32FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlInt32FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlInt32FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlInt32FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlInt32FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlInt32FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlInt32FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlInt32FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlInt32FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlInt32FromNullableInt32());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlInt32FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlInt32FromNullableInt16());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlInt32FromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlInt32FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlInt32FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlInt32FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlInt32FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlInt32FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlInt32FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlInt32FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlInt32FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlInt32FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlInt32FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlInt32FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlInt32FromSqlInt16());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlInt32FromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlInt32FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlInt32FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlInt32FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlInt32FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlInt32FromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlInt32FromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlInt32FromObject());

			return (Convert<T, P>)(object)(new SqlInt32Default<P>());
		}

		#endregion

		#region SqlInt64


		// Scalar Types.
		//
		sealed class SqlInt64FromInt64      : Convert<SqlInt64,Int64>      { public override SqlInt64 From(Int64 p)       { return p; } }
		sealed class SqlInt64FromString     : Convert<SqlInt64,String>     { public override SqlInt64 From(String p)      { return p == null? SqlInt64.Null: SqlInt64.Parse(p); } }

		sealed class SqlInt64FromSByte      : Convert<SqlInt64,SByte>      { public override SqlInt64 From(SByte p)       { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromInt16      : Convert<SqlInt64,Int16>      { public override SqlInt64 From(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromInt32      : Convert<SqlInt64,Int32>      { public override SqlInt64 From(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class SqlInt64FromByte       : Convert<SqlInt64,Byte>       { public override SqlInt64 From(Byte p)        { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromUInt16     : Convert<SqlInt64,UInt16>     { public override SqlInt64 From(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromUInt32     : Convert<SqlInt64,UInt32>     { public override SqlInt64 From(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromUInt64     : Convert<SqlInt64,UInt64>     { public override SqlInt64 From(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class SqlInt64FromSingle     : Convert<SqlInt64,Single>     { public override SqlInt64 From(Single p)      { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromDouble     : Convert<SqlInt64,Double>     { public override SqlInt64 From(Double p)      { return Convert.ToInt64(p); } }

		sealed class SqlInt64FromBoolean    : Convert<SqlInt64,Boolean>    { public override SqlInt64 From(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromDecimal    : Convert<SqlInt64,Decimal>    { public override SqlInt64 From(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class SqlInt64FromChar       : Convert<SqlInt64,Char>       { public override SqlInt64 From(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class SqlInt64FromNullableInt64      : Convert<SqlInt64,Int64?>      { public override SqlInt64 From(Int64? p)       { return p.HasValue?                 p.Value  : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableSByte      : Convert<SqlInt64,SByte?>      { public override SqlInt64 From(SByte? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableInt16      : Convert<SqlInt64,Int16?>      { public override SqlInt64 From(Int16? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableInt32      : Convert<SqlInt64,Int32?>      { public override SqlInt64 From(Int32? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class SqlInt64FromNullableByte       : Convert<SqlInt64,Byte?>       { public override SqlInt64 From(Byte? p)        { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableUInt16     : Convert<SqlInt64,UInt16?>     { public override SqlInt64 From(UInt16? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableUInt32     : Convert<SqlInt64,UInt32?>     { public override SqlInt64 From(UInt32? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableUInt64     : Convert<SqlInt64,UInt64?>     { public override SqlInt64 From(UInt64? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class SqlInt64FromNullableSingle     : Convert<SqlInt64,Single?>     { public override SqlInt64 From(Single? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableDouble     : Convert<SqlInt64,Double?>     { public override SqlInt64 From(Double? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class SqlInt64FromNullableBoolean    : Convert<SqlInt64,Boolean?>    { public override SqlInt64 From(Boolean? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableDecimal    : Convert<SqlInt64,Decimal?>    { public override SqlInt64 From(Decimal? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class SqlInt64FromNullableChar       : Convert<SqlInt64,Char?>       { public override SqlInt64 From(Char? p)        { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		// SqlTypes.
		//
		sealed class SqlInt64FromSqlString  : Convert<SqlInt64,SqlString>  { public override SqlInt64 From(SqlString p)   { return p.ToSqlInt64(); } }

		sealed class SqlInt64FromSqlByte    : Convert<SqlInt64,SqlByte>    { public override SqlInt64 From(SqlByte p)     { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlInt16   : Convert<SqlInt64,SqlInt16>   { public override SqlInt64 From(SqlInt16 p)    { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlInt32   : Convert<SqlInt64,SqlInt32>   { public override SqlInt64 From(SqlInt32 p)    { return p.ToSqlInt64(); } }

		sealed class SqlInt64FromSqlSingle  : Convert<SqlInt64,SqlSingle>  { public override SqlInt64 From(SqlSingle p)   { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlDouble  : Convert<SqlInt64,SqlDouble>  { public override SqlInt64 From(SqlDouble p)   { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlDecimal : Convert<SqlInt64,SqlDecimal> { public override SqlInt64 From(SqlDecimal p)  { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlMoney   : Convert<SqlInt64,SqlMoney>   { public override SqlInt64 From(SqlMoney p)    { return p.ToSqlInt64(); } }

		sealed class SqlInt64FromSqlBoolean : Convert<SqlInt64,SqlBoolean> { public override SqlInt64 From(SqlBoolean p)  { return p.ToSqlInt64(); } }
		sealed class SqlInt64FromSqlDateTime: Convert<SqlInt64,SqlDateTime>{ public override SqlInt64 From(SqlDateTime p) { return p.IsNull? SqlInt64.Null: Convert.ToInt64(p.Value); } }

		sealed class SqlInt64Default<Q>     : Convert<SqlInt64,Q>          { public override SqlInt64 From(Q p)           { return Convert<SqlInt64,object>.Instance.From(p); } }
		sealed class SqlInt64FromObject     : Convert<SqlInt64,object>     { public override SqlInt64 From(object p)      { return Convert.ToInt64(p); } }

		static Convert<T, P> GetSqlInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlInt64FromInt64());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlInt64FromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlInt64FromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlInt64FromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlInt64FromInt32());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlInt64FromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlInt64FromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlInt64FromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlInt64FromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlInt64FromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlInt64FromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlInt64FromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlInt64FromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlInt64FromChar());

			// Nullable Types.
			//
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlInt64FromNullableInt64());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlInt64FromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlInt64FromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlInt64FromNullableInt32());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlInt64FromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlInt64FromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlInt64FromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlInt64FromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlInt64FromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlInt64FromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlInt64FromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlInt64FromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlInt64FromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlInt64FromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlInt64FromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlInt64FromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlInt64FromSqlInt32());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlInt64FromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlInt64FromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlInt64FromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlInt64FromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlInt64FromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlInt64FromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlInt64FromObject());

			return (Convert<T, P>)(object)(new SqlInt64Default<P>());
		}

		#endregion

		#region SqlSingle


		// Scalar Types.
		//
		sealed class SqlSingleFromSingle     : Convert<SqlSingle,Single>     { public override SqlSingle From(Single p)      { return p; } }
		sealed class SqlSingleFromString     : Convert<SqlSingle,String>     { public override SqlSingle From(String p)      { return p == null? SqlSingle.Null: SqlSingle.Parse(p); } }

		sealed class SqlSingleFromSByte      : Convert<SqlSingle,SByte>      { public override SqlSingle From(SByte p)       { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromInt16      : Convert<SqlSingle,Int16>      { public override SqlSingle From(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromInt32      : Convert<SqlSingle,Int32>      { public override SqlSingle From(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromInt64      : Convert<SqlSingle,Int64>      { public override SqlSingle From(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class SqlSingleFromByte       : Convert<SqlSingle,Byte>       { public override SqlSingle From(Byte p)        { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromUInt16     : Convert<SqlSingle,UInt16>     { public override SqlSingle From(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromUInt32     : Convert<SqlSingle,UInt32>     { public override SqlSingle From(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromUInt64     : Convert<SqlSingle,UInt64>     { public override SqlSingle From(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class SqlSingleFromDouble     : Convert<SqlSingle,Double>     { public override SqlSingle From(Double p)      { return Convert.ToSingle(p); } }

		sealed class SqlSingleFromBoolean    : Convert<SqlSingle,Boolean>    { public override SqlSingle From(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class SqlSingleFromDecimal    : Convert<SqlSingle,Decimal>    { public override SqlSingle From(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class SqlSingleFromNullableSingle     : Convert<SqlSingle,Single?>     { public override SqlSingle From(Single? p)      { return p.HasValue?                  p.Value  : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableSByte      : Convert<SqlSingle,SByte?>      { public override SqlSingle From(SByte? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableInt16      : Convert<SqlSingle,Int16?>      { public override SqlSingle From(Int16? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableInt32      : Convert<SqlSingle,Int32?>      { public override SqlSingle From(Int32? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableInt64      : Convert<SqlSingle,Int64?>      { public override SqlSingle From(Int64? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class SqlSingleFromNullableByte       : Convert<SqlSingle,Byte?>       { public override SqlSingle From(Byte? p)        { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableUInt16     : Convert<SqlSingle,UInt16?>     { public override SqlSingle From(UInt16? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableUInt32     : Convert<SqlSingle,UInt32?>     { public override SqlSingle From(UInt32? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableUInt64     : Convert<SqlSingle,UInt64?>     { public override SqlSingle From(UInt64? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class SqlSingleFromNullableDouble     : Convert<SqlSingle,Double?>     { public override SqlSingle From(Double? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class SqlSingleFromNullableBoolean    : Convert<SqlSingle,Boolean?>    { public override SqlSingle From(Boolean? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class SqlSingleFromNullableDecimal    : Convert<SqlSingle,Decimal?>    { public override SqlSingle From(Decimal? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		// SqlTypes.
		//
		sealed class SqlSingleFromSqlString  : Convert<SqlSingle,SqlString>  { public override SqlSingle From(SqlString p)   { return p.ToSqlSingle(); } }

		sealed class SqlSingleFromSqlByte    : Convert<SqlSingle,SqlByte>    { public override SqlSingle From(SqlByte p)     { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlInt16   : Convert<SqlSingle,SqlInt16>   { public override SqlSingle From(SqlInt16 p)    { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlInt32   : Convert<SqlSingle,SqlInt32>   { public override SqlSingle From(SqlInt32 p)    { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlInt64   : Convert<SqlSingle,SqlInt64>   { public override SqlSingle From(SqlInt64 p)    { return p.ToSqlSingle(); } }

		sealed class SqlSingleFromSqlDouble  : Convert<SqlSingle,SqlDouble>  { public override SqlSingle From(SqlDouble p)   { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlDecimal : Convert<SqlSingle,SqlDecimal> { public override SqlSingle From(SqlDecimal p)  { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlMoney   : Convert<SqlSingle,SqlMoney>   { public override SqlSingle From(SqlMoney p)    { return p.ToSqlSingle(); } }

		sealed class SqlSingleFromSqlBoolean : Convert<SqlSingle,SqlBoolean> { public override SqlSingle From(SqlBoolean p)  { return p.ToSqlSingle(); } }
		sealed class SqlSingleFromSqlDateTime: Convert<SqlSingle,SqlDateTime>{ public override SqlSingle From(SqlDateTime p) { return p.IsNull? SqlSingle.Null: Convert.ToSingle(p.Value); } }

		sealed class SqlSingleDefault<Q>     : Convert<SqlSingle,Q>          { public override SqlSingle From(Q p)           { return Convert<SqlSingle,object>.Instance.From(p); } }
		sealed class SqlSingleFromObject     : Convert<SqlSingle,object>     { public override SqlSingle From(object p)      { return Convert.ToSingle(p); } }

		static Convert<T, P> GetSqlSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlSingleFromSingle());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlSingleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlSingleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlSingleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlSingleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlSingleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlSingleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlSingleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlSingleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlSingleFromUInt64());

			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlSingleFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlSingleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlSingleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlSingleFromNullableSingle());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlSingleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlSingleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlSingleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlSingleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlSingleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlSingleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlSingleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlSingleFromNullableUInt64());

			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlSingleFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlSingleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlSingleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlSingleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlSingleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlSingleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlSingleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlSingleFromSqlInt64());

			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlSingleFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlSingleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlSingleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlSingleFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlSingleFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlSingleFromObject());

			return (Convert<T, P>)(object)(new SqlSingleDefault<P>());
		}

		#endregion

		#region SqlDouble


		// Scalar Types.
		//
		sealed class SqlDoubleFromDouble     : Convert<SqlDouble,Double>     { public override SqlDouble From(Double p)      { return p; } }
		sealed class SqlDoubleFromString     : Convert<SqlDouble,String>     { public override SqlDouble From(String p)      { return p == null? SqlDouble.Null: SqlDouble.Parse(p); } }

		sealed class SqlDoubleFromSByte      : Convert<SqlDouble,SByte>      { public override SqlDouble From(SByte p)       { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromInt16      : Convert<SqlDouble,Int16>      { public override SqlDouble From(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromInt32      : Convert<SqlDouble,Int32>      { public override SqlDouble From(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromInt64      : Convert<SqlDouble,Int64>      { public override SqlDouble From(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class SqlDoubleFromByte       : Convert<SqlDouble,Byte>       { public override SqlDouble From(Byte p)        { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromUInt16     : Convert<SqlDouble,UInt16>     { public override SqlDouble From(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromUInt32     : Convert<SqlDouble,UInt32>     { public override SqlDouble From(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromUInt64     : Convert<SqlDouble,UInt64>     { public override SqlDouble From(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class SqlDoubleFromSingle     : Convert<SqlDouble,Single>     { public override SqlDouble From(Single p)      { return Convert.ToDouble(p); } }

		sealed class SqlDoubleFromBoolean    : Convert<SqlDouble,Boolean>    { public override SqlDouble From(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class SqlDoubleFromDecimal    : Convert<SqlDouble,Decimal>    { public override SqlDouble From(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class SqlDoubleFromNullableDouble     : Convert<SqlDouble,Double?>     { public override SqlDouble From(Double? p)      { return p.HasValue?                  p.Value  : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableSByte      : Convert<SqlDouble,SByte?>      { public override SqlDouble From(SByte? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableInt16      : Convert<SqlDouble,Int16?>      { public override SqlDouble From(Int16? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableInt32      : Convert<SqlDouble,Int32?>      { public override SqlDouble From(Int32? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableInt64      : Convert<SqlDouble,Int64?>      { public override SqlDouble From(Int64? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class SqlDoubleFromNullableByte       : Convert<SqlDouble,Byte?>       { public override SqlDouble From(Byte? p)        { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableUInt16     : Convert<SqlDouble,UInt16?>     { public override SqlDouble From(UInt16? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableUInt32     : Convert<SqlDouble,UInt32?>     { public override SqlDouble From(UInt32? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableUInt64     : Convert<SqlDouble,UInt64?>     { public override SqlDouble From(UInt64? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class SqlDoubleFromNullableSingle     : Convert<SqlDouble,Single?>     { public override SqlDouble From(Single? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class SqlDoubleFromNullableBoolean    : Convert<SqlDouble,Boolean?>    { public override SqlDouble From(Boolean? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class SqlDoubleFromNullableDecimal    : Convert<SqlDouble,Decimal?>    { public override SqlDouble From(Decimal? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		// SqlTypes.
		//
		sealed class SqlDoubleFromSqlString  : Convert<SqlDouble,SqlString>  { public override SqlDouble From(SqlString p)   { return p.ToSqlDouble(); } }

		sealed class SqlDoubleFromSqlByte    : Convert<SqlDouble,SqlByte>    { public override SqlDouble From(SqlByte p)     { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlInt16   : Convert<SqlDouble,SqlInt16>   { public override SqlDouble From(SqlInt16 p)    { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlInt32   : Convert<SqlDouble,SqlInt32>   { public override SqlDouble From(SqlInt32 p)    { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlInt64   : Convert<SqlDouble,SqlInt64>   { public override SqlDouble From(SqlInt64 p)    { return p.ToSqlDouble(); } }

		sealed class SqlDoubleFromSqlSingle  : Convert<SqlDouble,SqlSingle>  { public override SqlDouble From(SqlSingle p)   { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlDecimal : Convert<SqlDouble,SqlDecimal> { public override SqlDouble From(SqlDecimal p)  { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlMoney   : Convert<SqlDouble,SqlMoney>   { public override SqlDouble From(SqlMoney p)    { return p.ToSqlDouble(); } }

		sealed class SqlDoubleFromSqlBoolean : Convert<SqlDouble,SqlBoolean> { public override SqlDouble From(SqlBoolean p)  { return p.ToSqlDouble(); } }
		sealed class SqlDoubleFromSqlDateTime: Convert<SqlDouble,SqlDateTime>{ public override SqlDouble From(SqlDateTime p) { return p.IsNull? SqlDouble.Null: Convert.ToDouble(p.Value); } }

		sealed class SqlDoubleDefault<Q>     : Convert<SqlDouble,Q>          { public override SqlDouble From(Q p)           { return Convert<SqlDouble,object>.Instance.From(p); } }
		sealed class SqlDoubleFromObject     : Convert<SqlDouble,object>     { public override SqlDouble From(object p)      { return Convert.ToDouble(p); } }

		static Convert<T, P> GetSqlDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlDoubleFromDouble());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlDoubleFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlDoubleFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlDoubleFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlDoubleFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlDoubleFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlDoubleFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlDoubleFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlDoubleFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlDoubleFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlDoubleFromSingle());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlDoubleFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlDoubleFromDecimal());

			// Nullable Types.
			//
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlDoubleFromNullableDouble());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlDoubleFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlDoubleFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlDoubleFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlDoubleFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlDoubleFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlDoubleFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlDoubleFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlDoubleFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlDoubleFromNullableSingle());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlDoubleFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlDoubleFromNullableDecimal());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlDoubleFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlDoubleFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlDoubleFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlDoubleFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlDoubleFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlDoubleFromSqlSingle());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlDoubleFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlDoubleFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlDoubleFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlDoubleFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlDoubleFromObject());

			return (Convert<T, P>)(object)(new SqlDoubleDefault<P>());
		}

		#endregion

		#region SqlDecimal


		// Scalar Types.
		//
		sealed class SqlDecimalFromDecimal    : Convert<SqlDecimal,Decimal>    { public override SqlDecimal From(Decimal p)     { return p; } }
		sealed class SqlDecimalFromString     : Convert<SqlDecimal,String>     { public override SqlDecimal From(String p)      { return p == null? SqlDecimal.Null: SqlDecimal.Parse(p); } }

		sealed class SqlDecimalFromSByte      : Convert<SqlDecimal,SByte>      { public override SqlDecimal From(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromInt16      : Convert<SqlDecimal,Int16>      { public override SqlDecimal From(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromInt32      : Convert<SqlDecimal,Int32>      { public override SqlDecimal From(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromInt64      : Convert<SqlDecimal,Int64>      { public override SqlDecimal From(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class SqlDecimalFromByte       : Convert<SqlDecimal,Byte>       { public override SqlDecimal From(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromUInt16     : Convert<SqlDecimal,UInt16>     { public override SqlDecimal From(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromUInt32     : Convert<SqlDecimal,UInt32>     { public override SqlDecimal From(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromUInt64     : Convert<SqlDecimal,UInt64>     { public override SqlDecimal From(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class SqlDecimalFromSingle     : Convert<SqlDecimal,Single>     { public override SqlDecimal From(Single p)      { return Convert.ToDecimal(p); } }
		sealed class SqlDecimalFromDouble     : Convert<SqlDecimal,Double>     { public override SqlDecimal From(Double p)      { return Convert.ToDecimal(p); } }

		sealed class SqlDecimalFromBoolean    : Convert<SqlDecimal,Boolean>    { public override SqlDecimal From(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class SqlDecimalFromNullableDecimal    : Convert<SqlDecimal,Decimal?>    { public override SqlDecimal From(Decimal? p)     { return p.HasValue?                   p.Value  : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableSByte      : Convert<SqlDecimal,SByte?>      { public override SqlDecimal From(SByte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableInt16      : Convert<SqlDecimal,Int16?>      { public override SqlDecimal From(Int16? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableInt32      : Convert<SqlDecimal,Int32?>      { public override SqlDecimal From(Int32? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableInt64      : Convert<SqlDecimal,Int64?>      { public override SqlDecimal From(Int64? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class SqlDecimalFromNullableByte       : Convert<SqlDecimal,Byte?>       { public override SqlDecimal From(Byte? p)        { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableUInt16     : Convert<SqlDecimal,UInt16?>     { public override SqlDecimal From(UInt16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableUInt32     : Convert<SqlDecimal,UInt32?>     { public override SqlDecimal From(UInt32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableUInt64     : Convert<SqlDecimal,UInt64?>     { public override SqlDecimal From(UInt64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class SqlDecimalFromNullableSingle     : Convert<SqlDecimal,Single?>     { public override SqlDecimal From(Single? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class SqlDecimalFromNullableDouble     : Convert<SqlDecimal,Double?>     { public override SqlDecimal From(Double? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class SqlDecimalFromNullableBoolean    : Convert<SqlDecimal,Boolean?>    { public override SqlDecimal From(Boolean? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		// SqlTypes.
		//
		sealed class SqlDecimalFromSqlString  : Convert<SqlDecimal,SqlString>  { public override SqlDecimal From(SqlString p)   { return p.ToSqlDecimal(); } }

		sealed class SqlDecimalFromSqlByte    : Convert<SqlDecimal,SqlByte>    { public override SqlDecimal From(SqlByte p)     { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlInt16   : Convert<SqlDecimal,SqlInt16>   { public override SqlDecimal From(SqlInt16 p)    { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlInt32   : Convert<SqlDecimal,SqlInt32>   { public override SqlDecimal From(SqlInt32 p)    { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlInt64   : Convert<SqlDecimal,SqlInt64>   { public override SqlDecimal From(SqlInt64 p)    { return p.ToSqlDecimal(); } }

		sealed class SqlDecimalFromSqlSingle  : Convert<SqlDecimal,SqlSingle>  { public override SqlDecimal From(SqlSingle p)   { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlDouble  : Convert<SqlDecimal,SqlDouble>  { public override SqlDecimal From(SqlDouble p)   { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlMoney   : Convert<SqlDecimal,SqlMoney>   { public override SqlDecimal From(SqlMoney p)    { return p.ToSqlDecimal(); } }

		sealed class SqlDecimalFromSqlBoolean : Convert<SqlDecimal,SqlBoolean> { public override SqlDecimal From(SqlBoolean p)  { return p.ToSqlDecimal(); } }
		sealed class SqlDecimalFromSqlDateTime: Convert<SqlDecimal,SqlDateTime>{ public override SqlDecimal From(SqlDateTime p) { return p.IsNull? SqlDecimal.Null: Convert.ToDecimal(p.Value); } }

		sealed class SqlDecimalDefault<Q>     : Convert<SqlDecimal,Q>          { public override SqlDecimal From(Q p)           { return Convert<SqlDecimal,object>.Instance.From(p); } }
		sealed class SqlDecimalFromObject     : Convert<SqlDecimal,object>     { public override SqlDecimal From(object p)      { return Convert.ToDecimal(p); } }

		static Convert<T, P> GetSqlDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlDecimalFromDecimal());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlDecimalFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlDecimalFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlDecimalFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlDecimalFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlDecimalFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlDecimalFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlDecimalFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlDecimalFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlDecimalFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlDecimalFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlDecimalFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlDecimalFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlDecimalFromNullableDecimal());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlDecimalFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlDecimalFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlDecimalFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlDecimalFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlDecimalFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlDecimalFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlDecimalFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlDecimalFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlDecimalFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlDecimalFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlDecimalFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlDecimalFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlDecimalFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlDecimalFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlDecimalFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlDecimalFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlDecimalFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlDecimalFromSqlDouble());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlDecimalFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlDecimalFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlDecimalFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlDecimalFromObject());

			return (Convert<T, P>)(object)(new SqlDecimalDefault<P>());
		}

		#endregion

		#region SqlMoney


		// Scalar Types.
		//
		sealed class SqlMoneyFromDecimal    : Convert<SqlMoney,Decimal>    { public override SqlMoney From(Decimal p)     { return p; } }
		sealed class SqlMoneyFromString     : Convert<SqlMoney,String>     { public override SqlMoney From(String p)      { return p == null? SqlMoney.Null: SqlMoney.Parse(p); } }

		sealed class SqlMoneyFromSByte      : Convert<SqlMoney,SByte>      { public override SqlMoney From(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromInt16      : Convert<SqlMoney,Int16>      { public override SqlMoney From(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromInt32      : Convert<SqlMoney,Int32>      { public override SqlMoney From(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromInt64      : Convert<SqlMoney,Int64>      { public override SqlMoney From(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class SqlMoneyFromByte       : Convert<SqlMoney,Byte>       { public override SqlMoney From(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromUInt16     : Convert<SqlMoney,UInt16>     { public override SqlMoney From(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromUInt32     : Convert<SqlMoney,UInt32>     { public override SqlMoney From(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromUInt64     : Convert<SqlMoney,UInt64>     { public override SqlMoney From(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class SqlMoneyFromSingle     : Convert<SqlMoney,Single>     { public override SqlMoney From(Single p)      { return Convert.ToDecimal(p); } }
		sealed class SqlMoneyFromDouble     : Convert<SqlMoney,Double>     { public override SqlMoney From(Double p)      { return Convert.ToDecimal(p); } }

		sealed class SqlMoneyFromBoolean    : Convert<SqlMoney,Boolean>    { public override SqlMoney From(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class SqlMoneyFromNullableDecimal    : Convert<SqlMoney,Decimal?>    { public override SqlMoney From(Decimal? p)     { return p.HasValue?                   p.Value  : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableSByte      : Convert<SqlMoney,SByte?>      { public override SqlMoney From(SByte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableInt16      : Convert<SqlMoney,Int16?>      { public override SqlMoney From(Int16? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableInt32      : Convert<SqlMoney,Int32?>      { public override SqlMoney From(Int32? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableInt64      : Convert<SqlMoney,Int64?>      { public override SqlMoney From(Int64? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class SqlMoneyFromNullableByte       : Convert<SqlMoney,Byte?>       { public override SqlMoney From(Byte? p)        { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableUInt16     : Convert<SqlMoney,UInt16?>     { public override SqlMoney From(UInt16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableUInt32     : Convert<SqlMoney,UInt32?>     { public override SqlMoney From(UInt32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableUInt64     : Convert<SqlMoney,UInt64?>     { public override SqlMoney From(UInt64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class SqlMoneyFromNullableSingle     : Convert<SqlMoney,Single?>     { public override SqlMoney From(Single? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class SqlMoneyFromNullableDouble     : Convert<SqlMoney,Double?>     { public override SqlMoney From(Double? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class SqlMoneyFromNullableBoolean    : Convert<SqlMoney,Boolean?>    { public override SqlMoney From(Boolean? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		// SqlTypes.
		//
		sealed class SqlMoneyFromSqlString  : Convert<SqlMoney,SqlString>  { public override SqlMoney From(SqlString p)   { return p.ToSqlMoney(); } }

		sealed class SqlMoneyFromSqlByte    : Convert<SqlMoney,SqlByte>    { public override SqlMoney From(SqlByte p)     { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlInt16   : Convert<SqlMoney,SqlInt16>   { public override SqlMoney From(SqlInt16 p)    { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlInt32   : Convert<SqlMoney,SqlInt32>   { public override SqlMoney From(SqlInt32 p)    { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlInt64   : Convert<SqlMoney,SqlInt64>   { public override SqlMoney From(SqlInt64 p)    { return p.ToSqlMoney(); } }

		sealed class SqlMoneyFromSqlSingle  : Convert<SqlMoney,SqlSingle>  { public override SqlMoney From(SqlSingle p)   { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlDouble  : Convert<SqlMoney,SqlDouble>  { public override SqlMoney From(SqlDouble p)   { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlDecimal : Convert<SqlMoney,SqlDecimal> { public override SqlMoney From(SqlDecimal p)  { return p.ToSqlMoney(); } }

		sealed class SqlMoneyFromSqlBoolean : Convert<SqlMoney,SqlBoolean> { public override SqlMoney From(SqlBoolean p)  { return p.ToSqlMoney(); } }
		sealed class SqlMoneyFromSqlDateTime: Convert<SqlMoney,SqlDateTime>{ public override SqlMoney From(SqlDateTime p) { return p.IsNull? SqlMoney.Null: Convert.ToDecimal(p.Value); } }

		sealed class SqlMoneyDefault<Q>     : Convert<SqlMoney,Q>          { public override SqlMoney From(Q p)           { return Convert<SqlMoney,object>.Instance.From(p); } }
		sealed class SqlMoneyFromObject     : Convert<SqlMoney,object>     { public override SqlMoney From(object p)      { return Convert.ToDecimal(p); } }

		static Convert<T, P> GetSqlMoneyConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlMoneyFromDecimal());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlMoneyFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlMoneyFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlMoneyFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlMoneyFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlMoneyFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlMoneyFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlMoneyFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlMoneyFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlMoneyFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlMoneyFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlMoneyFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlMoneyFromBoolean());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlMoneyFromNullableDecimal());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlMoneyFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlMoneyFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlMoneyFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlMoneyFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlMoneyFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlMoneyFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlMoneyFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlMoneyFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlMoneyFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlMoneyFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlMoneyFromNullableBoolean());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlMoneyFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlMoneyFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlMoneyFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlMoneyFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlMoneyFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlMoneyFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlMoneyFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlMoneyFromSqlDecimal());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlMoneyFromSqlBoolean());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlMoneyFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlMoneyFromObject());

			return (Convert<T, P>)(object)(new SqlMoneyDefault<P>());
		}

		#endregion

		#region SqlBoolean


		// Scalar Types.
		//
		sealed class SqlBooleanFromBoolean    : Convert<SqlBoolean,Boolean>    { public override SqlBoolean From(Boolean p)     { return p; } }
		sealed class SqlBooleanFromString     : Convert<SqlBoolean,String>     { public override SqlBoolean From(String p)      { return p == null? SqlBoolean.Null: SqlBoolean.Parse(p); } }

		sealed class SqlBooleanFromSByte      : Convert<SqlBoolean,SByte>      { public override SqlBoolean From(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromInt16      : Convert<SqlBoolean,Int16>      { public override SqlBoolean From(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromInt32      : Convert<SqlBoolean,Int32>      { public override SqlBoolean From(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromInt64      : Convert<SqlBoolean,Int64>      { public override SqlBoolean From(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class SqlBooleanFromByte       : Convert<SqlBoolean,Byte>       { public override SqlBoolean From(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromUInt16     : Convert<SqlBoolean,UInt16>     { public override SqlBoolean From(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromUInt32     : Convert<SqlBoolean,UInt32>     { public override SqlBoolean From(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromUInt64     : Convert<SqlBoolean,UInt64>     { public override SqlBoolean From(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class SqlBooleanFromSingle     : Convert<SqlBoolean,Single>     { public override SqlBoolean From(Single p)      { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromDouble     : Convert<SqlBoolean,Double>     { public override SqlBoolean From(Double p)      { return Convert.ToBoolean(p); } }

		sealed class SqlBooleanFromDecimal    : Convert<SqlBoolean,Decimal>    { public override SqlBoolean From(Decimal p)     { return Convert.ToBoolean(p); } }
		sealed class SqlBooleanFromChar       : Convert<SqlBoolean,Char>       { public override SqlBoolean From(Char p)        { return Convert<Boolean,Char>.Instance.From(p); } }

		// Nullable Types.
		//
		sealed class SqlBooleanFromNullableBoolean    : Convert<SqlBoolean,Boolean?>    { public override SqlBoolean From(Boolean? p)     { return p.HasValue?                   p.Value  : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableSByte      : Convert<SqlBoolean,SByte?>      { public override SqlBoolean From(SByte? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableInt16      : Convert<SqlBoolean,Int16?>      { public override SqlBoolean From(Int16? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableInt32      : Convert<SqlBoolean,Int32?>      { public override SqlBoolean From(Int32? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableInt64      : Convert<SqlBoolean,Int64?>      { public override SqlBoolean From(Int64? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class SqlBooleanFromNullableByte       : Convert<SqlBoolean,Byte?>       { public override SqlBoolean From(Byte? p)        { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableUInt16     : Convert<SqlBoolean,UInt16?>     { public override SqlBoolean From(UInt16? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableUInt32     : Convert<SqlBoolean,UInt32?>     { public override SqlBoolean From(UInt32? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableUInt64     : Convert<SqlBoolean,UInt64?>     { public override SqlBoolean From(UInt64? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class SqlBooleanFromNullableSingle     : Convert<SqlBoolean,Single?>     { public override SqlBoolean From(Single? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableDouble     : Convert<SqlBoolean,Double?>     { public override SqlBoolean From(Double? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class SqlBooleanFromNullableDecimal    : Convert<SqlBoolean,Decimal?>    { public override SqlBoolean From(Decimal? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class SqlBooleanFromNullableChar       : Convert<SqlBoolean,Char?>       { public override SqlBoolean From(Char? p)        { return (p.HasValue)? Convert<Boolean,Char>.Instance.From(p.Value): SqlBoolean.Null; } }

		// SqlTypes.
		//
		sealed class SqlBooleanFromSqlString  : Convert<SqlBoolean,SqlString>  { public override SqlBoolean From(SqlString p)   { return p.ToSqlBoolean(); } }

		sealed class SqlBooleanFromSqlByte    : Convert<SqlBoolean,SqlByte>    { public override SqlBoolean From(SqlByte p)     { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlInt16   : Convert<SqlBoolean,SqlInt16>   { public override SqlBoolean From(SqlInt16 p)    { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlInt32   : Convert<SqlBoolean,SqlInt32>   { public override SqlBoolean From(SqlInt32 p)    { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlInt64   : Convert<SqlBoolean,SqlInt64>   { public override SqlBoolean From(SqlInt64 p)    { return p.ToSqlBoolean(); } }

		sealed class SqlBooleanFromSqlSingle  : Convert<SqlBoolean,SqlSingle>  { public override SqlBoolean From(SqlSingle p)   { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlDouble  : Convert<SqlBoolean,SqlDouble>  { public override SqlBoolean From(SqlDouble p)   { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlDecimal : Convert<SqlBoolean,SqlDecimal> { public override SqlBoolean From(SqlDecimal p)  { return p.ToSqlBoolean(); } }
		sealed class SqlBooleanFromSqlMoney   : Convert<SqlBoolean,SqlMoney>   { public override SqlBoolean From(SqlMoney p)    { return p.ToSqlBoolean(); } }

		sealed class SqlBooleanFromSqlDateTime: Convert<SqlBoolean,SqlDateTime>{ public override SqlBoolean From(SqlDateTime p) { return p.IsNull? SqlBoolean.Null: Convert.ToBoolean(p.Value); } }

		sealed class SqlBooleanDefault<Q>     : Convert<SqlBoolean,Q>          { public override SqlBoolean From(Q p)           { return Convert<SqlBoolean,object>.Instance.From(p); } }
		sealed class SqlBooleanFromObject     : Convert<SqlBoolean,object>     { public override SqlBoolean From(object p)      { return Convert.ToBoolean(p); } }

		static Convert<T, P> GetSqlBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlBooleanFromBoolean());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlBooleanFromString());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlBooleanFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlBooleanFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlBooleanFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlBooleanFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlBooleanFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlBooleanFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlBooleanFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlBooleanFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlBooleanFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlBooleanFromDouble());

			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlBooleanFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlBooleanFromChar());

			// Nullable Types.
			//
			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlBooleanFromNullableBoolean());
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlBooleanFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlBooleanFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlBooleanFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlBooleanFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlBooleanFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlBooleanFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlBooleanFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlBooleanFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlBooleanFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlBooleanFromNullableDouble());

			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlBooleanFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlBooleanFromNullableChar());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlBooleanFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlBooleanFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlBooleanFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlBooleanFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlBooleanFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlBooleanFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlBooleanFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlBooleanFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlBooleanFromSqlMoney());

			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlBooleanFromSqlDateTime());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlBooleanFromObject());

			return (Convert<T, P>)(object)(new SqlBooleanDefault<P>());
		}

		#endregion

		#region SqlDateTime


		// Scalar Types.
		//
		sealed class SqlDateTimeFromDateTime   : Convert<SqlDateTime,DateTime>   { public override SqlDateTime From(DateTime p)    { return p; } }
		sealed class SqlDateTimeFromString     : Convert<SqlDateTime,String>     { public override SqlDateTime From(String p)      { return p == null? SqlDateTime.Null: SqlDateTime.Parse(p); } }

		// Nullable Types.
		//
		sealed class SqlDateTimeFromNullableDateTime   : Convert<SqlDateTime,DateTime?>   { public override SqlDateTime From(DateTime? p)    { return p.HasValue?                    p.Value  : SqlDateTime.Null; } }

		// SqlTypes.
		//
		sealed class SqlDateTimeFromSqlString  : Convert<SqlDateTime,SqlString>  { public override SqlDateTime From(SqlString p)   { return p.ToSqlDateTime(); } }

		sealed class SqlDateTimeDefault<Q>     : Convert<SqlDateTime,Q>          { public override SqlDateTime From(Q p)           { return Convert<SqlDateTime,object>.Instance.From(p); } }
		sealed class SqlDateTimeFromObject     : Convert<SqlDateTime,object>     { public override SqlDateTime From(object p)      { return Convert.ToDateTime(p); } }

		static Convert<T, P> GetSqlDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new SqlDateTimeFromDateTime());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlDateTimeFromString());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new SqlDateTimeFromNullableDateTime());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlDateTimeFromSqlString());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlDateTimeFromObject());

			return (Convert<T, P>)(object)(new SqlDateTimeDefault<P>());
		}

		#endregion

		#region SqlGuid


		// Scalar Types.
		//
		sealed class SqlGuidFromGuid       : Convert<SqlGuid,Guid>       { public override SqlGuid From(Guid p)        { return p; } }
		sealed class SqlGuidFromString     : Convert<SqlGuid,String>     { public override SqlGuid From(String p)      { return p == null? SqlGuid.Null: SqlGuid.Parse(p); } }

		// Nullable Types.
		//
		sealed class SqlGuidFromNullableGuid       : Convert<SqlGuid,Guid?>       { public override SqlGuid From(Guid? p)        { return p.HasValue? p.Value : SqlGuid.Null; } }

		// SqlTypes.
		//
		sealed class SqlGuidFromSqlBinary  : Convert<SqlGuid,SqlBinary>  { public override SqlGuid From(SqlBinary p)   { return p.ToSqlGuid(); } }
		sealed class SqlGuidFromSqlString  : Convert<SqlGuid,SqlString>  { public override SqlGuid From(SqlString p)   { return p.ToSqlGuid(); } }
		sealed class SqlGuidFromType       : Convert<SqlGuid,Type>       { public override SqlGuid From(Type p)        { return p == null? SqlGuid.Null: p.GUID; } }

		sealed class SqlGuidDefault<Q>     : Convert<SqlGuid,Q>          { public override SqlGuid From(Q p)           { return Convert<SqlGuid,object>.Instance.From(p); } }
		sealed class SqlGuidFromObject     : Convert<SqlGuid,object>     { public override SqlGuid From(object p)     
			{
				if (p == null)
					return SqlGuid.Null;

				// Scalar Types.
				//
				if (p is Guid)        return Convert<SqlGuid,Guid>       .Instance.From((Guid)p);
				if (p is String)      return Convert<SqlGuid,String>     .Instance.From((String)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlGuid,Guid>       .Instance.From((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBinary)   return Convert<SqlGuid,SqlBinary>  .Instance.From((SqlBinary)p);
				if (p is SqlString)   return Convert<SqlGuid,SqlString>  .Instance.From((SqlString)p);
				if (p is Type)        return Convert<SqlGuid,Type>       .Instance.From((Type)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetSqlGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new SqlGuidFromGuid());
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlGuidFromString());

			// Nullable Types.
			//
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new SqlGuidFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new SqlGuidFromSqlBinary());
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlGuidFromSqlString());
			if (t == typeof(Type))        return (Convert<T, P>)(object)(new SqlGuidFromType());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlGuidFromObject());

			return (Convert<T, P>)(object)(new SqlGuidDefault<P>());
		}

		#endregion

		#region SqlBinary


		// Scalar Types.
		//
		sealed class SqlBinaryFromByteArray  : Convert<SqlBinary,Byte[]>     { public override SqlBinary From(Byte[] p)      { return p; } }
		sealed class SqlBinaryFromGuid       : Convert<SqlBinary,Guid>       { public override SqlBinary From(Guid p)        { return p == Guid.Empty? SqlBinary.Null: new SqlGuid(p).ToSqlBinary(); } }

		// Nullable Types.
		//
		sealed class SqlBinaryFromNullableGuid       : Convert<SqlBinary,Guid?>       { public override SqlBinary From(Guid? p)        { return p.HasValue? new SqlGuid(p.Value).ToSqlBinary(): SqlBinary.Null; } }

		// SqlTypes.
		//
		sealed class SqlBinaryFromSqlBytes   : Convert<SqlBinary,SqlBytes>   { public override SqlBinary From(SqlBytes p)    { return p.ToSqlBinary(); } }
		sealed class SqlBinaryFromSqlGuid    : Convert<SqlBinary,SqlGuid>    { public override SqlBinary From(SqlGuid p)     { return p.ToSqlBinary(); } }

		sealed class SqlBinaryDefault<Q>     : Convert<SqlBinary,Q>          { public override SqlBinary From(Q p)           { return Convert<SqlBinary,object>.Instance.From(p); } }
		sealed class SqlBinaryFromObject     : Convert<SqlBinary,object>     { public override SqlBinary From(object p)     
			{
				if (p == null)
					return SqlBinary.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<SqlBinary,Byte[]>     .Instance.From((Byte[])p);
				if (p is Guid)        return Convert<SqlBinary,Guid>       .Instance.From((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlBinary,Guid>       .Instance.From((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBytes)    return Convert<SqlBinary,SqlBytes>   .Instance.From((SqlBytes)p);
				if (p is SqlGuid)     return Convert<SqlBinary,SqlGuid>    .Instance.From((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetSqlBinaryConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (Convert<T, P>)(object)(new SqlBinaryFromByteArray());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new SqlBinaryFromGuid());

			// Nullable Types.
			//
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new SqlBinaryFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlBytes))    return (Convert<T, P>)(object)(new SqlBinaryFromSqlBytes());
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new SqlBinaryFromSqlGuid());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlBinaryFromObject());

			return (Convert<T, P>)(object)(new SqlBinaryDefault<P>());
		}

		#endregion

		#region SqlBytes


		// Scalar Types.
		//
		sealed class SqlBytesFromByteArray  : Convert<SqlBytes,Byte[]>     { public override SqlBytes From(Byte[] p)      { return p == null? SqlBytes.Null: new SqlBytes(p); } }
		sealed class SqlBytesFromStream     : Convert<SqlBytes,Stream>     { public override SqlBytes From(Stream p)      { return p == null? SqlBytes.Null: new SqlBytes(p); } }
		sealed class SqlBytesFromGuid       : Convert<SqlBytes,Guid>       { public override SqlBytes From(Guid p)        { return p == Guid.Empty? SqlBytes.Null: new SqlBytes(p.ToByteArray()); } }

		// Nullable Types.
		//
		sealed class SqlBytesFromNullableGuid       : Convert<SqlBytes,Guid?>       { public override SqlBytes From(Guid? p)        { return p.HasValue? new SqlBytes(p.Value.ToByteArray()): SqlBytes.Null; } }

		// SqlTypes.
		//
		sealed class SqlBytesFromSqlBinary  : Convert<SqlBytes,SqlBinary>  { public override SqlBytes From(SqlBinary p)   { return p.IsNull? SqlBytes.Null: new SqlBytes(p); } }
		sealed class SqlBytesFromSqlGuid    : Convert<SqlBytes,SqlGuid>    { public override SqlBytes From(SqlGuid p)     { return p.IsNull? SqlBytes.Null: new SqlBytes(p.ToByteArray()); } }

		sealed class SqlBytesDefault<Q>     : Convert<SqlBytes,Q>          { public override SqlBytes From(Q p)           { return Convert<SqlBytes,object>.Instance.From(p); } }
		sealed class SqlBytesFromObject     : Convert<SqlBytes,object>     { public override SqlBytes From(object p)     
			{
				if (p == null)
					return SqlBytes.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<SqlBytes,Byte[]>     .Instance.From((Byte[])p);
				if (p is Stream)      return Convert<SqlBytes,Stream>     .Instance.From((Stream)p);
				if (p is Guid)        return Convert<SqlBytes,Guid>       .Instance.From((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlBytes,Guid>       .Instance.From((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBinary)   return Convert<SqlBytes,SqlBinary>  .Instance.From((SqlBinary)p);
				if (p is SqlGuid)     return Convert<SqlBytes,SqlGuid>    .Instance.From((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetSqlBytesConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (Convert<T, P>)(object)(new SqlBytesFromByteArray());
			if (t == typeof(Stream))      return (Convert<T, P>)(object)(new SqlBytesFromStream());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new SqlBytesFromGuid());

			// Nullable Types.
			//
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new SqlBytesFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new SqlBytesFromSqlBinary());
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new SqlBytesFromSqlGuid());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlBytesFromObject());

			return (Convert<T, P>)(object)(new SqlBytesDefault<P>());
		}

		#endregion

		#region SqlChars


		// Scalar Types.
		//
		sealed class SqlCharsFromString     : Convert<SqlChars,String>     { public override SqlChars From(String p)      { return p == null? SqlChars.Null: new SqlChars(p.ToCharArray()); } }
		sealed class SqlCharsFromCharArray  : Convert<SqlChars,Char[]>     { public override SqlChars From(Char[] p)      { return p == null? SqlChars.Null: new SqlChars(p); } }

		sealed class SqlCharsFromSByte      : Convert<SqlChars,SByte>      { public override SqlChars From(SByte p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromInt16      : Convert<SqlChars,Int16>      { public override SqlChars From(Int16 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromInt32      : Convert<SqlChars,Int32>      { public override SqlChars From(Int32 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromInt64      : Convert<SqlChars,Int64>      { public override SqlChars From(Int64 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class SqlCharsFromByte       : Convert<SqlChars,Byte>       { public override SqlChars From(Byte p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromUInt16     : Convert<SqlChars,UInt16>     { public override SqlChars From(UInt16 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromUInt32     : Convert<SqlChars,UInt32>     { public override SqlChars From(UInt32 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromUInt64     : Convert<SqlChars,UInt64>     { public override SqlChars From(UInt64 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class SqlCharsFromSingle     : Convert<SqlChars,Single>     { public override SqlChars From(Single p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromDouble     : Convert<SqlChars,Double>     { public override SqlChars From(Double p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class SqlCharsFromBoolean    : Convert<SqlChars,Boolean>    { public override SqlChars From(Boolean p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromDecimal    : Convert<SqlChars,Decimal>    { public override SqlChars From(Decimal p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromChar       : Convert<SqlChars,Char>       { public override SqlChars From(Char p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromTimeSpan   : Convert<SqlChars,TimeSpan>   { public override SqlChars From(TimeSpan p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromDateTime   : Convert<SqlChars,DateTime>   { public override SqlChars From(DateTime p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class SqlCharsFromGuid       : Convert<SqlChars,Guid>       { public override SqlChars From(Guid p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		// Nullable Types.
		//
		sealed class SqlCharsFromNullableSByte      : Convert<SqlChars,SByte?>      { public override SqlChars From(SByte? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableInt16      : Convert<SqlChars,Int16?>      { public override SqlChars From(Int16? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableInt32      : Convert<SqlChars,Int32?>      { public override SqlChars From(Int32? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableInt64      : Convert<SqlChars,Int64?>      { public override SqlChars From(Int64? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class SqlCharsFromNullableByte       : Convert<SqlChars,Byte?>       { public override SqlChars From(Byte? p)        { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableUInt16     : Convert<SqlChars,UInt16?>     { public override SqlChars From(UInt16? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableUInt32     : Convert<SqlChars,UInt32?>     { public override SqlChars From(UInt32? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableUInt64     : Convert<SqlChars,UInt64?>     { public override SqlChars From(UInt64? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class SqlCharsFromNullableSingle     : Convert<SqlChars,Single?>     { public override SqlChars From(Single? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableDouble     : Convert<SqlChars,Double?>     { public override SqlChars From(Double? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class SqlCharsFromNullableBoolean    : Convert<SqlChars,Boolean?>    { public override SqlChars From(Boolean? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableDecimal    : Convert<SqlChars,Decimal?>    { public override SqlChars From(Decimal? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableChar       : Convert<SqlChars,Char?>       { public override SqlChars From(Char? p)        { return p.HasValue? new SqlChars(new Char[]{p.Value})       : SqlChars.Null; } }
		sealed class SqlCharsFromNullableTimeSpan   : Convert<SqlChars,TimeSpan?>   { public override SqlChars From(TimeSpan? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableDateTime   : Convert<SqlChars,DateTime?>   { public override SqlChars From(DateTime? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class SqlCharsFromNullableGuid       : Convert<SqlChars,Guid?>       { public override SqlChars From(Guid? p)        { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		// SqlTypes.
		//
		sealed class SqlCharsFromSqlString  : Convert<SqlChars,SqlString>  { public override SqlChars From(SqlString p)   { return (SqlChars)p; } }

		sealed class SqlCharsFromSqlByte    : Convert<SqlChars,SqlByte>    { public override SqlChars From(SqlByte p)     { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlInt16   : Convert<SqlChars,SqlInt16>   { public override SqlChars From(SqlInt16 p)    { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlInt32   : Convert<SqlChars,SqlInt32>   { public override SqlChars From(SqlInt32 p)    { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlInt64   : Convert<SqlChars,SqlInt64>   { public override SqlChars From(SqlInt64 p)    { return (SqlChars)p.ToSqlString(); } }

		sealed class SqlCharsFromSqlSingle  : Convert<SqlChars,SqlSingle>  { public override SqlChars From(SqlSingle p)   { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlDouble  : Convert<SqlChars,SqlDouble>  { public override SqlChars From(SqlDouble p)   { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlDecimal : Convert<SqlChars,SqlDecimal> { public override SqlChars From(SqlDecimal p)  { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlMoney   : Convert<SqlChars,SqlMoney>   { public override SqlChars From(SqlMoney p)    { return (SqlChars)p.ToSqlString(); } }

		sealed class SqlCharsFromSqlBoolean : Convert<SqlChars,SqlBoolean> { public override SqlChars From(SqlBoolean p)  { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlGuid    : Convert<SqlChars,SqlGuid>    { public override SqlChars From(SqlGuid p)     { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlDateTime: Convert<SqlChars,SqlDateTime>{ public override SqlChars From(SqlDateTime p) { return (SqlChars)p.ToSqlString(); } }
		sealed class SqlCharsFromSqlBinary  : Convert<SqlChars,SqlBinary>  { public override SqlChars From(SqlBinary p)   { return p.IsNull? SqlChars.Null: new SqlChars(p.ToString().ToCharArray()); } }
		sealed class SqlCharsFromType       : Convert<SqlChars,Type>       { public override SqlChars From(Type p)        { return p == null? SqlChars.Null: p.FullName.ToCharArray(); } }

		sealed class SqlCharsDefault<Q>     : Convert<SqlChars,Q>          { public override SqlChars From(Q p)           { return Convert<SqlChars,object>.Instance.From(p); } }
		sealed class SqlCharsFromObject     : Convert<SqlChars,object>     { public override SqlChars From(object p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		static Convert<T, P> GetSqlCharsConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (Convert<T, P>)(object)(new SqlCharsFromString());
			if (t == typeof(Char[]))      return (Convert<T, P>)(object)(new SqlCharsFromCharArray());

			if (t == typeof(SByte))       return (Convert<T, P>)(object)(new SqlCharsFromSByte());
			if (t == typeof(Int16))       return (Convert<T, P>)(object)(new SqlCharsFromInt16());
			if (t == typeof(Int32))       return (Convert<T, P>)(object)(new SqlCharsFromInt32());
			if (t == typeof(Int64))       return (Convert<T, P>)(object)(new SqlCharsFromInt64());

			if (t == typeof(Byte))        return (Convert<T, P>)(object)(new SqlCharsFromByte());
			if (t == typeof(UInt16))      return (Convert<T, P>)(object)(new SqlCharsFromUInt16());
			if (t == typeof(UInt32))      return (Convert<T, P>)(object)(new SqlCharsFromUInt32());
			if (t == typeof(UInt64))      return (Convert<T, P>)(object)(new SqlCharsFromUInt64());

			if (t == typeof(Single))      return (Convert<T, P>)(object)(new SqlCharsFromSingle());
			if (t == typeof(Double))      return (Convert<T, P>)(object)(new SqlCharsFromDouble());

			if (t == typeof(Boolean))     return (Convert<T, P>)(object)(new SqlCharsFromBoolean());
			if (t == typeof(Decimal))     return (Convert<T, P>)(object)(new SqlCharsFromDecimal());
			if (t == typeof(Char))        return (Convert<T, P>)(object)(new SqlCharsFromChar());
			if (t == typeof(TimeSpan))    return (Convert<T, P>)(object)(new SqlCharsFromTimeSpan());
			if (t == typeof(DateTime))    return (Convert<T, P>)(object)(new SqlCharsFromDateTime());
			if (t == typeof(Guid))        return (Convert<T, P>)(object)(new SqlCharsFromGuid());

			// Nullable Types.
			//
			if (t == typeof(SByte?))       return (Convert<T, P>)(object)(new SqlCharsFromNullableSByte());
			if (t == typeof(Int16?))       return (Convert<T, P>)(object)(new SqlCharsFromNullableInt16());
			if (t == typeof(Int32?))       return (Convert<T, P>)(object)(new SqlCharsFromNullableInt32());
			if (t == typeof(Int64?))       return (Convert<T, P>)(object)(new SqlCharsFromNullableInt64());

			if (t == typeof(Byte?))        return (Convert<T, P>)(object)(new SqlCharsFromNullableByte());
			if (t == typeof(UInt16?))      return (Convert<T, P>)(object)(new SqlCharsFromNullableUInt16());
			if (t == typeof(UInt32?))      return (Convert<T, P>)(object)(new SqlCharsFromNullableUInt32());
			if (t == typeof(UInt64?))      return (Convert<T, P>)(object)(new SqlCharsFromNullableUInt64());

			if (t == typeof(Single?))      return (Convert<T, P>)(object)(new SqlCharsFromNullableSingle());
			if (t == typeof(Double?))      return (Convert<T, P>)(object)(new SqlCharsFromNullableDouble());

			if (t == typeof(Boolean?))     return (Convert<T, P>)(object)(new SqlCharsFromNullableBoolean());
			if (t == typeof(Decimal?))     return (Convert<T, P>)(object)(new SqlCharsFromNullableDecimal());
			if (t == typeof(Char?))        return (Convert<T, P>)(object)(new SqlCharsFromNullableChar());
			if (t == typeof(TimeSpan?))    return (Convert<T, P>)(object)(new SqlCharsFromNullableTimeSpan());
			if (t == typeof(DateTime?))    return (Convert<T, P>)(object)(new SqlCharsFromNullableDateTime());
			if (t == typeof(Guid?))        return (Convert<T, P>)(object)(new SqlCharsFromNullableGuid());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlCharsFromSqlString());

			if (t == typeof(SqlByte))     return (Convert<T, P>)(object)(new SqlCharsFromSqlByte());
			if (t == typeof(SqlInt16))    return (Convert<T, P>)(object)(new SqlCharsFromSqlInt16());
			if (t == typeof(SqlInt32))    return (Convert<T, P>)(object)(new SqlCharsFromSqlInt32());
			if (t == typeof(SqlInt64))    return (Convert<T, P>)(object)(new SqlCharsFromSqlInt64());

			if (t == typeof(SqlSingle))   return (Convert<T, P>)(object)(new SqlCharsFromSqlSingle());
			if (t == typeof(SqlDouble))   return (Convert<T, P>)(object)(new SqlCharsFromSqlDouble());
			if (t == typeof(SqlDecimal))  return (Convert<T, P>)(object)(new SqlCharsFromSqlDecimal());
			if (t == typeof(SqlMoney))    return (Convert<T, P>)(object)(new SqlCharsFromSqlMoney());

			if (t == typeof(SqlBoolean))  return (Convert<T, P>)(object)(new SqlCharsFromSqlBoolean());
			if (t == typeof(SqlGuid))     return (Convert<T, P>)(object)(new SqlCharsFromSqlGuid());
			if (t == typeof(SqlDateTime)) return (Convert<T, P>)(object)(new SqlCharsFromSqlDateTime());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new SqlCharsFromSqlBinary());
			if (t == typeof(Type))        return (Convert<T, P>)(object)(new SqlCharsFromType());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlCharsFromObject());

			return (Convert<T, P>)(object)(new SqlCharsDefault<P>());
		}

		#endregion

		#region SqlXml


		// Scalar Types.
		//
		sealed class SqlXmlFromstring     : Convert<SqlXml,string>     { public override SqlXml From(string p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p))); } }

		sealed class SqlXmlFromStream     : Convert<SqlXml,Stream>     { public override SqlXml From(Stream p)      { return p == null? SqlXml.Null: new SqlXml(p); } }
		sealed class SqlXmlFromXmlReader  : Convert<SqlXml,XmlReader>  { public override SqlXml From(XmlReader p)   { return p == null? SqlXml.Null: new SqlXml(p); } }

		sealed class SqlXmlFromCharArray  : Convert<SqlXml,Char[]>     { public override SqlXml From(Char[] p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(new string(p)))); } }
		sealed class SqlXmlFromByteArray  : Convert<SqlXml,Byte[]>     { public override SqlXml From(Byte[] p)      { return p == null? SqlXml.Null: new SqlXml(new MemoryStream(p)); } }

		// SqlTypes.
		//
		sealed class SqlXmlFromSqlString  : Convert<SqlXml,SqlString>  { public override SqlXml From(SqlString p)   { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.Value))); } }
		sealed class SqlXmlFromSqlChars   : Convert<SqlXml,SqlChars>   { public override SqlXml From(SqlChars p)    { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.ToSqlString().Value))); } }
		sealed class SqlXmlFromSqlBinary  : Convert<SqlXml,SqlBinary>  { public override SqlXml From(SqlBinary p)   { return p.IsNull? SqlXml.Null: new SqlXml(new MemoryStream(p.Value)); } }
		sealed class SqlXmlFromSqlBytes   : Convert<SqlXml,SqlBytes>   { public override SqlXml From(SqlBytes p)    { return p.IsNull? SqlXml.Null: new SqlXml(p.Stream); } }

		sealed class SqlXmlDefault<Q>     : Convert<SqlXml,Q>          { public override SqlXml From(Q p)           { return Convert<SqlXml,object>.Instance.From(p); } }
		sealed class SqlXmlFromObject     : Convert<SqlXml,object>     { public override SqlXml From(object p)     
			{
				if (p == null)
					return SqlXml.Null;

				// Scalar Types.
				//
				if (p is string)      return Convert<SqlXml,string>     .Instance.From((string)p);

				if (p is Stream)      return Convert<SqlXml,Stream>     .Instance.From((Stream)p);
				if (p is XmlReader)   return Convert<SqlXml,XmlReader>  .Instance.From((XmlReader)p);

				if (p is Char[])      return Convert<SqlXml,Char[]>     .Instance.From((Char[])p);
				if (p is Byte[])      return Convert<SqlXml,Byte[]>     .Instance.From((Byte[])p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<SqlXml,SqlString>  .Instance.From((SqlString)p);
				if (p is SqlChars)    return Convert<SqlXml,SqlChars>   .Instance.From((SqlChars)p);
				if (p is SqlBinary)   return Convert<SqlXml,SqlBinary>  .Instance.From((SqlBinary)p);
				if (p is SqlBytes)    return Convert<SqlXml,SqlBytes>   .Instance.From((SqlBytes)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static Convert<T, P> GetSqlXmlConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(string))      return (Convert<T, P>)(object)(new SqlXmlFromstring());

			if (t == typeof(Stream))      return (Convert<T, P>)(object)(new SqlXmlFromStream());
			if (t == typeof(XmlReader))   return (Convert<T, P>)(object)(new SqlXmlFromXmlReader());

			if (t == typeof(Char[]))      return (Convert<T, P>)(object)(new SqlXmlFromCharArray());
			if (t == typeof(Byte[]))      return (Convert<T, P>)(object)(new SqlXmlFromByteArray());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (Convert<T, P>)(object)(new SqlXmlFromSqlString());
			if (t == typeof(SqlChars))    return (Convert<T, P>)(object)(new SqlXmlFromSqlChars());
			if (t == typeof(SqlBinary))   return (Convert<T, P>)(object)(new SqlXmlFromSqlBinary());
			if (t == typeof(SqlBytes))    return (Convert<T, P>)(object)(new SqlXmlFromSqlBytes());

			if (t == typeof(object))      return (Convert<T, P>)(object)(new SqlXmlFromObject());

			return (Convert<T, P>)(object)(new SqlXmlDefault<P>());
		}

		#endregion

		#endregion

	}

	public static class ConvertTo<T>
	{
		public static T From<P>(P p)
		{
			return Convert<T,P>.Instance.From(p);
		}
	}
}
