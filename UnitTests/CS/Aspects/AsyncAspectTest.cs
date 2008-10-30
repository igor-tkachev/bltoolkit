using System;
using System.Diagnostics;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace Aspects
{
	[TestFixture]
	public class AsyncAspectTest
	{
		private const int ExecutionTime = 200;

		public abstract class TestObject
		{
			public int Test(int intVal, string strVal)
			{
				System.Threading.Thread.Sleep(ExecutionTime + 10);
				return intVal;
			}

			[Async] public abstract IAsyncResult BeginTest(int intVal, string strVal);
			[Async] public abstract IAsyncResult BeginTest(int intVal, string strVal, AsyncCallback callback);
			[Async] public abstract IAsyncResult BeginTest(int intVal, string strVal, AsyncCallback callback, object state);
			[Async] public abstract int EndTest(IAsyncResult asyncResult);

			[Async("Test")]
			public abstract IAsyncResult AnyName(int intVal, string strVal, AsyncCallback callback, object state);
			[Async("Test", typeof(int), typeof(string))]
			public abstract int AnyName(IAsyncResult asyncResult);
		}

		public abstract class TestObject<T>
		{
			public T Test(T intVal)
			{
				System.Threading.Thread.Sleep(ExecutionTime + 10);
				return intVal;
			}

			[Async] public abstract IAsyncResult BeginTest(T intVal);
			[Async] public abstract T EndTest(IAsyncResult asyncResult);
		}

		[Test]
		public void AsyncTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstanceEx();
			Stopwatch sw = Stopwatch.StartNew();

			Assert.AreEqual(1, o.Test(1, null));
			long mss = sw.ElapsedMilliseconds;
			Assert.IsTrue(mss >= ExecutionTime);

			sw.Reset();
			sw.Start();

			IAsyncResult ar = o.BeginTest(2, "12");
			mss = sw.ElapsedMilliseconds;
			Assert.IsTrue(mss <= ExecutionTime);

			Assert.AreEqual(2, o.EndTest(ar));
			mss = sw.ElapsedMilliseconds;
			Assert.IsTrue(mss >= ExecutionTime);
		}

		[Test]
		public void GenericTest()
		{
			TestObject<DateTime> o = TypeAccessor<TestObject<DateTime>>.CreateInstanceEx();
			DateTime           now = DateTime.Now;
			IAsyncResult        ar = o.BeginTest(now);

			Assert.AreEqual(now, o.EndTest(ar));
		}

		private static void CallBack(IAsyncResult ar)
		{
			TestObject o = (TestObject) ar.AsyncState;
			Console.WriteLine("Callback");
			o.EndTest(ar);
		}

		[Test]
		public void CallbackTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstanceEx();

			o.BeginTest(2, null, new AsyncCallback(CallBack), o);
		}

		[Test]
		public void NoStateTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstanceEx();

			Assert.AreEqual(1, o.Test(1, null));

			IAsyncResult ar = o.BeginTest(2, null, null);
			Assert.AreEqual(2, o.EndTest(ar));
		}

		[Test]
		public void NoCallbackTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstanceEx();

			Assert.AreEqual(1, o.Test(1, null));

			IAsyncResult ar = o.BeginTest(2, "1234");
			Assert.AreEqual(2, o.EndTest(ar));
		}

		[Test]
		public void AnyNameTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstanceEx();

			IAsyncResult ar = o.AnyName(2, null, null, null);
			Assert.AreEqual(2, o.AnyName(ar));
		}
	}
}
