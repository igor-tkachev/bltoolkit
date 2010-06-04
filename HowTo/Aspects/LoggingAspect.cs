using System;
using System.Threading;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class LoggingAspectTest
	{
		[/*[a]*/Log/*[/a]*/]
		public /*[a]*/abstract/*[/a]*/ class TestClass
		{
			// Here we customize the logging settings.
			// This call will be logged in spite of default settings.
			//
			[/*[a]*/Log/*[/a]*/(/*[a]*/MinCallTime=50/*[/a]*/)]
			public /*[a]*/virtual/*[/a]*/ void Test1(int i)
			{
				Thread.Sleep(/*[a]*/100/*[/a]*/);
			}

			// This call is not going to be logged (see default settings below).
			//
			public /*[a]*/virtual/*[/a]*/ void Test2(DateTime dt)
			{
				Thread.Sleep(/*[a]*/100/*[/a]*/);
			}

			// By default exception calls are logged.
			//
			public /*[a]*/virtual/*[/a]*/ void Test3(string s)
			{
				throw new ApplicationException("Test exception.");
			}
		}

		[Test]
		public void Test()
		{
			// By setting MinCallTime to some value, we prevent logging any call
			// which is shorter than the provided value.
			//
			LoggingAspect.MinCallTime = /*[a]*/1000/*[/a]*/;

			TestClass t = /*[a]*/TypeAccessor/*[/a]*/<TestClass>.CreateInstance();

			t.Test1(1);
			t.Test2(DateTime.Now);

			try
			{
				t.Test3("3");
			}
			catch
			{
			}
		}
	}
}
