using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class AttributeExtensionCollection : ICollection
	{
		public AttributeExtension this[int index]
		{
			get
			{
				return this == _null || index < 0 || index >= _attributes.Count?
					AttributeExtension.Null: (AttributeExtension)_attributes[index];
			}
		}

		public object Value
		{
			get { return this == _null? null: this[0].Value; }
		}

		internal void Add(AttributeExtension attributeExtension)
		{
			if (this != _null)
				_attributes.Add(attributeExtension);
		}

		private readonly ArrayList _attributes = new ArrayList();

		private static readonly AttributeExtensionCollection _null = new AttributeExtensionCollection();
		public  static          AttributeExtensionCollection  Null
		{
			get { return _null;  }
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
			return _attributes.GetEnumerator();
		}

		#endregion
	}
}
