
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

		// A comment for SByte.
		// 
		#region SByte

		public static SByte ToSByte(String       p) { return Convert.ToSByte(p); }
		public static SByte ToSByte(object       p) { return Convert.ToSByte(p); }

		#endregion


		// A comment for String.
		// 
		#region String

		public static String ToString(SByte        p) { return p.ToString(); }

		// A meaningful comment for String-to-Byte converter.
		// 
		public static String ToString(Byte         p) { return p.ToString(); }
		public static String ToString(object       p) { return p.ToString(); }

		#endregion

		#region Int32

		public static Int32 ToInt32(SByte        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Int16        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(Int64        p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt16       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt32       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(UInt64       p) { return Convert.ToInt32(p); }
		public static Int32 ToInt32(object       p) { return Convert.ToInt32(p); }

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
