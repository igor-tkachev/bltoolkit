using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class AttributeInfoCollection : ICollection
	{
		public AttributeInfo this[string attributeName]
		{
			get
			{
				AttributeInfo value = (AttributeInfo)_attributes[attributeName];
				return value != null? value: AttributeInfo.Null;
			}
		}

		public void Add(AttributeInfo memberInfo)
		{
			if (!_isNull)
				_attributes[memberInfo.Name] = memberInfo;
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
