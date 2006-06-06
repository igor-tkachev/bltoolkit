using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class IndexValue : IComparable
	{
		public IndexValue(params object[] values)
		{
			if (values == null)
				throw new ArgumentNullException("values");

			_values = values;
			_hash = CalcHashCode(values);
		}

		private readonly object[] _values;
		private readonly int      _hash;

		private static int CalcHashCode(object[] values)
		{
			if (values.Length == 0)
				return 0;

			object o = values[0];
			int hash = o == null ? 0 : o.GetHashCode();

			for (int i = 1; i < values.Length; i++)
			{
				o = values[i];
				hash = ((hash << 5) + hash) ^ (o == null ? 0 : o.GetHashCode());
			}

			return hash;
		}

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

		public override int GetHashCode()
		{
			return _hash;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IndexValue) || _hash != ((IndexValue)obj)._hash)
				return false;

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
