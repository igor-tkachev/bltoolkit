
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
 * Created: 29 ???? 2006 ?.
 * By:      IT-TRUST\paul
 *
 */
#endregion

using System;
using System.Data.SqlTypes;
using System.IO;

namespace BLToolkit.Experimental
{
	public static partial class GenConvert<T,P>
	{
		public delegate T ConvertMethod(P p);

		public static readonly ConvertMethod From = GetConverter();
		public static ConvertMethod GetConverter()
		{
			Type t = typeof(T);

			// Convert to the same type.
			//
			if (t.IsAssignableFrom(typeof(P))) return (ConvertMethod)(object)(GenConvert<P,P>.ConvertMethod)(delegate(P p) { return p; });


			// A comment for SByte.
			// 
			if (t == typeof(SByte       )) return GetSByteConverter();

			// A comment for String.
			// 
			if (t == typeof(String      )) return GetStringConverter();
			if (t == typeof(Int32       )) return GetInt32Converter();

			// A comment for nullable SByte.
			// 
			if (t == typeof(SByte?      )) return GetNullableSByteConverter();

			#region Guid

			if (t == typeof(Guid        )) return GetGuidConverter();

			#endregion

			return delegate(P o) { return (T)Convert.ChangeType(o, typeof(T)); };
		}

		private static ConvertMethod GetSByteConverter()
		{
			Type t = typeof(P);

			if (t == typeof(String      )) return (ConvertMethod)(object)(GenConvert<SByte,String      >.ConvertMethod)(delegate(String       p) { return Convert.ToSByte(p); });

			return (ConvertMethod)(object)(GenConvert<SByte,P>.ConvertMethod)(delegate(P p) { return Convert.ToSByte(p); });
		}

		private static ConvertMethod GetStringConverter()
		{
			Type t = typeof(P);

			if (t == typeof(SByte       )) return (ConvertMethod)(object)(GenConvert<String,SByte       >.ConvertMethod)(delegate(SByte        p) { return p.ToString(); });

			// A meaningful comment for String-to-Byte converter.
			// 
			if (t == typeof(Byte        )) return (ConvertMethod)(object)(GenConvert<String,Byte        >.ConvertMethod)(delegate(Byte         p) { return p.ToString(); });

			return (ConvertMethod)(object)(GenConvert<String,P>.ConvertMethod)(delegate(P p) { return p.ToString(); });
		}

		private static ConvertMethod GetInt32Converter()
		{
			Type t = typeof(P);

			if (t == typeof(SByte       )) return (ConvertMethod)(object)(GenConvert<Int32,SByte       >.ConvertMethod)(delegate(SByte        p) { return Convert.ToInt32(p); });
			if (t == typeof(Int16       )) return (ConvertMethod)(object)(GenConvert<Int32,Int16       >.ConvertMethod)(delegate(Int16        p) { return Convert.ToInt32(p); });
			if (t == typeof(Int64       )) return (ConvertMethod)(object)(GenConvert<Int32,Int64       >.ConvertMethod)(delegate(Int64        p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt16      )) return (ConvertMethod)(object)(GenConvert<Int32,UInt16      >.ConvertMethod)(delegate(UInt16       p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt32      )) return (ConvertMethod)(object)(GenConvert<Int32,UInt32      >.ConvertMethod)(delegate(UInt32       p) { return Convert.ToInt32(p); });
			if (t == typeof(UInt64      )) return (ConvertMethod)(object)(GenConvert<Int32,UInt64      >.ConvertMethod)(delegate(UInt64       p) { return Convert.ToInt32(p); });

			return (ConvertMethod)(object)(GenConvert<Int32,P>.ConvertMethod)(delegate(P p) { return Convert.ToInt32(p); });
		}


		private static ConvertMethod GetNullableSByteConverter()
		{
			Type t = typeof(P);

			if (t == typeof(String      )) return (ConvertMethod)(object)(GenConvert<SByte?,String      >.ConvertMethod)(delegate(String       p) { return p == null? null: (SByte?)Convert.ToSByte(p); });

			return (ConvertMethod)(object)(GenConvert<SByte?,P>.ConvertMethod)(delegate(P p) { return Convert.ToSByte(p); });
		}

		#region Guid

		private static ConvertMethod GetGuidConverter()
		{
			Type t = typeof(P);

			if (t == typeof(String      )) return (ConvertMethod)(object)(GenConvert<Guid,String      >.ConvertMethod)(delegate(String       p) { return new Guid(p);                               });
			if (t == typeof(Guid?       )) return (ConvertMethod)(object)(GenConvert<Guid,Guid?       >.ConvertMethod)(delegate(Guid?        p) { return p.HasValue? p.Value : Guid.Empty;          });
			if (t == typeof(SqlString   )) return (ConvertMethod)(object)(GenConvert<Guid,SqlString   >.ConvertMethod)(delegate(SqlString    p) { return p.IsNull? Guid.Empty: new Guid(p.Value);   });
			if (t == typeof(SqlGuid     )) return (ConvertMethod)(object)(GenConvert<Guid,SqlGuid     >.ConvertMethod)(delegate(SqlGuid      p) { return p.IsNull? Guid.Empty: p.Value;             });
			if (t == typeof(SqlBinary   )) return (ConvertMethod)(object)(GenConvert<Guid,SqlBinary   >.ConvertMethod)(delegate(SqlBinary    p) { return p.IsNull? Guid.Empty: p.ToSqlGuid().Value; });

			return (ConvertMethod)(object)(GenConvert<Guid,P>.ConvertMethod)(delegate(P p)
			{
				if (p == null)
					return Guid.Empty;

				if (p is String      ) return GenConvert<Guid,String      >.From;
				if (p is Guid?       ) return GenConvert<Guid,Guid?       >.From;
				if (p is SqlString   ) return GenConvert<Guid,SqlString   >.From;
				if (p is SqlGuid     ) return GenConvert<Guid,SqlGuid     >.From;
				if (p is SqlBinary   ) return GenConvert<Guid,SqlBinary   >.From;

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			});
		}

		#endregion
	}
}
