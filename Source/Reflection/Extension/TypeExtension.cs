using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;

namespace BLToolkit.Reflection.Extension
{
	public class TypeExtension
	{
		#region Consts

		public sealed class NodeName
		{
			private NodeName() {}

			public const string Type   = "Type";
			public const string Member = "Member";
		}

		public sealed class AttrName
		{
			private AttrName() {}

			public const string Name        = "Name";
		}

		public sealed class ValueName
		{
			private ValueName() {}

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

		private TypeExtension(int i)
		{
			_members    = MemberExtensionCollection.Null;
			_attributes = AttributeNameCollection.  Null;
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

		private MemberExtensionCollection _members;
		public  MemberExtensionCollection  Members
		{
			get { return _members; }
		}

		private AttributeNameCollection _attributes;
		public  AttributeNameCollection  Attributes
		{
			get { return _attributes; }
		}

		private static TypeExtension _null = new TypeExtension(0);
		public  static TypeExtension  Null
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

					if (s == "true" || s == "yes")
						return true;
				}

				return Convert.ToBoolean(value);
			}

			return false;
		}

		public static object ChangeType(string value, Type type)
		{
			if (type == typeof(string))
				return value;

			if (type == typeof(bool))
				ToBoolean(value);

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
				if (typeNode.Name == NodeName.Type)
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
					if (attr.Name == AttrName.Name)
						ext.Name = attr.Value;
					else
						ext.Attributes.Add(attr.Name, attr.Value);
				}
			}

			foreach (XmlNode node in typeNode.ChildNodes)
			{
				if (node.Name == NodeName.Member)
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
					if (attr.Name == AttrName.Name)
						ext.Name = attr.Value;
					else
						ext.Attributes.Add(attr.Name, attr.Value);
				}
			}

			foreach (XmlNode node in memberNode.ChildNodes)
				ext.Attributes.Add(ParseAttribute(node));

			return ext;
		}

		private static AttributeExtension ParseAttribute(XmlNode attributeNode)
		{
			AttributeExtension ext = new AttributeExtension();

			ext.Name = attributeNode.Name;

			if (attributeNode.Attributes != null)
			{
				ext.Values.Add(ValueName.Value, attributeNode.InnerText);

				foreach (XmlAttribute attr in attributeNode.Attributes)
				{
					if (attr.Name == ValueName.Type)
						ext.Values.Add(ValueName.ValueType, attr.Value);
					else
						ext.Values.Add(attr.Name, attr.Value);
				}
			}

			return ext;
		}

		#endregion
	}
}
