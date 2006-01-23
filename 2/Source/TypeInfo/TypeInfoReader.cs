using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace BLToolkit.TypeInfo
{
	public class TypeInfoReader
	{
		private static XmlDocument GetXmlDocument(string xmlFile, Assembly assembly)
		{
			StreamReader streamReader = null;

			try
			{
				if (File.Exists(xmlFile))
				{
					streamReader = File.OpenText(xmlFile);
				}
				else
				{
					string combinePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, xmlFile);

					if (File.Exists(combinePath))
						streamReader = File.OpenText(combinePath);
				}

				bool embedded = streamReader == null;
				
				Stream stream   = embedded?
					assembly.GetManifestResourceStream(xmlFile):
					streamReader.BaseStream;

				if (stream == null)
					throw new TypeInfoException(
						string.Format("Could not find file '{0}'.", xmlFile));

				using (stream)
					return GetXmlDocument(stream);
			} 
			finally
			{
				if (streamReader != null)
					streamReader.Close();
			}
		}

		private static XmlDocument GetXmlDocument(Stream schemaStream)
		{
#if FW2
			XmlSchemaSet schema = new XmlSchemaSet();
#else
			XmlSchemaCollection schema = new XmlSchemaCollection();
#endif

			string resourceName = "BLToolkit.TypeInfo.TypeInfo.xsd";
			Stream mapSchema    = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

			if (mapSchema == null)
				throw new TypeInfoException(
					string.Format("Cannot load embedded resource '{0}'", resourceName));

			schema.Add(XmlSchema.Read(mapSchema, null));

#if FW2
			XmlReader reader = XmlReader.Create(schemaStream);

			reader.Settings.Schemas.Add(schema);
#else
			XmlValidatingReader reader = new XmlValidatingReader(new XmlTextReader(schemaStream));

			reader.ValidationType = ValidationType.Schema;
			reader.Schemas.Add(schema);
#endif

			XmlDocument doc = new XmlDocument();

			doc.Load(reader);

			return doc;
		}
	}
}
