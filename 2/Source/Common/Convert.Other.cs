using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	public static partial class Convert<T,P>
	{
		#region Type

		// Scalar Types.
		//
		sealed class T_S         : CB<Type,String>     { public override Type C(String p)      { return p == null      ? null: Type.GetType(p);                   } }
		sealed class T_AC        : CB<Type,Char[]>     { public override Type C(Char[] p)      { return p == null      ? null: Type.GetType(new string(p));       } }
		sealed class T_G         : CB<Type,Guid>       { public override Type C(Guid p)        { return p == Guid.Empty? null: Type.GetTypeFromCLSID(p);          } }

		// Nullable Types.
		//
		sealed class T_NG        : CB<Type,Guid?>      { public override Type C(Guid? p)       { return p.HasValue? Type.GetTypeFromCLSID(p.Value): null; } }

		// SqlTypes.
		//
		sealed class T_dbS       : CB<Type,SqlString>  { public override Type C(SqlString p)   { return p.IsNull       ? null: Type.GetType(p.Value);             } }
		sealed class T_dbAC      : CB<Type,SqlChars>   { public override Type C(SqlChars p)    { return p.IsNull       ? null: Type.GetType(new string(p.Value)); } }
		sealed class T_dbG       : CB<Type,SqlGuid>    { public override Type C(SqlGuid p)     { return p.IsNull       ? null: Type.GetTypeFromCLSID(p.Value);    } }

		sealed class T_O         : CB<Type ,object>    { public override Type C(object p)  
			{
				if (p == null) return null;

				// Scalar Types.
				//
				if (p is String)      return Convert<Type,String>     .I.C((String)p);
				if (p is Char[])      return Convert<Type,Char[]>     .I.C((Char[])p);
				if (p is Guid)        return Convert<Type,Guid>       .I.C((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Type,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<Type,SqlString>  .I.C((SqlString)p);
				if (p is SqlChars)    return Convert<Type,SqlChars>   .I.C((SqlChars)p);
				if (p is SqlGuid)     return Convert<Type,SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetTypeConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new T_S         ());
			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new T_AC        ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new T_G         ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new T_NG        ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new T_dbS       ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new T_dbAC      ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new T_dbG       ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new T_O         ());

			return (CB<T, P>)(object)Convert<Type, object>.I;
		}

		#endregion

		#region Stream


		// Scalar Types.
		//
		sealed class IOS_G       : CB<Stream,Guid>       { public override Stream C(Guid p)        { return p == Guid.Empty? Stream.Null: new MemoryStream(p.ToByteArray()); } }
		sealed class IOS_AU8     : CB<Stream,Byte[]>     { public override Stream C(Byte[] p)      { return p == null? Stream.Null: new MemoryStream(p); } }

		// Nullable Types.
		//
		sealed class IOS_NG      : CB<Stream,Guid?>      { public override Stream C(Guid? p)       { return p.HasValue? new MemoryStream(p.Value.ToByteArray()): Stream.Null; } }

		// SqlTypes.
		//
		sealed class IOS_dbAU8   : CB<Stream,SqlBytes>   { public override Stream C(SqlBytes p)    { return p.IsNull? Stream.Null: p.Stream;                  } }
		sealed class IOS_dbBin   : CB<Stream,SqlBinary>  { public override Stream C(SqlBinary p)   { return p.IsNull? Stream.Null: new MemoryStream(p.Value); } }
		sealed class IOS_dbG     : CB<Stream,SqlGuid>    { public override Stream C(SqlGuid p)     { return p.IsNull? Stream.Null: new MemoryStream(p.Value.ToByteArray()); } }

		sealed class IOS_O         : CB<Stream ,object>    { public override Stream C(object p)  
			{
				if (p == null) return Stream.Null;

				// Scalar Types.
				//
				if (p is Guid)        return Convert<Stream,Guid>       .I.C((Guid)p);
				if (p is Byte[])      return Convert<Stream,Byte[]>     .I.C((Byte[])p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Stream,Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBytes)    return Convert<Stream,SqlBytes>   .I.C((SqlBytes)p);
				if (p is SqlBinary)   return Convert<Stream,SqlBinary>  .I.C((SqlBinary)p);
				if (p is SqlGuid)     return Convert<Stream,SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetStreamConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new IOS_G       ());
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new IOS_AU8     ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new IOS_NG      ());

			// SqlTypes.
			//
			if (t == typeof(SqlBytes))    return (CB<T, P>)(object)(new IOS_dbAU8   ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new IOS_dbBin   ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new IOS_dbG     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new IOS_O       ());

			return (CB<T, P>)(object)Convert<Stream, object>.I;
		}

		#endregion

		#region XmlReader

		static readonly XmlReader NullXmlReader = null;

		// Scalar Types.
		//
		sealed class Xml_S       : CB<XmlReader,String>     { public override XmlReader C(String p)      { return p == null? NullXmlReader: XmlReader.Create(new StringReader(p)); } }

		sealed class Xml_IOS     : CB<XmlReader,Stream>     { public override XmlReader C(Stream p)      { return p == null? NullXmlReader: XmlReader.Create(p); } }
		sealed class Xml_TR      : CB<XmlReader,TextReader> { public override XmlReader C(TextReader p)  { return p == null? NullXmlReader: XmlReader.Create(p); } }

		sealed class Xml_AC      : CB<XmlReader,Char[]>     { public override XmlReader C(Char[] p)      { return p == null? NullXmlReader: XmlReader.Create(new StringReader(new string(p))); } }
		sealed class Xml_AU8     : CB<XmlReader,Byte[]>     { public override XmlReader C(Byte[] p)      { return p == null? NullXmlReader: XmlReader.Create(new MemoryStream(p)); } }

		// SqlTypes.
		//
		sealed class Xml_dbXml   : CB<XmlReader, SqlXml> { public override XmlReader C(SqlXml p)         { return p.IsNull? NullXmlReader: p.CreateReader(); } }
		sealed class Xml_dbS     : CB<XmlReader, SqlString> { public override XmlReader C(SqlString p)   { return p.IsNull? NullXmlReader: XmlReader.Create(new StringReader(p.Value)); } }
		sealed class Xml_dbAC    : CB<XmlReader,SqlChars>   { public override XmlReader C(SqlChars p)    { return p.IsNull? NullXmlReader: XmlReader.Create(new StringReader(p.ToSqlString().Value)); } }
		sealed class Xml_dbBin   : CB<XmlReader,SqlBinary>  { public override XmlReader C(SqlBinary p)   { return p.IsNull? NullXmlReader: XmlReader.Create(new MemoryStream(p.Value)); } }

		sealed class Xml_O       : CB<XmlReader ,object>    { public override XmlReader C(object p)  
			{
				if (p == null) return NullXmlReader;

				// Scalar Types.
				//
				if (p is String)      return Convert<XmlReader,String>     .I.C((String)p);

				if (p is Stream)      return Convert<XmlReader,Stream>     .I.C((Stream)p);
				if (p is TextReader)  return Convert<XmlReader,TextReader> .I.C((TextReader)p);

				if (p is Char[])      return Convert<XmlReader,Char[]>     .I.C((Char[])p);
				if (p is Byte[])      return Convert<XmlReader,Byte[]>     .I.C((Byte[])p);

				// SqlTypes.
				//
				if (p is SqlXml)      return Convert<XmlReader, SqlXml>    .I.C((SqlXml)p);
				if (p is SqlString)   return Convert<XmlReader, SqlString> .I.C((SqlString)p);
				if (p is SqlChars)    return Convert<XmlReader,SqlChars>   .I.C((SqlChars)p);
				if (p is SqlBinary)   return Convert<XmlReader,SqlBinary>  .I.C((SqlBinary)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetXmlReaderConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new Xml_S       ());

			if (t == typeof(Stream))      return (CB<T, P>)(object)(new Xml_IOS     ());
			if (t == typeof(TextReader))  return (CB<T, P>)(object)(new Xml_TR      ());

			if (t == typeof(Char[]))      return (CB<T, P>)(object)(new Xml_AC      ());
			if (t == typeof(Byte[]))      return (CB<T, P>)(object)(new Xml_AU8     ());

			// SqlTypes.
			//
			if (t == typeof(SqlXml))      return (CB<T, P>)(object)(new Xml_dbXml   ());
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new Xml_dbS     ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new Xml_dbAC    ());
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new Xml_dbBin   ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new Xml_O       ());

			return (CB<T, P>)(object)Convert<XmlReader, object>.I;
		}

		#endregion

		#region Byte[]

		static readonly Byte[] NullBytes = null;

		// Scalar Types.
		//
		sealed class AU8_IOS     : CB<Byte[],Stream>     { public override Byte[] C(Stream p)     
			{
				if (p == null)         return NullBytes;
				if (p is MemoryStream) return ((MemoryStream)p).ToArray();

				long   position = p.Seek(0, SeekOrigin.Begin);
				Byte[] bytes = new Byte[p.Length];
				p.Read(bytes, 0, bytes.Length);
				p.Position = position;

				return bytes;
			} }
		sealed class AU8_G       : CB<Byte[],Guid>       { public override Byte[] C(Guid p)        { return p == Guid.Empty? NullBytes: p.ToByteArray(); } }

		// Nullable Types.
		//
		sealed class AU8_NG      : CB<Byte[],Guid?>      { public override Byte[] C(Guid? p)       { return p.HasValue? p.Value.ToByteArray(): NullBytes; } }

		// SqlTypes.
		//
		sealed class AU8_dbBin   : CB<Byte[],SqlBinary>  { public override Byte[] C(SqlBinary p)   { return p.IsNull? NullBytes: p.Value; } }
		sealed class AU8_dbAU8   : CB<Byte[],SqlBytes>   { public override Byte[] C(SqlBytes p)    { return p.IsNull? NullBytes: p.Value; } }
		sealed class AU8_dbG     : CB<Byte[],SqlGuid>    { public override Byte[] C(SqlGuid p)     { return p.IsNull? NullBytes: p.ToByteArray(); } }

		sealed class AU8_O         : CB<Byte[] ,object>    { public override Byte[] C(object p)  
			{
				if (p == null) return NullBytes;

				// Scalar Types.
				//
				if (p is Stream)      return Convert<Byte[],Stream>     .I.C((Stream)p);
				if (p is Guid)        return Convert<Byte[],Guid>       .I.C((Guid)p);

				// Nullable Types.
				//
				if (p is Guid)        return Convert<Byte[],Guid>       .I.C((Guid)p);

				// SqlTypes.
				//
				if (p is SqlBinary)   return Convert<Byte[],SqlBinary>  .I.C((SqlBinary)p);
				if (p is SqlBytes)    return Convert<Byte[],SqlBytes>   .I.C((SqlBytes)p);
				if (p is SqlGuid)     return Convert<Byte[],SqlGuid>    .I.C((SqlGuid)p);

				throw new InvalidCastException(string.Format(
					"Invalid cast from {0} to {1}", typeof(P).FullName, typeof(T).FullName));
			} }

		static CB<T, P> GetByteArrayConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(Stream))      return (CB<T, P>)(object)(new AU8_IOS     ());
			if (t == typeof(Guid))        return (CB<T, P>)(object)(new AU8_G       ());

			// Nullable Types.
			//
			if (t == typeof(Guid?))       return (CB<T, P>)(object)(new AU8_NG      ());

			// SqlTypes.
			//
			if (t == typeof(SqlBinary))   return (CB<T, P>)(object)(new AU8_dbBin   ());
			if (t == typeof(SqlBytes))    return (CB<T, P>)(object)(new AU8_dbAU8   ());
			if (t == typeof(SqlGuid))     return (CB<T, P>)(object)(new AU8_dbG     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new AU8_O       ());

			return (CB<T, P>)(object)Convert<Byte[], object>.I;
		}

		#endregion

		#region Char[]

		static readonly Char[] NullChars = null;

		// Scalar Types.
		//
		sealed class AC_S        : CB<Char[],String>     { public override Char[] C(String p)      { return p == null? NullChars: p.ToCharArray(); } }

		// SqlTypes.
		//
		sealed class AC_dbS      : CB<Char[],SqlString>  { public override Char[] C(SqlString p)   { return p.IsNull? NullChars: p.Value.ToCharArray(); } }
		sealed class AC_dbAC     : CB<Char[],SqlChars>   { public override Char[] C(SqlChars p)    { return p.IsNull? NullChars: p.Value; } }

		sealed class AC_O         : CB<Char[] ,object>    { public override Char[] C(object p)  
			{
				if (p == null) return NullChars;

				// Scalar Types.
				//
				if (p is String)      return Convert<Char[],String>     .I.C((String)p);

				// SqlTypes.
				//
				if (p is SqlString)   return Convert<Char[],SqlString>  .I.C((SqlString)p);
				if (p is SqlChars)    return Convert<Char[],SqlChars>   .I.C((SqlChars)p);

				return Convert<string,object>.I.C(p).ToCharArray();
			} }

		static CB<T, P> GetCharArrayConverter()
		{
			Type t = typeof(P);


			// Scalar Types.
			//
			if (t == typeof(String))      return (CB<T, P>)(object)(new AC_S        ());

			// SqlTypes.
			//
			if (t == typeof(SqlString))   return (CB<T, P>)(object)(new AC_dbS      ());
			if (t == typeof(SqlChars))    return (CB<T, P>)(object)(new AC_dbAC     ());

			if (t == typeof(object))      return (CB<T, P>)(object)(new AC_O        ());

			return (CB<T, P>)(object)Convert<Char[], object>.I;
		}

		#endregion
	}
}
