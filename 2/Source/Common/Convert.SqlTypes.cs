using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		#region SqlTypes

		#region SqlString


		// Scalar Types.
		//
		sealed class dbS_S       : CB<SqlString,String>     { public override SqlString C(String p)      { return p == null? SqlString.Null: p; } }

		sealed class dbS_I8      : CB<SqlString,SByte>      { public override SqlString C(SByte p)       { return p.ToString(); } }
		sealed class dbS_I16     : CB<SqlString,Int16>      { public override SqlString C(Int16 p)       { return p.ToString(); } }
		sealed class dbS_I32     : CB<SqlString,Int32>      { public override SqlString C(Int32 p)       { return p.ToString(); } }
		sealed class dbS_I64     : CB<SqlString,Int64>      { public override SqlString C(Int64 p)       { return p.ToString(); } }

		sealed class dbS_U8      : CB<SqlString,Byte>       { public override SqlString C(Byte p)        { return p.ToString(); } }
		sealed class dbS_U16     : CB<SqlString,UInt16>     { public override SqlString C(UInt16 p)      { return p.ToString(); } }
		sealed class dbS_U32     : CB<SqlString,UInt32>     { public override SqlString C(UInt32 p)      { return p.ToString(); } }
		sealed class dbS_U64     : CB<SqlString,UInt64>     { public override SqlString C(UInt64 p)      { return p.ToString(); } }

		sealed class dbS_R4      : CB<SqlString,Single>     { public override SqlString C(Single p)      { return p.ToString(); } }
		sealed class dbS_R8      : CB<SqlString,Double>     { public override SqlString C(Double p)      { return p.ToString(); } }

		sealed class dbS_B       : CB<SqlString,Boolean>    { public override SqlString C(Boolean p)     { return p.ToString(); } }
		sealed class dbS_D       : CB<SqlString,Decimal>    { public override SqlString C(Decimal p)     { return p.ToString(); } }
		sealed class dbS_C       : CB<SqlString,Char>       { public override SqlString C(Char p)        { return p.ToString(); } }
		sealed class dbS_TS      : CB<SqlString,TimeSpan>   { public override SqlString C(TimeSpan p)    { return p.ToString(); } }
		sealed class dbS_DT      : CB<SqlString,DateTime>   { public override SqlString C(DateTime p)    { return p.ToString(); } }
		sealed class dbS_G       : CB<SqlString,Guid>       { public override SqlString C(Guid p)        { return p.ToString(); } }
		sealed class dbS_AC      : CB<SqlString,Char[]>     { public override SqlString C(Char[] p)      { return new String(p); } }

		// Nullable Types.
		//
		sealed class dbS_NI8     : CB<SqlString,SByte?>     { public override SqlString C(SByte? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NI16    : CB<SqlString,Int16?>     { public override SqlString C(Int16? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NI32    : CB<SqlString,Int32?>     { public override SqlString C(Int32? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NI64    : CB<SqlString,Int64?>     { public override SqlString C(Int64? p)      { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class dbS_NU8     : CB<SqlString,Byte?>      { public override SqlString C(Byte? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NU16    : CB<SqlString,UInt16?>    { public override SqlString C(UInt16? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NU32    : CB<SqlString,UInt32?>    { public override SqlString C(UInt32? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NU64    : CB<SqlString,UInt64?>    { public override SqlString C(UInt64? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class dbS_NR4     : CB<SqlString,Single?>    { public override SqlString C(Single? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NR8     : CB<SqlString,Double?>    { public override SqlString C(Double? p)     { return p.HasValue? p.ToString(): SqlString.Null; } }

		sealed class dbS_NB      : CB<SqlString,Boolean?>   { public override SqlString C(Boolean? p)    { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_ND      : CB<SqlString,Decimal?>   { public override SqlString C(Decimal? p)    { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NC      : CB<SqlString,Char?>      { public override SqlString C(Char? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NTS     : CB<SqlString,TimeSpan?>  { public override SqlString C(TimeSpan? p)   { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NDT     : CB<SqlString,DateTime?>  { public override SqlString C(DateTime? p)   { return p.HasValue? p.ToString(): SqlString.Null; } }
		sealed class dbS_NG      : CB<SqlString,Guid?>      { public override SqlString C(Guid? p)       { return p.HasValue? p.ToString(): SqlString.Null; } }

		// SqlTypes.
		//

		sealed class dbS_dbU8    : CB<SqlString,SqlByte>    { public override SqlString C(SqlByte p)     { return p.ToSqlString(); } }
		sealed class dbS_dbI16   : CB<SqlString,SqlInt16>   { public override SqlString C(SqlInt16 p)    { return p.ToSqlString(); } }
		sealed class dbS_dbI32   : CB<SqlString,SqlInt32>   { public override SqlString C(SqlInt32 p)    { return p.ToSqlString(); } }
		sealed class dbS_dbI64   : CB<SqlString,SqlInt64>   { public override SqlString C(SqlInt64 p)    { return p.ToSqlString(); } }

		sealed class dbS_dbR4    : CB<SqlString,SqlSingle>  { public override SqlString C(SqlSingle p)   { return p.ToSqlString(); } }
		sealed class dbS_dbR8    : CB<SqlString,SqlDouble>  { public override SqlString C(SqlDouble p)   { return p.ToSqlString(); } }
		sealed class dbS_dbD     : CB<SqlString,SqlDecimal> { public override SqlString C(SqlDecimal p)  { return p.ToSqlString(); } }
		sealed class dbS_dbM     : CB<SqlString,SqlMoney>   { public override SqlString C(SqlMoney p)    { return p.ToSqlString(); } }

		sealed class dbS_dbB     : CB<SqlString,SqlBoolean> { public override SqlString C(SqlBoolean p)  { return p.ToSqlString(); } }
		sealed class dbS_dbAC    : CB<SqlString,SqlChars>   { public override SqlString C(SqlChars p)    { return p.ToSqlString(); } }
		sealed class dbS_dbG     : CB<SqlString,SqlGuid>    { public override SqlString C(SqlGuid p)     { return p.ToSqlString(); } }
		sealed class dbS_dbDT    : CB<SqlString,SqlDateTime>{ public override SqlString C(SqlDateTime p) { return p.ToSqlString(); } }
		sealed class dbS_dbBin   : CB<SqlString,SqlBinary>  { public override SqlString C(SqlBinary p)   { return p.IsNull? SqlString.Null: p.ToString(); } }
		sealed class dbS_T       : CB<SqlString,Type>       { public override SqlString C(Type p)        { return p == null? SqlString.Null: p.FullName; } }

		sealed class dbS_O         : CB<SqlString ,object>    { public override SqlString C(object p)   { return Convert.ToString(p); } }

		static CB<T, P> GetSqlStringConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbS_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbS_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbS_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbS_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbS_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbS_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbS_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbS_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbS_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbS_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbS_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbS_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbS_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbS_C       ());
			if (t == typeof(TimeSpan))    return (CB<T, P>)(object)(new dbS_TS      ());
			if (t == typeof(DateTime))    return (CB<T, P>)(object)(new dbS_DT      ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new dbS_G       ());
			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new dbS_AC      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbS_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbS_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbS_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbS_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbS_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbS_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbS_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbS_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbS_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbS_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbS_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbS_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbS_NC      ());
			if (t == typeof(TimeSpan?))   return (CB<T, P>)(object)(new dbS_NTS     ());
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new dbS_NDT     ());
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new dbS_NG      ());

			// SqlTypes.
			//

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbS_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbS_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbS_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbS_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbS_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbS_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbS_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbS_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbS_dbB     ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new dbS_dbAC    ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new dbS_dbG     ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbS_dbDT    ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new dbS_dbBin   ());
			if (t == typeof(Type))        return (CB<T, P>)(object)(new dbS_T       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbS_O       ());

			return (CB<T, P>)(object)Convert<SqlString, object>.I;
		}

		#endregion

		#region SqlByte


		// Scalar Types.
		//
		sealed class dbU8_U8     : CB<SqlByte,Byte>       { public override SqlByte C(Byte p)        { return p; } }
		sealed class dbU8_S      : CB<SqlByte,String>     { public override SqlByte C(String p)      { return p == null? SqlByte.Null: SqlByte.Parse(p); } }

		sealed class dbU8_I8     : CB<SqlByte,SByte>      { public override SqlByte C(SByte p)       { return Convert.ToByte(p); } }
		sealed class dbU8_I16    : CB<SqlByte,Int16>      { public override SqlByte C(Int16 p)       { return Convert.ToByte(p); } }
		sealed class dbU8_I32    : CB<SqlByte,Int32>      { public override SqlByte C(Int32 p)       { return Convert.ToByte(p); } }
		sealed class dbU8_I64    : CB<SqlByte,Int64>      { public override SqlByte C(Int64 p)       { return Convert.ToByte(p); } }

		sealed class dbU8_U16    : CB<SqlByte,UInt16>     { public override SqlByte C(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class dbU8_U32    : CB<SqlByte,UInt32>     { public override SqlByte C(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class dbU8_U64    : CB<SqlByte,UInt64>     { public override SqlByte C(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class dbU8_R4     : CB<SqlByte,Single>     { public override SqlByte C(Single p)      { return Convert.ToByte(p); } }
		sealed class dbU8_R8     : CB<SqlByte,Double>     { public override SqlByte C(Double p)      { return Convert.ToByte(p); } }

		sealed class dbU8_B      : CB<SqlByte,Boolean>    { public override SqlByte C(Boolean p)     { return Convert.ToByte(p); } }
		sealed class dbU8_D      : CB<SqlByte,Decimal>    { public override SqlByte C(Decimal p)     { return Convert.ToByte(p); } }
		sealed class dbU8_C      : CB<SqlByte,Char>       { public override SqlByte C(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class dbU8_NU8    : CB<SqlByte,Byte?>      { public override SqlByte C(Byte? p)       { return p.HasValue?                p.Value  : SqlByte.Null; } }
		sealed class dbU8_NI8    : CB<SqlByte,SByte?>     { public override SqlByte C(SByte? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NI16   : CB<SqlByte,Int16?>     { public override SqlByte C(Int16? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NI32   : CB<SqlByte,Int32?>     { public override SqlByte C(Int32? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NI64   : CB<SqlByte,Int64?>     { public override SqlByte C(Int64? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class dbU8_NU16   : CB<SqlByte,UInt16?>    { public override SqlByte C(UInt16? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NU32   : CB<SqlByte,UInt32?>    { public override SqlByte C(UInt32? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NU64   : CB<SqlByte,UInt64?>    { public override SqlByte C(UInt64? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class dbU8_NR4    : CB<SqlByte,Single?>    { public override SqlByte C(Single? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NR8    : CB<SqlByte,Double?>    { public override SqlByte C(Double? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		sealed class dbU8_NB     : CB<SqlByte,Boolean?>   { public override SqlByte C(Boolean? p)    { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_ND     : CB<SqlByte,Decimal?>   { public override SqlByte C(Decimal? p)    { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }
		sealed class dbU8_NC     : CB<SqlByte,Char?>      { public override SqlByte C(Char? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; } }

		// SqlTypes.
		//
		sealed class dbU8_dbS    : CB<SqlByte,SqlString>  { public override SqlByte C(SqlString p)   { return p.ToSqlByte(); } }

		sealed class dbU8_dbI16  : CB<SqlByte,SqlInt16>   { public override SqlByte C(SqlInt16 p)    { return p.ToSqlByte(); } }
		sealed class dbU8_dbI32  : CB<SqlByte,SqlInt32>   { public override SqlByte C(SqlInt32 p)    { return p.ToSqlByte(); } }
		sealed class dbU8_dbI64  : CB<SqlByte,SqlInt64>   { public override SqlByte C(SqlInt64 p)    { return p.ToSqlByte(); } }

		sealed class dbU8_dbR4   : CB<SqlByte,SqlSingle>  { public override SqlByte C(SqlSingle p)   { return p.ToSqlByte(); } }
		sealed class dbU8_dbR8   : CB<SqlByte,SqlDouble>  { public override SqlByte C(SqlDouble p)   { return p.ToSqlByte(); } }
		sealed class dbU8_dbD    : CB<SqlByte,SqlDecimal> { public override SqlByte C(SqlDecimal p)  { return p.ToSqlByte(); } }
		sealed class dbU8_dbM    : CB<SqlByte,SqlMoney>   { public override SqlByte C(SqlMoney p)    { return p.ToSqlByte(); } }

		sealed class dbU8_dbB    : CB<SqlByte,SqlBoolean> { public override SqlByte C(SqlBoolean p)  { return p.ToSqlByte(); } }
		sealed class dbU8_dbDT   : CB<SqlByte,SqlDateTime>{ public override SqlByte C(SqlDateTime p) { return p.IsNull? SqlByte.Null: Convert.ToByte(p.Value); } }

		sealed class dbU8_O         : CB<SqlByte ,object>    { public override SqlByte C(object p)   { return Convert.ToByte(p); } }

		static CB<T, P> GetSqlByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbU8_U8     ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbU8_S      ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbU8_I8     ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbU8_I16    ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbU8_I32    ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbU8_I64    ());

			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbU8_U16    ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbU8_U32    ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbU8_U64    ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbU8_R4     ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbU8_R8     ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbU8_B      ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbU8_D      ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbU8_C      ());

			// Nullable Types.
			//
			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbU8_NU8    ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbU8_NI8    ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbU8_NI16   ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbU8_NI32   ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbU8_NI64   ());

			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbU8_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbU8_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbU8_NU64   ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbU8_NR4    ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbU8_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbU8_NB     ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbU8_ND     ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbU8_NC     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbU8_dbS    ());

			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbU8_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbU8_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbU8_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbU8_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbU8_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbU8_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbU8_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbU8_dbB    ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbU8_dbDT   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbU8_O      ());

			return (CB<T, P>)(object)Convert<SqlByte, object>.I;
		}

		#endregion

		#region SqlInt16


		// Scalar Types.
		//
		sealed class dbI16_I16   : CB<SqlInt16,Int16>      { public override SqlInt16 C(Int16 p)       { return p; } }
		sealed class dbI16_S     : CB<SqlInt16,String>     { public override SqlInt16 C(String p)      { return p == null? SqlInt16.Null: SqlInt16.Parse(p); } }

		sealed class dbI16_I8    : CB<SqlInt16,SByte>      { public override SqlInt16 C(SByte p)       { return Convert.ToInt16(p); } }
		sealed class dbI16_I32   : CB<SqlInt16,Int32>      { public override SqlInt16 C(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class dbI16_I64   : CB<SqlInt16,Int64>      { public override SqlInt16 C(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class dbI16_U8    : CB<SqlInt16,Byte>       { public override SqlInt16 C(Byte p)        { return Convert.ToInt16(p); } }
		sealed class dbI16_U16   : CB<SqlInt16,UInt16>     { public override SqlInt16 C(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class dbI16_U32   : CB<SqlInt16,UInt32>     { public override SqlInt16 C(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class dbI16_U64   : CB<SqlInt16,UInt64>     { public override SqlInt16 C(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class dbI16_R4    : CB<SqlInt16,Single>     { public override SqlInt16 C(Single p)      { return Convert.ToInt16(p); } }
		sealed class dbI16_R8    : CB<SqlInt16,Double>     { public override SqlInt16 C(Double p)      { return Convert.ToInt16(p); } }

		sealed class dbI16_B     : CB<SqlInt16,Boolean>    { public override SqlInt16 C(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class dbI16_D     : CB<SqlInt16,Decimal>    { public override SqlInt16 C(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class dbI16_C     : CB<SqlInt16,Char>       { public override SqlInt16 C(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class dbI16_NI16  : CB<SqlInt16,Int16?>     { public override SqlInt16 C(Int16? p)      { return p.HasValue?                 p.Value  : SqlInt16.Null; } }
		sealed class dbI16_NI8   : CB<SqlInt16,SByte?>     { public override SqlInt16 C(SByte? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NI32  : CB<SqlInt16,Int32?>     { public override SqlInt16 C(Int32? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NI64  : CB<SqlInt16,Int64?>     { public override SqlInt16 C(Int64? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class dbI16_NU8   : CB<SqlInt16,Byte?>      { public override SqlInt16 C(Byte? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NU16  : CB<SqlInt16,UInt16?>    { public override SqlInt16 C(UInt16? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NU32  : CB<SqlInt16,UInt32?>    { public override SqlInt16 C(UInt32? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NU64  : CB<SqlInt16,UInt64?>    { public override SqlInt16 C(UInt64? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class dbI16_NR4   : CB<SqlInt16,Single?>    { public override SqlInt16 C(Single? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NR8   : CB<SqlInt16,Double?>    { public override SqlInt16 C(Double? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		sealed class dbI16_NB    : CB<SqlInt16,Boolean?>   { public override SqlInt16 C(Boolean? p)    { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_ND    : CB<SqlInt16,Decimal?>   { public override SqlInt16 C(Decimal? p)    { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }
		sealed class dbI16_NC    : CB<SqlInt16,Char?>      { public override SqlInt16 C(Char? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; } }

		// SqlTypes.
		//
		sealed class dbI16_dbS   : CB<SqlInt16,SqlString>  { public override SqlInt16 C(SqlString p)   { return p.ToSqlInt16(); } }

		sealed class dbI16_dbU8  : CB<SqlInt16,SqlByte>    { public override SqlInt16 C(SqlByte p)     { return p.ToSqlInt16(); } }
		sealed class dbI16_dbI32 : CB<SqlInt16,SqlInt32>   { public override SqlInt16 C(SqlInt32 p)    { return p.ToSqlInt16(); } }
		sealed class dbI16_dbI64 : CB<SqlInt16,SqlInt64>   { public override SqlInt16 C(SqlInt64 p)    { return p.ToSqlInt16(); } }

		sealed class dbI16_dbR4  : CB<SqlInt16,SqlSingle>  { public override SqlInt16 C(SqlSingle p)   { return p.ToSqlInt16(); } }
		sealed class dbI16_dbR8  : CB<SqlInt16,SqlDouble>  { public override SqlInt16 C(SqlDouble p)   { return p.ToSqlInt16(); } }
		sealed class dbI16_dbD   : CB<SqlInt16,SqlDecimal> { public override SqlInt16 C(SqlDecimal p)  { return p.ToSqlInt16(); } }
		sealed class dbI16_dbM   : CB<SqlInt16,SqlMoney>   { public override SqlInt16 C(SqlMoney p)    { return p.ToSqlInt16(); } }

		sealed class dbI16_dbB   : CB<SqlInt16,SqlBoolean> { public override SqlInt16 C(SqlBoolean p)  { return p.ToSqlInt16(); } }
		sealed class dbI16_dbDT  : CB<SqlInt16,SqlDateTime>{ public override SqlInt16 C(SqlDateTime p) { return p.IsNull? SqlInt16.Null: Convert.ToInt16(p.Value); } }

		sealed class dbI16_O         : CB<SqlInt16 ,object>    { public override SqlInt16 C(object p)   { return Convert.ToInt16(p); } }

		static CB<T, P> GetSqlInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbI16_I16   ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbI16_S     ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbI16_I8    ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbI16_I32   ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbI16_I64   ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbI16_U8    ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbI16_U16   ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbI16_U32   ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbI16_U64   ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbI16_R4    ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbI16_R8    ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbI16_B     ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbI16_D     ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbI16_C     ());

			// Nullable Types.
			//
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbI16_NI16  ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbI16_NI8   ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbI16_NI32  ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbI16_NI64  ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbI16_NU8   ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbI16_NU16  ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbI16_NU32  ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbI16_NU64  ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbI16_NR4   ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbI16_NR8   ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbI16_NB    ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbI16_ND    ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbI16_NC    ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbI16_dbS   ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbI16_dbU8  ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbI16_dbI32 ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbI16_dbI64 ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbI16_dbR4  ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbI16_dbR8  ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbI16_dbD   ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbI16_dbM   ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbI16_dbB   ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbI16_dbDT  ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbI16_O     ());

			return (CB<T, P>)(object)Convert<SqlInt16, object>.I;
		}

		#endregion

		#region SqlInt32


		// Scalar Types.
		//
		sealed class dbI32_I32   : CB<SqlInt32,Int32>      { public override SqlInt32 C(Int32 p)       { return p; } }
		sealed class dbI32_S     : CB<SqlInt32,String>     { public override SqlInt32 C(String p)      { return p == null? SqlInt32.Null: SqlInt32.Parse(p); } }

		sealed class dbI32_I8    : CB<SqlInt32,SByte>      { public override SqlInt32 C(SByte p)       { return Convert.ToInt32(p); } }
		sealed class dbI32_I16   : CB<SqlInt32,Int16>      { public override SqlInt32 C(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class dbI32_I64   : CB<SqlInt32,Int64>      { public override SqlInt32 C(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class dbI32_U8    : CB<SqlInt32,Byte>       { public override SqlInt32 C(Byte p)        { return Convert.ToInt32(p); } }
		sealed class dbI32_U16   : CB<SqlInt32,UInt16>     { public override SqlInt32 C(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class dbI32_U32   : CB<SqlInt32,UInt32>     { public override SqlInt32 C(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class dbI32_U64   : CB<SqlInt32,UInt64>     { public override SqlInt32 C(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class dbI32_R4    : CB<SqlInt32,Single>     { public override SqlInt32 C(Single p)      { return Convert.ToInt32(p); } }
		sealed class dbI32_R8    : CB<SqlInt32,Double>     { public override SqlInt32 C(Double p)      { return Convert.ToInt32(p); } }

		sealed class dbI32_B     : CB<SqlInt32,Boolean>    { public override SqlInt32 C(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class dbI32_D     : CB<SqlInt32,Decimal>    { public override SqlInt32 C(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class dbI32_C     : CB<SqlInt32,Char>       { public override SqlInt32 C(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class dbI32_NI32  : CB<SqlInt32,Int32?>     { public override SqlInt32 C(Int32? p)      { return p.HasValue?                 p.Value  : SqlInt32.Null; } }
		sealed class dbI32_NI8   : CB<SqlInt32,SByte?>     { public override SqlInt32 C(SByte? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NI16  : CB<SqlInt32,Int16?>     { public override SqlInt32 C(Int16? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NI64  : CB<SqlInt32,Int64?>     { public override SqlInt32 C(Int64? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class dbI32_NU8   : CB<SqlInt32,Byte?>      { public override SqlInt32 C(Byte? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NU16  : CB<SqlInt32,UInt16?>    { public override SqlInt32 C(UInt16? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NU32  : CB<SqlInt32,UInt32?>    { public override SqlInt32 C(UInt32? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NU64  : CB<SqlInt32,UInt64?>    { public override SqlInt32 C(UInt64? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class dbI32_NR4   : CB<SqlInt32,Single?>    { public override SqlInt32 C(Single? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NR8   : CB<SqlInt32,Double?>    { public override SqlInt32 C(Double? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		sealed class dbI32_NB    : CB<SqlInt32,Boolean?>   { public override SqlInt32 C(Boolean? p)    { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_ND    : CB<SqlInt32,Decimal?>   { public override SqlInt32 C(Decimal? p)    { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }
		sealed class dbI32_NC    : CB<SqlInt32,Char?>      { public override SqlInt32 C(Char? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; } }

		// SqlTypes.
		//
		sealed class dbI32_dbS   : CB<SqlInt32,SqlString>  { public override SqlInt32 C(SqlString p)   { return p.ToSqlInt32(); } }

		sealed class dbI32_dbU8  : CB<SqlInt32,SqlByte>    { public override SqlInt32 C(SqlByte p)     { return p.ToSqlInt32(); } }
		sealed class dbI32_dbI16 : CB<SqlInt32,SqlInt16>   { public override SqlInt32 C(SqlInt16 p)    { return p.ToSqlInt32(); } }
		sealed class dbI32_dbI64 : CB<SqlInt32,SqlInt64>   { public override SqlInt32 C(SqlInt64 p)    { return p.ToSqlInt32(); } }

		sealed class dbI32_dbR4  : CB<SqlInt32,SqlSingle>  { public override SqlInt32 C(SqlSingle p)   { return p.ToSqlInt32(); } }
		sealed class dbI32_dbR8  : CB<SqlInt32,SqlDouble>  { public override SqlInt32 C(SqlDouble p)   { return p.ToSqlInt32(); } }
		sealed class dbI32_dbD   : CB<SqlInt32,SqlDecimal> { public override SqlInt32 C(SqlDecimal p)  { return p.ToSqlInt32(); } }
		sealed class dbI32_dbM   : CB<SqlInt32,SqlMoney>   { public override SqlInt32 C(SqlMoney p)    { return p.ToSqlInt32(); } }

		sealed class dbI32_dbB   : CB<SqlInt32,SqlBoolean> { public override SqlInt32 C(SqlBoolean p)  { return p.ToSqlInt32(); } }
		sealed class dbI32_dbDT  : CB<SqlInt32,SqlDateTime>{ public override SqlInt32 C(SqlDateTime p) { return p.IsNull? SqlInt32.Null: Convert.ToInt32(p.Value); } }

		sealed class dbI32_O         : CB<SqlInt32 ,object>    { public override SqlInt32 C(object p)   { return Convert.ToInt32(p); } }

		static CB<T, P> GetSqlInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbI32_I32   ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbI32_S     ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbI32_I8    ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbI32_I16   ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbI32_I64   ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbI32_U8    ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbI32_U16   ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbI32_U32   ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbI32_U64   ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbI32_R4    ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbI32_R8    ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbI32_B     ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbI32_D     ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbI32_C     ());

			// Nullable Types.
			//
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbI32_NI32  ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbI32_NI8   ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbI32_NI16  ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbI32_NI64  ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbI32_NU8   ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbI32_NU16  ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbI32_NU32  ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbI32_NU64  ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbI32_NR4   ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbI32_NR8   ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbI32_NB    ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbI32_ND    ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbI32_NC    ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbI32_dbS   ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbI32_dbU8  ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbI32_dbI16 ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbI32_dbI64 ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbI32_dbR4  ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbI32_dbR8  ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbI32_dbD   ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbI32_dbM   ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbI32_dbB   ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbI32_dbDT  ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbI32_O     ());

			return (CB<T, P>)(object)Convert<SqlInt32, object>.I;
		}

		#endregion

		#region SqlInt64


		// Scalar Types.
		//
		sealed class dbI64_I64   : CB<SqlInt64,Int64>      { public override SqlInt64 C(Int64 p)       { return p; } }
		sealed class dbI64_S     : CB<SqlInt64,String>     { public override SqlInt64 C(String p)      { return p == null? SqlInt64.Null: SqlInt64.Parse(p); } }

		sealed class dbI64_I8    : CB<SqlInt64,SByte>      { public override SqlInt64 C(SByte p)       { return Convert.ToInt64(p); } }
		sealed class dbI64_I16   : CB<SqlInt64,Int16>      { public override SqlInt64 C(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class dbI64_I32   : CB<SqlInt64,Int32>      { public override SqlInt64 C(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class dbI64_U8    : CB<SqlInt64,Byte>       { public override SqlInt64 C(Byte p)        { return Convert.ToInt64(p); } }
		sealed class dbI64_U16   : CB<SqlInt64,UInt16>     { public override SqlInt64 C(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class dbI64_U32   : CB<SqlInt64,UInt32>     { public override SqlInt64 C(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class dbI64_U64   : CB<SqlInt64,UInt64>     { public override SqlInt64 C(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class dbI64_R4    : CB<SqlInt64,Single>     { public override SqlInt64 C(Single p)      { return Convert.ToInt64(p); } }
		sealed class dbI64_R8    : CB<SqlInt64,Double>     { public override SqlInt64 C(Double p)      { return Convert.ToInt64(p); } }

		sealed class dbI64_B     : CB<SqlInt64,Boolean>    { public override SqlInt64 C(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class dbI64_D     : CB<SqlInt64,Decimal>    { public override SqlInt64 C(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class dbI64_C     : CB<SqlInt64,Char>       { public override SqlInt64 C(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class dbI64_NI64  : CB<SqlInt64,Int64?>     { public override SqlInt64 C(Int64? p)      { return p.HasValue?                 p.Value  : SqlInt64.Null; } }
		sealed class dbI64_NI8   : CB<SqlInt64,SByte?>     { public override SqlInt64 C(SByte? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NI16  : CB<SqlInt64,Int16?>     { public override SqlInt64 C(Int16? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NI32  : CB<SqlInt64,Int32?>     { public override SqlInt64 C(Int32? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class dbI64_NU8   : CB<SqlInt64,Byte?>      { public override SqlInt64 C(Byte? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NU16  : CB<SqlInt64,UInt16?>    { public override SqlInt64 C(UInt16? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NU32  : CB<SqlInt64,UInt32?>    { public override SqlInt64 C(UInt32? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NU64  : CB<SqlInt64,UInt64?>    { public override SqlInt64 C(UInt64? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class dbI64_NR4   : CB<SqlInt64,Single?>    { public override SqlInt64 C(Single? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NR8   : CB<SqlInt64,Double?>    { public override SqlInt64 C(Double? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		sealed class dbI64_NB    : CB<SqlInt64,Boolean?>   { public override SqlInt64 C(Boolean? p)    { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_ND    : CB<SqlInt64,Decimal?>   { public override SqlInt64 C(Decimal? p)    { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }
		sealed class dbI64_NC    : CB<SqlInt64,Char?>      { public override SqlInt64 C(Char? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; } }

		// SqlTypes.
		//
		sealed class dbI64_dbS   : CB<SqlInt64,SqlString>  { public override SqlInt64 C(SqlString p)   { return p.ToSqlInt64(); } }

		sealed class dbI64_dbU8  : CB<SqlInt64,SqlByte>    { public override SqlInt64 C(SqlByte p)     { return p.ToSqlInt64(); } }
		sealed class dbI64_dbI16 : CB<SqlInt64,SqlInt16>   { public override SqlInt64 C(SqlInt16 p)    { return p.ToSqlInt64(); } }
		sealed class dbI64_dbI32 : CB<SqlInt64,SqlInt32>   { public override SqlInt64 C(SqlInt32 p)    { return p.ToSqlInt64(); } }

		sealed class dbI64_dbR4  : CB<SqlInt64,SqlSingle>  { public override SqlInt64 C(SqlSingle p)   { return p.ToSqlInt64(); } }
		sealed class dbI64_dbR8  : CB<SqlInt64,SqlDouble>  { public override SqlInt64 C(SqlDouble p)   { return p.ToSqlInt64(); } }
		sealed class dbI64_dbD   : CB<SqlInt64,SqlDecimal> { public override SqlInt64 C(SqlDecimal p)  { return p.ToSqlInt64(); } }
		sealed class dbI64_dbM   : CB<SqlInt64,SqlMoney>   { public override SqlInt64 C(SqlMoney p)    { return p.ToSqlInt64(); } }

		sealed class dbI64_dbB   : CB<SqlInt64,SqlBoolean> { public override SqlInt64 C(SqlBoolean p)  { return p.ToSqlInt64(); } }
		sealed class dbI64_dbDT  : CB<SqlInt64,SqlDateTime>{ public override SqlInt64 C(SqlDateTime p) { return p.IsNull? SqlInt64.Null: Convert.ToInt64(p.Value); } }

		sealed class dbI64_O         : CB<SqlInt64 ,object>    { public override SqlInt64 C(object p)   { return Convert.ToInt64(p); } }

		static CB<T, P> GetSqlInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbI64_I64   ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbI64_S     ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbI64_I8    ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbI64_I16   ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbI64_I32   ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbI64_U8    ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbI64_U16   ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbI64_U32   ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbI64_U64   ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbI64_R4    ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbI64_R8    ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbI64_B     ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbI64_D     ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbI64_C     ());

			// Nullable Types.
			//
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbI64_NI64  ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbI64_NI8   ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbI64_NI16  ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbI64_NI32  ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbI64_NU8   ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbI64_NU16  ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbI64_NU32  ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbI64_NU64  ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbI64_NR4   ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbI64_NR8   ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbI64_NB    ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbI64_ND    ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbI64_NC    ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbI64_dbS   ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbI64_dbU8  ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbI64_dbI16 ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbI64_dbI32 ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbI64_dbR4  ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbI64_dbR8  ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbI64_dbD   ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbI64_dbM   ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbI64_dbB   ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbI64_dbDT  ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbI64_O     ());

			return (CB<T, P>)(object)Convert<SqlInt64, object>.I;
		}

		#endregion

		#region SqlSingle


		// Scalar Types.
		//
		sealed class dbR4_R4     : CB<SqlSingle,Single>     { public override SqlSingle C(Single p)      { return p; } }
		sealed class dbR4_S      : CB<SqlSingle,String>     { public override SqlSingle C(String p)      { return p == null? SqlSingle.Null: SqlSingle.Parse(p); } }

		sealed class dbR4_I8     : CB<SqlSingle,SByte>      { public override SqlSingle C(SByte p)       { return Convert.ToSingle(p); } }
		sealed class dbR4_I16    : CB<SqlSingle,Int16>      { public override SqlSingle C(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class dbR4_I32    : CB<SqlSingle,Int32>      { public override SqlSingle C(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class dbR4_I64    : CB<SqlSingle,Int64>      { public override SqlSingle C(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class dbR4_U8     : CB<SqlSingle,Byte>       { public override SqlSingle C(Byte p)        { return Convert.ToSingle(p); } }
		sealed class dbR4_U16    : CB<SqlSingle,UInt16>     { public override SqlSingle C(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class dbR4_U32    : CB<SqlSingle,UInt32>     { public override SqlSingle C(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class dbR4_U64    : CB<SqlSingle,UInt64>     { public override SqlSingle C(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class dbR4_R8     : CB<SqlSingle,Double>     { public override SqlSingle C(Double p)      { return Convert.ToSingle(p); } }

		sealed class dbR4_B      : CB<SqlSingle,Boolean>    { public override SqlSingle C(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class dbR4_D      : CB<SqlSingle,Decimal>    { public override SqlSingle C(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class dbR4_NR4    : CB<SqlSingle,Single?>    { public override SqlSingle C(Single? p)     { return p.HasValue?                  p.Value  : SqlSingle.Null; } }
		sealed class dbR4_NI8    : CB<SqlSingle,SByte?>     { public override SqlSingle C(SByte? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NI16   : CB<SqlSingle,Int16?>     { public override SqlSingle C(Int16? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NI32   : CB<SqlSingle,Int32?>     { public override SqlSingle C(Int32? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NI64   : CB<SqlSingle,Int64?>     { public override SqlSingle C(Int64? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class dbR4_NU8    : CB<SqlSingle,Byte?>      { public override SqlSingle C(Byte? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NU16   : CB<SqlSingle,UInt16?>    { public override SqlSingle C(UInt16? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NU32   : CB<SqlSingle,UInt32?>    { public override SqlSingle C(UInt32? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_NU64   : CB<SqlSingle,UInt64?>    { public override SqlSingle C(UInt64? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class dbR4_NR8    : CB<SqlSingle,Double?>    { public override SqlSingle C(Double? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		sealed class dbR4_NB     : CB<SqlSingle,Boolean?>   { public override SqlSingle C(Boolean? p)    { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }
		sealed class dbR4_ND     : CB<SqlSingle,Decimal?>   { public override SqlSingle C(Decimal? p)    { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; } }

		// SqlTypes.
		//
		sealed class dbR4_dbS    : CB<SqlSingle,SqlString>  { public override SqlSingle C(SqlString p)   { return p.ToSqlSingle(); } }

		sealed class dbR4_dbU8   : CB<SqlSingle,SqlByte>    { public override SqlSingle C(SqlByte p)     { return p.ToSqlSingle(); } }
		sealed class dbR4_dbI16  : CB<SqlSingle,SqlInt16>   { public override SqlSingle C(SqlInt16 p)    { return p.ToSqlSingle(); } }
		sealed class dbR4_dbI32  : CB<SqlSingle,SqlInt32>   { public override SqlSingle C(SqlInt32 p)    { return p.ToSqlSingle(); } }
		sealed class dbR4_dbI64  : CB<SqlSingle,SqlInt64>   { public override SqlSingle C(SqlInt64 p)    { return p.ToSqlSingle(); } }

		sealed class dbR4_dbR8   : CB<SqlSingle,SqlDouble>  { public override SqlSingle C(SqlDouble p)   { return p.ToSqlSingle(); } }
		sealed class dbR4_dbD    : CB<SqlSingle,SqlDecimal> { public override SqlSingle C(SqlDecimal p)  { return p.ToSqlSingle(); } }
		sealed class dbR4_dbM    : CB<SqlSingle,SqlMoney>   { public override SqlSingle C(SqlMoney p)    { return p.ToSqlSingle(); } }

		sealed class dbR4_dbB    : CB<SqlSingle,SqlBoolean> { public override SqlSingle C(SqlBoolean p)  { return p.ToSqlSingle(); } }
		sealed class dbR4_dbDT   : CB<SqlSingle,SqlDateTime>{ public override SqlSingle C(SqlDateTime p) { return p.IsNull? SqlSingle.Null: Convert.ToSingle(p.Value); } }

		sealed class dbR4_O         : CB<SqlSingle ,object>    { public override SqlSingle C(object p)   { return Convert.ToSingle(p); } }

		static CB<T, P> GetSqlSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbR4_R4     ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbR4_S      ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbR4_I8     ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbR4_I16    ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbR4_I32    ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbR4_I64    ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbR4_U8     ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbR4_U16    ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbR4_U32    ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbR4_U64    ());

			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbR4_R8     ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbR4_B      ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbR4_D      ());

			// Nullable Types.
			//
			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbR4_NR4    ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbR4_NI8    ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbR4_NI16   ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbR4_NI32   ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbR4_NI64   ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbR4_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbR4_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbR4_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbR4_NU64   ());

			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbR4_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbR4_NB     ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbR4_ND     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbR4_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbR4_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbR4_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbR4_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbR4_dbI64  ());

			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbR4_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbR4_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbR4_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbR4_dbB    ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbR4_dbDT   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbR4_O      ());

			return (CB<T, P>)(object)Convert<SqlSingle, object>.I;
		}

		#endregion

		#region SqlDouble


		// Scalar Types.
		//
		sealed class dbR8_R8     : CB<SqlDouble,Double>     { public override SqlDouble C(Double p)      { return p; } }
		sealed class dbR8_S      : CB<SqlDouble,String>     { public override SqlDouble C(String p)      { return p == null? SqlDouble.Null: SqlDouble.Parse(p); } }

		sealed class dbR8_I8     : CB<SqlDouble,SByte>      { public override SqlDouble C(SByte p)       { return Convert.ToDouble(p); } }
		sealed class dbR8_I16    : CB<SqlDouble,Int16>      { public override SqlDouble C(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class dbR8_I32    : CB<SqlDouble,Int32>      { public override SqlDouble C(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class dbR8_I64    : CB<SqlDouble,Int64>      { public override SqlDouble C(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class dbR8_U8     : CB<SqlDouble,Byte>       { public override SqlDouble C(Byte p)        { return Convert.ToDouble(p); } }
		sealed class dbR8_U16    : CB<SqlDouble,UInt16>     { public override SqlDouble C(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class dbR8_U32    : CB<SqlDouble,UInt32>     { public override SqlDouble C(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class dbR8_U64    : CB<SqlDouble,UInt64>     { public override SqlDouble C(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class dbR8_R4     : CB<SqlDouble,Single>     { public override SqlDouble C(Single p)      { return Convert.ToDouble(p); } }

		sealed class dbR8_B      : CB<SqlDouble,Boolean>    { public override SqlDouble C(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class dbR8_D      : CB<SqlDouble,Decimal>    { public override SqlDouble C(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class dbR8_NR8    : CB<SqlDouble,Double?>    { public override SqlDouble C(Double? p)     { return p.HasValue?                  p.Value  : SqlDouble.Null; } }
		sealed class dbR8_NI8    : CB<SqlDouble,SByte?>     { public override SqlDouble C(SByte? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NI16   : CB<SqlDouble,Int16?>     { public override SqlDouble C(Int16? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NI32   : CB<SqlDouble,Int32?>     { public override SqlDouble C(Int32? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NI64   : CB<SqlDouble,Int64?>     { public override SqlDouble C(Int64? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class dbR8_NU8    : CB<SqlDouble,Byte?>      { public override SqlDouble C(Byte? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NU16   : CB<SqlDouble,UInt16?>    { public override SqlDouble C(UInt16? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NU32   : CB<SqlDouble,UInt32?>    { public override SqlDouble C(UInt32? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_NU64   : CB<SqlDouble,UInt64?>    { public override SqlDouble C(UInt64? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class dbR8_NR4    : CB<SqlDouble,Single?>    { public override SqlDouble C(Single? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		sealed class dbR8_NB     : CB<SqlDouble,Boolean?>   { public override SqlDouble C(Boolean? p)    { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }
		sealed class dbR8_ND     : CB<SqlDouble,Decimal?>   { public override SqlDouble C(Decimal? p)    { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; } }

		// SqlTypes.
		//
		sealed class dbR8_dbS    : CB<SqlDouble,SqlString>  { public override SqlDouble C(SqlString p)   { return p.ToSqlDouble(); } }

		sealed class dbR8_dbU8   : CB<SqlDouble,SqlByte>    { public override SqlDouble C(SqlByte p)     { return p.ToSqlDouble(); } }
		sealed class dbR8_dbI16  : CB<SqlDouble,SqlInt16>   { public override SqlDouble C(SqlInt16 p)    { return p.ToSqlDouble(); } }
		sealed class dbR8_dbI32  : CB<SqlDouble,SqlInt32>   { public override SqlDouble C(SqlInt32 p)    { return p.ToSqlDouble(); } }
		sealed class dbR8_dbI64  : CB<SqlDouble,SqlInt64>   { public override SqlDouble C(SqlInt64 p)    { return p.ToSqlDouble(); } }

		sealed class dbR8_dbR4   : CB<SqlDouble,SqlSingle>  { public override SqlDouble C(SqlSingle p)   { return p.ToSqlDouble(); } }
		sealed class dbR8_dbD    : CB<SqlDouble,SqlDecimal> { public override SqlDouble C(SqlDecimal p)  { return p.ToSqlDouble(); } }
		sealed class dbR8_dbM    : CB<SqlDouble,SqlMoney>   { public override SqlDouble C(SqlMoney p)    { return p.ToSqlDouble(); } }

		sealed class dbR8_dbB    : CB<SqlDouble,SqlBoolean> { public override SqlDouble C(SqlBoolean p)  { return p.ToSqlDouble(); } }
		sealed class dbR8_dbDT   : CB<SqlDouble,SqlDateTime>{ public override SqlDouble C(SqlDateTime p) { return p.IsNull? SqlDouble.Null: Convert.ToDouble(p.Value); } }

		sealed class dbR8_O         : CB<SqlDouble ,object>    { public override SqlDouble C(object p)   { return Convert.ToDouble(p); } }

		static CB<T, P> GetSqlDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbR8_R8     ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbR8_S      ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbR8_I8     ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbR8_I16    ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbR8_I32    ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbR8_I64    ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbR8_U8     ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbR8_U16    ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbR8_U32    ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbR8_U64    ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbR8_R4     ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbR8_B      ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbR8_D      ());

			// Nullable Types.
			//
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbR8_NR8    ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbR8_NI8    ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbR8_NI16   ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbR8_NI32   ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbR8_NI64   ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbR8_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbR8_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbR8_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbR8_NU64   ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbR8_NR4    ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbR8_NB     ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbR8_ND     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbR8_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbR8_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbR8_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbR8_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbR8_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbR8_dbR4   ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbR8_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbR8_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbR8_dbB    ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbR8_dbDT   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbR8_O      ());

			return (CB<T, P>)(object)Convert<SqlDouble, object>.I;
		}

		#endregion

		#region SqlDecimal


		// Scalar Types.
		//
		sealed class dbD_D       : CB<SqlDecimal,Decimal>    { public override SqlDecimal C(Decimal p)     { return p; } }
		sealed class dbD_S       : CB<SqlDecimal,String>     { public override SqlDecimal C(String p)      { return p == null? SqlDecimal.Null: SqlDecimal.Parse(p); } }

		sealed class dbD_I8      : CB<SqlDecimal,SByte>      { public override SqlDecimal C(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class dbD_I16     : CB<SqlDecimal,Int16>      { public override SqlDecimal C(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class dbD_I32     : CB<SqlDecimal,Int32>      { public override SqlDecimal C(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class dbD_I64     : CB<SqlDecimal,Int64>      { public override SqlDecimal C(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class dbD_U8      : CB<SqlDecimal,Byte>       { public override SqlDecimal C(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class dbD_U16     : CB<SqlDecimal,UInt16>     { public override SqlDecimal C(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class dbD_U32     : CB<SqlDecimal,UInt32>     { public override SqlDecimal C(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class dbD_U64     : CB<SqlDecimal,UInt64>     { public override SqlDecimal C(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class dbD_R4      : CB<SqlDecimal,Single>     { public override SqlDecimal C(Single p)      { return Convert.ToDecimal(p); } }
		sealed class dbD_R8      : CB<SqlDecimal,Double>     { public override SqlDecimal C(Double p)      { return Convert.ToDecimal(p); } }

		sealed class dbD_B       : CB<SqlDecimal,Boolean>    { public override SqlDecimal C(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class dbD_ND      : CB<SqlDecimal,Decimal?>   { public override SqlDecimal C(Decimal? p)    { return p.HasValue?                   p.Value  : SqlDecimal.Null; } }
		sealed class dbD_NI8     : CB<SqlDecimal,SByte?>     { public override SqlDecimal C(SByte? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NI16    : CB<SqlDecimal,Int16?>     { public override SqlDecimal C(Int16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NI32    : CB<SqlDecimal,Int32?>     { public override SqlDecimal C(Int32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NI64    : CB<SqlDecimal,Int64?>     { public override SqlDecimal C(Int64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class dbD_NU8     : CB<SqlDecimal,Byte?>      { public override SqlDecimal C(Byte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NU16    : CB<SqlDecimal,UInt16?>    { public override SqlDecimal C(UInt16? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NU32    : CB<SqlDecimal,UInt32?>    { public override SqlDecimal C(UInt32? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NU64    : CB<SqlDecimal,UInt64?>    { public override SqlDecimal C(UInt64? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class dbD_NR4     : CB<SqlDecimal,Single?>    { public override SqlDecimal C(Single? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }
		sealed class dbD_NR8     : CB<SqlDecimal,Double?>    { public override SqlDecimal C(Double? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		sealed class dbD_NB      : CB<SqlDecimal,Boolean?>   { public override SqlDecimal C(Boolean? p)    { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; } }

		// SqlTypes.
		//
		sealed class dbD_dbS     : CB<SqlDecimal,SqlString>  { public override SqlDecimal C(SqlString p)   { return p.ToSqlDecimal(); } }

		sealed class dbD_dbU8    : CB<SqlDecimal,SqlByte>    { public override SqlDecimal C(SqlByte p)     { return p.ToSqlDecimal(); } }
		sealed class dbD_dbI16   : CB<SqlDecimal,SqlInt16>   { public override SqlDecimal C(SqlInt16 p)    { return p.ToSqlDecimal(); } }
		sealed class dbD_dbI32   : CB<SqlDecimal,SqlInt32>   { public override SqlDecimal C(SqlInt32 p)    { return p.ToSqlDecimal(); } }
		sealed class dbD_dbI64   : CB<SqlDecimal,SqlInt64>   { public override SqlDecimal C(SqlInt64 p)    { return p.ToSqlDecimal(); } }

		sealed class dbD_dbR4    : CB<SqlDecimal,SqlSingle>  { public override SqlDecimal C(SqlSingle p)   { return p.ToSqlDecimal(); } }
		sealed class dbD_dbR8    : CB<SqlDecimal,SqlDouble>  { public override SqlDecimal C(SqlDouble p)   { return p.ToSqlDecimal(); } }
		sealed class dbD_dbM     : CB<SqlDecimal,SqlMoney>   { public override SqlDecimal C(SqlMoney p)    { return p.ToSqlDecimal(); } }

		sealed class dbD_dbB     : CB<SqlDecimal,SqlBoolean> { public override SqlDecimal C(SqlBoolean p)  { return p.ToSqlDecimal(); } }
		sealed class dbD_dbDT    : CB<SqlDecimal,SqlDateTime>{ public override SqlDecimal C(SqlDateTime p) { return p.IsNull? SqlDecimal.Null: Convert.ToDecimal(p.Value); } }

		sealed class dbD_O         : CB<SqlDecimal ,object>    { public override SqlDecimal C(object p)   { return Convert.ToDecimal(p); } }

		static CB<T, P> GetSqlDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbD_D       ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbD_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbD_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbD_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbD_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbD_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbD_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbD_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbD_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbD_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbD_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbD_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbD_B       ());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbD_ND      ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbD_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbD_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbD_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbD_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbD_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbD_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbD_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbD_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbD_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbD_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbD_NB      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbD_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbD_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbD_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbD_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbD_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbD_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbD_dbR8    ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbD_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbD_dbB     ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbD_dbDT    ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbD_O       ());

			return (CB<T, P>)(object)Convert<SqlDecimal, object>.I;
		}

		#endregion

		#region SqlMoney


		// Scalar Types.
		//
		sealed class dbM_D       : CB<SqlMoney,Decimal>    { public override SqlMoney C(Decimal p)     { return p; } }
		sealed class dbM_S       : CB<SqlMoney,String>     { public override SqlMoney C(String p)      { return p == null? SqlMoney.Null: SqlMoney.Parse(p); } }

		sealed class dbM_I8      : CB<SqlMoney,SByte>      { public override SqlMoney C(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class dbM_I16     : CB<SqlMoney,Int16>      { public override SqlMoney C(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class dbM_I32     : CB<SqlMoney,Int32>      { public override SqlMoney C(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class dbM_I64     : CB<SqlMoney,Int64>      { public override SqlMoney C(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class dbM_U8      : CB<SqlMoney,Byte>       { public override SqlMoney C(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class dbM_U16     : CB<SqlMoney,UInt16>     { public override SqlMoney C(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class dbM_U32     : CB<SqlMoney,UInt32>     { public override SqlMoney C(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class dbM_U64     : CB<SqlMoney,UInt64>     { public override SqlMoney C(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class dbM_R4      : CB<SqlMoney,Single>     { public override SqlMoney C(Single p)      { return Convert.ToDecimal(p); } }
		sealed class dbM_R8      : CB<SqlMoney,Double>     { public override SqlMoney C(Double p)      { return Convert.ToDecimal(p); } }

		sealed class dbM_B       : CB<SqlMoney,Boolean>    { public override SqlMoney C(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class dbM_ND      : CB<SqlMoney,Decimal?>   { public override SqlMoney C(Decimal? p)    { return p.HasValue?                   p.Value  : SqlMoney.Null; } }
		sealed class dbM_NI8     : CB<SqlMoney,SByte?>     { public override SqlMoney C(SByte? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NI16    : CB<SqlMoney,Int16?>     { public override SqlMoney C(Int16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NI32    : CB<SqlMoney,Int32?>     { public override SqlMoney C(Int32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NI64    : CB<SqlMoney,Int64?>     { public override SqlMoney C(Int64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class dbM_NU8     : CB<SqlMoney,Byte?>      { public override SqlMoney C(Byte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NU16    : CB<SqlMoney,UInt16?>    { public override SqlMoney C(UInt16? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NU32    : CB<SqlMoney,UInt32?>    { public override SqlMoney C(UInt32? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NU64    : CB<SqlMoney,UInt64?>    { public override SqlMoney C(UInt64? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class dbM_NR4     : CB<SqlMoney,Single?>    { public override SqlMoney C(Single? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }
		sealed class dbM_NR8     : CB<SqlMoney,Double?>    { public override SqlMoney C(Double? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		sealed class dbM_NB      : CB<SqlMoney,Boolean?>   { public override SqlMoney C(Boolean? p)    { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; } }

		// SqlTypes.
		//
		sealed class dbM_dbS     : CB<SqlMoney,SqlString>  { public override SqlMoney C(SqlString p)   { return p.ToSqlMoney(); } }

		sealed class dbM_dbU8    : CB<SqlMoney,SqlByte>    { public override SqlMoney C(SqlByte p)     { return p.ToSqlMoney(); } }
		sealed class dbM_dbI16   : CB<SqlMoney,SqlInt16>   { public override SqlMoney C(SqlInt16 p)    { return p.ToSqlMoney(); } }
		sealed class dbM_dbI32   : CB<SqlMoney,SqlInt32>   { public override SqlMoney C(SqlInt32 p)    { return p.ToSqlMoney(); } }
		sealed class dbM_dbI64   : CB<SqlMoney,SqlInt64>   { public override SqlMoney C(SqlInt64 p)    { return p.ToSqlMoney(); } }

		sealed class dbM_dbR4    : CB<SqlMoney,SqlSingle>  { public override SqlMoney C(SqlSingle p)   { return p.ToSqlMoney(); } }
		sealed class dbM_dbR8    : CB<SqlMoney,SqlDouble>  { public override SqlMoney C(SqlDouble p)   { return p.ToSqlMoney(); } }
		sealed class dbM_dbD     : CB<SqlMoney,SqlDecimal> { public override SqlMoney C(SqlDecimal p)  { return p.ToSqlMoney(); } }

		sealed class dbM_dbB     : CB<SqlMoney,SqlBoolean> { public override SqlMoney C(SqlBoolean p)  { return p.ToSqlMoney(); } }
		sealed class dbM_dbDT    : CB<SqlMoney,SqlDateTime>{ public override SqlMoney C(SqlDateTime p) { return p.IsNull? SqlMoney.Null: Convert.ToDecimal(p.Value); } }

		sealed class dbM_O         : CB<SqlMoney ,object>    { public override SqlMoney C(object p)   { return Convert.ToDecimal(p); } }

		static CB<T, P> GetSqlMoneyConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbM_D       ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbM_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbM_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbM_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbM_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbM_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbM_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbM_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbM_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbM_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbM_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbM_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbM_B       ());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbM_ND      ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbM_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbM_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbM_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbM_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbM_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbM_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbM_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbM_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbM_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbM_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbM_NB      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbM_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbM_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbM_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbM_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbM_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbM_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbM_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbM_dbD     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbM_dbB     ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbM_dbDT    ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbM_O       ());

			return (CB<T, P>)(object)Convert<SqlMoney, object>.I;
		}

		#endregion

		#region SqlBoolean


		// Scalar Types.
		//
		sealed class dbB_B       : CB<SqlBoolean,Boolean>    { public override SqlBoolean C(Boolean p)     { return p; } }
		sealed class dbB_S       : CB<SqlBoolean,String>     { public override SqlBoolean C(String p)      { return p == null? SqlBoolean.Null: SqlBoolean.Parse(p); } }

		sealed class dbB_I8      : CB<SqlBoolean,SByte>      { public override SqlBoolean C(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class dbB_I16     : CB<SqlBoolean,Int16>      { public override SqlBoolean C(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class dbB_I32     : CB<SqlBoolean,Int32>      { public override SqlBoolean C(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class dbB_I64     : CB<SqlBoolean,Int64>      { public override SqlBoolean C(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class dbB_U8      : CB<SqlBoolean,Byte>       { public override SqlBoolean C(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class dbB_U16     : CB<SqlBoolean,UInt16>     { public override SqlBoolean C(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class dbB_U32     : CB<SqlBoolean,UInt32>     { public override SqlBoolean C(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class dbB_U64     : CB<SqlBoolean,UInt64>     { public override SqlBoolean C(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class dbB_R4      : CB<SqlBoolean,Single>     { public override SqlBoolean C(Single p)      { return Convert.ToBoolean(p); } }
		sealed class dbB_R8      : CB<SqlBoolean,Double>     { public override SqlBoolean C(Double p)      { return Convert.ToBoolean(p); } }

		sealed class dbB_D       : CB<SqlBoolean,Decimal>    { public override SqlBoolean C(Decimal p)     { return Convert.ToBoolean(p); } }
		sealed class dbB_C       : CB<SqlBoolean,Char>       { public override SqlBoolean C(Char p)        { return Convert<Boolean,Char>.I.C(p); } }

		// Nullable Types.
		//
		sealed class dbB_NB      : CB<SqlBoolean,Boolean?>   { public override SqlBoolean C(Boolean? p)    { return p.HasValue?                   p.Value  : SqlBoolean.Null; } }
		sealed class dbB_NI8     : CB<SqlBoolean,SByte?>     { public override SqlBoolean C(SByte? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NI16    : CB<SqlBoolean,Int16?>     { public override SqlBoolean C(Int16? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NI32    : CB<SqlBoolean,Int32?>     { public override SqlBoolean C(Int32? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NI64    : CB<SqlBoolean,Int64?>     { public override SqlBoolean C(Int64? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class dbB_NU8     : CB<SqlBoolean,Byte?>      { public override SqlBoolean C(Byte? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NU16    : CB<SqlBoolean,UInt16?>    { public override SqlBoolean C(UInt16? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NU32    : CB<SqlBoolean,UInt32?>    { public override SqlBoolean C(UInt32? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NU64    : CB<SqlBoolean,UInt64?>    { public override SqlBoolean C(UInt64? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class dbB_NR4     : CB<SqlBoolean,Single?>    { public override SqlBoolean C(Single? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NR8     : CB<SqlBoolean,Double?>    { public override SqlBoolean C(Double? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }

		sealed class dbB_ND      : CB<SqlBoolean,Decimal?>   { public override SqlBoolean C(Decimal? p)    { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; } }
		sealed class dbB_NC      : CB<SqlBoolean,Char?>      { public override SqlBoolean C(Char? p)       { return (p.HasValue)? Convert<Boolean,Char>.I.C(p.Value): SqlBoolean.Null; } }

		// SqlTypes.
		//
		sealed class dbB_dbS     : CB<SqlBoolean,SqlString>  { public override SqlBoolean C(SqlString p)   { return p.ToSqlBoolean(); } }

		sealed class dbB_dbU8    : CB<SqlBoolean,SqlByte>    { public override SqlBoolean C(SqlByte p)     { return p.ToSqlBoolean(); } }
		sealed class dbB_dbI16   : CB<SqlBoolean,SqlInt16>   { public override SqlBoolean C(SqlInt16 p)    { return p.ToSqlBoolean(); } }
		sealed class dbB_dbI32   : CB<SqlBoolean,SqlInt32>   { public override SqlBoolean C(SqlInt32 p)    { return p.ToSqlBoolean(); } }
		sealed class dbB_dbI64   : CB<SqlBoolean,SqlInt64>   { public override SqlBoolean C(SqlInt64 p)    { return p.ToSqlBoolean(); } }

		sealed class dbB_dbR4    : CB<SqlBoolean,SqlSingle>  { public override SqlBoolean C(SqlSingle p)   { return p.ToSqlBoolean(); } }
		sealed class dbB_dbR8    : CB<SqlBoolean,SqlDouble>  { public override SqlBoolean C(SqlDouble p)   { return p.ToSqlBoolean(); } }
		sealed class dbB_dbD     : CB<SqlBoolean,SqlDecimal> { public override SqlBoolean C(SqlDecimal p)  { return p.ToSqlBoolean(); } }
		sealed class dbB_dbM     : CB<SqlBoolean,SqlMoney>   { public override SqlBoolean C(SqlMoney p)    { return p.ToSqlBoolean(); } }

		sealed class dbB_dbDT    : CB<SqlBoolean,SqlDateTime>{ public override SqlBoolean C(SqlDateTime p) { return p.IsNull? SqlBoolean.Null: Convert.ToBoolean(p.Value); } }

		sealed class dbB_O         : CB<SqlBoolean ,object>    { public override SqlBoolean C(object p)   { return Convert.ToBoolean(p); } }

		static CB<T, P> GetSqlBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbB_B       ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbB_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbB_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbB_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbB_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbB_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbB_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbB_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbB_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbB_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbB_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbB_R8      ());

			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbB_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbB_C       ());

			// Nullable Types.
			//
			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbB_NB      ());
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbB_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbB_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbB_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbB_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbB_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbB_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbB_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbB_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbB_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbB_NR8     ());

			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbB_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbB_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbB_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbB_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbB_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbB_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbB_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbB_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbB_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbB_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbB_dbM     ());

			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbB_dbDT    ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbB_O       ());

			return (CB<T, P>)(object)Convert<SqlBoolean, object>.I;
		}

		#endregion

		#region SqlDateTime


		// Scalar Types.
		//
		sealed class dbDT_DT     : CB<SqlDateTime,DateTime>   { public override SqlDateTime C(DateTime p)    { return p; } }
		sealed class dbDT_S      : CB<SqlDateTime,String>     { public override SqlDateTime C(String p)      { return p == null? SqlDateTime.Null: SqlDateTime.Parse(p); } }

		// Nullable Types.
		//
		sealed class dbDT_NDT    : CB<SqlDateTime,DateTime?>  { public override SqlDateTime C(DateTime? p)   { return p.HasValue?                    p.Value  : SqlDateTime.Null; } }

		// SqlTypes.
		//
		sealed class dbDT_dbS    : CB<SqlDateTime,SqlString>  { public override SqlDateTime C(SqlString p)   { return p.ToSqlDateTime(); } }

		sealed class dbDT_O         : CB<SqlDateTime ,object>    { public override SqlDateTime C(object p)   { return Convert.ToDateTime(p); } }

		static CB<T, P> GetSqlDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(DateTime))    return (CB<T, P>)(object)(new dbDT_DT     ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbDT_S      ());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new dbDT_NDT    ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbDT_dbS    ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbDT_O      ());

			return (CB<T, P>)(object)Convert<SqlDateTime, object>.I;
		}

		#endregion

		#region SqlGuid


		// Scalar Types.
		//
		sealed class dbG_G       : CB<SqlGuid,Guid>       { public override SqlGuid C(Guid p)        { return p; } }
		sealed class dbG_S       : CB<SqlGuid,String>     { public override SqlGuid C(String p)      { return p == null? SqlGuid.Null: SqlGuid.Parse(p); } }

		// Nullable Types.
		//
		sealed class dbG_NG      : CB<SqlGuid,Guid?>      { public override SqlGuid C(Guid? p)       { return p.HasValue? p.Value : SqlGuid.Null; } }

		// SqlTypes.
		//
		sealed class dbG_dbBin   : CB<SqlGuid,SqlBinary>  { public override SqlGuid C(SqlBinary p)   { return p.ToSqlGuid(); } }
		sealed class dbG_dbS     : CB<SqlGuid,SqlString>  { public override SqlGuid C(SqlString p)   { return p.ToSqlGuid(); } }
		sealed class dbG_T       : CB<SqlGuid,Type>       { public override SqlGuid C(Type p)        { return p == null? SqlGuid.Null: p.GUID; } }

		sealed class dbG_O         : CB<SqlGuid ,object>    { public override SqlGuid C(object p)  
			{
				if (p == null) return SqlGuid.Null;

				// Scalar Types.
				//
				if (p is Guid)        return Convert<SqlGuid,Guid>       .I.C((Guid)p);
				if (p is String)      return Convert<SqlGuid,String>     .I.C((String)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlGuid,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBinary)   return Convert<SqlGuid,SqlBinary>  .I.C((SqlBinary)p);
				if (p is SqlString)   return Convert<SqlGuid,SqlString>  .I.C((SqlString)p);
				if (p is Type)        return Convert<SqlGuid,Type>       .I.C((Type)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetSqlGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new dbG_G       ());
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbG_S       ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new dbG_NG      ());

			// SqlTypes.
			//
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new dbG_dbBin   ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbG_dbS     ());
			if (t == typeof(Type))        return (CB<T, P>)(object)(new dbG_T       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbG_O       ());

			return (CB<T, P>)(object)Convert<SqlGuid, object>.I;
		}

		#endregion

		#region SqlBinary


		// Scalar Types.
		//
		sealed class dbBin_AU8   : CB<SqlBinary,Byte[]>     { public override SqlBinary C(Byte[] p)      { return p; } }
		sealed class dbBin_G     : CB<SqlBinary,Guid>       { public override SqlBinary C(Guid p)        { return p == Guid.Empty? SqlBinary.Null: new SqlGuid(p).ToSqlBinary(); } }

		// Nullable Types.
		//
		sealed class dbBin_NG    : CB<SqlBinary,Guid?>      { public override SqlBinary C(Guid? p)       { return p.HasValue? new SqlGuid(p.Value).ToSqlBinary(): SqlBinary.Null; } }

		// SqlTypes.
		//
		sealed class dbBin_dbAU8 : CB<SqlBinary,SqlBytes>   { public override SqlBinary C(SqlBytes p)    { return p.ToSqlBinary(); } }
		sealed class dbBin_dbG   : CB<SqlBinary,SqlGuid>    { public override SqlBinary C(SqlGuid p)     { return p.ToSqlBinary(); } }

		sealed class dbBin_O         : CB<SqlBinary ,object>    { public override SqlBinary C(object p)  
			{
				if (p == null) return SqlBinary.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<SqlBinary,Byte[]>     .I.C((Byte[])p);
				if (p is Guid)        return Convert<SqlBinary,Guid>       .I.C((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlBinary,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBytes)    return Convert<SqlBinary,SqlBytes>   .I.C((SqlBytes)p);
				if (p is SqlGuid)     return Convert<SqlBinary,SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetSqlBinaryConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new dbBin_AU8   ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new dbBin_G     ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new dbBin_NG    ());

			// SqlTypes.
			//
			if (t == typeof(SqlBytes))    return (CB<T, P>)(object)(new dbBin_dbAU8 ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new dbBin_dbG   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbBin_O     ());

			return (CB<T, P>)(object)Convert<SqlBinary, object>.I;
		}

		#endregion

		#region SqlBytes


		// Scalar Types.
		//
		sealed class dbAU8_AU8   : CB<SqlBytes,Byte[]>     { public override SqlBytes C(Byte[] p)      { return p == null? SqlBytes.Null: new SqlBytes(p); } }
		sealed class dbAU8_IOS   : CB<SqlBytes,Stream>     { public override SqlBytes C(Stream p)      { return p == null? SqlBytes.Null: new SqlBytes(p); } }
		sealed class dbAU8_G     : CB<SqlBytes,Guid>       { public override SqlBytes C(Guid p)        { return p == Guid.Empty? SqlBytes.Null: new SqlBytes(p.ToByteArray()); } }

		// Nullable Types.
		//
		sealed class dbAU8_NG    : CB<SqlBytes,Guid?>      { public override SqlBytes C(Guid? p)       { return p.HasValue? new SqlBytes(p.Value.ToByteArray()): SqlBytes.Null; } }

		// SqlTypes.
		//
		sealed class dbAU8_dbBin : CB<SqlBytes,SqlBinary>  { public override SqlBytes C(SqlBinary p)   { return p.IsNull? SqlBytes.Null: new SqlBytes(p); } }
		sealed class dbAU8_dbG   : CB<SqlBytes,SqlGuid>    { public override SqlBytes C(SqlGuid p)     { return p.IsNull? SqlBytes.Null: new SqlBytes(p.ToByteArray()); } }

		sealed class dbAU8_O         : CB<SqlBytes ,object>    { public override SqlBytes C(object p)  
			{
				if (p == null) return SqlBytes.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<SqlBytes,Byte[]>     .I.C((Byte[])p);
				if (p is Stream)      return Convert<SqlBytes,Stream>     .I.C((Stream)p);
				if (p is Guid)        return Convert<SqlBytes,Guid>       .I.C((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<SqlBytes,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBinary)   return Convert<SqlBytes,SqlBinary>  .I.C((SqlBinary)p);
				if (p is SqlGuid)     return Convert<SqlBytes,SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetSqlBytesConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new dbAU8_AU8   ());
			if (t == typeof(Stream))      return (CB<T, P>)(object)(new dbAU8_IOS   ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new dbAU8_G     ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new dbAU8_NG    ());

			// SqlTypes.
			//
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new dbAU8_dbBin ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new dbAU8_dbG   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbAU8_O     ());

			return (CB<T, P>)(object)Convert<SqlBytes, object>.I;
		}

		#endregion

		#region SqlChars


		// Scalar Types.
		//
		sealed class dbAC_S      : CB<SqlChars,String>     { public override SqlChars C(String p)      { return p == null? SqlChars.Null: new SqlChars(p.ToCharArray()); } }
		sealed class dbAC_AC     : CB<SqlChars,Char[]>     { public override SqlChars C(Char[] p)      { return p == null? SqlChars.Null: new SqlChars(p); } }

		sealed class dbAC_I8     : CB<SqlChars,SByte>      { public override SqlChars C(SByte p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_I16    : CB<SqlChars,Int16>      { public override SqlChars C(Int16 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_I32    : CB<SqlChars,Int32>      { public override SqlChars C(Int32 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_I64    : CB<SqlChars,Int64>      { public override SqlChars C(Int64 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class dbAC_U8     : CB<SqlChars,Byte>       { public override SqlChars C(Byte p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_U16    : CB<SqlChars,UInt16>     { public override SqlChars C(UInt16 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_U32    : CB<SqlChars,UInt32>     { public override SqlChars C(UInt32 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_U64    : CB<SqlChars,UInt64>     { public override SqlChars C(UInt64 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class dbAC_R4     : CB<SqlChars,Single>     { public override SqlChars C(Single p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_R8     : CB<SqlChars,Double>     { public override SqlChars C(Double p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		sealed class dbAC_B      : CB<SqlChars,Boolean>    { public override SqlChars C(Boolean p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_D      : CB<SqlChars,Decimal>    { public override SqlChars C(Decimal p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_C      : CB<SqlChars,Char>       { public override SqlChars C(Char p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_TS     : CB<SqlChars,TimeSpan>   { public override SqlChars C(TimeSpan p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_DT     : CB<SqlChars,DateTime>   { public override SqlChars C(DateTime p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); } }
		sealed class dbAC_G      : CB<SqlChars,Guid>       { public override SqlChars C(Guid p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		// Nullable Types.
		//
		sealed class dbAC_NI8    : CB<SqlChars,SByte?>     { public override SqlChars C(SByte? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NI16   : CB<SqlChars,Int16?>     { public override SqlChars C(Int16? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NI32   : CB<SqlChars,Int32?>     { public override SqlChars C(Int32? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NI64   : CB<SqlChars,Int64?>     { public override SqlChars C(Int64? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class dbAC_NU8    : CB<SqlChars,Byte?>      { public override SqlChars C(Byte? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NU16   : CB<SqlChars,UInt16?>    { public override SqlChars C(UInt16? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NU32   : CB<SqlChars,UInt32?>    { public override SqlChars C(UInt32? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NU64   : CB<SqlChars,UInt64?>    { public override SqlChars C(UInt64? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class dbAC_NR4    : CB<SqlChars,Single?>    { public override SqlChars C(Single? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NR8    : CB<SqlChars,Double?>    { public override SqlChars C(Double? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		sealed class dbAC_NB     : CB<SqlChars,Boolean?>   { public override SqlChars C(Boolean? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_ND     : CB<SqlChars,Decimal?>   { public override SqlChars C(Decimal? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NC     : CB<SqlChars,Char?>      { public override SqlChars C(Char? p)       { return p.HasValue? new SqlChars(new Char[]{p.Value})       : SqlChars.Null; } }
		sealed class dbAC_NTS    : CB<SqlChars,TimeSpan?>  { public override SqlChars C(TimeSpan? p)   { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NDT    : CB<SqlChars,DateTime?>  { public override SqlChars C(DateTime? p)   { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }
		sealed class dbAC_NG     : CB<SqlChars,Guid?>      { public override SqlChars C(Guid? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; } }

		// SqlTypes.
		//
		sealed class dbAC_dbS    : CB<SqlChars,SqlString>  { public override SqlChars C(SqlString p)   { return (SqlChars)p; } }

		sealed class dbAC_dbU8   : CB<SqlChars,SqlByte>    { public override SqlChars C(SqlByte p)     { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbI16  : CB<SqlChars,SqlInt16>   { public override SqlChars C(SqlInt16 p)    { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbI32  : CB<SqlChars,SqlInt32>   { public override SqlChars C(SqlInt32 p)    { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbI64  : CB<SqlChars,SqlInt64>   { public override SqlChars C(SqlInt64 p)    { return (SqlChars)p.ToSqlString(); } }

		sealed class dbAC_dbR4   : CB<SqlChars,SqlSingle>  { public override SqlChars C(SqlSingle p)   { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbR8   : CB<SqlChars,SqlDouble>  { public override SqlChars C(SqlDouble p)   { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbD    : CB<SqlChars,SqlDecimal> { public override SqlChars C(SqlDecimal p)  { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbM    : CB<SqlChars,SqlMoney>   { public override SqlChars C(SqlMoney p)    { return (SqlChars)p.ToSqlString(); } }

		sealed class dbAC_dbB    : CB<SqlChars,SqlBoolean> { public override SqlChars C(SqlBoolean p)  { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbG    : CB<SqlChars,SqlGuid>    { public override SqlChars C(SqlGuid p)     { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbDT   : CB<SqlChars,SqlDateTime>{ public override SqlChars C(SqlDateTime p) { return (SqlChars)p.ToSqlString(); } }
		sealed class dbAC_dbBin  : CB<SqlChars,SqlBinary>  { public override SqlChars C(SqlBinary p)   { return p.IsNull? SqlChars.Null: new SqlChars(p.ToString().ToCharArray()); } }
		sealed class dbAC_T      : CB<SqlChars,Type>       { public override SqlChars C(Type p)        { return p == null? SqlChars.Null: new SqlChars(p.FullName.ToCharArray()); } }

		sealed class dbAC_O         : CB<SqlChars ,object>    { public override SqlChars C(object p)   { return new SqlChars(Convert.ToString(p).ToCharArray()); } }

		static CB<T, P> GetSqlCharsConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbAC_S      ());
			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new dbAC_AC     ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new dbAC_I8     ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new dbAC_I16    ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new dbAC_I32    ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new dbAC_I64    ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new dbAC_U8     ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new dbAC_U16    ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new dbAC_U32    ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new dbAC_U64    ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new dbAC_R4     ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new dbAC_R8     ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new dbAC_B      ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new dbAC_D      ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new dbAC_C      ());
			if (t == typeof(TimeSpan))    return (CB<T, P>)(object)(new dbAC_TS     ());
			if (t == typeof(DateTime))    return (CB<T, P>)(object)(new dbAC_DT     ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new dbAC_G      ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new dbAC_NI8    ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new dbAC_NI16   ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new dbAC_NI32   ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new dbAC_NI64   ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new dbAC_NU8    ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new dbAC_NU16   ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new dbAC_NU32   ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new dbAC_NU64   ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new dbAC_NR4    ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new dbAC_NR8    ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new dbAC_NB     ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new dbAC_ND     ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new dbAC_NC     ());
			if (t == typeof(TimeSpan?))   return (CB<T, P>)(object)(new dbAC_NTS    ());
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new dbAC_NDT    ());
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new dbAC_NG     ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbAC_dbS    ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new dbAC_dbU8   ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new dbAC_dbI16  ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new dbAC_dbI32  ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new dbAC_dbI64  ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new dbAC_dbR4   ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new dbAC_dbR8   ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new dbAC_dbD    ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new dbAC_dbM    ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new dbAC_dbB    ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new dbAC_dbG    ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new dbAC_dbDT   ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new dbAC_dbBin  ());
			if (t == typeof(Type))        return (CB<T, P>)(object)(new dbAC_T      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbAC_O      ());

			return (CB<T, P>)(object)Convert<SqlChars, object>.I;
		}

		#endregion

		#region SqlXml


		// Scalar Types.
		//
		sealed class dbXml_S     : CB<SqlXml,String>     { public override SqlXml C(String p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p))); } }

		sealed class dbXml_IOS   : CB<SqlXml,Stream>     { public override SqlXml C(Stream p)      { return p == null? SqlXml.Null: new SqlXml(p); } }
		sealed class dbXml_Xml   : CB<SqlXml,XmlReader>  { public override SqlXml C(XmlReader p)   { return p == null? SqlXml.Null: new SqlXml(p); } }

		sealed class dbXml_AC    : CB<SqlXml,Char[]>     { public override SqlXml C(Char[] p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(new string(p)))); } }
		sealed class dbXml_AU8   : CB<SqlXml,Byte[]>     { public override SqlXml C(Byte[] p)      { return p == null? SqlXml.Null: new SqlXml(new MemoryStream(p)); } }

		// SqlTypes.
		//
		sealed class dbXml_dbS   : CB<SqlXml,SqlString>  { public override SqlXml C(SqlString p)   { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.Value))); } }
		sealed class dbXml_dbAC  : CB<SqlXml,SqlChars>   { public override SqlXml C(SqlChars p)    { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.ToSqlString().Value))); } }
		sealed class dbXml_dbBin : CB<SqlXml,SqlBinary>  { public override SqlXml C(SqlBinary p)   { return p.IsNull? SqlXml.Null: new SqlXml(new MemoryStream(p.Value)); } }
		sealed class dbXml_dbAU8 : CB<SqlXml,SqlBytes>   { public override SqlXml C(SqlBytes p)    { return p.IsNull? SqlXml.Null: new SqlXml(p.Stream); } }

		sealed class dbXml_O         : CB<SqlXml ,object>    { public override SqlXml C(object p)  
			{
				if (p == null) return SqlXml.Null;

				// Scalar Types.
				//
				if (p is String)      return Convert<SqlXml,String>     .I.C((String)p);

				if (p is Stream)      return Convert<SqlXml,Stream>     .I.C((Stream)p);
				if (p is XmlReader)   return Convert<SqlXml,XmlReader>  .I.C((XmlReader)p);

				if (p is Char[])      return Convert<SqlXml,Char[]>     .I.C((Char[])p);
				if (p is Byte[])      return Convert<SqlXml,Byte[]>     .I.C((Byte[])p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<SqlXml,SqlString>  .I.C((SqlString)p);
				if (p is SqlChars)    return Convert<SqlXml,SqlChars>   .I.C((SqlChars)p);
				if (p is SqlBinary)   return Convert<SqlXml,SqlBinary>  .I.C((SqlBinary)p);
				if (p is SqlBytes)    return Convert<SqlXml,SqlBytes>   .I.C((SqlBytes)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetSqlXmlConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new dbXml_S     ());

			if (t == typeof(Stream))      return (CB<T, P>)(object)(new dbXml_IOS   ());
			if (t == typeof(XmlReader))   return (CB<T, P>)(object)(new dbXml_Xml   ());

			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new dbXml_AC    ());
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new dbXml_AU8   ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new dbXml_dbS   ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new dbXml_dbAC  ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new dbXml_dbBin ());
			if (t == typeof(SqlBytes))    return (CB<T, P>)(object)(new dbXml_dbAU8 ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new dbXml_O     ());

			return (CB<T, P>)(object)Convert<SqlXml, object>.I;
		}

		#endregion

		#endregion
	}
}
