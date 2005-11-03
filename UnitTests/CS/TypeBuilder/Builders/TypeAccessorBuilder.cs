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
			TestObject1 o = (TestObject1)TypeAccessor.GetAccessor(typeof(TestObject1)).CreateInstance();
			Assert.AreEqual(10, o.Value);

			o = (TestObject1)TypeAccessor.GetAccessor(typeof(TestObject1)).CreateInstance(null);
			Assert.AreEqual(20, o.Value);

			InitContext ic = new InitContext();
			ic.Parameters = new object[] { 30 };
			o = (TestObject1)TypeAccessor.GetAccessor(typeof(TestObject1)).CreateInstance(ic);
			Assert.AreEqual(30, o.Value);
		}
	}
}
