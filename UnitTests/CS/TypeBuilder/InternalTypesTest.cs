using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

[assembly: InternalsVisibleTo("InternalTypesTest.dll, PublicKey=00240000048000009400000006020000002400005253413100040000010001001d9a12fa5826334c27adac46b64048c08dc48a37113586f0b315baefeecad081ce1d907ef8879ea1dcea6decb9f0d87840ff60fc5bd2a3919469284481b6ae7b73ebb327503cd16c9ecd95b6ed9decc80116dfbe680dc1ad83c5aa89af3e48f5f9f94444901168e58a782f0831d88f6e00f47cd9eb209c40064fb5b002ef79be")]

namespace TypeBuilder
{
	[TestFixture]
	public class InternalTypesTest
	{
		internal abstract class TestObject
		{
			public abstract string Value { get; set; }
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;
			TypeFactory.SetGlobalAssembly("InternalTypesTest.dll", new Version(1,2,3,4), "TypeBuilder/InternalTypesTest.snk");

			TestObject o = TypeAccessor.CreateInstance<TestObject>();
			Assert.IsNotNull(o);

			TypeFactory.SaveGlobalAssembly();
		}
	}
}
