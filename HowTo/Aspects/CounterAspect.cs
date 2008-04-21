using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using System.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class CounterAspectTest
	{
		public /*[a]*/abstract/*[/a]*/ class TestClass
		{
			// This is a method we will collect statistic for.
			//
			[/*[a]*/Counter/*[/a]*/]
			public /*[a]*/virtual/*[/a]*/ void TestMethod()
			{
			}
		}

		[Test]
		public void Test()
		{
			TestClass t = TypeAccessor<TestClass>.CreateInstance();

			for (int i = 0; i < 10; i++)
				t.TestMethod();

			MethodInfo     methodInfo = typeof(TestClass).GetMethod("TestMethod");
			MethodCallCounter counter = CounterAspect.GetCounter(methodInfo);

			Assert.AreEqual(10, counter.TotalCount);

			Console.WriteLine(@"
Method         : {0}.{1}
TotalCount     : {2}
ExceptionCount : {3}
CachedCount    : {4}
CurrentCalls   : {5}
TotalTime      : {6}
MinTime        : {7}
MaxTime        : {8}
AverageTime    : {9}
",
				counter.MethodInfo.DeclaringType.Name,
				counter.MethodInfo.Name,
				counter.TotalCount,              // total actual calls (no cached calls)
				counter.ExceptionCount,          // calls with exceptions
				counter.CachedCount,             // cached calls
				counter.CurrentCalls.Count,      // current calls (make sense for multithreading)
				counter.TotalTime,               // total work time
				counter.MinTime,                 // min call time
				counter.MaxTime,                 // max call time
				counter.AverageTime);            // average call time
		}
	}
}
