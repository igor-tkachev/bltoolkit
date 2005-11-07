using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder.Builders
{
	[TestFixture]
	public class TypeAccessorBuilder
	{
		public TypeAccessorBuilder()
		{
			TypeFactory.SaveTypes = true;
		}

		public class TestObject1
		{
			public TestObject1()
			{
				Value = 10;
			}

			public TestObject1(InitContext init)
			{
				Value = init == null || init.Parameters == null? 20: (int)init.Parameters[0];
			}

			public int Value;
		}

		[Test]
		public void Test1()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));
			Assert.AreEqual(10, o.Value);

			o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1), null);
			Assert.AreEqual(20, o.Value);

			InitContext ic = new InitContext();
			ic.Parameters = new object[] { 30 };
			o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1), ic);
			Assert.AreEqual(30, o.Value);
		}

		public class TestObject2
		{
			public TestObject2()
			{
				Value = 10;
			}

			public int Value;
		}

		[Test]
		public void Test2()
		{
			TestObject2 o = (TestObject2)TypeAccessor.CreateInstance(typeof(TestObject2));
			Assert.AreEqual(10, o.Value);

			o = (TestObject2)TypeAccessor.CreateInstance(typeof(TestObject2), null);
			Assert.AreEqual(10, o.Value);

			InitContext ic = new InitContext();
			ic.Parameters = new object[] { 30 };
			o = (TestObject2)TypeAccessor.CreateInstance(typeof(TestObject2), ic);
			Assert.AreEqual(10, o.Value);
		}

		public class TestObject3
		{
			public TestObject3(InitContext init)
			{
				Value = init == null || init.Parameters == null ? 20 : (int)init.Parameters[0];
			}

			public int Value;
		}

		[Test]
		public void Test3()
		{
			TestObject3 o = (TestObject3)TypeAccessor.CreateInstance(typeof(TestObject3));
			Assert.AreEqual(20, o.Value);

			o = (TestObject3)TypeAccessor.CreateInstance(typeof(TestObject3), null);
			Assert.AreEqual(20, o.Value);

			InitContext ic = new InitContext();
			ic.Parameters = new object[] { 30 };
			o = (TestObject3)TypeAccessor.CreateInstance(typeof(TestObject3), ic);
			Assert.AreEqual(30, o.Value);
		}

		public abstract class TestObject4
		{
			protected TestObject4(InitContext init)
			{
				Value = init == null || init.Parameters == null ? 20 : (int)init.Parameters[0];
			}

			class ParamAttribute : ParameterAttribute
			{
				public ParamAttribute() : base(new object()) { }
			}

			public class InnerObject
			{
				public InnerObject(object param) { }
			}

			[Param]
			public abstract InnerObject ObjValue { get; set; }

			public abstract int Value { get; set; }
		}

		[Test]
		public void Test4()
		{
			TestObject4 o = (TestObject4)TypeAccessor.CreateInstance(typeof(TestObject4));
			Assert.AreEqual(20, o.Value);

			o = (TestObject4)TypeAccessor.CreateInstance(typeof(TestObject4), null);
			Assert.AreEqual(20, o.Value);

			InitContext ic = new InitContext();
			ic.Parameters = new object[] { 30 };
			o = (TestObject4)TypeAccessor.CreateInstance(typeof(TestObject4), ic);
			Assert.AreEqual(30, o.Value);
		}
	}
}
