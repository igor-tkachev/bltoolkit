using System;
using System.Collections;

namespace BLToolkit.Reflection.Extension
{
	public class ValueCollection : ICollection
	{
		public object Value
		{
			get { return _value; }
		}

		public object this[string name]
		{
			get { return _values[name]; }
		}

		public void Add(string name, string value)
		{
			if (this != _null)
			{
				bool isType = name.EndsWith(TypeExtension.ValueName.TypePostfix);

				if (isType)
				{
					Type   type      = Type.GetType(value, true);
					string valueName =
						name.Substring(0, name.Length - TypeExtension.ValueName.TypePostfix.Length);

					_values[name] = type;

					object val = _values[valueName];

					if (val != null && val.GetType() != type)
					{
						_values[valueName] = val = TypeExtension.ChangeType(val.ToString(), type);

						if (valueName == TypeExtension.ValueName.Value)
							_value = val;
					}
				}
				else
				{
					Type   type = (Type)_values[name + TypeExtension.ValueName.TypePostfix];
					object val  = value;

					if (type != null && type != _value.GetType())
						val = TypeExtension.ChangeType(value, type);

					_values[name] = val;

					if (name == TypeExtension.ValueName.Value)
						_value = val;
				}
			}
		}

		private          object    _value;
		private readonly Hashtable _values = new Hashtable();

		private static readonly ValueCollection _null = new ValueCollection();
		public  static          ValueCollection  Null
		{
			get { return _null;  }
		}

		#region ICollection Members

		public void CopyTo(Array array, int index)
		{
			_values.Values.CopyTo(array, index);
		}

		public int Count
		{
			get { return _values.Count; }
		}

		public bool IsSynchronized
		{
			get { return _values.IsSynchronized; }
		}

		public object SyncRoot
		{
			get { return _values.SyncRoot; }
		}

		#endregion

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _values.Values.GetEnumerator();
		}

		#endregion
	}
}
