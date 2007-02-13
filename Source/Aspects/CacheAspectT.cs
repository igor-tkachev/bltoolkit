using System;
using System.Collections;

namespace BLToolkit.Aspects
{
	public class CacheAspect<T> : CacheAspect
	{
		private static IDictionary _cache = new Hashtable();
		public  static IDictionary  Cache
		{
			get { return _cache; }
		}

		protected override IDictionary GetCache(InterceptCallInfo info)
		{
			return Cache;
		}
	}
}
