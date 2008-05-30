using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace Aspects
{
	[TestFixture]
	public class ClearCacheAspect
	{
		public abstract class TestClass
		{
			int _value;

			[Cache]
			public virtual int Test1()
			{
				return _value++;
			}

			[ClearCache("Test1")]
			public abstract void ClearTest1();

			[Cache]
			public virtual int Test2()
			{
				return _value++;
			}

			[Cache]
			public virtual int Test2(int i)
			{
				return _value++;
			}

			[ClearCache("Test2")]
			public abstract void ClearTest2();

			[ClearCache("Test2", typeof(int))]
			public abstract void ClearTest2a();

			[ClearCache("Test2"), ClearCache("Test2", typeof(int))]
			public abstract void ClearTest2b();
		}

		public abstract class TestClass1
		{
			[ClearCache(typeof(TestClass), "Test2")]
			public abstract void ClearTest();

			[ClearCache(typeof(TestClass), "Test2", typeof(int))]
			public abstract void ClearTest1();

			protected abstract int Test();

			[ClearCache("Test")]
			public abstract void ClearTest3();

			[ClearCache(typeof(TestClass))]
			public abstract void ClearTest4();

			[ClearCache]
			public abstract void ClearTest5();
		}

		[Test]
		public void Test1()
		{
			TestClass tc = TypeAccessor<TestClass>.CreateInstance();

			int value1 = tc.Test1();
			int value2 = tc.Test1();

			Assert.AreEqual(value1, value2);

			tc.ClearTest1();

			Assert.AreNotEqual(value1, tc.Test1());
		}

		[Test]
		public void Test2()
		{
			TestClass tc = TypeAccessor<TestClass>.CreateInstance();

			tc.ClearTest2();

			int value1 = tc.Test2();
			int value2 = tc.Test2();

			Assert.AreEqual(value1, value2);

			tc.ClearTest2();

			Assert.AreNotEqual(value1, tc.Test2());
		}

		[Test]
		public void Test2a()
		{
			TestClass tc = TypeAccessor<TestClass>.CreateInstance();

			tc.ClearTest2a();

			int value1 = tc.Test2(1);
			int value2 = tc.Test2(1);

			Assert.AreEqual(value1, value2);

			tc.ClearTest2a();

			Assert.AreNotEqual(value1, tc.Test2(1));
		}

		[Test]
		public void Test2b()
		{
			TestClass tc = TypeAccessor<TestClass>.CreateInstance();

			tc.ClearTest2b();

			int value1 = tc.Test2();
			int value2 = tc.Test2();
			int value3 = tc.Test2(1);
			int value4 = tc.Test2(1);

			Assert.AreEqual(value1, value2);
			Assert.AreEqual(value3, value4);

			tc.ClearTest2b();

			Assert.AreNotEqual(value1, tc.Test2());
			Assert.AreNotEqual(value3, tc.Test2(1));
		}

		[Test]
		public void Test3()
		{
			TestClass  tc1 = TypeAccessor<TestClass>. CreateInstance();
			TestClass1 tc2 = TypeAccessor<TestClass1>.CreateInstance();

			tc1.ClearTest2b();

			int value1 = tc1.Test2();
			int value2 = tc1.Test2();
			int value3 = tc1.Test2(1);
			int value4 = tc1.Test2(1);

			Assert.AreEqual(value1, value2);
			Assert.AreEqual(value3, value4);

			tc2.ClearTest();
			tc2.ClearTest1();

			Assert.AreNotEqual(value1, tc1.Test2());
			Assert.AreNotEqual(value3, tc1.Test2(1));
		}

		[Test]
		public void Test4()
		{
			TestClass1 tc = TypeAccessor<TestClass1>.CreateInstance();

			tc.ClearTest3();
		}

		[Test]
		public void Test5()
		{
			TestClass  tc1 = TypeAccessor<TestClass>. CreateInstance();
			TestClass1 tc2 = TypeAccessor<TestClass1>.CreateInstance();

			tc1.ClearTest2b();

			int value1 = tc1.Test2();
			int value2 = tc1.Test2();
			int value3 = tc1.Test2(1);
			int value4 = tc1.Test2(1);

			Assert.AreEqual(value1, value2);
			Assert.AreEqual(value3, value4);

			tc2.ClearTest();
			tc2.ClearTest4();

			Assert.AreNotEqual(value1, tc1.Test2());
			Assert.AreNotEqual(value3, tc1.Test2(1));
		}

		[Test]
		public void Test6()
		{
			TestClass1 tc = TypeAccessor<TestClass1>.CreateInstance();

			tc.ClearTest5();
		}
	}
}
