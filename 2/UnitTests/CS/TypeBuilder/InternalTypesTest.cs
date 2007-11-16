using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

[assembly: InternalsVisibleTo("InternalTypesTest, PublicKey=00240000048000009400000006020000002400005253413100040000010001001d9a12fa5826334c27adac46b64048c08dc48a37113586f0b315baefeecad081ce1d907ef8879ea1dcea6decb9f0d87840ff60fc5bd2a3919469284481b6ae7b73ebb327503cd16c9ecd95b6ed9decc80116dfbe680dc1ad83c5aa89af3e48f5f9f94444901168e58a782f0831d88f6e00f47cd9eb209c40064fb5b002ef79be")]

namespace TypeBuilder
{
	[TestFixture]
	public class InternalTypesTest
	{
		internal abstract class InternalObject
		{
			public abstract string PublicValue { get; set; }
		}

		public abstract class PublicObject
		{
			internal string InternalField;
			internal abstract string InternalValue { get; set; }
			internal protected abstract string ProtectedInternalValue { get; set; }
			public abstract string PublicValue { get; internal set; }
			public string NonAbstractValue
			{
				         get { return InternalField;  }
				internal set { InternalField = value; }
			}
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;
			TypeFactory.SetGlobalAssembly("InternalTypesTest.dll", new Version(1,2,3,4), "TypeBuilder/InternalTypesTest.snk");

			InternalObject o = TypeAccessor.CreateInstance<InternalObject>();
			Assert.IsNotNull(o);

			PublicObject o2 = TypeAccessor.CreateInstance<PublicObject>();
			Assert.IsNotNull(o2);

			TypeAccessor ta = TypeAccessor<PublicObject>.Instance;
			Assert.IsNotNull(ta["InternalField"]);
			Assert.IsTrue(ta["InternalField"].HasGetter);
			Assert.IsTrue(ta["InternalField"].HasSetter);
			Assert.IsNotNull(ta["PublicValue"]);
			Assert.IsTrue(ta["PublicValue"].HasGetter);
			Assert.IsTrue(ta["PublicValue"].HasSetter);
			Assert.IsNotNull(ta["InternalValue"]);
			Assert.IsTrue(ta["InternalValue"].HasGetter);
			Assert.IsTrue(ta["InternalValue"].HasSetter);
			Assert.IsNotNull(ta["ProtectedInternalValue"]);
			Assert.IsTrue(ta["ProtectedInternalValue"].HasGetter);
			Assert.IsTrue(ta["ProtectedInternalValue"].HasSetter);
			Assert.IsNotNull(ta["NonAbstractValue"]);
			Assert.IsTrue(ta["NonAbstractValue"].HasGetter);
			Assert.IsTrue(ta["NonAbstractValue"].HasSetter);


			TypeFactory.SaveGlobalAssembly();
		}
	}
}
