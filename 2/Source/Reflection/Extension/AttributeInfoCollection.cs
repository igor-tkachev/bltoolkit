using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class AttributeExtensionCollection : ICollection
	{
		public AttributeExtension this[string attributeName]
		{
			get
			{
				if (_isNull)
					return AttributeExtension.Null;

				AttributeExtension value = (AttributeExtension)_attributes[attributeName];
				return value != null? value: AttributeExtension.Null;
			}
		}

		public void Add(AttributeExtension memberExtension)
		{
			if (!_isNull)
				_attributes[memberExtension.Name] = memberExtension;
		}

		private  Hashtable _attributes = new Hashtable();
		internal bool      _isNull;

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
