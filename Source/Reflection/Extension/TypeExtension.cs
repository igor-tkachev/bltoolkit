using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace BLToolkit.Reflection.Extension
{
	public class TypeExtension
	{
		#region Consts

		public static class NodeName
		{
			public const string Type   = "Type";
			public const string Member = "Member";
		}

		public static class AttrName
		{
			public const string Name        = "Name";
		}

		public static class ValueName
		{
			public const char   Delimiter   = '-';
			public const string Value       = "Value";
			public const string Type        = "Type";
			public const string ValueType   = "Value-Type";
			public const string TypePostfix = "-Type";
		}

		#endregion

		#region Public Instance Members

		public TypeExtension()
		{
			_members    = new MemberExtensionCollection();
			_attributes = new AttributeNameCollection();
		}

		private TypeExtension(
			MemberExtensionCollection  members,
			AttributeNameCollection    attributes)
		{
			_members    = members;
			_attributes = attributes;
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

		private readonly MemberExtensionCollection _members;
		public           MemberExtensionCollection  Members
		{
			get { return _members; }
		}

		private readonly AttributeNameCollection _attributes;
		public           AttributeNameCollection  Attributes
		{
			get { return _attributes; }
		}

		private static readonly TypeExtension _null = new TypeExtension(MemberExtensionCollection.Null, AttributeNameCollection.Null);
		public           static TypeExtension  Null
		{
			get { return _null; }
		}

		#endregion

		#region Conversion

		public static bool ToBoolean(object value,  bool defaultValue)
		{
			if (value == null)
				return defaultValue;

			return ToBoolean(value);
		}

		public static bool ToBoolean(object value)
		{
			if (value != null)
			{
				if (value is bool)
					return (bool)value;

				string s = value as string;

				if (s != null)
				{
					if (s == "1")
						return true;

					s = s.ToLower();

					if (s == "true" || s == "yes" || s == "on")
						return true;
				}

				return Convert.ToBoolean(value);
			}

			return false;
		}

		public static object ChangeType(object value, Type type)
		{
			if (value == null || type == value.GetType())
				return value;

			if (type == typeof(string))
				return value.ToString();

			if (type == typeof(bool))
				return ToBoolean(value);

			if (type.IsEnum)
			{
				if (value is string)
					return Enum.Parse(type, value.ToString());
			}

			return Convert.ChangeType(value, type);
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

				if (embedded && stream == null)
				{
					string[] names = assembly.GetManifestResourceNames();

					// Prepend file anme with a dot to avoid partial name matching.
					//
					xmlFile = "." + xmlFile;

					foreach (string name in names)
					{
						if (name.EndsWith(xmlFile))
						{
							stream = assembly.GetManifestResourceStream(name);
							break;
						}
					}
				}

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

		public static TypeExtension GetTypeExtension(Type type, ExtensionList typeExtensions)
		{
			object[] attrs = type.GetCustomAttributes(typeof(TypeExtensionAttribute), true);

			if (attrs != null && attrs.Length != 0)
			{
				TypeExtensionAttribute attr = (TypeExtensionAttribute)attrs[0];

				if (!string.IsNullOrEmpty(attr.FileName))
					typeExtensions = GetExtenstions(attr.FileName, type.Assembly);

				if (typeExtensions != null && !string.IsNullOrEmpty(attr.TypeName))
					return typeExtensions[attr.TypeName];
			}

			return typeExtensions != null? typeExtensions[type]: Null;
		}

		#endregion

		#region Private Static Members

		private static ExtensionList CreateTypeInfo(XmlDocument doc)
		{
			ExtensionList list = new ExtensionList();

			foreach (XmlNode typeNode in doc.DocumentElement.ChildNodes)
				if (typeNode.LocalName == NodeName.Type)
					list.Add(ParseType(typeNode));

			return list;
		}

		private static TypeExtension ParseType(XmlNode typeNode)
		{
			TypeExtension ext = new TypeExtension();

			if (typeNode.Attributes != null)
			{
				foreach (XmlAttribute attr in typeNode.Attributes)
				{
					if (attr.LocalName == AttrName.Name)
						ext.Name = attr.Value;
					else
						ext.Attributes.Add(attr.LocalName, attr.Value);
				}
			}

			foreach (XmlNode node in typeNode.ChildNodes)
			{
				if (node.LocalName == NodeName.Member)
					ext.Members.Add(ParseMember(node));
				else
					ext.Attributes.Add(ParseAttribute(node));
			}

			return ext;
		}

		private static MemberExtension ParseMember(XmlNode memberNode)
		{
			MemberExtension ext = new MemberExtension();

			if (memberNode.Attributes != null)
			{
				foreach (XmlAttribute attr in memberNode.Attributes)
				{
					if (attr.LocalName == AttrName.Name)
						ext.Name = attr.Value;
					else
						ext.Attributes.Add(attr.LocalName, attr.Value);
				}
			}

			foreach (XmlNode node in memberNode.ChildNodes)
				ext.Attributes.Add(ParseAttribute(node));

			return ext;
		}

		private static AttributeExtension ParseAttribute(XmlNode attributeNode)
		{
			AttributeExtension ext = new AttributeExtension();

			ext.Name = attributeNode.LocalName;

			if (attributeNode.Attributes != null)
			{
				ext.Values.Add(ValueName.Value, attributeNode.InnerText);

				foreach (XmlAttribute attr in attributeNode.Attributes)
				{
					if (attr.LocalName == ValueName.Type)
						ext.Values.Add(ValueName.ValueType, attr.Value);
					else
						ext.Values.Add(attr.LocalName, attr.Value);
				}
			}

			foreach (XmlNode node in attributeNode.ChildNodes)
				ext.Attributes.Add(ParseAttribute(node));

			return ext;
		}

		#endregion
	}
}
