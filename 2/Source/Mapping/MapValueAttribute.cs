using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Mapping
{
	[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public class MapValueAttribute : Attribute
	{
		public MapValueAttribute(object value1)
		{
			SetValues(null, value1);
		}

		public MapValueAttribute(object origValue, object value1)
		{
			SetValues(origValue, value1);
		}

		public MapValueAttribute(object origValue, object value1, object value2)
		{
			SetValues(origValue, value1, value2);
		}

		public MapValueAttribute(object origValue, object value1, object value2, object value3)
		{
			SetValues(origValue, value1, value2, value3);
		}

		public MapValueAttribute(object origValue, object value1, object value2, object value3, object value4)
		{
			SetValues(origValue, value1, value2, value3, value4);
		}
		
		public MapValueAttribute(object origValue, object value1, object value2, object value3, object value4, object value5)
		{
			SetValues(origValue, value1, value2, value3, value4, value5);
		}

		protected void SetValues(object origValue, params object[] values)
		{
			_origValue = origValue;
			_values    = values;
		}

		private object _origValue;
		public  object  OrigValue
		{
			get { return _origValue; }
		}

		private object[] _values;
		public  object[]  Values
		{
			get { return _values; }
		}
	}
}
