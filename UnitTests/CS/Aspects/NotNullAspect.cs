using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	//[TestFixture]
	public class NotNullAspect
	{
		public NotNullAspect()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class Object1
		{
			public virtual void Foo1(string str1, [NotNull] string str2, string str3) {}
			public virtual void Foo2(string str1, [NotNull("Null")] string str2, string str3) { }
			public virtual void Foo3(string str1, [NotNull("Null: {0}")] string str2, string str3) { }
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: str2")]
		public void Test1()
		{
			Object1 o = (Object1)TypeAccessor.GetAccessor(typeof(Object1)).CreateInstance();

			o.Foo1("str1", null, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Null")]
		public void Test2()
		{
			Object1 o = (Object1)TypeAccessor.GetAccessor(typeof(Object1)).CreateInstance();

			o.Foo2("str1", null, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Null: str2")]
		public void Test3()
		{
			Object1 o = (Object1)TypeAccessor.GetAccessor(typeof(Object1)).CreateInstance();

			o.Foo3("str1", null, "str3");
		}
	}
}
