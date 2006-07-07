using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		#region Scalar Types

		#region String

		sealed class S_T         : CB<String,Type>       { public override String C(Type p)        { return p == null? null: p.FullName; } }

		// Scalar Types.
		//
		sealed class S_I8        : CB<String,SByte>      { public override String C(SByte p)       { return p.ToString(); } }
		sealed class S_I16       : CB<String,Int16>      { public override String C(Int16 p)       { return p.ToString(); } }
		sealed class S_I32       : CB<String,Int32>      { public override String C(Int32 p)       { return p.ToString(); } }
		sealed class S_I64       : CB<String,Int64>      { public override String C(Int64 p)       { return p.ToString(); } }

		sealed class S_U8        : CB<String,Byte>       { public override String C(Byte p)        { return p.ToString(); } }
		sealed class S_U16       : CB<String,UInt16>     { public override String C(UInt16 p)      { return p.ToString(); } }
		sealed class S_U32       : CB<String,UInt32>     { public override String C(UInt32 p)      { return p.ToString(); } }
		sealed class S_U64       : CB<String,UInt64>     { public override String C(UInt64 p)      { return p.ToString(); } }

		sealed class S_R4        : CB<String,Single>     { public override String C(Single p)      { return p.ToString(); } }
		sealed class S_R8        : CB<String,Double>     { public override String C(Double p)      { return p.ToString(); } }

		sealed class S_B         : CB<String,Boolean>    { public override String C(Boolean p)     { return p.ToString(); } }
		sealed class S_D         : CB<String,Decimal>    { public override String C(Decimal p)     { return p.ToString(); } }
		sealed class S_C         : CB<String,Char>       { public override String C(Char p)        { return p.ToString(); } }
		sealed class S_TS        : CB<String,TimeSpan>   { public override String C(TimeSpan p)    { return p.ToString(); } }
		sealed class S_DT        : CB<String,DateTime>   { public override String C(DateTime p)    { return p.ToString(); } }
		sealed class S_G         : CB<String,Guid>       { public override String C(Guid p)        { return p.ToString(); } }

		// Nullable Types.
		//
		sealed class S_NI8       : CB<String,SByte?>     { public override String C(SByte? p)      { return p.ToString(); } }
		sealed class S_NI16      : CB<String,Int16?>     { public override String C(Int16? p)      { return p.ToString(); } }
		sealed class S_NI32      : CB<String,Int32?>     { public override String C(Int32? p)      { return p.ToString(); } }
		sealed class S_NI64      : CB<String,Int64?>     { public override String C(Int64? p)      { return p.ToString(); } }

		sealed class S_NU8       : CB<String,Byte?>      { public override String C(Byte? p)       { return p.ToString(); } }
		sealed class S_NU16      : CB<String,UInt16?>    { public override String C(UInt16? p)     { return p.ToString(); } }
		sealed class S_NU32      : CB<String,UInt32?>    { public override String C(UInt32? p)     { return p.ToString(); } }
		sealed class S_NU64      : CB<String,UInt64?>    { public override String C(UInt64? p)     { return p.ToString(); } }

		sealed class S_NR4       : CB<String,Single?>    { public override String C(Single? p)     { return p.ToString(); } }
		sealed class S_NR8       : CB<String,Double?>    { public override String C(Double? p)     { return p.ToString(); } }

		sealed class S_NB        : CB<String,Boolean?>   { public override String C(Boolean? p)    { return p.ToString(); } }
		sealed class S_ND        : CB<String,Decimal?>   { public override String C(Decimal? p)    { return p.ToString(); } }
		sealed class S_NC        : CB<String,Char?>      { public override String C(Char? p)       { return p.ToString(); } }
		sealed class S_NTS       : CB<String,TimeSpan?>  { public override String C(TimeSpan? p)   { return p.ToString(); } }
		sealed class S_NDT       : CB<String,DateTime?>  { public override String C(DateTime? p)   { return p.ToString(); } }
		sealed class S_NG        : CB<String,Guid?>      { public override String C(Guid? p)       { return p.ToString(); } }

		// SqlTypes.
		//
		sealed class S_dbS       : CB<String,SqlString>  { public override String C(SqlString p)   { return p.ToString(); } }

		sealed class S_dbU8      : CB<String,SqlByte>    { public override String C(SqlByte p)     { return p.ToString(); } }
		sealed class S_dbI16     : CB<String,SqlInt16>   { public override String C(SqlInt16 p)    { return p.ToString(); } }
		sealed class S_dbI32     : CB<String,SqlInt32>   { public override String C(SqlInt32 p)    { return p.ToString(); } }
		sealed class S_dbI64     : CB<String,SqlInt64>   { public override String C(SqlInt64 p)    { return p.ToString(); } }

		sealed class S_dbR4      : CB<String,SqlSingle>  { public override String C(SqlSingle p)   { return p.ToString(); } }
		sealed class S_dbR8      : CB<String,SqlDouble>  { public override String C(SqlDouble p)   { return p.ToString(); } }
		sealed class S_dbD       : CB<String,SqlDecimal> { public override String C(SqlDecimal p)  { return p.ToString(); } }
		sealed class S_dbM       : CB<String,SqlMoney>   { public override String C(SqlMoney p)    { return p.ToString(); } }

		sealed class S_dbB       : CB<String,SqlBoolean> { public override String C(SqlBoolean p)  { return p.ToString(); } }
		sealed class S_dbG       : CB<String,SqlGuid>    { public override String C(SqlGuid p)     { return p.ToString(); } }
		sealed class S_dbBin     : CB<String,SqlBinary>  { public override String C(SqlBinary p)   { return p.ToString(); } }

		sealed class S_O         : CB<String ,object>    { public override String C(object p)   { return Convert.ToString(p); } }

		static CB<T, P> GetStringConverter()
		{
			Type t = typeof(P);

			if (t == typeof(Type))        return (CB<T, P>)(object)(new S_T         ());

			// Scalar Types.
			//
			if (t == typeof(SByte))       return (CB<T, P>)(object)(new S_I8        ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new S_I16       ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new S_I32       ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new S_I64       ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new S_U8        ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new S_U16       ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new S_U32       ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new S_U64       ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new S_R4        ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new S_R8        ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new S_B         ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new S_D         ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new S_C         ());
			if (t == typeof(TimeSpan))    return (CB<T, P>)(object)(new S_TS        ());
			if (t == typeof(DateTime))    return (CB<T, P>)(object)(new S_DT        ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new S_G         ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new S_NI8       ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new S_NI16      ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new S_NI32      ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new S_NI64      ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new S_NU8       ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new S_NU16      ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new S_NU32      ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new S_NU64      ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new S_NR4       ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new S_NR8       ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new S_NB        ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new S_ND        ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new S_NC        ());
			if (t == typeof(TimeSpan?))   return (CB<T, P>)(object)(new S_NTS       ());
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new S_NDT       ());
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new S_NG        ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new S_dbS       ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new S_dbU8      ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new S_dbI16     ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new S_dbI32     ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new S_dbI64     ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new S_dbR4      ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new S_dbR8      ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new S_dbD       ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new S_dbM       ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new S_dbB       ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new S_dbG       ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new S_dbBin     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new S_O         ());

			return (CB<T, P>)(object)Convert<String, object>.I;
		}

		#endregion

		#region SByte


		// Scalar Types.
		//
		sealed class I8_S        : CB<SByte,String>     { public override SByte C(String p)      { return Convert.ToSByte(p); } }

		sealed class I8_I16      : CB<SByte,Int16>      { public override SByte C(Int16 p)       { return Convert.ToSByte(p); } }
		sealed class I8_I32      : CB<SByte,Int32>      { public override SByte C(Int32 p)       { return Convert.ToSByte(p); } }
		sealed class I8_I64      : CB<SByte,Int64>      { public override SByte C(Int64 p)       { return Convert.ToSByte(p); } }

		sealed class I8_U8       : CB<SByte,Byte>       { public override SByte C(Byte p)        { return Convert.ToSByte(p); } }
		sealed class I8_U16      : CB<SByte,UInt16>     { public override SByte C(UInt16 p)      { return Convert.ToSByte(p); } }
		sealed class I8_U32      : CB<SByte,UInt32>     { public override SByte C(UInt32 p)      { return Convert.ToSByte(p); } }
		sealed class I8_U64      : CB<SByte,UInt64>     { public override SByte C(UInt64 p)      { return Convert.ToSByte(p); } }

		sealed class I8_R4       : CB<SByte,Single>     { public override SByte C(Single p)      { return Convert.ToSByte(p); } }
		sealed class I8_R8       : CB<SByte,Double>     { public override SByte C(Double p)      { return Convert.ToSByte(p); } }

		sealed class I8_B        : CB<SByte,Boolean>    { public override SByte C(Boolean p)     { return Convert.ToSByte(p); } }
		sealed class I8_D        : CB<SByte,Decimal>    { public override SByte C(Decimal p)     { return Convert.ToSByte(p); } }
		sealed class I8_C        : CB<SByte,Char>       { public override SByte C(Char p)        { return Convert.ToSByte(p); } }

		// Nullable Types.
		//
		sealed class I8_NI8      : CB<SByte,SByte?>     { public override SByte C(SByte? p)      { return p.HasValue?                 p.Value : (SByte)0; } }

		sealed class I8_NI16     : CB<SByte,Int16?>     { public override SByte C(Int16? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NI32     : CB<SByte,Int32?>     { public override SByte C(Int32? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NI64     : CB<SByte,Int64?>     { public override SByte C(Int64? p)      { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class I8_NU8      : CB<SByte,Byte?>      { public override SByte C(Byte? p)       { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NU16     : CB<SByte,UInt16?>    { public override SByte C(UInt16? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NU32     : CB<SByte,UInt32?>    { public override SByte C(UInt32? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NU64     : CB<SByte,UInt64?>    { public override SByte C(UInt64? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class I8_NR4      : CB<SByte,Single?>    { public override SByte C(Single? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NR8      : CB<SByte,Double?>    { public override SByte C(Double? p)     { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		sealed class I8_NB       : CB<SByte,Boolean?>   { public override SByte C(Boolean? p)    { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_ND       : CB<SByte,Decimal?>   { public override SByte C(Decimal? p)    { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }
		sealed class I8_NC       : CB<SByte,Char?>      { public override SByte C(Char? p)       { return p.HasValue? Convert.ToSByte(p.Value): (SByte)0; } }

		// SqlTypes.
		//
		sealed class I8_dbS      : CB<SByte,SqlString>  { public override SByte C(SqlString p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class I8_dbU8     : CB<SByte,SqlByte>    { public override SByte C(SqlByte p)     { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbI16    : CB<SByte,SqlInt16>   { public override SByte C(SqlInt16 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbI32    : CB<SByte,SqlInt32>   { public override SByte C(SqlInt32 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbI64    : CB<SByte,SqlInt64>   { public override SByte C(SqlInt64 p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class I8_dbR4     : CB<SByte,SqlSingle>  { public override SByte C(SqlSingle p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbR8     : CB<SByte,SqlDouble>  { public override SByte C(SqlDouble p)   { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbD      : CB<SByte,SqlDecimal> { public override SByte C(SqlDecimal p)  { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }
		sealed class I8_dbM      : CB<SByte,SqlMoney>   { public override SByte C(SqlMoney p)    { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class I8_dbB      : CB<SByte,SqlBoolean> { public override SByte C(SqlBoolean p)  { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); } }

		sealed class I8_O         : CB<SByte ,object>    { public override SByte C(object p)   { return Convert.ToSByte(p); } }

		static CB<T, P> GetSByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new I8_S        ());

			if (t == typeof(Int16))       return (CB<T, P>)(object)(new I8_I16      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new I8_I32      ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new I8_I64      ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new I8_U8       ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new I8_U16      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new I8_U32      ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new I8_U64      ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new I8_R4       ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new I8_R8       ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new I8_B        ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new I8_D        ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new I8_C        ());

			// Nullable Types.
			//
			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new I8_NI8      ());

			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new I8_NI16     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new I8_NI32     ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new I8_NI64     ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new I8_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new I8_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new I8_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new I8_NU64     ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new I8_NR4      ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new I8_NR8      ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new I8_NB       ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new I8_ND       ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new I8_NC       ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new I8_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new I8_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new I8_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new I8_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new I8_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new I8_dbR4     ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new I8_dbR8     ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new I8_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new I8_dbM      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new I8_dbB      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new I8_O        ());

			return (CB<T, P>)(object)Convert<SByte, object>.I;
		}

		#endregion

		#region Int16


		// Scalar Types.
		//
		sealed class I16_S       : CB<Int16,String>     { public override Int16 C(String p)      { return Convert.ToInt16(p); } }

		sealed class I16_I8      : CB<Int16,SByte>      { public override Int16 C(SByte p)       { return Convert.ToInt16(p); } }
		sealed class I16_I32     : CB<Int16,Int32>      { public override Int16 C(Int32 p)       { return Convert.ToInt16(p); } }
		sealed class I16_I64     : CB<Int16,Int64>      { public override Int16 C(Int64 p)       { return Convert.ToInt16(p); } }

		sealed class I16_U8      : CB<Int16,Byte>       { public override Int16 C(Byte p)        { return Convert.ToInt16(p); } }
		sealed class I16_U16     : CB<Int16,UInt16>     { public override Int16 C(UInt16 p)      { return Convert.ToInt16(p); } }
		sealed class I16_U32     : CB<Int16,UInt32>     { public override Int16 C(UInt32 p)      { return Convert.ToInt16(p); } }
		sealed class I16_U64     : CB<Int16,UInt64>     { public override Int16 C(UInt64 p)      { return Convert.ToInt16(p); } }

		sealed class I16_R4      : CB<Int16,Single>     { public override Int16 C(Single p)      { return Convert.ToInt16(p); } }
		sealed class I16_R8      : CB<Int16,Double>     { public override Int16 C(Double p)      { return Convert.ToInt16(p); } }

		sealed class I16_B       : CB<Int16,Boolean>    { public override Int16 C(Boolean p)     { return Convert.ToInt16(p); } }
		sealed class I16_D       : CB<Int16,Decimal>    { public override Int16 C(Decimal p)     { return Convert.ToInt16(p); } }
		sealed class I16_C       : CB<Int16,Char>       { public override Int16 C(Char p)        { return Convert.ToInt16(p); } }

		// Nullable Types.
		//
		sealed class I16_NI16    : CB<Int16,Int16?>     { public override Int16 C(Int16? p)      { return p.HasValue?                 p.Value : (Int16)0; } }

		sealed class I16_NI8     : CB<Int16,SByte?>     { public override Int16 C(SByte? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NI32    : CB<Int16,Int32?>     { public override Int16 C(Int32? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NI64    : CB<Int16,Int64?>     { public override Int16 C(Int64? p)      { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class I16_NU8     : CB<Int16,Byte?>      { public override Int16 C(Byte? p)       { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NU16    : CB<Int16,UInt16?>    { public override Int16 C(UInt16? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NU32    : CB<Int16,UInt32?>    { public override Int16 C(UInt32? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NU64    : CB<Int16,UInt64?>    { public override Int16 C(UInt64? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class I16_NR4     : CB<Int16,Single?>    { public override Int16 C(Single? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NR8     : CB<Int16,Double?>    { public override Int16 C(Double? p)     { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		sealed class I16_NB      : CB<Int16,Boolean?>   { public override Int16 C(Boolean? p)    { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_ND      : CB<Int16,Decimal?>   { public override Int16 C(Decimal? p)    { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }
		sealed class I16_NC      : CB<Int16,Char?>      { public override Int16 C(Char? p)       { return p.HasValue? Convert.ToInt16(p.Value): (Int16)0; } }

		// SqlTypes.
		//
		sealed class I16_dbI16   : CB<Int16,SqlInt16>   { public override Int16 C(SqlInt16 p)    { return p.IsNull? (Int16)0:                 p.Value;  } }
		sealed class I16_dbS     : CB<Int16,SqlString>  { public override Int16 C(SqlString p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class I16_dbU8    : CB<Int16,SqlByte>    { public override Int16 C(SqlByte p)     { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class I16_dbI32   : CB<Int16,SqlInt32>   { public override Int16 C(SqlInt32 p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class I16_dbI64   : CB<Int16,SqlInt64>   { public override Int16 C(SqlInt64 p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class I16_dbR4    : CB<Int16,SqlSingle>  { public override Int16 C(SqlSingle p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class I16_dbR8    : CB<Int16,SqlDouble>  { public override Int16 C(SqlDouble p)   { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class I16_dbD     : CB<Int16,SqlDecimal> { public override Int16 C(SqlDecimal p)  { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }
		sealed class I16_dbM     : CB<Int16,SqlMoney>   { public override Int16 C(SqlMoney p)    { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class I16_dbB     : CB<Int16,SqlBoolean> { public override Int16 C(SqlBoolean p)  { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); } }

		sealed class I16_O         : CB<Int16 ,object>    { public override Int16 C(object p)   { return Convert.ToInt16(p); } }

		static CB<T, P> GetInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new I16_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new I16_I8      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new I16_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new I16_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new I16_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new I16_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new I16_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new I16_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new I16_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new I16_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new I16_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new I16_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new I16_C       ());

			// Nullable Types.
			//
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new I16_NI16    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new I16_NI8     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new I16_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new I16_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new I16_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new I16_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new I16_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new I16_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new I16_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new I16_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new I16_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new I16_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new I16_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new I16_dbI16   ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new I16_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new I16_dbU8    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new I16_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new I16_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new I16_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new I16_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new I16_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new I16_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new I16_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new I16_O       ());

			return (CB<T, P>)(object)Convert<Int16, object>.I;
		}

		#endregion

		#region Int32


		// Scalar Types.
		//
		sealed class I32_S       : CB<Int32,String>     { public override Int32 C(String p)      { return Convert.ToInt32(p); } }

		sealed class I32_I8      : CB<Int32,SByte>      { public override Int32 C(SByte p)       { return Convert.ToInt32(p); } }
		sealed class I32_I16     : CB<Int32,Int16>      { public override Int32 C(Int16 p)       { return Convert.ToInt32(p); } }
		sealed class I32_I64     : CB<Int32,Int64>      { public override Int32 C(Int64 p)       { return Convert.ToInt32(p); } }

		sealed class I32_U8      : CB<Int32,Byte>       { public override Int32 C(Byte p)        { return Convert.ToInt32(p); } }
		sealed class I32_U16     : CB<Int32,UInt16>     { public override Int32 C(UInt16 p)      { return Convert.ToInt32(p); } }
		sealed class I32_U32     : CB<Int32,UInt32>     { public override Int32 C(UInt32 p)      { return Convert.ToInt32(p); } }
		sealed class I32_U64     : CB<Int32,UInt64>     { public override Int32 C(UInt64 p)      { return Convert.ToInt32(p); } }

		sealed class I32_R4      : CB<Int32,Single>     { public override Int32 C(Single p)      { return Convert.ToInt32(p); } }
		sealed class I32_R8      : CB<Int32,Double>     { public override Int32 C(Double p)      { return Convert.ToInt32(p); } }

		sealed class I32_B       : CB<Int32,Boolean>    { public override Int32 C(Boolean p)     { return Convert.ToInt32(p); } }
		sealed class I32_D       : CB<Int32,Decimal>    { public override Int32 C(Decimal p)     { return Convert.ToInt32(p); } }
		sealed class I32_C       : CB<Int32,Char>       { public override Int32 C(Char p)        { return Convert.ToInt32(p); } }

		// Nullable Types.
		//
		sealed class I32_NI32    : CB<Int32,Int32?>     { public override Int32 C(Int32? p)      { return p.HasValue?                 p.Value : 0; } }

		sealed class I32_NI8     : CB<Int32,SByte?>     { public override Int32 C(SByte? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NI16    : CB<Int32,Int16?>     { public override Int32 C(Int16? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NI64    : CB<Int32,Int64?>     { public override Int32 C(Int64? p)      { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class I32_NU8     : CB<Int32,Byte?>      { public override Int32 C(Byte? p)       { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NU16    : CB<Int32,UInt16?>    { public override Int32 C(UInt16? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NU32    : CB<Int32,UInt32?>    { public override Int32 C(UInt32? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NU64    : CB<Int32,UInt64?>    { public override Int32 C(UInt64? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class I32_NR4     : CB<Int32,Single?>    { public override Int32 C(Single? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NR8     : CB<Int32,Double?>    { public override Int32 C(Double? p)     { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		sealed class I32_NB      : CB<Int32,Boolean?>   { public override Int32 C(Boolean? p)    { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_ND      : CB<Int32,Decimal?>   { public override Int32 C(Decimal? p)    { return p.HasValue? Convert.ToInt32(p.Value): 0; } }
		sealed class I32_NC      : CB<Int32,Char?>      { public override Int32 C(Char? p)       { return p.HasValue? Convert.ToInt32(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class I32_dbI32   : CB<Int32,SqlInt32>   { public override Int32 C(SqlInt32 p)    { return p.IsNull? 0:                 p.Value;  } }
		sealed class I32_dbS     : CB<Int32,SqlString>  { public override Int32 C(SqlString p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class I32_dbU8    : CB<Int32,SqlByte>    { public override Int32 C(SqlByte p)     { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class I32_dbI16   : CB<Int32,SqlInt16>   { public override Int32 C(SqlInt16 p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class I32_dbI64   : CB<Int32,SqlInt64>   { public override Int32 C(SqlInt64 p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class I32_dbR4    : CB<Int32,SqlSingle>  { public override Int32 C(SqlSingle p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class I32_dbR8    : CB<Int32,SqlDouble>  { public override Int32 C(SqlDouble p)   { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class I32_dbD     : CB<Int32,SqlDecimal> { public override Int32 C(SqlDecimal p)  { return p.IsNull? 0: Convert.ToInt32(p.Value); } }
		sealed class I32_dbM     : CB<Int32,SqlMoney>   { public override Int32 C(SqlMoney p)    { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class I32_dbB     : CB<Int32,SqlBoolean> { public override Int32 C(SqlBoolean p)  { return p.IsNull? 0: Convert.ToInt32(p.Value); } }

		sealed class I32_O         : CB<Int32 ,object>    { public override Int32 C(object p)   { return Convert.ToInt32(p); } }

		static CB<T, P> GetInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new I32_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new I32_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new I32_I16     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new I32_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new I32_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new I32_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new I32_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new I32_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new I32_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new I32_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new I32_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new I32_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new I32_C       ());

			// Nullable Types.
			//
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new I32_NI32    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new I32_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new I32_NI16    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new I32_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new I32_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new I32_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new I32_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new I32_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new I32_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new I32_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new I32_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new I32_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new I32_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new I32_dbI32   ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new I32_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new I32_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new I32_dbI16   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new I32_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new I32_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new I32_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new I32_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new I32_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new I32_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new I32_O       ());

			return (CB<T, P>)(object)Convert<Int32, object>.I;
		}

		#endregion

		#region Int64


		// Scalar Types.
		//
		sealed class I64_S       : CB<Int64,String>     { public override Int64 C(String p)      { return Convert.ToInt64(p); } }

		sealed class I64_I8      : CB<Int64,SByte>      { public override Int64 C(SByte p)       { return Convert.ToInt64(p); } }
		sealed class I64_I16     : CB<Int64,Int16>      { public override Int64 C(Int16 p)       { return Convert.ToInt64(p); } }
		sealed class I64_I32     : CB<Int64,Int32>      { public override Int64 C(Int32 p)       { return Convert.ToInt64(p); } }

		sealed class I64_U8      : CB<Int64,Byte>       { public override Int64 C(Byte p)        { return Convert.ToInt64(p); } }
		sealed class I64_U16     : CB<Int64,UInt16>     { public override Int64 C(UInt16 p)      { return Convert.ToInt64(p); } }
		sealed class I64_U32     : CB<Int64,UInt32>     { public override Int64 C(UInt32 p)      { return Convert.ToInt64(p); } }
		sealed class I64_U64     : CB<Int64,UInt64>     { public override Int64 C(UInt64 p)      { return Convert.ToInt64(p); } }

		sealed class I64_R4      : CB<Int64,Single>     { public override Int64 C(Single p)      { return Convert.ToInt64(p); } }
		sealed class I64_R8      : CB<Int64,Double>     { public override Int64 C(Double p)      { return Convert.ToInt64(p); } }

		sealed class I64_B       : CB<Int64,Boolean>    { public override Int64 C(Boolean p)     { return Convert.ToInt64(p); } }
		sealed class I64_D       : CB<Int64,Decimal>    { public override Int64 C(Decimal p)     { return Convert.ToInt64(p); } }
		sealed class I64_C       : CB<Int64,Char>       { public override Int64 C(Char p)        { return Convert.ToInt64(p); } }

		// Nullable Types.
		//
		sealed class I64_NI64    : CB<Int64,Int64?>     { public override Int64 C(Int64? p)      { return p.HasValue?                 p.Value : 0; } }

		sealed class I64_NI8     : CB<Int64,SByte?>     { public override Int64 C(SByte? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NI16    : CB<Int64,Int16?>     { public override Int64 C(Int16? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NI32    : CB<Int64,Int32?>     { public override Int64 C(Int32? p)      { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class I64_NU8     : CB<Int64,Byte?>      { public override Int64 C(Byte? p)       { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NU16    : CB<Int64,UInt16?>    { public override Int64 C(UInt16? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NU32    : CB<Int64,UInt32?>    { public override Int64 C(UInt32? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NU64    : CB<Int64,UInt64?>    { public override Int64 C(UInt64? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class I64_NR4     : CB<Int64,Single?>    { public override Int64 C(Single? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NR8     : CB<Int64,Double?>    { public override Int64 C(Double? p)     { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		sealed class I64_NB      : CB<Int64,Boolean?>   { public override Int64 C(Boolean? p)    { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_ND      : CB<Int64,Decimal?>   { public override Int64 C(Decimal? p)    { return p.HasValue? Convert.ToInt64(p.Value): 0; } }
		sealed class I64_NC      : CB<Int64,Char?>      { public override Int64 C(Char? p)       { return p.HasValue? Convert.ToInt64(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class I64_dbI64   : CB<Int64,SqlInt64>   { public override Int64 C(SqlInt64 p)    { return p.IsNull? 0:                 p.Value;  } }
		sealed class I64_dbS     : CB<Int64,SqlString>  { public override Int64 C(SqlString p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class I64_dbU8    : CB<Int64,SqlByte>    { public override Int64 C(SqlByte p)     { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class I64_dbI16   : CB<Int64,SqlInt16>   { public override Int64 C(SqlInt16 p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class I64_dbI32   : CB<Int64,SqlInt32>   { public override Int64 C(SqlInt32 p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class I64_dbR4    : CB<Int64,SqlSingle>  { public override Int64 C(SqlSingle p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class I64_dbR8    : CB<Int64,SqlDouble>  { public override Int64 C(SqlDouble p)   { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class I64_dbD     : CB<Int64,SqlDecimal> { public override Int64 C(SqlDecimal p)  { return p.IsNull? 0: Convert.ToInt64(p.Value); } }
		sealed class I64_dbM     : CB<Int64,SqlMoney>   { public override Int64 C(SqlMoney p)    { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class I64_dbB     : CB<Int64,SqlBoolean> { public override Int64 C(SqlBoolean p)  { return p.IsNull? 0: Convert.ToInt64(p.Value); } }

		sealed class I64_O         : CB<Int64 ,object>    { public override Int64 C(object p)   { return Convert.ToInt64(p); } }

		static CB<T, P> GetInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new I64_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new I64_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new I64_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new I64_I32     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new I64_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new I64_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new I64_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new I64_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new I64_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new I64_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new I64_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new I64_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new I64_C       ());

			// Nullable Types.
			//
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new I64_NI64    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new I64_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new I64_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new I64_NI32    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new I64_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new I64_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new I64_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new I64_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new I64_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new I64_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new I64_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new I64_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new I64_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new I64_dbI64   ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new I64_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new I64_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new I64_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new I64_dbI32   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new I64_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new I64_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new I64_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new I64_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new I64_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new I64_O       ());

			return (CB<T, P>)(object)Convert<Int64, object>.I;
		}

		#endregion

		#region Byte


		// Scalar Types.
		//
		sealed class U8_S        : CB<Byte,String>     { public override Byte C(String p)      { return Convert.ToByte(p); } }

		sealed class U8_I8       : CB<Byte,SByte>      { public override Byte C(SByte p)       { return Convert.ToByte(p); } }
		sealed class U8_I16      : CB<Byte,Int16>      { public override Byte C(Int16 p)       { return Convert.ToByte(p); } }
		sealed class U8_I32      : CB<Byte,Int32>      { public override Byte C(Int32 p)       { return Convert.ToByte(p); } }
		sealed class U8_I64      : CB<Byte,Int64>      { public override Byte C(Int64 p)       { return Convert.ToByte(p); } }

		sealed class U8_U16      : CB<Byte,UInt16>     { public override Byte C(UInt16 p)      { return Convert.ToByte(p); } }
		sealed class U8_U32      : CB<Byte,UInt32>     { public override Byte C(UInt32 p)      { return Convert.ToByte(p); } }
		sealed class U8_U64      : CB<Byte,UInt64>     { public override Byte C(UInt64 p)      { return Convert.ToByte(p); } }

		sealed class U8_R4       : CB<Byte,Single>     { public override Byte C(Single p)      { return Convert.ToByte(p); } }
		sealed class U8_R8       : CB<Byte,Double>     { public override Byte C(Double p)      { return Convert.ToByte(p); } }

		sealed class U8_B        : CB<Byte,Boolean>    { public override Byte C(Boolean p)     { return Convert.ToByte(p); } }
		sealed class U8_D        : CB<Byte,Decimal>    { public override Byte C(Decimal p)     { return Convert.ToByte(p); } }
		sealed class U8_C        : CB<Byte,Char>       { public override Byte C(Char p)        { return Convert.ToByte(p); } }

		// Nullable Types.
		//
		sealed class U8_NU8      : CB<Byte,Byte?>      { public override Byte C(Byte? p)       { return p.HasValue?                p.Value : (Byte)0; } }

		sealed class U8_NI8      : CB<Byte,SByte?>     { public override Byte C(SByte? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NI16     : CB<Byte,Int16?>     { public override Byte C(Int16? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NI32     : CB<Byte,Int32?>     { public override Byte C(Int32? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NI64     : CB<Byte,Int64?>     { public override Byte C(Int64? p)      { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class U8_NU16     : CB<Byte,UInt16?>    { public override Byte C(UInt16? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NU32     : CB<Byte,UInt32?>    { public override Byte C(UInt32? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NU64     : CB<Byte,UInt64?>    { public override Byte C(UInt64? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class U8_NR4      : CB<Byte,Single?>    { public override Byte C(Single? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NR8      : CB<Byte,Double?>    { public override Byte C(Double? p)     { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		sealed class U8_NB       : CB<Byte,Boolean?>   { public override Byte C(Boolean? p)    { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_ND       : CB<Byte,Decimal?>   { public override Byte C(Decimal? p)    { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }
		sealed class U8_NC       : CB<Byte,Char?>      { public override Byte C(Char? p)       { return p.HasValue? Convert.ToByte(p.Value): (Byte)0; } }

		// SqlTypes.
		//
		sealed class U8_dbU8     : CB<Byte,SqlByte>    { public override Byte C(SqlByte p)     { return p.IsNull? (Byte)0:                p.Value;  } }
		sealed class U8_dbS      : CB<Byte,SqlString>  { public override Byte C(SqlString p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class U8_dbI16    : CB<Byte,SqlInt16>   { public override Byte C(SqlInt16 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class U8_dbI32    : CB<Byte,SqlInt32>   { public override Byte C(SqlInt32 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class U8_dbI64    : CB<Byte,SqlInt64>   { public override Byte C(SqlInt64 p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class U8_dbR4     : CB<Byte,SqlSingle>  { public override Byte C(SqlSingle p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class U8_dbR8     : CB<Byte,SqlDouble>  { public override Byte C(SqlDouble p)   { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class U8_dbD      : CB<Byte,SqlDecimal> { public override Byte C(SqlDecimal p)  { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }
		sealed class U8_dbM      : CB<Byte,SqlMoney>   { public override Byte C(SqlMoney p)    { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class U8_dbB      : CB<Byte,SqlBoolean> { public override Byte C(SqlBoolean p)  { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); } }

		sealed class U8_O         : CB<Byte ,object>    { public override Byte C(object p)   { return Convert.ToByte(p); } }

		static CB<T, P> GetByteConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new U8_S        ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new U8_I8       ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new U8_I16      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new U8_I32      ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new U8_I64      ());

			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new U8_U16      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new U8_U32      ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new U8_U64      ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new U8_R4       ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new U8_R8       ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new U8_B        ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new U8_D        ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new U8_C        ());

			// Nullable Types.
			//
			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new U8_NU8      ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new U8_NI8      ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new U8_NI16     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new U8_NI32     ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new U8_NI64     ());

			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new U8_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new U8_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new U8_NU64     ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new U8_NR4      ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new U8_NR8      ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new U8_NB       ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new U8_ND       ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new U8_NC       ());

			// SqlTypes.
			//
			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new U8_dbU8     ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new U8_dbS      ());

			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new U8_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new U8_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new U8_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new U8_dbR4     ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new U8_dbR8     ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new U8_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new U8_dbM      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new U8_dbB      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new U8_O        ());

			return (CB<T, P>)(object)Convert<Byte, object>.I;
		}

		#endregion

		#region UInt16


		// Scalar Types.
		//
		sealed class U16_S       : CB<UInt16,String>     { public override UInt16 C(String p)      { return Convert.ToUInt16(p); } }

		sealed class U16_I8      : CB<UInt16,SByte>      { public override UInt16 C(SByte p)       { return Convert.ToUInt16(p); } }
		sealed class U16_I16     : CB<UInt16,Int16>      { public override UInt16 C(Int16 p)       { return Convert.ToUInt16(p); } }
		sealed class U16_I32     : CB<UInt16,Int32>      { public override UInt16 C(Int32 p)       { return Convert.ToUInt16(p); } }
		sealed class U16_I64     : CB<UInt16,Int64>      { public override UInt16 C(Int64 p)       { return Convert.ToUInt16(p); } }

		sealed class U16_U8      : CB<UInt16,Byte>       { public override UInt16 C(Byte p)        { return Convert.ToUInt16(p); } }
		sealed class U16_U32     : CB<UInt16,UInt32>     { public override UInt16 C(UInt32 p)      { return Convert.ToUInt16(p); } }
		sealed class U16_U64     : CB<UInt16,UInt64>     { public override UInt16 C(UInt64 p)      { return Convert.ToUInt16(p); } }

		sealed class U16_R4      : CB<UInt16,Single>     { public override UInt16 C(Single p)      { return Convert.ToUInt16(p); } }
		sealed class U16_R8      : CB<UInt16,Double>     { public override UInt16 C(Double p)      { return Convert.ToUInt16(p); } }

		sealed class U16_B       : CB<UInt16,Boolean>    { public override UInt16 C(Boolean p)     { return Convert.ToUInt16(p); } }
		sealed class U16_D       : CB<UInt16,Decimal>    { public override UInt16 C(Decimal p)     { return Convert.ToUInt16(p); } }
		sealed class U16_C       : CB<UInt16,Char>       { public override UInt16 C(Char p)        { return Convert.ToUInt16(p); } }

		// Nullable Types.
		//
		sealed class U16_NU16    : CB<UInt16,UInt16?>    { public override UInt16 C(UInt16? p)     { return p.HasValue?                  p.Value : (UInt16)0; } }

		sealed class U16_NI8     : CB<UInt16,SByte?>     { public override UInt16 C(SByte? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NI16    : CB<UInt16,Int16?>     { public override UInt16 C(Int16? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NI32    : CB<UInt16,Int32?>     { public override UInt16 C(Int32? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NI64    : CB<UInt16,Int64?>     { public override UInt16 C(Int64? p)      { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class U16_NU8     : CB<UInt16,Byte?>      { public override UInt16 C(Byte? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NU32    : CB<UInt16,UInt32?>    { public override UInt16 C(UInt32? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NU64    : CB<UInt16,UInt64?>    { public override UInt16 C(UInt64? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class U16_NR4     : CB<UInt16,Single?>    { public override UInt16 C(Single? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NR8     : CB<UInt16,Double?>    { public override UInt16 C(Double? p)     { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		sealed class U16_NB      : CB<UInt16,Boolean?>   { public override UInt16 C(Boolean? p)    { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_ND      : CB<UInt16,Decimal?>   { public override UInt16 C(Decimal? p)    { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }
		sealed class U16_NC      : CB<UInt16,Char?>      { public override UInt16 C(Char? p)       { return p.HasValue? Convert.ToUInt16(p.Value): (UInt16)0; } }

		// SqlTypes.
		//
		sealed class U16_dbS     : CB<UInt16,SqlString>  { public override UInt16 C(SqlString p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class U16_dbU8    : CB<UInt16,SqlByte>    { public override UInt16 C(SqlByte p)     { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbI16   : CB<UInt16,SqlInt16>   { public override UInt16 C(SqlInt16 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbI32   : CB<UInt16,SqlInt32>   { public override UInt16 C(SqlInt32 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbI64   : CB<UInt16,SqlInt64>   { public override UInt16 C(SqlInt64 p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class U16_dbR4    : CB<UInt16,SqlSingle>  { public override UInt16 C(SqlSingle p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbR8    : CB<UInt16,SqlDouble>  { public override UInt16 C(SqlDouble p)   { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbD     : CB<UInt16,SqlDecimal> { public override UInt16 C(SqlDecimal p)  { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }
		sealed class U16_dbM     : CB<UInt16,SqlMoney>   { public override UInt16 C(SqlMoney p)    { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class U16_dbB     : CB<UInt16,SqlBoolean> { public override UInt16 C(SqlBoolean p)  { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); } }

		sealed class U16_O         : CB<UInt16 ,object>    { public override UInt16 C(object p)   { return Convert.ToUInt16(p); } }

		static CB<T, P> GetUInt16Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new U16_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new U16_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new U16_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new U16_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new U16_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new U16_U8      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new U16_U32     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new U16_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new U16_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new U16_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new U16_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new U16_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new U16_C       ());

			// Nullable Types.
			//
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new U16_NU16    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new U16_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new U16_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new U16_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new U16_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new U16_NU8     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new U16_NU32    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new U16_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new U16_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new U16_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new U16_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new U16_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new U16_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new U16_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new U16_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new U16_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new U16_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new U16_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new U16_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new U16_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new U16_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new U16_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new U16_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new U16_O       ());

			return (CB<T, P>)(object)Convert<UInt16, object>.I;
		}

		#endregion

		#region UInt32


		// Scalar Types.
		//
		sealed class U32_S       : CB<UInt32,String>     { public override UInt32 C(String p)      { return Convert.ToUInt32(p); } }

		sealed class U32_I8      : CB<UInt32,SByte>      { public override UInt32 C(SByte p)       { return Convert.ToUInt32(p); } }
		sealed class U32_I16     : CB<UInt32,Int16>      { public override UInt32 C(Int16 p)       { return Convert.ToUInt32(p); } }
		sealed class U32_I32     : CB<UInt32,Int32>      { public override UInt32 C(Int32 p)       { return Convert.ToUInt32(p); } }
		sealed class U32_I64     : CB<UInt32,Int64>      { public override UInt32 C(Int64 p)       { return Convert.ToUInt32(p); } }

		sealed class U32_U8      : CB<UInt32,Byte>       { public override UInt32 C(Byte p)        { return Convert.ToUInt32(p); } }
		sealed class U32_U16     : CB<UInt32,UInt16>     { public override UInt32 C(UInt16 p)      { return Convert.ToUInt32(p); } }
		sealed class U32_U64     : CB<UInt32,UInt64>     { public override UInt32 C(UInt64 p)      { return Convert.ToUInt32(p); } }

		sealed class U32_R4      : CB<UInt32,Single>     { public override UInt32 C(Single p)      { return Convert.ToUInt32(p); } }
		sealed class U32_R8      : CB<UInt32,Double>     { public override UInt32 C(Double p)      { return Convert.ToUInt32(p); } }

		sealed class U32_B       : CB<UInt32,Boolean>    { public override UInt32 C(Boolean p)     { return Convert.ToUInt32(p); } }
		sealed class U32_D       : CB<UInt32,Decimal>    { public override UInt32 C(Decimal p)     { return Convert.ToUInt32(p); } }
		sealed class U32_C       : CB<UInt32,Char>       { public override UInt32 C(Char p)        { return Convert.ToUInt32(p); } }

		// Nullable Types.
		//
		sealed class U32_NU32    : CB<UInt32,UInt32?>    { public override UInt32 C(UInt32? p)     { return p.HasValue?                  p.Value : (UInt32)0; } }

		sealed class U32_NI8     : CB<UInt32,SByte?>     { public override UInt32 C(SByte? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NI16    : CB<UInt32,Int16?>     { public override UInt32 C(Int16? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NI32    : CB<UInt32,Int32?>     { public override UInt32 C(Int32? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NI64    : CB<UInt32,Int64?>     { public override UInt32 C(Int64? p)      { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class U32_NU8     : CB<UInt32,Byte?>      { public override UInt32 C(Byte? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NU16    : CB<UInt32,UInt16?>    { public override UInt32 C(UInt16? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NU64    : CB<UInt32,UInt64?>    { public override UInt32 C(UInt64? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class U32_NR4     : CB<UInt32,Single?>    { public override UInt32 C(Single? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NR8     : CB<UInt32,Double?>    { public override UInt32 C(Double? p)     { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		sealed class U32_NB      : CB<UInt32,Boolean?>   { public override UInt32 C(Boolean? p)    { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_ND      : CB<UInt32,Decimal?>   { public override UInt32 C(Decimal? p)    { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }
		sealed class U32_NC      : CB<UInt32,Char?>      { public override UInt32 C(Char? p)       { return p.HasValue? Convert.ToUInt32(p.Value): (UInt32)0; } }

		// SqlTypes.
		//
		sealed class U32_dbS     : CB<UInt32,SqlString>  { public override UInt32 C(SqlString p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class U32_dbU8    : CB<UInt32,SqlByte>    { public override UInt32 C(SqlByte p)     { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbI16   : CB<UInt32,SqlInt16>   { public override UInt32 C(SqlInt16 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbI32   : CB<UInt32,SqlInt32>   { public override UInt32 C(SqlInt32 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbI64   : CB<UInt32,SqlInt64>   { public override UInt32 C(SqlInt64 p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class U32_dbR4    : CB<UInt32,SqlSingle>  { public override UInt32 C(SqlSingle p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbR8    : CB<UInt32,SqlDouble>  { public override UInt32 C(SqlDouble p)   { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbD     : CB<UInt32,SqlDecimal> { public override UInt32 C(SqlDecimal p)  { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }
		sealed class U32_dbM     : CB<UInt32,SqlMoney>   { public override UInt32 C(SqlMoney p)    { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class U32_dbB     : CB<UInt32,SqlBoolean> { public override UInt32 C(SqlBoolean p)  { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); } }

		sealed class U32_O         : CB<UInt32 ,object>    { public override UInt32 C(object p)   { return Convert.ToUInt32(p); } }

		static CB<T, P> GetUInt32Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new U32_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new U32_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new U32_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new U32_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new U32_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new U32_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new U32_U16     ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new U32_U64     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new U32_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new U32_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new U32_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new U32_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new U32_C       ());

			// Nullable Types.
			//
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new U32_NU32    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new U32_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new U32_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new U32_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new U32_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new U32_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new U32_NU16    ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new U32_NU64    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new U32_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new U32_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new U32_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new U32_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new U32_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new U32_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new U32_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new U32_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new U32_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new U32_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new U32_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new U32_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new U32_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new U32_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new U32_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new U32_O       ());

			return (CB<T, P>)(object)Convert<UInt32, object>.I;
		}

		#endregion

		#region UInt64


		// Scalar Types.
		//
		sealed class U64_S       : CB<UInt64,String>     { public override UInt64 C(String p)      { return Convert.ToUInt64(p); } }

		sealed class U64_I8      : CB<UInt64,SByte>      { public override UInt64 C(SByte p)       { return Convert.ToUInt64(p); } }
		sealed class U64_I16     : CB<UInt64,Int16>      { public override UInt64 C(Int16 p)       { return Convert.ToUInt64(p); } }
		sealed class U64_I32     : CB<UInt64,Int32>      { public override UInt64 C(Int32 p)       { return Convert.ToUInt64(p); } }
		sealed class U64_I64     : CB<UInt64,Int64>      { public override UInt64 C(Int64 p)       { return Convert.ToUInt64(p); } }

		sealed class U64_U8      : CB<UInt64,Byte>       { public override UInt64 C(Byte p)        { return Convert.ToUInt64(p); } }
		sealed class U64_U16     : CB<UInt64,UInt16>     { public override UInt64 C(UInt16 p)      { return Convert.ToUInt64(p); } }
		sealed class U64_U32     : CB<UInt64,UInt32>     { public override UInt64 C(UInt32 p)      { return Convert.ToUInt64(p); } }

		sealed class U64_R4      : CB<UInt64,Single>     { public override UInt64 C(Single p)      { return Convert.ToUInt64(p); } }
		sealed class U64_R8      : CB<UInt64,Double>     { public override UInt64 C(Double p)      { return Convert.ToUInt64(p); } }

		sealed class U64_B       : CB<UInt64,Boolean>    { public override UInt64 C(Boolean p)     { return Convert.ToUInt64(p); } }
		sealed class U64_D       : CB<UInt64,Decimal>    { public override UInt64 C(Decimal p)     { return Convert.ToUInt64(p); } }
		sealed class U64_C       : CB<UInt64,Char>       { public override UInt64 C(Char p)        { return Convert.ToUInt64(p); } }

		// Nullable Types.
		//
		sealed class U64_NU64    : CB<UInt64,UInt64?>    { public override UInt64 C(UInt64? p)     { return p.HasValue?                  p.Value : (UInt64)0; } }

		sealed class U64_NI8     : CB<UInt64,SByte?>     { public override UInt64 C(SByte? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NI16    : CB<UInt64,Int16?>     { public override UInt64 C(Int16? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NI32    : CB<UInt64,Int32?>     { public override UInt64 C(Int32? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NI64    : CB<UInt64,Int64?>     { public override UInt64 C(Int64? p)      { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class U64_NU8     : CB<UInt64,Byte?>      { public override UInt64 C(Byte? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NU16    : CB<UInt64,UInt16?>    { public override UInt64 C(UInt16? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NU32    : CB<UInt64,UInt32?>    { public override UInt64 C(UInt32? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class U64_NR4     : CB<UInt64,Single?>    { public override UInt64 C(Single? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NR8     : CB<UInt64,Double?>    { public override UInt64 C(Double? p)     { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		sealed class U64_NB      : CB<UInt64,Boolean?>   { public override UInt64 C(Boolean? p)    { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_ND      : CB<UInt64,Decimal?>   { public override UInt64 C(Decimal? p)    { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }
		sealed class U64_NC      : CB<UInt64,Char?>      { public override UInt64 C(Char? p)       { return p.HasValue? Convert.ToUInt64(p.Value): (UInt64)0; } }

		// SqlTypes.
		//
		sealed class U64_dbS     : CB<UInt64,SqlString>  { public override UInt64 C(SqlString p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class U64_dbU8    : CB<UInt64,SqlByte>    { public override UInt64 C(SqlByte p)     { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbI16   : CB<UInt64,SqlInt16>   { public override UInt64 C(SqlInt16 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbI32   : CB<UInt64,SqlInt32>   { public override UInt64 C(SqlInt32 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbI64   : CB<UInt64,SqlInt64>   { public override UInt64 C(SqlInt64 p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class U64_dbR4    : CB<UInt64,SqlSingle>  { public override UInt64 C(SqlSingle p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbR8    : CB<UInt64,SqlDouble>  { public override UInt64 C(SqlDouble p)   { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbD     : CB<UInt64,SqlDecimal> { public override UInt64 C(SqlDecimal p)  { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }
		sealed class U64_dbM     : CB<UInt64,SqlMoney>   { public override UInt64 C(SqlMoney p)    { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class U64_dbB     : CB<UInt64,SqlBoolean> { public override UInt64 C(SqlBoolean p)  { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); } }

		sealed class U64_O         : CB<UInt64 ,object>    { public override UInt64 C(object p)   { return Convert.ToUInt64(p); } }

		static CB<T, P> GetUInt64Converter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new U64_S       ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new U64_I8      ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new U64_I16     ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new U64_I32     ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new U64_I64     ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new U64_U8      ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new U64_U16     ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new U64_U32     ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new U64_R4      ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new U64_R8      ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new U64_B       ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new U64_D       ());
			if (t == typeof(Char))        return (CB<T, P>)(object)(new U64_C       ());

			// Nullable Types.
			//
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new U64_NU64    ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new U64_NI8     ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new U64_NI16    ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new U64_NI32    ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new U64_NI64    ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new U64_NU8     ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new U64_NU16    ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new U64_NU32    ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new U64_NR4     ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new U64_NR8     ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new U64_NB      ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new U64_ND      ());
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new U64_NC      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new U64_dbS     ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new U64_dbU8    ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new U64_dbI16   ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new U64_dbI32   ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new U64_dbI64   ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new U64_dbR4    ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new U64_dbR8    ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new U64_dbD     ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new U64_dbM     ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new U64_dbB     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new U64_O       ());

			return (CB<T, P>)(object)Convert<UInt64, object>.I;
		}

		#endregion

		#region Char


		// Scalar Types.
		//
		sealed class C_S         : CB<Char,String>     { public override Char C(String p)      { return Convert.ToChar(p); } }

		sealed class C_I8        : CB<Char,SByte>      { public override Char C(SByte p)       { return Convert.ToChar(p); } }
		sealed class C_I16       : CB<Char,Int16>      { public override Char C(Int16 p)       { return Convert.ToChar(p); } }
		sealed class C_I32       : CB<Char,Int32>      { public override Char C(Int32 p)       { return Convert.ToChar(p); } }
		sealed class C_I64       : CB<Char,Int64>      { public override Char C(Int64 p)       { return Convert.ToChar(p); } }

		sealed class C_U8        : CB<Char,Byte>       { public override Char C(Byte p)        { return Convert.ToChar(p); } }
		sealed class C_U16       : CB<Char,UInt16>     { public override Char C(UInt16 p)      { return Convert.ToChar(p); } }
		sealed class C_U32       : CB<Char,UInt32>     { public override Char C(UInt32 p)      { return Convert.ToChar(p); } }
		sealed class C_U64       : CB<Char,UInt64>     { public override Char C(UInt64 p)      { return Convert.ToChar(p); } }
		sealed class C_B         : CB<Char,Boolean>    { public override Char C(Boolean p)     { return p? '1':'0'; } }

		// Nullable Types.
		//
		sealed class C_NC        : CB<Char,Char?>      { public override Char C(Char? p)       { return p.HasValue?                p.Value : (Char)0; } }

		sealed class C_NI8       : CB<Char,SByte?>     { public override Char C(SByte? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NI16      : CB<Char,Int16?>     { public override Char C(Int16? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NI32      : CB<Char,Int32?>     { public override Char C(Int32? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NI64      : CB<Char,Int64?>     { public override Char C(Int64? p)      { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }

		sealed class C_NU8       : CB<Char,Byte?>      { public override Char C(Byte? p)       { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NU16      : CB<Char,UInt16?>    { public override Char C(UInt16? p)     { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NU32      : CB<Char,UInt32?>    { public override Char C(UInt32? p)     { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NU64      : CB<Char,UInt64?>    { public override Char C(UInt64? p)     { return p.HasValue? Convert.ToChar(p.Value): (Char)0; } }
		sealed class C_NB        : CB<Char,Boolean?>   { public override Char C(Boolean? p)    { return p.HasValue? p.Value? '1':'0'       : (Char)0; } }

		// SqlTypes.
		//
		sealed class C_dbS       : CB<Char,SqlString>  { public override Char C(SqlString p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class C_dbU8      : CB<Char,SqlByte>    { public override Char C(SqlByte p)     { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbI16     : CB<Char,SqlInt16>   { public override Char C(SqlInt16 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbI32     : CB<Char,SqlInt32>   { public override Char C(SqlInt32 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbI64     : CB<Char,SqlInt64>   { public override Char C(SqlInt64 p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class C_dbR4      : CB<Char,SqlSingle>  { public override Char C(SqlSingle p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbR8      : CB<Char,SqlDouble>  { public override Char C(SqlDouble p)   { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbD       : CB<Char,SqlDecimal> { public override Char C(SqlDecimal p)  { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }
		sealed class C_dbM       : CB<Char,SqlMoney>   { public override Char C(SqlMoney p)    { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class C_dbB       : CB<Char,SqlBoolean> { public override Char C(SqlBoolean p)  { return p.IsNull? (Char)0: Convert.ToChar(p.Value); } }

		sealed class C_O         : CB<Char ,object>    { public override Char C(object p)   { return Convert.ToChar(p); } }

		static CB<T, P> GetCharConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new C_S         ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new C_I8        ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new C_I16       ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new C_I32       ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new C_I64       ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new C_U8        ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new C_U16       ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new C_U32       ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new C_U64       ());
			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new C_B         ());

			// Nullable Types.
			//
			if (t == typeof(Char?))       return (CB<T, P>)(object)(new C_NC        ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new C_NI8       ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new C_NI16      ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new C_NI32      ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new C_NI64      ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new C_NU8       ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new C_NU16      ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new C_NU32      ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new C_NU64      ());
			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new C_NB        ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new C_dbS       ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new C_dbU8      ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new C_dbI16     ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new C_dbI32     ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new C_dbI64     ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new C_dbR4      ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new C_dbR8      ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new C_dbD       ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new C_dbM       ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new C_dbB       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new C_O         ());

			return (CB<T, P>)(object)Convert<Char, object>.I;
		}

		#endregion

		#region Single


		// Scalar Types.
		//
		sealed class R4_S        : CB<Single,String>     { public override Single C(String p)      { return Convert.ToSingle(p); } }

		sealed class R4_I8       : CB<Single,SByte>      { public override Single C(SByte p)       { return Convert.ToSingle(p); } }
		sealed class R4_I16      : CB<Single,Int16>      { public override Single C(Int16 p)       { return Convert.ToSingle(p); } }
		sealed class R4_I32      : CB<Single,Int32>      { public override Single C(Int32 p)       { return Convert.ToSingle(p); } }
		sealed class R4_I64      : CB<Single,Int64>      { public override Single C(Int64 p)       { return Convert.ToSingle(p); } }

		sealed class R4_U8       : CB<Single,Byte>       { public override Single C(Byte p)        { return Convert.ToSingle(p); } }
		sealed class R4_U16      : CB<Single,UInt16>     { public override Single C(UInt16 p)      { return Convert.ToSingle(p); } }
		sealed class R4_U32      : CB<Single,UInt32>     { public override Single C(UInt32 p)      { return Convert.ToSingle(p); } }
		sealed class R4_U64      : CB<Single,UInt64>     { public override Single C(UInt64 p)      { return Convert.ToSingle(p); } }

		sealed class R4_R8       : CB<Single,Double>     { public override Single C(Double p)      { return Convert.ToSingle(p); } }

		sealed class R4_B        : CB<Single,Boolean>    { public override Single C(Boolean p)     { return Convert.ToSingle(p); } }
		sealed class R4_D        : CB<Single,Decimal>    { public override Single C(Decimal p)     { return Convert.ToSingle(p); } }

		// Nullable Types.
		//
		sealed class R4_NR4      : CB<Single,Single?>    { public override Single C(Single? p)     { return p.HasValue?                  p.Value : 0; } }

		sealed class R4_NI8      : CB<Single,SByte?>     { public override Single C(SByte? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NI16     : CB<Single,Int16?>     { public override Single C(Int16? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NI32     : CB<Single,Int32?>     { public override Single C(Int32? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NI64     : CB<Single,Int64?>     { public override Single C(Int64? p)      { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class R4_NU8      : CB<Single,Byte?>      { public override Single C(Byte? p)       { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NU16     : CB<Single,UInt16?>    { public override Single C(UInt16? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NU32     : CB<Single,UInt32?>    { public override Single C(UInt32? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_NU64     : CB<Single,UInt64?>    { public override Single C(UInt64? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class R4_NR8      : CB<Single,Double?>    { public override Single C(Double? p)     { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		sealed class R4_NB       : CB<Single,Boolean?>   { public override Single C(Boolean? p)    { return p.HasValue? Convert.ToSingle(p.Value): 0; } }
		sealed class R4_ND       : CB<Single,Decimal?>   { public override Single C(Decimal? p)    { return p.HasValue? Convert.ToSingle(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class R4_dbR4     : CB<Single,SqlSingle>  { public override Single C(SqlSingle p)   { return p.IsNull? 0:                  p.Value;  } }
		sealed class R4_dbS      : CB<Single,SqlString>  { public override Single C(SqlString p)   { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class R4_dbU8     : CB<Single,SqlByte>    { public override Single C(SqlByte p)     { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class R4_dbI16    : CB<Single,SqlInt16>   { public override Single C(SqlInt16 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class R4_dbI32    : CB<Single,SqlInt32>   { public override Single C(SqlInt32 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class R4_dbI64    : CB<Single,SqlInt64>   { public override Single C(SqlInt64 p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class R4_dbR8     : CB<Single,SqlDouble>  { public override Single C(SqlDouble p)   { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class R4_dbD      : CB<Single,SqlDecimal> { public override Single C(SqlDecimal p)  { return p.IsNull? 0: Convert.ToSingle(p.Value); } }
		sealed class R4_dbM      : CB<Single,SqlMoney>   { public override Single C(SqlMoney p)    { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class R4_dbB      : CB<Single,SqlBoolean> { public override Single C(SqlBoolean p)  { return p.IsNull? 0: Convert.ToSingle(p.Value); } }

		sealed class R4_O         : CB<Single ,object>    { public override Single C(object p)   { return Convert.ToSingle(p); } }

		static CB<T, P> GetSingleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new R4_S        ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new R4_I8       ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new R4_I16      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new R4_I32      ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new R4_I64      ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new R4_U8       ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new R4_U16      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new R4_U32      ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new R4_U64      ());

			if (t == typeof(Double))      return (CB<T, P>)(object)(new R4_R8       ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new R4_B        ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new R4_D        ());

			// Nullable Types.
			//
			if (t == typeof(Single?))     return (CB<T, P>)(object)(new R4_NR4      ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new R4_NI8      ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new R4_NI16     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new R4_NI32     ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new R4_NI64     ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new R4_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new R4_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new R4_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new R4_NU64     ());

			if (t == typeof(Double?))     return (CB<T, P>)(object)(new R4_NR8      ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new R4_NB       ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new R4_ND       ());

			// SqlTypes.
			//
			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new R4_dbR4     ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new R4_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new R4_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new R4_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new R4_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new R4_dbI64    ());

			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new R4_dbR8     ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new R4_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new R4_dbM      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new R4_dbB      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new R4_O        ());

			return (CB<T, P>)(object)Convert<Single, object>.I;
		}

		#endregion

		#region Double


		// Scalar Types.
		//
		sealed class R8_S        : CB<Double,String>     { public override Double C(String p)      { return Convert.ToDouble(p); } }

		sealed class R8_I8       : CB<Double,SByte>      { public override Double C(SByte p)       { return Convert.ToDouble(p); } }
		sealed class R8_I16      : CB<Double,Int16>      { public override Double C(Int16 p)       { return Convert.ToDouble(p); } }
		sealed class R8_I32      : CB<Double,Int32>      { public override Double C(Int32 p)       { return Convert.ToDouble(p); } }
		sealed class R8_I64      : CB<Double,Int64>      { public override Double C(Int64 p)       { return Convert.ToDouble(p); } }

		sealed class R8_U8       : CB<Double,Byte>       { public override Double C(Byte p)        { return Convert.ToDouble(p); } }
		sealed class R8_U16      : CB<Double,UInt16>     { public override Double C(UInt16 p)      { return Convert.ToDouble(p); } }
		sealed class R8_U32      : CB<Double,UInt32>     { public override Double C(UInt32 p)      { return Convert.ToDouble(p); } }
		sealed class R8_U64      : CB<Double,UInt64>     { public override Double C(UInt64 p)      { return Convert.ToDouble(p); } }

		sealed class R8_R4       : CB<Double,Single>     { public override Double C(Single p)      { return Convert.ToDouble(p); } }

		sealed class R8_B        : CB<Double,Boolean>    { public override Double C(Boolean p)     { return Convert.ToDouble(p); } }
		sealed class R8_D        : CB<Double,Decimal>    { public override Double C(Decimal p)     { return Convert.ToDouble(p); } }

		// Nullable Types.
		//
		sealed class R8_NR8      : CB<Double,Double?>    { public override Double C(Double? p)     { return p.HasValue?                  p.Value : 0; } }

		sealed class R8_NI8      : CB<Double,SByte?>     { public override Double C(SByte? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NI16     : CB<Double,Int16?>     { public override Double C(Int16? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NI32     : CB<Double,Int32?>     { public override Double C(Int32? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NI64     : CB<Double,Int64?>     { public override Double C(Int64? p)      { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class R8_NU8      : CB<Double,Byte?>      { public override Double C(Byte? p)       { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NU16     : CB<Double,UInt16?>    { public override Double C(UInt16? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NU32     : CB<Double,UInt32?>    { public override Double C(UInt32? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_NU64     : CB<Double,UInt64?>    { public override Double C(UInt64? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class R8_NR4      : CB<Double,Single?>    { public override Double C(Single? p)     { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		sealed class R8_NB       : CB<Double,Boolean?>   { public override Double C(Boolean? p)    { return p.HasValue? Convert.ToDouble(p.Value): 0; } }
		sealed class R8_ND       : CB<Double,Decimal?>   { public override Double C(Decimal? p)    { return p.HasValue? Convert.ToDouble(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class R8_dbR8     : CB<Double,SqlDouble>  { public override Double C(SqlDouble p)   { return p.IsNull? 0:                  p.Value;  } }
		sealed class R8_dbS      : CB<Double,SqlString>  { public override Double C(SqlString p)   { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class R8_dbU8     : CB<Double,SqlByte>    { public override Double C(SqlByte p)     { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class R8_dbI16    : CB<Double,SqlInt16>   { public override Double C(SqlInt16 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class R8_dbI32    : CB<Double,SqlInt32>   { public override Double C(SqlInt32 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class R8_dbI64    : CB<Double,SqlInt64>   { public override Double C(SqlInt64 p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class R8_dbR4     : CB<Double,SqlSingle>  { public override Double C(SqlSingle p)   { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class R8_dbD      : CB<Double,SqlDecimal> { public override Double C(SqlDecimal p)  { return p.IsNull? 0: Convert.ToDouble(p.Value); } }
		sealed class R8_dbM      : CB<Double,SqlMoney>   { public override Double C(SqlMoney p)    { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class R8_dbB      : CB<Double,SqlBoolean> { public override Double C(SqlBoolean p)  { return p.IsNull? 0: Convert.ToDouble(p.Value); } }

		sealed class R8_O         : CB<Double ,object>    { public override Double C(object p)   { return Convert.ToDouble(p); } }

		static CB<T, P> GetDoubleConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new R8_S        ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new R8_I8       ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new R8_I16      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new R8_I32      ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new R8_I64      ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new R8_U8       ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new R8_U16      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new R8_U32      ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new R8_U64      ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new R8_R4       ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new R8_B        ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new R8_D        ());

			// Nullable Types.
			//
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new R8_NR8      ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new R8_NI8      ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new R8_NI16     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new R8_NI32     ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new R8_NI64     ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new R8_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new R8_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new R8_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new R8_NU64     ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new R8_NR4      ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new R8_NB       ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new R8_ND       ());

			// SqlTypes.
			//
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new R8_dbR8     ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new R8_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new R8_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new R8_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new R8_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new R8_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new R8_dbR4     ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new R8_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new R8_dbM      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new R8_dbB      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new R8_O        ());

			return (CB<T, P>)(object)Convert<Double, object>.I;
		}

		#endregion

		#region Boolean


		// Scalar Types.
		//
		sealed class B_S         : CB<Boolean,String>     { public override Boolean C(String p)      { return Convert.ToBoolean(p); } }

		sealed class B_I8        : CB<Boolean,SByte>      { public override Boolean C(SByte p)       { return Convert.ToBoolean(p); } }
		sealed class B_I16       : CB<Boolean,Int16>      { public override Boolean C(Int16 p)       { return Convert.ToBoolean(p); } }
		sealed class B_I32       : CB<Boolean,Int32>      { public override Boolean C(Int32 p)       { return Convert.ToBoolean(p); } }
		sealed class B_I64       : CB<Boolean,Int64>      { public override Boolean C(Int64 p)       { return Convert.ToBoolean(p); } }

		sealed class B_U8        : CB<Boolean,Byte>       { public override Boolean C(Byte p)        { return Convert.ToBoolean(p); } }
		sealed class B_U16       : CB<Boolean,UInt16>     { public override Boolean C(UInt16 p)      { return Convert.ToBoolean(p); } }
		sealed class B_U32       : CB<Boolean,UInt32>     { public override Boolean C(UInt32 p)      { return Convert.ToBoolean(p); } }
		sealed class B_U64       : CB<Boolean,UInt64>     { public override Boolean C(UInt64 p)      { return Convert.ToBoolean(p); } }

		sealed class B_R4        : CB<Boolean,Single>     { public override Boolean C(Single p)      { return Convert.ToBoolean(p); } }
		sealed class B_R8        : CB<Boolean,Double>     { public override Boolean C(Double p)      { return Convert.ToBoolean(p); } }

		sealed class B_D         : CB<Boolean,Decimal>    { public override Boolean C(Decimal p)     { return Convert.ToBoolean(p); } }

		sealed class B_C         : CB<Boolean,Char>       { public override Boolean C(Char p)       
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
		sealed class B_NB        : CB<Boolean,Boolean?>   { public override Boolean C(Boolean? p)    { return p.HasValue? p.Value                   : false; } }

		sealed class B_NI8       : CB<Boolean,SByte?>     { public override Boolean C(SByte? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NI16      : CB<Boolean,Int16?>     { public override Boolean C(Int16? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NI32      : CB<Boolean,Int32?>     { public override Boolean C(Int32? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NI64      : CB<Boolean,Int64?>     { public override Boolean C(Int64? p)      { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class B_NU8       : CB<Boolean,Byte?>      { public override Boolean C(Byte? p)       { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NU16      : CB<Boolean,UInt16?>    { public override Boolean C(UInt16? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NU32      : CB<Boolean,UInt32?>    { public override Boolean C(UInt32? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NU64      : CB<Boolean,UInt64?>    { public override Boolean C(UInt64? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class B_NR4       : CB<Boolean,Single?>    { public override Boolean C(Single? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }
		sealed class B_NR8       : CB<Boolean,Double?>    { public override Boolean C(Double? p)     { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class B_ND        : CB<Boolean,Decimal?>   { public override Boolean C(Decimal? p)    { return p.HasValue? Convert.ToBoolean(p.Value): false; } }

		sealed class B_NC        : CB<Boolean,Char?>      { public override Boolean C(Char? p)       { return (p.HasValue)? Convert<Boolean,Char>.I.C(p.Value): false; } }

		// SqlTypes.
		//
		sealed class B_dbB       : CB<Boolean,SqlBoolean> { public override Boolean C(SqlBoolean p)  { return p.IsNull? false:                   p.Value;  } }
		sealed class B_dbS       : CB<Boolean,SqlString>  { public override Boolean C(SqlString p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }

		sealed class B_dbU8      : CB<Boolean,SqlByte>    { public override Boolean C(SqlByte p)     { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbI16     : CB<Boolean,SqlInt16>   { public override Boolean C(SqlInt16 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbI32     : CB<Boolean,SqlInt32>   { public override Boolean C(SqlInt32 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbI64     : CB<Boolean,SqlInt64>   { public override Boolean C(SqlInt64 p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }

		sealed class B_dbR4      : CB<Boolean,SqlSingle>  { public override Boolean C(SqlSingle p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbR8      : CB<Boolean,SqlDouble>  { public override Boolean C(SqlDouble p)   { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbD       : CB<Boolean,SqlDecimal> { public override Boolean C(SqlDecimal p)  { return p.IsNull? false: Convert.ToBoolean(p.Value); } }
		sealed class B_dbM       : CB<Boolean,SqlMoney>   { public override Boolean C(SqlMoney p)    { return p.IsNull? false: Convert.ToBoolean(p.Value); } }


		sealed class B_O         : CB<Boolean ,object>    { public override Boolean C(object p)   { return Convert.ToBoolean(p); } }

		static CB<T, P> GetBooleanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new B_S         ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new B_I8        ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new B_I16       ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new B_I32       ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new B_I64       ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new B_U8        ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new B_U16       ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new B_U32       ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new B_U64       ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new B_R4        ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new B_R8        ());

			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new B_D         ());

			if (t == typeof(Char))        return (CB<T, P>)(object)(new B_C         ());

			// Nullable Types.
			//
			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new B_NB        ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new B_NI8       ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new B_NI16      ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new B_NI32      ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new B_NI64      ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new B_NU8       ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new B_NU16      ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new B_NU32      ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new B_NU64      ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new B_NR4       ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new B_NR8       ());

			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new B_ND        ());

			if (t == typeof(Char?))       return (CB<T, P>)(object)(new B_NC        ());

			// SqlTypes.
			//
			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new B_dbB       ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new B_dbS       ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new B_dbU8      ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new B_dbI16     ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new B_dbI32     ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new B_dbI64     ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new B_dbR4      ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new B_dbR8      ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new B_dbD       ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new B_dbM       ());


			if (t == typeof(object))      return (CB<T, P>)(object)(new B_O         ());

			return (CB<T, P>)(object)Convert<Boolean, object>.I;
		}

		#endregion

		#region Decimal


		// Scalar Types.
		//
		sealed class D_S         : CB<Decimal,String>     { public override Decimal C(String p)      { return Convert.ToDecimal(p); } }

		sealed class D_I8        : CB<Decimal,SByte>      { public override Decimal C(SByte p)       { return Convert.ToDecimal(p); } }
		sealed class D_I16       : CB<Decimal,Int16>      { public override Decimal C(Int16 p)       { return Convert.ToDecimal(p); } }
		sealed class D_I32       : CB<Decimal,Int32>      { public override Decimal C(Int32 p)       { return Convert.ToDecimal(p); } }
		sealed class D_I64       : CB<Decimal,Int64>      { public override Decimal C(Int64 p)       { return Convert.ToDecimal(p); } }

		sealed class D_U8        : CB<Decimal,Byte>       { public override Decimal C(Byte p)        { return Convert.ToDecimal(p); } }
		sealed class D_U16       : CB<Decimal,UInt16>     { public override Decimal C(UInt16 p)      { return Convert.ToDecimal(p); } }
		sealed class D_U32       : CB<Decimal,UInt32>     { public override Decimal C(UInt32 p)      { return Convert.ToDecimal(p); } }
		sealed class D_U64       : CB<Decimal,UInt64>     { public override Decimal C(UInt64 p)      { return Convert.ToDecimal(p); } }

		sealed class D_R4        : CB<Decimal,Single>     { public override Decimal C(Single p)      { return Convert.ToDecimal(p); } }
		sealed class D_R8        : CB<Decimal,Double>     { public override Decimal C(Double p)      { return Convert.ToDecimal(p); } }

		sealed class D_B         : CB<Decimal,Boolean>    { public override Decimal C(Boolean p)     { return Convert.ToDecimal(p); } }

		// Nullable Types.
		//
		sealed class D_ND        : CB<Decimal,Decimal?>   { public override Decimal C(Decimal? p)    { return p.HasValue?                   p.Value : 0; } }

		sealed class D_NI8       : CB<Decimal,SByte?>     { public override Decimal C(SByte? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NI16      : CB<Decimal,Int16?>     { public override Decimal C(Int16? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NI32      : CB<Decimal,Int32?>     { public override Decimal C(Int32? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NI64      : CB<Decimal,Int64?>     { public override Decimal C(Int64? p)      { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class D_NU8       : CB<Decimal,Byte?>      { public override Decimal C(Byte? p)       { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NU16      : CB<Decimal,UInt16?>    { public override Decimal C(UInt16? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NU32      : CB<Decimal,UInt32?>    { public override Decimal C(UInt32? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NU64      : CB<Decimal,UInt64?>    { public override Decimal C(UInt64? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class D_NR4       : CB<Decimal,Single?>    { public override Decimal C(Single? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }
		sealed class D_NR8       : CB<Decimal,Double?>    { public override Decimal C(Double? p)     { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		sealed class D_NB        : CB<Decimal,Boolean?>   { public override Decimal C(Boolean? p)    { return p.HasValue? Convert.ToDecimal(p.Value): 0; } }

		// SqlTypes.
		//
		sealed class D_dbD       : CB<Decimal,SqlDecimal> { public override Decimal C(SqlDecimal p)  { return p.IsNull? 0:                   p.Value;  } }
		sealed class D_dbM       : CB<Decimal,SqlMoney>   { public override Decimal C(SqlMoney p)    { return p.IsNull? 0:                   p.Value;  } }
		sealed class D_dbS       : CB<Decimal,SqlString>  { public override Decimal C(SqlString p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class D_dbU8      : CB<Decimal,SqlByte>    { public override Decimal C(SqlByte p)     { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class D_dbI16     : CB<Decimal,SqlInt16>   { public override Decimal C(SqlInt16 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class D_dbI32     : CB<Decimal,SqlInt32>   { public override Decimal C(SqlInt32 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class D_dbI64     : CB<Decimal,SqlInt64>   { public override Decimal C(SqlInt64 p)    { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class D_dbR4      : CB<Decimal,SqlSingle>  { public override Decimal C(SqlSingle p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }
		sealed class D_dbR8      : CB<Decimal,SqlDouble>  { public override Decimal C(SqlDouble p)   { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class D_dbB       : CB<Decimal,SqlBoolean> { public override Decimal C(SqlBoolean p)  { return p.IsNull? 0: Convert.ToDecimal(p.Value); } }

		sealed class D_O         : CB<Decimal ,object>    { public override Decimal C(object p)   { return Convert.ToDecimal(p); } }

		static CB<T, P> GetDecimalConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new D_S         ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new D_I8        ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new D_I16       ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new D_I32       ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new D_I64       ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new D_U8        ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new D_U16       ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new D_U32       ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new D_U64       ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new D_R4        ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new D_R8        ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new D_B         ());

			// Nullable Types.
			//
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new D_ND        ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new D_NI8       ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new D_NI16      ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new D_NI32      ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new D_NI64      ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new D_NU8       ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new D_NU16      ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new D_NU32      ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new D_NU64      ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new D_NR4       ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new D_NR8       ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new D_NB        ());

			// SqlTypes.
			//
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new D_dbD       ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new D_dbM       ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new D_dbS       ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new D_dbU8      ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new D_dbI16     ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new D_dbI32     ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new D_dbI64     ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new D_dbR4      ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new D_dbR8      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new D_dbB       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new D_O         ());

			return (CB<T, P>)(object)Convert<Decimal, object>.I;
		}

		#endregion

		#region DateTime


		// Scalar Types.
		//
		sealed class DT_S        : CB<DateTime,String>     { public override DateTime C(String p)      { return Convert.ToDateTime(p); } }

		sealed class DT_I8       : CB<DateTime,SByte>      { public override DateTime C(SByte p)       { return Convert.ToDateTime(p); } }
		sealed class DT_I16      : CB<DateTime,Int16>      { public override DateTime C(Int16 p)       { return Convert.ToDateTime(p); } }
		sealed class DT_I32      : CB<DateTime,Int32>      { public override DateTime C(Int32 p)       { return Convert.ToDateTime(p); } }
		sealed class DT_I64      : CB<DateTime,Int64>      { public override DateTime C(Int64 p)       { return Convert.ToDateTime(p); } }

		sealed class DT_U8       : CB<DateTime,Byte>       { public override DateTime C(Byte p)        { return Convert.ToDateTime(p); } }
		sealed class DT_U16      : CB<DateTime,UInt16>     { public override DateTime C(UInt16 p)      { return Convert.ToDateTime(p); } }
		sealed class DT_U32      : CB<DateTime,UInt32>     { public override DateTime C(UInt32 p)      { return Convert.ToDateTime(p); } }
		sealed class DT_U64      : CB<DateTime,UInt64>     { public override DateTime C(UInt64 p)      { return Convert.ToDateTime(p); } }

		sealed class DT_R4       : CB<DateTime,Single>     { public override DateTime C(Single p)      { return Convert.ToDateTime(p); } }
		sealed class DT_R8       : CB<DateTime,Double>     { public override DateTime C(Double p)      { return Convert.ToDateTime(p); } }

		sealed class DT_B        : CB<DateTime,Boolean>    { public override DateTime C(Boolean p)     { return Convert.ToDateTime(p); } }
		sealed class DT_D        : CB<DateTime,Decimal>    { public override DateTime C(Decimal p)     { return Convert.ToDateTime(p); } }

		// Nullable Types.
		//
		sealed class DT_NDT      : CB<DateTime,DateTime?>  { public override DateTime C(DateTime? p)   { return p.HasValue?                    p.Value : DateTime.MinValue; } }

		sealed class DT_NI8      : CB<DateTime,SByte?>     { public override DateTime C(SByte? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NI16     : CB<DateTime,Int16?>     { public override DateTime C(Int16? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NI32     : CB<DateTime,Int32?>     { public override DateTime C(Int32? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NI64     : CB<DateTime,Int64?>     { public override DateTime C(Int64? p)      { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DT_NU8      : CB<DateTime,Byte?>      { public override DateTime C(Byte? p)       { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NU16     : CB<DateTime,UInt16?>    { public override DateTime C(UInt16? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NU32     : CB<DateTime,UInt32?>    { public override DateTime C(UInt32? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NU64     : CB<DateTime,UInt64?>    { public override DateTime C(UInt64? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DT_NR4      : CB<DateTime,Single?>    { public override DateTime C(Single? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_NR8      : CB<DateTime,Double?>    { public override DateTime C(Double? p)     { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		sealed class DT_NB       : CB<DateTime,Boolean?>   { public override DateTime C(Boolean? p)    { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }
		sealed class DT_ND       : CB<DateTime,Decimal?>   { public override DateTime C(Decimal? p)    { return p.HasValue? Convert.ToDateTime(p.Value): DateTime.MinValue; } }

		// SqlTypes.
		//
		sealed class DT_dbDT     : CB<DateTime,SqlDateTime>{ public override DateTime C(SqlDateTime p) { return p.IsNull? DateTime.MinValue:                    p.Value;  } }
		sealed class DT_dbS      : CB<DateTime,SqlString>  { public override DateTime C(SqlString p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DT_dbU8     : CB<DateTime,SqlByte>    { public override DateTime C(SqlByte p)     { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbI16    : CB<DateTime,SqlInt16>   { public override DateTime C(SqlInt16 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbI32    : CB<DateTime,SqlInt32>   { public override DateTime C(SqlInt32 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbI64    : CB<DateTime,SqlInt64>   { public override DateTime C(SqlInt64 p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DT_dbR4     : CB<DateTime,SqlSingle>  { public override DateTime C(SqlSingle p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbR8     : CB<DateTime,SqlDouble>  { public override DateTime C(SqlDouble p)   { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbD      : CB<DateTime,SqlDecimal> { public override DateTime C(SqlDecimal p)  { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }
		sealed class DT_dbM      : CB<DateTime,SqlMoney>   { public override DateTime C(SqlMoney p)    { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DT_dbB      : CB<DateTime,SqlBoolean> { public override DateTime C(SqlBoolean p)  { return p.IsNull? DateTime.MinValue: Convert.ToDateTime(p.Value); } }

		sealed class DT_O         : CB<DateTime ,object>    { public override DateTime C(object p)   { return Convert.ToDateTime(p); } }

		static CB<T, P> GetDateTimeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new DT_S        ());

			if (t == typeof(SByte))       return (CB<T, P>)(object)(new DT_I8       ());
			if (t == typeof(Int16))       return (CB<T, P>)(object)(new DT_I16      ());
			if (t == typeof(Int32))       return (CB<T, P>)(object)(new DT_I32      ());
			if (t == typeof(Int64))       return (CB<T, P>)(object)(new DT_I64      ());

			if (t == typeof(Byte))        return (CB<T, P>)(object)(new DT_U8       ());
			if (t == typeof(UInt16))      return (CB<T, P>)(object)(new DT_U16      ());
			if (t == typeof(UInt32))      return (CB<T, P>)(object)(new DT_U32      ());
			if (t == typeof(UInt64))      return (CB<T, P>)(object)(new DT_U64      ());

			if (t == typeof(Single))      return (CB<T, P>)(object)(new DT_R4       ());
			if (t == typeof(Double))      return (CB<T, P>)(object)(new DT_R8       ());

			if (t == typeof(Boolean))     return (CB<T, P>)(object)(new DT_B        ());
			if (t == typeof(Decimal))     return (CB<T, P>)(object)(new DT_D        ());

			// Nullable Types.
			//
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new DT_NDT      ());

			if (t == typeof(SByte?))      return (CB<T, P>)(object)(new DT_NI8      ());
			if (t == typeof(Int16?))      return (CB<T, P>)(object)(new DT_NI16     ());
			if (t == typeof(Int32?))      return (CB<T, P>)(object)(new DT_NI32     ());
			if (t == typeof(Int64?))      return (CB<T, P>)(object)(new DT_NI64     ());

			if (t == typeof(Byte?))       return (CB<T, P>)(object)(new DT_NU8      ());
			if (t == typeof(UInt16?))     return (CB<T, P>)(object)(new DT_NU16     ());
			if (t == typeof(UInt32?))     return (CB<T, P>)(object)(new DT_NU32     ());
			if (t == typeof(UInt64?))     return (CB<T, P>)(object)(new DT_NU64     ());

			if (t == typeof(Single?))     return (CB<T, P>)(object)(new DT_NR4      ());
			if (t == typeof(Double?))     return (CB<T, P>)(object)(new DT_NR8      ());

			if (t == typeof(Boolean?))    return (CB<T, P>)(object)(new DT_NB       ());
			if (t == typeof(Decimal?))    return (CB<T, P>)(object)(new DT_ND       ());

			// SqlTypes.
			//
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new DT_dbDT     ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new DT_dbS      ());

			if (t == typeof(SqlByte))     return (CB<T, P>)(object)(new DT_dbU8     ());
			if (t == typeof(SqlInt16))    return (CB<T, P>)(object)(new DT_dbI16    ());
			if (t == typeof(SqlInt32))    return (CB<T, P>)(object)(new DT_dbI32    ());
			if (t == typeof(SqlInt64))    return (CB<T, P>)(object)(new DT_dbI64    ());

			if (t == typeof(SqlSingle))   return (CB<T, P>)(object)(new DT_dbR4     ());
			if (t == typeof(SqlDouble))   return (CB<T, P>)(object)(new DT_dbR8     ());
			if (t == typeof(SqlDecimal))  return (CB<T, P>)(object)(new DT_dbD      ());
			if (t == typeof(SqlMoney))    return (CB<T, P>)(object)(new DT_dbM      ());

			if (t == typeof(SqlBoolean))  return (CB<T, P>)(object)(new DT_dbB      ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new DT_O        ());

			return (CB<T, P>)(object)Convert<DateTime, object>.I;
		}

		#endregion

		#region TimeSpan


		// Scalar Types.
		//
		sealed class TS_S        : CB<TimeSpan,String>     { public override TimeSpan C(String p)      { return p == null? TimeSpan.MinValue: TimeSpan.Parse(p); } }
		sealed class TS_DT       : CB<TimeSpan,DateTime>   { public override TimeSpan C(DateTime p)    { return p - DateTime.MinValue; } }

		// Nullable Types.
		//
		sealed class TS_NTS      : CB<TimeSpan,TimeSpan?>  { public override TimeSpan C(TimeSpan? p)   { return p.HasValue? p.Value                    : TimeSpan.MinValue; } }
		sealed class TS_NDT      : CB<TimeSpan,DateTime?>  { public override TimeSpan C(DateTime? p)   { return p.HasValue? p.Value - DateTime.MinValue: TimeSpan.MinValue; } }

		// SqlTypes.
		//
		sealed class TS_dbS      : CB<TimeSpan,SqlString>  { public override TimeSpan C(SqlString p)   { return p.IsNull? TimeSpan.MinValue: TimeSpan.Parse(p.Value);     } }
		sealed class TS_dbDT     : CB<TimeSpan,SqlDateTime>{ public override TimeSpan C(SqlDateTime p) { return p.IsNull? TimeSpan.MinValue: p.Value - DateTime.MinValue; } }

		sealed class TS_O         : CB<TimeSpan ,object>    { public override TimeSpan C(object p)  
			{
				if (p == null) return TimeSpan.MinValue;

				// Scalar Types.
				//
				if (p is String)      return Convert<TimeSpan,String>     .I.C((String)p);
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .I.C((DateTime)p);

				// Nullable Types.
				//
				if (p is TimeSpan)    return Convert<TimeSpan,TimeSpan>   .I.C((TimeSpan)p);
				if (p is DateTime)    return Convert<TimeSpan,DateTime>   .I.C((DateTime)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<TimeSpan,SqlString>  .I.C((SqlString)p);
				if (p is SqlDateTime) return Convert<TimeSpan,SqlDateTime>.I.C((SqlDateTime)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetTimeSpanConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new TS_S        ());
			if (t == typeof(DateTime))    return (CB<T, P>)(object)(new TS_DT       ());

			// Nullable Types.
			//
			if (t == typeof(TimeSpan?))   return (CB<T, P>)(object)(new TS_NTS      ());
			if (t == typeof(DateTime?))   return (CB<T, P>)(object)(new TS_NDT      ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new TS_dbS      ());
			if (t == typeof(SqlDateTime)) return (CB<T, P>)(object)(new TS_dbDT     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new TS_O        ());

			return (CB<T, P>)(object)Convert<TimeSpan, object>.I;
		}

		#endregion

		#region Guid


		// Scalar Types.
		//
		sealed class G_S         : CB<Guid,String>     { public override Guid C(String p)      { return p == null? Guid.Empty: new Guid(p); } }

		// Nullable Types.
		//
		sealed class G_NG        : CB<Guid,Guid?>      { public override Guid C(Guid? p)       { return p.HasValue? p.Value : Guid.Empty; } }

		// SqlTypes.
		//
		sealed class G_dbG       : CB<Guid,SqlGuid>    { public override Guid C(SqlGuid p)     { return p.IsNull? Guid.Empty: p.Value;             } }
		sealed class G_dbS       : CB<Guid,SqlString>  { public override Guid C(SqlString p)   { return p.IsNull? Guid.Empty: new Guid(p.Value);   } }
		sealed class G_dbBin     : CB<Guid,SqlBinary>  { public override Guid C(SqlBinary p)   { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; } }
		sealed class G_T         : CB<Guid,Type>       { public override Guid C(Type p)        { return p == null? Guid.Empty: p.GUID; } }

		sealed class G_O         : CB<Guid ,object>    { public override Guid C(object p)  
			{
				if (p == null) return Guid.Empty;

				// Scalar Types.
				//
				if (p is String)      return Convert<Guid,String>     .I.C((String)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Guid,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlGuid)     return Convert<Guid,SqlGuid>    .I.C((SqlGuid)p);
				if (p is SqlString)   return Convert<Guid,SqlString>  .I.C((SqlString)p);
				if (p is SqlBinary)   return Convert<Guid,SqlBinary>  .I.C((SqlBinary)p);
				if (p is Type)        return Convert<Guid,Type>       .I.C((Type)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetGuidConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new G_S         ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new G_NG        ());

			// SqlTypes.
			//
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new G_dbG       ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new G_dbS       ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new G_dbBin     ());
			if (t == typeof(Type))        return (CB<T, P>)(object)(new G_T         ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new G_O         ());

			return (CB<T, P>)(object)Convert<Guid, object>.I;
		}

		#endregion

		#region Stream


		// Scalar Types.
		//
		sealed class IOS_AU8     : CB<Stream,Byte[]>     { public override Stream C(Byte[] p)      { return p == null? Stream.Null: new MemoryStream(p); } }

		// SqlTypes.
		//
		sealed class IOS_dbAU8   : CB<Stream,SqlBytes>   { public override Stream C(SqlBytes p)    { return p.IsNull? Stream.Null: p.Stream;                  } }
		sealed class IOS_dbBin   : CB<Stream,SqlBinary>  { public override Stream C(SqlBinary p)   { return p.IsNull? Stream.Null: new MemoryStream(p.Value); } }

		sealed class IOS_O         : CB<Stream ,object>    { public override Stream C(object p)  
			{
				if (p == null) return Stream.Null;

				// Scalar Types.
				//
				if (p is Byte[])      return Convert<Stream,Byte[]>     .I.C((Byte[])p);

				// SqlTypes.
				//
				if (p is SqlBytes)    return Convert<Stream,SqlBytes>   .I.C((SqlBytes)p);
				if (p is SqlBinary)   return Convert<Stream,SqlBinary>  .I.C((SqlBinary)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetStreamConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new IOS_AU8     ());

			// SqlTypes.
			//
			if (t == typeof(SqlBytes))    return (CB<T, P>)(object)(new IOS_dbAU8   ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new IOS_dbBin   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new IOS_O       ());

			return (CB<T, P>)(object)Convert<Stream, object>.I;
		}

		#endregion

		#endregion
	}
}
