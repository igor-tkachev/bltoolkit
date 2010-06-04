using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[TestFixture]
	public class NotNullTest
	{
		public /*[a]*/abstract/*[/a]*/ class TestObject
		{
			public /*[a]*/virtual/*[/a]*/ void Foo1(string str1, [/*[a]*/NotNull/*[/a]*/]              string str2, string str3) {}
			public /*[a]*/virtual/*[/a]*/ void Foo2(string str1, [/*[a]*/NotNull/*[/a]*/(/*[a]*/"Null"/*[/a]*/)]      string str2, string str3) {}
			public /*[a]*/virtual/*[/a]*/ void Foo3(string str1, [/*[a]*/NotNull/*[/a]*/(/*[a]*/"Null: {0}"/*[/a]*/)] string str2, string str3) {}

			public static TestObject CreateInstance() { return /*[a]*/TypeAccessor/*[/a]*/<TestObject>.CreateInstance(); }
		}

		[Test, ExpectedException(typeof(ArgumentNullException))] // Error message is localized by framework.
		public void Test1()
		{
			TestObject o = TestObject.CreateInstance();

			o.Foo1("str1", /*[a]*/null/*[/a]*/, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), ExpectedMessage="Null")]
		public void Test2()
		{
			TestObject o = TestObject.CreateInstance();

			o.Foo2("str1", /*[a]*/null/*[/a]*/, "str3");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), ExpectedMessage="Null: str2")]
		public void Test3()
		{
			TestObject o = TestObject.CreateInstance();

			o.Foo3("str1", /*[a]*/null/*[/a]*/, "str3");
		}
	}
}
