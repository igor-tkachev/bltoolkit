using System;
using System.Diagnostics.CodeAnalysis;

namespace BLToolkit.Mapping
{
	[SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
	[AttributeUsage(
		AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum |
		AttributeTargets.Class | AttributeTargets.Interface,
		AllowMultiple=true)]
	public class MapValueAttribute : Attribute
	{
		public MapValueAttribute(object value1)
		{
			SetValues(null, null, value1);
		}

		public MapValueAttribute(object[] values)
		{
			SetValues(null, null, values);
		}

		public MapValueAttribute(object origValue, object[] values)
		{
			SetValues(null, origValue, values);
		}

		public MapValueAttribute(object origValue, object value1)
		{
			SetValues(null, origValue, value1);
		}

		public MapValueAttribute(object origValue, object value1, object value2)
		{
			SetValues(null, origValue, value1, value2);
		}

		public MapValueAttribute(object origValue, object value1, object value2, object value3)
		{
			SetValues(null, origValue, value1, value2, value3);
		}

		public MapValueAttribute(object origValue, object value1, object value2, object value3, object value4)
		{
			SetValues(null, origValue, value1, value2, value3, value4);
		}
		
		public MapValueAttribute(object origValue, object value1, object value2, object value3, object value4, object value5)
		{
			SetValues(null, origValue, value1, value2, value3, value4, value5);
		}

		public MapValueAttribute(Type type, object origValue, object[] values)
		{
			SetValues(type, origValue, values);
		}

		public MapValueAttribute(Type type, object origValue, object value1)
		{
			SetValues(type, origValue, value1);
		}

		public MapValueAttribute(Type type, object origValue, object value1, object value2)
		{
			SetValues(type, origValue, value1, value2);
		}

		public MapValueAttribute(Type type, object origValue, object value1, object value2, object value3)
		{
			SetValues(type, origValue, value1, value2, value3);
		}

		public MapValueAttribute(Type type, object origValue, object value1, object value2, object value3, object value4)
		{
			SetValues(type, origValue, value1, value2, value3, value4);
		}
		
		public MapValueAttribute(Type type, object origValue, object value1, object value2, object value3, object value4, object value5)
		{
			SetValues(type, origValue, value1, value2, value3, value4, value5);
		}

		protected void SetValues(Type type, object origValue, params object[] values)
		{
			_type      = type;
			_origValue = origValue;
			_values    = values;
		}

		private Type   _type;
		public  object  Type
		{
			get { return _type; }
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
