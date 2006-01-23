using System;
using System.Collections;

namespace BLToolkit.TypeInfo
{
	public class ValueCollection : ICollection
	{
		public string Value
		{
			get { return this[0]; }
		}

		public string this[int index]
		{
			get
			{
				return index < 0 || index >= _values.Count? null: (string)_values[index];
			}
		}

		public void Add(string value)
		{
			if (!_isNull)
				_values.Add(value);
		}

		private  ArrayList _values = new ArrayList();
		internal bool      _isNull;

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_values.CopyTo(array, index);
		}

		public int Count
		{
			get { return _values.Count; }
		}

		public bool IsSynchronized
		{
			get { return _values.IsSynchronized; ; }
		}

		public object SyncRoot
		{
			get { return _values.SyncRoot;; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _values.GetEnumerator();
		}

		#endregion
	}
}
