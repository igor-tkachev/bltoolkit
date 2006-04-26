using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class IndexValue : IComparable
	{
		public IndexValue(params object[] values)
		{
			_values = values;

			CalcHashCode();
		}

		private object[] _values;
		public  object[]  Values
		{
			get { return _values; }
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			object[] objValues = ((IndexValue)obj).Values;

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

		int hash;

		private void CalcHashCode()
		{
			hash = 0;

			for (int i = 0; i < _values.Length; i++)
			{
				object o = _values[i];

				if (o != null)
					hash += o.GetHashCode();
			}
		}

		public override int GetHashCode()
		{
			return hash;
		}

		public override bool Equals(object obj)
		{
			object[] values = ((IndexValue)obj)._values;

			if (_values.Length != values.Length)
				return false;

			for (int i = 0; i < _values.Length; i++)
				if (!_values[i].Equals(values[i]))
					return false;

			return true;
		}

		#endregion
	}
}
