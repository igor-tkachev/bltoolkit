using System;
using System.Collections;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
	public class LoggingAspectTest
	{
		[Log]
		public abstract class TestClass
		{
			public abstract int Test();

			[Log("LogParameters=true")]
			public virtual void Test(ArrayList list, int i, string s, char c)
			{
			}

			[Log("LogParameters=true")]
			public virtual void Test(int i)
			{
				throw new ApplicationException("test exception");
			}
		}

		[Test]
		public void Test1()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			t.Test();
			t.Test(new ArrayList(), 567, "876", 'X');
		}

		[Test, ExpectedException(typeof(ApplicationException))]
		public void Test2()
		{
			TestClass t = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			t.Test(123);
		}
	}
}
