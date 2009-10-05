using System;

using NUnit.Framework;

using BLToolkit.Reflection;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessorTest3
	{
		class TestObject
		{
			private int _field = 15;
			public  int  Field
			{
				get { return _field; }
			}
		}

		[Test]
		public void Test()
		{
			var o  = TypeAccessor<TestObject>.CreateInstance();
			var ma = ExprMemberAccessor.GetMemberAccessor(TypeAccessor<TestObject>.Instance, "_field");

			ma.SetInt32(o, 5);

			Assert.AreEqual(5, o.Field);
		}

		[Test]
		public void TestAnonymous()
		{
			var o = new { Field1 = 1 };
			var a = TypeAccessor.GetAccessor(o);

			Assert.AreEqual(1, a["Field1"].GetInt32(o));
		}
	}
}
