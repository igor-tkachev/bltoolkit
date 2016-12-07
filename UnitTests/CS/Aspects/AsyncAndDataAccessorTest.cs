using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace Aspects
{
	[TestFixture]
	public class AsyncAndDataAccessorTest
	{
		public abstract class TestObject : DataAccessor
		{
#if FIREBIRD
			[SqlQuery("SELECT cast(@intVal as int) as intval From dual")]
#else
			[SqlQuery("SELECT @intVal")]
#endif
			public abstract int Test(int @intVal);

			[Async] public abstract IAsyncResult BeginTest(int intVal);
			[Async] public abstract int          EndTest(IAsyncResult asyncResult);
		}

		[Test]
		public void Test()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			IAsyncResult ar = o.BeginTest(42);
			Assert.AreEqual(42, o.EndTest(ar));
		}
	}
}
