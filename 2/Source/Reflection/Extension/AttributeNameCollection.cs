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

				AttributeExtensionCollection value =
					(AttributeExtensionCollection)_attributes[attributeName];

				return value != null? value: AttributeExtensionCollection.Null;
			}
		}

		public void Add(AttributeExtension memberExtension)
		{
			if (this != _null)
			{
				AttributeExtensionCollection attr =
					(AttributeExtensionCollection)_attributes[memberExtension.Name];

				if (attr == null)
					_attributes[memberExtension.Name] = attr = new AttributeExtensionCollection();

				attr.Add(memberExtension);
			}
		}

		private Hashtable _attributes = new Hashtable();

		private static AttributeNameCollection _null = new AttributeNameCollection();
		public  static AttributeNameCollection  Null
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
			get { return _attributes.IsSynchronized; ; }
		}

		public object SyncRoot
		{
			get { return _attributes.SyncRoot;; }
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
