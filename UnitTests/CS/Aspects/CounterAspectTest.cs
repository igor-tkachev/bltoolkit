using System;
using System.Security.Principal;
using System.Threading;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
	public class CounterAspectTest
	{
		[Log]
		public abstract class TestClass
		{
			[Counter]
			public virtual void Test()
			{
			}

			[Counter]
			public virtual void LongTest()
			{
				Thread.Sleep(100);
			}
		}

		[Test]
		public void Test()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			for (int i = 0; i < 10; i++)
				t.Test();

			MethodCallCounter counter = CounterAspect.GetCounter(typeof(TestClass).GetMethod("Test"));

			Assert.AreEqual(10, counter.TotalCount);

			Console.WriteLine(counter.TotalTime);

			new Thread(new ThreadStart(t.LongTest)).Start();
			Thread.Sleep(20);

			lock (CounterAspect.Counters.SyncRoot) foreach (MethodCallCounter c in CounterAspect.Counters)
			{
				Console.WriteLine("{0}.{1,-10} | {2,2} | {3,2} | {4}",
					c.MethodInfo.DeclaringType.Name,
					c.MethodInfo.Name,
					c.TotalCount,
					c.CurrentCalls.Count,
					c.TotalTime);

				lock (c.CurrentCalls.SyncRoot) for (int i = 0; i < c.CurrentCalls.Count; i++)
				{
					InterceptCallInfo ci = (InterceptCallInfo)c.CurrentCalls[i];
					IPrincipal        pr = ci.CurrentPrincipal;

					Console.WriteLine("{0,15} | {1}",
						pr == null? "***" : pr.Identity.Name,
						DateTime.Now - ci.BeginCallTime);
				}
			}
		}

		public abstract class TestClass2
		{
			[Counter]
			public virtual void Test()
			{
			}
		}

		[Test]
		public void Test2()
		{
			// custom create counter delegate returns null
			CounterAspect.CreateCounter = mi => null;

			var t = (TestClass2)TypeAccessor.CreateInstance(typeof(TestClass2));

			// interceptor should fallback to default counter implementation
			t.Test();
		}

	}
}
