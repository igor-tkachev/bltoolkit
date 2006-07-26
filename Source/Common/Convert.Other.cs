using System;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

namespace BLToolkit.Common
{
	#region Other types

	#region Type

	partial class ConvertPartial<T,P>: IConvertible<Type,P>
	{
		Type IConvertible<Type,P>.From(P p) { return Convert<Type,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<Type,String>, 
		IConvertible<Type,Char[]>, 
		IConvertible<Type,Guid>, 

		// Nullable Types.
		// 
		IConvertible<Type,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<Type,SqlString>, 
		IConvertible<Type,SqlChars>, 
		IConvertible<Type,SqlGuid>, 

		IConvertible<Type,object>

	{
		// Scalar Types.
		// 
		Type IConvertible<Type,String>.     From(String p)      { return p == null      ? null: Type.GetType(p);                   }
		Type IConvertible<Type,Char[]>.     From(Char[] p)      { return p == null      ? null: Type.GetType(new string(p));       }
		Type IConvertible<Type,Guid>.       From(Guid p)        { return p == Guid.Empty? null: Type.GetTypeFromCLSID(p);          }

		// Nullable Types.
		// 
		Type IConvertible<Type,Guid?>.      From(Guid? p)       { return p.HasValue? Type.GetTypeFromCLSID(p.Value): null; }

		// SqlTypes.
		// 
		Type IConvertible<Type,SqlString>.  From(SqlString p)   { return p.IsNull       ? null: Type.GetType(p.Value);             }
		Type IConvertible<Type,SqlChars>.   From(SqlChars p)    { return p.IsNull       ? null: Type.GetType(new string(p.Value)); }
		Type IConvertible<Type,SqlGuid>.    From(SqlGuid p)     { return p.IsNull       ? null: Type.GetTypeFromCLSID(p.Value);    }

		Type IConvertible<Type,object>.     From(object p)     
		{
			if (p == null) return null;

			// Scalar Types.
			//
			if (p is String)      return Convert<Type,String>     .Instance.From((String)p);
			if (p is Char[])      return Convert<Type,Char[]>     .Instance.From((Char[])p);
			if (p is Guid)        return Convert<Type,Guid>       .Instance.From((Guid)p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<Type,Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlString)   return Convert<Type,SqlString>  .Instance.From((SqlString)p);
			if (p is SqlChars)    return Convert<Type,SqlChars>   .Instance.From((SqlChars)p);
			if (p is SqlGuid)     return Convert<Type,SqlGuid>    .Instance.From((SqlGuid)p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region Stream

	partial class ConvertPartial<T,P>: IConvertible<Stream,P>
	{
		Stream IConvertible<Stream,P>.From(P p) { return Convert<Stream,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<Stream,Guid>, 
		IConvertible<Stream,Byte[]>, 

		// Nullable Types.
		// 
		IConvertible<Stream,Guid?>, 

		// SqlTypes.
		// 
		IConvertible<Stream,SqlBytes>, 
		IConvertible<Stream,SqlBinary>, 
		IConvertible<Stream,SqlGuid>, 

		IConvertible<Stream,object>

	{
		// Scalar Types.
		// 
		Stream IConvertible<Stream,Guid>.       From(Guid p)        { return p == Guid.Empty? Stream.Null: new MemoryStream(p.ToByteArray()); }
		Stream IConvertible<Stream,Byte[]>.     From(Byte[] p)      { return p == null? Stream.Null: new MemoryStream(p); }

		// Nullable Types.
		// 
		Stream IConvertible<Stream,Guid?>.      From(Guid? p)       { return p.HasValue? new MemoryStream(p.Value.ToByteArray()): Stream.Null; }

		// SqlTypes.
		// 
		Stream IConvertible<Stream,SqlBytes>.   From(SqlBytes p)    { return p.IsNull? Stream.Null: p.Stream;                  }
		Stream IConvertible<Stream,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? Stream.Null: new MemoryStream(p.Value); }
		Stream IConvertible<Stream,SqlGuid>.    From(SqlGuid p)     { return p.IsNull? Stream.Null: new MemoryStream(p.Value.ToByteArray()); }

		Stream IConvertible<Stream,object>.     From(object p)     
		{
			if (p == null) return Stream.Null;

			// Scalar Types.
			//
			if (p is Guid)        return Convert<Stream,Guid>       .Instance.From((Guid)p);
			if (p is Byte[])      return Convert<Stream,Byte[]>     .Instance.From((Byte[])p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<Stream,Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlBytes)    return Convert<Stream,SqlBytes>   .Instance.From((SqlBytes)p);
			if (p is SqlBinary)   return Convert<Stream,SqlBinary>  .Instance.From((SqlBinary)p);
			if (p is SqlGuid)     return Convert<Stream,SqlGuid>    .Instance.From((SqlGuid)p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region Byte[]

	partial class ConvertPartial<T,P>: IConvertible<Byte[],P>
	{
		Byte[] IConvertible<Byte[],P>.From(P p) { return Convert<Byte[],object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<Byte[],Stream>, 
		IConvertible<Byte[],Guid>, 

		// Nullable Types.
		// 
		IConvertible<Byte[],Guid?>, 

		// SqlTypes.
		// 
		IConvertible<Byte[],SqlBinary>, 
		IConvertible<Byte[],SqlBytes>, 
		IConvertible<Byte[],SqlGuid>, 

		IConvertible<Byte[],object>

	{
		// Scalar Types.
		// 
		Byte[] IConvertible<Byte[],Stream>.     From(Stream p)     
		{
					if (p == null)         return null;
					if (p is MemoryStream) return ((MemoryStream)p).ToArray();

					long   position = p.Seek(0, SeekOrigin.Begin);
					Byte[] bytes = new Byte[p.Length];
					p.Read(bytes, 0, bytes.Length);
					p.Position = position;

					return bytes;
				
		}
		Byte[] IConvertible<Byte[],Guid>.       From(Guid p)        { return p == Guid.Empty? null: p.ToByteArray(); }

		// Nullable Types.
		// 
		Byte[] IConvertible<Byte[],Guid?>.      From(Guid? p)       { return p.HasValue? p.Value.ToByteArray(): null; }

		// SqlTypes.
		// 
		Byte[] IConvertible<Byte[],SqlBinary>.  From(SqlBinary p)   { return p.IsNull? null: p.Value; }
		Byte[] IConvertible<Byte[],SqlBytes>.   From(SqlBytes p)    { return p.IsNull? null: p.Value; }
		Byte[] IConvertible<Byte[],SqlGuid>.    From(SqlGuid p)     { return p.IsNull? null: p.ToByteArray(); }

		Byte[] IConvertible<Byte[],object>.     From(object p)     
		{
			if (p == null) return null;

			// Scalar Types.
			//
			if (p is Stream)      return Convert<Byte[],Stream>     .Instance.From((Stream)p);
			if (p is Guid)        return Convert<Byte[],Guid>       .Instance.From((Guid)p);

			// Nullable Types.
			//
			if (p is Guid?)       return Convert<Byte[],Guid?>      .Instance.From((Guid?)p);

			// SqlTypes.
			//
			if (p is SqlBinary)   return Convert<Byte[],SqlBinary>  .Instance.From((SqlBinary)p);
			if (p is SqlBytes)    return Convert<Byte[],SqlBytes>   .Instance.From((SqlBytes)p);
			if (p is SqlGuid)     return Convert<Byte[],SqlGuid>    .Instance.From((SqlGuid)p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#region Char[]

	partial class ConvertPartial<T,P>: IConvertible<Char[],P>
	{
		Char[] IConvertible<Char[],P>.From(P p) { return Convert<Char[],object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<Char[],String>, 

		// SqlTypes.
		// 
		IConvertible<Char[],SqlString>, 
		IConvertible<Char[],SqlChars>, 

		IConvertible<Char[],object>

	{
		// Scalar Types.
		// 
		Char[] IConvertible<Char[],String>.     From(String p)      { return p == null? null: p.ToCharArray(); }

		// SqlTypes.
		// 
		Char[] IConvertible<Char[],SqlString>.  From(SqlString p)   { return p.IsNull? null: p.Value.ToCharArray(); }
		Char[] IConvertible<Char[],SqlChars>.   From(SqlChars p)    { return p.IsNull? null: p.Value; }

		Char[] IConvertible<Char[],object>.     From(object p)     
		{
			if (p == null) return null;

			// Scalar Types.
			//
			if (p is String)      return Convert<Char[],String>     .Instance.From((String)p);

			// SqlTypes.
			//
			if (p is SqlString)   return Convert<Char[],SqlString>  .Instance.From((SqlString)p);
			if (p is SqlChars)    return Convert<Char[],SqlChars>   .Instance.From((SqlChars)p);

				return Convert<string,object>.Instance.From(p).ToCharArray();
		}
	}

	#endregion

	#region XmlReader

	partial class ConvertPartial<T,P>: IConvertible<XmlReader,P>
	{
		XmlReader IConvertible<XmlReader,P>.From(P p) { return Convert<XmlReader,object>.Instance.From(p); }
	}

	partial class ConvertExplicit<T,P>:
		// Scalar Types.
		// 
		IConvertible<XmlReader,String>, 

		// SqlTypes.
		// 
		IConvertible<XmlReader,SqlXml>, 
		IConvertible<XmlReader,SqlString>, 
		IConvertible<XmlReader,SqlChars>, 
		IConvertible<XmlReader,SqlBinary>, 

		// Other Types.
		// 
		IConvertible<XmlReader,Stream>, 
		IConvertible<XmlReader,TextReader>, 

		IConvertible<XmlReader,Char[]>, 
		IConvertible<XmlReader,Byte[]>, 

		IConvertible<XmlReader,object>

	{
		// Scalar Types.
		// 
		XmlReader IConvertible<XmlReader,String>.     From(String p)      { return p == null? null: XmlReader.Create(new StringReader(p)); }

		// SqlTypes.
		// 
		XmlReader IConvertible<XmlReader,SqlXml>.     From(SqlXml p)      { return p.IsNull? null: p.CreateReader(); }
		XmlReader IConvertible<XmlReader,SqlString>.  From(SqlString p)   { return p.IsNull? null: XmlReader.Create(new StringReader(p.Value)); }
		XmlReader IConvertible<XmlReader,SqlChars>.   From(SqlChars p)    { return p.IsNull? null: XmlReader.Create(new StringReader(p.ToSqlString().Value)); }
		XmlReader IConvertible<XmlReader,SqlBinary>.  From(SqlBinary p)   { return p.IsNull? null: XmlReader.Create(new MemoryStream(p.Value)); }

		// Other Types.
		// 
		XmlReader IConvertible<XmlReader,Stream>.     From(Stream p)      { return p == null? null: XmlReader.Create(p); }
		XmlReader IConvertible<XmlReader,TextReader>. From(TextReader p)  { return p == null? null: XmlReader.Create(p); }

		XmlReader IConvertible<XmlReader,Char[]>.     From(Char[] p)      { return p == null? null: XmlReader.Create(new StringReader(new string(p))); }
		XmlReader IConvertible<XmlReader,Byte[]>.     From(Byte[] p)      { return p == null? null: XmlReader.Create(new MemoryStream(p)); }

		XmlReader IConvertible<XmlReader,object>.     From(object p)     
		{
			if (p == null) return null;

			// Scalar Types.
			//
			if (p is String)      return Convert<XmlReader,String>     .Instance.From((String)p);

			// SqlTypes.
			//
			if (p is SqlXml)      return Convert<XmlReader,SqlXml>     .Instance.From((SqlXml)p);
			if (p is SqlString)   return Convert<XmlReader,SqlString>  .Instance.From((SqlString)p);
			if (p is SqlChars)    return Convert<XmlReader,SqlChars>   .Instance.From((SqlChars)p);
			if (p is SqlBinary)   return Convert<XmlReader,SqlBinary>  .Instance.From((SqlBinary)p);

			// Other Types.
			//
			if (p is Stream)      return Convert<XmlReader,Stream>     .Instance.From((Stream)p);
			if (p is TextReader)  return Convert<XmlReader,TextReader> .Instance.From((TextReader)p);

			if (p is Char[])      return Convert<XmlReader,Char[]>     .Instance.From((Char[])p);
			if (p is Byte[])      return Convert<XmlReader,Byte[]>     .Instance.From((Byte[])p);

			throw new InvalidCastException(string.Format(
				"Invalid cast from {0} to {1}", p.GetType().FullName, typeof(T).FullName));
		}
	}

	#endregion

	#endregion


}
