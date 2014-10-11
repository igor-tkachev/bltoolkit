using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessorTest2
	{
		public class TestObject
		{
			public int Field;
		}

		[Test]
		public void Test()
		{
			TestObject o1 = TypeAccessor<TestObject>.CreateInstance();
			TestObject o2 = TypeAccessor.CreateInstance<TestObject>();
		}

		public class TestObject1
		{
			public int?                             IntField;
			public Dictionary<int?, List<decimal?>> ListField = new Dictionary<int?, List<decimal?>>();
		}

		[Test]
		public void Write()
		{
			TestObject1 o = new TestObject1();

			TypeAccessor.WriteConsole(o);
			TypeAccessor.WriteDebug(o);
		}

		[Test]
		public void TestLongName()
		{
			EditableList<TestObject> o = TypeAccessor<EditableList<TestObject>>.CreateInstance();
		}
	}
}
