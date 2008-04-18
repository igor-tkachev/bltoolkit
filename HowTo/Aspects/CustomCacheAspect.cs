using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class CustomCacheAspectTest
	{
		public class /*[a]*/CustomCacheAspect/*[/a]*/ : CacheAspect
		{
			private static IDictionary _cache = new Hashtable();
			public  static IDictionary  Cache
			{
				get { return _cache; }
			}

			protected /*[a]*/override/*[/a]*/ IDictionary GetCache(InterceptCallInfo info)
			{
				return Cache;
			}
		}

		public /*[a]*/abstract/*[/a]*/ class CustomTestClass
		{
			public static int Value;

			[Cache(/*[a]*/typeof(CustomCacheAspect)/*[/a]*/, MaxSeconds = 1)]
			public /*[a]*/virtual/*[/a]*/ int CachedMethod(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void Test()
		{
			CustomTestClass t = TypeAccessor<CustomTestClass>.CreateInstance();

			CustomTestClass.Value = 1; Assert.AreEqual(1, t.CachedMethod(1, 1));
			CustomTestClass.Value = 2; Assert.AreEqual(1, t.CachedMethod(1, 1));
			CustomTestClass.Value = 3; Assert.AreEqual(3, t.CachedMethod(2, 1));

			CustomCacheAspect.Cache.Clear();
			CustomTestClass.Value = 4; Assert.AreEqual(4, t.CachedMethod(2, 1));
		}
	}
}
