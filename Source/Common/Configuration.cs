using System;
using System.Collections.Generic;
using System.Text;

namespace BLToolkit.Common
{
	public static class Configuration
	{
		private static bool _trimOnMapping = false;
		public  static bool  TrimOnMapping
		{
			get { return _trimOnMapping;  }
			set { _trimOnMapping = value; }
		}

		private static bool _trimDictionaryKey = true;
		public  static bool  TrimDictionaryKey
		{
			get { return _trimDictionaryKey;  }
			set { _trimDictionaryKey = value; }
		}

		private static bool _notifyOnEqualSet = false;
		public  static bool  NotifyOnEqualSet
		{
			get { return _notifyOnEqualSet;  }
			set { _notifyOnEqualSet = value; }
		}
	}
}
