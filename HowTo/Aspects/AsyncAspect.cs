using System;
using System.Threading;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	public /*[a]*/abstract/*[/a]*/ class AsyncTestObject
	{
		// This is the member we will call asynchronously.
		//
		public /*[a]*/int/*[/a]*/ /*[a]*/Test/*[/a]*/(/*[a]*/int intVal, string strVal/*[/a]*/)
		{
			Thread.Sleep(200);
			return intVal;
		}

		// Begin async methods should take the same parameter list as the Test method and return IAsyncResult.
		// Two additional parameters can be provided: AsyncCallback and state object.
		// 'Begin' prefix is a part of the default naming convention.
		//
		[/*[a]*/Async/*[/a]*/] public abstract /*[a]*/IAsyncResult/*[/a]*/ /*[a]*/BeginTest/*[/a]*/(/*[a]*/int intVal, string strVal/*[/a]*/);
		[/*[a]*/Async/*[/a]*/] public abstract /*[a]*/IAsyncResult/*[/a]*/ /*[a]*/BeginTest/*[/a]*/(/*[a]*/int intVal, string strVal/*[/a]*/, /*[a]*/AsyncCallback/*[/a]*/ callback);
		[/*[a]*/Async/*[/a]*/] public abstract /*[a]*/IAsyncResult/*[/a]*/ /*[a]*/BeginTest/*[/a]*/(/*[a]*/int intVal, string strVal/*[/a]*/, /*[a]*/AsyncCallback/*[/a]*/ callback, /*[a]*/object/*[/a]*/ state);

		// End async methods should take IAsyncResult and return the same type as the Test method.
		// 'End' prefix is a part of the default naming convention.
		//
		[/*[a]*/Async/*[/a]*/] public abstract /*[a]*/int/*[/a]*/          /*[a]*/EndTest/*[/a]*/  (/*[a]*/IAsyncResult/*[/a]*/ asyncResult);

		// Begin/End naming convention is not required. You can use any name 
		// if you provide the target method name as a parameter of the Async attribute.
		//
		[/*[a]*/Async/*[/a]*/("Test")]
		public abstract /*[a]*/IAsyncResult/*[/a]*/ AnyName(/*[a]*/int intVal/*[/a]*/, /*[a]*/string strVal/*[/a]*/, /*[a]*/AsyncCallback/*[/a]*/ callback, /*[a]*/object/*[/a]*/ state);

		[/*[a]*/Async/*[/a]*/("Test", typeof(int), typeof(string))]
		public abstract /*[a]*/int/*[/a]*/          AnyName(/*[a]*/IAsyncResult/*[/a]*/ asyncResult);
	}

	[TestFixture]
	public class AsyncAspect
	{
		[Test]
		public void AsyncTest()
		{
			AsyncTestObject o = /*[a]*/TypeAccessor/*[/a]*/<AsyncTestObject>.CreateInstance();

			IAsyncResult ar = o./*[a]*/BeginTest/*[/a]*/(1, "10");
			Assert.AreEqual(1, o./*[a]*/EndTest/*[/a]*/(ar));
		}

		private static void /*[a]*/CallBack/*[/a]*/(IAsyncResult ar)
		{
			Console.WriteLine("Callback");

			AsyncTestObject o = (AsyncTestObject)ar.AsyncState;
			o.EndTest(ar);
		}

		[Test]
		public void CallbackTest()
		{
			AsyncTestObject o = TypeAccessor<AsyncTestObject>.CreateInstance();

			o.BeginTest(2, null, /*[a]*/CallBack/*[/a]*/, /*[a]*/o/*[/a]*/);
		}

		[Test]
		public void AnyNameTest()
		{
			AsyncTestObject o = TypeAccessor<AsyncTestObject>.CreateInstance();

			IAsyncResult ar = o./*[a]*/AnyName/*[/a]*/(2, null, null, null);
			Assert.AreEqual(2, o./*[a]*/AnyName/*[/a]*/(ar));
		}
	}
}
