using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class ClearCacheAspect
	{
		public /*[a]*/abstract/*[/a]*/ class TestClass
		{
			public static int Value;

			// This is a method we will cache. Cached return value depends on input parameters.
			// We will change the 'Value' field outside of the class and see how it affects the result.
			//
			[/*[a]*/Cache/*[/a]*/(MaxCacheTime=500, IsWeak=false)]
			public /*[a]*/virtual/*[/a]*/ int CachedMethod(int p1, int p2)
			{
				return Value;
			}

			// This method clears the CachedMethod cache.
			//
			[/*[a]*/ClearCache/*[/a]*/(/*[a]*/"CachedMethod"/*[/a]*/)]
			public abstract void ClearCache();

			// The CachedMethod is specified by name and parameters.
			// Also you can use declaring method type.
			//
			[/*[a]*/ClearCache/*[/a]*/(/*[a]*/"CachedMethod"/*[/a]*/, /*[a]*/typeof(int), typeof(int)/*[/a]*/)]
			public abstract void ClearCache2();

			// This method clears all caches for provided type.
			//
			[/*[a]*/ClearCache/*[/a]*/(/*[a]*/typeof(TestClass)/*[/a]*/)]
			public abstract void ClearAll();

			// This method clears all caches for current type.
			//
			[/*[a]*/ClearCache/*[/a]*/]
			public abstract void ClearAll2();

			public static TestClass CreateInstance()
			{
				// Use TypeAccessor to create an instance of an abstract class.
				//
				return /*[a]*/TypeAccessor/*[/a]*/<TestClass>.CreateInstance();
			}
		}

		[Test]
		public void Test()
		{
			TestClass tc = TypeAccessor<TestClass>.CreateInstance();

			TestClass.Value = 1;

			int value1 = tc.CachedMethod(1, 2);

			TestClass.Value = 2;

			int value2 = tc.CachedMethod(1, 2);

			// The cached values are equal.
			//
			Assert.AreEqual(value1, value2);

			tc.ClearCache();

			TestClass.Value = 3;

			// Previous and returned values are not equal.
			//
			Assert.AreNotEqual(value1, tc.CachedMethod(1, 2));

			tc.ClearCache2();
		}
	}
}
