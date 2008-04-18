using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	public /*[a]*/abstract/*[/a]*/ class TestClass
	{
		public static int Value;

		[/*[a]*/Cache/*[/a]*/(MaxCacheTime=500, IsWeak=false)]
		public /*[a]*/virtual/*[/a]*/ int CachedMethod(int p1, int p2)
		{
			return Value;
		}

		public static TestClass CreateInstance()
		{
			// Use TypeAccessor to create an instance of an abstract class.
			//
			return /*[a]*/TypeAccessor/*[/a]*/<TestClass>.CreateInstance();
		}
	}

	[TestFixture]
	public class CacheAspectTest
	{
		[Test]
		public void Test1()
		{
			TestClass tc = TestClass.CreateInstance();

			DateTime begin = DateTime.Now;

			// Initial setup for the test static variable.
			//
			TestClass.Value = 777;

			while (tc.CachedMethod(2, 2) == 777)
			{
				// This change will not affect the Test method return value for 500 ms.
				//
				TestClass.Value++;
			}

			Assert.IsTrue((DateTime.Now - begin).TotalMilliseconds >= 500);
		}

		[Test]
		public void Test2()
		{
			TestClass tc = TestClass.CreateInstance();

			// Return value depends on parameter values.
			//
			TestClass.Value = 1; Assert.AreEqual(1, tc.CachedMethod(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, tc.CachedMethod(1, 1)); // no change
			TestClass.Value = 3; Assert.AreEqual(3, tc.CachedMethod(2, 1));

			// However we can clear cache manually.
			// For particular method:
			//
			CacheAspect.ClearCache(typeof(TestClass), "CachedMethod", typeof(int), typeof(int));
			TestClass.Value = 4; Assert.AreEqual(4, tc.CachedMethod(2, 1));

			// By MethodInfo:
			//
			MethodInfo methodInfo = tc.GetType().GetMethod("CachedMethod", new Type[] { typeof(int), typeof(int) });
			CacheAspect.ClearCache(methodInfo);
			TestClass.Value = 5; Assert.AreEqual(5, tc.CachedMethod(2, 1));

			// For the all cached methods.
			//
			CacheAspect.ClearCache();
			TestClass.Value = 6; Assert.AreEqual(6, tc.CachedMethod(2, 1));
		}
	}
}
