using System;

namespace BLToolkit.Mapping
{
	public class MapValue
	{
		private object _origValue;
		public  object  OrigValue
		{
			get { return _origValue;  }
			set { _origValue = value; }
		}

		private object[] _mapValues;
		public  object[]  MapValues
		{
			get { return _mapValues;  }
			set { _mapValues = value; }
		}
	}
}
