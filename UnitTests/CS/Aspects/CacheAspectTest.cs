using System;
using System.Collections;
using BLToolkit.Aspects;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
	public class CacheAspectTest
	{
		[Log]
		public abstract class TestClass
		{
			public static int Value;

			[Cache(500, false)]
			public virtual int Test(int i1, int i2)
			{
				return Value;
			}

			[InstanceCache]
			public virtual int InstanceTest(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void Test()
		{
			TestClass t = TypeAccessor.CreateInstance<TestClass>();

			DateTime begin = DateTime.Now;

			for (TestClass.Value = 777; t.Test(2, 2) == 777; TestClass.Value++)
				continue;

			Assert.IsTrue((DateTime.Now - begin).TotalMilliseconds >= 500);

			TestClass.Value = 1; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 3; Assert.AreEqual(3, t.Test(2, 1));

			CacheAspect.ClearCache(typeof(TestClass), "Test", typeof(int), typeof(int));
			TestClass.Value = 4; Assert.AreEqual(4, t.Test(2, 1));

			CacheAspect.ClearCache(t.GetType(), "Test", typeof(int), typeof(int));
			TestClass.Value = 5; Assert.AreEqual(5, t.Test(2, 1));

			CacheAspect.ClearCache();
			TestClass.Value = 6; Assert.AreEqual(6, t.Test(2, 1));
		}

		[Test]
		public void InstanceTest()
		{
			TestClass t = TypeAccessor.CreateInstance<TestClass>();

			TestClass.Value = 1; Assert.AreEqual(1, t.InstanceTest(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, t.InstanceTest(1, 1));
			TestClass.Value = 3; Assert.AreEqual(3, t.InstanceTest(2, 1));
			TestClass.Value = 4;

			t = TypeAccessor.CreateInstance<TestClass>();

			Assert.AreNotEqual(1, t.InstanceTest(1, 1));
			Assert.AreNotEqual(3, t.InstanceTest(2, 1));
		}

		public class CustomCacheAspect : CacheAspect
		{
			private static IDictionary _methodcache = new Hashtable();
			public  static IDictionary  MethodCache
			{
				get { return _methodcache; }
			}

			protected override IDictionary CreateCache()
			{
				return MethodCache;
			}
		}

		[Log]
		public abstract class CustomTestClass
		{
			public static int Value;

			[Cache(typeof(CustomCacheAspect), MaxCacheTime = 500)]
			public virtual int Test(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void CustomCacheAspectTest()
		{
			CustomTestClass t = (CustomTestClass)TypeAccessor.CreateInstance(typeof(CustomTestClass));

			CustomTestClass.Value = 1; Assert.AreEqual(1, t.Test(1, 1));
			CustomTestClass.Value = 2; Assert.AreEqual(1, t.Test(1, 1));
			CustomTestClass.Value = 3; Assert.AreEqual(3, t.Test(2, 1));

			CustomCacheAspect.MethodCache.Clear();
			CustomTestClass.Value = 4; Assert.AreEqual(4, t.Test(2, 1));
		}
	}
}
