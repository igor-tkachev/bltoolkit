using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;

namespace BLToolkit.Reflection.Extension
{
	public class TypeExtension
	{
		#region Public Instance Members

		static TypeExtension()
		{
			_null._members.   _isNull = true;
			_null._attributes._isNull = true;
		}

		private string _name;
		public  string  Name
		{
			get { return _name;  }
			set { _name = value; }
		}

		public MemberExtension this[string memberName]
		{
			get { return _members[memberName]; }
		}

		private MemberExtensionCollection _members = new MemberExtensionCollection();
		public  MemberExtensionCollection  Members
		{
			get { return _members; }
		}

		private AttributeExtensionCollection _attributes = new AttributeExtensionCollection();
		public  AttributeExtensionCollection  Attributes
		{
			get { return _attributes; }
		}

		private static TypeExtension _null = new TypeExtension();
		public  static TypeExtension  Null
		{
			get { return _null; }
		}

		#endregion

		#region Conversion

		public static bool ToBoolean(object value)
		{
			if (value != null)
			{
				if (value is bool)
					return (bool)value;

				string s = value as string;

				if (s != null && (s == "1" || s.ToLower() == "true"))
					return true;

				return Convert.ToBoolean(value);
			}

			return false;
		}

		#endregion

		#region Public Static Members
		
		public static ExtensionList GetExtenstions(string xmlFile)
		{
			return GetExtenstions(xmlFile, Assembly.GetCallingAssembly());
		}

		public static ExtensionList GetExtenstions(string xmlFile, Assembly assembly)
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
					throw new TypeExtensionException(
						string.Format("Could not find file '{0}'.", xmlFile));

				using (stream)
					return GetExtenstions(stream);
			} 
			finally
			{
				if (streamReader != null)
					streamReader.Close();
			}
		}

		public static ExtensionList GetExtenstions(Stream xmlDocStream)
		{
			XmlDocument doc = new XmlDocument();

			doc.Load(xmlDocStream);

			return CreateTypeInfo(doc);
		}

		public static TypeExtension GetTypeExtenstion(Type type, ExtensionList typeExtensions)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TypeExtensionAttribute), true);

			if (attrs != null && attrs.Length != 0)
			{
				TypeExtensionAttribute attr = (TypeExtensionAttribute)attrs[0];

				if (attr.FileName != null && attr.FileName.Length > 0)
					typeExtensions = GetExtenstions(attr.FileName, type.Assembly);

				if (typeExtensions != null && attr.TypeName != null && attr.TypeName.Length > 0)
					return typeExtensions[attr.TypeName];
			}

			return typeExtensions != null? typeExtensions[type]: TypeExtension.Null;
		}

		#endregion

		#region Private Static Members

		private static ExtensionList CreateTypeInfo(XmlDocument doc)
		{
			ExtensionList list = new ExtensionList();

			foreach (XmlNode typeNode in doc.DocumentElement.ChildNodes)
			{
				if (typeNode.Name == "Type")
				{
					TypeExtension ext = ParseType(typeNode);

					if (ext != null)
						list.Add(ext);
				}
			}

			return list;
		}

		private static TypeExtension ParseType(XmlNode typeNode)
		{
			TypeExtension ext = new TypeExtension();

			foreach (XmlNode node in typeNode.ChildNodes)
			{
				if (node.Name == "Member")
				{
					MemberExtension memberExt = ParseMember(node);

					if (memberExt != null)
						ext.Members.Add(memberExt);
				}
				else
				{
					AttributeExtension attributeInfo = ParseAttribute(node);

					if (attributeInfo != null)
						ext.Attributes.Add(attributeInfo);
				}
			}

			if (typeNode.Attributes != null)
			{
				foreach (XmlAttribute attr in typeNode.Attributes)
				{
					if (attr.Name == "Name")
						ext.Name = attr.Value;
					else
					{
						AttributeExtension attributeInfo = new AttributeExtension();

						attributeInfo.Name = attr.Name;
						attributeInfo.Values.Add(attr.Value);

						ext.Attributes.Add(attributeInfo);
					}
				}
			}

			return ext;
		}

		private static MemberExtension ParseMember(XmlNode memberNode)
		{
			MemberExtension ext = new MemberExtension();

			foreach (XmlNode node in memberNode.ChildNodes)
			{
				AttributeExtension attributeExt = ParseAttribute(node);

				if (attributeExt != null)
					ext.Attributes.Add(attributeExt);
			}

			if (memberNode.Attributes != null)
			{
				foreach (XmlAttribute attr in memberNode.Attributes)
				{
					if (attr.Name == "Name")
						ext.Name = attr.Value;
					else
					{
						AttributeExtension attributeExt = new AttributeExtension();

						attributeExt.Name = attr.Name;
						attributeExt.Values.Add(attr.Value);

						ext.Attributes.Add(attributeExt);
					}
				}
			}

			return ext;
		}

		private static AttributeExtension ParseAttribute(XmlNode attributeNode)
		{
			AttributeExtension ext = new AttributeExtension();

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
						ext.Name = attr.Value;
					}
					else
					{
					}
				}
			}

			return ext;
		}

		#endregion
	}
}
