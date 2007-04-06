namespace BLToolkit.Common
{
	public static class Configuration
	{
		public enum NullEquivalent { DBNull, Null, Value }

		private static NullEquivalent _checkNullReturnIfNull = NullEquivalent.DBNull;
		public  static NullEquivalent  CheckNullReturnIfNull
		{
			get { return _checkNullReturnIfNull;  }
			set { _checkNullReturnIfNull = value; }
		}

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

		private static bool _notifyOnEqualSet = true;
		public  static bool  NotifyOnEqualSet
		{
			get { return _notifyOnEqualSet;  }
			set { _notifyOnEqualSet = value; }
		}
	}
}
