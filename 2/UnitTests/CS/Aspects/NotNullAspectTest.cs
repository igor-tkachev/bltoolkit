using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	[TestFixture]
	public class NotNullAspectTest
	{
		public NotNullAspectTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject1
		{
			public virtual void Foo1(string str1, [NotNull] string str2, string str3) {}
			public virtual void Foo2(string str1, [NotNull("Null")] string str2, string str3) { }
			public virtual void Foo3(string str1, [NotNull("Null: {0}")] string str2, string str3) { }
		}

		[Test, ExpectedException(typeof(ArgumentNullException))] // Error message is localized by framework.
		public void Test1()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			o.Foo1("str1", null, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Null")]
		public void Test2()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			o.Foo2("str1", null, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Null: str2")]
		public void Test3()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			o.Foo3("str1", null, "str3");
		}
	}
}
