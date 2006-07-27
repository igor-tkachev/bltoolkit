using System;
using System.Data.SqlTypes;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		#region Nullable Types

		#region SByte?


		// Scalar Types.
		//
		sealed class NI8_I8      : CB<SByte?,SByte>      { public override SByte? C(SByte p)       { return p; } }
		sealed class NI8_S       : CB<SByte?,String>     { public override SByte? C(String p)      { return p == null? null: (SByte?)Convert.ToSByte(p); } }

		sealed class NI8_I16     : CB<SByte?,Int16>      { public override SByte? C(Int16 p)       { return Convert.ToSByte(p); } }
		sealed class NI8_I32     : CB<SByte?,Int32>      { public override SByte? C(Int32 p)       { return Convert.ToSByte(p); } }
		sealed class NI8_I64     : CB<SByte?,Int64>      { public override SByte? C(Int64 p)       { return Convert.ToSByte(p); } }

		sealed class NI8_U8      : CB<SByte?,Byte>       { public override SByte? C(Byte p)        { return Convert.ToSByte(p); } }
		sealed class NI8_U16     : CB<SByte?,UInt16>     { public override SByte? C(UInt16 p)      { return Convert.ToSByte(p); } }
		sealed class NI8_U32     : CB<SByte?,UInt32>     { public override SByte? C(UInt32 p)      { return Convert.ToSByte(p); } }
		sealed class NI8_U64     : CB<SByte?,UInt64>     { public override SByte? C(UInt64 p)      { return Convert.ToSByte(p); } }

		sealed class NI8_R4      : CB<SByte?,Single>     { public override SByte? C(Single p)      { return Convert.ToSByte(p); } }
		sealed class NI8_R8      : CB<SByte?,Double>     { public override SByte? C(Double p)      { return Convert.ToSByte(p); } }

		sealed class NI8_B       : CB<SByte?,Boolean>    { public override SByte? C(Boolean p)     { return Convert.ToSByte(p); } }
		sealed class NI8_D       : CB<SByte?,Decimal>    { public override SByte? C(Decimal p)     { return Convert.ToSByte(p); } }
		sealed class NI8_C       : CB<SByte?,Char>       { public override SByte? C(Char p)        { return Convert.ToSByte(p); } }

		// Nullable Types.
		//
		sealed class NI8_NI16    : CB<SByte?,Int16?>     { public override SByte? C(Int16? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NI32    : CB<SByte?,Int32?>     { public override SByte? C(Int32? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NI64    : CB<SByte?,Int64?>     { public override SByte? C(Int64? p)      { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NI8_NU8     : CB<SByte?,Byte?>      { public override SByte? C(Byte? p)       { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NU16    : CB<SByte?,UInt16?>    { public override SByte? C(UInt16? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NU32    : CB<SByte?,UInt32?>    { public override SByte? C(UInt32? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NU64    : CB<SByte?,UInt64?>    { public override SByte? C(UInt64? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NI8_NR4     : CB<SByte?,Single?>    { public override SByte? C(Single? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NR8     : CB<SByte?,Double?>    { public override SByte? C(Double? p)     { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		sealed class NI8_NB      : CB<SByte?,Boolean?>   { public override SByte? C(Boolean? p)    { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_ND      : CB<SByte?,Decimal?>   { public override SByte? C(Decimal? p)    { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }
		sealed class NI8_NC      : CB<SByte?,Char?>      { public override SByte? C(Char? p)       { return p.HasValue? (SByte?)Convert.ToSByte(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NI8_dbS     : CB<SByte?,SqlString>  { public override SByte? C(SqlString p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NI8_dbU8    : CB<SByte?,SqlByte>    { public override SByte? C(SqlByte p)     { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbI16   : CB<SByte?,SqlInt16>   { public override SByte? C(SqlInt16 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbI32   : CB<SByte?,SqlInt32>   { public override SByte? C(SqlInt32 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbI64   : CB<SByte?,SqlInt64>   { public override SByte? C(SqlInt64 p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NI8_dbR4    : CB<SByte?,SqlSingle>  { public override SByte? C(SqlSingle p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbR8    : CB<SByte?,SqlDouble>  { public override SByte? C(SqlDouble p)   { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbD     : CB<SByte?,SqlDecimal> { public override SByte? C(SqlDecimal p)  { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }
		sealed class NI8_dbM     : CB<SByte?,SqlMoney>   { public override SByte? C(SqlMoney p)    { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NI8_dbB     : CB<SByte?,SqlBoolean> { public override SByte? C(SqlBoolean p)  { return p.IsNull? null: (SByte?)Convert.ToSByte(p.Value); } }

		sealed class NI8<Q>      : CB<SByte?,Q>          { public override SByte? C(Q p)           { return Convert<SByte?,object>.From(p); } }
		sealed class NI8_O       : CB<SByte?,object>     { public override SByte? C(object p)      { return Convert.ToSByte(p); } }

		static CB<T,P> GetNullableSByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NI8_I8      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NI8_S       ());

			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NI8_I16     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NI8_I32     ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NI8_I64     ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NI8_U8      ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NI8_U16     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NI8_U32     ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NI8_U64     ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NI8_R4      ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NI8_R8      ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NI8_B       ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NI8_D       ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NI8_C       ());

			// Nullable Types.
			//
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NI8_NI16    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NI8_NI32    ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NI8_NI64    ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NI8_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NI8_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NI8_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NI8_NU64    ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NI8_NR4     ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NI8_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NI8_NB      ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NI8_ND      ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NI8_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NI8_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NI8_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NI8_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NI8_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NI8_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NI8_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NI8_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NI8_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NI8_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NI8_dbB     ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NI8_O       ());

			return (CB<T,P>)(object)(new NI8<P>());
		}

		#endregion

		#region Int16?


		// Scalar Types.
		//
		sealed class NI16_I16    : CB<Int16?,Int16>      { public override Int16? C(Int16 p)       { return p; } }
		sealed class NI16_S      : CB<Int16?,String>     { public override Int16? C(String p)      { return p == null? null: (Int16?)Convert.ToInt16(p); } }

		sealed class NI16_I8     : CB<Int16?,SByte>      { public override Int16? C(SByte p)       { return Convert.ToInt16(p); } }
		sealed class NI16_I32    : CB<Int16?,Int32>      { public override Int16? C(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class NI16_I64    : CB<Int16?,Int64>      { public override Int16? C(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class NI16_U8     : CB<Int16?,Byte>       { public override Int16? C(Byte p)        { return Convert.ToInt16(p); } }
		sealed class NI16_U16    : CB<Int16?,UInt16>     { public override Int16? C(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class NI16_U32    : CB<Int16?,UInt32>     { public override Int16? C(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class NI16_U64    : CB<Int16?,UInt64>     { public override Int16? C(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class NI16_R4     : CB<Int16?,Single>     { public override Int16? C(Single p)      { return Convert.ToInt16(p); } }
		sealed class NI16_R8     : CB<Int16?,Double>     { public override Int16? C(Double p)      { return Convert.ToInt16(p); } }

		sealed class NI16_B      : CB<Int16?,Boolean>    { public override Int16? C(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class NI16_D      : CB<Int16?,Decimal>    { public override Int16? C(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class NI16_C      : CB<Int16?,Char>       { public override Int16? C(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class NI16_NI8    : CB<Int16?,SByte?>     { public override Int16? C(SByte? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NI32   : CB<Int16?,Int32?>     { public override Int16? C(Int32? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NI64   : CB<Int16?,Int64?>     { public override Int16? C(Int64? p)      { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NI16_NU8    : CB<Int16?,Byte?>      { public override Int16? C(Byte? p)       { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NU16   : CB<Int16?,UInt16?>    { public override Int16? C(UInt16? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NU32   : CB<Int16?,UInt32?>    { public override Int16? C(UInt32? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NU64   : CB<Int16?,UInt64?>    { public override Int16? C(UInt64? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NI16_NR4    : CB<Int16?,Single?>    { public override Int16? C(Single? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NR8    : CB<Int16?,Double?>    { public override Int16? C(Double? p)     { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		sealed class NI16_NB     : CB<Int16?,Boolean?>   { public override Int16? C(Boolean? p)    { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_ND     : CB<Int16?,Decimal?>   { public override Int16? C(Decimal? p)    { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }
		sealed class NI16_NC     : CB<Int16?,Char?>      { public override Int16? C(Char? p)       { return p.HasValue? (Int16?)Convert.ToInt16(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NI16_dbI16  : CB<Int16?,SqlInt16>   { public override Int16? C(SqlInt16 p)    { return p.IsNull? null: (Int16?)                p.Value;  } }
		sealed class NI16_dbS    : CB<Int16?,SqlString>  { public override Int16? C(SqlString p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NI16_dbU8   : CB<Int16?,SqlByte>    { public override Int16? C(SqlByte p)     { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NI16_dbI32  : CB<Int16?,SqlInt32>   { public override Int16? C(SqlInt32 p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NI16_dbI64  : CB<Int16?,SqlInt64>   { public override Int16? C(SqlInt64 p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NI16_dbR4   : CB<Int16?,SqlSingle>  { public override Int16? C(SqlSingle p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NI16_dbR8   : CB<Int16?,SqlDouble>  { public override Int16? C(SqlDouble p)   { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NI16_dbD    : CB<Int16?,SqlDecimal> { public override Int16? C(SqlDecimal p)  { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }
		sealed class NI16_dbM    : CB<Int16?,SqlMoney>   { public override Int16? C(SqlMoney p)    { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NI16_dbB    : CB<Int16?,SqlBoolean> { public override Int16? C(SqlBoolean p)  { return p.IsNull? null: (Int16?)Convert.ToInt16(p.Value); } }

		sealed class NI16<Q>     : CB<Int16?,Q>          { public override Int16? C(Q p)           { return Convert<Int16?,object>.From(p); } }
		sealed class NI16_O      : CB<Int16?,object>     { public override Int16? C(object p)      { return Convert.ToInt16(p); } }

		static CB<T,P> GetNullableInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NI16_I16    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NI16_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NI16_I8     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NI16_I32    ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NI16_I64    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NI16_U8     ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NI16_U16    ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NI16_U32    ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NI16_U64    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NI16_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NI16_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NI16_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NI16_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NI16_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NI16_NI8    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NI16_NI32   ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NI16_NI64   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NI16_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NI16_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NI16_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NI16_NU64   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NI16_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NI16_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NI16_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NI16_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NI16_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NI16_dbI16  ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NI16_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NI16_dbU8   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NI16_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NI16_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NI16_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NI16_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NI16_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NI16_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NI16_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NI16_O      ());

			return (CB<T,P>)(object)(new NI16<P>());
		}

		#endregion

		#region Int32?


		// Scalar Types.
		//
		sealed class NI32_I32    : CB<Int32?,Int32>      { public override Int32? C(Int32 p)       { return p; } }
		sealed class NI32_S      : CB<Int32?,String>     { public override Int32? C(String p)      { return p == null? null: (Int32?)Convert.ToInt32(p); } }

		sealed class NI32_I8     : CB<Int32?,SByte>      { public override Int32? C(SByte p)       { return Convert.ToInt32(p); } }
		sealed class NI32_I16    : CB<Int32?,Int16>      { public override Int32? C(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class NI32_I64    : CB<Int32?,Int64>      { public override Int32? C(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class NI32_U8     : CB<Int32?,Byte>       { public override Int32? C(Byte p)        { return Convert.ToInt32(p); } }
		sealed class NI32_U16    : CB<Int32?,UInt16>     { public override Int32? C(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class NI32_U32    : CB<Int32?,UInt32>     { public override Int32? C(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class NI32_U64    : CB<Int32?,UInt64>     { public override Int32? C(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class NI32_R4     : CB<Int32?,Single>     { public override Int32? C(Single p)      { return Convert.ToInt32(p); } }
		sealed class NI32_R8     : CB<Int32?,Double>     { public override Int32? C(Double p)      { return Convert.ToInt32(p); } }

		sealed class NI32_B      : CB<Int32?,Boolean>    { public override Int32? C(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class NI32_D      : CB<Int32?,Decimal>    { public override Int32? C(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class NI32_C      : CB<Int32?,Char>       { public override Int32? C(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class NI32_NI8    : CB<Int32?,SByte?>     { public override Int32? C(SByte? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NI16   : CB<Int32?,Int16?>     { public override Int32? C(Int16? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NI64   : CB<Int32?,Int64?>     { public override Int32? C(Int64? p)      { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NI32_NU8    : CB<Int32?,Byte?>      { public override Int32? C(Byte? p)       { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NU16   : CB<Int32?,UInt16?>    { public override Int32? C(UInt16? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NU32   : CB<Int32?,UInt32?>    { public override Int32? C(UInt32? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NU64   : CB<Int32?,UInt64?>    { public override Int32? C(UInt64? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NI32_NR4    : CB<Int32?,Single?>    { public override Int32? C(Single? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NR8    : CB<Int32?,Double?>    { public override Int32? C(Double? p)     { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		sealed class NI32_NB     : CB<Int32?,Boolean?>   { public override Int32? C(Boolean? p)    { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_ND     : CB<Int32?,Decimal?>   { public override Int32? C(Decimal? p)    { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }
		sealed class NI32_NC     : CB<Int32?,Char?>      { public override Int32? C(Char? p)       { return p.HasValue? (Int32?)Convert.ToInt32(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NI32_dbI32  : CB<Int32?,SqlInt32>   { public override Int32? C(SqlInt32 p)    { return p.IsNull? null: (Int32?)                p.Value;  } }
		sealed class NI32_dbS    : CB<Int32?,SqlString>  { public override Int32? C(SqlString p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NI32_dbU8   : CB<Int32?,SqlByte>    { public override Int32? C(SqlByte p)     { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NI32_dbI16  : CB<Int32?,SqlInt16>   { public override Int32? C(SqlInt16 p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NI32_dbI64  : CB<Int32?,SqlInt64>   { public override Int32? C(SqlInt64 p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NI32_dbR4   : CB<Int32?,SqlSingle>  { public override Int32? C(SqlSingle p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NI32_dbR8   : CB<Int32?,SqlDouble>  { public override Int32? C(SqlDouble p)   { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NI32_dbD    : CB<Int32?,SqlDecimal> { public override Int32? C(SqlDecimal p)  { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }
		sealed class NI32_dbM    : CB<Int32?,SqlMoney>   { public override Int32? C(SqlMoney p)    { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NI32_dbB    : CB<Int32?,SqlBoolean> { public override Int32? C(SqlBoolean p)  { return p.IsNull? null: (Int32?)Convert.ToInt32(p.Value); } }

		sealed class NI32<Q>     : CB<Int32?,Q>          { public override Int32? C(Q p)           { return Convert<Int32?,object>.From(p); } }
		sealed class NI32_O      : CB<Int32?,object>     { public override Int32? C(object p)      { return Convert.ToInt32(p); } }

		static CB<T,P> GetNullableInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NI32_I32    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NI32_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NI32_I8     ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NI32_I16    ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NI32_I64    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NI32_U8     ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NI32_U16    ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NI32_U32    ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NI32_U64    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NI32_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NI32_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NI32_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NI32_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NI32_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NI32_NI8    ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NI32_NI16   ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NI32_NI64   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NI32_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NI32_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NI32_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NI32_NU64   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NI32_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NI32_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NI32_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NI32_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NI32_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NI32_dbI32  ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NI32_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NI32_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NI32_dbI16  ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NI32_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NI32_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NI32_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NI32_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NI32_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NI32_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NI32_O      ());

			return (CB<T,P>)(object)(new NI32<P>());
		}

		#endregion

		#region Int64?


		// Scalar Types.
		//
		sealed class NI64_I64    : CB<Int64?,Int64>      { public override Int64? C(Int64 p)       { return p; } }
		sealed class NI64_S      : CB<Int64?,String>     { public override Int64? C(String p)      { return p == null? null: (Int64?)Convert.ToInt64(p); } }

		sealed class NI64_I8     : CB<Int64?,SByte>      { public override Int64? C(SByte p)       { return Convert.ToInt64(p); } }
		sealed class NI64_I16    : CB<Int64?,Int16>      { public override Int64? C(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class NI64_I32    : CB<Int64?,Int32>      { public override Int64? C(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class NI64_U8     : CB<Int64?,Byte>       { public override Int64? C(Byte p)        { return Convert.ToInt64(p); } }
		sealed class NI64_U16    : CB<Int64?,UInt16>     { public override Int64? C(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class NI64_U32    : CB<Int64?,UInt32>     { public override Int64? C(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class NI64_U64    : CB<Int64?,UInt64>     { public override Int64? C(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class NI64_R4     : CB<Int64?,Single>     { public override Int64? C(Single p)      { return Convert.ToInt64(p); } }
		sealed class NI64_R8     : CB<Int64?,Double>     { public override Int64? C(Double p)      { return Convert.ToInt64(p); } }

		sealed class NI64_B      : CB<Int64?,Boolean>    { public override Int64? C(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class NI64_D      : CB<Int64?,Decimal>    { public override Int64? C(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class NI64_C      : CB<Int64?,Char>       { public override Int64? C(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class NI64_NI8    : CB<Int64?,SByte?>     { public override Int64? C(SByte? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NI16   : CB<Int64?,Int16?>     { public override Int64? C(Int16? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NI32   : CB<Int64?,Int32?>     { public override Int64? C(Int32? p)      { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NI64_NU8    : CB<Int64?,Byte?>      { public override Int64? C(Byte? p)       { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NU16   : CB<Int64?,UInt16?>    { public override Int64? C(UInt16? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NU32   : CB<Int64?,UInt32?>    { public override Int64? C(UInt32? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NU64   : CB<Int64?,UInt64?>    { public override Int64? C(UInt64? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NI64_NR4    : CB<Int64?,Single?>    { public override Int64? C(Single? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NR8    : CB<Int64?,Double?>    { public override Int64? C(Double? p)     { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		sealed class NI64_NB     : CB<Int64?,Boolean?>   { public override Int64? C(Boolean? p)    { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_ND     : CB<Int64?,Decimal?>   { public override Int64? C(Decimal? p)    { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }
		sealed class NI64_NC     : CB<Int64?,Char?>      { public override Int64? C(Char? p)       { return p.HasValue? (Int64?)Convert.ToInt64(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NI64_dbI64  : CB<Int64?,SqlInt64>   { public override Int64? C(SqlInt64 p)    { return p.IsNull? null: (Int64?)                p.Value;  } }
		sealed class NI64_dbS    : CB<Int64?,SqlString>  { public override Int64? C(SqlString p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NI64_dbU8   : CB<Int64?,SqlByte>    { public override Int64? C(SqlByte p)     { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NI64_dbI16  : CB<Int64?,SqlInt16>   { public override Int64? C(SqlInt16 p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NI64_dbI32  : CB<Int64?,SqlInt32>   { public override Int64? C(SqlInt32 p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NI64_dbR4   : CB<Int64?,SqlSingle>  { public override Int64? C(SqlSingle p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NI64_dbR8   : CB<Int64?,SqlDouble>  { public override Int64? C(SqlDouble p)   { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NI64_dbD    : CB<Int64?,SqlDecimal> { public override Int64? C(SqlDecimal p)  { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }
		sealed class NI64_dbM    : CB<Int64?,SqlMoney>   { public override Int64? C(SqlMoney p)    { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NI64_dbB    : CB<Int64?,SqlBoolean> { public override Int64? C(SqlBoolean p)  { return p.IsNull? null: (Int64?)Convert.ToInt64(p.Value); } }

		sealed class NI64<Q>     : CB<Int64?,Q>          { public override Int64? C(Q p)           { return Convert<Int64?,object>.From(p); } }
		sealed class NI64_O      : CB<Int64?,object>     { public override Int64? C(object p)      { return Convert.ToInt64(p); } }

		static CB<T,P> GetNullableInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NI64_I64    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NI64_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NI64_I8     ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NI64_I16    ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NI64_I32    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NI64_U8     ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NI64_U16    ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NI64_U32    ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NI64_U64    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NI64_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NI64_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NI64_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NI64_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NI64_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NI64_NI8    ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NI64_NI16   ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NI64_NI32   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NI64_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NI64_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NI64_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NI64_NU64   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NI64_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NI64_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NI64_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NI64_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NI64_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NI64_dbI64  ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NI64_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NI64_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NI64_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NI64_dbI32  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NI64_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NI64_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NI64_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NI64_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NI64_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NI64_O      ());

			return (CB<T,P>)(object)(new NI64<P>());
		}

		#endregion

		#region Byte?


		// Scalar Types.
		//
		sealed class NU8_U8      : CB<Byte?,Byte>       { public override Byte? C(Byte p)        { return p; } }
		sealed class NU8_S       : CB<Byte?,String>     { public override Byte? C(String p)      { return p == null? null: (Byte?)Convert.ToByte(p); } }

		sealed class NU8_I8      : CB<Byte?,SByte>      { public override Byte? C(SByte p)       { return Convert.ToByte(p); } }
		sealed class NU8_I16     : CB<Byte?,Int16>      { public override Byte? C(Int16 p)       { return Convert.ToByte(p); } }
		sealed class NU8_I32     : CB<Byte?,Int32>      { public override Byte? C(Int32 p)       { return Convert.ToByte(p); } }
		sealed class NU8_I64     : CB<Byte?,Int64>      { public override Byte? C(Int64 p)       { return Convert.ToByte(p); } }

		sealed class NU8_U16     : CB<Byte?,UInt16>     { public override Byte? C(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class NU8_U32     : CB<Byte?,UInt32>     { public override Byte? C(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class NU8_U64     : CB<Byte?,UInt64>     { public override Byte? C(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class NU8_R4      : CB<Byte?,Single>     { public override Byte? C(Single p)      { return Convert.ToByte(p); } }
		sealed class NU8_R8      : CB<Byte?,Double>     { public override Byte? C(Double p)      { return Convert.ToByte(p); } }

		sealed class NU8_B       : CB<Byte?,Boolean>    { public override Byte? C(Boolean p)     { return Convert.ToByte(p); } }
		sealed class NU8_D       : CB<Byte?,Decimal>    { public override Byte? C(Decimal p)     { return Convert.ToByte(p); } }
		sealed class NU8_C       : CB<Byte?,Char>       { public override Byte? C(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class NU8_NI8     : CB<Byte?,SByte?>     { public override Byte? C(SByte? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NI16    : CB<Byte?,Int16?>     { public override Byte? C(Int16? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NI32    : CB<Byte?,Int32?>     { public override Byte? C(Int32? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NI64    : CB<Byte?,Int64?>     { public override Byte? C(Int64? p)      { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NU8_NU16    : CB<Byte?,UInt16?>    { public override Byte? C(UInt16? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NU32    : CB<Byte?,UInt32?>    { public override Byte? C(UInt32? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NU64    : CB<Byte?,UInt64?>    { public override Byte? C(UInt64? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NU8_NR4     : CB<Byte?,Single?>    { public override Byte? C(Single? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NR8     : CB<Byte?,Double?>    { public override Byte? C(Double? p)     { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		sealed class NU8_NB      : CB<Byte?,Boolean?>   { public override Byte? C(Boolean? p)    { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_ND      : CB<Byte?,Decimal?>   { public override Byte? C(Decimal? p)    { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }
		sealed class NU8_NC      : CB<Byte?,Char?>      { public override Byte? C(Char? p)       { return p.HasValue? (Byte?)Convert.ToByte(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NU8_dbU8    : CB<Byte?,SqlByte>    { public override Byte? C(SqlByte p)     { return p.IsNull? null: (Byte?)               p.Value;  } }
		sealed class NU8_dbS     : CB<Byte?,SqlString>  { public override Byte? C(SqlString p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NU8_dbI16   : CB<Byte?,SqlInt16>   { public override Byte? C(SqlInt16 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NU8_dbI32   : CB<Byte?,SqlInt32>   { public override Byte? C(SqlInt32 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NU8_dbI64   : CB<Byte?,SqlInt64>   { public override Byte? C(SqlInt64 p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NU8_dbR4    : CB<Byte?,SqlSingle>  { public override Byte? C(SqlSingle p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NU8_dbR8    : CB<Byte?,SqlDouble>  { public override Byte? C(SqlDouble p)   { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NU8_dbD     : CB<Byte?,SqlDecimal> { public override Byte? C(SqlDecimal p)  { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }
		sealed class NU8_dbM     : CB<Byte?,SqlMoney>   { public override Byte? C(SqlMoney p)    { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NU8_dbB     : CB<Byte?,SqlBoolean> { public override Byte? C(SqlBoolean p)  { return p.IsNull? null: (Byte?)Convert.ToByte(p.Value); } }

		sealed class NU8<Q>      : CB<Byte?,Q>          { public override Byte? C(Q p)           { return Convert<Byte?,object>.From(p); } }
		sealed class NU8_O       : CB<Byte?,object>     { public override Byte? C(object p)      { return Convert.ToByte(p); } }

		static CB<T,P> GetNullableByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NU8_U8      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NU8_S       ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NU8_I8      ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NU8_I16     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NU8_I32     ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NU8_I64     ());

			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NU8_U16     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NU8_U32     ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NU8_U64     ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NU8_R4      ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NU8_R8      ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NU8_B       ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NU8_D       ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NU8_C       ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NU8_NI8     ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NU8_NI16    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NU8_NI32    ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NU8_NI64    ());

			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NU8_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NU8_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NU8_NU64    ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NU8_NR4     ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NU8_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NU8_NB      ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NU8_ND      ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NU8_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NU8_dbU8    ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NU8_dbS     ());

			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NU8_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NU8_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NU8_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NU8_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NU8_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NU8_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NU8_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NU8_dbB     ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NU8_O       ());

			return (CB<T,P>)(object)(new NU8<P>());
		}

		#endregion

		#region UInt16?


		// Scalar Types.
		//
		sealed class NU16_U16    : CB<UInt16?,UInt16>     { public override UInt16? C(UInt16 p)      { return p; } }
		sealed class NU16_S      : CB<UInt16?,String>     { public override UInt16? C(String p)      { return p == null? null: (UInt16?)Convert.ToUInt16(p); } }

		sealed class NU16_I8     : CB<UInt16?,SByte>      { public override UInt16? C(SByte p)       { return Convert.ToUInt16(p); } }
		sealed class NU16_I16    : CB<UInt16?,Int16>      { public override UInt16? C(Int16 p)       { return Convert.ToUInt16(p); } }
		sealed class NU16_I32    : CB<UInt16?,Int32>      { public override UInt16? C(Int32 p)       { return Convert.ToUInt16(p); } }
		sealed class NU16_I64    : CB<UInt16?,Int64>      { public override UInt16? C(Int64 p)       { return Convert.ToUInt16(p); } }

		sealed class NU16_U8     : CB<UInt16?,Byte>       { public override UInt16? C(Byte p)        { return Convert.ToUInt16(p); } }
		sealed class NU16_U32    : CB<UInt16?,UInt32>     { public override UInt16? C(UInt32 p)      { return Convert.ToUInt16(p); } }
		sealed class NU16_U64    : CB<UInt16?,UInt64>     { public override UInt16? C(UInt64 p)      { return Convert.ToUInt16(p); } }

		sealed class NU16_R4     : CB<UInt16?,Single>     { public override UInt16? C(Single p)      { return Convert.ToUInt16(p); } }
		sealed class NU16_R8     : CB<UInt16?,Double>     { public override UInt16? C(Double p)      { return Convert.ToUInt16(p); } }

		sealed class NU16_B      : CB<UInt16?,Boolean>    { public override UInt16? C(Boolean p)     { return Convert.ToUInt16(p); } }
		sealed class NU16_D      : CB<UInt16?,Decimal>    { public override UInt16? C(Decimal p)     { return Convert.ToUInt16(p); } }
		sealed class NU16_C      : CB<UInt16?,Char>       { public override UInt16? C(Char p)        { return Convert.ToUInt16(p); } }

		// Nullable Types.
		//
		sealed class NU16_NI8    : CB<UInt16?,SByte?>     { public override UInt16? C(SByte? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NI16   : CB<UInt16?,Int16?>     { public override UInt16? C(Int16? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NI32   : CB<UInt16?,Int32?>     { public override UInt16? C(Int32? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NI64   : CB<UInt16?,Int64?>     { public override UInt16? C(Int64? p)      { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NU16_NU8    : CB<UInt16?,Byte?>      { public override UInt16? C(Byte? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NU32   : CB<UInt16?,UInt32?>    { public override UInt16? C(UInt32? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NU64   : CB<UInt16?,UInt64?>    { public override UInt16? C(UInt64? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NU16_NR4    : CB<UInt16?,Single?>    { public override UInt16? C(Single? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NR8    : CB<UInt16?,Double?>    { public override UInt16? C(Double? p)     { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		sealed class NU16_NB     : CB<UInt16?,Boolean?>   { public override UInt16? C(Boolean? p)    { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_ND     : CB<UInt16?,Decimal?>   { public override UInt16? C(Decimal? p)    { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }
		sealed class NU16_NC     : CB<UInt16?,Char?>      { public override UInt16? C(Char? p)       { return p.HasValue? (UInt16?)Convert.ToUInt16(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NU16_dbS    : CB<UInt16?,SqlString>  { public override UInt16? C(SqlString p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NU16_dbU8   : CB<UInt16?,SqlByte>    { public override UInt16? C(SqlByte p)     { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbI16  : CB<UInt16?,SqlInt16>   { public override UInt16? C(SqlInt16 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbI32  : CB<UInt16?,SqlInt32>   { public override UInt16? C(SqlInt32 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbI64  : CB<UInt16?,SqlInt64>   { public override UInt16? C(SqlInt64 p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NU16_dbR4   : CB<UInt16?,SqlSingle>  { public override UInt16? C(SqlSingle p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbR8   : CB<UInt16?,SqlDouble>  { public override UInt16? C(SqlDouble p)   { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbD    : CB<UInt16?,SqlDecimal> { public override UInt16? C(SqlDecimal p)  { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }
		sealed class NU16_dbM    : CB<UInt16?,SqlMoney>   { public override UInt16? C(SqlMoney p)    { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NU16_dbB    : CB<UInt16?,SqlBoolean> { public override UInt16? C(SqlBoolean p)  { return p.IsNull? null: (UInt16?)Convert.ToUInt16(p.Value); } }

		sealed class NU16<Q>     : CB<UInt16?,Q>          { public override UInt16? C(Q p)           { return Convert<UInt16?,object>.From(p); } }
		sealed class NU16_O      : CB<UInt16?,object>     { public override UInt16? C(object p)      { return Convert.ToUInt16(p); } }

		static CB<T,P> GetNullableUInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NU16_U16    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NU16_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NU16_I8     ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NU16_I16    ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NU16_I32    ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NU16_I64    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NU16_U8     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NU16_U32    ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NU16_U64    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NU16_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NU16_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NU16_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NU16_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NU16_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NU16_NI8    ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NU16_NI16   ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NU16_NI32   ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NU16_NI64   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NU16_NU8    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NU16_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NU16_NU64   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NU16_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NU16_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NU16_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NU16_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NU16_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NU16_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NU16_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NU16_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NU16_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NU16_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NU16_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NU16_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NU16_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NU16_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NU16_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NU16_O      ());

			return (CB<T,P>)(object)(new NU16<P>());
		}

		#endregion

		#region UInt32?


		// Scalar Types.
		//
		sealed class NU32_U32    : CB<UInt32?,UInt32>     { public override UInt32? C(UInt32 p)      { return p; } }
		sealed class NU32_S      : CB<UInt32?,String>     { public override UInt32? C(String p)      { return p == null? null: (UInt32?)Convert.ToUInt32(p); } }

		sealed class NU32_I8     : CB<UInt32?,SByte>      { public override UInt32? C(SByte p)       { return Convert.ToUInt32(p); } }
		sealed class NU32_I16    : CB<UInt32?,Int16>      { public override UInt32? C(Int16 p)       { return Convert.ToUInt32(p); } }
		sealed class NU32_I32    : CB<UInt32?,Int32>      { public override UInt32? C(Int32 p)       { return Convert.ToUInt32(p); } }
		sealed class NU32_I64    : CB<UInt32?,Int64>      { public override UInt32? C(Int64 p)       { return Convert.ToUInt32(p); } }

		sealed class NU32_U8     : CB<UInt32?,Byte>       { public override UInt32? C(Byte p)        { return Convert.ToUInt32(p); } }
		sealed class NU32_U16    : CB<UInt32?,UInt16>     { public override UInt32? C(UInt16 p)      { return Convert.ToUInt32(p); } }
		sealed class NU32_U64    : CB<UInt32?,UInt64>     { public override UInt32? C(UInt64 p)      { return Convert.ToUInt32(p); } }

		sealed class NU32_R4     : CB<UInt32?,Single>     { public override UInt32? C(Single p)      { return Convert.ToUInt32(p); } }
		sealed class NU32_R8     : CB<UInt32?,Double>     { public override UInt32? C(Double p)      { return Convert.ToUInt32(p); } }

		sealed class NU32_B      : CB<UInt32?,Boolean>    { public override UInt32? C(Boolean p)     { return Convert.ToUInt32(p); } }
		sealed class NU32_D      : CB<UInt32?,Decimal>    { public override UInt32? C(Decimal p)     { return Convert.ToUInt32(p); } }
		sealed class NU32_C      : CB<UInt32?,Char>       { public override UInt32? C(Char p)        { return Convert.ToUInt32(p); } }

		// Nullable Types.
		//
		sealed class NU32_NI8    : CB<UInt32?,SByte?>     { public override UInt32? C(SByte? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NI16   : CB<UInt32?,Int16?>     { public override UInt32? C(Int16? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NI32   : CB<UInt32?,Int32?>     { public override UInt32? C(Int32? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NI64   : CB<UInt32?,Int64?>     { public override UInt32? C(Int64? p)      { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NU32_NU8    : CB<UInt32?,Byte?>      { public override UInt32? C(Byte? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NU16   : CB<UInt32?,UInt16?>    { public override UInt32? C(UInt16? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NU64   : CB<UInt32?,UInt64?>    { public override UInt32? C(UInt64? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NU32_NR4    : CB<UInt32?,Single?>    { public override UInt32? C(Single? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NR8    : CB<UInt32?,Double?>    { public override UInt32? C(Double? p)     { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		sealed class NU32_NB     : CB<UInt32?,Boolean?>   { public override UInt32? C(Boolean? p)    { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_ND     : CB<UInt32?,Decimal?>   { public override UInt32? C(Decimal? p)    { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }
		sealed class NU32_NC     : CB<UInt32?,Char?>      { public override UInt32? C(Char? p)       { return p.HasValue? (UInt32?)Convert.ToUInt32(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NU32_dbS    : CB<UInt32?,SqlString>  { public override UInt32? C(SqlString p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NU32_dbU8   : CB<UInt32?,SqlByte>    { public override UInt32? C(SqlByte p)     { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbI16  : CB<UInt32?,SqlInt16>   { public override UInt32? C(SqlInt16 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbI32  : CB<UInt32?,SqlInt32>   { public override UInt32? C(SqlInt32 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbI64  : CB<UInt32?,SqlInt64>   { public override UInt32? C(SqlInt64 p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NU32_dbR4   : CB<UInt32?,SqlSingle>  { public override UInt32? C(SqlSingle p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbR8   : CB<UInt32?,SqlDouble>  { public override UInt32? C(SqlDouble p)   { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbD    : CB<UInt32?,SqlDecimal> { public override UInt32? C(SqlDecimal p)  { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }
		sealed class NU32_dbM    : CB<UInt32?,SqlMoney>   { public override UInt32? C(SqlMoney p)    { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NU32_dbB    : CB<UInt32?,SqlBoolean> { public override UInt32? C(SqlBoolean p)  { return p.IsNull? null: (UInt32?)Convert.ToUInt32(p.Value); } }

		sealed class NU32<Q>     : CB<UInt32?,Q>          { public override UInt32? C(Q p)           { return Convert<UInt32?,object>.From(p); } }
		sealed class NU32_O      : CB<UInt32?,object>     { public override UInt32? C(object p)      { return Convert.ToUInt32(p); } }

		static CB<T,P> GetNullableUInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NU32_U32    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NU32_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NU32_I8     ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NU32_I16    ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NU32_I32    ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NU32_I64    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NU32_U8     ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NU32_U16    ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NU32_U64    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NU32_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NU32_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NU32_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NU32_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NU32_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NU32_NI8    ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NU32_NI16   ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NU32_NI32   ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NU32_NI64   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NU32_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NU32_NU16   ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NU32_NU64   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NU32_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NU32_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NU32_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NU32_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NU32_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NU32_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NU32_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NU32_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NU32_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NU32_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NU32_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NU32_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NU32_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NU32_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NU32_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NU32_O      ());

			return (CB<T,P>)(object)(new NU32<P>());
		}

		#endregion

		#region UInt64?


		// Scalar Types.
		//
		sealed class NU64_U64    : CB<UInt64?,UInt64>     { public override UInt64? C(UInt64 p)      { return p; } }
		sealed class NU64_S      : CB<UInt64?,String>     { public override UInt64? C(String p)      { return p == null? null: (UInt64?)Convert.ToUInt64(p); } }

		sealed class NU64_I8     : CB<UInt64?,SByte>      { public override UInt64? C(SByte p)       { return Convert.ToUInt64(p); } }
		sealed class NU64_I16    : CB<UInt64?,Int16>      { public override UInt64? C(Int16 p)       { return Convert.ToUInt64(p); } }
		sealed class NU64_I32    : CB<UInt64?,Int32>      { public override UInt64? C(Int32 p)       { return Convert.ToUInt64(p); } }
		sealed class NU64_I64    : CB<UInt64?,Int64>      { public override UInt64? C(Int64 p)       { return Convert.ToUInt64(p); } }

		sealed class NU64_U8     : CB<UInt64?,Byte>       { public override UInt64? C(Byte p)        { return Convert.ToUInt64(p); } }
		sealed class NU64_U16    : CB<UInt64?,UInt16>     { public override UInt64? C(UInt16 p)      { return Convert.ToUInt64(p); } }
		sealed class NU64_U32    : CB<UInt64?,UInt32>     { public override UInt64? C(UInt32 p)      { return Convert.ToUInt64(p); } }

		sealed class NU64_R4     : CB<UInt64?,Single>     { public override UInt64? C(Single p)      { return Convert.ToUInt64(p); } }
		sealed class NU64_R8     : CB<UInt64?,Double>     { public override UInt64? C(Double p)      { return Convert.ToUInt64(p); } }

		sealed class NU64_B      : CB<UInt64?,Boolean>    { public override UInt64? C(Boolean p)     { return Convert.ToUInt64(p); } }
		sealed class NU64_D      : CB<UInt64?,Decimal>    { public override UInt64? C(Decimal p)     { return Convert.ToUInt64(p); } }
		sealed class NU64_C      : CB<UInt64?,Char>       { public override UInt64? C(Char p)        { return Convert.ToUInt64(p); } }

		// Nullable Types.
		//
		sealed class NU64_NI8    : CB<UInt64?,SByte?>     { public override UInt64? C(SByte? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NI16   : CB<UInt64?,Int16?>     { public override UInt64? C(Int16? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NI32   : CB<UInt64?,Int32?>     { public override UInt64? C(Int32? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NI64   : CB<UInt64?,Int64?>     { public override UInt64? C(Int64? p)      { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NU64_NU8    : CB<UInt64?,Byte?>      { public override UInt64? C(Byte? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NU16   : CB<UInt64?,UInt16?>    { public override UInt64? C(UInt16? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NU32   : CB<UInt64?,UInt32?>    { public override UInt64? C(UInt32? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NU64_NR4    : CB<UInt64?,Single?>    { public override UInt64? C(Single? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NR8    : CB<UInt64?,Double?>    { public override UInt64? C(Double? p)     { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		sealed class NU64_NB     : CB<UInt64?,Boolean?>   { public override UInt64? C(Boolean? p)    { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_ND     : CB<UInt64?,Decimal?>   { public override UInt64? C(Decimal? p)    { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }
		sealed class NU64_NC     : CB<UInt64?,Char?>      { public override UInt64? C(Char? p)       { return p.HasValue? (UInt64?)Convert.ToUInt64(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NU64_dbS    : CB<UInt64?,SqlString>  { public override UInt64? C(SqlString p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NU64_dbU8   : CB<UInt64?,SqlByte>    { public override UInt64? C(SqlByte p)     { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbI16  : CB<UInt64?,SqlInt16>   { public override UInt64? C(SqlInt16 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbI32  : CB<UInt64?,SqlInt32>   { public override UInt64? C(SqlInt32 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbI64  : CB<UInt64?,SqlInt64>   { public override UInt64? C(SqlInt64 p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NU64_dbR4   : CB<UInt64?,SqlSingle>  { public override UInt64? C(SqlSingle p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbR8   : CB<UInt64?,SqlDouble>  { public override UInt64? C(SqlDouble p)   { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbD    : CB<UInt64?,SqlDecimal> { public override UInt64? C(SqlDecimal p)  { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }
		sealed class NU64_dbM    : CB<UInt64?,SqlMoney>   { public override UInt64? C(SqlMoney p)    { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NU64_dbB    : CB<UInt64?,SqlBoolean> { public override UInt64? C(SqlBoolean p)  { return p.IsNull? null: (UInt64?)Convert.ToUInt64(p.Value); } }

		sealed class NU64<Q>     : CB<UInt64?,Q>          { public override UInt64? C(Q p)           { return Convert<UInt64?,object>.From(p); } }
		sealed class NU64_O      : CB<UInt64?,object>     { public override UInt64? C(object p)      { return Convert.ToUInt64(p); } }

		static CB<T,P> GetNullableUInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NU64_U64    ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NU64_S      ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NU64_I8     ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NU64_I16    ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NU64_I32    ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NU64_I64    ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NU64_U8     ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NU64_U16    ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NU64_U32    ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NU64_R4     ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NU64_R8     ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NU64_B      ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NU64_D      ());
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NU64_C      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NU64_NI8    ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NU64_NI16   ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NU64_NI32   ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NU64_NI64   ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NU64_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NU64_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NU64_NU32   ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NU64_NR4    ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NU64_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NU64_NB     ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NU64_ND     ());
			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NU64_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NU64_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NU64_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NU64_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NU64_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NU64_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NU64_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NU64_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NU64_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NU64_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NU64_dbB    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NU64_O      ());

			return (CB<T,P>)(object)(new NU64<P>());
		}

		#endregion

		#region Char?


		// Scalar Types.
		//
		sealed class NC_C        : CB<Char?,Char>       { public override Char? C(Char p)        { return p; } }
		sealed class NC_S        : CB<Char?,String>     { public override Char? C(String p)      { return p == null? null: (Char?)Convert.ToChar(p); } }

		sealed class NC_I8       : CB<Char?,SByte>      { public override Char? C(SByte p)       { return Convert.ToChar(p); } }
		sealed class NC_I16      : CB<Char?,Int16>      { public override Char? C(Int16 p)       { return Convert.ToChar(p); } }
		sealed class NC_I32      : CB<Char?,Int32>      { public override Char? C(Int32 p)       { return Convert.ToChar(p); } }
		sealed class NC_I64      : CB<Char?,Int64>      { public override Char? C(Int64 p)       { return Convert.ToChar(p); } }

		sealed class NC_U8       : CB<Char?,Byte>       { public override Char? C(Byte p)        { return Convert.ToChar(p); } }
		sealed class NC_U16      : CB<Char?,UInt16>     { public override Char? C(UInt16 p)      { return Convert.ToChar(p); } }
		sealed class NC_U32      : CB<Char?,UInt32>     { public override Char? C(UInt32 p)      { return Convert.ToChar(p); } }
		sealed class NC_U64      : CB<Char?,UInt64>     { public override Char? C(UInt64 p)      { return Convert.ToChar(p); } }
		sealed class NC_B        : CB<Char?,Boolean>    { public override Char? C(Boolean p)     { return p? '1':'0'; } }

		// Nullable Types.
		//
		sealed class NC_NI8      : CB<Char?,SByte?>     { public override Char? C(SByte? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NI16     : CB<Char?,Int16?>     { public override Char? C(Int16? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NI32     : CB<Char?,Int32?>     { public override Char? C(Int32? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NI64     : CB<Char?,Int64?>     { public override Char? C(Int64? p)      { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }

		sealed class NC_NU8      : CB<Char?,Byte?>      { public override Char? C(Byte? p)       { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NU16     : CB<Char?,UInt16?>    { public override Char? C(UInt16? p)     { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NU32     : CB<Char?,UInt32?>    { public override Char? C(UInt32? p)     { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NU64     : CB<Char?,UInt64?>    { public override Char? C(UInt64? p)     { return p.HasValue? (Char?)Convert.ToChar(p.Value) : null; } }
		sealed class NC_NB       : CB<Char?,Boolean?>   { public override Char? C(Boolean? p)    { return p.HasValue? (Char?)(p.Value? '1':'0')      : null; } }

		// SqlTypes.
		//
		sealed class NC_dbU8     : CB<Char?,SqlByte>    { public override Char? C(SqlByte p)     { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NC_dbI16    : CB<Char?,SqlInt16>   { public override Char? C(SqlInt16 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NC_dbI32    : CB<Char?,SqlInt32>   { public override Char? C(SqlInt32 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NC_dbI64    : CB<Char?,SqlInt64>   { public override Char? C(SqlInt64 p)    { return p.IsNull? null: (Char?)Convert.ToChar(p.Value); } }
		sealed class NC_dbB      : CB<Char?,SqlBoolean> { public override Char? C(SqlBoolean p)  { return p.IsNull? null: (Char?)(p.Value? '1':'0'); } }

		sealed class NC<Q>       : CB<Char?,Q>          { public override Char? C(Q p)           { return Convert<Char?,object>.From(p); } }
		sealed class NC_O        : CB<Char?,object>     { public override Char? C(object p)      { return Convert.ToChar(p); } }

		static CB<T,P> GetNullableCharConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Char))        return (CB<T,P>)(object)(new NC_C        ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NC_S        ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NC_I8       ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NC_I16      ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NC_I32      ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NC_I64      ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NC_U8       ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NC_U16      ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NC_U32      ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NC_U64      ());
			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NC_B        ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NC_NI8      ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NC_NI16     ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NC_NI32     ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NC_NI64     ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NC_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NC_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NC_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NC_NU64     ());
			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NC_NB       ());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NC_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NC_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NC_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NC_dbI64    ());
			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NC_dbB      ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NC_O        ());

			return (CB<T,P>)(object)(new NC<P>());
		}

		#endregion

		#region Single?


		// Scalar Types.
		//
		sealed class NR4_R4      : CB<Single?,Single>     { public override Single? C(Single p)      { return p; } }
		sealed class NR4_S       : CB<Single?,String>     { public override Single? C(String p)      { return p == null? null: (Single?)Convert.ToSingle(p); } }

		sealed class NR4_I8      : CB<Single?,SByte>      { public override Single? C(SByte p)       { return Convert.ToSingle(p); } }
		sealed class NR4_I16     : CB<Single?,Int16>      { public override Single? C(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class NR4_I32     : CB<Single?,Int32>      { public override Single? C(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class NR4_I64     : CB<Single?,Int64>      { public override Single? C(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class NR4_U8      : CB<Single?,Byte>       { public override Single? C(Byte p)        { return Convert.ToSingle(p); } }
		sealed class NR4_U16     : CB<Single?,UInt16>     { public override Single? C(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class NR4_U32     : CB<Single?,UInt32>     { public override Single? C(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class NR4_U64     : CB<Single?,UInt64>     { public override Single? C(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class NR4_R8      : CB<Single?,Double>     { public override Single? C(Double p)      { return Convert.ToSingle(p); } }

		sealed class NR4_B       : CB<Single?,Boolean>    { public override Single? C(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class NR4_D       : CB<Single?,Decimal>    { public override Single? C(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class NR4_NI8     : CB<Single?,SByte?>     { public override Single? C(SByte? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NI16    : CB<Single?,Int16?>     { public override Single? C(Int16? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NI32    : CB<Single?,Int32?>     { public override Single? C(Int32? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NI64    : CB<Single?,Int64?>     { public override Single? C(Int64? p)      { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NR4_NU8     : CB<Single?,Byte?>      { public override Single? C(Byte? p)       { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NU16    : CB<Single?,UInt16?>    { public override Single? C(UInt16? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NU32    : CB<Single?,UInt32?>    { public override Single? C(UInt32? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_NU64    : CB<Single?,UInt64?>    { public override Single? C(UInt64? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NR4_NR8     : CB<Single?,Double?>    { public override Single? C(Double? p)     { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		sealed class NR4_NB      : CB<Single?,Boolean?>   { public override Single? C(Boolean? p)    { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }
		sealed class NR4_ND      : CB<Single?,Decimal?>   { public override Single? C(Decimal? p)    { return p.HasValue? (Single?)Convert.ToSingle(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NR4_dbR4    : CB<Single?,SqlSingle>  { public override Single? C(SqlSingle p)   { return p.IsNull? null: (Single?)                 p.Value;  } }
		sealed class NR4_dbS     : CB<Single?,SqlString>  { public override Single? C(SqlString p)   { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NR4_dbU8    : CB<Single?,SqlByte>    { public override Single? C(SqlByte p)     { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NR4_dbI16   : CB<Single?,SqlInt16>   { public override Single? C(SqlInt16 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NR4_dbI32   : CB<Single?,SqlInt32>   { public override Single? C(SqlInt32 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NR4_dbI64   : CB<Single?,SqlInt64>   { public override Single? C(SqlInt64 p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NR4_dbR8    : CB<Single?,SqlDouble>  { public override Single? C(SqlDouble p)   { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NR4_dbD     : CB<Single?,SqlDecimal> { public override Single? C(SqlDecimal p)  { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }
		sealed class NR4_dbM     : CB<Single?,SqlMoney>   { public override Single? C(SqlMoney p)    { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NR4_dbB     : CB<Single?,SqlBoolean> { public override Single? C(SqlBoolean p)  { return p.IsNull? null: (Single?)Convert.ToSingle(p.Value); } }

		sealed class NR4<Q>      : CB<Single?,Q>          { public override Single? C(Q p)           { return Convert<Single?,object>.From(p); } }
		sealed class NR4_O       : CB<Single?,object>     { public override Single? C(object p)      { return Convert.ToSingle(p); } }

		static CB<T,P> GetNullableSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Single))      return (CB<T,P>)(object)(new NR4_R4      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NR4_S       ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NR4_I8      ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NR4_I16     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NR4_I32     ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NR4_I64     ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NR4_U8      ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NR4_U16     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NR4_U32     ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NR4_U64     ());

			if (t == typeof(Double))      return (CB<T,P>)(object)(new NR4_R8      ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NR4_B       ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NR4_D       ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NR4_NI8     ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NR4_NI16    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NR4_NI32    ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NR4_NI64    ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NR4_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NR4_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NR4_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NR4_NU64    ());

			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NR4_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NR4_NB      ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NR4_ND      ());

			// SqlTypes.
			//
			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NR4_dbR4    ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NR4_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NR4_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NR4_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NR4_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NR4_dbI64   ());

			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NR4_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NR4_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NR4_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NR4_dbB     ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NR4_O       ());

			return (CB<T,P>)(object)(new NR4<P>());
		}

		#endregion

		#region Double?


		// Scalar Types.
		//
		sealed class NR8_R8      : CB<Double?,Double>     { public override Double? C(Double p)      { return p; } }
		sealed class NR8_S       : CB<Double?,String>     { public override Double? C(String p)      { return p == null? null: (Double?)Convert.ToDouble(p); } }

		sealed class NR8_I8      : CB<Double?,SByte>      { public override Double? C(SByte p)       { return Convert.ToDouble(p); } }
		sealed class NR8_I16     : CB<Double?,Int16>      { public override Double? C(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class NR8_I32     : CB<Double?,Int32>      { public override Double? C(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class NR8_I64     : CB<Double?,Int64>      { public override Double? C(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class NR8_U8      : CB<Double?,Byte>       { public override Double? C(Byte p)        { return Convert.ToDouble(p); } }
		sealed class NR8_U16     : CB<Double?,UInt16>     { public override Double? C(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class NR8_U32     : CB<Double?,UInt32>     { public override Double? C(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class NR8_U64     : CB<Double?,UInt64>     { public override Double? C(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class NR8_R4      : CB<Double?,Single>     { public override Double? C(Single p)      { return Convert.ToDouble(p); } }

		sealed class NR8_B       : CB<Double?,Boolean>    { public override Double? C(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class NR8_D       : CB<Double?,Decimal>    { public override Double? C(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class NR8_NI8     : CB<Double?,SByte?>     { public override Double? C(SByte? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NI16    : CB<Double?,Int16?>     { public override Double? C(Int16? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NI32    : CB<Double?,Int32?>     { public override Double? C(Int32? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NI64    : CB<Double?,Int64?>     { public override Double? C(Int64? p)      { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NR8_NU8     : CB<Double?,Byte?>      { public override Double? C(Byte? p)       { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NU16    : CB<Double?,UInt16?>    { public override Double? C(UInt16? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NU32    : CB<Double?,UInt32?>    { public override Double? C(UInt32? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_NU64    : CB<Double?,UInt64?>    { public override Double? C(UInt64? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NR8_NR4     : CB<Double?,Single?>    { public override Double? C(Single? p)     { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		sealed class NR8_NB      : CB<Double?,Boolean?>   { public override Double? C(Boolean? p)    { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }
		sealed class NR8_ND      : CB<Double?,Decimal?>   { public override Double? C(Decimal? p)    { return p.HasValue? (Double?)Convert.ToDouble(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NR8_dbR8    : CB<Double?,SqlDouble>  { public override Double? C(SqlDouble p)   { return p.IsNull? null: (Double?)                 p.Value;  } }
		sealed class NR8_dbS     : CB<Double?,SqlString>  { public override Double? C(SqlString p)   { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NR8_dbU8    : CB<Double?,SqlByte>    { public override Double? C(SqlByte p)     { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NR8_dbI16   : CB<Double?,SqlInt16>   { public override Double? C(SqlInt16 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NR8_dbI32   : CB<Double?,SqlInt32>   { public override Double? C(SqlInt32 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NR8_dbI64   : CB<Double?,SqlInt64>   { public override Double? C(SqlInt64 p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NR8_dbR4    : CB<Double?,SqlSingle>  { public override Double? C(SqlSingle p)   { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NR8_dbD     : CB<Double?,SqlDecimal> { public override Double? C(SqlDecimal p)  { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }
		sealed class NR8_dbM     : CB<Double?,SqlMoney>   { public override Double? C(SqlMoney p)    { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NR8_dbB     : CB<Double?,SqlBoolean> { public override Double? C(SqlBoolean p)  { return p.IsNull? null: (Double?)Convert.ToDouble(p.Value); } }

		sealed class NR8<Q>      : CB<Double?,Q>          { public override Double? C(Q p)           { return Convert<Double?,object>.From(p); } }
		sealed class NR8_O       : CB<Double?,object>     { public override Double? C(object p)      { return Convert.ToDouble(p); } }

		static CB<T,P> GetNullableDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NR8_R8      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NR8_S       ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NR8_I8      ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NR8_I16     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NR8_I32     ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NR8_I64     ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NR8_U8      ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NR8_U16     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NR8_U32     ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NR8_U64     ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NR8_R4      ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NR8_B       ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NR8_D       ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NR8_NI8     ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NR8_NI16    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NR8_NI32    ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NR8_NI64    ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NR8_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NR8_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NR8_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NR8_NU64    ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NR8_NR4     ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NR8_NB      ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NR8_ND      ());

			// SqlTypes.
			//
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NR8_dbR8    ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NR8_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NR8_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NR8_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NR8_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NR8_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NR8_dbR4    ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NR8_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NR8_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NR8_dbB     ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NR8_O       ());

			return (CB<T,P>)(object)(new NR8<P>());
		}

		#endregion

		#region Boolean?


		// Scalar Types.
		//
		sealed class NB_B        : CB<Boolean?,Boolean>    { public override Boolean? C(Boolean p)     { return p; } }
		sealed class NB_S        : CB<Boolean?,String>     { public override Boolean? C(String p)      { return Convert.ToBoolean(p); } }

		sealed class NB_I8       : CB<Boolean?,SByte>      { public override Boolean? C(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class NB_I16      : CB<Boolean?,Int16>      { public override Boolean? C(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class NB_I32      : CB<Boolean?,Int32>      { public override Boolean? C(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class NB_I64      : CB<Boolean?,Int64>      { public override Boolean? C(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class NB_U8       : CB<Boolean?,Byte>       { public override Boolean? C(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class NB_U16      : CB<Boolean?,UInt16>     { public override Boolean? C(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class NB_U32      : CB<Boolean?,UInt32>     { public override Boolean? C(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class NB_U64      : CB<Boolean?,UInt64>     { public override Boolean? C(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class NB_R4       : CB<Boolean?,Single>     { public override Boolean? C(Single p)      { return Convert.ToBoolean(p); } }
		sealed class NB_R8       : CB<Boolean?,Double>     { public override Boolean? C(Double p)      { return Convert.ToBoolean(p); } }

		sealed class NB_D        : CB<Boolean?,Decimal>    { public override Boolean? C(Decimal p)     { return Convert.ToBoolean(p); } }

		sealed class NB_C        : CB<Boolean?,Char>       { public override Boolean? C(Char p)        { return Convert<Boolean,Char>.I.C(p); } }

		// Nullable Types.
		//
		sealed class NB_NI8      : CB<Boolean?,SByte?>     { public override Boolean? C(SByte? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NI16     : CB<Boolean?,Int16?>     { public override Boolean? C(Int16? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NI32     : CB<Boolean?,Int32?>     { public override Boolean? C(Int32? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NI64     : CB<Boolean?,Int64?>     { public override Boolean? C(Int64? p)      { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NB_NU8      : CB<Boolean?,Byte?>      { public override Boolean? C(Byte? p)       { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NU16     : CB<Boolean?,UInt16?>    { public override Boolean? C(UInt16? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NU32     : CB<Boolean?,UInt32?>    { public override Boolean? C(UInt32? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NU64     : CB<Boolean?,UInt64?>    { public override Boolean? C(UInt64? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NB_NR4      : CB<Boolean?,Single?>    { public override Boolean? C(Single? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }
		sealed class NB_NR8      : CB<Boolean?,Double?>    { public override Boolean? C(Double? p)     { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NB_ND       : CB<Boolean?,Decimal?>   { public override Boolean? C(Decimal? p)    { return p.HasValue? (Boolean?)Convert.ToBoolean(p.Value): null; } }

		sealed class NB_NC       : CB<Boolean?,Char?>      { public override Boolean? C(Char? p)       { return p.HasValue? (Boolean?)Convert<Boolean,Char>.I.C(p.Value): null; } }

		// SqlTypes.
		//
		sealed class NB_dbB      : CB<Boolean?,SqlBoolean> { public override Boolean? C(SqlBoolean p)  { return p.IsNull? null: (Boolean?)                  p.Value;  } }
		sealed class NB_dbS      : CB<Boolean?,SqlString>  { public override Boolean? C(SqlString p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }

		sealed class NB_dbU8     : CB<Boolean?,SqlByte>    { public override Boolean? C(SqlByte p)     { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbI16    : CB<Boolean?,SqlInt16>   { public override Boolean? C(SqlInt16 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbI32    : CB<Boolean?,SqlInt32>   { public override Boolean? C(SqlInt32 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbI64    : CB<Boolean?,SqlInt64>   { public override Boolean? C(SqlInt64 p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }

		sealed class NB_dbR4     : CB<Boolean?,SqlSingle>  { public override Boolean? C(SqlSingle p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbR8     : CB<Boolean?,SqlDouble>  { public override Boolean? C(SqlDouble p)   { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbD      : CB<Boolean?,SqlDecimal> { public override Boolean? C(SqlDecimal p)  { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }
		sealed class NB_dbM      : CB<Boolean?,SqlMoney>   { public override Boolean? C(SqlMoney p)    { return p.IsNull? null: (Boolean?)Convert.ToBoolean(p.Value); } }


		sealed class NB<Q>       : CB<Boolean?,Q>          { public override Boolean? C(Q p)           { return Convert<Boolean?,object>.From(p); } }
		sealed class NB_O        : CB<Boolean?,object>     { public override Boolean? C(object p)      { return Convert.ToBoolean(p); } }

		static CB<T,P> GetNullableBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NB_B        ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NB_S        ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NB_I8       ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NB_I16      ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NB_I32      ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NB_I64      ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NB_U8       ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NB_U16      ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NB_U32      ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NB_U64      ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NB_R4       ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NB_R8       ());

			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NB_D        ());

			if (t == typeof(Char))        return (CB<T,P>)(object)(new NB_C        ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NB_NI8      ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NB_NI16     ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NB_NI32     ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NB_NI64     ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NB_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NB_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NB_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NB_NU64     ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NB_NR4      ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NB_NR8      ());

			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NB_ND       ());

			if (t == typeof(Char?))       return (CB<T,P>)(object)(new NB_NC       ());

			// SqlTypes.
			//
			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NB_dbB      ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NB_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NB_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NB_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NB_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NB_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NB_dbR4     ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NB_dbR8     ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NB_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NB_dbM      ());


			if (t == typeof(object))      return (CB<T,P>)(object)(new NB_O        ());

			return (CB<T,P>)(object)(new NB<P>());
		}

		#endregion

		#region Decimal?


		// Scalar Types.
		//
		sealed class ND_D        : CB<Decimal?,Decimal>    { public override Decimal? C(Decimal p)     { return p; } }
		sealed class ND_S        : CB<Decimal?,String>     { public override Decimal? C(String p)      { return p == null? null: (Decimal?)Convert.ToDecimal(p); } }

		sealed class ND_I8       : CB<Decimal?,SByte>      { public override Decimal? C(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class ND_I16      : CB<Decimal?,Int16>      { public override Decimal? C(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class ND_I32      : CB<Decimal?,Int32>      { public override Decimal? C(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class ND_I64      : CB<Decimal?,Int64>      { public override Decimal? C(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class ND_U8       : CB<Decimal?,Byte>       { public override Decimal? C(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class ND_U16      : CB<Decimal?,UInt16>     { public override Decimal? C(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class ND_U32      : CB<Decimal?,UInt32>     { public override Decimal? C(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class ND_U64      : CB<Decimal?,UInt64>     { public override Decimal? C(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class ND_R4       : CB<Decimal?,Single>     { public override Decimal? C(Single p)      { return Convert.ToDecimal(p); } }
		sealed class ND_R8       : CB<Decimal?,Double>     { public override Decimal? C(Double p)      { return Convert.ToDecimal(p); } }

		sealed class ND_B        : CB<Decimal?,Boolean>    { public override Decimal? C(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class ND_NI8      : CB<Decimal?,SByte?>     { public override Decimal? C(SByte? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NI16     : CB<Decimal?,Int16?>     { public override Decimal? C(Int16? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NI32     : CB<Decimal?,Int32?>     { public override Decimal? C(Int32? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NI64     : CB<Decimal?,Int64?>     { public override Decimal? C(Int64? p)      { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class ND_NU8      : CB<Decimal?,Byte?>      { public override Decimal? C(Byte? p)       { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NU16     : CB<Decimal?,UInt16?>    { public override Decimal? C(UInt16? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NU32     : CB<Decimal?,UInt32?>    { public override Decimal? C(UInt32? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NU64     : CB<Decimal?,UInt64?>    { public override Decimal? C(UInt64? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class ND_NR4      : CB<Decimal?,Single?>    { public override Decimal? C(Single? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }
		sealed class ND_NR8      : CB<Decimal?,Double?>    { public override Decimal? C(Double? p)     { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		sealed class ND_NB       : CB<Decimal?,Boolean?>   { public override Decimal? C(Boolean? p)    { return p.HasValue? (Decimal?)Convert.ToDecimal(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class ND_dbD      : CB<Decimal?,SqlDecimal> { public override Decimal? C(SqlDecimal p)  { return p.IsNull? null: (Decimal?)                  p.Value;  } }
		sealed class ND_dbM      : CB<Decimal?,SqlMoney>   { public override Decimal? C(SqlMoney p)    { return p.IsNull? null: (Decimal?)                  p.Value;  } }
		sealed class ND_dbS      : CB<Decimal?,SqlString>  { public override Decimal? C(SqlString p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class ND_dbU8     : CB<Decimal?,SqlByte>    { public override Decimal? C(SqlByte p)     { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class ND_dbI16    : CB<Decimal?,SqlInt16>   { public override Decimal? C(SqlInt16 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class ND_dbI32    : CB<Decimal?,SqlInt32>   { public override Decimal? C(SqlInt32 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class ND_dbI64    : CB<Decimal?,SqlInt64>   { public override Decimal? C(SqlInt64 p)    { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class ND_dbR4     : CB<Decimal?,SqlSingle>  { public override Decimal? C(SqlSingle p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }
		sealed class ND_dbR8     : CB<Decimal?,SqlDouble>  { public override Decimal? C(SqlDouble p)   { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class ND_dbB      : CB<Decimal?,SqlBoolean> { public override Decimal? C(SqlBoolean p)  { return p.IsNull? null: (Decimal?)Convert.ToDecimal(p.Value); } }

		sealed class ND<Q>       : CB<Decimal?,Q>          { public override Decimal? C(Q p)           { return Convert<Decimal?,object>.From(p); } }
		sealed class ND_O        : CB<Decimal?,object>     { public override Decimal? C(object p)      { return Convert.ToDecimal(p); } }

		static CB<T,P> GetNullableDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new ND_D        ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new ND_S        ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new ND_I8       ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new ND_I16      ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new ND_I32      ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new ND_I64      ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new ND_U8       ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new ND_U16      ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new ND_U32      ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new ND_U64      ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new ND_R4       ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new ND_R8       ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new ND_B        ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new ND_NI8      ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new ND_NI16     ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new ND_NI32     ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new ND_NI64     ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new ND_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new ND_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new ND_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new ND_NU64     ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new ND_NR4      ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new ND_NR8      ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new ND_NB       ());

			// SqlTypes.
			//
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new ND_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new ND_dbM      ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new ND_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new ND_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new ND_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new ND_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new ND_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new ND_dbR4     ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new ND_dbR8     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new ND_dbB      ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new ND_O        ());

			return (CB<T,P>)(object)(new ND<P>());
		}

		#endregion

		#region DateTime?


		// Scalar Types.
		//
		sealed class NDT_DT      : CB<DateTime?,DateTime>   { public override DateTime? C(DateTime p)    { return p; } }
		sealed class NDT_S       : CB<DateTime?,String>     { public override DateTime? C(String p)      { return p == null? null: (DateTime?)Convert.ToDateTime(p); } }

		sealed class NDT_I8      : CB<DateTime?,SByte>      { public override DateTime? C(SByte p)       { return Convert.ToDateTime(p); } }
		sealed class NDT_I16     : CB<DateTime?,Int16>      { public override DateTime? C(Int16 p)       { return Convert.ToDateTime(p); } }
		sealed class NDT_I32     : CB<DateTime?,Int32>      { public override DateTime? C(Int32 p)       { return Convert.ToDateTime(p); } }
		sealed class NDT_I64     : CB<DateTime?,Int64>      { public override DateTime? C(Int64 p)       { return Convert.ToDateTime(p); } }

		sealed class NDT_U8      : CB<DateTime?,Byte>       { public override DateTime? C(Byte p)        { return Convert.ToDateTime(p); } }
		sealed class NDT_U16     : CB<DateTime?,UInt16>     { public override DateTime? C(UInt16 p)      { return Convert.ToDateTime(p); } }
		sealed class NDT_U32     : CB<DateTime?,UInt32>     { public override DateTime? C(UInt32 p)      { return Convert.ToDateTime(p); } }
		sealed class NDT_U64     : CB<DateTime?,UInt64>     { public override DateTime? C(UInt64 p)      { return Convert.ToDateTime(p); } }

		sealed class NDT_R4      : CB<DateTime?,Single>     { public override DateTime? C(Single p)      { return Convert.ToDateTime(p); } }
		sealed class NDT_R8      : CB<DateTime?,Double>     { public override DateTime? C(Double p)      { return Convert.ToDateTime(p); } }

		sealed class NDT_B       : CB<DateTime?,Boolean>    { public override DateTime? C(Boolean p)     { return Convert.ToDateTime(p); } }
		sealed class NDT_D       : CB<DateTime?,Decimal>    { public override DateTime? C(Decimal p)     { return Convert.ToDateTime(p); } }

		// Nullable Types.
		//
		sealed class NDT_NI8     : CB<DateTime?,SByte?>     { public override DateTime? C(SByte? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NI16    : CB<DateTime?,Int16?>     { public override DateTime? C(Int16? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NI32    : CB<DateTime?,Int32?>     { public override DateTime? C(Int32? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NI64    : CB<DateTime?,Int64?>     { public override DateTime? C(Int64? p)      { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NDT_NU8     : CB<DateTime?,Byte?>      { public override DateTime? C(Byte? p)       { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NU16    : CB<DateTime?,UInt16?>    { public override DateTime? C(UInt16? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NU32    : CB<DateTime?,UInt32?>    { public override DateTime? C(UInt32? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NU64    : CB<DateTime?,UInt64?>    { public override DateTime? C(UInt64? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NDT_NR4     : CB<DateTime?,Single?>    { public override DateTime? C(Single? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_NR8     : CB<DateTime?,Double?>    { public override DateTime? C(Double? p)     { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		sealed class NDT_NB      : CB<DateTime?,Boolean?>   { public override DateTime? C(Boolean? p)    { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }
		sealed class NDT_ND      : CB<DateTime?,Decimal?>   { public override DateTime? C(Decimal? p)    { return p.HasValue? (DateTime?)Convert.ToDateTime(p.Value) : null; } }

		// SqlTypes.
		//
		sealed class NDT_dbS     : CB<DateTime?,SqlString>  { public override DateTime? C(SqlString p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NDT_dbU8    : CB<DateTime?,SqlByte>    { public override DateTime? C(SqlByte p)     { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbI16   : CB<DateTime?,SqlInt16>   { public override DateTime? C(SqlInt16 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbI32   : CB<DateTime?,SqlInt32>   { public override DateTime? C(SqlInt32 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbI64   : CB<DateTime?,SqlInt64>   { public override DateTime? C(SqlInt64 p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NDT_dbR4    : CB<DateTime?,SqlSingle>  { public override DateTime? C(SqlSingle p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbR8    : CB<DateTime?,SqlDouble>  { public override DateTime? C(SqlDouble p)   { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbD     : CB<DateTime?,SqlDecimal> { public override DateTime? C(SqlDecimal p)  { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbM     : CB<DateTime?,SqlMoney>   { public override DateTime? C(SqlMoney p)    { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }

		sealed class NDT_dbB     : CB<DateTime?,SqlBoolean> { public override DateTime? C(SqlBoolean p)  { return p.IsNull? null: (DateTime?)Convert.ToDateTime(p.Value); } }
		sealed class NDT_dbDT    : CB<DateTime?,SqlDateTime>{ public override DateTime? C(SqlDateTime p) { return p.IsNull? null: (DateTime?)                   p.Value;  } }

		sealed class NDT<Q>      : CB<DateTime?,Q>          { public override DateTime? C(Q p)           { return Convert<DateTime?,object>.From(p); } }
		sealed class NDT_O       : CB<DateTime?,object>     { public override DateTime? C(object p)      { return Convert.ToDateTime(p); } }

		static CB<T,P> GetNullableDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(DateTime))    return (CB<T,P>)(object)(new NDT_DT      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NDT_S       ());

			if (t == typeof(SByte))       return (CB<T,P>)(object)(new NDT_I8      ());
			if (t == typeof(Int16))       return (CB<T,P>)(object)(new NDT_I16     ());
			if (t == typeof(Int32))       return (CB<T,P>)(object)(new NDT_I32     ());
			if (t == typeof(Int64))       return (CB<T,P>)(object)(new NDT_I64     ());

			if (t == typeof(Byte))        return (CB<T,P>)(object)(new NDT_U8      ());
			if (t == typeof(UInt16))      return (CB<T,P>)(object)(new NDT_U16     ());
			if (t == typeof(UInt32))      return (CB<T,P>)(object)(new NDT_U32     ());
			if (t == typeof(UInt64))      return (CB<T,P>)(object)(new NDT_U64     ());

			if (t == typeof(Single))      return (CB<T,P>)(object)(new NDT_R4      ());
			if (t == typeof(Double))      return (CB<T,P>)(object)(new NDT_R8      ());

			if (t == typeof(Boolean))     return (CB<T,P>)(object)(new NDT_B       ());
			if (t == typeof(Decimal))     return (CB<T,P>)(object)(new NDT_D       ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T,P>)(object)(new NDT_NI8     ());
			if (t == typeof(Int16?))      return (CB<T,P>)(object)(new NDT_NI16    ());
			if (t == typeof(Int32?))      return (CB<T,P>)(object)(new NDT_NI32    ());
			if (t == typeof(Int64?))      return (CB<T,P>)(object)(new NDT_NI64    ());

			if (t == typeof(Byte?))       return (CB<T,P>)(object)(new NDT_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T,P>)(object)(new NDT_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T,P>)(object)(new NDT_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T,P>)(object)(new NDT_NU64    ());

			if (t == typeof(Single?))     return (CB<T,P>)(object)(new NDT_NR4     ());
			if (t == typeof(Double?))     return (CB<T,P>)(object)(new NDT_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T,P>)(object)(new NDT_NB      ());
			if (t == typeof(Decimal?))    return (CB<T,P>)(object)(new NDT_ND      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NDT_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T,P>)(object)(new NDT_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T,P>)(object)(new NDT_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T,P>)(object)(new NDT_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T,P>)(object)(new NDT_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T,P>)(object)(new NDT_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T,P>)(object)(new NDT_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T,P>)(object)(new NDT_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T,P>)(object)(new NDT_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T,P>)(object)(new NDT_dbB     ());
			if (t == typeof(SqlDateTime)) return (CB<T,P>)(object)(new NDT_dbDT    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NDT_O       ());

			return (CB<T,P>)(object)(new NDT<P>());
		}

		#endregion

		#region TimeSpan?


		// Scalar Types.
		//
		sealed class NTS_TS      : CB<TimeSpan?,TimeSpan>   { public override TimeSpan? C(TimeSpan p)    { return p; } }
		sealed class NTS_S       : CB<TimeSpan?,String>     { public override TimeSpan? C(String p)      { return p == null? null: (TimeSpan?)TimeSpan.Parse(p); } }
		sealed class NTS_DT      : CB<TimeSpan?,DateTime>   { public override TimeSpan? C(DateTime p)    { return p - DateTime.MinValue; } }

		// Nullable Types.
		//
		sealed class NTS_NDT     : CB<TimeSpan?,DateTime?>  { public override TimeSpan? C(DateTime? p)   { return p.HasValue? (TimeSpan?)(p.Value - DateTime.MinValue) : null; } }

		// SqlTypes.
		//
		sealed class NTS_dbS     : CB<TimeSpan?,SqlString>  { public override TimeSpan? C(SqlString p)   { return p.IsNull? null: (TimeSpan?)TimeSpan.Parse(p.Value);       } }
		sealed class NTS_dbDT    : CB<TimeSpan?,SqlDateTime>{ public override TimeSpan? C(SqlDateTime p) { return p.IsNull? null: (TimeSpan?)(p.Value - DateTime.MinValue); } }

		sealed class NTS<Q>      : CB<TimeSpan?,Q>          { public override TimeSpan? C(Q p)           { return Convert<TimeSpan?,object>.From(p); } }
		sealed class NTS_O       : CB<TimeSpan?,object>     { public override TimeSpan? C(object p)     
			{
				if (p == null) return null;

				// Scalar Types.
				//
				if (p is TimeSpan)    return Convert<TimeSpan?,TimeSpan>   .I.C((TimeSpan)p);
				if (p is String)      return Convert<TimeSpan?,String>     .I.C((String)p);
				if (p is DateTime)    return Convert<TimeSpan?,DateTime>   .I.C((DateTime)p);

				// Nullable Types.
				//
				if (p is DateTime?)   return Convert<TimeSpan?,DateTime?>  .I.C((DateTime?)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<TimeSpan?,SqlString>  .I.C((SqlString)p);
				if (p is SqlDateTime) return Convert<TimeSpan?,SqlDateTime>.I.C((SqlDateTime)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
			} }

		static CB<T,P> GetNullableTimeSpanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(TimeSpan))    return (CB<T,P>)(object)(new NTS_TS      ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NTS_S       ());
			if (t == typeof(DateTime))    return (CB<T,P>)(object)(new NTS_DT      ());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))   return (CB<T,P>)(object)(new NTS_NDT     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NTS_dbS     ());
			if (t == typeof(SqlDateTime)) return (CB<T,P>)(object)(new NTS_dbDT    ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NTS_O       ());

			return (CB<T,P>)(object)(new NTS<P>());
		}

		#endregion

		#region Guid?


		// Scalar Types.
		//
		sealed class NG_G        : CB<Guid?,Guid>       { public override Guid? C(Guid p)        { return p; } }
		sealed class NG_S        : CB<Guid?,String>     { public override Guid? C(String p)      { return p == null? null: (Guid?)new Guid(p); } }

		// Nullable Types.
		//

		// SqlTypes.
		//
		sealed class NG_dbG      : CB<Guid?,SqlGuid>    { public override Guid? C(SqlGuid p)     { return p.IsNull? null: (Guid?)p.Value;             } }
		sealed class NG_dbS      : CB<Guid?,SqlString>  { public override Guid? C(SqlString p)   { return p.IsNull? null: (Guid?)new Guid(p.Value);   } }
		sealed class NG_dbBin    : CB<Guid?,SqlBinary>  { public override Guid? C(SqlBinary p)   { return p.IsNull? null: (Guid?)p.ToSqlGuid().Value; } }

		// Other Types.
		//
		sealed class NG_T        : CB<Guid?,Type>       { public override Guid? C(Type p)        { return p == null? null: (Guid?)p.GUID; } }
		sealed class NG_AU8      : CB<Guid?,Byte[]>     { public override Guid? C(Byte[] p)      { return p == null? null: (Guid?)new Guid(p); } }

		sealed class NG<Q>       : CB<Guid?,Q>          { public override Guid? C(Q p)           { return Convert<Guid?,object>.From(p); } }
		sealed class NG_O        : CB<Guid?,object>     { public override Guid? C(object p)     
			{
				if (p == null) return null;

				// Scalar Types.
				//
				if (p is Guid)        return Convert<Guid?,Guid>       .I.C((Guid)p);
				if (p is String)      return Convert<Guid?,String>     .I.C((String)p);

				// Nullable Types.
				//

				// SqlTypes.
				//
				if (p is SqlGuid)     return Convert<Guid?,SqlGuid>    .I.C((SqlGuid)p);
				if (p is SqlString)   return Convert<Guid?,SqlString>  .I.C((SqlString)p);
				if (p is SqlBinary)   return Convert<Guid?,SqlBinary>  .I.C((SqlBinary)p);

				// Other Types.
				//
				if (p is Type)        return Convert<Guid?,Type>       .I.C((Type)p);
				if (p is Byte[])      return Convert<Guid?,Byte[]>     .I.C((Byte[])p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
			} }

		static CB<T,P> GetNullableGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Guid))        return (CB<T,P>)(object)(new NG_G        ());
			if (t == typeof(String))      return (CB<T,P>)(object)(new NG_S        ());

			// Nullable Types.
			//

			// SqlTypes.
			//
			if (t == typeof(SqlGuid))     return (CB<T,P>)(object)(new NG_dbG      ());
			if (t == typeof(SqlString))   return (CB<T,P>)(object)(new NG_dbS      ());
			if (t == typeof(SqlBinary))   return (CB<T,P>)(object)(new NG_dbBin    ());

			// Other Types.
			//
			if (t == typeof(Type))        return (CB<T,P>)(object)(new NG_T        ());
			if (t == typeof(Byte[]))      return (CB<T,P>)(object)(new NG_AU8      ());

			if (t == typeof(object))      return (CB<T,P>)(object)(new NG_O        ());

			return (CB<T,P>)(object)(new NG<P>());
		}

		#endregion

		#endregion
	}
}
