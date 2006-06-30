
#region Generated File
/*
 * GENERATED FILE -- DO NOT EDIT
 *
 * Generator: TransformCodeGenerator, Version=1.0.2244.27090, Culture=neutral, PublicKeyToken=null
 * Version:   1.0.2244.27090
 *
 *
 * Generated code from "GenConvert.xml"
 *
 * Created: 30 ???? 2006 ?.
 * By:      IT-TRUST\paul
 *
 */
#endregion

using System;
using System.Data.SqlTypes;
using System.IO;

namespace BLToolkit.Experimental
{
	public static partial class GenConvert
	{

		// Scalar Types.
		// 
		#region String


		// Scalar Types.
		// 
		public static String ToString(SByte        p) { return p.ToString(); }
		public static String ToString(Int16        p) { return p.ToString(); }
		public static String ToString(Int32        p) { return p.ToString(); }
		public static String ToString(Int64        p) { return p.ToString(); }
		public static String ToString(Byte         p) { return p.ToString(); }
		public static String ToString(UInt16       p) { return p.ToString(); }
		public static String ToString(UInt32       p) { return p.ToString(); }
		public static String ToString(UInt64       p) { return p.ToString(); }
		public static String ToString(Char         p) { return p.ToString(); }
		public static String ToString(Single       p) { return p.ToString(); }
		public static String ToString(Double       p) { return p.ToString(); }
		public static String ToString(Boolean      p) { return p.ToString(); }
		public static String ToString(Decimal      p) { return p.ToString(); }
		public static String ToString(DateTime     p) { return p.ToString(); }
		public static String ToString(TimeSpan     p) { return p.ToString(); }
		public static String ToString(Guid         p) { return p.ToString(); }

		// Nullable Types.
		// 
		public static String ToString(SByte?       p) { return p.ToString(); }
		public static String ToString(Int16?       p) { return p.ToString(); }
		public static String ToString(Int32?       p) { return p.ToString(); }
		public static String ToString(Int64?       p) { return p.ToString(); }
		public static String ToString(Byte?        p) { return p.ToString(); }
		public static String ToString(UInt16?      p) { return p.ToString(); }
		public static String ToString(UInt32?      p) { return p.ToString(); }
		public static String ToString(UInt64?      p) { return p.ToString(); }
		public static String ToString(Char?        p) { return p.ToString(); }
		public static String ToString(Single?      p) { return p.ToString(); }
		public static String ToString(Double?      p) { return p.ToString(); }
		public static String ToString(Boolean?     p) { return p.ToString(); }
		public static String ToString(Decimal?     p) { return p.ToString(); }
		public static String ToString(DateTime?    p) { return p.ToString(); }
		public static String ToString(TimeSpan?    p) { return p.ToString(); }
		public static String ToString(Guid?        p) { return p.ToString(); }

		// SqlTypes.
		// 
		public static String ToString(SqlString    p) { return p.ToString(); }
		public static String ToString(SqlByte      p) { return p.ToString(); }
		public static String ToString(SqlInt16     p) { return p.ToString(); }
		public static String ToString(SqlInt32     p) { return p.ToString(); }
		public static String ToString(SqlInt64     p) { return p.ToString(); }
		public static String ToString(SqlSingle    p) { return p.ToString(); }
		public static String ToString(SqlDouble    p) { return p.ToString(); }
		public static String ToString(SqlDecimal   p) { return p.ToString(); }
		public static String ToString(SqlMoney     p) { return p.ToString(); }
		public static String ToString(SqlBoolean   p) { return p.ToString(); }
		public static String ToString(SqlDateTime  p) { return p.ToString(); }
		public static String ToString(SqlGuid      p) { return p.ToString(); }
		public static String ToString(SqlBinary    p) { return p.ToString(); }

		public static String ToString(object       p) { return p.ToString(); }

		#endregion

		#region SByte


		// Scalar Types.
		// 
		public static SByte ToSByte(String       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Int16        p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Int32        p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Int64        p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Byte         p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(UInt16       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(UInt32       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(UInt64       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Char         p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Single       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Double       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Boolean      p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(Decimal      p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(DateTime     p) { return Convert.ToSByte(p); }

		// Nullable Types.
		// 
		public static SByte ToSByte(SByte?       p) { return p.HasValue?                 p.Value  : (SByte)0; }
		public static SByte ToSByte(Int16?       p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Int32?       p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Int64?       p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Byte?        p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(UInt16?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(UInt32?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(UInt64?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Char?        p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Single?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Double?      p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Boolean?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(Decimal?     p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }
		public static SByte ToSByte(DateTime?    p) { return p.HasValue? Convert.ToSByte(p.Value) : (SByte)0; }

		// SqlTypes.
		// 
		public static SByte ToSByte(SqlString    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlByte      p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlInt16     p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlInt32     p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlInt64     p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlSingle    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlDouble    p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlDecimal   p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlMoney     p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlBoolean   p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }
		public static SByte ToSByte(SqlDateTime  p) { return p.IsNull? (SByte)0: Convert.ToSByte(p.Value); }

		public static SByte ToSByte(object       p) { return Convert.ToSByte(p); }

		#endregion

		#region Int16


		// Scalar Types.
		// 
		public static Int16 ToInt16(String       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(SByte        p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Int32        p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Int64        p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Byte         p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(UInt16       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(UInt32       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(UInt64       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Char         p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Single       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Double       p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Boolean      p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(Decimal      p) { return Convert.ToInt16(p); }
		public static Int16 ToInt16(DateTime     p) { return Convert.ToInt16(p); }

		// Nullable Types.
		// 
		public static Int16 ToInt16(Int16?       p) { return p.HasValue?                 p.Value  : (Int16)0; }
		public static Int16 ToInt16(SByte?       p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Int32?       p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Int64?       p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Byte?        p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(UInt16?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(UInt32?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(UInt64?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Char?        p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Single?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Double?      p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Boolean?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(Decimal?     p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }
		public static Int16 ToInt16(DateTime?    p) { return p.HasValue? Convert.ToInt16(p.Value) : (Int16)0; }

		// SqlTypes.
		// 
		public static Int16 ToInt16(SqlString    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlByte      p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlInt16     p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlInt32     p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlInt64     p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlSingle    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlDouble    p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlDecimal   p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlMoney     p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlBoolean   p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }
		public static Int16 ToInt16(SqlDateTime  p) { return p.IsNull? (Int16)0: Convert.ToInt16(p.Value); }

		public static Int16 ToInt16(object       p) { return Convert.ToInt16(p); }

		#endregion

		#region Int32


		// Scalar Types.
		// 
		public static Int32 ToInt32(String       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(SByte        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Int16        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Int64        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Byte         p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt16       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt32       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt64       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Char         p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Single       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Double       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Boolean      p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Decimal      p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(DateTime     p) { return Convert.ToInt32(p); }

		// Nullable Types.
		// 
		public static Int32 ToInt32(Int32?       p) { return p.HasValue?                 p.Value  : (Int32)0; }
		public static Int32 ToInt32(SByte?       p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Int16?       p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Int64?       p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Byte?        p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(UInt16?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(UInt32?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(UInt64?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Char?        p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Single?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Double?      p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Boolean?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(Decimal?     p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }
		public static Int32 ToInt32(DateTime?    p) { return p.HasValue? Convert.ToInt32(p.Value) : (Int32)0; }

		// SqlTypes.
		// 
		public static Int32 ToInt32(SqlString    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlByte      p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlInt16     p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlInt32     p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlInt64     p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlSingle    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlDouble    p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlDecimal   p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlMoney     p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlBoolean   p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }
		public static Int32 ToInt32(SqlDateTime  p) { return p.IsNull? (Int32)0: Convert.ToInt32(p.Value); }

		public static Int32 ToInt32(object       p) { return Convert.ToInt32(p); }

		#endregion

		#region Int64


		// Scalar Types.
		// 
		public static Int64 ToInt64(String       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(SByte        p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Int16        p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Int32        p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Byte         p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(UInt16       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(UInt32       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(UInt64       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Char         p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Single       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Double       p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Boolean      p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(Decimal      p) { return Convert.ToInt64(p); }
		public static Int64 ToInt64(DateTime     p) { return Convert.ToInt64(p); }

		// Nullable Types.
		// 
		public static Int64 ToInt64(Int64?       p) { return p.HasValue?                 p.Value  : (Int64)0; }
		public static Int64 ToInt64(SByte?       p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Int16?       p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Int32?       p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Byte?        p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(UInt16?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(UInt32?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(UInt64?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Char?        p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Single?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Double?      p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Boolean?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(Decimal?     p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }
		public static Int64 ToInt64(DateTime?    p) { return p.HasValue? Convert.ToInt64(p.Value) : (Int64)0; }

		// SqlTypes.
		// 
		public static Int64 ToInt64(SqlString    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlByte      p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlInt16     p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlInt32     p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlInt64     p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlSingle    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlDouble    p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlDecimal   p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlMoney     p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlBoolean   p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }
		public static Int64 ToInt64(SqlDateTime  p) { return p.IsNull? (Int64)0: Convert.ToInt64(p.Value); }

		public static Int64 ToInt64(object       p) { return Convert.ToInt64(p); }

		#endregion

		#region Byte


		// Scalar Types.
		// 
		public static Byte ToByte(String       p) { return Convert.ToByte(p); }
		public static Byte ToByte(SByte        p) { return Convert.ToByte(p); }
		public static Byte ToByte(Int16        p) { return Convert.ToByte(p); }
		public static Byte ToByte(Int32        p) { return Convert.ToByte(p); }
		public static Byte ToByte(Int64        p) { return Convert.ToByte(p); }
		public static Byte ToByte(UInt16       p) { return Convert.ToByte(p); }
		public static Byte ToByte(UInt32       p) { return Convert.ToByte(p); }
		public static Byte ToByte(UInt64       p) { return Convert.ToByte(p); }
		public static Byte ToByte(Char         p) { return Convert.ToByte(p); }
		public static Byte ToByte(Single       p) { return Convert.ToByte(p); }
		public static Byte ToByte(Double       p) { return Convert.ToByte(p); }
		public static Byte ToByte(Boolean      p) { return Convert.ToByte(p); }
		public static Byte ToByte(Decimal      p) { return Convert.ToByte(p); }
		public static Byte ToByte(DateTime     p) { return Convert.ToByte(p); }

		// Nullable Types.
		// 
		public static Byte ToByte(Byte?        p) { return p.HasValue?                 p.Value  : (Byte)0; }
		public static Byte ToByte(SByte?       p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Int16?       p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Int32?       p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Int64?       p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(UInt16?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(UInt32?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(UInt64?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Char?        p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Single?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Double?      p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Boolean?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(Decimal?     p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }
		public static Byte ToByte(DateTime?    p) { return p.HasValue? Convert.ToByte(p.Value) : (Byte)0; }

		// SqlTypes.
		// 
		public static Byte ToByte(SqlString    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlByte      p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlInt16     p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlInt32     p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlInt64     p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlSingle    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlDouble    p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlDecimal   p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlMoney     p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlBoolean   p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }
		public static Byte ToByte(SqlDateTime  p) { return p.IsNull? (Byte)0: Convert.ToByte(p.Value); }

		public static Byte ToByte(object       p) { return Convert.ToByte(p); }

		#endregion

		#region UInt16


		// Scalar Types.
		// 
		public static UInt16 ToUInt16(String       p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Byte         p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Int16        p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Int32        p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Int64        p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(SByte        p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(UInt32       p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(UInt64       p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Char         p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Single       p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Double       p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Boolean      p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(Decimal      p) { return Convert.ToUInt16(p); }
		public static UInt16 ToUInt16(DateTime     p) { return Convert.ToUInt16(p); }

		// Nullable Types.
		// 
		public static UInt16 ToUInt16(UInt16?      p) { return p.HasValue?                 p.Value  : (UInt16)0; }
		public static UInt16 ToUInt16(SByte?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Int16?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Int32?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Int64?       p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Byte?        p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(UInt32?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(UInt64?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Char?        p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Single?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Double?      p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Boolean?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(Decimal?     p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }
		public static UInt16 ToUInt16(DateTime?    p) { return p.HasValue? Convert.ToUInt16(p.Value) : (UInt16)0; }

		// SqlTypes.
		// 
		public static UInt16 ToUInt16(SqlString    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlByte      p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlInt16     p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlInt32     p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlInt64     p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlSingle    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlDouble    p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlDecimal   p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlMoney     p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlBoolean   p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }
		public static UInt16 ToUInt16(SqlDateTime  p) { return p.IsNull? (UInt16)0: Convert.ToUInt16(p.Value); }

		public static UInt16 ToUInt16(object       p) { return Convert.ToUInt16(p); }

		#endregion

		#region UInt32


		// Scalar Types.
		// 
		public static UInt32 ToUInt32(String       p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(SByte        p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Int16        p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Int32        p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Int64        p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Byte         p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(UInt16       p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(UInt64       p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Char         p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Single       p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Double       p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Boolean      p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(Decimal      p) { return Convert.ToUInt32(p); }
		public static UInt32 ToUInt32(DateTime     p) { return Convert.ToUInt32(p); }

		// Nullable Types.
		// 
		public static UInt32 ToUInt32(UInt32?      p) { return p.HasValue?                 p.Value  : (UInt32)0; }
		public static UInt32 ToUInt32(SByte?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Int16?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Int32?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Int64?       p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Byte?        p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(UInt16?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(UInt64?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Char?        p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Single?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Double?      p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Boolean?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(Decimal?     p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }
		public static UInt32 ToUInt32(DateTime?    p) { return p.HasValue? Convert.ToUInt32(p.Value) : (UInt32)0; }

		// SqlTypes.
		// 
		public static UInt32 ToUInt32(SqlString    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlByte      p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlInt16     p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlInt32     p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlInt64     p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlSingle    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlDouble    p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlDecimal   p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlMoney     p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlBoolean   p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }
		public static UInt32 ToUInt32(SqlDateTime  p) { return p.IsNull? (UInt32)0: Convert.ToUInt32(p.Value); }

		public static UInt32 ToUInt32(object       p) { return Convert.ToUInt32(p); }

		#endregion

		#region UInt64


		// Scalar Types.
		// 
		public static UInt64 ToUInt64(String       p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(SByte        p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Int16        p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Int32        p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Int64        p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Byte         p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(UInt16       p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(UInt32       p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Char         p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Single       p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Double       p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Boolean      p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(Decimal      p) { return Convert.ToUInt64(p); }
		public static UInt64 ToUInt64(DateTime     p) { return Convert.ToUInt64(p); }

		// Nullable Types.
		// 
		public static UInt64 ToUInt64(UInt64?      p) { return p.HasValue?                 p.Value  : (UInt64)0; }
		public static UInt64 ToUInt64(SByte?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Int16?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Int32?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Int64?       p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Byte?        p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(UInt16?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(UInt32?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Char?        p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Single?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Double?      p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Boolean?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(Decimal?     p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }
		public static UInt64 ToUInt64(DateTime?    p) { return p.HasValue? Convert.ToUInt64(p.Value) : (UInt64)0; }

		// SqlTypes.
		// 
		public static UInt64 ToUInt64(SqlString    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlByte      p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlInt16     p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlInt32     p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlInt64     p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlSingle    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlDouble    p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlDecimal   p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlMoney     p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlBoolean   p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }
		public static UInt64 ToUInt64(SqlDateTime  p) { return p.IsNull? (UInt64)0: Convert.ToUInt64(p.Value); }

		public static UInt64 ToUInt64(object       p) { return Convert.ToUInt64(p); }

		#endregion



		// A comment for nullable SByte.
		// 
		#region NullableSByte

		public static SByte? ToNullableSByte(String       p) { return p == null? null: (SByte?)Convert.ToSByte(p); }

		public static SByte? ToNullableSByte(object       p) { return Convert.ToSByte(p); }

		#endregion

		#region TestRegion

		#region Guid

		public static Guid ToGuid(String       p) { return new Guid(p);                               }
		public static Guid ToGuid(Guid?        p) { return p.HasValue? p.Value : Guid.Empty;          }
		public static Guid ToGuid(SqlString    p) { return p.IsNull? Guid.Empty: new Guid(p.Value);   }
		public static Guid ToGuid(SqlGuid      p) { return p.IsNull? Guid.Empty: p.Value;             }
		public static Guid ToGuid(SqlBinary    p) { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; }

		public static Guid ToGuid(object       p)
		{
			if (p == null)
				return Guid.Empty;

			if (p is Guid        ) return (Guid        )p;


			if (p is String      ) return ToGuid((String      )p);
			if (p is Guid?       ) return ToGuid((Guid?       )p);
			if (p is SqlString   ) return ToGuid((SqlString   )p);
			if (p is SqlGuid     ) return ToGuid((SqlGuid     )p);
			if (p is SqlBinary   ) return ToGuid((SqlBinary   )p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(Guid        ).FullName));
		}

		#endregion

		#endregion
	}
}
