/*
 * File:    TypeDescriptor.cs
 * Created: 11/11/2003
 * Author:  Igor Tkachev
 *          mailto:it@rsdn.ru
 */

using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace Rsdn.Framework.Data.Mapping
{
	internal class MappingSchema
	{
		private XmlNamespaceManager _xmlNamespace;
		private XmlNode             _xmlSchema;
		private XmlDocument         _xmlDoc;

		public MappingSchema()
		{
		}

		public MappingSchema(XmlNamespaceManager ns, XmlNode n, XmlDocument doc)
		{
			_xmlNamespace = ns;
			_xmlSchema    = n;
			_xmlDoc       = doc;
		}

		private void ThrowConversionException(
			string    attrType,
			string    attrName,
			object    value,
			string    fieldName,
			Exception innerException)
		{
			throw new RsdnMapException(string.Format(
				"Could not convert value '{0}' to the {1} '{2}' attribute of the '{3}' field .", 
				value,
				attrType,
				attrName,
				fieldName),
				innerException);
		}

		public bool HasIgnoreAttribute(object[] attributes, string name)
		{
			if (_xmlSchema != null)
			{
				string path = string.Format("{0}field[@name='{1}']", _xmlNamespace != null? "m:": "", name);

				XmlNodeList list = _xmlNamespace != null? 
					_xmlSchema.SelectNodes(path, _xmlNamespace): 
					_xmlSchema.SelectNodes(path);
			
				if (list != null && list.Count > 0)
				{
					XmlAttribute attr = list[0].Attributes["ignore"];

					if (attr != null)
					{
						try
						{
							return Convert.ToBoolean(attr.Value);
						}
						catch (Exception ex)
						{
							ThrowConversionException("boolean", "ignore", attr.Value, name, ex);
						}
					}
				}
			}

			return attributes.Length != 0;
		}

		public object[] GetMapAttributes(Type type)
		{
			if (_xmlSchema != null)
			{
				XmlNodeList list = _xmlNamespace != null?
					_xmlSchema.SelectNodes("m:map", _xmlNamespace):
					_xmlSchema.SelectNodes("map");

				if (list != null && list.Count != 0)
				{
					ArrayList al = new ArrayList(list.Count);

					foreach (XmlNode node in list)
					{
						if (node.Attributes["source"] != null &&
							node.Attributes["target"] != null)
						{
							al.Add(new MapFieldAttribute(
								node.Attributes["source"].Value,
								node.Attributes["target"].Value));
						}
					}

					return al.ToArray();
				}
			}

			return MapDescriptor.GetAllAttributes(type, typeof(MapFieldAttribute)).ToArray();
			//return type.GetCustomAttributes(typeof(MapFieldAttribute), true);
		}

		public Attribute[] GetFieldAttributes(MemberInfo memberInfo)
		{
			return GetFieldAttributes(
				Attribute.GetCustomAttributes(memberInfo, typeof(MapFieldAttribute)), memberInfo.Name);
		}

		private Attribute[] GetFieldAttributes(Attribute[] attributes, string fieldName)
		{
			if (_xmlSchema != null)
			{
				string path = string.Format(
					"{0}field[@name='{1}']", _xmlNamespace != null? "m:": "", fieldName);

				XmlNodeList list = _xmlNamespace != null? 
					_xmlSchema.SelectNodes(path, _xmlNamespace): 
					_xmlSchema.SelectNodes(path);
			
				if (list != null && list.Count != 0)
				{
					ArrayList al = new ArrayList(list.Count);

					foreach (XmlNode node in list)
					{
						MapFieldAttribute attr = new MapFieldAttribute(fieldName);

						if (node.Attributes["source"] != null)
							attr.SourceName = node.Attributes["source"].Value;

						try
						{
							if (node.Attributes["nullable"] != null)
								attr.IsNullable = Convert.ToBoolean(node.Attributes["nullable"].Value);
						}
						catch (Exception ex)
						{
							ThrowConversionException(
								"boolean", "nullable", node.Attributes["nullable"].Value, fieldName, ex);
						}

						try
						{
							if (node.Attributes["trimmable"] != null)
								attr.IsTrimmable = Convert.ToBoolean(node.Attributes["trimmable"].Value);
						}
						catch (Exception ex)
						{
							ThrowConversionException(
								"boolean", "trimmable", node.Attributes["trimmable"].Value, fieldName, ex);
						}

						al.Add(attr);
					}

					if (al.Count != 0)
					{
						Attribute[] res = new Attribute[al.Count];

						for (int i = 0; i < al.Count; i++)
							res[i] = al[i] as Attribute;

						return res;
					}
				}
			}

			return attributes;
		}

		public Attribute[] GetValueAttributes(FieldInfo fieldInfo, string name, Type type)
		{
			return GetValueAttributes(
				Attribute.GetCustomAttributes(fieldInfo, typeof(MapValueAttribute)), name, type);
		}

		public Attribute[] GetValueAttributes(PropertyInfo propertyInfo, string name, Type type)
		{
			return GetValueAttributes(
				Attribute.GetCustomAttributes(propertyInfo, typeof(MapValueAttribute)), name, type);
		}

		private Attribute[] GetValueAttributes(Attribute[] attributes, string name, Type type)
		{
			Attribute[] typeAttributes = GetValueTypeAttributes(type);

			if (_xmlSchema != null)
			{
				// Get value attributes.
				//
				XmlNodeList list  = SelectSchema("{0}field[@name='{1}']/{0}value", null, name);
				ArrayList   attrs = GetValueAttributeArray(list, type, name);

				if (attrs.Count == 0)
					foreach (MapValueAttribute a in typeAttributes)
						if (a.DefinedInXmlSchema == true && !a.IsNullValue && !a.IsDefValue)
							attrs.Add(a);

				bool isValue = attrs.Count != 0;

				// Get null value attribute.
				//
				list = SelectSchema("{0}field[@name='{1}']/{0}null_value", null, name);

				ArrayList nullAttrs = GetNullValueAttributeArray(list, type, string.Empty);

				if (nullAttrs.Count == 0)
					foreach (MapValueAttribute a in typeAttributes)
						if (a.DefinedInXmlSchema == true && a.IsNullValue)
							nullAttrs.Add(a);

				bool isNull = nullAttrs.Count != 0;

				if (isNull) attrs.Add(nullAttrs[0]);
				
				// Get default value attribute.
				//
				list = SelectSchema("{0}field[@name='{1}']/{0}default_value", null, name);
			
				ArrayList defAttrs = GetDefaultValueAttributeArray(list, type, string.Empty);

				if (defAttrs.Count == 0)
					foreach (MapValueAttribute a in typeAttributes)
						if (a.DefinedInXmlSchema == true && a.IsDefValue)
							defAttrs.Add(a);
				
				bool isDef = defAttrs.Count != 0;

				if (isDef) attrs.Add(defAttrs[0]);

				// Merge the xml schema & member attributes.
				//
				bool isValueAttributes = false;

				foreach (MapValueAttribute a in attributes)
				{
					if (a.IsNullValue)
					{
						if (isNull == false)
						{
							isNull = true;
							attrs.Add(a);
						}
					}
					else if (a.IsDefValue)
					{
						if (isDef == false)
						{
							isDef = true;
							attrs.Add(a);
						}
					}
					else
					{
						if (isValue == false)
						{
							isValueAttributes = true;
							attrs.Add(a);
						}
					}
				}

				isValue |= isValueAttributes;

				// Merge xml schema & type attributes.
				//
				foreach (MapValueAttribute a in typeAttributes)
				{
					if ((!isNull  && a.IsNullValue) ||
						(!isDef   && a.IsDefValue)  ||
						(!isValue && !a.IsNullValue && !a.IsDefValue))
					{
						attrs.Add(a);
					}
				}
				
				attributes = new Attribute[attrs.Count];

				for (int i = 0; i < attrs.Count; i++)
					attributes[i] = (Attribute)attrs[i];
			}

			if (attributes.Length == 0)
				attributes = typeAttributes;

			return attributes;
		}

		private static object ChangeType(string value, Type type)
		{
			if (type.IsEnum)
			{
				return Enum.Parse(type, value, true);
			}
			else
			{
				return Convert.ChangeType(value, type);
			}
		}

		private static ArrayList GetValueAttributeArray(XmlNodeList list, Type type, string name)
		{
			if (list != null && list.Count != 0)
			{
				ArrayList al = new ArrayList(list.Count);

				foreach (XmlNode node in list)
				{
					if (node.Attributes["target"] != null && 
						node.Attributes["source"] != null &&
						node.Attributes["source_type"] != null)
					{
						Type   sourceType = Type.GetType(node.Attributes["source_type"].Value, true, true);
						object target     = ChangeType  (node.Attributes["target"].Value, type);
						object source     = ChangeType  (node.Attributes["source"].Value, sourceType);

						MapValueAttribute attr = new MapValueAttribute(target, source);

						attr.SetDefinedInXmlSchema();
						al.Add(attr);
					}
					else
					{
						if (name.Length != 0)
						{
							throw new RsdnMapException(string.Format(
								"Field '{0}' of the '{1}' type has wrong format of values.",
								name, type));
						}
						else
						{
							throw new RsdnMapException(string.Format(
								"Type '{1}' has wrong format of values.", type));
						}
					}
				}

				return al;
			}

			return new ArrayList();
		}

		private static ArrayList GetNullValueAttributeArray(XmlNodeList list, Type type, string name)
		{
			if (list != null && list.Count != 0)
			{
				ArrayList al = new ArrayList(list.Count);

				foreach (XmlNode node in list)
				{
					if (node.Attributes["target"] != null)
					{
						object target = ChangeType(node.Attributes["target"].Value, type);

						MapNullValueAttribute attr = new MapNullValueAttribute(target);

						attr.SetDefinedInXmlSchema();
						al.Add(attr);
					}
					else
					{
						if (name.Length != 0)
						{
							throw new RsdnMapException(string.Format(
								"Field '{0}' of the '{1}' type has wrong format of values.",
								name, type));
						}
						else
						{
							throw new RsdnMapException(string.Format(
								"Type '{1}' has wrong format of values.", type));
						}
					}
				}

				return al;
			}

			return new ArrayList();
		}

		private static ArrayList GetDefaultValueAttributeArray(XmlNodeList list, Type type, string name)
		{
			if (list != null && list.Count != 0)
			{
				ArrayList al = new ArrayList(list.Count);

				foreach (XmlNode node in list)
				{
					if (node.Attributes["target"] != null)
					{
						object target = ChangeType(node.Attributes["target"].Value, type);

						MapDefaultValueAttribute attr = new MapDefaultValueAttribute(target);

						attr.SetDefinedInXmlSchema();
						al.Add(attr);
					}
					else
					{
						if (name.Length != 0)
						{
							throw new RsdnMapException(string.Format(
								"Field '{0}' of the '{1}' type has wrong format of values.",
								name, type));
						}
						else
						{
							throw new RsdnMapException(string.Format(
								"Type '{1}' has wrong format of values.", type));
						}
					}
				}

				return al;
			}

			return new ArrayList();
		}

		const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

		private static ArrayList GetEnumAttributes(Type type)
		{
			ArrayList attributes = null;

			FieldInfo[] fields = type.GetFields();

			foreach (FieldInfo fi in fields)
			{
				if ((fi.Attributes & EnumField) == EnumField)
				{
					Attribute[] enumAttributes = 
						Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

					foreach (MapValueAttribute attr in enumAttributes)
					{
						if (attributes == null)
						{
							attributes = new ArrayList(fields.Length);
						}

						attributes.Add(attr);

						if (attr.MappedValue == null)
						{
							if (attr is MapDefaultValueAttribute)
								attr.MappedValue = null;
							else
								attr.MappedValue = attr.TypeValue;
						}

						attr.TypeValue   = Enum.Parse(type, fi.Name);
					}
				}
			}

			return attributes;
		}

		private static object[] GetCustomAttributes(Type type)
		{
			object[] attributes = type.GetCustomAttributes(typeof(MapValueAttribute), true);

			if (type.IsEnum)
			{
				ArrayList enumAttributes = GetEnumAttributes(type);

				if (enumAttributes != null && enumAttributes.Count != 0)
				{
					Attribute[] temp = new Attribute[attributes.Length + enumAttributes.Count];

					attributes.CopyTo(temp, 0);
					enumAttributes.CopyTo(0, temp, attributes.Length, enumAttributes.Count);

					return temp;
				}
			}

			return attributes;
		}

		public Attribute[] GetValueTypeAttributes(Type type)
		{
			object[]    objectArray = GetCustomAttributes(type);
			Attribute[] attributes  = new Attribute[objectArray.Length];

			for (int i = 0; i < objectArray.Length; i++)
				attributes[i] = (MapValueAttribute)objectArray[i];

			if (_xmlDoc != null)
			{
				string   typeName    = null;
				object[] customAttrs = type.GetCustomAttributes(typeof(MapXmlAttribute), true);

				if (customAttrs != null && customAttrs.Length != 0)
					typeName = ((MapXmlAttribute)customAttrs[0]).TypeName;

				if (typeName == null)
                    typeName = type.FullName.Replace("+", ".");

				// Get value attributes.
				//
				XmlNodeList list    = SelectDoc("{0}value_type[@name='{1}']/{0}value", null, typeName);
				ArrayList   attrs   = GetValueAttributeArray(list, type, string.Empty);
				bool        isValue = attrs.Count != 0;

				// Get nullable attribute.
				//
				list = SelectDoc("{0}value_type[@name='{1}']/{0}null_value", null, typeName);

				ArrayList nullAttrs = GetNullValueAttributeArray(list, type, string.Empty);
				bool      isNull    = nullAttrs.Count != 0;

				if (isNull) attrs.Add(nullAttrs[0]);
				
				// Get default attribute.
				//
				list = SelectDoc("{0}value_type[@name='{1}']/{0}default_value", null, typeName);
			
				ArrayList defAttrs = GetDefaultValueAttributeArray(list, type, string.Empty);
				bool      isDef    = defAttrs.Count != 0;

				if (isDef) attrs.Add(defAttrs[0]);

				// Merge xml schema & attributes.
				//
				foreach (MapValueAttribute a in attributes)
				{
					if ((!isNull  && a.IsNullValue) ||
						(!isDef   && a.IsDefValue)  ||
						(!isValue && !a.IsNullValue && !a.IsDefValue))
					{
						attrs.Add(a);
					}
				}

				attributes = new Attribute[attrs.Count];

				for (int i = 0; i < attrs.Count; i++)
					attributes[i] = (Attribute)attrs[i];
			}

			return attributes;
		}

		private XmlNodeList SelectDoc(string query, params object[] parameters)
		{
			parameters[0] = _xmlNamespace != null? "m:": "";

			string path = string.Format(query, parameters);

			return _xmlNamespace != null? 
				_xmlDoc.DocumentElement.SelectNodes(path, _xmlNamespace): 
				_xmlDoc.DocumentElement.SelectNodes(path);
		}

		private XmlNodeList SelectSchema(string query, params object[] parameters)
		{
			parameters[0] = _xmlNamespace != null? "m:": "";

			string path = string.Format(query, parameters);

			return _xmlNamespace != null? 
				_xmlSchema.SelectNodes(path, _xmlNamespace): 
				_xmlSchema.SelectNodes(path);
		}
	}
}
