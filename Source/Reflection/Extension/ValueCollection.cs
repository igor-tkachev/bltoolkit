using System;
using System.Collections;

namespace BLToolkit.Reflection
{
	public class ValueCollection : ICollection
	{
		public object Value
		{
			get { return this[0]; }
		}

		public object this[int index]
		{
			get { return this == _null || index < 0 || index >= _values.Count? null: _values[index]; }
		}

		public object this[string name]
		{
			get { return _namedValues[name]; }
		}

		public void Add(string value)
		{
			if (this != _null)
				_values.Add(value);
		}

		public void Add(string name, string value)
		{
			if (this != _null)
			{
				_values.Add(value);
				_namedValues[name] = value;
			}
		}

		private ArrayList _values      = new ArrayList();
		private Hashtable _namedValues = new Hashtable();

		private static ValueCollection _null = new ValueCollection();
		public  static ValueCollection  Null
		{
			get { return _null;  }
		}

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
