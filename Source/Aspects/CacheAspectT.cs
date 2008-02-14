using System.Collections;
using System.Collections.Generic;
using BLToolkit.Common;

namespace BLToolkit.Aspects
{
	public class CacheAspect<T> : CacheAspect
	{
		private static readonly IDictionary _cache = new Dictionary<CompoundValue, object>();
		public  static          IDictionary  Cache
		{
			get { return _cache; }
		}

		protected override IDictionary GetCache(InterceptCallInfo info)
		{
			return Cache;
		}
	}
}
