using System;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace EditableObjects
{
	[TestFixture]
	public class InnerObjectTest
	{
		public abstract class TestClass : EditableObject
		{
			public abstract TestClass1 Test1 { get; set; }
		}

		public abstract class TestClass1 : EditableObject
		{
			public TestClass1() {}
			public TestClass1(InitContext ctx) : base() {}

			[NoInstance]
			public abstract TestClass2 Test2 { get; set; }
		}

		public abstract class TestClass2 : EditableObject
		{
		}

		[Test]
		public void Test()
		{
			TestClass test = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));

			Assert.IsNull(test.Test1.Test2);
		}
	}
}
