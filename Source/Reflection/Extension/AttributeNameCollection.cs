using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class AttributeNameCollection : ICollection
	{
		public AttributeExtensionCollection this[string attributeName]
		{
			get
			{
				if (this == _null)
					return AttributeExtensionCollection.Null;

				AttributeExtensionCollection ext =
					(AttributeExtensionCollection)_attributes[attributeName];

				return ext ?? AttributeExtensionCollection.Null;
			}
		}

		public void Add(AttributeExtension attributeExtension)
		{
			if (this != _null)
			{
				// Add attribute.
				//
				AttributeExtensionCollection attr =
					(AttributeExtensionCollection)_attributes[attributeExtension.Name];

				if (attr == null)
					_attributes[attributeExtension.Name] = attr = new AttributeExtensionCollection();

				attr.Add(attributeExtension);

				/*
				// Convert value type.
				//
				bool isType = attributeExtension.Name.EndsWith(TypeExtension.AttrName.TypePostfix);

				if (isType)
				{
					string attrName = attributeExtension.Name.Substring(
						0, attributeExtension.Name.Length - 5);

					AttributeExtensionCollection ext =
						(AttributeExtensionCollection)_attributes[attrName];

					if (ext != null && ext.Count == 1)
						ext[0].Values.ChangeValueType(attributeExtension.Value.ToString());
				}
				else
				{
					string attrName = attributeExtension.Name + TypeExtension.AttrName.TypePostfix;

					AttributeExtensionCollection ext =
						(AttributeExtensionCollection)_attributes[attrName];

					if (ext != null && ext.Count == 1)
						attributeExtension.Values.ChangeValueType(ext.Value.ToString());
				}
				*/
			}
		}

		public void Add(string name, string value)
		{
			if (this != _null)
			{
				string attrName  = name;
				string valueName = string.Empty;

				int idx = name.IndexOf(TypeExtension.ValueName.Delimiter);

				if (idx > 0)
				{
					valueName = name.Substring(idx + 1).TrimStart(TypeExtension.ValueName.Delimiter);
					attrName  = name.Substring(0, idx);
				}

				if (valueName.Length == 0)
					valueName = TypeExtension.ValueName.Value;
				else if (valueName == TypeExtension.ValueName.Type)
					valueName = TypeExtension.ValueName.ValueType;

				AttributeExtensionCollection ext =
					(AttributeExtensionCollection)_attributes[attrName];

				if (ext != null)
				{
					ext[0].Values.Add(valueName, value);
				}
				else
				{
					AttributeExtension attributeExtension = new AttributeExtension();

					attributeExtension.Name = name;
					attributeExtension.Values.Add(valueName, value);

					Add(attributeExtension);
				}
			}
		}

		private readonly Hashtable _attributes = new Hashtable();

		private static readonly AttributeNameCollection _null = new AttributeNameCollection();
		public  static          AttributeNameCollection  Null
		{
			get { return _null; }
		}

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_attributes.CopyTo(array, index);
		}

		public int Count
		{
			get { return _attributes.Count; }
		}

		public bool IsSynchronized
		{
			get { return _attributes.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _attributes.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _attributes.Values.GetEnumerator();
		}

		#endregion
	}
}
