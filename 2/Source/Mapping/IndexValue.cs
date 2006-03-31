using System;
using System.Collections;

namespace BLToolkit.Mapping
{
	public class IndexValue : IComparable
	{
		public IndexValue(params object[] values)
		{
			_values = values;
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
	}
}
