using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	#region SqlTypes

	#region SqlString

	partial class ConvertPartial<T,P>: IConvertible<SqlString,P>
	{
		SqlString IConvertible<SqlString,P>.From(P p) { return Convert<SqlString,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlString,String>, 

		IConvertible<SqlString,SByte>, 
		IConvertible<SqlString,Int16>, 
		IConvertible<SqlString,Int32>, 
		IConvertible<SqlString,Int64>, 

		IConvertible<SqlString,Byte>, 
		IConvertible<SqlString,UInt16>, 
		IConvertible<SqlString,UInt32>, 
		IConvertible<SqlString,UInt64>, 

		IConvertible<SqlString,Single>, 
		IConvertible<SqlString,Double>, 

		IConvertible<SqlString,Boolean>, 
		IConvertible<SqlString,Decimal>, 
		IConvertible<SqlString,Char>, 
		IConvertible<SqlString,TimeSpan>, 
		IConvertible<SqlString,DateTime>, 
		IConvertible<SqlString,Guid>, 
		IConvertible<SqlString,Char[]>, 

		// Nullable Types.
		// 
		IConvertible<SqlString,SByte?>, 
		IConvertible<SqlString,Int16?>, 
		IConvertible<SqlString,Int32?>, 
		IConvertible<SqlString,Int64?>, 

		IConvertible<SqlString,Byte?>, 
		IConvertible<SqlString,UInt16?>, 
		IConvertible<SqlString,UInt32?>, 
		IConvertible<SqlString,UInt64?>, 

		IConvertible<SqlString,Single?>, 
		IConvertible<SqlString,Double?>, 

		IConvertible<SqlString,Boolean?>, 
		IConvertible<SqlString,Decimal?>, 
		IConvertible<SqlString,Char?>, 
		IConvertible<SqlString,TimeSpan?>, 
		IConvertible<SqlString,DateTime?>, 
		IConvertible<SqlString,Guid?>, 

		// SqlTypes.
		// 

		IConvertible<SqlString,SqlByte>, 
		IConvertible<SqlString,SqlInt16>, 
		IConvertible<SqlString,SqlInt32>, 
		IConvertible<SqlString,SqlInt64>, 

		IConvertible<SqlString,SqlSingle>, 
		IConvertible<SqlString,SqlDouble>, 
		IConvertible<SqlString,SqlDecimal>, 
		IConvertible<SqlString,SqlMoney>, 

		IConvertible<SqlString,SqlBoolean>, 
		IConvertible<SqlString,SqlChars>, 
		IConvertible<SqlString,SqlGuid>, 
		IConvertible<SqlString,SqlDateTime>, 
		IConvertible<SqlString,SqlBinary>, 

		IConvertible<SqlString,Type>, 
		IConvertible<SqlString,object>

	{
		// Scalar Types.
		// 
		SqlString IConvertible<SqlString,String>.     From(String p)      { return p == null? SqlString.Null: p; }

		SqlString IConvertible<SqlString,SByte>.      From(SByte p)       { return p.ToString(); }
		SqlString IConvertible<SqlString,Int16>.      From(Int16 p)       { return p.ToString(); }
		SqlString IConvertible<SqlString,Int32>.      From(Int32 p)       { return p.ToString(); }
		SqlString IConvertible<SqlString,Int64>.      From(Int64 p)       { return p.ToString(); }

		SqlString IConvertible<SqlString,Byte>.       From(Byte p)        { return p.ToString(); }
		SqlString IConvertible<SqlString,UInt16>.     From(UInt16 p)      { return p.ToString(); }
		SqlString IConvertible<SqlString,UInt32>.     From(UInt32 p)      { return p.ToString(); }
		SqlString IConvertible<SqlString,UInt64>.     From(UInt64 p)      { return p.ToString(); }

		SqlString IConvertible<SqlString,Single>.     From(Single p)      { return p.ToString(); }
		SqlString IConvertible<SqlString,Double>.     From(Double p)      { return p.ToString(); }

		SqlString IConvertible<SqlString,Boolean>.    From(Boolean p)     { return p.ToString(); }
		SqlString IConvertible<SqlString,Decimal>.    From(Decimal p)     { return p.ToString(); }
		SqlString IConvertible<SqlString,Char>.       From(Char p)        { return p.ToString(); }
		SqlString IConvertible<SqlString,TimeSpan>.   From(TimeSpan p)    { return p.ToString(); }
		SqlString IConvertible<SqlString,DateTime>.   From(DateTime p)    { return p.ToString(); }
		SqlString IConvertible<SqlString,Guid>.       From(Guid p)        { return p.ToString(); }
		SqlString IConvertible<SqlString,Char[]>.     From(Char[] p)      { return new String(p); }

		// Nullable Types.
		// 
		SqlString IConvertible<SqlString,SByte?>.     From(SByte? p)      { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Int16?>.     From(Int16? p)      { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Int32?>.     From(Int32? p)      { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Int64?>.     From(Int64? p)      { return p.HasValue? p.ToString(): SqlString.Null; }

		SqlString IConvertible<SqlString,Byte?>.      From(Byte? p)       { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,UInt16?>.    From(UInt16? p)     { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,UInt32?>.    From(UInt32? p)     { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,UInt64?>.    From(UInt64? p)     { return p.HasValue? p.ToString(): SqlString.Null; }

		SqlString IConvertible<SqlString,Single?>.    From(Single? p)     { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Double?>.    From(Double? p)     { return p.HasValue? p.ToString(): SqlString.Null; }

		SqlString IConvertible<SqlString,Boolean?>.   From(Boolean? p)    { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Decimal?>.   From(Decimal? p)    { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Char?>.      From(Char? p)       { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,TimeSpan?>.  From(TimeSpan? p)   { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,DateTime?>.  From(DateTime? p)   { return p.HasValue? p.ToString(): SqlString.Null; }
		SqlString IConvertible<SqlString,Guid?>.      From(Guid? p)       { return p.HasValue? p.ToString(): SqlString.Null; }

		// SqlTypes.
		// 

		SqlString IConvertible<SqlString,SqlByte>.    From(SqlByte p)     { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlString(); }

		SqlString IConvertible<SqlString,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlString(); }

		SqlString IConvertible<SqlString,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlChars>.   From(SqlChars p)    { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlGuid>.    From(SqlGuid p)     { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlDateTime>.From(SqlDateTime p) { return p.ToSqlString(); }
		SqlString IConvertible<SqlString,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? SqlString.Null: p.ToString(); }

		SqlString IConvertible<SqlString,Type>.       From(Type p)        { return p == null? SqlString.Null: p.FullName; }
		SqlString IConvertible<SqlString,object>.     From(object p)      { return Convert.ToString(p); }
	}

	#endregion

	#region SqlByte

	partial class ConvertPartial<T,P>: IConvertible<SqlByte,P>
	{
		SqlByte IConvertible<SqlByte,P>.From(P p) { return Convert<SqlByte,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlByte,Byte>, 
		IConvertible<SqlByte,String>, 

		IConvertible<SqlByte,SByte>, 
		IConvertible<SqlByte,Int16>, 
		IConvertible<SqlByte,Int32>, 
		IConvertible<SqlByte,Int64>, 

		IConvertible<SqlByte,UInt16>, 
		IConvertible<SqlByte,UInt32>, 
		IConvertible<SqlByte,UInt64>, 

		IConvertible<SqlByte,Single>, 
		IConvertible<SqlByte,Double>, 

		IConvertible<SqlByte,Boolean>, 
		IConvertible<SqlByte,Decimal>, 
		IConvertible<SqlByte,Char>, 

		// Nullable Types.
		// 
		IConvertible<SqlByte,Byte?>, 
		IConvertible<SqlByte,SByte?>, 
		IConvertible<SqlByte,Int16?>, 
		IConvertible<SqlByte,Int32?>, 
		IConvertible<SqlByte,Int64?>, 

		IConvertible<SqlByte,UInt16?>, 
		IConvertible<SqlByte,UInt32?>, 
		IConvertible<SqlByte,UInt64?>, 

		IConvertible<SqlByte,Single?>, 
		IConvertible<SqlByte,Double?>, 

		IConvertible<SqlByte,Boolean?>, 
		IConvertible<SqlByte,Decimal?>, 
		IConvertible<SqlByte,Char?>, 

		// SqlTypes.
		// 
		IConvertible<SqlByte,SqlString>, 

		IConvertible<SqlByte,SqlInt16>, 
		IConvertible<SqlByte,SqlInt32>, 
		IConvertible<SqlByte,SqlInt64>, 

		IConvertible<SqlByte,SqlSingle>, 
		IConvertible<SqlByte,SqlDouble>, 
		IConvertible<SqlByte,SqlDecimal>, 
		IConvertible<SqlByte,SqlMoney>, 

		IConvertible<SqlByte,SqlBoolean>, 
		IConvertible<SqlByte,SqlDateTime>, 

		IConvertible<SqlByte,object>

	{
		// Scalar Types.
		// 
		SqlByte IConvertible<SqlByte,Byte>.       From(Byte p)        { return p; }
		SqlByte IConvertible<SqlByte,String>.     From(String p)      { return p == null? SqlByte.Null: SqlByte.Parse(p); }

		SqlByte IConvertible<SqlByte,SByte>.      From(SByte p)       { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Int16>.      From(Int16 p)       { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Int32>.      From(Int32 p)       { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Int64>.      From(Int64 p)       { return Convert.ToByte(p); }

		SqlByte IConvertible<SqlByte,UInt16>.     From(UInt16 p)      { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,UInt32>.     From(UInt32 p)      { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,UInt64>.     From(UInt64 p)      { return Convert.ToByte(p); }

		SqlByte IConvertible<SqlByte,Single>.     From(Single p)      { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Double>.     From(Double p)      { return Convert.ToByte(p); }

		SqlByte IConvertible<SqlByte,Boolean>.    From(Boolean p)     { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Decimal>.    From(Decimal p)     { return Convert.ToByte(p); }
		SqlByte IConvertible<SqlByte,Char>.       From(Char p)        { return Convert.ToByte(p); }

		// Nullable Types.
		// 
		SqlByte IConvertible<SqlByte,Byte?>.      From(Byte? p)       { return p.HasValue?                p.Value  : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }

		SqlByte IConvertible<SqlByte,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }

		SqlByte IConvertible<SqlByte,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }

		SqlByte IConvertible<SqlByte,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }
		SqlByte IConvertible<SqlByte,Char?>.      From(Char? p)       { return p.HasValue? Convert.ToByte(p.Value) : SqlByte.Null; }

		// SqlTypes.
		// 
		SqlByte IConvertible<SqlByte,SqlString>.  From(SqlString p)   { return p.ToSqlByte(); }

		SqlByte IConvertible<SqlByte,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlByte(); }

		SqlByte IConvertible<SqlByte,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlByte(); }

		SqlByte IConvertible<SqlByte,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlByte(); }
		SqlByte IConvertible<SqlByte,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlByte.Null: Convert.ToByte(p.Value); }

		SqlByte IConvertible<SqlByte,object>.     From(object p)      { return Convert.ToByte(p); }
	}

	#endregion

	#region SqlInt16

	partial class ConvertPartial<T,P>: IConvertible<SqlInt16,P>
	{
		SqlInt16 IConvertible<SqlInt16,P>.From(P p) { return Convert<SqlInt16,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlInt16,Int16>, 
		IConvertible<SqlInt16,String>, 

		IConvertible<SqlInt16,SByte>, 
		IConvertible<SqlInt16,Int32>, 
		IConvertible<SqlInt16,Int64>, 

		IConvertible<SqlInt16,Byte>, 
		IConvertible<SqlInt16,UInt16>, 
		IConvertible<SqlInt16,UInt32>, 
		IConvertible<SqlInt16,UInt64>, 

		IConvertible<SqlInt16,Single>, 
		IConvertible<SqlInt16,Double>, 

		IConvertible<SqlInt16,Boolean>, 
		IConvertible<SqlInt16,Decimal>, 
		IConvertible<SqlInt16,Char>, 

		// Nullable Types.
		// 
		IConvertible<SqlInt16,Int16?>, 
		IConvertible<SqlInt16,SByte?>, 
		IConvertible<SqlInt16,Int32?>, 
		IConvertible<SqlInt16,Int64?>, 

		IConvertible<SqlInt16,Byte?>, 
		IConvertible<SqlInt16,UInt16?>, 
		IConvertible<SqlInt16,UInt32?>, 
		IConvertible<SqlInt16,UInt64?>, 

		IConvertible<SqlInt16,Single?>, 
		IConvertible<SqlInt16,Double?>, 

		IConvertible<SqlInt16,Boolean?>, 
		IConvertible<SqlInt16,Decimal?>, 
		IConvertible<SqlInt16,Char?>, 

		// SqlTypes.
		// 
		IConvertible<SqlInt16,SqlString>, 

		IConvertible<SqlInt16,SqlByte>, 
		IConvertible<SqlInt16,SqlInt32>, 
		IConvertible<SqlInt16,SqlInt64>, 

		IConvertible<SqlInt16,SqlSingle>, 
		IConvertible<SqlInt16,SqlDouble>, 
		IConvertible<SqlInt16,SqlDecimal>, 
		IConvertible<SqlInt16,SqlMoney>, 

		IConvertible<SqlInt16,SqlBoolean>, 
		IConvertible<SqlInt16,SqlDateTime>, 

		IConvertible<SqlInt16,object>

	{
		// Scalar Types.
		// 
		SqlInt16 IConvertible<SqlInt16,Int16>.      From(Int16 p)       { return p; }
		SqlInt16 IConvertible<SqlInt16,String>.     From(String p)      { return p == null? SqlInt16.Null: SqlInt16.Parse(p); }

		SqlInt16 IConvertible<SqlInt16,SByte>.      From(SByte p)       { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,Int32>.      From(Int32 p)       { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,Int64>.      From(Int64 p)       { return Convert.ToInt16(p); }

		SqlInt16 IConvertible<SqlInt16,Byte>.       From(Byte p)        { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,UInt16>.     From(UInt16 p)      { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,UInt32>.     From(UInt32 p)      { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,UInt64>.     From(UInt64 p)      { return Convert.ToInt16(p); }

		SqlInt16 IConvertible<SqlInt16,Single>.     From(Single p)      { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,Double>.     From(Double p)      { return Convert.ToInt16(p); }

		SqlInt16 IConvertible<SqlInt16,Boolean>.    From(Boolean p)     { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,Decimal>.    From(Decimal p)     { return Convert.ToInt16(p); }
		SqlInt16 IConvertible<SqlInt16,Char>.       From(Char p)        { return Convert.ToInt16(p); }

		// Nullable Types.
		// 
		SqlInt16 IConvertible<SqlInt16,Int16?>.     From(Int16? p)      { return p.HasValue?                 p.Value  : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }

		SqlInt16 IConvertible<SqlInt16,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }

		SqlInt16 IConvertible<SqlInt16,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }

		SqlInt16 IConvertible<SqlInt16,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }
		SqlInt16 IConvertible<SqlInt16,Char?>.      From(Char? p)       { return p.HasValue? Convert.ToInt16(p.Value) : SqlInt16.Null; }

		// SqlTypes.
		// 
		SqlInt16 IConvertible<SqlInt16,SqlString>.  From(SqlString p)   { return p.ToSqlInt16(); }

		SqlInt16 IConvertible<SqlInt16,SqlByte>.    From(SqlByte p)     { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlInt16(); }

		SqlInt16 IConvertible<SqlInt16,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlInt16(); }

		SqlInt16 IConvertible<SqlInt16,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlInt16(); }
		SqlInt16 IConvertible<SqlInt16,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlInt16.Null: Convert.ToInt16(p.Value); }

		SqlInt16 IConvertible<SqlInt16,object>.     From(object p)      { return Convert.ToInt16(p); }
	}

	#endregion

	#region SqlInt32

	partial class ConvertPartial<T,P>: IConvertible<SqlInt32,P>
	{
		SqlInt32 IConvertible<SqlInt32,P>.From(P p) { return Convert<SqlInt32,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlInt32,Int32>, 
		IConvertible<SqlInt32,String>, 

		IConvertible<SqlInt32,SByte>, 
		IConvertible<SqlInt32,Int16>, 
		IConvertible<SqlInt32,Int64>, 

		IConvertible<SqlInt32,Byte>, 
		IConvertible<SqlInt32,UInt16>, 
		IConvertible<SqlInt32,UInt32>, 
		IConvertible<SqlInt32,UInt64>, 

		IConvertible<SqlInt32,Single>, 
		IConvertible<SqlInt32,Double>, 

		IConvertible<SqlInt32,Boolean>, 
		IConvertible<SqlInt32,Decimal>, 
		IConvertible<SqlInt32,Char>, 

		// Nullable Types.
		// 
		IConvertible<SqlInt32,Int32?>, 
		IConvertible<SqlInt32,SByte?>, 
		IConvertible<SqlInt32,Int16?>, 
		IConvertible<SqlInt32,Int64?>, 

		IConvertible<SqlInt32,Byte?>, 
		IConvertible<SqlInt32,UInt16?>, 
		IConvertible<SqlInt32,UInt32?>, 
		IConvertible<SqlInt32,UInt64?>, 

		IConvertible<SqlInt32,Single?>, 
		IConvertible<SqlInt32,Double?>, 

		IConvertible<SqlInt32,Boolean?>, 
		IConvertible<SqlInt32,Decimal?>, 
		IConvertible<SqlInt32,Char?>, 

		// SqlTypes.
		// 
		IConvertible<SqlInt32,SqlString>, 

		IConvertible<SqlInt32,SqlByte>, 
		IConvertible<SqlInt32,SqlInt16>, 
		IConvertible<SqlInt32,SqlInt64>, 

		IConvertible<SqlInt32,SqlSingle>, 
		IConvertible<SqlInt32,SqlDouble>, 
		IConvertible<SqlInt32,SqlDecimal>, 
		IConvertible<SqlInt32,SqlMoney>, 

		IConvertible<SqlInt32,SqlBoolean>, 
		IConvertible<SqlInt32,SqlDateTime>, 

		IConvertible<SqlInt32,object>

	{
		// Scalar Types.
		// 
		SqlInt32 IConvertible<SqlInt32,Int32>.      From(Int32 p)       { return p; }
		SqlInt32 IConvertible<SqlInt32,String>.     From(String p)      { return p == null? SqlInt32.Null: SqlInt32.Parse(p); }

		SqlInt32 IConvertible<SqlInt32,SByte>.      From(SByte p)       { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,Int16>.      From(Int16 p)       { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,Int64>.      From(Int64 p)       { return Convert.ToInt32(p); }

		SqlInt32 IConvertible<SqlInt32,Byte>.       From(Byte p)        { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,UInt16>.     From(UInt16 p)      { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,UInt32>.     From(UInt32 p)      { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,UInt64>.     From(UInt64 p)      { return Convert.ToInt32(p); }

		SqlInt32 IConvertible<SqlInt32,Single>.     From(Single p)      { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,Double>.     From(Double p)      { return Convert.ToInt32(p); }

		SqlInt32 IConvertible<SqlInt32,Boolean>.    From(Boolean p)     { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,Decimal>.    From(Decimal p)     { return Convert.ToInt32(p); }
		SqlInt32 IConvertible<SqlInt32,Char>.       From(Char p)        { return Convert.ToInt32(p); }

		// Nullable Types.
		// 
		SqlInt32 IConvertible<SqlInt32,Int32?>.     From(Int32? p)      { return p.HasValue?                 p.Value  : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }

		SqlInt32 IConvertible<SqlInt32,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }

		SqlInt32 IConvertible<SqlInt32,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }

		SqlInt32 IConvertible<SqlInt32,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }
		SqlInt32 IConvertible<SqlInt32,Char?>.      From(Char? p)       { return p.HasValue? Convert.ToInt32(p.Value) : SqlInt32.Null; }

		// SqlTypes.
		// 
		SqlInt32 IConvertible<SqlInt32,SqlString>.  From(SqlString p)   { return p.ToSqlInt32(); }

		SqlInt32 IConvertible<SqlInt32,SqlByte>.    From(SqlByte p)     { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlInt32(); }

		SqlInt32 IConvertible<SqlInt32,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlInt32(); }

		SqlInt32 IConvertible<SqlInt32,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlInt32(); }
		SqlInt32 IConvertible<SqlInt32,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlInt32.Null: Convert.ToInt32(p.Value); }

		SqlInt32 IConvertible<SqlInt32,object>.     From(object p)      { return Convert.ToInt32(p); }
	}

	#endregion

	#region SqlInt64

	partial class ConvertPartial<T,P>: IConvertible<SqlInt64,P>
	{
		SqlInt64 IConvertible<SqlInt64,P>.From(P p) { return Convert<SqlInt64,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlInt64,Int64>, 
		IConvertible<SqlInt64,String>, 

		IConvertible<SqlInt64,SByte>, 
		IConvertible<SqlInt64,Int16>, 
		IConvertible<SqlInt64,Int32>, 

		IConvertible<SqlInt64,Byte>, 
		IConvertible<SqlInt64,UInt16>, 
		IConvertible<SqlInt64,UInt32>, 
		IConvertible<SqlInt64,UInt64>, 

		IConvertible<SqlInt64,Single>, 
		IConvertible<SqlInt64,Double>, 

		IConvertible<SqlInt64,Boolean>, 
		IConvertible<SqlInt64,Decimal>, 
		IConvertible<SqlInt64,Char>, 

		// Nullable Types.
		// 
		IConvertible<SqlInt64,Int64?>, 
		IConvertible<SqlInt64,SByte?>, 
		IConvertible<SqlInt64,Int16?>, 
		IConvertible<SqlInt64,Int32?>, 

		IConvertible<SqlInt64,Byte?>, 
		IConvertible<SqlInt64,UInt16?>, 
		IConvertible<SqlInt64,UInt32?>, 
		IConvertible<SqlInt64,UInt64?>, 

		IConvertible<SqlInt64,Single?>, 
		IConvertible<SqlInt64,Double?>, 

		IConvertible<SqlInt64,Boolean?>, 
		IConvertible<SqlInt64,Decimal?>, 
		IConvertible<SqlInt64,Char?>, 

		// SqlTypes.
		// 
		IConvertible<SqlInt64,SqlString>, 

		IConvertible<SqlInt64,SqlByte>, 
		IConvertible<SqlInt64,SqlInt16>, 
		IConvertible<SqlInt64,SqlInt32>, 

		IConvertible<SqlInt64,SqlSingle>, 
		IConvertible<SqlInt64,SqlDouble>, 
		IConvertible<SqlInt64,SqlDecimal>, 
		IConvertible<SqlInt64,SqlMoney>, 

		IConvertible<SqlInt64,SqlBoolean>, 
		IConvertible<SqlInt64,SqlDateTime>, 

		IConvertible<SqlInt64,object>

	{
		// Scalar Types.
		// 
		SqlInt64 IConvertible<SqlInt64,Int64>.      From(Int64 p)       { return p; }
		SqlInt64 IConvertible<SqlInt64,String>.     From(String p)      { return p == null? SqlInt64.Null: SqlInt64.Parse(p); }

		SqlInt64 IConvertible<SqlInt64,SByte>.      From(SByte p)       { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,Int16>.      From(Int16 p)       { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,Int32>.      From(Int32 p)       { return Convert.ToInt64(p); }

		SqlInt64 IConvertible<SqlInt64,Byte>.       From(Byte p)        { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,UInt16>.     From(UInt16 p)      { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,UInt32>.     From(UInt32 p)      { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,UInt64>.     From(UInt64 p)      { return Convert.ToInt64(p); }

		SqlInt64 IConvertible<SqlInt64,Single>.     From(Single p)      { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,Double>.     From(Double p)      { return Convert.ToInt64(p); }

		SqlInt64 IConvertible<SqlInt64,Boolean>.    From(Boolean p)     { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,Decimal>.    From(Decimal p)     { return Convert.ToInt64(p); }
		SqlInt64 IConvertible<SqlInt64,Char>.       From(Char p)        { return Convert.ToInt64(p); }

		// Nullable Types.
		// 
		SqlInt64 IConvertible<SqlInt64,Int64?>.     From(Int64? p)      { return p.HasValue?                 p.Value  : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }

		SqlInt64 IConvertible<SqlInt64,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }

		SqlInt64 IConvertible<SqlInt64,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }

		SqlInt64 IConvertible<SqlInt64,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }
		SqlInt64 IConvertible<SqlInt64,Char?>.      From(Char? p)       { return p.HasValue? Convert.ToInt64(p.Value) : SqlInt64.Null; }

		// SqlTypes.
		// 
		SqlInt64 IConvertible<SqlInt64,SqlString>.  From(SqlString p)   { return p.ToSqlInt64(); }

		SqlInt64 IConvertible<SqlInt64,SqlByte>.    From(SqlByte p)     { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlInt64(); }

		SqlInt64 IConvertible<SqlInt64,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlInt64(); }

		SqlInt64 IConvertible<SqlInt64,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlInt64(); }
		SqlInt64 IConvertible<SqlInt64,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlInt64.Null: Convert.ToInt64(p.Value); }

		SqlInt64 IConvertible<SqlInt64,object>.     From(object p)      { return Convert.ToInt64(p); }
	}

	#endregion

	#region SqlSingle

	partial class ConvertPartial<T,P>: IConvertible<SqlSingle,P>
	{
		SqlSingle IConvertible<SqlSingle,P>.From(P p) { return Convert<SqlSingle,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlSingle,Single>, 
		IConvertible<SqlSingle,String>, 

		IConvertible<SqlSingle,SByte>, 
		IConvertible<SqlSingle,Int16>, 
		IConvertible<SqlSingle,Int32>, 
		IConvertible<SqlSingle,Int64>, 

		IConvertible<SqlSingle,Byte>, 
		IConvertible<SqlSingle,UInt16>, 
		IConvertible<SqlSingle,UInt32>, 
		IConvertible<SqlSingle,UInt64>, 

		IConvertible<SqlSingle,Double>, 

		IConvertible<SqlSingle,Boolean>, 
		IConvertible<SqlSingle,Decimal>, 

		// Nullable Types.
		// 
		IConvertible<SqlSingle,Single?>, 
		IConvertible<SqlSingle,SByte?>, 
		IConvertible<SqlSingle,Int16?>, 
		IConvertible<SqlSingle,Int32?>, 
		IConvertible<SqlSingle,Int64?>, 

		IConvertible<SqlSingle,Byte?>, 
		IConvertible<SqlSingle,UInt16?>, 
		IConvertible<SqlSingle,UInt32?>, 
		IConvertible<SqlSingle,UInt64?>, 

		IConvertible<SqlSingle,Double?>, 

		IConvertible<SqlSingle,Boolean?>, 
		IConvertible<SqlSingle,Decimal?>, 

		// SqlTypes.
		// 
		IConvertible<SqlSingle,SqlString>, 

		IConvertible<SqlSingle,SqlByte>, 
		IConvertible<SqlSingle,SqlInt16>, 
		IConvertible<SqlSingle,SqlInt32>, 
		IConvertible<SqlSingle,SqlInt64>, 

		IConvertible<SqlSingle,SqlDouble>, 
		IConvertible<SqlSingle,SqlDecimal>, 
		IConvertible<SqlSingle,SqlMoney>, 

		IConvertible<SqlSingle,SqlBoolean>, 
		IConvertible<SqlSingle,SqlDateTime>, 

		IConvertible<SqlSingle,object>

	{
		// Scalar Types.
		// 
		SqlSingle IConvertible<SqlSingle,Single>.     From(Single p)      { return p; }
		SqlSingle IConvertible<SqlSingle,String>.     From(String p)      { return p == null? SqlSingle.Null: SqlSingle.Parse(p); }

		SqlSingle IConvertible<SqlSingle,SByte>.      From(SByte p)       { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,Int16>.      From(Int16 p)       { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,Int32>.      From(Int32 p)       { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,Int64>.      From(Int64 p)       { return Convert.ToSingle(p); }

		SqlSingle IConvertible<SqlSingle,Byte>.       From(Byte p)        { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,UInt16>.     From(UInt16 p)      { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,UInt32>.     From(UInt32 p)      { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,UInt64>.     From(UInt64 p)      { return Convert.ToSingle(p); }

		SqlSingle IConvertible<SqlSingle,Double>.     From(Double p)      { return Convert.ToSingle(p); }

		SqlSingle IConvertible<SqlSingle,Boolean>.    From(Boolean p)     { return Convert.ToSingle(p); }
		SqlSingle IConvertible<SqlSingle,Decimal>.    From(Decimal p)     { return Convert.ToSingle(p); }

		// Nullable Types.
		// 
		SqlSingle IConvertible<SqlSingle,Single?>.    From(Single? p)     { return p.HasValue?                  p.Value  : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }

		SqlSingle IConvertible<SqlSingle,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }

		SqlSingle IConvertible<SqlSingle,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }

		SqlSingle IConvertible<SqlSingle,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }
		SqlSingle IConvertible<SqlSingle,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToSingle(p.Value) : SqlSingle.Null; }

		// SqlTypes.
		// 
		SqlSingle IConvertible<SqlSingle,SqlString>.  From(SqlString p)   { return p.ToSqlSingle(); }

		SqlSingle IConvertible<SqlSingle,SqlByte>.    From(SqlByte p)     { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlSingle(); }

		SqlSingle IConvertible<SqlSingle,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlSingle(); }

		SqlSingle IConvertible<SqlSingle,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlSingle(); }
		SqlSingle IConvertible<SqlSingle,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlSingle.Null: Convert.ToSingle(p.Value); }

		SqlSingle IConvertible<SqlSingle,object>.     From(object p)      { return Convert.ToSingle(p); }
	}

	#endregion

	#region SqlDouble

	partial class ConvertPartial<T,P>: IConvertible<SqlDouble,P>
	{
		SqlDouble IConvertible<SqlDouble,P>.From(P p) { return Convert<SqlDouble,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlDouble,Double>, 
		IConvertible<SqlDouble,String>, 

		IConvertible<SqlDouble,SByte>, 
		IConvertible<SqlDouble,Int16>, 
		IConvertible<SqlDouble,Int32>, 
		IConvertible<SqlDouble,Int64>, 

		IConvertible<SqlDouble,Byte>, 
		IConvertible<SqlDouble,UInt16>, 
		IConvertible<SqlDouble,UInt32>, 
		IConvertible<SqlDouble,UInt64>, 

		IConvertible<SqlDouble,Single>, 

		IConvertible<SqlDouble,Boolean>, 
		IConvertible<SqlDouble,Decimal>, 

		// Nullable Types.
		// 
		IConvertible<SqlDouble,Double?>, 
		IConvertible<SqlDouble,SByte?>, 
		IConvertible<SqlDouble,Int16?>, 
		IConvertible<SqlDouble,Int32?>, 
		IConvertible<SqlDouble,Int64?>, 

		IConvertible<SqlDouble,Byte?>, 
		IConvertible<SqlDouble,UInt16?>, 
		IConvertible<SqlDouble,UInt32?>, 
		IConvertible<SqlDouble,UInt64?>, 

		IConvertible<SqlDouble,Single?>, 

		IConvertible<SqlDouble,Boolean?>, 
		IConvertible<SqlDouble,Decimal?>, 

		// SqlTypes.
		// 
		IConvertible<SqlDouble,SqlString>, 

		IConvertible<SqlDouble,SqlByte>, 
		IConvertible<SqlDouble,SqlInt16>, 
		IConvertible<SqlDouble,SqlInt32>, 
		IConvertible<SqlDouble,SqlInt64>, 

		IConvertible<SqlDouble,SqlSingle>, 
		IConvertible<SqlDouble,SqlDecimal>, 
		IConvertible<SqlDouble,SqlMoney>, 

		IConvertible<SqlDouble,SqlBoolean>, 
		IConvertible<SqlDouble,SqlDateTime>, 

		IConvertible<SqlDouble,object>

	{
		// Scalar Types.
		// 
		SqlDouble IConvertible<SqlDouble,Double>.     From(Double p)      { return p; }
		SqlDouble IConvertible<SqlDouble,String>.     From(String p)      { return p == null? SqlDouble.Null: SqlDouble.Parse(p); }

		SqlDouble IConvertible<SqlDouble,SByte>.      From(SByte p)       { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,Int16>.      From(Int16 p)       { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,Int32>.      From(Int32 p)       { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,Int64>.      From(Int64 p)       { return Convert.ToDouble(p); }

		SqlDouble IConvertible<SqlDouble,Byte>.       From(Byte p)        { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,UInt16>.     From(UInt16 p)      { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,UInt32>.     From(UInt32 p)      { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,UInt64>.     From(UInt64 p)      { return Convert.ToDouble(p); }

		SqlDouble IConvertible<SqlDouble,Single>.     From(Single p)      { return Convert.ToDouble(p); }

		SqlDouble IConvertible<SqlDouble,Boolean>.    From(Boolean p)     { return Convert.ToDouble(p); }
		SqlDouble IConvertible<SqlDouble,Decimal>.    From(Decimal p)     { return Convert.ToDouble(p); }

		// Nullable Types.
		// 
		SqlDouble IConvertible<SqlDouble,Double?>.    From(Double? p)     { return p.HasValue?                  p.Value  : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }

		SqlDouble IConvertible<SqlDouble,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }

		SqlDouble IConvertible<SqlDouble,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }

		SqlDouble IConvertible<SqlDouble,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }
		SqlDouble IConvertible<SqlDouble,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToDouble(p.Value) : SqlDouble.Null; }

		// SqlTypes.
		// 
		SqlDouble IConvertible<SqlDouble,SqlString>.  From(SqlString p)   { return p.ToSqlDouble(); }

		SqlDouble IConvertible<SqlDouble,SqlByte>.    From(SqlByte p)     { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlDouble(); }

		SqlDouble IConvertible<SqlDouble,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlDouble(); }

		SqlDouble IConvertible<SqlDouble,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlDouble(); }
		SqlDouble IConvertible<SqlDouble,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlDouble.Null: Convert.ToDouble(p.Value); }

		SqlDouble IConvertible<SqlDouble,object>.     From(object p)      { return Convert.ToDouble(p); }
	}

	#endregion

	#region SqlDecimal

	partial class ConvertPartial<T,P>: IConvertible<SqlDecimal,P>
	{
		SqlDecimal IConvertible<SqlDecimal,P>.From(P p) { return Convert<SqlDecimal,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlDecimal,Decimal>, 
		IConvertible<SqlDecimal,String>, 

		IConvertible<SqlDecimal,SByte>, 
		IConvertible<SqlDecimal,Int16>, 
		IConvertible<SqlDecimal,Int32>, 
		IConvertible<SqlDecimal,Int64>, 

		IConvertible<SqlDecimal,Byte>, 
		IConvertible<SqlDecimal,UInt16>, 
		IConvertible<SqlDecimal,UInt32>, 
		IConvertible<SqlDecimal,UInt64>, 

		IConvertible<SqlDecimal,Single>, 
		IConvertible<SqlDecimal,Double>, 

		IConvertible<SqlDecimal,Boolean>, 

		// Nullable Types.
		// 
		IConvertible<SqlDecimal,Decimal?>, 
		IConvertible<SqlDecimal,SByte?>, 
		IConvertible<SqlDecimal,Int16?>, 
		IConvertible<SqlDecimal,Int32?>, 
		IConvertible<SqlDecimal,Int64?>, 

		IConvertible<SqlDecimal,Byte?>, 
		IConvertible<SqlDecimal,UInt16?>, 
		IConvertible<SqlDecimal,UInt32?>, 
		IConvertible<SqlDecimal,UInt64?>, 

		IConvertible<SqlDecimal,Single?>, 
		IConvertible<SqlDecimal,Double?>, 

		IConvertible<SqlDecimal,Boolean?>, 

		// SqlTypes.
		// 
		IConvertible<SqlDecimal,SqlString>, 

		IConvertible<SqlDecimal,SqlByte>, 
		IConvertible<SqlDecimal,SqlInt16>, 
		IConvertible<SqlDecimal,SqlInt32>, 
		IConvertible<SqlDecimal,SqlInt64>, 

		IConvertible<SqlDecimal,SqlSingle>, 
		IConvertible<SqlDecimal,SqlDouble>, 
		IConvertible<SqlDecimal,SqlMoney>, 

		IConvertible<SqlDecimal,SqlBoolean>, 
		IConvertible<SqlDecimal,SqlDateTime>, 

		IConvertible<SqlDecimal,object>

	{
		// Scalar Types.
		// 
		SqlDecimal IConvertible<SqlDecimal,Decimal>.    From(Decimal p)     { return p; }
		SqlDecimal IConvertible<SqlDecimal,String>.     From(String p)      { return p == null? SqlDecimal.Null: SqlDecimal.Parse(p); }

		SqlDecimal IConvertible<SqlDecimal,SByte>.      From(SByte p)       { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,Int16>.      From(Int16 p)       { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,Int32>.      From(Int32 p)       { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,Int64>.      From(Int64 p)       { return Convert.ToDecimal(p); }

		SqlDecimal IConvertible<SqlDecimal,Byte>.       From(Byte p)        { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,UInt16>.     From(UInt16 p)      { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,UInt32>.     From(UInt32 p)      { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,UInt64>.     From(UInt64 p)      { return Convert.ToDecimal(p); }

		SqlDecimal IConvertible<SqlDecimal,Single>.     From(Single p)      { return Convert.ToDecimal(p); }
		SqlDecimal IConvertible<SqlDecimal,Double>.     From(Double p)      { return Convert.ToDecimal(p); }

		SqlDecimal IConvertible<SqlDecimal,Boolean>.    From(Boolean p)     { return Convert.ToDecimal(p); }

		// Nullable Types.
		// 
		SqlDecimal IConvertible<SqlDecimal,Decimal?>.   From(Decimal? p)    { return p.HasValue?                   p.Value  : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }

		SqlDecimal IConvertible<SqlDecimal,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }

		SqlDecimal IConvertible<SqlDecimal,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }
		SqlDecimal IConvertible<SqlDecimal,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }

		SqlDecimal IConvertible<SqlDecimal,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToDecimal(p.Value) : SqlDecimal.Null; }

		// SqlTypes.
		// 
		SqlDecimal IConvertible<SqlDecimal,SqlString>.  From(SqlString p)   { return p.ToSqlDecimal(); }

		SqlDecimal IConvertible<SqlDecimal,SqlByte>.    From(SqlByte p)     { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlDecimal(); }

		SqlDecimal IConvertible<SqlDecimal,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlDecimal(); }

		SqlDecimal IConvertible<SqlDecimal,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlDecimal(); }
		SqlDecimal IConvertible<SqlDecimal,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlDecimal.Null: Convert.ToDecimal(p.Value); }

		SqlDecimal IConvertible<SqlDecimal,object>.     From(object p)      { return Convert.ToDecimal(p); }
	}

	#endregion

	#region SqlMoney

	partial class ConvertPartial<T,P>: IConvertible<SqlMoney,P>
	{
		SqlMoney IConvertible<SqlMoney,P>.From(P p) { return Convert<SqlMoney,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlMoney,Decimal>, 
		IConvertible<SqlMoney,String>, 

		IConvertible<SqlMoney,SByte>, 
		IConvertible<SqlMoney,Int16>, 
		IConvertible<SqlMoney,Int32>, 
		IConvertible<SqlMoney,Int64>, 

		IConvertible<SqlMoney,Byte>, 
		IConvertible<SqlMoney,UInt16>, 
		IConvertible<SqlMoney,UInt32>, 
		IConvertible<SqlMoney,UInt64>, 

		IConvertible<SqlMoney,Single>, 
		IConvertible<SqlMoney,Double>, 

		IConvertible<SqlMoney,Boolean>, 

		// Nullable Types.
		// 
		IConvertible<SqlMoney,Decimal?>, 
		IConvertible<SqlMoney,SByte?>, 
		IConvertible<SqlMoney,Int16?>, 
		IConvertible<SqlMoney,Int32?>, 
		IConvertible<SqlMoney,Int64?>, 

		IConvertible<SqlMoney,Byte?>, 
		IConvertible<SqlMoney,UInt16?>, 
		IConvertible<SqlMoney,UInt32?>, 
		IConvertible<SqlMoney,UInt64?>, 

		IConvertible<SqlMoney,Single?>, 
		IConvertible<SqlMoney,Double?>, 

		IConvertible<SqlMoney,Boolean?>, 

		// SqlTypes.
		// 
		IConvertible<SqlMoney,SqlString>, 

		IConvertible<SqlMoney,SqlByte>, 
		IConvertible<SqlMoney,SqlInt16>, 
		IConvertible<SqlMoney,SqlInt32>, 
		IConvertible<SqlMoney,SqlInt64>, 

		IConvertible<SqlMoney,SqlSingle>, 
		IConvertible<SqlMoney,SqlDouble>, 
		IConvertible<SqlMoney,SqlDecimal>, 

		IConvertible<SqlMoney,SqlBoolean>, 
		IConvertible<SqlMoney,SqlDateTime>, 

		IConvertible<SqlMoney,object>

	{
		// Scalar Types.
		// 
		SqlMoney IConvertible<SqlMoney,Decimal>.    From(Decimal p)     { return p; }
		SqlMoney IConvertible<SqlMoney,String>.     From(String p)      { return p == null? SqlMoney.Null: SqlMoney.Parse(p); }

		SqlMoney IConvertible<SqlMoney,SByte>.      From(SByte p)       { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,Int16>.      From(Int16 p)       { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,Int32>.      From(Int32 p)       { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,Int64>.      From(Int64 p)       { return Convert.ToDecimal(p); }

		SqlMoney IConvertible<SqlMoney,Byte>.       From(Byte p)        { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,UInt16>.     From(UInt16 p)      { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,UInt32>.     From(UInt32 p)      { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,UInt64>.     From(UInt64 p)      { return Convert.ToDecimal(p); }

		SqlMoney IConvertible<SqlMoney,Single>.     From(Single p)      { return Convert.ToDecimal(p); }
		SqlMoney IConvertible<SqlMoney,Double>.     From(Double p)      { return Convert.ToDecimal(p); }

		SqlMoney IConvertible<SqlMoney,Boolean>.    From(Boolean p)     { return Convert.ToDecimal(p); }

		// Nullable Types.
		// 
		SqlMoney IConvertible<SqlMoney,Decimal?>.   From(Decimal? p)    { return p.HasValue?                   p.Value  : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }

		SqlMoney IConvertible<SqlMoney,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }

		SqlMoney IConvertible<SqlMoney,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }
		SqlMoney IConvertible<SqlMoney,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }

		SqlMoney IConvertible<SqlMoney,Boolean?>.   From(Boolean? p)    { return p.HasValue? Convert.ToDecimal(p.Value) : SqlMoney.Null; }

		// SqlTypes.
		// 
		SqlMoney IConvertible<SqlMoney,SqlString>.  From(SqlString p)   { return p.ToSqlMoney(); }

		SqlMoney IConvertible<SqlMoney,SqlByte>.    From(SqlByte p)     { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlMoney(); }

		SqlMoney IConvertible<SqlMoney,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlMoney(); }

		SqlMoney IConvertible<SqlMoney,SqlBoolean>. From(SqlBoolean p)  { return p.ToSqlMoney(); }
		SqlMoney IConvertible<SqlMoney,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlMoney.Null: Convert.ToDecimal(p.Value); }

		SqlMoney IConvertible<SqlMoney,object>.     From(object p)      { return Convert.ToDecimal(p); }
	}

	#endregion

	#region SqlBoolean

	partial class ConvertPartial<T,P>: IConvertible<SqlBoolean,P>
	{
		SqlBoolean IConvertible<SqlBoolean,P>.From(P p) { return Convert<SqlBoolean,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlBoolean,Boolean>, 
		IConvertible<SqlBoolean,String>, 

		IConvertible<SqlBoolean,SByte>, 
		IConvertible<SqlBoolean,Int16>, 
		IConvertible<SqlBoolean,Int32>, 
		IConvertible<SqlBoolean,Int64>, 

		IConvertible<SqlBoolean,Byte>, 
		IConvertible<SqlBoolean,UInt16>, 
		IConvertible<SqlBoolean,UInt32>, 
		IConvertible<SqlBoolean,UInt64>, 

		IConvertible<SqlBoolean,Single>, 
		IConvertible<SqlBoolean,Double>, 

		IConvertible<SqlBoolean,Decimal>, 
		IConvertible<SqlBoolean,Char>, 

		// Nullable Types.
		// 
		IConvertible<SqlBoolean,Boolean?>, 
		IConvertible<SqlBoolean,SByte?>, 
		IConvertible<SqlBoolean,Int16?>, 
		IConvertible<SqlBoolean,Int32?>, 
		IConvertible<SqlBoolean,Int64?>, 

		IConvertible<SqlBoolean,Byte?>, 
		IConvertible<SqlBoolean,UInt16?>, 
		IConvertible<SqlBoolean,UInt32?>, 
		IConvertible<SqlBoolean,UInt64?>, 

		IConvertible<SqlBoolean,Single?>, 
		IConvertible<SqlBoolean,Double?>, 

		IConvertible<SqlBoolean,Decimal?>, 
		IConvertible<SqlBoolean,Char?>, 

		// SqlTypes.
		// 
		IConvertible<SqlBoolean,SqlString>, 

		IConvertible<SqlBoolean,SqlByte>, 
		IConvertible<SqlBoolean,SqlInt16>, 
		IConvertible<SqlBoolean,SqlInt32>, 
		IConvertible<SqlBoolean,SqlInt64>, 

		IConvertible<SqlBoolean,SqlSingle>, 
		IConvertible<SqlBoolean,SqlDouble>, 
		IConvertible<SqlBoolean,SqlDecimal>, 
		IConvertible<SqlBoolean,SqlMoney>, 

		IConvertible<SqlBoolean,SqlDateTime>, 

		IConvertible<SqlBoolean,object>

	{
		// Scalar Types.
		// 
		SqlBoolean IConvertible<SqlBoolean,Boolean>.    From(Boolean p)     { return p; }
		SqlBoolean IConvertible<SqlBoolean,String>.     From(String p)      { return p == null? SqlBoolean.Null: SqlBoolean.Parse(p); }

		SqlBoolean IConvertible<SqlBoolean,SByte>.      From(SByte p)       { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,Int16>.      From(Int16 p)       { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,Int32>.      From(Int32 p)       { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,Int64>.      From(Int64 p)       { return Convert.ToBoolean(p); }

		SqlBoolean IConvertible<SqlBoolean,Byte>.       From(Byte p)        { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,UInt16>.     From(UInt16 p)      { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,UInt32>.     From(UInt32 p)      { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,UInt64>.     From(UInt64 p)      { return Convert.ToBoolean(p); }

		SqlBoolean IConvertible<SqlBoolean,Single>.     From(Single p)      { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,Double>.     From(Double p)      { return Convert.ToBoolean(p); }

		SqlBoolean IConvertible<SqlBoolean,Decimal>.    From(Decimal p)     { return Convert.ToBoolean(p); }
		SqlBoolean IConvertible<SqlBoolean,Char>.       From(Char p)        { return Convert<Boolean,Char>.Instance.From(p); }

		// Nullable Types.
		// 
		SqlBoolean IConvertible<SqlBoolean,Boolean?>.   From(Boolean? p)    { return p.HasValue?                   p.Value  : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,SByte?>.     From(SByte? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,Int16?>.     From(Int16? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,Int32?>.     From(Int32? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,Int64?>.     From(Int64? p)      { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }

		SqlBoolean IConvertible<SqlBoolean,Byte?>.      From(Byte? p)       { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,UInt16?>.    From(UInt16? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,UInt32?>.    From(UInt32? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,UInt64?>.    From(UInt64? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }

		SqlBoolean IConvertible<SqlBoolean,Single?>.    From(Single? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,Double?>.    From(Double? p)     { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }

		SqlBoolean IConvertible<SqlBoolean,Decimal?>.   From(Decimal? p)    { return p.HasValue? Convert.ToBoolean(p.Value) : SqlBoolean.Null; }
		SqlBoolean IConvertible<SqlBoolean,Char?>.      From(Char? p)       { return (p.HasValue)? Convert<Boolean,Char>.Instance.From(p.Value): SqlBoolean.Null; }

		// SqlTypes.
		// 
		SqlBoolean IConvertible<SqlBoolean,SqlString>.  From(SqlString p)   { return p.ToSqlBoolean(); }

		SqlBoolean IConvertible<SqlBoolean,SqlByte>.    From(SqlByte p)     { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlInt16>.   From(SqlInt16 p)    { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlInt32>.   From(SqlInt32 p)    { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlInt64>.   From(SqlInt64 p)    { return p.ToSqlBoolean(); }

		SqlBoolean IConvertible<SqlBoolean,SqlSingle>.  From(SqlSingle p)   { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlDouble>.  From(SqlDouble p)   { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlDecimal>. From(SqlDecimal p)  { return p.ToSqlBoolean(); }
		SqlBoolean IConvertible<SqlBoolean,SqlMoney>.   From(SqlMoney p)    { return p.ToSqlBoolean(); }

		SqlBoolean IConvertible<SqlBoolean,SqlDateTime>.From(SqlDateTime p) { return p.IsNull? SqlBoolean.Null: Convert.ToBoolean(p.Value); }

		SqlBoolean IConvertible<SqlBoolean,object>.     From(object p)      { return Convert.ToBoolean(p); }
	}

	#endregion

	#region SqlDateTime

	partial class ConvertPartial<T,P>: IConvertible<SqlDateTime,P>
	{
		SqlDateTime IConvertible<SqlDateTime,P>.From(P p) { return Convert<SqlDateTime,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlDateTime,DateTime>, 
		IConvertible<SqlDateTime,String>, 

		// Nullable Types.
		// 
		IConvertible<SqlDateTime,DateTime?>, 

		// SqlTypes.
		// 
		IConvertible<SqlDateTime,SqlString>, 

		IConvertible<SqlDateTime,object>

	{
		// Scalar Types.
		// 
		SqlDateTime IConvertible<SqlDateTime,DateTime>.   From(DateTime p)    { return p; }
		SqlDateTime IConvertible<SqlDateTime,String>.     From(String p)      { return p == null? SqlDateTime.Null: SqlDateTime.Parse(p); }

		// Nullable Types.
		// 
		SqlDateTime IConvertible<SqlDateTime,DateTime?>.  From(DateTime? p)   { return p.HasValue?                    p.Value  : SqlDateTime.Null; }

		// SqlTypes.
		// 
		SqlDateTime IConvertible<SqlDateTime,SqlString>.  From(SqlString p)   { return p.ToSqlDateTime(); }

		SqlDateTime IConvertible<SqlDateTime,object>.     From(object p)      { return Convert.ToDateTime(p); }
	}

	#endregion

	#region SqlGuid

	partial class ConvertPartial<T,P>: IConvertible<SqlGuid,P>
	{
		SqlGuid IConvertible<SqlGuid,P>.From(P p) { return Convert<SqlGuid,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlGuid,Guid>, 
		IConvertible<SqlGuid,String>, 

		// Nullable Types.
		// 
		IConvertible<SqlGuid,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<SqlGuid,SqlBinary>, 
		IConvertible<SqlGuid,SqlBytes>, 
		IConvertible<SqlGuid,SqlString>, 

		// Other Types.
		// 
		IConvertible<SqlGuid,Type>, 
		IConvertible<SqlGuid,Byte[]>, 

		IConvertible<SqlGuid,object>

	{
		// Scalar Types.
		// 
		SqlGuid IConvertible<SqlGuid,Guid>.       From(Guid p)        { return p; }
		SqlGuid IConvertible<SqlGuid,String>.     From(String p)      { return p == null? SqlGuid.Null: SqlGuid.Parse(p); }

		// Nullable Types.
		// 
		SqlGuid IConvertible<SqlGuid,Guid?>.      From(Guid? p)       { return p.HasValue? p.Value : SqlGuid.Null; }

		// SqlTypes.
		// 
		SqlGuid IConvertible<SqlGuid,SqlBinary>.  From(SqlBinary p)   { return p.ToSqlGuid(); }
		SqlGuid IConvertible<SqlGuid,SqlBytes>.   From(SqlBytes p)    { return p.ToSqlBinary().ToSqlGuid(); }
		SqlGuid IConvertible<SqlGuid,SqlString>.  From(SqlString p)   { return p.ToSqlGuid(); }

		// Other Types.
		// 
		SqlGuid IConvertible<SqlGuid,Type>.       From(Type p)        { return p == null? SqlGuid.Null: p.GUID; }
		SqlGuid IConvertible<SqlGuid,Byte[]>.     From(Byte[] p)      { return p == null? SqlGuid.Null: new SqlGuid(p); }

		SqlGuid IConvertible<SqlGuid,object>.     From(object p)     
		{
			if (p == null) return SqlGuid.Null;

			// Scalar Types.
			//
			if (p is Guid)        return Convert<SqlGuid,Guid>       .Instance.From((Guid)p);
			if (p is String)      return Convert<SqlGuid,String>     .Instance.From((String)p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<SqlGuid,Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlBinary)   return Convert<SqlGuid,SqlBinary>  .Instance.From((SqlBinary)p);
			if (p is SqlBytes)    return Convert<SqlGuid,SqlBytes>   .Instance.From((SqlBytes)p);
			if (p is SqlString)   return Convert<SqlGuid,SqlString>  .Instance.From((SqlString)p);

			// Other Types.
			//
			if (p is Type)        return Convert<SqlGuid,Type>       .Instance.From((Type)p);
			if (p is Byte[])      return Convert<SqlGuid,Byte[]>     .Instance.From((Byte[])p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region SqlBinary

	partial class ConvertPartial<T,P>: IConvertible<SqlBinary,P>
	{
		SqlBinary IConvertible<SqlBinary,P>.From(P p) { return Convert<SqlBinary,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlBinary,Byte[]>, 
		IConvertible<SqlBinary,Guid>, 

		// Nullable Types.
		// 
		IConvertible<SqlBinary,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<SqlBinary,SqlBytes>, 
		IConvertible<SqlBinary,SqlGuid>, 

		IConvertible<SqlBinary,object>

	{
		// Scalar Types.
		// 
		SqlBinary IConvertible<SqlBinary,Byte[]>.     From(Byte[] p)      { return p; }
		SqlBinary IConvertible<SqlBinary,Guid>.       From(Guid p)        { return p == Guid.Empty? SqlBinary.Null: new SqlGuid(p).ToSqlBinary(); }

		// Nullable Types.
		// 
		SqlBinary IConvertible<SqlBinary,Guid?>.      From(Guid? p)       { return p.HasValue? new SqlGuid(p.Value).ToSqlBinary(): SqlBinary.Null; }

		// SqlTypes.
		// 
		SqlBinary IConvertible<SqlBinary,SqlBytes>.   From(SqlBytes p)    { return p.ToSqlBinary(); }
		SqlBinary IConvertible<SqlBinary,SqlGuid>.    From(SqlGuid p)     { return p.ToSqlBinary(); }

		SqlBinary IConvertible<SqlBinary,object>.     From(object p)     
		{
			if (p == null) return SqlBinary.Null;

			// Scalar Types.
			//
			if (p is Byte[])      return Convert<SqlBinary,Byte[]>     .Instance.From((Byte[])p);
			if (p is Guid)        return Convert<SqlBinary,Guid>       .Instance.From((Guid)p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<SqlBinary,Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlBytes)    return Convert<SqlBinary,SqlBytes>   .Instance.From((SqlBytes)p);
			if (p is SqlGuid)     return Convert<SqlBinary,SqlGuid>    .Instance.From((SqlGuid)p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region SqlBytes

	partial class ConvertPartial<T,P>: IConvertible<SqlBytes,P>
	{
		SqlBytes IConvertible<SqlBytes,P>.From(P p) { return Convert<SqlBytes,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlBytes,Byte[]>, 
		IConvertible<SqlBytes,Stream>, 
		IConvertible<SqlBytes,Guid>, 

		// Nullable Types.
		// 
		IConvertible<SqlBytes,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<SqlBytes,SqlBinary>, 
		IConvertible<SqlBytes,SqlGuid>, 

		IConvertible<SqlBytes,object>

	{
		// Scalar Types.
		// 
		SqlBytes IConvertible<SqlBytes,Byte[]>.     From(Byte[] p)      { return p == null? SqlBytes.Null: new SqlBytes(p); }
		SqlBytes IConvertible<SqlBytes,Stream>.     From(Stream p)      { return p == null? SqlBytes.Null: new SqlBytes(p); }
		SqlBytes IConvertible<SqlBytes,Guid>.       From(Guid p)        { return p == Guid.Empty? SqlBytes.Null: new SqlBytes(p.ToByteArray()); }

		// Nullable Types.
		// 
		SqlBytes IConvertible<SqlBytes,Guid?>.      From(Guid? p)       { return p.HasValue? new SqlBytes(p.Value.ToByteArray()): SqlBytes.Null; }

		// SqlTypes.
		// 
		SqlBytes IConvertible<SqlBytes,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? SqlBytes.Null: new SqlBytes(p); }
		SqlBytes IConvertible<SqlBytes,SqlGuid>.    From(SqlGuid p)     { return p.IsNull? SqlBytes.Null: new SqlBytes(p.ToByteArray()); }

		SqlBytes IConvertible<SqlBytes,object>.     From(object p)     
		{
			if (p == null) return SqlBytes.Null;

			// Scalar Types.
			//
			if (p is Byte[])      return Convert<SqlBytes,Byte[]>     .Instance.From((Byte[])p);
			if (p is Stream)      return Convert<SqlBytes,Stream>     .Instance.From((Stream)p);
			if (p is Guid)        return Convert<SqlBytes,Guid>       .Instance.From((Guid)p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<SqlBytes,Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlBinary)   return Convert<SqlBytes,SqlBinary>  .Instance.From((SqlBinary)p);
			if (p is SqlGuid)     return Convert<SqlBytes,SqlGuid>    .Instance.From((SqlGuid)p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region SqlChars

	partial class ConvertPartial<T,P>: IConvertible<SqlChars,P>
	{
		SqlChars IConvertible<SqlChars,P>.From(P p) { return Convert<SqlChars,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlChars,String>, 
		IConvertible<SqlChars,Char[]>, 

		IConvertible<SqlChars,SByte>, 
		IConvertible<SqlChars,Int16>, 
		IConvertible<SqlChars,Int32>, 
		IConvertible<SqlChars,Int64>, 

		IConvertible<SqlChars,Byte>, 
		IConvertible<SqlChars,UInt16>, 
		IConvertible<SqlChars,UInt32>, 
		IConvertible<SqlChars,UInt64>, 

		IConvertible<SqlChars,Single>, 
		IConvertible<SqlChars,Double>, 

		IConvertible<SqlChars,Boolean>, 
		IConvertible<SqlChars,Decimal>, 
		IConvertible<SqlChars,Char>, 
		IConvertible<SqlChars,TimeSpan>, 
		IConvertible<SqlChars,DateTime>, 
		IConvertible<SqlChars,Guid>, 

		// Nullable Types.
		// 
		IConvertible<SqlChars,SByte?>, 
		IConvertible<SqlChars,Int16?>, 
		IConvertible<SqlChars,Int32?>, 
		IConvertible<SqlChars,Int64?>, 

		IConvertible<SqlChars,Byte?>, 
		IConvertible<SqlChars,UInt16?>, 
		IConvertible<SqlChars,UInt32?>, 
		IConvertible<SqlChars,UInt64?>, 

		IConvertible<SqlChars,Single?>, 
		IConvertible<SqlChars,Double?>, 

		IConvertible<SqlChars,Boolean?>, 
		IConvertible<SqlChars,Decimal?>, 
		IConvertible<SqlChars,Char?>, 
		IConvertible<SqlChars,TimeSpan?>, 
		IConvertible<SqlChars,DateTime?>, 
		IConvertible<SqlChars,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<SqlChars,SqlString>, 

		IConvertible<SqlChars,SqlByte>, 
		IConvertible<SqlChars,SqlInt16>, 
		IConvertible<SqlChars,SqlInt32>, 
		IConvertible<SqlChars,SqlInt64>, 

		IConvertible<SqlChars,SqlSingle>, 
		IConvertible<SqlChars,SqlDouble>, 
		IConvertible<SqlChars,SqlDecimal>, 
		IConvertible<SqlChars,SqlMoney>, 

		IConvertible<SqlChars,SqlBoolean>, 
		IConvertible<SqlChars,SqlGuid>, 
		IConvertible<SqlChars,SqlDateTime>, 
		IConvertible<SqlChars,SqlBinary>, 

		IConvertible<SqlChars,Type>, 
		IConvertible<SqlChars,object>

	{
		// Scalar Types.
		// 
		SqlChars IConvertible<SqlChars,String>.     From(String p)      { return p == null? SqlChars.Null: new SqlChars(p.ToCharArray()); }
		SqlChars IConvertible<SqlChars,Char[]>.     From(Char[] p)      { return p == null? SqlChars.Null: new SqlChars(p); }

		SqlChars IConvertible<SqlChars,SByte>.      From(SByte p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Int16>.      From(Int16 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Int32>.      From(Int32 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Int64>.      From(Int64 p)       { return new SqlChars(Convert.ToString(p).ToCharArray()); }

		SqlChars IConvertible<SqlChars,Byte>.       From(Byte p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,UInt16>.     From(UInt16 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,UInt32>.     From(UInt32 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,UInt64>.     From(UInt64 p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }

		SqlChars IConvertible<SqlChars,Single>.     From(Single p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Double>.     From(Double p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }

		SqlChars IConvertible<SqlChars,Boolean>.    From(Boolean p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Decimal>.    From(Decimal p)     { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Char>.       From(Char p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,TimeSpan>.   From(TimeSpan p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,DateTime>.   From(DateTime p)    { return new SqlChars(Convert.ToString(p).ToCharArray()); }
		SqlChars IConvertible<SqlChars,Guid>.       From(Guid p)        { return new SqlChars(Convert.ToString(p).ToCharArray()); }

		// Nullable Types.
		// 
		SqlChars IConvertible<SqlChars,SByte?>.     From(SByte? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Int16?>.     From(Int16? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Int32?>.     From(Int32? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Int64?>.     From(Int64? p)      { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		SqlChars IConvertible<SqlChars,Byte?>.      From(Byte? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,UInt16?>.    From(UInt16? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,UInt32?>.    From(UInt32? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,UInt64?>.    From(UInt64? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		SqlChars IConvertible<SqlChars,Single?>.    From(Single? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Double?>.    From(Double? p)     { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		SqlChars IConvertible<SqlChars,Boolean?>.   From(Boolean? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Decimal?>.   From(Decimal? p)    { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Char?>.      From(Char? p)       { return p.HasValue? new SqlChars(new Char[]{p.Value})       : SqlChars.Null; }
		SqlChars IConvertible<SqlChars,TimeSpan?>.  From(TimeSpan? p)   { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,DateTime?>.  From(DateTime? p)   { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		SqlChars IConvertible<SqlChars,Guid?>.      From(Guid? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		// SqlTypes.
		// 
		SqlChars IConvertible<SqlChars,SqlString>.  From(SqlString p)   { return (SqlChars)p; }

		SqlChars IConvertible<SqlChars,SqlByte>.    From(SqlByte p)     { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlInt16>.   From(SqlInt16 p)    { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlInt32>.   From(SqlInt32 p)    { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlInt64>.   From(SqlInt64 p)    { return (SqlChars)p.ToSqlString(); }

		SqlChars IConvertible<SqlChars,SqlSingle>.  From(SqlSingle p)   { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlDouble>.  From(SqlDouble p)   { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlDecimal>. From(SqlDecimal p)  { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlMoney>.   From(SqlMoney p)    { return (SqlChars)p.ToSqlString(); }

		SqlChars IConvertible<SqlChars,SqlBoolean>. From(SqlBoolean p)  { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlGuid>.    From(SqlGuid p)     { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlDateTime>.From(SqlDateTime p) { return (SqlChars)p.ToSqlString(); }
		SqlChars IConvertible<SqlChars,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? SqlChars.Null: new SqlChars(p.ToString().ToCharArray()); }

		SqlChars IConvertible<SqlChars,Type>.       From(Type p)        { return p == null? SqlChars.Null: new SqlChars(p.FullName.ToCharArray()); }
		SqlChars IConvertible<SqlChars,object>.     From(object p)      { return new SqlChars(Convert.ToString(p).ToCharArray()); }
	}

	#endregion

	#region SqlXml

	partial class ConvertPartial<T,P>: IConvertible<SqlXml,P>
	{
		SqlXml IConvertible<SqlXml,P>.From(P p) { return Convert<SqlXml,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<SqlXml,String>, 

		IConvertible<SqlXml,Stream>, 
		IConvertible<SqlXml,XmlReader>, 

		IConvertible<SqlXml,Char[]>, 
		IConvertible<SqlXml,Byte[]>, 

		// SqlTypes.
		// 
		IConvertible<SqlXml,SqlString>, 
		IConvertible<SqlXml,SqlChars>, 
		IConvertible<SqlXml,SqlBinary>, 
		IConvertible<SqlXml,SqlBytes>, 

		IConvertible<SqlXml,object>

	{
		// Scalar Types.
		// 
		SqlXml IConvertible<SqlXml,String>.     From(String p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p))); }

		SqlXml IConvertible<SqlXml,Stream>.     From(Stream p)      { return p == null? SqlXml.Null: new SqlXml(p); }
		SqlXml IConvertible<SqlXml,XmlReader>.  From(XmlReader p)   { return p == null? SqlXml.Null: new SqlXml(p); }

		SqlXml IConvertible<SqlXml,Char[]>.     From(Char[] p)      { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(new string(p)))); }
		SqlXml IConvertible<SqlXml,Byte[]>.     From(Byte[] p)      { return p == null? SqlXml.Null: new SqlXml(new MemoryStream(p)); }

		// SqlTypes.
		// 
		SqlXml IConvertible<SqlXml,SqlString>.  From(SqlString p)   { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.Value))); }
		SqlXml IConvertible<SqlXml,SqlChars>.   From(SqlChars p)    { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.ToSqlString().Value))); }
		SqlXml IConvertible<SqlXml,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? SqlXml.Null: new SqlXml(new MemoryStream(p.Value)); }
		SqlXml IConvertible<SqlXml,SqlBytes>.   From(SqlBytes p)    { return p.IsNull? SqlXml.Null: new SqlXml(p.Stream); }

		SqlXml IConvertible<SqlXml,object>.     From(object p)     
		{
			if (p == null) return SqlXml.Null;

			// Scalar Types.
			//
			if (p is String)      return Convert<SqlXml,String>     .Instance.From((String)p);

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
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#endregion


}
