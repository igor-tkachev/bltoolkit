using System;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace BLToolkit.TypeInfo
{
	public class TypeInfoReader
	{
		#region Public Members
		
		public static TypeInfoCollection GetTypeInfo(string xmlFile)
		{
			return GetTypeInfo(xmlFile, Assembly.GetCallingAssembly());
		}

		public static TypeInfoCollection GetTypeInfo(string xmlFile, Assembly assembly)
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
				
				Stream stream = embedded?
					assembly.GetManifestResourceStream(xmlFile):
					streamReader.BaseStream;

				if (stream == null)
					throw new TypeInfoException(
						string.Format("Could not find file '{0}'.", xmlFile));

				using (stream)
					return GetTypeInfo(stream);
			} 
			finally
			{
				if (streamReader != null)
					streamReader.Close();
			}
		}

		public static TypeInfoCollection GetTypeInfo(Stream xmlDocStream)
		{
			XmlDocument doc = new XmlDocument();

			doc.Load(xmlDocStream);

			return CreateTypeInfo(doc);
		}

		public static TypeInfo GetTypeInfo(Type type, TypeInfoCollection typeInfoCollection)
		{
			object[] customAttrs = type.GetCustomAttributes(typeof(TypeInfoAttribute), true);

			if (customAttrs != null && customAttrs.Length != 0)
			{
				TypeInfoAttribute attr = (TypeInfoAttribute)customAttrs[0];

				if (attr.FileName != null && attr.FileName.Length > 0)
					typeInfoCollection = GetTypeInfo(attr.FileName, type.Assembly);

				if (typeInfoCollection != null && attr.TypeName != null && attr.TypeName.Length > 0)
					return typeInfoCollection[attr.TypeName];
			}

			if (typeInfoCollection != null)
				foreach (TypeInfo info in typeInfoCollection)
					if (info.Name == type.Name || info.Name == type.FullName)
						return info;

			return TypeInfo.Null;
		}

		#endregion

		#region Private Members

		private static TypeInfoCollection CreateTypeInfo(XmlDocument doc)
		{
			TypeInfoCollection list = new TypeInfoCollection();

			foreach (XmlNode typeNode in doc.DocumentElement.ChildNodes)
			{
				if (typeNode.Name == "Type")
				{
					TypeInfo typeInfo = ParseType(typeNode);

					if (typeInfo != null)
						list.Add(typeInfo);
				}
			}

			return list;
		}

		private static TypeInfo ParseType(XmlNode typeNode)
		{
			TypeInfo info = new TypeInfo();

			foreach (XmlNode node in typeNode.ChildNodes)
			{
				if (node.Name == "Member")
				{
					MemberInfo memberInfo = ParseMember(node);

					if (memberInfo != null)
						info.Members.Add(memberInfo);
				}
				else
				{
					AttributeInfo attributeInfo = ParseAttribute(node);

					if (attributeInfo != null)
						info.Attributes.Add(attributeInfo);
				}
			}

			if (typeNode.Attributes != null)
			{
				foreach (XmlAttribute attr in typeNode.Attributes)
				{
					if (attr.Name == "Name")
						info.Name = attr.Value;
					else
					{
						AttributeInfo attributeInfo = new AttributeInfo();

						attributeInfo.Name = attr.Name;
						attributeInfo.Values.Add(attr.Value);

						info.Attributes.Add(attributeInfo);
					}
				}
			}

			return info;
		}

		private static MemberInfo ParseMember(XmlNode memberNode)
		{
			MemberInfo info = new MemberInfo();

			foreach (XmlNode node in memberNode.ChildNodes)
			{
				AttributeInfo attributeInfo = ParseAttribute(node);

				if (attributeInfo != null)
					info.Attributes.Add(attributeInfo);
			}

			if (memberNode.Attributes != null)
			{
				foreach (XmlAttribute attr in memberNode.Attributes)
				{
					if (attr.Name == "Name")
						info.Name = attr.Value;
					else
					{
						AttributeInfo attributeInfo = new AttributeInfo();

						attributeInfo.Name = attr.Name;
						attributeInfo.Values.Add(attr.Value);

						info.Attributes.Add(attributeInfo);
					}
				}
			}

			return info;
		}

		private static AttributeInfo ParseAttribute(XmlNode attributeNode)
		{
			AttributeInfo info = new AttributeInfo();

			/*
			foreach (XmlNode node in attributeNode.ChildNodes)
			{
			}
			*/

			if (attributeNode.Attributes != null)
			{
				foreach (XmlAttribute attr in attributeNode.Attributes)
				{
					if (attr.Name == "Name")
					{
						info.Name = attr.Value;
					}
					else
					{
					}
				}
			}

			return info;
		}

		#endregion
	}
}
