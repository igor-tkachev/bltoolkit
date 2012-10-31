using System;

namespace BLToolkit.Aspects
{
	public class CacheAspectItem
	{
		private        DateTime _maxCacheTime;
		public virtual DateTime  MaxCacheTime
		{
			get { return _maxCacheTime;  }
			set { _maxCacheTime = value; }
		}

		private object _returnValue;
		public  object  ReturnValue
		{
			get { return _returnValue;  }
			set { _returnValue = value; }
		}

		private object[] _refValues;
		public  object[]  RefValues
		{
			get { return _refValues;  }
			set { _refValues = value; }
		}

		public virtual bool IsExpired
		{
			get { return _maxCacheTime <= DateTime.Now; }
		}
	}
}
