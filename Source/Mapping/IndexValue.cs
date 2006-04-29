using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class IndexValue : IComparable
	{
		public IndexValue(params object[] values)
		{
			if (values == null) throw new ArgumentNullException("values");

			_values = values;

			CalcHashCode();
		}

		private object[] _values;

		#region IComparable Members

		public int CompareTo(object obj)
		{
			object[] objValues = ((IndexValue)obj)._values;

			if (_values.Length != objValues.Length)
				return _values.Length - objValues.Length;

			for (int i = 0; i < _values.Length; i++)
			{
				int n = Comparer.Default.Compare(_values[i], objValues[i]);

				if (n != 0)
					return n;
			}

			return 0;
		}

		#endregion

		#region Object Overrides

		private int _hash;

		private void CalcHashCode()
		{
			if (_values.Length == 0)
			{
				_hash = 0;
			}
			else
			{
				object  o = _values[0];
				_hash = o == null ? 0 : o.GetHashCode();

				for (int i = 1; i < _values.Length; i++)
				{
					o = _values[i];
					_hash = ((_hash << 5) + _hash) ^ (o == null ? 0 : o.GetHashCode());
				}
			}
		}

		public override int GetHashCode()
		{
			return _hash;
		}

		public override bool Equals(object obj)
		{
			object[] values = ((IndexValue)obj)._values;

			if (_values.Length != values.Length)
				return false;

			for (int i = 0; i < _values.Length; i++)
				if (_values[i].Equals(values[i]) == false)
					return false;

			return true;
		}

		#endregion
	}
}
