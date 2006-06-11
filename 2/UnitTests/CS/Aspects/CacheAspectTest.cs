using System;

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

			[Cache(100)]
			public virtual int Test(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void Test()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			DateTime begin = DateTime.Now;

			for (TestClass.Value = 777; t.Test(2, 2) == 777; TestClass.Value++);

			Assert.IsTrue((DateTime.Now - begin).TotalMilliseconds >= 100);

			TestClass.Value = 1; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 3; Assert.AreEqual(3, t.Test(2, 1));
		}
	}
}
