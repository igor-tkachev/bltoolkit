using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class MixinAspectTest
	{
		public interface ITestInterface1
		{
			int TestMethod(int value);
		}

		public class TestInterface1Impl : ITestInterface1
		{
			public int TestMethod(int value) { return value; }
		}

		public interface ITestInterface2
		{
			int TestMethod1(int value);
			int TestMethod2(int value);
		}

		public class TestInterface2Impl : ITestInterface2
		{
			public int TestMethod1(int value) { return value; }
			public int TestMethod2(int value) { return value; }
		}

		[/*[a]*/Mixin/*[/a]*/(/*[a]*/typeof/*[/a]*/(/*[a]*/ITestInterface1/*[/a]*/), /*[a]*/"_testInterface1"/*[/a]*/)]
		[/*[a]*/Mixin/*[/a]*/(/*[a]*/typeof/*[/a]*/(/*[a]*/ITestInterface2/*[/a]*/), /*[a]*/"TestInterface2"/*[/a]*/, "'{0}.{1}' is null.")]
		public /*[a]*/abstract/*[/a]*/ class TestClass
		{
			public TestClass()
			{
				/*[a]*/_testInterface1/*[/a]*/ = new /*[a]*/TestInterface1Impl/*[/a]*/();
			}

			/*[a]*/protected/*[/a]*/ /*[a]*/object/*[/a]*/          /*[a]*/_testInterface1/*[/a]*/;

			private   ITestInterface2 _testInterface2;
			public    /*[a]*/ITestInterface2/*[/a]*/  /*[a]*/TestInterface2/*[/a]*/
			{
				get { return _testInterface2 ?? (_testInterface2 = new /*[a]*/TestInterface2Impl/*[/a]*/()); }
			}

			[/*[a]*/MixinOverride/*[/a]*/(typeof(/*[a]*/ITestInterface2/*[/a]*/))]
			protected int TestMethod1(int value) { return /*[a]*/15/*[/a]*/; }
		}

		[Test]
		public void Test()
		{
			TestClass       tc = /*[a]*/TypeAccessor/*[/a]*/<TestClass>.CreateInstance();
			ITestInterface1 i1 = (ITestInterface1)tc;
			ITestInterface2 i2 = (ITestInterface2)tc;

			Assert.AreEqual(/*[a]*/10/*[/a]*/, i1.TestMethod (/*[a]*/10/*[/a]*/));
			Assert.AreEqual(/*[a]*/15/*[/a]*/, i2.TestMethod1(/*[a]*/20/*[/a]*/));
			Assert.AreEqual(/*[a]*/30/*[/a]*/, i2.TestMethod2(/*[a]*/30/*[/a]*/));
		}
	}
}
