using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using BLToolkit.Properties;

namespace BLToolkit.Common
{
	/// <summary>Converts a base data type to another base data type.</summary>
	public class Convert
	{
		#region Scalar Types

		#region String

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(SByte p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int16 p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int32 p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int64 p)           { return p.ToString(); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Byte p)            { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt16 p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt32 p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt64 p)          { return p.ToString(); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Single p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Double p)          { return p.ToString(); }

		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Boolean p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Decimal p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Char p)            { return p.ToString(); }
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(TimeSpan p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(DateTime p)        { return p.ToString(); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(DateTimeOffset p)  { return p.ToString(); }
		#endif
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Guid p)            { return p.ToString(); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(SByte? p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int16? p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int32? p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Int64? p)          { return p.ToString(); }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Byte? p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt16? p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt32? p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>String</c> value.</summary>
		[CLSCompliant(false)]
		public static String ToString(UInt64? p)         { return p.ToString(); }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Single? p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Double? p)         { return p.ToString(); }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Boolean? p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Decimal? p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Char? p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(TimeSpan? p)       { return p.ToString(); }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(DateTime? p)       { return p.ToString(); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(DateTimeOffset? p) { return p.ToString(); }
		#endif
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Guid? p)           { return p.ToString(); }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlString p)       { return p.ToString(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlByte p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlInt16 p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlInt32 p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlInt64 p)        { return p.ToString(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlSingle p)       { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlDouble p)       { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlDecimal p)      { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlMoney p)        { return p.ToString(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlBoolean p)      { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlGuid p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlChars p)        { return p.IsNull?  null : p.ToSqlString().Value; }
		/// <summary>Converts the value from <c>SqlXml</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(SqlXml p)          { return p.IsNull?  null : p.Value; }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Type p)            { return p == null? null: p.FullName; }
		/// <summary>Converts the value from <c>XmlDocument</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(XmlDocument p)     { return p == null? null: p.InnerXml; }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>String</c> value.</summary>
		public static String ToString(Byte[] p)          { return p == null? null: System.Text.Encoding.UTF8.GetString(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>String</c> value.</summary>
		public static String ToString(object p)         
		{
			if (p == null || p is DBNull) return String.Empty;

			if (p is String) return (String)p;

			// Scalar Types.
			//
			if (p is SByte)           return ToString((SByte)p);
			if (p is Int16)           return ToString((Int16)p);
			if (p is Int32)           return ToString((Int32)p);
			if (p is Int64)           return ToString((Int64)p);

			if (p is Byte)            return ToString((Byte)p);
			if (p is UInt16)          return ToString((UInt16)p);
			if (p is UInt32)          return ToString((UInt32)p);
			if (p is UInt64)          return ToString((UInt64)p);

			if (p is Single)          return ToString((Single)p);
			if (p is Double)          return ToString((Double)p);

			if (p is Boolean)         return ToString((Boolean)p);
			if (p is Decimal)         return ToString((Decimal)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToString((SqlString)p);

			if (p is SqlByte)         return ToString((SqlByte)p);
			if (p is SqlInt16)        return ToString((SqlInt16)p);
			if (p is SqlInt32)        return ToString((SqlInt32)p);
			if (p is SqlInt64)        return ToString((SqlInt64)p);

			if (p is SqlSingle)       return ToString((SqlSingle)p);
			if (p is SqlDouble)       return ToString((SqlDouble)p);
			if (p is SqlDecimal)      return ToString((SqlDecimal)p);
			if (p is SqlMoney)        return ToString((SqlMoney)p);

			if (p is SqlBoolean)      return ToString((SqlBoolean)p);
			if (p is SqlChars)        return ToString((SqlChars)p);
			if (p is SqlXml)          return ToString((SqlXml)p);

			// Other Types
			//
			if (p is Type)            return ToString((Type)p);
			if (p is XmlDocument)     return ToString((XmlDocument)p);
			if (p is Byte[])          return ToString((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToString(null);
			if (p is IFormattable) return ((IFormattable)p).ToString(null, null);

			return p.ToString();
		}

		#endregion

		#region SByte

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(String p)          { return p == null? (SByte)0: SByte.Parse(p); }

		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int16 p)           { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int32 p)           { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int64 p)           { return      checked((SByte)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Byte p)            { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt16 p)          { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt32 p)          { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt64 p)          { return      checked((SByte)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Single p)          { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Double p)          { return      checked((SByte)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Decimal p)         { return      checked((SByte)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Boolean p)         { return       (SByte)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Char p)            { return      checked((SByte)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SByte? p)          { return p.HasValue?                 p.Value: (SByte)0; }

		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int16? p)          { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int32? p)          { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Int64? p)          { return p.HasValue? checked((SByte)p.Value): (SByte)0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Byte? p)           { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt16? p)         { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt32? p)         { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(UInt64? p)         { return p.HasValue? checked((SByte)p.Value): (SByte)0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Single? p)         { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Double? p)         { return p.HasValue? checked((SByte)p.Value): (SByte)0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Decimal? p)        { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Char? p)           { return p.HasValue? checked((SByte)p.Value): (SByte)0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Boolean? p)        { return (p.HasValue && p.Value)?   (SByte)1: (SByte)0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlString p)       { return p.IsNull? (SByte)0:        ToSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlByte p)         { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlInt16 p)        { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlInt32 p)        { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlInt64 p)        { return p.IsNull? (SByte)0:        ToSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlSingle p)       { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlDouble p)       { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlDecimal p)      { return p.IsNull? (SByte)0:        ToSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlMoney p)        { return p.IsNull? (SByte)0:        ToSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(SqlBoolean p)      { return p.IsNull? (SByte)0:        ToSByte(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(Byte[] p)          { return p == null || p.Length == 0? (SByte)0: checked((SByte)p[0]); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte ToSByte(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is SByte) return (SByte)p;

			// Scalar Types.
			//
			if (p is String)          return ToSByte((String)p);

			if (p is Int16)           return ToSByte((Int16)p);
			if (p is Int32)           return ToSByte((Int32)p);
			if (p is Int64)           return ToSByte((Int64)p);

			if (p is Byte)            return ToSByte((Byte)p);
			if (p is UInt16)          return ToSByte((UInt16)p);
			if (p is UInt32)          return ToSByte((UInt32)p);
			if (p is UInt64)          return ToSByte((UInt64)p);

			if (p is Single)          return ToSByte((Single)p);
			if (p is Double)          return ToSByte((Double)p);

			if (p is Decimal)         return ToSByte((Decimal)p);
			if (p is Boolean)         return ToSByte((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSByte((SqlString)p);

			if (p is SqlByte)         return ToSByte((SqlByte)p);
			if (p is SqlInt16)        return ToSByte((SqlInt16)p);
			if (p is SqlInt32)        return ToSByte((SqlInt32)p);
			if (p is SqlInt64)        return ToSByte((SqlInt64)p);

			if (p is SqlSingle)       return ToSByte((SqlSingle)p);
			if (p is SqlDouble)       return ToSByte((SqlDouble)p);
			if (p is SqlDecimal)      return ToSByte((SqlDecimal)p);
			if (p is SqlMoney)        return ToSByte((SqlMoney)p);

			if (p is SqlBoolean)      return ToSByte((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToSByte((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToSByte(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(SByte));
		}

		#endregion

		#region Int16

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(String p)          { return p == null? (Int16)0: Int16.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(SByte p)           { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Int32 p)           { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Int64 p)           { return      checked((Int16)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Byte p)            { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt16 p)          { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt32 p)          { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt64 p)          { return      checked((Int16)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Single p)          { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Double p)          { return      checked((Int16)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Decimal p)         { return      checked((Int16)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Boolean p)         { return       (Int16)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Char p)            { return      checked((Int16)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Int16? p)          { return p.HasValue?                 p.Value: (Int16)0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(SByte? p)          { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Int32? p)          { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Int64? p)          { return p.HasValue? checked((Int16)p.Value): (Int16)0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Byte? p)           { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt16? p)         { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt32? p)         { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int16</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16 ToInt16(UInt64? p)         { return p.HasValue? checked((Int16)p.Value): (Int16)0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Single? p)         { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Double? p)         { return p.HasValue? checked((Int16)p.Value): (Int16)0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Decimal? p)        { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Char? p)           { return p.HasValue? checked((Int16)p.Value): (Int16)0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Boolean? p)        { return (p.HasValue && p.Value)?   (Int16)1: (Int16)0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlInt16 p)        { return p.IsNull? (Int16)0:                p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlString p)       { return p.IsNull? (Int16)0:        ToInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlByte p)         { return p.IsNull? (Int16)0:        ToInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlInt32 p)        { return p.IsNull? (Int16)0:        ToInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlInt64 p)        { return p.IsNull? (Int16)0:        ToInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlSingle p)       { return p.IsNull? (Int16)0:        ToInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlDouble p)       { return p.IsNull? (Int16)0:        ToInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlDecimal p)      { return p.IsNull? (Int16)0:        ToInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlMoney p)        { return p.IsNull? (Int16)0:        ToInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(SqlBoolean p)      { return p.IsNull? (Int16)0:        ToInt16(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(Byte[] p)          { return p == null || p.Length == 0? (Int16)0: BitConverter.ToInt16(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int16</c> value.</summary>
		public static Int16 ToInt16(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is Int16) return (Int16)p;

			// Scalar Types.
			//
			if (p is String)          return ToInt16((String)p);

			if (p is SByte)           return ToInt16((SByte)p);
			if (p is Int32)           return ToInt16((Int32)p);
			if (p is Int64)           return ToInt16((Int64)p);

			if (p is Byte)            return ToInt16((Byte)p);
			if (p is UInt16)          return ToInt16((UInt16)p);
			if (p is UInt32)          return ToInt16((UInt32)p);
			if (p is UInt64)          return ToInt16((UInt64)p);

			if (p is Single)          return ToInt16((Single)p);
			if (p is Double)          return ToInt16((Double)p);

			if (p is Decimal)         return ToInt16((Decimal)p);
			if (p is Boolean)         return ToInt16((Boolean)p);

			// SqlTypes
			//
			if (p is SqlInt16)        return ToInt16((SqlInt16)p);
			if (p is SqlString)       return ToInt16((SqlString)p);

			if (p is SqlByte)         return ToInt16((SqlByte)p);
			if (p is SqlInt32)        return ToInt16((SqlInt32)p);
			if (p is SqlInt64)        return ToInt16((SqlInt64)p);

			if (p is SqlSingle)       return ToInt16((SqlSingle)p);
			if (p is SqlDouble)       return ToInt16((SqlDouble)p);
			if (p is SqlDecimal)      return ToInt16((SqlDecimal)p);
			if (p is SqlMoney)        return ToInt16((SqlMoney)p);

			if (p is SqlBoolean)      return ToInt16((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToInt16((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt16(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int16));
		}

		#endregion

		#region Int32

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(String p)          { return p == null? 0: Int32.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(SByte p)           { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Int16 p)           { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Int64 p)           { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Byte p)            { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt16 p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt32 p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt64 p)          { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Single p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Double p)          { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Decimal p)         { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Boolean p)         { return p? 1: 0; }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Char p)            { return checked((Int32)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Int32? p)          { return p.HasValue?                 p.Value: 0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(SByte? p)          { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Int16? p)          { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Int64? p)          { return p.HasValue? checked((Int32)p.Value): 0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Byte? p)           { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt16? p)         { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt32? p)         { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int32</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32 ToInt32(UInt64? p)         { return p.HasValue? checked((Int32)p.Value): 0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Single? p)         { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Double? p)         { return p.HasValue? checked((Int32)p.Value): 0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Decimal? p)        { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Char? p)           { return p.HasValue? checked((Int32)p.Value): 0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Boolean? p)        { return (p.HasValue && p.Value)?   1: 0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlInt32 p)        { return p.IsNull? 0:         p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlString p)       { return p.IsNull? 0: ToInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlByte p)         { return p.IsNull? 0: ToInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlInt16 p)        { return p.IsNull? 0: ToInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlInt64 p)        { return p.IsNull? 0: ToInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlSingle p)       { return p.IsNull? 0: ToInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlDouble p)       { return p.IsNull? 0: ToInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlDecimal p)      { return p.IsNull? 0: ToInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlMoney p)        { return p.IsNull? 0: ToInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(SqlBoolean p)      { return p.IsNull? 0: ToInt32(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(Byte[] p)          { return p == null || p.Length == 0? 0: BitConverter.ToInt32(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int32</c> value.</summary>
		public static Int32 ToInt32(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is Int32) return (Int32)p;

			// Scalar Types.
			//
			if (p is String)          return ToInt32((String)p);

			if (p is SByte)           return ToInt32((SByte)p);
			if (p is Int16)           return ToInt32((Int16)p);
			if (p is Int64)           return ToInt32((Int64)p);

			if (p is Byte)            return ToInt32((Byte)p);
			if (p is UInt16)          return ToInt32((UInt16)p);
			if (p is UInt32)          return ToInt32((UInt32)p);
			if (p is UInt64)          return ToInt32((UInt64)p);

			if (p is Single)          return ToInt32((Single)p);
			if (p is Double)          return ToInt32((Double)p);

			if (p is Decimal)         return ToInt32((Decimal)p);
			if (p is Boolean)         return ToInt32((Boolean)p);

			// SqlTypes
			//
			if (p is SqlInt32)        return ToInt32((SqlInt32)p);
			if (p is SqlString)       return ToInt32((SqlString)p);

			if (p is SqlByte)         return ToInt32((SqlByte)p);
			if (p is SqlInt16)        return ToInt32((SqlInt16)p);
			if (p is SqlInt64)        return ToInt32((SqlInt64)p);

			if (p is SqlSingle)       return ToInt32((SqlSingle)p);
			if (p is SqlDouble)       return ToInt32((SqlDouble)p);
			if (p is SqlDecimal)      return ToInt32((SqlDecimal)p);
			if (p is SqlMoney)        return ToInt32((SqlMoney)p);

			if (p is SqlBoolean)      return ToInt32((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToInt32((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt32(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int32));
		}

		#endregion

		#region Int64

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(String p)          { return p == null? 0: Int64.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(SByte p)           { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Int16 p)           { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Int32 p)           { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Byte p)            { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt16 p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt32 p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt64 p)          { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Single p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Double p)          { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Decimal p)         { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Char p)            { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Boolean p)         { return p? 1: 0; }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(DateTime p)        { return (p - DateTime.MinValue).Ticks; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(DateTimeOffset p)  { return (p - DateTime.MinValue).Ticks; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(TimeSpan p)        { return p.Ticks; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Int64? p)          { return p.HasValue?                 p.Value: 0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(SByte? p)          { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Int16? p)          { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Int32? p)          { return p.HasValue? checked((Int64)p.Value): 0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Byte? p)           { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt16? p)         { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt32? p)         { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int64</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64 ToInt64(UInt64? p)         { return p.HasValue? checked((Int64)p.Value): 0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Single? p)         { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Double? p)         { return p.HasValue? checked((Int64)p.Value): 0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Decimal? p)        { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Char? p)           { return p.HasValue? checked((Int64)p.Value): 0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Boolean? p)        { return (p.HasValue && p.Value)? 1: 0; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(DateTime? p)       { return p.HasValue? (p.Value - DateTime.MinValue).Ticks: 0; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(DateTimeOffset? p) { return p.HasValue? (p.Value - DateTime.MinValue).Ticks: 0; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(TimeSpan? p)       { return p.HasValue? p.Value.Ticks: 0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlInt64 p)        { return p.IsNull? 0:         p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlString p)       { return p.IsNull? 0: ToInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlByte p)         { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlInt16 p)        { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlInt32 p)        { return p.IsNull? 0: ToInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlSingle p)       { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlDouble p)       { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlDecimal p)      { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlMoney p)        { return p.IsNull? 0: ToInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlBoolean p)      { return p.IsNull? 0: ToInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(SqlDateTime p)     { return p.IsNull? 0: ToInt64(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(Byte[] p)          { return p == null || p.Length == 0? 0: BitConverter.ToInt64(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int64</c> value.</summary>
		public static Int64 ToInt64(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is Int64) return (Int64)p;

			// Scalar Types.
			//
			if (p is String)          return ToInt64((String)p);

			if (p is SByte)           return ToInt64((SByte)p);
			if (p is Int16)           return ToInt64((Int16)p);
			if (p is Int32)           return ToInt64((Int32)p);

			if (p is Byte)            return ToInt64((Byte)p);
			if (p is UInt16)          return ToInt64((UInt16)p);
			if (p is UInt32)          return ToInt64((UInt32)p);
			if (p is UInt64)          return ToInt64((UInt64)p);

			if (p is Single)          return ToInt64((Single)p);
			if (p is Double)          return ToInt64((Double)p);

			if (p is Decimal)         return ToInt64((Decimal)p);
			if (p is Boolean)         return ToInt64((Boolean)p);
			if (p is DateTime)        return ToInt64((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToInt64((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToInt64((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlInt64)        return ToInt64((SqlInt64)p);
			if (p is SqlString)       return ToInt64((SqlString)p);

			if (p is SqlByte)         return ToInt64((SqlByte)p);
			if (p is SqlInt16)        return ToInt64((SqlInt16)p);
			if (p is SqlInt32)        return ToInt64((SqlInt32)p);

			if (p is SqlSingle)       return ToInt64((SqlSingle)p);
			if (p is SqlDouble)       return ToInt64((SqlDouble)p);
			if (p is SqlDecimal)      return ToInt64((SqlDecimal)p);
			if (p is SqlMoney)        return ToInt64((SqlMoney)p);

			if (p is SqlBoolean)      return ToInt64((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToInt64((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt64(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int64));
		}

		#endregion

		#region Byte

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(String p)          { return p == null? (Byte)0: Byte.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(SByte p)           { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int16 p)           { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int32 p)           { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int64 p)           { return      checked((Byte)p); }

		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt16 p)          { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt32 p)          { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt64 p)          { return      checked((Byte)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Single p)          { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Double p)          { return      checked((Byte)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Decimal p)         { return      checked((Byte)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Boolean p)         { return       (Byte)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Char p)            { return      checked((Byte)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Byte? p)           { return p.HasValue?               p.Value:  (Byte)0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(SByte? p)          { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int16? p)          { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int32? p)          { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Int64? p)          { return p.HasValue? checked((Byte)p.Value): (Byte)0; }

		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt16? p)         { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt32? p)         { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Byte</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte ToByte(UInt64? p)         { return p.HasValue? checked((Byte)p.Value): (Byte)0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Single? p)         { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Double? p)         { return p.HasValue? checked((Byte)p.Value): (Byte)0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Decimal? p)        { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Char? p)           { return p.HasValue? checked((Byte)p.Value): (Byte)0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Boolean? p)        { return (p.HasValue && p.Value)?   (Byte)1: (Byte)0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlByte p)         { return p.IsNull? (Byte)0:               p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlString p)       { return p.IsNull? (Byte)0:        ToByte(p.Value); }

		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlInt16 p)        { return p.IsNull? (Byte)0:        ToByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlInt32 p)        { return p.IsNull? (Byte)0:        ToByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlInt64 p)        { return p.IsNull? (Byte)0:        ToByte(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlSingle p)       { return p.IsNull? (Byte)0:        ToByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlDouble p)       { return p.IsNull? (Byte)0:        ToByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlDecimal p)      { return p.IsNull? (Byte)0:        ToByte(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlMoney p)        { return p.IsNull? (Byte)0:        ToByte(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(SqlBoolean p)      { return p.IsNull? (Byte)0:        ToByte(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(Byte[] p)          { return p == null || p.Length == 0? (Byte)0: p[0]; }

		/// <summary>Converts the value of a specified object to an equivalent <c>Byte</c> value.</summary>
		public static Byte ToByte(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is Byte) return (Byte)p;

			// Scalar Types.
			//
			if (p is String)          return ToByte((String)p);

			if (p is SByte)           return ToByte((SByte)p);
			if (p is Int16)           return ToByte((Int16)p);
			if (p is Int32)           return ToByte((Int32)p);
			if (p is Int64)           return ToByte((Int64)p);

			if (p is UInt16)          return ToByte((UInt16)p);
			if (p is UInt32)          return ToByte((UInt32)p);
			if (p is UInt64)          return ToByte((UInt64)p);

			if (p is Single)          return ToByte((Single)p);
			if (p is Double)          return ToByte((Double)p);

			if (p is Decimal)         return ToByte((Decimal)p);
			if (p is Boolean)         return ToByte((Boolean)p);

			// SqlTypes
			//
			if (p is SqlByte)         return ToByte((SqlByte)p);
			if (p is SqlString)       return ToByte((SqlString)p);

			if (p is SqlInt16)        return ToByte((SqlInt16)p);
			if (p is SqlInt32)        return ToByte((SqlInt32)p);
			if (p is SqlInt64)        return ToByte((SqlInt64)p);

			if (p is SqlSingle)       return ToByte((SqlSingle)p);
			if (p is SqlDouble)       return ToByte((SqlDouble)p);
			if (p is SqlDecimal)      return ToByte((SqlDecimal)p);
			if (p is SqlMoney)        return ToByte((SqlMoney)p);

			if (p is SqlBoolean)      return ToByte((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToByte((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToByte(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Byte));
		}

		#endregion

		#region UInt16

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(String p)          { return p == null? (UInt16)0: UInt16.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SByte p)           { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int16 p)           { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int32 p)           { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int64 p)           { return      checked((UInt16)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Byte p)            { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(UInt32 p)          { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(UInt64 p)          { return      checked((UInt16)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Single p)          { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Double p)          { return      checked((UInt16)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Decimal p)         { return      checked((UInt16)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Boolean p)         { return       (UInt16)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Char p)            { return      checked((UInt16)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(UInt16? p)         { return p.HasValue?                 p.Value:  (UInt16)0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SByte? p)          { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int16? p)          { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int32? p)          { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Int64? p)          { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Byte? p)           { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(UInt32? p)         { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(UInt64? p)         { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Single? p)         { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Double? p)         { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Decimal? p)        { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Char? p)           { return p.HasValue? checked((UInt16)p.Value): (UInt16)0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Boolean? p)        { return (p.HasValue && p.Value)?   (UInt16)1: (UInt16)0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlString p)       { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlByte p)         { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlInt16 p)        { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlInt32 p)        { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlInt64 p)        { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlSingle p)       { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlDouble p)       { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlDecimal p)      { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlMoney p)        { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(SqlBoolean p)      { return p.IsNull? (UInt16)0:        ToUInt16(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(Byte[] p)          { return p == null || p.Length == 0? (UInt16)0: BitConverter.ToUInt16(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16 ToUInt16(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is UInt16) return (UInt16)p;

			// Scalar Types.
			//
			if (p is String)          return ToUInt16((String)p);

			if (p is SByte)           return ToUInt16((SByte)p);
			if (p is Int16)           return ToUInt16((Int16)p);
			if (p is Int32)           return ToUInt16((Int32)p);
			if (p is Int64)           return ToUInt16((Int64)p);

			if (p is Byte)            return ToUInt16((Byte)p);
			if (p is UInt32)          return ToUInt16((UInt32)p);
			if (p is UInt64)          return ToUInt16((UInt64)p);

			if (p is Single)          return ToUInt16((Single)p);
			if (p is Double)          return ToUInt16((Double)p);

			if (p is Decimal)         return ToUInt16((Decimal)p);
			if (p is Boolean)         return ToUInt16((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToUInt16((SqlString)p);

			if (p is SqlByte)         return ToUInt16((SqlByte)p);
			if (p is SqlInt16)        return ToUInt16((SqlInt16)p);
			if (p is SqlInt32)        return ToUInt16((SqlInt32)p);
			if (p is SqlInt64)        return ToUInt16((SqlInt64)p);

			if (p is SqlSingle)       return ToUInt16((SqlSingle)p);
			if (p is SqlDouble)       return ToUInt16((SqlDouble)p);
			if (p is SqlDecimal)      return ToUInt16((SqlDecimal)p);
			if (p is SqlMoney)        return ToUInt16((SqlMoney)p);

			if (p is SqlBoolean)      return ToUInt16((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToUInt16((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt16(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt16));
		}

		#endregion

		#region UInt32

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(String p)          { return p == null? 0: UInt32.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SByte p)           { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int16 p)           { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int32 p)           { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int64 p)           { return      checked((UInt32)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Byte p)            { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(UInt16 p)          { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(UInt64 p)          { return      checked((UInt32)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Single p)          { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Double p)          { return      checked((UInt32)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Decimal p)         { return      checked((UInt32)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Boolean p)         { return       (UInt32)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Char p)            { return      checked((UInt32)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(UInt32? p)         { return p.HasValue?                 p.Value:  0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SByte? p)          { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int16? p)          { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int32? p)          { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Int64? p)          { return p.HasValue? checked((UInt32)p.Value): 0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Byte? p)           { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(UInt16? p)         { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(UInt64? p)         { return p.HasValue? checked((UInt32)p.Value): 0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Single? p)         { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Double? p)         { return p.HasValue? checked((UInt32)p.Value): 0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Decimal? p)        { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Char? p)           { return p.HasValue? checked((UInt32)p.Value): 0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Boolean? p)        { return (p.HasValue && p.Value)?   (UInt32)1: 0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlString p)       { return p.IsNull? 0:        ToUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlByte p)         { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlInt16 p)        { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlInt32 p)        { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlInt64 p)        { return p.IsNull? 0:        ToUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlSingle p)       { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlDouble p)       { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlDecimal p)      { return p.IsNull? 0:        ToUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlMoney p)        { return p.IsNull? 0:        ToUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(SqlBoolean p)      { return p.IsNull? 0:        ToUInt32(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(Byte[] p)          { return p == null || p.Length == 0? 0: BitConverter.ToUInt32(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32 ToUInt32(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is UInt32) return (UInt32)p;

			// Scalar Types.
			//
			if (p is String)          return ToUInt32((String)p);

			if (p is SByte)           return ToUInt32((SByte)p);
			if (p is Int16)           return ToUInt32((Int16)p);
			if (p is Int32)           return ToUInt32((Int32)p);
			if (p is Int64)           return ToUInt32((Int64)p);

			if (p is Byte)            return ToUInt32((Byte)p);
			if (p is UInt16)          return ToUInt32((UInt16)p);
			if (p is UInt64)          return ToUInt32((UInt64)p);

			if (p is Single)          return ToUInt32((Single)p);
			if (p is Double)          return ToUInt32((Double)p);

			if (p is Decimal)         return ToUInt32((Decimal)p);
			if (p is Boolean)         return ToUInt32((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToUInt32((SqlString)p);

			if (p is SqlByte)         return ToUInt32((SqlByte)p);
			if (p is SqlInt16)        return ToUInt32((SqlInt16)p);
			if (p is SqlInt32)        return ToUInt32((SqlInt32)p);
			if (p is SqlInt64)        return ToUInt32((SqlInt64)p);

			if (p is SqlSingle)       return ToUInt32((SqlSingle)p);
			if (p is SqlDouble)       return ToUInt32((SqlDouble)p);
			if (p is SqlDecimal)      return ToUInt32((SqlDecimal)p);
			if (p is SqlMoney)        return ToUInt32((SqlMoney)p);

			if (p is SqlBoolean)      return ToUInt32((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToUInt32((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt32(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt32));
		}

		#endregion

		#region UInt64

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(String p)          { return p == null? 0: UInt64.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SByte p)           { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int16 p)           { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int32 p)           { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int64 p)           { return checked((UInt64)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Byte p)            { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(UInt16 p)          { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(UInt32 p)          { return checked((UInt64)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Single p)          { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Double p)          { return checked((UInt64)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Decimal p)         { return checked((UInt64)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Boolean p)         { return (UInt64)(p? 1: 0); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Char p)            { return checked((UInt64)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(UInt64? p)         { return p.HasValue?                 p.Value:  0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SByte? p)          { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int16? p)          { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int32? p)          { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Int64? p)          { return p.HasValue? checked((UInt64)p.Value): 0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Byte? p)           { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(UInt16? p)         { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(UInt32? p)         { return p.HasValue? checked((UInt64)p.Value): 0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Single? p)         { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Double? p)         { return p.HasValue? checked((UInt64)p.Value): 0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Decimal? p)        { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Char? p)           { return p.HasValue? checked((UInt64)p.Value): 0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Boolean? p)        { return (p.HasValue && p.Value)?   (UInt64)1: 0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlString p)       { return p.IsNull? 0:        ToUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlByte p)         { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlInt16 p)        { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlInt32 p)        { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlInt64 p)        { return p.IsNull? 0:        ToUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlSingle p)       { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlDouble p)       { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlDecimal p)      { return p.IsNull? 0:        ToUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlMoney p)        { return p.IsNull? 0:        ToUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(SqlBoolean p)      { return p.IsNull? 0:        ToUInt64(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(Byte[] p)          { return p == null || p.Length == 0? 0: BitConverter.ToUInt64(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64 ToUInt64(object p)         
		{
			if (p == null || p is DBNull) return 0;

			if (p is UInt64) return (UInt64)p;

			// Scalar Types.
			//
			if (p is String)          return ToUInt64((String)p);

			if (p is SByte)           return ToUInt64((SByte)p);
			if (p is Int16)           return ToUInt64((Int16)p);
			if (p is Int32)           return ToUInt64((Int32)p);
			if (p is Int64)           return ToUInt64((Int64)p);

			if (p is Byte)            return ToUInt64((Byte)p);
			if (p is UInt16)          return ToUInt64((UInt16)p);
			if (p is UInt32)          return ToUInt64((UInt32)p);

			if (p is Single)          return ToUInt64((Single)p);
			if (p is Double)          return ToUInt64((Double)p);

			if (p is Decimal)         return ToUInt64((Decimal)p);
			if (p is Boolean)         return ToUInt64((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToUInt64((SqlString)p);

			if (p is SqlByte)         return ToUInt64((SqlByte)p);
			if (p is SqlInt16)        return ToUInt64((SqlInt16)p);
			if (p is SqlInt32)        return ToUInt64((SqlInt32)p);
			if (p is SqlInt64)        return ToUInt64((SqlInt64)p);

			if (p is SqlSingle)       return ToUInt64((SqlSingle)p);
			if (p is SqlDouble)       return ToUInt64((SqlDouble)p);
			if (p is SqlDecimal)      return ToUInt64((SqlDecimal)p);
			if (p is SqlMoney)        return ToUInt64((SqlMoney)p);

			if (p is SqlBoolean)      return ToUInt64((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToUInt64((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt64(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt64));
		}

		#endregion

		#region Char

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(String p)          { return p == null? (Char)0: Char.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(SByte p)           { return      checked((Char)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int16 p)           { return      checked((Char)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int32 p)           { return      checked((Char)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int64 p)           { return      checked((Char)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Byte p)            { return      checked((Char)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt16 p)          { return      checked((Char)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt32 p)          { return      checked((Char)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt64 p)          { return      checked((Char)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Single p)          { return      checked((Char)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Double p)          { return      checked((Char)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Decimal p)         { return      checked((Char)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Boolean p)         { return       (Char)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Char? p)           { return p.HasValue?                p.Value:  (Char)0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(SByte? p)          { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int16? p)          { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int32? p)          { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Int64? p)          { return p.HasValue? checked((Char)p.Value): (Char)0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Byte? p)           { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt16? p)         { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt32? p)         { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Char</c> value.</summary>
		[CLSCompliant(false)]
		public static Char ToChar(UInt64? p)         { return p.HasValue? checked((Char)p.Value): (Char)0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Single? p)         { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Double? p)         { return p.HasValue? checked((Char)p.Value): (Char)0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Decimal? p)        { return p.HasValue? checked((Char)p.Value): (Char)0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Boolean? p)        { return (p.HasValue && p.Value)?   (Char)1: (Char)0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlString p)       { return p.IsNull? (Char)0:        ToChar(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlByte p)         { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlInt16 p)        { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlInt32 p)        { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlInt64 p)        { return p.IsNull? (Char)0:        ToChar(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlSingle p)       { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlDouble p)       { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlDecimal p)      { return p.IsNull? (Char)0:        ToChar(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlMoney p)        { return p.IsNull? (Char)0:        ToChar(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(SqlBoolean p)      { return p.IsNull? (Char)0:        ToChar(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(Byte[] p)          { return p == null || p.Length == 0? (Char)0: BitConverter.ToChar(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Char</c> value.</summary>
		public static Char ToChar(object p)         
		{
			if (p == null || p is DBNull) return '\x0';

			if (p is Char) return (Char)p;

			// Scalar Types.
			//
			if (p is String)          return ToChar((String)p);

			if (p is SByte)           return ToChar((SByte)p);
			if (p is Int16)           return ToChar((Int16)p);
			if (p is Int32)           return ToChar((Int32)p);
			if (p is Int64)           return ToChar((Int64)p);

			if (p is Byte)            return ToChar((Byte)p);
			if (p is UInt16)          return ToChar((UInt16)p);
			if (p is UInt32)          return ToChar((UInt32)p);
			if (p is UInt64)          return ToChar((UInt64)p);

			if (p is Single)          return ToChar((Single)p);
			if (p is Double)          return ToChar((Double)p);

			if (p is Decimal)         return ToChar((Decimal)p);
			if (p is Boolean)         return ToChar((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToChar((SqlString)p);

			if (p is SqlByte)         return ToChar((SqlByte)p);
			if (p is SqlInt16)        return ToChar((SqlInt16)p);
			if (p is SqlInt32)        return ToChar((SqlInt32)p);
			if (p is SqlInt64)        return ToChar((SqlInt64)p);

			if (p is SqlSingle)       return ToChar((SqlSingle)p);
			if (p is SqlDouble)       return ToChar((SqlDouble)p);
			if (p is SqlDecimal)      return ToChar((SqlDecimal)p);
			if (p is SqlMoney)        return ToChar((SqlMoney)p);

			if (p is SqlBoolean)      return ToChar((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToChar((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToChar(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Char));
		}

		#endregion

		#region Single

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(String p)          { return p == null? 0.0f: Single.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(SByte p)           { return      checked((Single)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int16 p)           { return      checked((Single)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int32 p)           { return      checked((Single)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int64 p)           { return      checked((Single)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Byte p)            { return      checked((Single)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt16 p)          { return      checked((Single)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt32 p)          { return      checked((Single)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt64 p)          { return      checked((Single)p); }

		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Double p)          { return      checked((Single)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Decimal p)         { return      checked((Single)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Boolean p)         { return      p?      1.0f: 0.0f; }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Char p)            { return      checked((Single)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Single? p)         { return p.HasValue?                 p.Value:  0.0f; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(SByte? p)          { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int16? p)          { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int32? p)          { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Int64? p)          { return p.HasValue? checked((Single)p.Value): 0.0f; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Byte? p)           { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt16? p)         { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt32? p)         { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Single</c> value.</summary>
		[CLSCompliant(false)]
		public static Single ToSingle(UInt64? p)         { return p.HasValue? checked((Single)p.Value): 0.0f; }

		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Double? p)         { return p.HasValue? checked((Single)p.Value): 0.0f; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Decimal? p)        { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Char? p)           { return p.HasValue? checked((Single)p.Value): 0.0f; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Boolean? p)        { return (p.HasValue && p.Value)?        1.0f: 0.0f; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlSingle p)       { return p.IsNull? 0.0f:                 p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlString p)       { return p.IsNull? 0.0f:        ToSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlByte p)         { return p.IsNull? 0.0f:        ToSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlInt16 p)        { return p.IsNull? 0.0f:        ToSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlInt32 p)        { return p.IsNull? 0.0f:        ToSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlInt64 p)        { return p.IsNull? 0.0f:        ToSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlDouble p)       { return p.IsNull? 0.0f:        ToSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlDecimal p)      { return p.IsNull? 0.0f:        ToSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlMoney p)        { return p.IsNull? 0.0f:        ToSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(SqlBoolean p)      { return p.IsNull? 0.0f:        ToSingle(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(Byte[] p)          { return p == null || p.Length == 0? 0.0f: BitConverter.ToSingle(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Single</c> value.</summary>
		public static Single ToSingle(object p)         
		{
			if (p == null || p is DBNull) return 0.0f;

			if (p is Single) return (Single)p;

			// Scalar Types.
			//
			if (p is String)          return ToSingle((String)p);

			if (p is SByte)           return ToSingle((SByte)p);
			if (p is Int16)           return ToSingle((Int16)p);
			if (p is Int32)           return ToSingle((Int32)p);
			if (p is Int64)           return ToSingle((Int64)p);

			if (p is Byte)            return ToSingle((Byte)p);
			if (p is UInt16)          return ToSingle((UInt16)p);
			if (p is UInt32)          return ToSingle((UInt32)p);
			if (p is UInt64)          return ToSingle((UInt64)p);

			if (p is Double)          return ToSingle((Double)p);

			if (p is Decimal)         return ToSingle((Decimal)p);
			if (p is Boolean)         return ToSingle((Boolean)p);

			// SqlTypes
			//
			if (p is SqlSingle)       return ToSingle((SqlSingle)p);
			if (p is SqlString)       return ToSingle((SqlString)p);

			if (p is SqlByte)         return ToSingle((SqlByte)p);
			if (p is SqlInt16)        return ToSingle((SqlInt16)p);
			if (p is SqlInt32)        return ToSingle((SqlInt32)p);
			if (p is SqlInt64)        return ToSingle((SqlInt64)p);

			if (p is SqlDouble)       return ToSingle((SqlDouble)p);
			if (p is SqlDecimal)      return ToSingle((SqlDecimal)p);
			if (p is SqlMoney)        return ToSingle((SqlMoney)p);

			if (p is SqlBoolean)      return ToSingle((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToSingle((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToSingle(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Single));
		}

		#endregion

		#region Double

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(String p)          { return p == null? 0.0: Double.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(SByte p)           { return      checked((Double)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int16 p)           { return      checked((Double)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int32 p)           { return      checked((Double)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int64 p)           { return      checked((Double)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Byte p)            { return      checked((Double)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt16 p)          { return      checked((Double)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt32 p)          { return      checked((Double)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt64 p)          { return      checked((Double)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Single p)          { return      checked((Double)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Decimal p)         { return      checked((Double)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Boolean p)         { return      p?      1.0: 0.0; }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Char p)            { return      checked((Double)p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(DateTime p)        { return (p - DateTime.MinValue).TotalDays; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(DateTimeOffset p)  { return (p - DateTime.MinValue).TotalDays; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(TimeSpan p)        { return p.TotalDays; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Double? p)         { return p.HasValue?                 p.Value:  0.0; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(SByte? p)          { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int16? p)          { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int32? p)          { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Int64? p)          { return p.HasValue? checked((Double)p.Value):  0.0; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Byte? p)           { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt16? p)         { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt32? p)         { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Double</c> value.</summary>
		[CLSCompliant(false)]
		public static Double ToDouble(UInt64? p)         { return p.HasValue? checked((Double)p.Value):  0.0; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Single? p)         { return p.HasValue? checked((Double)p.Value):  0.0; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Decimal? p)        { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Char? p)           { return p.HasValue? checked((Double)p.Value):  0.0; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Boolean? p)        { return (p.HasValue && p.Value)?         1.0: 0.0; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(DateTime? p)       { return p.HasValue? (p.Value - DateTime.MinValue).TotalDays: 0.0; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(DateTimeOffset? p) { return p.HasValue? (p.Value - DateTime.MinValue).TotalDays: 0.0; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(TimeSpan? p)       { return p.HasValue? p.Value.TotalDays: 0.0; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlDouble p)       { return p.IsNull? 0.0:                 p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlString p)       { return p.IsNull? 0.0:        ToDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlByte p)         { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlInt16 p)        { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlInt32 p)        { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlInt64 p)        { return p.IsNull? 0.0:        ToDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlSingle p)       { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlDecimal p)      { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlMoney p)        { return p.IsNull? 0.0:        ToDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlBoolean p)      { return p.IsNull? 0.0:        ToDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(SqlDateTime p)     { return p.IsNull? 0.0:        ToDouble(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(Byte[] p)          { return p == null || p.Length == 0? 0.0: BitConverter.ToDouble(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Double</c> value.</summary>
		public static Double ToDouble(object p)         
		{
			if (p == null || p is DBNull) return 0.0;

			if (p is Double) return (Double)p;

			// Scalar Types.
			//
			if (p is String)          return ToDouble((String)p);

			if (p is SByte)           return ToDouble((SByte)p);
			if (p is Int16)           return ToDouble((Int16)p);
			if (p is Int32)           return ToDouble((Int32)p);
			if (p is Int64)           return ToDouble((Int64)p);

			if (p is Byte)            return ToDouble((Byte)p);
			if (p is UInt16)          return ToDouble((UInt16)p);
			if (p is UInt32)          return ToDouble((UInt32)p);
			if (p is UInt64)          return ToDouble((UInt64)p);

			if (p is Single)          return ToDouble((Single)p);

			if (p is Decimal)         return ToDouble((Decimal)p);
			if (p is Boolean)         return ToDouble((Boolean)p);
			if (p is DateTime)        return ToDouble((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToDouble((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToDouble((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlDouble)       return ToDouble((SqlDouble)p);
			if (p is SqlString)       return ToDouble((SqlString)p);

			if (p is SqlByte)         return ToDouble((SqlByte)p);
			if (p is SqlInt16)        return ToDouble((SqlInt16)p);
			if (p is SqlInt32)        return ToDouble((SqlInt32)p);
			if (p is SqlInt64)        return ToDouble((SqlInt64)p);

			if (p is SqlSingle)       return ToDouble((SqlSingle)p);
			if (p is SqlDecimal)      return ToDouble((SqlDecimal)p);
			if (p is SqlMoney)        return ToDouble((SqlMoney)p);

			if (p is SqlBoolean)      return ToDouble((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToDouble((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDouble(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Double));
		}

		#endregion

		#region Boolean

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(String p)          { return p == null? false: Boolean.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(SByte p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int16 p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int32 p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int64 p)           { return p != 0; }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Byte p)            { return p != 0; }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt16 p)          { return p != 0; }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt32 p)          { return p != 0; }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt64 p)          { return p != 0; }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Single p)          { return p != 0; }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Double p)          { return p != 0; }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Decimal p)         { return p != 0; }

		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Char p)           
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

			throw CreateInvalidCastException(typeof(Char), typeof(Boolean));
		}

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Boolean? p)        { return p.HasValue? p.Value:      false; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(SByte? p)          { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int16? p)          { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int32? p)          { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Int64? p)          { return p.HasValue? p.Value != 0: false; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Byte? p)           { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt16? p)         { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt32? p)         { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Boolean</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean ToBoolean(UInt64? p)         { return p.HasValue? p.Value != 0: false; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Single? p)         { return p.HasValue? p.Value != 0: false; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Double? p)         { return p.HasValue? p.Value != 0: false; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Decimal? p)        { return p.HasValue? p.Value != 0: false; }

		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Char? p)           { return p.HasValue? ToBoolean(p.Value): false; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlBoolean p)      { return p.IsNull? false:           p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlString p)       { return p.IsNull? false: ToBoolean(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlByte p)         { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlInt16 p)        { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlInt32 p)        { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlInt64 p)        { return p.IsNull? false: ToBoolean(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlSingle p)       { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlDouble p)       { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlDecimal p)      { return p.IsNull? false: ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(SqlMoney p)        { return p.IsNull? false: ToBoolean(p.Value); }


		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(Byte[] p)          { return p == null || p.Length == 0? false: BitConverter.ToBoolean(p, 0); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Boolean</c> value.</summary>
		public static Boolean ToBoolean(object p)         
		{
			if (p == null || p is DBNull) return false;

			if (p is Boolean) return (Boolean)p;

			// Scalar Types.
			//
			if (p is String)          return ToBoolean((String)p);

			if (p is SByte)           return ToBoolean((SByte)p);
			if (p is Int16)           return ToBoolean((Int16)p);
			if (p is Int32)           return ToBoolean((Int32)p);
			if (p is Int64)           return ToBoolean((Int64)p);

			if (p is Byte)            return ToBoolean((Byte)p);
			if (p is UInt16)          return ToBoolean((UInt16)p);
			if (p is UInt32)          return ToBoolean((UInt32)p);
			if (p is UInt64)          return ToBoolean((UInt64)p);

			if (p is Single)          return ToBoolean((Single)p);
			if (p is Double)          return ToBoolean((Double)p);

			if (p is Decimal)         return ToBoolean((Decimal)p);

			if (p is Char)            return ToBoolean((Char)p);

			// SqlTypes
			//
			if (p is SqlBoolean)      return ToBoolean((SqlBoolean)p);
			if (p is SqlString)       return ToBoolean((SqlString)p);

			if (p is SqlByte)         return ToBoolean((SqlByte)p);
			if (p is SqlInt16)        return ToBoolean((SqlInt16)p);
			if (p is SqlInt32)        return ToBoolean((SqlInt32)p);
			if (p is SqlInt64)        return ToBoolean((SqlInt64)p);

			if (p is SqlSingle)       return ToBoolean((SqlSingle)p);
			if (p is SqlDouble)       return ToBoolean((SqlDouble)p);
			if (p is SqlDecimal)      return ToBoolean((SqlDecimal)p);
			if (p is SqlMoney)        return ToBoolean((SqlMoney)p);


			// Other Types
			//
			if (p is Byte[])          return ToBoolean((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToBoolean(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Boolean));
		}

		#endregion

		#region Decimal

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(String p)          { return p == null? 0.0m: Decimal.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(SByte p)           { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int16 p)           { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int32 p)           { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int64 p)           { return      checked((Decimal)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Byte p)            { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt16 p)          { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt32 p)          { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt64 p)          { return      checked((Decimal)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Single p)          { return      checked((Decimal)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Double p)          { return      checked((Decimal)p); }

		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Boolean p)         { return      p?      1.0m: 0.0m; }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Char p)            { return      checked((Decimal)p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Decimal? p)        { return p.HasValue?                  p.Value:  0.0m; }

		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(SByte? p)          { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int16? p)          { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int32? p)          { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Int64? p)          { return p.HasValue? checked((Decimal)p.Value): 0.0m; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Byte? p)           { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt16? p)         { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt32? p)         { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Decimal</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal ToDecimal(UInt64? p)         { return p.HasValue? checked((Decimal)p.Value): 0.0m; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Single? p)         { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Double? p)         { return p.HasValue? checked((Decimal)p.Value): 0.0m; }

		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Char? p)           { return p.HasValue? checked((Decimal)p.Value): 0.0m; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Boolean? p)        { return (p.HasValue && p.Value)?         1.0m: 0.0m; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlDecimal p)      { return p.IsNull? 0.0m:           p.Value;  }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlMoney p)        { return p.IsNull? 0.0m:           p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlString p)       { return p.IsNull? 0.0m: ToDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlByte p)         { return p.IsNull? 0.0m: ToDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlInt16 p)        { return p.IsNull? 0.0m: ToDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlInt32 p)        { return p.IsNull? 0.0m: ToDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlInt64 p)        { return p.IsNull? 0.0m: ToDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlSingle p)       { return p.IsNull? 0.0m: ToDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlDouble p)       { return p.IsNull? 0.0m: ToDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(SqlBoolean p)      { return p.IsNull? 0.0m: ToDecimal(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(Byte[] p)         
		{
					if (p == null || p.Length == 0) return 0.0m;

					int[] bits = new int[p.Length / sizeof(int)];

					Buffer.BlockCopy(p, 0, bits, 0, p.Length);
					return new Decimal(bits);
		}

		/// <summary>Converts the value of a specified object to an equivalent <c>Decimal</c> value.</summary>
		public static Decimal ToDecimal(object p)         
		{
			if (p == null || p is DBNull) return 0.0m;

			if (p is Decimal) return (Decimal)p;

			// Scalar Types.
			//
			if (p is String)          return ToDecimal((String)p);

			if (p is SByte)           return ToDecimal((SByte)p);
			if (p is Int16)           return ToDecimal((Int16)p);
			if (p is Int32)           return ToDecimal((Int32)p);
			if (p is Int64)           return ToDecimal((Int64)p);

			if (p is Byte)            return ToDecimal((Byte)p);
			if (p is UInt16)          return ToDecimal((UInt16)p);
			if (p is UInt32)          return ToDecimal((UInt32)p);
			if (p is UInt64)          return ToDecimal((UInt64)p);

			if (p is Single)          return ToDecimal((Single)p);
			if (p is Double)          return ToDecimal((Double)p);

			if (p is Boolean)         return ToDecimal((Boolean)p);

			// SqlTypes
			//
			if (p is SqlDecimal)      return ToDecimal((SqlDecimal)p);
			if (p is SqlMoney)        return ToDecimal((SqlMoney)p);
			if (p is SqlString)       return ToDecimal((SqlString)p);

			if (p is SqlByte)         return ToDecimal((SqlByte)p);
			if (p is SqlInt16)        return ToDecimal((SqlInt16)p);
			if (p is SqlInt32)        return ToDecimal((SqlInt32)p);
			if (p is SqlInt64)        return ToDecimal((SqlInt64)p);

			if (p is SqlSingle)       return ToDecimal((SqlSingle)p);
			if (p is SqlDouble)       return ToDecimal((SqlDouble)p);

			if (p is SqlBoolean)      return ToDecimal((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToDecimal((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDecimal(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Decimal));
		}

		#endregion

		#region DateTime

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(String p)          { return p == null? DateTime.MinValue: DateTime.Parse(p); }
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(TimeSpan p)        { return DateTime.MinValue + p; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(Int64 p)           { return DateTime.MinValue + TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(Double p)          { return DateTime.MinValue + TimeSpan.FromDays(p); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(DateTimeOffset p)  { return p.LocalDateTime; }
		#endif

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(DateTime? p)       { return p.HasValue?                                               p.Value:  DateTime.MinValue; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(DateTimeOffset? p) { return p.HasValue?                                   p.Value.LocalDateTime:  DateTime.MinValue; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(TimeSpan? p)       { return p.HasValue? DateTime.MinValue +                           p.Value:  DateTime.MinValue; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(Int64? p)          { return p.HasValue? DateTime.MinValue +        TimeSpan.FromTicks(p.Value): DateTime.MinValue; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(Double? p)         { return p.HasValue? DateTime.MinValue + TimeSpan.FromDays(p.Value): DateTime.MinValue; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(SqlDateTime p)     { return p.IsNull? DateTime.MinValue:                                               p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(SqlString p)       { return p.IsNull? DateTime.MinValue:                                    ToDateTime(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(SqlInt64 p)        { return p.IsNull? DateTime.MinValue: DateTime.MinValue +        TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(SqlDouble p)       { return p.IsNull? DateTime.MinValue: DateTime.MinValue + TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(Byte[] p)          { return p == null || p.Length == 0? DateTime.MinValue: DateTime.FromBinary(ToInt64(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>DateTime</c> value.</summary>
		public static DateTime ToDateTime(object p)         
		{
			if (p == null || p is DBNull) return DateTime.MinValue;

			if (p is DateTime) return (DateTime)p;

			// Scalar Types.
			//
			if (p is String)          return ToDateTime((String)p);
			if (p is TimeSpan)        return ToDateTime((TimeSpan)p);
			if (p is Int64)           return ToDateTime((Int64)p);
			if (p is Double)          return ToDateTime((Double)p);
			#if FW3
			if (p is DateTimeOffset)  return ToDateTime((DateTimeOffset)p);
			#endif

			// SqlTypes
			//
			if (p is SqlDateTime)     return ToDateTime((SqlDateTime)p);
			if (p is SqlString)       return ToDateTime((SqlString)p);
			if (p is SqlInt64)        return ToDateTime((SqlInt64)p);
			if (p is SqlDouble)       return ToDateTime((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToDateTime((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDateTime(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(DateTime));
		}

		#endregion

		#if FW3

		#region DateTimeOffset

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(String p)          { return p == null? DateTimeOffset.MinValue: DateTimeOffset.Parse(p); }
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(TimeSpan p)        { return DateTimeOffset.MinValue + p; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(Int64 p)           { return DateTimeOffset.MinValue + TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(Double p)          { return DateTimeOffset.MinValue + TimeSpan.FromDays(p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(DateTime p)        { return p; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(DateTimeOffset? p) { return p.HasValue?                                         p.Value:  DateTimeOffset.MinValue; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(DateTime? p)       { return p.HasValue?                                               p.Value:  DateTimeOffset.MinValue; }
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(TimeSpan? p)       { return p.HasValue? DateTimeOffset.MinValue +                           p.Value:  DateTimeOffset.MinValue; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(Int64? p)          { return p.HasValue? DateTimeOffset.MinValue +        TimeSpan.FromTicks(p.Value): DateTimeOffset.MinValue; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(Double? p)         { return p.HasValue? DateTimeOffset.MinValue + TimeSpan.FromDays(p.Value): DateTimeOffset.MinValue; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(SqlDateTime p)     { return p.IsNull? DateTimeOffset.MinValue:                                               p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(SqlString p)       { return p.IsNull? DateTimeOffset.MinValue:                                    ToDateTimeOffset(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(SqlInt64 p)        { return p.IsNull? DateTimeOffset.MinValue: DateTimeOffset.MinValue +        TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(SqlDouble p)       { return p.IsNull? DateTimeOffset.MinValue: DateTimeOffset.MinValue + TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(Byte[] p)          { return p == null || p.Length == 0? DateTimeOffset.MinValue: new DateTimeOffset(ToDateTime(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>DateTimeOffset</c> value.</summary>
		public static DateTimeOffset ToDateTimeOffset(object p)         
		{
			if (p == null || p is DBNull) return DateTimeOffset.MinValue;

			if (p is DateTimeOffset) return (DateTimeOffset)p;

			// Scalar Types.
			//
			if (p is String)          return ToDateTimeOffset((String)p);
			if (p is TimeSpan)        return ToDateTimeOffset((TimeSpan)p);
			if (p is Int64)           return ToDateTimeOffset((Int64)p);
			if (p is Double)          return ToDateTimeOffset((Double)p);
			if (p is DateTime)        return ToDateTimeOffset((DateTime)p);

			// SqlTypes
			//
			if (p is SqlDateTime)     return ToDateTimeOffset((SqlDateTime)p);
			if (p is SqlString)       return ToDateTimeOffset((SqlString)p);
			if (p is SqlInt64)        return ToDateTimeOffset((SqlInt64)p);
			if (p is SqlDouble)       return ToDateTimeOffset((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToDateTimeOffset((Byte[])p);

			if (p is IConvertible) return ToDateTimeOffset(((IConvertible)p).ToDateTime(null));
			
			throw CreateInvalidCastException(p.GetType(), typeof(DateTimeOffset));
		}

		#endregion

		#endif

		#region TimeSpan

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(String p)          { return p == null? TimeSpan.MinValue: TimeSpan.Parse(p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(DateTime p)        { return p - DateTime.MinValue; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(DateTimeOffset p)  { return p - DateTimeOffset.MinValue; }
		#endif
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(Int64 p)           { return TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(Double p)          { return TimeSpan.FromDays(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(TimeSpan? p)       { return p.HasValue? p.Value:                            TimeSpan.MinValue; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(DateTime? p)       { return p.HasValue? p.Value - DateTime.MinValue:        TimeSpan.MinValue; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(DateTimeOffset? p) { return p.HasValue? p.Value - DateTimeOffset.MinValue:        TimeSpan.MinValue; }
		#endif
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(Int64? p)          { return p.HasValue? TimeSpan.FromTicks(p.Value):        TimeSpan.MinValue; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(Double? p)         { return p.HasValue? TimeSpan.FromDays(p.Value): TimeSpan.MinValue; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(SqlString p)       { return p.IsNull? TimeSpan.MinValue: TimeSpan.Parse(p.Value);     }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(SqlDateTime p)     { return p.IsNull? TimeSpan.MinValue: p.Value - DateTime.MinValue; }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(SqlInt64 p)        { return p.IsNull? TimeSpan.MinValue: TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(SqlDouble p)       { return p.IsNull? TimeSpan.MinValue: TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(Byte[] p)          { return p == null || p.Length == 0? TimeSpan.MinValue: TimeSpan.FromTicks(ToInt64(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>TimeSpan</c> value.</summary>
		public static TimeSpan ToTimeSpan(object p)         
		{
			if (p == null || p is DBNull) return TimeSpan.MinValue;

			if (p is TimeSpan) return (TimeSpan)p;

			// Scalar Types.
			//
			if (p is String)          return ToTimeSpan((String)p);
			if (p is DateTime)        return ToTimeSpan((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToTimeSpan((DateTimeOffset)p);
			#endif
			if (p is Int64)           return ToTimeSpan((Int64)p);
			if (p is Double)          return ToTimeSpan((Double)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToTimeSpan((SqlString)p);
			if (p is SqlDateTime)     return ToTimeSpan((SqlDateTime)p);
			if (p is SqlInt64)        return ToTimeSpan((SqlInt64)p);
			if (p is SqlDouble)       return ToTimeSpan((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToTimeSpan((Byte[])p);

			throw CreateInvalidCastException(p.GetType(), typeof(TimeSpan));
		}

		#endregion

		#region Guid

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(String p)          { return p == null? Guid.Empty: new Guid(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(Guid? p)           { return p.HasValue? p.Value : Guid.Empty; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(SqlGuid p)         { return p.IsNull? Guid.Empty: p.Value;             }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(SqlString p)       { return p.IsNull? Guid.Empty: new Guid(p.Value);   }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(SqlBinary p)       { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; }

		// Other Types.
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(Byte[] p)          { return p == null? Guid.Empty: new Guid(p); }
		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(Type p)            { return p == null? Guid.Empty: p.GUID; }

		/// <summary>Converts the value of a specified object to an equivalent <c>Guid</c> value.</summary>
		public static Guid ToGuid(object p)         
		{
			if (p == null || p is DBNull) return Guid.Empty;

			if (p is Guid) return (Guid)p;

			// Scalar Types.
			//
			if (p is String)          return ToGuid((String)p);

			// SqlTypes
			//
			if (p is SqlGuid)         return ToGuid((SqlGuid)p);
			if (p is SqlString)       return ToGuid((SqlString)p);
			if (p is SqlBinary)       return ToGuid((SqlBinary)p);

			// Other Types.
			//
			if (p is Byte[])          return ToGuid((Byte[])p);
			if (p is Type)            return ToGuid((Type)p);

			throw CreateInvalidCastException(p.GetType(), typeof(Guid));
		}

		#endregion

		#endregion

		#region Nullable Types

		#region SByte?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SByte p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(String p)          { return p == null? null: (SByte?)SByte.Parse(p); }

		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int16 p)           { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int32 p)           { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int64 p)           { return      checked((SByte?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Byte p)            { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt16 p)          { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt32 p)          { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt64 p)          { return      checked((SByte?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Single p)          { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Double p)          { return      checked((SByte?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Decimal p)         { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Char p)            { return      checked((SByte?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Boolean p)         { return       (SByte?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int16? p)          { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int32? p)          { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Int64? p)          { return p.HasValue? checked((SByte?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Byte? p)           { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt16? p)         { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt32? p)         { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(UInt64? p)         { return p.HasValue? checked((SByte?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Single? p)         { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Double? p)         { return p.HasValue? checked((SByte?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Decimal? p)        { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Char? p)           { return p.HasValue? checked((SByte?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Boolean? p)        { return p.HasValue? (SByte?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlString p)       { return p.IsNull? null: ToNullableSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlByte p)         { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlInt16 p)        { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlInt32 p)        { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlInt64 p)        { return p.IsNull? null: ToNullableSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlSingle p)       { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlDouble p)       { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlDecimal p)      { return p.IsNull? null: ToNullableSByte(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlMoney p)        { return p.IsNull? null: ToNullableSByte(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(SqlBoolean p)      { return p.IsNull? null: ToNullableSByte(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(Byte[] p)          { return p == null || p.Length == 0? null: (SByte?)ToSByte(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SByte?</c> value.</summary>
		[CLSCompliant(false)]
		public static SByte? ToNullableSByte(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is SByte) return (SByte)p;

			// Scalar Types.
			//
			if (p is SByte)           return ToNullableSByte((SByte)p);
			if (p is String)          return ToNullableSByte((String)p);

			if (p is Int16)           return ToNullableSByte((Int16)p);
			if (p is Int32)           return ToNullableSByte((Int32)p);
			if (p is Int64)           return ToNullableSByte((Int64)p);

			if (p is Byte)            return ToNullableSByte((Byte)p);
			if (p is UInt16)          return ToNullableSByte((UInt16)p);
			if (p is UInt32)          return ToNullableSByte((UInt32)p);
			if (p is UInt64)          return ToNullableSByte((UInt64)p);

			if (p is Single)          return ToNullableSByte((Single)p);
			if (p is Double)          return ToNullableSByte((Double)p);

			if (p is Decimal)         return ToNullableSByte((Decimal)p);
			if (p is Boolean)         return ToNullableSByte((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableSByte((SqlString)p);

			if (p is SqlByte)         return ToNullableSByte((SqlByte)p);
			if (p is SqlInt16)        return ToNullableSByte((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableSByte((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableSByte((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableSByte((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableSByte((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableSByte((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableSByte((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableSByte((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableSByte((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToSByte(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(SByte?));
		}

		#endregion

		#region Int16?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Int16 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(String p)          { return p == null? null: (Int16?)Int16.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(SByte p)           { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Int32 p)           { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Int64 p)           { return      checked((Int16?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Byte p)            { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt16 p)          { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt32 p)          { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt64 p)          { return      checked((Int16?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Single p)          { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Double p)          { return      checked((Int16?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Decimal p)         { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Char p)            { return      checked((Int16?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Boolean p)         { return       (Int16?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(SByte? p)          { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Int32? p)          { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Int64? p)          { return p.HasValue? checked((Int16?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Byte? p)           { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt16? p)         { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt32? p)         { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int16?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int16? ToNullableInt16(UInt64? p)         { return p.HasValue? checked((Int16?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Single? p)         { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Double? p)         { return p.HasValue? checked((Int16?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Decimal? p)        { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Char? p)           { return p.HasValue? checked((Int16?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Boolean? p)        { return p.HasValue? (Int16?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlInt16 p)        { return p.IsNull? null:         (Int16?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlString p)       { return p.IsNull? null: ToNullableInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlByte p)         { return p.IsNull? null: ToNullableInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlInt32 p)        { return p.IsNull? null: ToNullableInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlInt64 p)        { return p.IsNull? null: ToNullableInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlSingle p)       { return p.IsNull? null: ToNullableInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlDouble p)       { return p.IsNull? null: ToNullableInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlDecimal p)      { return p.IsNull? null: ToNullableInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlMoney p)        { return p.IsNull? null: ToNullableInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(SqlBoolean p)      { return p.IsNull? null: ToNullableInt16(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(Byte[] p)          { return p == null || p.Length == 0? null: (Int16?)ToInt16(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int16?</c> value.</summary>
		public static Int16? ToNullableInt16(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Int16) return (Int16)p;

			// Scalar Types.
			//
			if (p is Int16)           return ToNullableInt16((Int16)p);
			if (p is String)          return ToNullableInt16((String)p);

			if (p is SByte)           return ToNullableInt16((SByte)p);
			if (p is Int32)           return ToNullableInt16((Int32)p);
			if (p is Int64)           return ToNullableInt16((Int64)p);

			if (p is Byte)            return ToNullableInt16((Byte)p);
			if (p is UInt16)          return ToNullableInt16((UInt16)p);
			if (p is UInt32)          return ToNullableInt16((UInt32)p);
			if (p is UInt64)          return ToNullableInt16((UInt64)p);

			if (p is Single)          return ToNullableInt16((Single)p);
			if (p is Double)          return ToNullableInt16((Double)p);

			if (p is Decimal)         return ToNullableInt16((Decimal)p);
			if (p is Boolean)         return ToNullableInt16((Boolean)p);

			// SqlTypes
			//
			if (p is SqlInt16)        return ToNullableInt16((SqlInt16)p);
			if (p is SqlString)       return ToNullableInt16((SqlString)p);

			if (p is SqlByte)         return ToNullableInt16((SqlByte)p);
			if (p is SqlInt32)        return ToNullableInt16((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableInt16((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableInt16((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableInt16((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableInt16((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableInt16((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableInt16((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableInt16((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt16(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int16?));
		}

		#endregion

		#region Int32?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Int32 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(String p)          { return p == null? null: (Int32?)Int32.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(SByte p)           { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Int16 p)           { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Int64 p)           { return checked((Int32?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Byte p)            { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt16 p)          { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt32 p)          { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt64 p)          { return checked((Int32?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Single p)          { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Double p)          { return checked((Int32?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Decimal p)         { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Char p)            { return checked((Int32?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Boolean p)         { return p? 1: 0; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(SByte? p)          { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Int16? p)          { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Int64? p)          { return p.HasValue? checked((Int32?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Byte? p)           { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt16? p)         { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt32? p)         { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int32?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int32? ToNullableInt32(UInt64? p)         { return p.HasValue? checked((Int32?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Single? p)         { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Double? p)         { return p.HasValue? checked((Int32?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Decimal? p)        { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Char? p)           { return p.HasValue? checked((Int32?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Boolean? p)        { return p.HasValue? (Int32?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlInt32 p)        { return p.IsNull? null:         (Int32?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlString p)       { return p.IsNull? null: ToNullableInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlByte p)         { return p.IsNull? null: ToNullableInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlInt16 p)        { return p.IsNull? null: ToNullableInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlInt64 p)        { return p.IsNull? null: ToNullableInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlSingle p)       { return p.IsNull? null: ToNullableInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlDouble p)       { return p.IsNull? null: ToNullableInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlDecimal p)      { return p.IsNull? null: ToNullableInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlMoney p)        { return p.IsNull? null: ToNullableInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(SqlBoolean p)      { return p.IsNull? null: ToNullableInt32(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(Byte[] p)          { return p == null || p.Length == 0? null: (Int32?)ToInt32(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int32?</c> value.</summary>
		public static Int32? ToNullableInt32(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Int32) return (Int32)p;

			// Scalar Types.
			//
			if (p is Int32)           return ToNullableInt32((Int32)p);
			if (p is String)          return ToNullableInt32((String)p);

			if (p is SByte)           return ToNullableInt32((SByte)p);
			if (p is Int16)           return ToNullableInt32((Int16)p);
			if (p is Int64)           return ToNullableInt32((Int64)p);

			if (p is Byte)            return ToNullableInt32((Byte)p);
			if (p is UInt16)          return ToNullableInt32((UInt16)p);
			if (p is UInt32)          return ToNullableInt32((UInt32)p);
			if (p is UInt64)          return ToNullableInt32((UInt64)p);

			if (p is Single)          return ToNullableInt32((Single)p);
			if (p is Double)          return ToNullableInt32((Double)p);

			if (p is Decimal)         return ToNullableInt32((Decimal)p);
			if (p is Boolean)         return ToNullableInt32((Boolean)p);

			// SqlTypes
			//
			if (p is SqlInt32)        return ToNullableInt32((SqlInt32)p);
			if (p is SqlString)       return ToNullableInt32((SqlString)p);

			if (p is SqlByte)         return ToNullableInt32((SqlByte)p);
			if (p is SqlInt16)        return ToNullableInt32((SqlInt16)p);
			if (p is SqlInt64)        return ToNullableInt32((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableInt32((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableInt32((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableInt32((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableInt32((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableInt32((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableInt32((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt32(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int32?));
		}

		#endregion

		#region Int64?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Int64 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(String p)          { return p == null? null: (Int64?)Int64.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(SByte p)           { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Int16 p)           { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Int32 p)           { return checked((Int64?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Byte p)            { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt16 p)          { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt32 p)          { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt64 p)          { return checked((Int64?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Single p)          { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Double p)          { return checked((Int64?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Decimal p)         { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Char p)            { return checked((Int64?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Boolean p)         { return p? 1: 0; }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(DateTime p)        { return (p - DateTime.MinValue).Ticks; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(DateTimeOffset p)  { return (p - DateTimeOffset.MinValue).Ticks; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(TimeSpan p)        { return p.Ticks; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(SByte? p)          { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Int16? p)          { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Int32? p)          { return p.HasValue? checked((Int64?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Byte? p)           { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt16? p)         { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt32? p)         { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Int64?</c> value.</summary>
		[CLSCompliant(false)]
		public static Int64? ToNullableInt64(UInt64? p)         { return p.HasValue? checked((Int64?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Single? p)         { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Double? p)         { return p.HasValue? checked((Int64?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Decimal? p)        { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Char? p)           { return p.HasValue? checked((Int64?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Boolean? p)        { return p.HasValue? (Int64?)(p.Value? 1: 0):  null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(DateTime? p)       { return p.HasValue? (Int64?)(p.Value - DateTime.MinValue).Ticks: null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(DateTimeOffset? p) { return p.HasValue? (Int64?)(p.Value - DateTimeOffset.MinValue).Ticks: null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(TimeSpan? p)       { return p.HasValue? (Int64?)p.Value.Ticks: null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlInt64 p)        { return p.IsNull? null:         (Int64?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlString p)       { return p.IsNull? null: ToNullableInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlByte p)         { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlInt16 p)        { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlInt32 p)        { return p.IsNull? null: ToNullableInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlSingle p)       { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlDouble p)       { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlDecimal p)      { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlMoney p)        { return p.IsNull? null: ToNullableInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlBoolean p)      { return p.IsNull? null: ToNullableInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(SqlDateTime p)     { return p.IsNull? null: ToNullableInt64(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(Byte[] p)          { return p == null || p.Length == 0? null: (Int64?)ToInt64(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Int64?</c> value.</summary>
		public static Int64? ToNullableInt64(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Int64) return (Int64)p;

			// Scalar Types.
			//
			if (p is Int64)           return ToNullableInt64((Int64)p);
			if (p is String)          return ToNullableInt64((String)p);

			if (p is SByte)           return ToNullableInt64((SByte)p);
			if (p is Int16)           return ToNullableInt64((Int16)p);
			if (p is Int32)           return ToNullableInt64((Int32)p);

			if (p is Byte)            return ToNullableInt64((Byte)p);
			if (p is UInt16)          return ToNullableInt64((UInt16)p);
			if (p is UInt32)          return ToNullableInt64((UInt32)p);
			if (p is UInt64)          return ToNullableInt64((UInt64)p);

			if (p is Single)          return ToNullableInt64((Single)p);
			if (p is Double)          return ToNullableInt64((Double)p);

			if (p is Decimal)         return ToNullableInt64((Decimal)p);
			if (p is Boolean)         return ToNullableInt64((Boolean)p);
			if (p is DateTime)        return ToNullableInt64((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToNullableInt64((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToNullableInt64((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlInt64)        return ToNullableInt64((SqlInt64)p);
			if (p is SqlString)       return ToNullableInt64((SqlString)p);

			if (p is SqlByte)         return ToNullableInt64((SqlByte)p);
			if (p is SqlInt16)        return ToNullableInt64((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableInt64((SqlInt32)p);

			if (p is SqlSingle)       return ToNullableInt64((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableInt64((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableInt64((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableInt64((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableInt64((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableInt64((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToInt64(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Int64?));
		}

		#endregion

		#region Byte?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Byte p)            { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(String p)          { return p == null? null: (Byte?)Byte.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(SByte p)           { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int16 p)           { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int32 p)           { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int64 p)           { return      checked((Byte?)p); }

		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt16 p)          { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt32 p)          { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt64 p)          { return      checked((Byte?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Single p)          { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Double p)          { return      checked((Byte?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Decimal p)         { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Char p)            { return      checked((Byte?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Boolean p)         { return       (Byte?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(SByte? p)          { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int16? p)          { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int32? p)          { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Int64? p)          { return p.HasValue? checked((Byte?)p.Value): null; }

		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt16? p)         { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt32? p)         { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Byte?</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte? ToNullableByte(UInt64? p)         { return p.HasValue? checked((Byte?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Single? p)         { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Double? p)         { return p.HasValue? checked((Byte?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Decimal? p)        { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Char? p)           { return p.HasValue? checked((Byte?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Boolean? p)        { return p.HasValue? (Byte?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlByte p)         { return p.IsNull? null:         (Byte?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlString p)       { return p.IsNull? null: ToNullableByte(p.Value); }

		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlInt16 p)        { return p.IsNull? null: ToNullableByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlInt32 p)        { return p.IsNull? null: ToNullableByte(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlInt64 p)        { return p.IsNull? null: ToNullableByte(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlSingle p)       { return p.IsNull? null: ToNullableByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlDouble p)       { return p.IsNull? null: ToNullableByte(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlDecimal p)      { return p.IsNull? null: ToNullableByte(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlMoney p)        { return p.IsNull? null: ToNullableByte(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(SqlBoolean p)      { return p.IsNull? null: ToNullableByte(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(Byte[] p)          { return p == null || p.Length == 0? null: (Byte?)ToByte(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Byte?</c> value.</summary>
		public static Byte? ToNullableByte(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Byte) return (Byte)p;

			// Scalar Types.
			//
			if (p is Byte)            return ToNullableByte((Byte)p);
			if (p is String)          return ToNullableByte((String)p);

			if (p is SByte)           return ToNullableByte((SByte)p);
			if (p is Int16)           return ToNullableByte((Int16)p);
			if (p is Int32)           return ToNullableByte((Int32)p);
			if (p is Int64)           return ToNullableByte((Int64)p);

			if (p is UInt16)          return ToNullableByte((UInt16)p);
			if (p is UInt32)          return ToNullableByte((UInt32)p);
			if (p is UInt64)          return ToNullableByte((UInt64)p);

			if (p is Single)          return ToNullableByte((Single)p);
			if (p is Double)          return ToNullableByte((Double)p);

			if (p is Decimal)         return ToNullableByte((Decimal)p);
			if (p is Boolean)         return ToNullableByte((Boolean)p);

			// SqlTypes
			//
			if (p is SqlByte)         return ToNullableByte((SqlByte)p);
			if (p is SqlString)       return ToNullableByte((SqlString)p);

			if (p is SqlInt16)        return ToNullableByte((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableByte((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableByte((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableByte((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableByte((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableByte((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableByte((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableByte((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableByte((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToByte(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Byte?));
		}

		#endregion

		#region UInt16?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(UInt16 p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(String p)          { return p == null? null: (UInt16?)UInt16.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SByte p)           { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int16 p)           { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int32 p)           { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int64 p)           { return      checked((UInt16?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Byte p)            { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(UInt32 p)          { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(UInt64 p)          { return      checked((UInt16?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Single p)          { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Double p)          { return      checked((UInt16?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Decimal p)         { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Char p)            { return      checked((UInt16?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Boolean p)         { return       (UInt16?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SByte? p)          { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int16? p)          { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int32? p)          { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Int64? p)          { return p.HasValue? checked((UInt16?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Byte? p)           { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(UInt32? p)         { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(UInt64? p)         { return p.HasValue? checked((UInt16?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Single? p)         { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Double? p)         { return p.HasValue? checked((UInt16?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Decimal? p)        { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Char? p)           { return p.HasValue? checked((UInt16?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Boolean? p)        { return p.HasValue? (UInt16?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlString p)       { return p.IsNull? null: ToNullableUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlByte p)         { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlInt16 p)        { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlInt32 p)        { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlInt64 p)        { return p.IsNull? null: ToNullableUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlSingle p)       { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlDouble p)       { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlDecimal p)      { return p.IsNull? null: ToNullableUInt16(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlMoney p)        { return p.IsNull? null: ToNullableUInt16(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(SqlBoolean p)      { return p.IsNull? null: ToNullableUInt16(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(Byte[] p)          { return p == null || p.Length == 0? null: (UInt16?)ToUInt16(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt16?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt16? ToNullableUInt16(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is UInt16) return (UInt16)p;

			// Scalar Types.
			//
			if (p is UInt16)          return ToNullableUInt16((UInt16)p);
			if (p is String)          return ToNullableUInt16((String)p);

			if (p is SByte)           return ToNullableUInt16((SByte)p);
			if (p is Int16)           return ToNullableUInt16((Int16)p);
			if (p is Int32)           return ToNullableUInt16((Int32)p);
			if (p is Int64)           return ToNullableUInt16((Int64)p);

			if (p is Byte)            return ToNullableUInt16((Byte)p);
			if (p is UInt32)          return ToNullableUInt16((UInt32)p);
			if (p is UInt64)          return ToNullableUInt16((UInt64)p);

			if (p is Single)          return ToNullableUInt16((Single)p);
			if (p is Double)          return ToNullableUInt16((Double)p);

			if (p is Decimal)         return ToNullableUInt16((Decimal)p);
			if (p is Boolean)         return ToNullableUInt16((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableUInt16((SqlString)p);

			if (p is SqlByte)         return ToNullableUInt16((SqlByte)p);
			if (p is SqlInt16)        return ToNullableUInt16((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableUInt16((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableUInt16((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableUInt16((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableUInt16((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableUInt16((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableUInt16((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableUInt16((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableUInt16((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt16(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt16?));
		}

		#endregion

		#region UInt32?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(UInt32 p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(String p)          { return p == null? null: (UInt32?)UInt32.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SByte p)           { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int16 p)           { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int32 p)           { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int64 p)           { return      checked((UInt32?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Byte p)            { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(UInt16 p)          { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(UInt64 p)          { return      checked((UInt32?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Single p)          { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Double p)          { return      checked((UInt32?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Decimal p)         { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Char p)            { return      checked((UInt32?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Boolean p)         { return       (UInt32?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SByte? p)          { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int16? p)          { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int32? p)          { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Int64? p)          { return p.HasValue? checked((UInt32?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Byte? p)           { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(UInt16? p)         { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(UInt64? p)         { return p.HasValue? checked((UInt32?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Single? p)         { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Double? p)         { return p.HasValue? checked((UInt32?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Decimal? p)        { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Char? p)           { return p.HasValue? checked((UInt32?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Boolean? p)        { return p.HasValue? (UInt32?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlString p)       { return p.IsNull? null: ToNullableUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlByte p)         { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlInt16 p)        { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlInt32 p)        { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlInt64 p)        { return p.IsNull? null: ToNullableUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlSingle p)       { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlDouble p)       { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlDecimal p)      { return p.IsNull? null: ToNullableUInt32(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlMoney p)        { return p.IsNull? null: ToNullableUInt32(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(SqlBoolean p)      { return p.IsNull? null: ToNullableUInt32(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(Byte[] p)          { return p == null || p.Length == 0? null: (UInt32?)ToUInt32(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt32?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt32? ToNullableUInt32(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is UInt32) return (UInt32)p;

			// Scalar Types.
			//
			if (p is UInt32)          return ToNullableUInt32((UInt32)p);
			if (p is String)          return ToNullableUInt32((String)p);

			if (p is SByte)           return ToNullableUInt32((SByte)p);
			if (p is Int16)           return ToNullableUInt32((Int16)p);
			if (p is Int32)           return ToNullableUInt32((Int32)p);
			if (p is Int64)           return ToNullableUInt32((Int64)p);

			if (p is Byte)            return ToNullableUInt32((Byte)p);
			if (p is UInt16)          return ToNullableUInt32((UInt16)p);
			if (p is UInt64)          return ToNullableUInt32((UInt64)p);

			if (p is Single)          return ToNullableUInt32((Single)p);
			if (p is Double)          return ToNullableUInt32((Double)p);

			if (p is Decimal)         return ToNullableUInt32((Decimal)p);
			if (p is Boolean)         return ToNullableUInt32((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableUInt32((SqlString)p);

			if (p is SqlByte)         return ToNullableUInt32((SqlByte)p);
			if (p is SqlInt16)        return ToNullableUInt32((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableUInt32((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableUInt32((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableUInt32((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableUInt32((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableUInt32((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableUInt32((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableUInt32((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableUInt32((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt32(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt32?));
		}

		#endregion

		#region UInt64?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(UInt64 p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(String p)          { return p == null? null: (UInt64?)UInt64.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SByte p)           { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int16 p)           { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int32 p)           { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int64 p)           { return checked((UInt64?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Byte p)            { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(UInt16 p)          { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(UInt32 p)          { return checked((UInt64?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Single p)          { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Double p)          { return checked((UInt64?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Decimal p)         { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Char p)            { return checked((UInt64?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Boolean p)         { return (UInt64?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SByte? p)          { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int16? p)          { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int32? p)          { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Int64? p)          { return p.HasValue? checked((UInt64?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Byte? p)           { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(UInt16? p)         { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(UInt32? p)         { return p.HasValue? checked((UInt64?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Single? p)         { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Double? p)         { return p.HasValue? checked((UInt64?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Decimal? p)        { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Char? p)           { return p.HasValue? checked((UInt64?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Boolean? p)        { return p.HasValue? (UInt64?)(p.Value? 1: 0):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlString p)       { return p.IsNull? null: ToNullableUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlByte p)         { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlInt16 p)        { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlInt32 p)        { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlInt64 p)        { return p.IsNull? null: ToNullableUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlSingle p)       { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlDouble p)       { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlDecimal p)      { return p.IsNull? null: ToNullableUInt64(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlMoney p)        { return p.IsNull? null: ToNullableUInt64(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(SqlBoolean p)      { return p.IsNull? null: ToNullableUInt64(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(Byte[] p)          { return p == null || p.Length == 0? null: (UInt64?)ToUInt64(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>UInt64?</c> value.</summary>
		[CLSCompliant(false)]
		public static UInt64? ToNullableUInt64(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is UInt64) return (UInt64)p;

			// Scalar Types.
			//
			if (p is UInt64)          return ToNullableUInt64((UInt64)p);
			if (p is String)          return ToNullableUInt64((String)p);

			if (p is SByte)           return ToNullableUInt64((SByte)p);
			if (p is Int16)           return ToNullableUInt64((Int16)p);
			if (p is Int32)           return ToNullableUInt64((Int32)p);
			if (p is Int64)           return ToNullableUInt64((Int64)p);

			if (p is Byte)            return ToNullableUInt64((Byte)p);
			if (p is UInt16)          return ToNullableUInt64((UInt16)p);
			if (p is UInt32)          return ToNullableUInt64((UInt32)p);

			if (p is Single)          return ToNullableUInt64((Single)p);
			if (p is Double)          return ToNullableUInt64((Double)p);

			if (p is Decimal)         return ToNullableUInt64((Decimal)p);
			if (p is Boolean)         return ToNullableUInt64((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableUInt64((SqlString)p);

			if (p is SqlByte)         return ToNullableUInt64((SqlByte)p);
			if (p is SqlInt16)        return ToNullableUInt64((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableUInt64((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableUInt64((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableUInt64((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableUInt64((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableUInt64((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableUInt64((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableUInt64((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableUInt64((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToUInt64(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(UInt64?));
		}

		#endregion

		#region Char?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Char p)            { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(String p)          { return p == null? null: (Char?)Char.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(SByte p)           { return checked((Char?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int16 p)           { return checked((Char?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int32 p)           { return checked((Char?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int64 p)           { return checked((Char?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Byte p)            { return checked((Char?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt16 p)          { return checked((Char?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt32 p)          { return checked((Char?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt64 p)          { return checked((Char?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Single p)          { return checked((Char?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Double p)          { return checked((Char?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Decimal p)         { return checked((Char?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Boolean p)         { return (Char?)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(SByte? p)          { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int16? p)          { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int32? p)          { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Int64? p)          { return p.HasValue? checked((Char?)p.Value) : null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Byte? p)           { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt16? p)         { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt32? p)         { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Char?</c> value.</summary>
		[CLSCompliant(false)]
		public static Char? ToNullableChar(UInt64? p)         { return p.HasValue? checked((Char?)p.Value) : null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Single? p)         { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Double? p)         { return p.HasValue? checked((Char?)p.Value) : null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Decimal? p)        { return p.HasValue? checked((Char?)p.Value) : null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Boolean? p)        { return p.HasValue? (Char?)(p.Value? 1: 0)  : null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlString p)       { return p.IsNull? null: ToNullableChar(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlByte p)         { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlInt16 p)        { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlInt32 p)        { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlInt64 p)        { return p.IsNull? null: ToNullableChar(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlSingle p)       { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlDouble p)       { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlDecimal p)      { return p.IsNull? null: ToNullableChar(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlMoney p)        { return p.IsNull? null: ToNullableChar(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(SqlBoolean p)      { return p.IsNull? null: ToNullableChar(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(Byte[] p)          { return p == null || p.Length == 0? null: (Char?)ToChar(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Char?</c> value.</summary>
		public static Char? ToNullableChar(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Char) return (Char)p;

			// Scalar Types.
			//
			if (p is Char)            return ToNullableChar((Char)p);
			if (p is String)          return ToNullableChar((String)p);

			if (p is SByte)           return ToNullableChar((SByte)p);
			if (p is Int16)           return ToNullableChar((Int16)p);
			if (p is Int32)           return ToNullableChar((Int32)p);
			if (p is Int64)           return ToNullableChar((Int64)p);

			if (p is Byte)            return ToNullableChar((Byte)p);
			if (p is UInt16)          return ToNullableChar((UInt16)p);
			if (p is UInt32)          return ToNullableChar((UInt32)p);
			if (p is UInt64)          return ToNullableChar((UInt64)p);

			if (p is Single)          return ToNullableChar((Single)p);
			if (p is Double)          return ToNullableChar((Double)p);

			if (p is Decimal)         return ToNullableChar((Decimal)p);
			if (p is Boolean)         return ToNullableChar((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableChar((SqlString)p);

			if (p is SqlByte)         return ToNullableChar((SqlByte)p);
			if (p is SqlInt16)        return ToNullableChar((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableChar((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableChar((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableChar((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableChar((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableChar((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableChar((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableChar((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableChar((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToChar(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Char?));
		}

		#endregion

		#region Single?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Single p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(String p)          { return p == null? null: (Single?)Single.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(SByte p)           { return checked((Single?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int16 p)           { return checked((Single?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int32 p)           { return checked((Single?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int64 p)           { return checked((Single?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Byte p)            { return checked((Single?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt16 p)          { return checked((Single?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt32 p)          { return checked((Single?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt64 p)          { return checked((Single?)p); }

		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Double p)          { return checked((Single?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Decimal p)         { return checked((Single?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Char p)            { return checked((Single?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Boolean p)         { return p? 1.0f: 0.0f; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(SByte? p)          { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int16? p)          { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int32? p)          { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Int64? p)          { return p.HasValue? checked((Single?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Byte? p)           { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt16? p)         { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt32? p)         { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Single?</c> value.</summary>
		[CLSCompliant(false)]
		public static Single? ToNullableSingle(UInt64? p)         { return p.HasValue? checked((Single?)p.Value): null; }

		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Double? p)         { return p.HasValue? checked((Single?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Decimal? p)        { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Char? p)           { return p.HasValue? checked((Single?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Boolean? p)        { return p.HasValue? (Single?)(p.Value? 1.0f: 0.0f):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlSingle p)       { return p.IsNull? null:         (Single?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlString p)       { return p.IsNull? null: ToNullableSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlByte p)         { return p.IsNull? null: ToNullableSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlInt16 p)        { return p.IsNull? null: ToNullableSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlInt32 p)        { return p.IsNull? null: ToNullableSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlInt64 p)        { return p.IsNull? null: ToNullableSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlDouble p)       { return p.IsNull? null: ToNullableSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlDecimal p)      { return p.IsNull? null: ToNullableSingle(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlMoney p)        { return p.IsNull? null: ToNullableSingle(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(SqlBoolean p)      { return p.IsNull? null: ToNullableSingle(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(Byte[] p)          { return p == null || p.Length == 0? null: (Single?)ToSingle(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Single?</c> value.</summary>
		public static Single? ToNullableSingle(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Single) return (Single)p;

			// Scalar Types.
			//
			if (p is Single)          return ToNullableSingle((Single)p);
			if (p is String)          return ToNullableSingle((String)p);

			if (p is SByte)           return ToNullableSingle((SByte)p);
			if (p is Int16)           return ToNullableSingle((Int16)p);
			if (p is Int32)           return ToNullableSingle((Int32)p);
			if (p is Int64)           return ToNullableSingle((Int64)p);

			if (p is Byte)            return ToNullableSingle((Byte)p);
			if (p is UInt16)          return ToNullableSingle((UInt16)p);
			if (p is UInt32)          return ToNullableSingle((UInt32)p);
			if (p is UInt64)          return ToNullableSingle((UInt64)p);

			if (p is Double)          return ToNullableSingle((Double)p);

			if (p is Decimal)         return ToNullableSingle((Decimal)p);
			if (p is Boolean)         return ToNullableSingle((Boolean)p);

			// SqlTypes
			//
			if (p is SqlSingle)       return ToNullableSingle((SqlSingle)p);
			if (p is SqlString)       return ToNullableSingle((SqlString)p);

			if (p is SqlByte)         return ToNullableSingle((SqlByte)p);
			if (p is SqlInt16)        return ToNullableSingle((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableSingle((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableSingle((SqlInt64)p);

			if (p is SqlDouble)       return ToNullableSingle((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableSingle((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableSingle((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableSingle((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableSingle((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToSingle(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Single?));
		}

		#endregion

		#region Double?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Double p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(String p)          { return p == null? null: (Double?)Double.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(SByte p)           { return checked((Double?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int16 p)           { return checked((Double?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int32 p)           { return checked((Double?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int64 p)           { return checked((Double?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Byte p)            { return checked((Double?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt16 p)          { return checked((Double?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt32 p)          { return checked((Double?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt64 p)          { return checked((Double?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Single p)          { return checked((Double?)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Decimal p)         { return checked((Double?)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Char p)            { return checked((Double?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Boolean p)         { return p? 1.0: 0.0; }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(DateTime p)        { return (p - DateTime.MinValue).TotalDays; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(DateTimeOffset p)  { return (p - DateTimeOffset.MinValue).TotalDays; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(TimeSpan p)        { return p.TotalDays; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(SByte? p)          { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int16? p)          { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int32? p)          { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Int64? p)          { return p.HasValue? checked((Double?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Byte? p)           { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt16? p)         { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt32? p)         { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Double?</c> value.</summary>
		[CLSCompliant(false)]
		public static Double? ToNullableDouble(UInt64? p)         { return p.HasValue? checked((Double?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Single? p)         { return p.HasValue? checked((Double?)p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Decimal? p)        { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Char? p)           { return p.HasValue? checked((Double?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Boolean? p)        { return p.HasValue? (Double?)(p.Value? 1.0: 0.0):  null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(DateTime? p)       { return p.HasValue? (Double?)(p.Value - DateTime.MinValue).TotalDays: null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(DateTimeOffset? p) { return p.HasValue? (Double?)(p.Value - DateTimeOffset.MinValue).TotalDays: null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(TimeSpan? p)       { return p.HasValue? (Double?)p.Value.TotalDays: null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlDouble p)       { return p.IsNull? null:         (Double?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlString p)       { return p.IsNull? null: ToNullableDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlByte p)         { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlInt16 p)        { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlInt32 p)        { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlInt64 p)        { return p.IsNull? null: ToNullableDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlSingle p)       { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlDecimal p)      { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlMoney p)        { return p.IsNull? null: ToNullableDouble(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlBoolean p)      { return p.IsNull? null: ToNullableDouble(p.Value); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(SqlDateTime p)     { return p.IsNull? null: (Double?)(p.Value - DateTime.MinValue).TotalDays; }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(Byte[] p)          { return p == null || p.Length == 0? null: (Double?)ToDouble(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Double?</c> value.</summary>
		public static Double? ToNullableDouble(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Double) return (Double)p;

			// Scalar Types.
			//
			if (p is Double)          return ToNullableDouble((Double)p);
			if (p is String)          return ToNullableDouble((String)p);

			if (p is SByte)           return ToNullableDouble((SByte)p);
			if (p is Int16)           return ToNullableDouble((Int16)p);
			if (p is Int32)           return ToNullableDouble((Int32)p);
			if (p is Int64)           return ToNullableDouble((Int64)p);

			if (p is Byte)            return ToNullableDouble((Byte)p);
			if (p is UInt16)          return ToNullableDouble((UInt16)p);
			if (p is UInt32)          return ToNullableDouble((UInt32)p);
			if (p is UInt64)          return ToNullableDouble((UInt64)p);

			if (p is Single)          return ToNullableDouble((Single)p);

			if (p is Decimal)         return ToNullableDouble((Decimal)p);
			if (p is Boolean)         return ToNullableDouble((Boolean)p);
			if (p is DateTime)        return ToNullableDouble((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToNullableDouble((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToNullableDouble((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlDouble)       return ToNullableDouble((SqlDouble)p);
			if (p is SqlString)       return ToNullableDouble((SqlString)p);

			if (p is SqlByte)         return ToNullableDouble((SqlByte)p);
			if (p is SqlInt16)        return ToNullableDouble((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableDouble((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableDouble((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableDouble((SqlSingle)p);
			if (p is SqlDecimal)      return ToNullableDouble((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableDouble((SqlMoney)p);

			if (p is SqlBoolean)      return ToNullableDouble((SqlBoolean)p);
			if (p is SqlDateTime)     return ToNullableDouble((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableDouble((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDouble(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Double?));
		}

		#endregion

		#region Boolean?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Boolean p)         { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(String p)          { return p == null? null: (Boolean?)Boolean.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(SByte p)           { return ToBoolean(p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int16 p)           { return ToBoolean(p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int32 p)           { return ToBoolean(p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int64 p)           { return ToBoolean(p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Byte p)            { return ToBoolean(p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt16 p)          { return ToBoolean(p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt32 p)          { return ToBoolean(p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt64 p)          { return ToBoolean(p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Single p)          { return ToBoolean(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Double p)          { return ToBoolean(p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Decimal p)         { return ToBoolean(p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Char p)            { return ToBoolean(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(SByte? p)          { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int16? p)          { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int32? p)          { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Int64? p)          { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Byte? p)           { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt16? p)         { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt32? p)         { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Boolean?</c> value.</summary>
		[CLSCompliant(false)]
		public static Boolean? ToNullableBoolean(UInt64? p)         { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Single? p)         { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Double? p)         { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Decimal? p)        { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Char? p)           { return p.HasValue? (Boolean?)ToBoolean(p.Value): null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlBoolean p)      { return p.IsNull? null: (Boolean?)          p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlString p)       { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlByte p)         { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlInt16 p)        { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlInt32 p)        { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlInt64 p)        { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlSingle p)       { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlDouble p)       { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlDecimal p)      { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(SqlMoney p)        { return p.IsNull? null: (Boolean?)ToBoolean(p.Value); }


		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(Byte[] p)          { return p == null || p.Length == 0? null: (Boolean?)ToBoolean(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Boolean?</c> value.</summary>
		public static Boolean? ToNullableBoolean(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Boolean) return (Boolean)p;

			// Scalar Types.
			//
			if (p is Boolean)         return ToNullableBoolean((Boolean)p);
			if (p is String)          return ToNullableBoolean((String)p);

			if (p is SByte)           return ToNullableBoolean((SByte)p);
			if (p is Int16)           return ToNullableBoolean((Int16)p);
			if (p is Int32)           return ToNullableBoolean((Int32)p);
			if (p is Int64)           return ToNullableBoolean((Int64)p);

			if (p is Byte)            return ToNullableBoolean((Byte)p);
			if (p is UInt16)          return ToNullableBoolean((UInt16)p);
			if (p is UInt32)          return ToNullableBoolean((UInt32)p);
			if (p is UInt64)          return ToNullableBoolean((UInt64)p);

			if (p is Single)          return ToNullableBoolean((Single)p);
			if (p is Double)          return ToNullableBoolean((Double)p);

			if (p is Decimal)         return ToNullableBoolean((Decimal)p);

			// SqlTypes
			//
			if (p is SqlBoolean)      return ToNullableBoolean((SqlBoolean)p);
			if (p is SqlString)       return ToNullableBoolean((SqlString)p);

			if (p is SqlByte)         return ToNullableBoolean((SqlByte)p);
			if (p is SqlInt16)        return ToNullableBoolean((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableBoolean((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableBoolean((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableBoolean((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableBoolean((SqlDouble)p);
			if (p is SqlDecimal)      return ToNullableBoolean((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableBoolean((SqlMoney)p);


			// Other Types
			//
			if (p is Byte[])          return ToNullableBoolean((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToBoolean(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Boolean?));
		}

		#endregion

		#region Decimal?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Decimal p)         { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(String p)          { return p == null? null: (Decimal?)Decimal.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(SByte p)           { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int16 p)           { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int32 p)           { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int64 p)           { return checked((Decimal?)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Byte p)            { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt16 p)          { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt32 p)          { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt64 p)          { return checked((Decimal?)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Single p)          { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Double p)          { return checked((Decimal?)p); }

		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Char p)            { return checked((Decimal?)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Boolean p)         { return p? 1.0m: 0.0m; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(SByte? p)          { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int16? p)          { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int32? p)          { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Int64? p)          { return p.HasValue? checked((Decimal?)p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Byte? p)           { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt16? p)         { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt32? p)         { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Decimal?</c> value.</summary>
		[CLSCompliant(false)]
		public static Decimal? ToNullableDecimal(UInt64? p)         { return p.HasValue? checked((Decimal?)p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Single? p)         { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Double? p)         { return p.HasValue? checked((Decimal?)p.Value): null; }

		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Char? p)           { return p.HasValue? checked((Decimal?)p.Value): null; }
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Boolean? p)        { return p.HasValue? (Decimal?)(p.Value? 1.0m: 0.0m):  null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlDecimal p)      { return p.IsNull? null:         (Decimal?)p.Value;  }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlMoney p)        { return p.IsNull? null:         (Decimal?)p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlString p)       { return p.IsNull? null: ToNullableDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlByte p)         { return p.IsNull? null: ToNullableDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlInt16 p)        { return p.IsNull? null: ToNullableDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlInt32 p)        { return p.IsNull? null: ToNullableDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlInt64 p)        { return p.IsNull? null: ToNullableDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlSingle p)       { return p.IsNull? null: ToNullableDecimal(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlDouble p)       { return p.IsNull? null: ToNullableDecimal(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(SqlBoolean p)      { return p.IsNull? null: ToNullableDecimal(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(Byte[] p)          { return p == null || p.Length == 0? null: (Decimal?)ToDecimal(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Decimal?</c> value.</summary>
		public static Decimal? ToNullableDecimal(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Decimal) return (Decimal)p;

			// Scalar Types.
			//
			if (p is Decimal)         return ToNullableDecimal((Decimal)p);
			if (p is String)          return ToNullableDecimal((String)p);

			if (p is SByte)           return ToNullableDecimal((SByte)p);
			if (p is Int16)           return ToNullableDecimal((Int16)p);
			if (p is Int32)           return ToNullableDecimal((Int32)p);
			if (p is Int64)           return ToNullableDecimal((Int64)p);

			if (p is Byte)            return ToNullableDecimal((Byte)p);
			if (p is UInt16)          return ToNullableDecimal((UInt16)p);
			if (p is UInt32)          return ToNullableDecimal((UInt32)p);
			if (p is UInt64)          return ToNullableDecimal((UInt64)p);

			if (p is Single)          return ToNullableDecimal((Single)p);
			if (p is Double)          return ToNullableDecimal((Double)p);

			if (p is Boolean)         return ToNullableDecimal((Boolean)p);

			// SqlTypes
			//
			if (p is SqlDecimal)      return ToNullableDecimal((SqlDecimal)p);
			if (p is SqlMoney)        return ToNullableDecimal((SqlMoney)p);
			if (p is SqlString)       return ToNullableDecimal((SqlString)p);

			if (p is SqlByte)         return ToNullableDecimal((SqlByte)p);
			if (p is SqlInt16)        return ToNullableDecimal((SqlInt16)p);
			if (p is SqlInt32)        return ToNullableDecimal((SqlInt32)p);
			if (p is SqlInt64)        return ToNullableDecimal((SqlInt64)p);

			if (p is SqlSingle)       return ToNullableDecimal((SqlSingle)p);
			if (p is SqlDouble)       return ToNullableDecimal((SqlDouble)p);

			if (p is SqlBoolean)      return ToNullableDecimal((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableDecimal((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDecimal(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(Decimal?));
		}

		#endregion

		#region DateTime?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(DateTime p)        { return p; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(DateTimeOffset p)  { return p.LocalDateTime; }
		#endif
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(String p)          { return p == null? null: (DateTime?)DateTime.Parse(p); }

		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(TimeSpan p)        { return DateTime.MinValue + p; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(Int64 p)           { return DateTime.MinValue + TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(Double p)          { return DateTime.MinValue + TimeSpan.FromDays(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(TimeSpan? p)       { return p.HasValue? DateTime.MinValue +                           p.Value:  (DateTime?)null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(Int64? p)          { return p.HasValue? DateTime.MinValue +        TimeSpan.FromTicks(p.Value): (DateTime?)null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(Double? p)         { return p.HasValue? DateTime.MinValue + TimeSpan.FromDays(p.Value): (DateTime?)null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(DateTimeOffset? p) { return p.HasValue? p.Value.LocalDateTime: (DateTime?)null; }
		#endif

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(SqlDateTime p)     { return p.IsNull? (DateTime?)null:                                               p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(SqlString p)       { return p.IsNull? (DateTime?)null:                                    ToDateTime(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(SqlInt64 p)        { return p.IsNull? (DateTime?)null: DateTime.MinValue +        TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(SqlDouble p)       { return p.IsNull? (DateTime?)null: DateTime.MinValue + TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(Byte[] p)          { return p == null || p.Length == 0? null: (DateTime?)ToDateTime(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>DateTime?</c> value.</summary>
		public static DateTime? ToNullableDateTime(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is DateTime) return (DateTime)p;

			// Scalar Types.
			//
			if (p is DateTime)        return ToNullableDateTime((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToNullableDateTime((DateTimeOffset)p);
			#endif
			if (p is String)          return ToNullableDateTime((String)p);

			if (p is TimeSpan)        return ToNullableDateTime((TimeSpan)p);
			if (p is Int64)           return ToNullableDateTime((Int64)p);
			if (p is Double)          return ToNullableDateTime((Double)p);

			// SqlTypes
			//
			if (p is SqlDateTime)     return ToNullableDateTime((SqlDateTime)p);
			if (p is SqlString)       return ToNullableDateTime((SqlString)p);
			if (p is SqlInt64)        return ToNullableDateTime((SqlInt64)p);
			if (p is SqlDouble)       return ToNullableDateTime((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableDateTime((Byte[])p);

			if (p is IConvertible) return ((IConvertible)p).ToDateTime(null);
			
			throw CreateInvalidCastException(p.GetType(), typeof(DateTime?));
		}

		#endregion

		#if FW3
		#region DateTimeOffset?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(DateTime p)        { return p; }
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(DateTimeOffset p)  { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(String p)          { return p == null? null: (DateTimeOffset?)DateTimeOffset.Parse(p); }

		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(TimeSpan p)        { return DateTimeOffset.MinValue + p; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(Int64 p)           { return DateTimeOffset.MinValue + TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(Double p)          { return DateTimeOffset.MinValue + TimeSpan.FromDays(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(TimeSpan? p)       { return p.HasValue? DateTimeOffset.MinValue +                           p.Value:  (DateTimeOffset?)null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(Int64? p)          { return p.HasValue? DateTimeOffset.MinValue +        TimeSpan.FromTicks(p.Value): (DateTimeOffset?)null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(Double? p)         { return p.HasValue? DateTimeOffset.MinValue + TimeSpan.FromDays(p.Value): (DateTimeOffset?)null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(DateTime? p)       { return p.HasValue? p: null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(SqlDateTime p)     { return p.IsNull? (DateTimeOffset?)null:                                               p.Value;  }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(SqlString p)       { return p.IsNull? (DateTimeOffset?)null:                                    ToDateTimeOffset(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(SqlInt64 p)        { return p.IsNull? (DateTimeOffset?)null: DateTimeOffset.MinValue +        TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(SqlDouble p)       { return p.IsNull? (DateTimeOffset?)null: DateTimeOffset.MinValue + TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(Byte[] p)          { return p == null || p.Length == 0? null: (DateTimeOffset?)ToDateTimeOffset(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>DateTimeOffset?</c> value.</summary>
		public static DateTimeOffset? ToNullableDateTimeOffset(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is DateTimeOffset) return (DateTimeOffset)p;

			// Scalar Types.
			//
			if (p is DateTime)        return ToNullableDateTimeOffset((DateTime)p);
			if (p is DateTimeOffset)  return ToNullableDateTimeOffset((DateTimeOffset)p);
			if (p is String)          return ToNullableDateTimeOffset((String)p);

			if (p is TimeSpan)        return ToNullableDateTimeOffset((TimeSpan)p);
			if (p is Int64)           return ToNullableDateTimeOffset((Int64)p);
			if (p is Double)          return ToNullableDateTimeOffset((Double)p);

			// SqlTypes
			//
			if (p is SqlDateTime)     return ToNullableDateTimeOffset((SqlDateTime)p);
			if (p is SqlString)       return ToNullableDateTimeOffset((SqlString)p);
			if (p is SqlInt64)        return ToNullableDateTimeOffset((SqlInt64)p);
			if (p is SqlDouble)       return ToNullableDateTimeOffset((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableDateTimeOffset((Byte[])p);

			if (p is IConvertible) return ToDateTimeOffset(((IConvertible)p).ToDateTime(null));
			
			throw CreateInvalidCastException(p.GetType(), typeof(DateTimeOffset?));
		}

		#endregion

		#endif
		#region TimeSpan?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(TimeSpan p)        { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(String p)          { return p == null? null: (TimeSpan?)TimeSpan.Parse(p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(DateTime p)        { return p - DateTime.MinValue; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(DateTimeOffset p)  { return p - DateTimeOffset.MinValue; }
		#endif
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(Int64 p)           { return TimeSpan.FromTicks(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(Double p)          { return TimeSpan.FromDays(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(DateTime? p)       { return p.HasValue? p.Value - DateTime.MinValue: (TimeSpan?)null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(DateTimeOffset? p) { return p.HasValue? p.Value - DateTimeOffset.MinValue: (TimeSpan?)null; }
		#endif
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(Int64? p)          { return p.HasValue? TimeSpan.FromTicks(p.Value): (TimeSpan?)null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(Double? p)         { return p.HasValue? TimeSpan.FromDays(p.Value): (TimeSpan?)null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(SqlString p)       { return p.IsNull? (TimeSpan?)null: TimeSpan.Parse(p.Value);     }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(SqlDateTime p)     { return p.IsNull? (TimeSpan?)null: p.Value - DateTime.MinValue; }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(SqlInt64 p)        { return p.IsNull? (TimeSpan?)null: TimeSpan.FromTicks(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(SqlDouble p)       { return p.IsNull? (TimeSpan?)null: TimeSpan.FromDays(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(Byte[] p)          { return p == null || p.Length == 0? null: (TimeSpan?)ToTimeSpan(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>TimeSpan?</c> value.</summary>
		public static TimeSpan? ToNullableTimeSpan(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is TimeSpan) return (TimeSpan)p;

			// Scalar Types.
			//
			if (p is TimeSpan)        return ToNullableTimeSpan((TimeSpan)p);
			if (p is String)          return ToNullableTimeSpan((String)p);
			if (p is DateTime)        return ToNullableTimeSpan((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToNullableTimeSpan((DateTimeOffset)p);
			#endif
			if (p is Int64)           return ToNullableTimeSpan((Int64)p);
			if (p is Double)          return ToNullableTimeSpan((Double)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToNullableTimeSpan((SqlString)p);
			if (p is SqlDateTime)     return ToNullableTimeSpan((SqlDateTime)p);
			if (p is SqlInt64)        return ToNullableTimeSpan((SqlInt64)p);
			if (p is SqlDouble)       return ToNullableTimeSpan((SqlDouble)p);

			// Other Types
			//
			if (p is Byte[])          return ToNullableTimeSpan((Byte[])p);

			throw CreateInvalidCastException(p.GetType(), typeof(TimeSpan?));
		}

		#endregion

		#region Guid?

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(Guid p)            { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(String p)          { return p == null? null: (Guid?)new Guid(p); }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(SqlGuid p)         { return p.IsNull? null: (Guid?)p.Value;             }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(SqlString p)       { return p.IsNull? null: (Guid?)new Guid(p.Value);   }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(SqlBinary p)       { return p.IsNull? null: (Guid?)p.ToSqlGuid().Value; }

		// Other Types.
		// 
		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(Type p)            { return p == null? null: (Guid?)p.GUID; }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(Byte[] p)          { return p == null? null: (Guid?)new Guid(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Guid?</c> value.</summary>
		public static Guid? ToNullableGuid(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Guid) return (Guid)p;

			// Scalar Types.
			//
			if (p is Guid)            return ToNullableGuid((Guid)p);
			if (p is String)          return ToNullableGuid((String)p);

			// SqlTypes
			//
			if (p is SqlGuid)         return ToNullableGuid((SqlGuid)p);
			if (p is SqlString)       return ToNullableGuid((SqlString)p);
			if (p is SqlBinary)       return ToNullableGuid((SqlBinary)p);

			// Other Types.
			//
			if (p is Type)            return ToNullableGuid((Type)p);
			if (p is Byte[])          return ToNullableGuid((Byte[])p);

			throw CreateInvalidCastException(p.GetType(), typeof(Guid?));
		}

		#endregion

		#endregion

		#region SqlTypes

		#region SqlString

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(String p)          { return p ?? SqlString.Null; }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(SByte p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int16 p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int32 p)           { return p.ToString(); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int64 p)           { return p.ToString(); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Byte p)            { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt16 p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt32 p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt64 p)          { return p.ToString(); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Single p)          { return p.ToString(); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Double p)          { return p.ToString(); }

		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Boolean p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Decimal p)         { return p.ToString(); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Char p)            { return p.ToString(); }
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(TimeSpan p)        { return p.ToString(); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(DateTime p)        { return p.ToString(); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(DateTimeOffset p)  { return p.ToString(); }
		#endif
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Guid p)            { return p.ToString(); }
		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Char[] p)          { return new String(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(SByte? p)          { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int16? p)          { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int32? p)          { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Int64? p)          { return p.HasValue? p.ToString(): SqlString.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Byte? p)           { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt16? p)         { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt32? p)         { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlString</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlString ToSqlString(UInt64? p)         { return p.HasValue? p.ToString(): SqlString.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Single? p)         { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Double? p)         { return p.HasValue? p.ToString(): SqlString.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Boolean? p)        { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Decimal? p)        { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Char? p)           { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(TimeSpan? p)       { return p.HasValue? p.ToString(): SqlString.Null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(DateTime? p)       { return p.HasValue? p.ToString(): SqlString.Null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(DateTimeOffset? p) { return p.HasValue? p.ToString(): SqlString.Null; }
		#endif
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Guid? p)           { return p.HasValue? p.ToString(): SqlString.Null; }

		// SqlTypes
		// 

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlByte p)         { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlInt16 p)        { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlInt32 p)        { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlInt64 p)        { return p.ToSqlString(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlSingle p)       { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlDouble p)       { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlDecimal p)      { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlMoney p)        { return p.ToSqlString(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlBoolean p)      { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlChars p)        { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlXml</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlXml p)          { return p.IsNull? SqlString.Null: p.Value; }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlGuid p)         { return p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(SqlDateTime p)     { return p.ToSqlString(); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Type p)            { return p == null? SqlString.Null: p.FullName; }
		/// <summary>Converts the value from <c>XmlDocument</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(XmlDocument p)     { return p == null? SqlString.Null: p.InnerXml; }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(Byte[] p)          { return p == null || p.Length == 0? SqlString.Null: new SqlString(ToString(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlString</c> value.</summary>
		public static SqlString ToSqlString(object p)         
		{
			if (p == null || p is DBNull) return SqlString.Null;

			if (p is SqlString) return (SqlString)p;

			// Scalar Types.
			//
			if (p is String)          return ToSqlString((String)p);

			if (p is SByte)           return ToSqlString((SByte)p);
			if (p is Int16)           return ToSqlString((Int16)p);
			if (p is Int32)           return ToSqlString((Int32)p);
			if (p is Int64)           return ToSqlString((Int64)p);

			if (p is Byte)            return ToSqlString((Byte)p);
			if (p is UInt16)          return ToSqlString((UInt16)p);
			if (p is UInt32)          return ToSqlString((UInt32)p);
			if (p is UInt64)          return ToSqlString((UInt64)p);

			if (p is Single)          return ToSqlString((Single)p);
			if (p is Double)          return ToSqlString((Double)p);

			if (p is Boolean)         return ToSqlString((Boolean)p);
			if (p is Decimal)         return ToSqlString((Decimal)p);
			if (p is Char[])          return ToSqlString((Char[])p);

			// SqlTypes
			//

			if (p is SqlByte)         return ToSqlString((SqlByte)p);
			if (p is SqlInt16)        return ToSqlString((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlString((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlString((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlString((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlString((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlString((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlString((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlString((SqlBoolean)p);
			if (p is SqlXml)          return ToSqlString((SqlXml)p);

			// Other Types
			//
			if (p is Type)            return ToSqlString((Type)p);
			if (p is XmlDocument)     return ToSqlString((XmlDocument)p);
			if (p is Byte[])          return ToSqlString((Byte[])p);

			return ToString(p);
		}

		#endregion

		#region SqlByte

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Byte p)            { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(String p)          { return p == null? SqlByte.Null: SqlByte.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(SByte p)           { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int16 p)           { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int32 p)           { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int64 p)           { return checked((Byte)p); }

		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt16 p)          { return checked((Byte)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt32 p)          { return checked((Byte)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt64 p)          { return checked((Byte)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Single p)          { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Double p)          { return checked((Byte)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Decimal p)         { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Char p)            { return checked((Byte)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Boolean p)         { return (Byte)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Byte? p)           { return p.HasValue?        p.Value:  SqlByte.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(SByte? p)          { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int16? p)          { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int32? p)          { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Int64? p)          { return p.HasValue? ToByte(p.Value): SqlByte.Null; }

		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt16? p)         { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt32? p)         { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlByte</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlByte ToSqlByte(UInt64? p)         { return p.HasValue? ToByte(p.Value): SqlByte.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Single? p)         { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Double? p)         { return p.HasValue? ToByte(p.Value): SqlByte.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Boolean? p)        { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Decimal? p)        { return p.HasValue? ToByte(p.Value): SqlByte.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Char? p)           { return p.HasValue? ToByte(p.Value): SqlByte.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlString p)       { return p.ToSqlByte(); }

		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlInt16 p)        { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlInt32 p)        { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlInt64 p)        { return p.ToSqlByte(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlSingle p)       { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlDouble p)       { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlDecimal p)      { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlMoney p)        { return p.ToSqlByte(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlBoolean p)      { return p.ToSqlByte(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(SqlDateTime p)     { return p.IsNull? SqlByte.Null: ToByte(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(Byte[] p)          { return p == null || p.Length == 0? SqlByte.Null: new SqlByte(ToByte(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlByte</c> value.</summary>
		public static SqlByte ToSqlByte(object p)         
		{
			if (p == null || p is DBNull) return SqlByte.Null;

			if (p is SqlByte) return (SqlByte)p;

			// Scalar Types.
			//
			if (p is Byte)            return ToSqlByte((Byte)p);
			if (p is String)          return ToSqlByte((String)p);

			if (p is SByte)           return ToSqlByte((SByte)p);
			if (p is Int16)           return ToSqlByte((Int16)p);
			if (p is Int32)           return ToSqlByte((Int32)p);
			if (p is Int64)           return ToSqlByte((Int64)p);

			if (p is UInt16)          return ToSqlByte((UInt16)p);
			if (p is UInt32)          return ToSqlByte((UInt32)p);
			if (p is UInt64)          return ToSqlByte((UInt64)p);

			if (p is Single)          return ToSqlByte((Single)p);
			if (p is Double)          return ToSqlByte((Double)p);

			if (p is Decimal)         return ToSqlByte((Decimal)p);
			if (p is Boolean)         return ToSqlByte((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlByte((SqlString)p);

			if (p is SqlInt16)        return ToSqlByte((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlByte((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlByte((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlByte((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlByte((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlByte((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlByte((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlByte((SqlBoolean)p);
			if (p is SqlDateTime)     return ToSqlByte((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlByte((Byte[])p);

			return ToByte(p);
		}

		#endregion

		#region SqlInt16

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int16 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(String p)          { return p == null? SqlInt16.Null: SqlInt16.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(SByte p)           { return checked((Int16)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int32 p)           { return checked((Int16)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int64 p)           { return checked((Int16)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Byte p)            { return checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt16 p)          { return checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt32 p)          { return checked((Int16)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt64 p)          { return checked((Int16)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Single p)          { return checked((Int16)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Double p)          { return checked((Int16)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Decimal p)         { return checked((Int16)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Char p)            { return checked((Int16)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Boolean p)         { return (Int16)(p? 1: 0); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int16? p)          { return p.HasValue?         p.Value:  SqlInt16.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(SByte? p)          { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int32? p)          { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Int64? p)          { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Byte? p)           { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt16? p)         { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt32? p)         { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt16 ToSqlInt16(UInt64? p)         { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Single? p)         { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Double? p)         { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Boolean? p)        { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Decimal? p)        { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Char? p)           { return p.HasValue? ToInt16(p.Value): SqlInt16.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlString p)       { return p.ToSqlInt16(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlByte p)         { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlInt32 p)        { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlInt64 p)        { return p.ToSqlInt16(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlSingle p)       { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlDouble p)       { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlDecimal p)      { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlMoney p)        { return p.ToSqlInt16(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlBoolean p)      { return p.ToSqlInt16(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(SqlDateTime p)     { return p.IsNull? SqlInt16.Null: ToInt16(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(Byte[] p)          { return p == null || p.Length == 0? SqlInt16.Null: new SqlInt16(ToInt16(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlInt16</c> value.</summary>
		public static SqlInt16 ToSqlInt16(object p)         
		{
			if (p == null || p is DBNull) return SqlInt16.Null;

			if (p is SqlInt16) return (SqlInt16)p;

			// Scalar Types.
			//
			if (p is Int16)           return ToSqlInt16((Int16)p);
			if (p is String)          return ToSqlInt16((String)p);

			if (p is SByte)           return ToSqlInt16((SByte)p);
			if (p is Int32)           return ToSqlInt16((Int32)p);
			if (p is Int64)           return ToSqlInt16((Int64)p);

			if (p is Byte)            return ToSqlInt16((Byte)p);
			if (p is UInt16)          return ToSqlInt16((UInt16)p);
			if (p is UInt32)          return ToSqlInt16((UInt32)p);
			if (p is UInt64)          return ToSqlInt16((UInt64)p);

			if (p is Single)          return ToSqlInt16((Single)p);
			if (p is Double)          return ToSqlInt16((Double)p);

			if (p is Decimal)         return ToSqlInt16((Decimal)p);
			if (p is Boolean)         return ToSqlInt16((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlInt16((SqlString)p);

			if (p is SqlByte)         return ToSqlInt16((SqlByte)p);
			if (p is SqlInt32)        return ToSqlInt16((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlInt16((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlInt16((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlInt16((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlInt16((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlInt16((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlInt16((SqlBoolean)p);
			if (p is SqlDateTime)     return ToSqlInt16((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlInt16((Byte[])p);

			return ToInt16(p);
		}

		#endregion

		#region SqlInt32

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int32 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(String p)          { return p == null? SqlInt32.Null: SqlInt32.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(SByte p)           { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int16 p)           { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int64 p)           { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Byte p)            { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt16 p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt32 p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt64 p)          { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Single p)          { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Double p)          { return checked((Int32)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Decimal p)         { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Char p)            { return checked((Int32)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Boolean p)         { return p? 1: 0; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int32? p)          { return p.HasValue?         p.Value:  SqlInt32.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(SByte? p)          { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int16? p)          { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Int64? p)          { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Byte? p)           { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt16? p)         { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt32? p)         { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt32 ToSqlInt32(UInt64? p)         { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Single? p)         { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Double? p)         { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Boolean? p)        { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Decimal? p)        { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Char? p)           { return p.HasValue? ToInt32(p.Value): SqlInt32.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlString p)       { return p.ToSqlInt32(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlByte p)         { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlInt16 p)        { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlInt64 p)        { return p.ToSqlInt32(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlSingle p)       { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlDouble p)       { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlDecimal p)      { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlMoney p)        { return p.ToSqlInt32(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlBoolean p)      { return p.ToSqlInt32(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(SqlDateTime p)     { return p.IsNull? SqlInt32.Null: ToInt32(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(Byte[] p)          { return p == null || p.Length == 0? SqlInt32.Null: new SqlInt32(ToInt32(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlInt32</c> value.</summary>
		public static SqlInt32 ToSqlInt32(object p)         
		{
			if (p == null || p is DBNull) return SqlInt32.Null;

			if (p is SqlInt32) return (SqlInt32)p;

			// Scalar Types.
			//
			if (p is Int32)           return ToSqlInt32((Int32)p);
			if (p is String)          return ToSqlInt32((String)p);

			if (p is SByte)           return ToSqlInt32((SByte)p);
			if (p is Int16)           return ToSqlInt32((Int16)p);
			if (p is Int64)           return ToSqlInt32((Int64)p);

			if (p is Byte)            return ToSqlInt32((Byte)p);
			if (p is UInt16)          return ToSqlInt32((UInt16)p);
			if (p is UInt32)          return ToSqlInt32((UInt32)p);
			if (p is UInt64)          return ToSqlInt32((UInt64)p);

			if (p is Single)          return ToSqlInt32((Single)p);
			if (p is Double)          return ToSqlInt32((Double)p);

			if (p is Decimal)         return ToSqlInt32((Decimal)p);
			if (p is Boolean)         return ToSqlInt32((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlInt32((SqlString)p);

			if (p is SqlByte)         return ToSqlInt32((SqlByte)p);
			if (p is SqlInt16)        return ToSqlInt32((SqlInt16)p);
			if (p is SqlInt64)        return ToSqlInt32((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlInt32((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlInt32((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlInt32((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlInt32((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlInt32((SqlBoolean)p);
			if (p is SqlDateTime)     return ToSqlInt32((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlInt32((Byte[])p);

			return ToInt32(p);
		}

		#endregion

		#region SqlInt64

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int64 p)           { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(String p)          { return p == null? SqlInt64.Null: SqlInt64.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(SByte p)           { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int16 p)           { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int32 p)           { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Byte p)            { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt16 p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt32 p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt64 p)          { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Single p)          { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Double p)          { return checked((Int64)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Decimal p)         { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Char p)            { return checked((Int64)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Boolean p)         { return p? 1: 0; }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(DateTime p)        { return (p - DateTime.MinValue).Ticks; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(DateTimeOffset p)  { return (p - DateTimeOffset.MinValue).Ticks; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(TimeSpan p)        { return p.Ticks; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int64? p)          { return p.HasValue?         p.Value:  SqlInt64.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(SByte? p)          { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int16? p)          { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Int32? p)          { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Byte? p)           { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt16? p)         { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt32? p)         { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlInt64 ToSqlInt64(UInt64? p)         { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Single? p)         { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Double? p)         { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Boolean? p)        { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Decimal? p)        { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Char? p)           { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(DateTime? p)       { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(DateTimeOffset? p) { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(TimeSpan? p)       { return p.HasValue? ToInt64(p.Value): SqlInt64.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlString p)       { return p.ToSqlInt64(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlByte p)         { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlInt16 p)        { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlInt32 p)        { return p.ToSqlInt64(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlSingle p)       { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlDouble p)       { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlDecimal p)      { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlMoney p)        { return p.ToSqlInt64(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlBoolean p)      { return p.ToSqlInt64(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(SqlDateTime p)     { return p.IsNull? SqlInt64.Null: ToInt64(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(Byte[] p)          { return p == null || p.Length == 0? SqlInt64.Null: new SqlInt64(ToInt64(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlInt64</c> value.</summary>
		public static SqlInt64 ToSqlInt64(object p)         
		{
			if (p == null || p is DBNull) return SqlInt64.Null;

			if (p is SqlInt64) return (SqlInt64)p;

			// Scalar Types.
			//
			if (p is Int64)           return ToSqlInt64((Int64)p);
			if (p is String)          return ToSqlInt64((String)p);

			if (p is SByte)           return ToSqlInt64((SByte)p);
			if (p is Int16)           return ToSqlInt64((Int16)p);
			if (p is Int32)           return ToSqlInt64((Int32)p);

			if (p is Byte)            return ToSqlInt64((Byte)p);
			if (p is UInt16)          return ToSqlInt64((UInt16)p);
			if (p is UInt32)          return ToSqlInt64((UInt32)p);
			if (p is UInt64)          return ToSqlInt64((UInt64)p);

			if (p is Single)          return ToSqlInt64((Single)p);
			if (p is Double)          return ToSqlInt64((Double)p);

			if (p is Decimal)         return ToSqlInt64((Decimal)p);
			if (p is Boolean)         return ToSqlInt64((Boolean)p);
			if (p is DateTime)        return ToSqlInt64((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToSqlInt64((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToSqlInt64((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlInt64((SqlString)p);

			if (p is SqlByte)         return ToSqlInt64((SqlByte)p);
			if (p is SqlInt16)        return ToSqlInt64((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlInt64((SqlInt32)p);

			if (p is SqlSingle)       return ToSqlInt64((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlInt64((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlInt64((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlInt64((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlInt64((SqlBoolean)p);
			if (p is SqlDateTime)     return ToSqlInt64((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlInt64((Byte[])p);

			return ToInt64(p);
		}

		#endregion

		#region SqlSingle

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Single p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(String p)          { return p == null? SqlSingle.Null: SqlSingle.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(SByte p)           { return checked((Single)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int16 p)           { return checked((Single)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int32 p)           { return checked((Single)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int64 p)           { return checked((Single)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Byte p)            { return checked((Single)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt16 p)          { return checked((Single)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt32 p)          { return checked((Single)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt64 p)          { return checked((Single)p); }

		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Double p)          { return checked((Single)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Decimal p)         { return checked((Single)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Char p)            { return checked((Single)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Boolean p)         { return p? 1.0f: 0.0f; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Single? p)         { return p.HasValue?          p.Value:  SqlSingle.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(SByte? p)          { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int16? p)          { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int32? p)          { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Int64? p)          { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Byte? p)           { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt16? p)         { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt32? p)         { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlSingle ToSqlSingle(UInt64? p)         { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }

		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Double? p)         { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Boolean? p)        { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Decimal? p)        { return p.HasValue? ToSingle(p.Value): SqlSingle.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlString p)       { return p.ToSqlSingle(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlByte p)         { return p.ToSqlSingle(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlInt16 p)        { return p.ToSqlSingle(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlInt32 p)        { return p.ToSqlSingle(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlInt64 p)        { return p.ToSqlSingle(); }

		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlDouble p)       { return p.ToSqlSingle(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlDecimal p)      { return p.ToSqlSingle(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlMoney p)        { return p.ToSqlSingle(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(SqlBoolean p)      { return p.ToSqlSingle(); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(Byte[] p)          { return p == null || p.Length == 0? SqlSingle.Null: new SqlSingle(ToSingle(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlSingle</c> value.</summary>
		public static SqlSingle ToSqlSingle(object p)         
		{
			if (p == null || p is DBNull) return SqlSingle.Null;

			if (p is SqlSingle) return (SqlSingle)p;

			// Scalar Types.
			//
			if (p is Single)          return ToSqlSingle((Single)p);
			if (p is String)          return ToSqlSingle((String)p);

			if (p is SByte)           return ToSqlSingle((SByte)p);
			if (p is Int16)           return ToSqlSingle((Int16)p);
			if (p is Int32)           return ToSqlSingle((Int32)p);
			if (p is Int64)           return ToSqlSingle((Int64)p);

			if (p is Byte)            return ToSqlSingle((Byte)p);
			if (p is UInt16)          return ToSqlSingle((UInt16)p);
			if (p is UInt32)          return ToSqlSingle((UInt32)p);
			if (p is UInt64)          return ToSqlSingle((UInt64)p);

			if (p is Double)          return ToSqlSingle((Double)p);

			if (p is Decimal)         return ToSqlSingle((Decimal)p);
			if (p is Boolean)         return ToSqlSingle((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlSingle((SqlString)p);

			if (p is SqlByte)         return ToSqlSingle((SqlByte)p);
			if (p is SqlInt16)        return ToSqlSingle((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlSingle((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlSingle((SqlInt64)p);

			if (p is SqlDouble)       return ToSqlSingle((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlSingle((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlSingle((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlSingle((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlSingle((Byte[])p);

			return ToSingle(p);
		}

		#endregion

		#region SqlDouble

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Double p)          { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(String p)          { return p == null? SqlDouble.Null: SqlDouble.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(SByte p)           { return checked((Double)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int16 p)           { return checked((Double)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int32 p)           { return checked((Double)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int64 p)           { return checked((Double)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Byte p)            { return checked((Double)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt16 p)          { return checked((Double)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt32 p)          { return checked((Double)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt64 p)          { return checked((Double)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Single p)          { return checked((Double)p); }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Decimal p)         { return checked((Double)p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Char p)            { return checked((Double)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Boolean p)         { return p? 1.0: 0.0; }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(DateTime p)        { return (p - DateTime.MinValue).TotalDays; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(DateTimeOffset p)  { return (p - DateTimeOffset.MinValue).TotalDays; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(TimeSpan p)        { return p.TotalDays; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Double? p)         { return p.HasValue?          p.Value:  SqlDouble.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(SByte? p)          { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int16? p)          { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int32? p)          { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Int64? p)          { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Byte? p)           { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt16? p)         { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt32? p)         { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDouble ToSqlDouble(UInt64? p)         { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Single? p)         { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Boolean? p)        { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Decimal? p)        { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(DateTime? p)       { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(DateTimeOffset? p) { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(TimeSpan? p)       { return p.HasValue? ToDouble(p.Value): SqlDouble.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlString p)       { return p.ToSqlDouble(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlByte p)         { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlInt16 p)        { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlInt32 p)        { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlInt64 p)        { return p.ToSqlDouble(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlSingle p)       { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlDecimal p)      { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlMoney p)        { return p.ToSqlDouble(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlBoolean p)      { return p.ToSqlDouble(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(SqlDateTime p)     { return p.IsNull? SqlDouble.Null: ToDouble(p.Value); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(Byte[] p)          { return p == null || p.Length == 0? SqlDouble.Null: new SqlDouble(ToDouble(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlDouble</c> value.</summary>
		public static SqlDouble ToSqlDouble(object p)         
		{
			if (p == null || p is DBNull) return SqlDouble.Null;

			if (p is SqlDouble) return (SqlDouble)p;

			// Scalar Types.
			//
			if (p is Double)          return ToSqlDouble((Double)p);
			if (p is String)          return ToSqlDouble((String)p);

			if (p is SByte)           return ToSqlDouble((SByte)p);
			if (p is Int16)           return ToSqlDouble((Int16)p);
			if (p is Int32)           return ToSqlDouble((Int32)p);
			if (p is Int64)           return ToSqlDouble((Int64)p);

			if (p is Byte)            return ToSqlDouble((Byte)p);
			if (p is UInt16)          return ToSqlDouble((UInt16)p);
			if (p is UInt32)          return ToSqlDouble((UInt32)p);
			if (p is UInt64)          return ToSqlDouble((UInt64)p);

			if (p is Single)          return ToSqlDouble((Single)p);

			if (p is Decimal)         return ToSqlDouble((Decimal)p);
			if (p is Boolean)         return ToSqlDouble((Boolean)p);
			if (p is DateTime)        return ToSqlDouble((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToSqlDouble((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToSqlDouble((TimeSpan)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlDouble((SqlString)p);

			if (p is SqlByte)         return ToSqlDouble((SqlByte)p);
			if (p is SqlInt16)        return ToSqlDouble((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlDouble((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlDouble((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlDouble((SqlSingle)p);
			if (p is SqlDecimal)      return ToSqlDouble((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlDouble((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlDouble((SqlBoolean)p);
			if (p is SqlDateTime)     return ToSqlDouble((SqlDateTime)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlDouble((Byte[])p);

			return ToDouble(p);
		}

		#endregion

		#region SqlDecimal

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Decimal p)         { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(String p)          { return p == null? SqlDecimal.Null: SqlDecimal.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(SByte p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int16 p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int32 p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int64 p)           { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Byte p)            { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt16 p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt32 p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt64 p)          { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Single p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Double p)          { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Char p)            { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Boolean p)         { return p? 1.0m: 0.0m; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Decimal? p)        { return p.HasValue?           p.Value:  SqlDecimal.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(SByte? p)          { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int16? p)          { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int32? p)          { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Int64? p)          { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Byte? p)           { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt16? p)         { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt32? p)         { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlDecimal ToSqlDecimal(UInt64? p)         { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Single? p)         { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Double? p)         { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Boolean? p)        { return p.HasValue? ToDecimal(p.Value): SqlDecimal.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlString p)       { return p.ToSqlDecimal(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlByte p)         { return p.ToSqlDecimal(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlInt16 p)        { return p.ToSqlDecimal(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlInt32 p)        { return p.ToSqlDecimal(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlInt64 p)        { return p.ToSqlDecimal(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlSingle p)       { return p.ToSqlDecimal(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlDouble p)       { return p.ToSqlDecimal(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlMoney p)        { return p.ToSqlDecimal(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(SqlBoolean p)      { return p.ToSqlDecimal(); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(Byte[] p)          { return p == null || p.Length == 0? SqlDecimal.Null: new SqlDecimal(ToDecimal(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlDecimal</c> value.</summary>
		public static SqlDecimal ToSqlDecimal(object p)         
		{
			if (p == null || p is DBNull) return SqlDecimal.Null;

			if (p is SqlDecimal) return (SqlDecimal)p;

			// Scalar Types.
			//
			if (p is Decimal)         return ToSqlDecimal((Decimal)p);
			if (p is String)          return ToSqlDecimal((String)p);

			if (p is SByte)           return ToSqlDecimal((SByte)p);
			if (p is Int16)           return ToSqlDecimal((Int16)p);
			if (p is Int32)           return ToSqlDecimal((Int32)p);
			if (p is Int64)           return ToSqlDecimal((Int64)p);

			if (p is Byte)            return ToSqlDecimal((Byte)p);
			if (p is UInt16)          return ToSqlDecimal((UInt16)p);
			if (p is UInt32)          return ToSqlDecimal((UInt32)p);
			if (p is UInt64)          return ToSqlDecimal((UInt64)p);

			if (p is Single)          return ToSqlDecimal((Single)p);
			if (p is Double)          return ToSqlDecimal((Double)p);

			if (p is Boolean)         return ToSqlDecimal((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlDecimal((SqlString)p);

			if (p is SqlByte)         return ToSqlDecimal((SqlByte)p);
			if (p is SqlInt16)        return ToSqlDecimal((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlDecimal((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlDecimal((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlDecimal((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlDecimal((SqlDouble)p);
			if (p is SqlMoney)        return ToSqlDecimal((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlDecimal((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlDecimal((Byte[])p);

			return ToDecimal(p);
		}

		#endregion

		#region SqlMoney

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Decimal p)         { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(String p)          { return p == null? SqlMoney.Null: SqlMoney.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(SByte p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int16 p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int32 p)           { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int64 p)           { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Byte p)            { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt16 p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt32 p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt64 p)          { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Single p)          { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Double p)          { return checked((Decimal)p); }

		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Char p)            { return checked((Decimal)p); }
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Boolean p)         { return p? 1.0m: 0.0m; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Decimal? p)        { return p.HasValue?           p.Value:  SqlMoney.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(SByte? p)          { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int16? p)          { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int32? p)          { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Int64? p)          { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Byte? p)           { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt16? p)         { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt32? p)         { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlMoney ToSqlMoney(UInt64? p)         { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Single? p)         { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Double? p)         { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Boolean? p)        { return p.HasValue? ToDecimal(p.Value): SqlMoney.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlString p)       { return p.ToSqlMoney(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlByte p)         { return p.ToSqlMoney(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlInt16 p)        { return p.ToSqlMoney(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlInt32 p)        { return p.ToSqlMoney(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlInt64 p)        { return p.ToSqlMoney(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlSingle p)       { return p.ToSqlMoney(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlDouble p)       { return p.ToSqlMoney(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlDecimal p)      { return p.ToSqlMoney(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(SqlBoolean p)      { return p.ToSqlMoney(); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(Byte[] p)          { return p == null || p.Length == 0? SqlMoney.Null: new SqlMoney(ToDecimal(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlMoney</c> value.</summary>
		public static SqlMoney ToSqlMoney(object p)         
		{
			if (p == null || p is DBNull) return SqlMoney.Null;

			if (p is SqlMoney) return (SqlMoney)p;

			// Scalar Types.
			//
			if (p is Decimal)         return ToSqlMoney((Decimal)p);
			if (p is String)          return ToSqlMoney((String)p);

			if (p is SByte)           return ToSqlMoney((SByte)p);
			if (p is Int16)           return ToSqlMoney((Int16)p);
			if (p is Int32)           return ToSqlMoney((Int32)p);
			if (p is Int64)           return ToSqlMoney((Int64)p);

			if (p is Byte)            return ToSqlMoney((Byte)p);
			if (p is UInt16)          return ToSqlMoney((UInt16)p);
			if (p is UInt32)          return ToSqlMoney((UInt32)p);
			if (p is UInt64)          return ToSqlMoney((UInt64)p);

			if (p is Single)          return ToSqlMoney((Single)p);
			if (p is Double)          return ToSqlMoney((Double)p);

			if (p is Boolean)         return ToSqlMoney((Boolean)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlMoney((SqlString)p);

			if (p is SqlByte)         return ToSqlMoney((SqlByte)p);
			if (p is SqlInt16)        return ToSqlMoney((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlMoney((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlMoney((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlMoney((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlMoney((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlMoney((SqlDecimal)p);

			if (p is SqlBoolean)      return ToSqlMoney((SqlBoolean)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlMoney((Byte[])p);

			return ToDecimal(p);
		}

		#endregion

		#region SqlBoolean

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Boolean p)         { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(String p)          { return p == null? SqlBoolean.Null: SqlBoolean.Parse(p); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(SByte p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int16 p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int32 p)           { return p != 0; }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int64 p)           { return p != 0; }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Byte p)            { return p != 0; }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt16 p)          { return p != 0; }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt32 p)          { return p != 0; }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt64 p)          { return p != 0; }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Single p)          { return p != 0; }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Double p)          { return p != 0; }

		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Decimal p)         { return p != 0; }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Char p)            { return p != 0; }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Boolean? p)        { return p.HasValue?           p.Value:  SqlBoolean.Null; }
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(SByte? p)          { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int16? p)          { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int32? p)          { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Int64? p)          { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Byte? p)           { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt16? p)         { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt32? p)         { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlBoolean ToSqlBoolean(UInt64? p)         { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Single? p)         { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Double? p)         { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }

		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Decimal? p)        { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Char? p)           { return p.HasValue? ToBoolean(p.Value): SqlBoolean.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlString p)       { return p.ToSqlBoolean(); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlByte p)         { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlInt16 p)        { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlInt32 p)        { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlInt64 p)        { return p.ToSqlBoolean(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlSingle p)       { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlDouble p)       { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlDecimal p)      { return p.ToSqlBoolean(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(SqlMoney p)        { return p.ToSqlBoolean(); }


		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(Byte[] p)          { return p == null || p.Length == 0? SqlBoolean.Null: new SqlBoolean(ToBoolean(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlBoolean</c> value.</summary>
		public static SqlBoolean ToSqlBoolean(object p)         
		{
			if (p == null || p is DBNull) return SqlBoolean.Null;

			if (p is SqlBoolean) return (SqlBoolean)p;

			// Scalar Types.
			//
			if (p is Boolean)         return ToSqlBoolean((Boolean)p);
			if (p is String)          return ToSqlBoolean((String)p);

			if (p is SByte)           return ToSqlBoolean((SByte)p);
			if (p is Int16)           return ToSqlBoolean((Int16)p);
			if (p is Int32)           return ToSqlBoolean((Int32)p);
			if (p is Int64)           return ToSqlBoolean((Int64)p);

			if (p is Byte)            return ToSqlBoolean((Byte)p);
			if (p is UInt16)          return ToSqlBoolean((UInt16)p);
			if (p is UInt32)          return ToSqlBoolean((UInt32)p);
			if (p is UInt64)          return ToSqlBoolean((UInt64)p);

			if (p is Single)          return ToSqlBoolean((Single)p);
			if (p is Double)          return ToSqlBoolean((Double)p);

			if (p is Decimal)         return ToSqlBoolean((Decimal)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlBoolean((SqlString)p);

			if (p is SqlByte)         return ToSqlBoolean((SqlByte)p);
			if (p is SqlInt16)        return ToSqlBoolean((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlBoolean((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlBoolean((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlBoolean((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlBoolean((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlBoolean((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlBoolean((SqlMoney)p);


			// Other Types
			//
			if (p is Byte[])          return ToSqlBoolean((Byte[])p);

			return ToBoolean(p);
		}

		#endregion

		#region SqlDateTime

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(String p)          { return p == null? SqlDateTime.Null: SqlDateTime.Parse(p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(DateTime p)        { return p; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(DateTimeOffset p)  { return p.LocalDateTime; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(TimeSpan p)        { return ToDateTime(p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(Int64 p)           { return ToDateTime(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(Double p)          { return ToDateTime(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(DateTime? p)       { return p.HasValue?            p.Value:  SqlDateTime.Null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(DateTimeOffset? p) { return p.HasValue?            p.Value.LocalDateTime:  SqlDateTime.Null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(TimeSpan? p)       { return p.HasValue? ToDateTime(p.Value): SqlDateTime.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(Int64? p)          { return p.HasValue? ToDateTime(p.Value): SqlDateTime.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(Double? p)         { return p.HasValue? ToDateTime(p.Value): SqlDateTime.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(SqlString p)       { return p.ToSqlDateTime(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(SqlInt64 p)        { return p.IsNull? SqlDateTime.Null: ToDateTime(p); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(SqlDouble p)       { return p.IsNull? SqlDateTime.Null: ToDateTime(p); }

		// Other Types
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(Byte[] p)          { return p == null || p.Length == 0? SqlDateTime.Null: new SqlDateTime(ToDateTime(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlDateTime</c> value.</summary>
		public static SqlDateTime ToSqlDateTime(object p)         
		{
			if (p == null || p is DBNull) return SqlDateTime.Null;

			if (p is SqlDateTime) return (SqlDateTime)p;

			// Scalar Types.
			//
			if (p is String)          return ToSqlDateTime((String)p);
			if (p is DateTime)        return ToSqlDateTime((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToSqlDateTime((DateTimeOffset)p);
			#endif

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlDateTime((SqlString)p);

			// Other Types
			//
			if (p is Byte[])          return ToSqlDateTime((Byte[])p);

			return ToDateTime(p);
		}

		#endregion

		#region SqlGuid

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(Guid p)            { return p; }
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(String p)          { return p == null? SqlGuid.Null: SqlGuid.Parse(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(Guid? p)           { return p.HasValue? p.Value : SqlGuid.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(SqlBinary p)       { return p.ToSqlGuid(); }
		/// <summary>Converts the value from <c>SqlBytes</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(SqlBytes p)        { return p.ToSqlBinary().ToSqlGuid(); }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(SqlString p)       { return p.ToSqlGuid(); }

		// Other Types.
		// 
		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(Type p)            { return p == null? SqlGuid.Null: p.GUID; }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(Byte[] p)          { return p == null? SqlGuid.Null: new SqlGuid(p); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlGuid</c> value.</summary>
		public static SqlGuid ToSqlGuid(object p)         
		{
			if (p == null || p is DBNull) return SqlGuid.Null;

			if (p is SqlGuid) return (SqlGuid)p;

			// Scalar Types.
			//
			if (p is Guid)            return ToSqlGuid((Guid)p);
			if (p is String)          return ToSqlGuid((String)p);

			// SqlTypes
			//
			if (p is SqlBytes)        return ToSqlGuid((SqlBytes)p);

			// Other Types.
			//
			if (p is Type)            return ToSqlGuid((Type)p);
			if (p is Byte[])          return ToSqlGuid((Byte[])p);

			return ToGuid(p);
		}

		#endregion

		#region SqlBinary

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(Byte[] p)          { return p; }
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(Guid p)            { return p == Guid.Empty? SqlBinary.Null: new SqlGuid(p).ToSqlBinary(); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(Guid? p)           { return p.HasValue? new SqlGuid(p.Value).ToSqlBinary(): SqlBinary.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBytes</c> to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(SqlBytes p)        { return p.ToSqlBinary(); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(SqlGuid p)         { return p.ToSqlBinary(); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlBinary</c> value.</summary>
		public static SqlBinary ToSqlBinary(object p)         
		{
			if (p == null || p is DBNull) return SqlBinary.Null;

			if (p is SqlBinary) return (SqlBinary)p;

			// Scalar Types.
			//
			if (p is Byte[])          return ToSqlBinary((Byte[])p);
			if (p is Guid)            return ToSqlBinary((Guid)p);

			// SqlTypes
			//

			return ToByteArray(p);
		}

		#endregion

		#region SqlBytes

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(Byte[] p)          { return p == null? SqlBytes.Null: new SqlBytes(p); }
		/// <summary>Converts the value from <c>Stream</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(Stream p)          { return p == null? SqlBytes.Null: new SqlBytes(p); }
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(Guid p)            { return p == Guid.Empty? SqlBytes.Null: new SqlBytes(p.ToByteArray()); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(Guid? p)           { return p.HasValue? new SqlBytes(p.Value.ToByteArray()): SqlBytes.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(SqlBinary p)       { return p.IsNull? SqlBytes.Null: new SqlBytes(p); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(SqlGuid p)         { return p.IsNull? SqlBytes.Null: new SqlBytes(p.ToByteArray()); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlBytes</c> value.</summary>
		public static SqlBytes ToSqlBytes(object p)         
		{
			if (p == null || p is DBNull) return SqlBytes.Null;

			if (p is SqlBytes) return (SqlBytes)p;

			// Scalar Types.
			//
			if (p is Byte[])          return ToSqlBytes((Byte[])p);
			if (p is Stream)          return ToSqlBytes((Stream)p);
			if (p is Guid)            return ToSqlBytes((Guid)p);

			// SqlTypes
			//
			if (p is SqlBinary)       return ToSqlBytes((SqlBinary)p);
			if (p is SqlGuid)         return ToSqlBytes((SqlGuid)p);

			return new SqlBytes(ToByteArray(p));
		}

		#endregion

		#region SqlChars

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(String p)          { return p == null? SqlChars.Null: new SqlChars(p.ToCharArray()); }
		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Char[] p)          { return p == null? SqlChars.Null: new SqlChars(p); }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Byte[] p)          { return p == null? SqlChars.Null: new SqlChars(ToCharArray(p)); }

		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(SByte p)           { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int16 p)           { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int32 p)           { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int64 p)           { return new SqlChars(ToString(p).ToCharArray()); }

		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Byte p)            { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt16 p)          { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt32 p)          { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt64 p)          { return new SqlChars(ToString(p).ToCharArray()); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Single p)          { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Double p)          { return new SqlChars(ToString(p).ToCharArray()); }

		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Boolean p)         { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Decimal p)         { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Char p)            { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(TimeSpan p)        { return new SqlChars(ToString(p).ToCharArray()); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(DateTime p)        { return new SqlChars(ToString(p).ToCharArray()); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(DateTimeOffset p)  { return new SqlChars(ToString(p).ToCharArray()); }
		#endif
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Guid p)            { return new SqlChars(ToString(p).ToCharArray()); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(SByte? p)          { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int16? p)          { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int32? p)          { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Int64? p)          { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Byte? p)           { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt16? p)         { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt32? p)         { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>SqlChars</c> value.</summary>
		[CLSCompliant(false)]
		public static SqlChars ToSqlChars(UInt64? p)         { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Single? p)         { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Double? p)         { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Boolean? p)        { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Decimal? p)        { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Char? p)           { return p.HasValue? new SqlChars(new Char[]{p.Value})       : SqlChars.Null; }
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(TimeSpan? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(DateTime? p)       { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(DateTimeOffset? p) { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }
		#endif
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Guid? p)           { return p.HasValue? new SqlChars(p.ToString().ToCharArray()): SqlChars.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlString p)       { return (SqlChars)p; }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlByte p)         { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlInt16 p)        { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlInt32 p)        { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlInt64 p)        { return (SqlChars)p.ToSqlString(); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlSingle p)       { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlDouble p)       { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlDecimal p)      { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlMoney p)        { return (SqlChars)p.ToSqlString(); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlBoolean p)      { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlGuid p)         { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlDateTime</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlDateTime p)     { return (SqlChars)p.ToSqlString(); }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(SqlBinary p)       { return p.IsNull? SqlChars.Null: new SqlChars(p.ToString().ToCharArray()); }

		/// <summary>Converts the value from <c>Type</c> to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(Type p)            { return p == null? SqlChars.Null: new SqlChars(p.FullName.ToCharArray()); }
		/// <summary>Converts the value of a specified object to an equivalent <c>SqlChars</c> value.</summary>
		public static SqlChars ToSqlChars(object p)         
		{
			if (p == null || p is DBNull) return SqlChars.Null;

			if (p is SqlChars) return (SqlChars)p;

			// Scalar Types.
			//
			if (p is String)          return ToSqlChars((String)p);
			if (p is Char[])          return ToSqlChars((Char[])p);
			if (p is Byte[])          return ToSqlChars((Byte[])p);

			if (p is SByte)           return ToSqlChars((SByte)p);
			if (p is Int16)           return ToSqlChars((Int16)p);
			if (p is Int32)           return ToSqlChars((Int32)p);
			if (p is Int64)           return ToSqlChars((Int64)p);

			if (p is Byte)            return ToSqlChars((Byte)p);
			if (p is UInt16)          return ToSqlChars((UInt16)p);
			if (p is UInt32)          return ToSqlChars((UInt32)p);
			if (p is UInt64)          return ToSqlChars((UInt64)p);

			if (p is Single)          return ToSqlChars((Single)p);
			if (p is Double)          return ToSqlChars((Double)p);

			if (p is Boolean)         return ToSqlChars((Boolean)p);
			if (p is Decimal)         return ToSqlChars((Decimal)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlChars((SqlString)p);

			if (p is SqlByte)         return ToSqlChars((SqlByte)p);
			if (p is SqlInt16)        return ToSqlChars((SqlInt16)p);
			if (p is SqlInt32)        return ToSqlChars((SqlInt32)p);
			if (p is SqlInt64)        return ToSqlChars((SqlInt64)p);

			if (p is SqlSingle)       return ToSqlChars((SqlSingle)p);
			if (p is SqlDouble)       return ToSqlChars((SqlDouble)p);
			if (p is SqlDecimal)      return ToSqlChars((SqlDecimal)p);
			if (p is SqlMoney)        return ToSqlChars((SqlMoney)p);

			if (p is SqlBoolean)      return ToSqlChars((SqlBoolean)p);
			if (p is SqlBinary)       return ToSqlChars((SqlBinary)p);
			if (p is Type)            return ToSqlChars((Type)p);

			return new SqlChars(ToString(p).ToCharArray());
		}

		#endregion

		#region SqlXml

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(String p)          { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p))); }

		/// <summary>Converts the value from <c>Stream</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(Stream p)          { return p == null? SqlXml.Null: new SqlXml(p); }
		/// <summary>Converts the value from <c>XmlReader</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(XmlReader p)       { return p == null? SqlXml.Null: new SqlXml(p); }
		/// <summary>Converts the value from <c>XmlDocument</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(XmlDocument p)     { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.InnerXml))); }

		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(Char[] p)          { return p == null? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(new string(p)))); }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(Byte[] p)          { return p == null? SqlXml.Null: new SqlXml(new MemoryStream(p)); }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(SqlString p)       { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.Value))); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(SqlChars p)        { return p.IsNull? SqlXml.Null: new SqlXml(new XmlTextReader(new StringReader(p.ToSqlString().Value))); }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(SqlBinary p)       { return p.IsNull? SqlXml.Null: new SqlXml(new MemoryStream(p.Value)); }
		/// <summary>Converts the value from <c>SqlBytes</c> to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(SqlBytes p)        { return p.IsNull? SqlXml.Null: new SqlXml(p.Stream); }

		/// <summary>Converts the value of a specified object to an equivalent <c>SqlXml</c> value.</summary>
		public static SqlXml ToSqlXml(object p)         
		{
			if (p == null || p is DBNull) return SqlXml.Null;

			if (p is SqlXml) return (SqlXml)p;

			// Scalar Types.
			//
			if (p is String)          return ToSqlXml((String)p);

			if (p is XmlDocument)     return ToSqlXml((XmlDocument)p);

			if (p is Char[])          return ToSqlXml((Char[])p);
			if (p is Byte[])          return ToSqlXml((Byte[])p);

			// SqlTypes
			//
			if (p is SqlString)       return ToSqlXml((SqlString)p);
			if (p is SqlChars)        return ToSqlXml((SqlChars)p);
			if (p is SqlBinary)       return ToSqlXml((SqlBinary)p);
			if (p is SqlBytes)        return ToSqlXml((SqlBytes)p);

			throw CreateInvalidCastException(p.GetType(), typeof(SqlXml));
		}

		#endregion

		#endregion

		#region Other Types

		#region Type

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(String p)          { return p == null?       null: Type.GetType(p);                   }
		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(Char[] p)          { return p == null?       null: Type.GetType(new string(p));       }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(Byte[] p)          { return p == null?       null: Type.GetTypeFromCLSID(ToGuid(p));  }
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(Guid p)            { return p == Guid.Empty? null: Type.GetTypeFromCLSID(p);          }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(Guid? p)           { return p.HasValue? Type.GetTypeFromCLSID(p.Value): null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(SqlString p)       { return p.IsNull       ? null: Type.GetType(p.Value);             }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(SqlChars p)        { return p.IsNull       ? null: Type.GetType(new string(p.Value)); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(SqlGuid p)         { return p.IsNull       ? null: Type.GetTypeFromCLSID(p.Value);    }

		/// <summary>Converts the value of a specified object to an equivalent <c>Type</c> value.</summary>
		public static Type ToType(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Type) return (Type)p;

			// Scalar Types.
			//
			if (p is String)          return ToType((String)p);
			if (p is Char[])          return ToType((Char[])p);
			if (p is Byte[])          return ToType((Byte[])p);
			if (p is Guid)            return ToType((Guid)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToType((SqlString)p);
			if (p is SqlChars)        return ToType((SqlChars)p);
			if (p is SqlGuid)         return ToType((SqlGuid)p);

			throw CreateInvalidCastException(p.GetType(), typeof(Type));
		}

		#endregion

		#region Stream

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(Guid p)            { return p == Guid.Empty? Stream.Null: new MemoryStream(p.ToByteArray()); }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(Byte[] p)          { return p == null? Stream.Null: new MemoryStream(p); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(Guid? p)           { return p.HasValue? new MemoryStream(p.Value.ToByteArray()): Stream.Null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBytes</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(SqlBytes p)        { return p.IsNull? Stream.Null: p.Stream;                  }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(SqlBinary p)       { return p.IsNull? Stream.Null: new MemoryStream(p.Value); }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(SqlGuid p)         { return p.IsNull? Stream.Null: new MemoryStream(p.Value.ToByteArray()); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Stream</c> value.</summary>
		public static Stream ToStream(object p)         
		{
			if (p == null || p is DBNull) return Stream.Null;

			if (p is Stream) return (Stream)p;

			// Scalar Types.
			//
			if (p is Guid)            return ToStream((Guid)p);
			if (p is Byte[])          return ToStream((Byte[])p);

			// SqlTypes
			//
			if (p is SqlBytes)        return ToStream((SqlBytes)p);
			if (p is SqlBinary)       return ToStream((SqlBinary)p);
			if (p is SqlGuid)         return ToStream((SqlGuid)p);

			throw CreateInvalidCastException(p.GetType(), typeof(Stream));
		}

		#endregion

		#region Byte[]

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(String p)          { return p == null? null: System.Text.Encoding.UTF8.GetBytes(p); }
		/// <summary>Converts the value from <c>Byte</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Byte p)            { return new byte[]{p}; }
		/// <summary>Converts the value from <c>SByte</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(SByte p)           { return new byte[]{checked((Byte)p)}; }
		/// <summary>Converts the value from <c>Decimal</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Decimal p)        
		{
			int[]  bits  = Decimal.GetBits(p);
			Byte[] bytes = new Byte[Buffer.ByteLength(bits)];

			Buffer.BlockCopy(bits, 0, bytes, 0, bytes.Length);
			return bytes;
		}
		/// <summary>Converts the value from <c>Int16</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int16 p)           { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>Int32</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int32 p)           { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>Int64</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int64 p)           { return BitConverter.GetBytes(p); }

		/// <summary>Converts the value from <c>UInt16</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt16 p)          { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>UInt32</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt32 p)          { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>UInt64</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt64 p)          { return BitConverter.GetBytes(p); }

		/// <summary>Converts the value from <c>Single</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Single p)          { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>Double</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Double p)          { return BitConverter.GetBytes(p); }

		/// <summary>Converts the value from <c>Boolean</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Boolean p)         { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>Char</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Char p)            { return BitConverter.GetBytes(p); }
		/// <summary>Converts the value from <c>DateTime</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(DateTime p)        { return ToByteArray(p.ToBinary()); }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(DateTimeOffset p)  { return ToByteArray(p.LocalDateTime.ToBinary()); }
		#endif
		/// <summary>Converts the value from <c>TimeSpan</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(TimeSpan p)        { return ToByteArray(p.Ticks); }
		/// <summary>Converts the value from <c>Stream</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Stream p)         
		{
			if (p == null)         return null;
			if (p is MemoryStream) return ((MemoryStream)p).ToArray();

			long   position = p.Seek(0, SeekOrigin.Begin);
			Byte[] bytes = new Byte[p.Length];
			p.Read(bytes, 0, bytes.Length);
			p.Position = position;

			return bytes;
		}
		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Char[] p)         
		{
			Byte[] bytes = new Byte[Buffer.ByteLength(p)];

			Buffer.BlockCopy(p, 0, bytes, 0, bytes.Length);
			return bytes;
		}
		/// <summary>Converts the value from <c>Guid</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Guid p)            { return p == Guid.Empty? null: p.ToByteArray(); }

		// Nullable Types.
		// 
		/// <summary>Converts the value from <c>SByte?</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(SByte? p)          { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Int16?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int16? p)          { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Int32?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int32? p)          { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Int64?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Int64? p)          { return p.HasValue? ToByteArray(p.Value): null; }

		/// <summary>Converts the value from <c>Byte?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Byte? p)           { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>UInt16?</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt16? p)         { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>UInt32?</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt32? p)         { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>UInt64?</c> to an equivalent <c>Byte[]</c> value.</summary>
		[CLSCompliant(false)]
		public static Byte[] ToByteArray(UInt64? p)         { return p.HasValue? ToByteArray(p.Value): null; }

		/// <summary>Converts the value from <c>Single?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Single? p)         { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Double?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Double? p)         { return p.HasValue? ToByteArray(p.Value): null; }

		/// <summary>Converts the value from <c>Boolean?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Boolean? p)        { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Decimal?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Decimal? p)        { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Char?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Char? p)           { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>DateTime?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(DateTime? p)       { return p.HasValue? ToByteArray(p.Value): null; }
		#if FW3
		/// <summary>Converts the value from <c>DateTimeOffset?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(DateTimeOffset? p) { return p.HasValue? ToByteArray(p.Value): null; }
		#endif
		/// <summary>Converts the value from <c>TimeSpan?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(TimeSpan? p)       { return p.HasValue? ToByteArray(p.Value): null; }
		/// <summary>Converts the value from <c>Guid?</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(Guid? p)           { return p.HasValue? ToByteArray(p.Value): null; }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlBinary p)       { return p.IsNull? null: p.Value; }
		/// <summary>Converts the value from <c>SqlBytes</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlBytes p)        { return p.IsNull? null: p.Value; }
		/// <summary>Converts the value from <c>SqlGuid</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlGuid p)         { return p.IsNull? null: p.ToByteArray(); }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlString p)       { return p.IsNull? null: ToByteArray(p.Value); }

		/// <summary>Converts the value from <c>SqlByte</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlByte p)         { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlInt16</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlInt16 p)        { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlInt32</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlInt32 p)        { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlInt64</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlInt64 p)        { return p.IsNull? null: ToByteArray(p.Value); }

		/// <summary>Converts the value from <c>SqlSingle</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlSingle p)       { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlDouble</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlDouble p)       { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlDecimal</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlDecimal p)      { return p.IsNull? null: ToByteArray(p.Value); }
		/// <summary>Converts the value from <c>SqlMoney</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlMoney p)        { return p.IsNull? null: ToByteArray(p.Value); }

		/// <summary>Converts the value from <c>SqlBoolean</c> to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(SqlBoolean p)      { return p.IsNull? null: ToByteArray(p.Value); }

		/// <summary>Converts the value of a specified object to an equivalent <c>Byte[]</c> value.</summary>
		public static Byte[] ToByteArray(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Byte[]) return (Byte[])p;

			// Scalar Types.
			//
			if (p is String)          return ToByteArray((String)p);
			if (p is Byte)            return ToByteArray((Byte)p);
			if (p is SByte)           return ToByteArray((SByte)p);
			if (p is Decimal)         return ToByteArray((Decimal)p);
			if (p is Int16)           return ToByteArray((Int16)p);
			if (p is Int32)           return ToByteArray((Int32)p);
			if (p is Int64)           return ToByteArray((Int64)p);

			if (p is UInt16)          return ToByteArray((UInt16)p);
			if (p is UInt32)          return ToByteArray((UInt32)p);
			if (p is UInt64)          return ToByteArray((UInt64)p);

			if (p is Single)          return ToByteArray((Single)p);
			if (p is Double)          return ToByteArray((Double)p);

			if (p is Boolean)         return ToByteArray((Boolean)p);
			if (p is DateTime)        return ToByteArray((DateTime)p);
			#if FW3
			if (p is DateTimeOffset)  return ToByteArray((DateTimeOffset)p);
			#endif
			if (p is TimeSpan)        return ToByteArray((TimeSpan)p);
			if (p is Stream)          return ToByteArray((Stream)p);
			if (p is Char[])          return ToByteArray((Char[])p);
			if (p is Guid)            return ToByteArray((Guid)p);

			// SqlTypes
			//
			if (p is SqlBinary)       return ToByteArray((SqlBinary)p);
			if (p is SqlBytes)        return ToByteArray((SqlBytes)p);
			if (p is SqlGuid)         return ToByteArray((SqlGuid)p);
			if (p is SqlString)       return ToByteArray((SqlString)p);

			if (p is SqlByte)         return ToByteArray((SqlByte)p);
			if (p is SqlInt16)        return ToByteArray((SqlInt16)p);
			if (p is SqlInt32)        return ToByteArray((SqlInt32)p);
			if (p is SqlInt64)        return ToByteArray((SqlInt64)p);

			if (p is SqlSingle)       return ToByteArray((SqlSingle)p);
			if (p is SqlDouble)       return ToByteArray((SqlDouble)p);
			if (p is SqlDecimal)      return ToByteArray((SqlDecimal)p);
			if (p is SqlMoney)        return ToByteArray((SqlMoney)p);

			if (p is SqlBoolean)      return ToByteArray((SqlBoolean)p);

			throw CreateInvalidCastException(p.GetType(), typeof(Byte[]));
		}

		#endregion

		#region Char[]

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>Char[]</c> value.</summary>
		public static Char[] ToCharArray(String p)          { return p == null? null: p.ToCharArray(); }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>Char[]</c> value.</summary>
		public static Char[] ToCharArray(SqlString p)       { return p.IsNull? null: p.Value.ToCharArray(); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>Char[]</c> value.</summary>
		public static Char[] ToCharArray(SqlChars p)        { return p.IsNull? null: p.Value; }

		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>Char[]</c> value.</summary>
		public static Char[] ToCharArray(Byte[] p)         
		{
				if (p == null) return null;

				Char[] chars = new Char[p.Length / sizeof(Char)];

				Buffer.BlockCopy(p, 0, chars, 0, p.Length);
				return chars;
			
		}
		/// <summary>Converts the value of a specified object to an equivalent <c>Char[]</c> value.</summary>
		public static Char[] ToCharArray(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is Char[]) return (Char[])p;

			// Scalar Types.
			//
			if (p is String)          return ToCharArray((String)p);

			// SqlTypes
			//
			if (p is SqlString)       return ToCharArray((SqlString)p);
			if (p is SqlChars)        return ToCharArray((SqlChars)p);
			if (p is Byte[])          return ToCharArray((Byte[])p);

			return ToString(p).ToCharArray();
		}

		#endregion

		#region XmlReader

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(String p)          { return p == null? null: new XmlTextReader(new StringReader(p)); }

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlXml</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(SqlXml p)          { return p.IsNull? null: p.CreateReader(); }
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(SqlString p)       { return p.IsNull? null: new XmlTextReader(new StringReader(p.Value)); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(SqlChars p)        { return p.IsNull? null: new XmlTextReader(new StringReader(p.ToSqlString().Value)); }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(SqlBinary p)       { return p.IsNull? null: new XmlTextReader(new MemoryStream(p.Value)); }

		// Other Types.
		// 
		/// <summary>Converts the value from <c>Stream</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(Stream p)          { return p == null? null: new XmlTextReader(p); }
		/// <summary>Converts the value from <c>TextReader</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(TextReader p)      { return p == null? null: new XmlTextReader(p); }
		/// <summary>Converts the value from <c>XmlDocument</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(XmlDocument p)     { return p == null? null: new XmlTextReader(new StringReader(p.InnerXml)); }

		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(Char[] p)          { return p == null? null: new XmlTextReader(new StringReader(new string(p))); }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(Byte[] p)          { return p == null? null: new XmlTextReader(new MemoryStream(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>XmlReader</c> value.</summary>
		public static XmlReader ToXmlReader(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is XmlReader) return (XmlReader)p;

			// Scalar Types.
			//
			if (p is String)          return ToXmlReader((String)p);

			// SqlTypes
			//
			if (p is SqlXml)          return ToXmlReader((SqlXml)p);
			if (p is SqlString)       return ToXmlReader((SqlString)p);
			if (p is SqlChars)        return ToXmlReader((SqlChars)p);
			if (p is SqlBinary)       return ToXmlReader((SqlBinary)p);

			// Other Types.
			//
			if (p is XmlDocument)     return ToXmlReader((XmlDocument)p);

			if (p is Char[])          return ToXmlReader((Char[])p);
			if (p is Byte[])          return ToXmlReader((Byte[])p);

			throw CreateInvalidCastException(p.GetType(), typeof(XmlReader));
		}

		#endregion

		#region XmlDocument

		// Scalar Types.
		// 
		/// <summary>Converts the value from <c>String</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(String p)         
		{
			if (string.IsNullOrEmpty(p)) return null;

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(p);
			return doc;
		}

		// SqlTypes
		// 
		/// <summary>Converts the value from <c>SqlString</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(SqlString p)       { return p.IsNull? null: ToXmlDocument(p.Value); }
		/// <summary>Converts the value from <c>SqlXml</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(SqlXml p)          { return p.IsNull? null: ToXmlDocument(p.Value); }
		/// <summary>Converts the value from <c>SqlChars</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(SqlChars p)        { return p.IsNull? null: ToXmlDocument(p.ToSqlString().Value); }
		/// <summary>Converts the value from <c>SqlBinary</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(SqlBinary p)       { return p.IsNull? null: ToXmlDocument(new MemoryStream(p.Value)); }

		// Other Types.
		// 
		/// <summary>Converts the value from <c>Stream</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(Stream p)         
		{
					if (p == null) return null;

					XmlDocument doc = new XmlDocument();
					doc.Load(p);
					return doc;
				
		}
		/// <summary>Converts the value from <c>TextReader</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(TextReader p)     
		{
					if (p == null) return null;

					XmlDocument doc = new XmlDocument();
					doc.Load(p);
					return doc;
				
		}
		/// <summary>Converts the value from <c>XmlReader</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(XmlReader p)      
		{
					if (p == null) return null;

					XmlDocument doc = new XmlDocument();
					doc.Load(p);
					return doc;
				
		}

		/// <summary>Converts the value from <c>Char[]</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(Char[] p)          { return p == null || p.Length == 0? null: ToXmlDocument(new string(p)); }
		/// <summary>Converts the value from <c>Byte[]</c> to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(Byte[] p)          { return p == null || p.Length == 0? null: ToXmlDocument(new MemoryStream(p)); }

		/// <summary>Converts the value of a specified object to an equivalent <c>XmlDocument</c> value.</summary>
		public static XmlDocument ToXmlDocument(object p)         
		{
			if (p == null || p is DBNull) return null;

			if (p is XmlDocument) return (XmlDocument)p;

			// Scalar Types.
			//
			if (p is String)          return ToXmlDocument((String)p);

			// SqlTypes
			//
			if (p is SqlChars)        return ToXmlDocument((SqlChars)p);
			if (p is SqlBinary)       return ToXmlDocument((SqlBinary)p);

			// Other Types.
			//

			if (p is Char[])          return ToXmlDocument((Char[])p);
			if (p is Byte[])          return ToXmlDocument((Byte[])p);

			throw CreateInvalidCastException(p.GetType(), typeof(XmlDocument));
		}

		#endregion

		#endregion

		private static Exception CreateInvalidCastException(Type originalType, Type conversionType)
		{
			return new InvalidCastException(string.Format(
				Resources.Convert_InvalidCast, originalType.FullName, conversionType.FullName));
		}
	}
}
