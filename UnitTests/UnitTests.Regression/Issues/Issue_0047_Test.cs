using System;
using System.Security.Principal;
using System.Threading;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
    public class Issue_0047_Test
	{
		public abstract class TestClass
		{
			[Counter]
			public virtual void Test()
			{
			}
		}

		[Test]
		public void Test()
		{
            // custom create counter delegate returns null
            CounterAspect.CreateCounter = mi => null;

			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

            // interceptor should fallback to default counter implementation
            t.Test();
		}
	}
}
