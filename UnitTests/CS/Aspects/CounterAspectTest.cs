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

			CounterAspect.Counter counter = CounterAspect.GetCounter(typeof(TestClass).GetMethod("Test"));

			Assert.AreEqual(10, counter.TotalCount);

			Console.WriteLine(counter.TotalTime);

			new Thread(new ThreadStart(t.LongTest)).Start();

			for (int j = 0; j < 5; j++)
			{
				Thread.Sleep(20);
				lock (CounterAspect.Counters.SyncRoot) foreach (CounterAspect.Counter c in CounterAspect.Counters)
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
						IPrincipal pr = (IPrincipal)ci.Items["CurrentPrincipal"];

						Console.WriteLine("{0,15} | {1}",
							pr == null ? "***" : pr.Identity.Name,
							DateTime.Now - ci.BeginCallTime);
					}
				}
			}
		}
	}
}
