using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	[TestFixture]
	public class OverloadAspectTest
	{
		public abstract class TestObject<T>
		{
			public T Test(T inVal, out T outVal)
			{
				outVal = inVal;
				return inVal;
			}

			public string Test(DateTime dateVal, string inVal)
			{
				return inVal.ToUpper();
			}

			[Overload] abstract public T Test(T inVal);
			[Overload] abstract public T Test(T inVal, out T outVal, ref T refVal);
			[Overload] abstract public T Test(T inVal, DateTime dateVal);
		}

		public abstract class TestObject
		{
			public int    IntValue;
			public string StrValue;
			public Guid   GuidValue;

			public void Test(int intVal, Guid guidVal)
			{
				IntValue  = intVal;
				GuidValue = guidVal;
				StrValue  = "(default)";
			}

			public void Test(int intVal, string strVal)
			{
				IntValue  = intVal;
				StrValue  = strVal;
				GuidValue = Guid.Empty;
			}

			public void OutRefTest(int inVal, out int outVal, ref int refVal)
			{
				outVal = inVal;
				refVal += inVal;
			}

			public void OutRefStructTest(int? inVal, out int? outVal, ref int? refVal)
			{
				outVal = inVal;
				refVal += inVal;
			}

			protected static int StaticMethod(int intVal, ref Guid guidVal)
			{
				return intVal;
			}

			public T Generic<T>(T inVal, out T outVal)
			{
				outVal = inVal;
				return inVal;
			}

			// Becomes
			// public override void Test(Guid guidVal)
			// {
			//   Test(default(int), guidVal);
			// }
			//
			[Overload] public abstract void Test(Guid guidVal);

			// Becomes
			// public override void Test(Guid guidVal, int intVal)
			// {
			//   Test(intVal, guidVal);
			// }
			//
			[Overload] public abstract void Test(Guid guidVal, int intVal);

			// Becomes
			// public override void Test(string strVal)
			// {
			//   Test(default(int), strVal);
			// }
			//
			[Overload]
			public abstract void Test(string strVal);

			// Overload method name may be altered.
			//
			[Overload("Test")]
			public abstract void GuidTest(Guid guidVal, DateTime dateVal);

			// Parameter types of the method to overload.
			//
			[Overload(typeof(int), typeof(Guid))]
			public abstract void Test(DateTime strVal);

			// There may be more or less parameters in the overloaded method.
			//
			[Overload] public abstract void OutRefTest(int inVal, out int outVal, ref int refVal, out string strVal);
			[Overload] public abstract void OutRefTest(int inVal);

			// Value types and ref value types also works.
			//
			[Overload] public abstract void OutRefStructTest(int? inVal, out int? outVal, ref int? refVal, out Guid guidVal);
			[Overload] public abstract void OutRefStructTest(int? inVal);

			// We can overload static methods.
			//
			[Overload] public abstract int StaticMethod(int intVal);

			// We can overload methods declared in a base type.
			//
			[Overload] public abstract string ToString(int intVal);

			// A generic method can be overloaded by an other generic method.
			//
			[Overload] public abstract T Generic<T>(T inVal);
		}

		[Test]
		public void OverloadTest()
		{
			TypeFactory.SaveTypes = true;

			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			o.Test(12345, "str");

			Assert.AreEqual(12345, o.IntValue);
			Assert.AreEqual("str", o.StrValue);
			Assert.AreEqual(Guid.Empty, o.GuidValue);

			o.Test(Guid.NewGuid(), 123);

			Assert.AreEqual(123, o.IntValue);
			Assert.AreEqual("(default)", o.StrValue);
			Assert.AreNotEqual(Guid.Empty, o.GuidValue);

			o.Test("foo");

			Assert.AreEqual(0, o.IntValue);
			Assert.AreEqual("foo", o.StrValue);
			Assert.AreEqual(Guid.Empty, o.GuidValue);

			o.Test(Guid.NewGuid());

			Assert.AreEqual(0, o.IntValue);
			Assert.AreEqual("(default)", o.StrValue);
			Assert.AreNotEqual(Guid.Empty, o.GuidValue);

			Assert.AreEqual(1, o.Generic(1));
		}

		[Test]
		public void AnyNameTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			o.GuidTest(Guid.NewGuid(), DateTime.Now);

			Assert.AreEqual(0, o.IntValue);
			Assert.AreEqual("(default)", o.StrValue);
			Assert.AreNotEqual(Guid.Empty, o.GuidValue);
		}

		[Test]
		public void ExplicitParameterTypesTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			o.Test(DateTime.Now);

			Assert.AreEqual(0, o.IntValue);
			Assert.AreEqual("(default)", o.StrValue);
			Assert.AreEqual(Guid.Empty, o.GuidValue);
		}

		[Test]
		public void StaticMethodTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			int intVal = o.StaticMethod(123);

			Assert.AreEqual(123, intVal);
		}

		[Test]
		public void BaseTypeMethodTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			Assert.AreEqual(o.ToString(), o.ToString(123));
		}

		[Test]
		public void OutRefTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			const int inVal = 123;
			int refVal = 99;
			int outVal;
			string strVal;
			o.OutRefTest(inVal, out outVal, ref refVal, out strVal);

			Assert.AreEqual(inVal, outVal);
			Assert.AreEqual(222, refVal);
			Assert.AreEqual(string.Empty, strVal);

			o.OutRefTest(inVal);
		}

		[Test]
		public void OutRefStructTest()
		{
			TestObject o = TypeAccessor<TestObject>.CreateInstance();

			int? inVal = 123;
			int? refVal = 99;
			int? outVal;
			Guid guidVal;
			o.OutRefStructTest(inVal, out outVal, ref refVal, out guidVal);

			Assert.AreEqual(inVal, outVal);
			Assert.AreEqual(222, refVal);
			Assert.AreEqual(Guid.Empty, guidVal);

			o.OutRefStructTest(inVal);
		}

		[Test]
		public void GenericTypeTest()
		{
			TestObject<int?> o = TypeAccessor<TestObject<int?>>.CreateInstance();

			int? inVal = 123;
			int? refVal = 99;
			int? outVal;
			o.Test(inVal, out outVal, ref refVal);

			Assert.AreEqual(inVal, outVal);
			Assert.AreEqual(refVal, 99);

			Assert.AreEqual(inVal, o.Test(inVal));
			Assert.AreEqual(12, o.Test(12, DateTime.Now));

			TestObject<string> o2 = TypeAccessor<TestObject<string>>.CreateInstance();

			// When T is a string, method Test(DateTime, string) becomes the best match.
			//
			Assert.AreEqual("STR", o2.Test("str", DateTime.Now));
		}

	}
}