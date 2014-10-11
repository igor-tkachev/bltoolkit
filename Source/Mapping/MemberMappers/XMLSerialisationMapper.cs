using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BLToolkit.Mapping.MemberMappers
{
	public class XMLSerialisationMapper : MemberMapper
	{
		public override void SetValue(object o, object value)
		{
			if (value != null) this.MemberAccessor.SetValue(o, this.Deserialize(value.ToString()));
		}

		public override object GetValue(object o)
		{
			return this.XmlSerialize(this.MemberAccessor.GetValue(o));
		}

		string XmlSerialize(object obj)
		{
			if (obj == null) return null;
			XmlSerializer serializer = new XmlSerializer(this.Type);
			MemoryStream ms = new MemoryStream();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.OmitXmlDeclaration = true;
			settings.Indent = true;
			XmlWriter writer = XmlWriter.Create(ms, settings);
			XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
			namespaces.Add(string.Empty, string.Empty);
			serializer.Serialize(writer, obj, namespaces);
			StreamReader r = new StreamReader(ms);
			r.BaseStream.Seek(0, SeekOrigin.Begin);

			return r.ReadToEnd();
		}

		object Deserialize(string txt)
		{
			object retVal = null;
			if (string.IsNullOrEmpty(txt)) return null;

			try
			{
				XmlSerializer ser = new XmlSerializer(this.Type);
				StringReader stringReader = new StringReader(txt);
				XmlTextReader xmlReader = new XmlTextReader(stringReader);
				retVal = ser.Deserialize(xmlReader);
				xmlReader.Close();
				stringReader.Close();
			}
			catch (Exception)
			{
			}
			return retVal;
		}
	}
}
