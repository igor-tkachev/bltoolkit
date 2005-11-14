using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Reflection;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessor2
	{
		public class TestObject
		{
			public int Field;
		}

		[Test]
		public void Test()
		{
			TestObject o1 = TypeAccessor<TestObject>.GetAccessor().CreateInstance();

			TestObject o2 = TypeAccessor.CreateInstance<TestObject>();
		}

		public class TestObject1
		{
			public int?                             IntField;
			public Dictionary<int?, List<decimal?>> ListField = new Dictionary<int?,List<decimal?>>();
		}

		[Test]
		public void Write()
		{
			TestObject1 o = new TestObject1();

			TypeAccessor.WriteConsole(o);
			TypeAccessor.WriteDebug(o);
		}
	}
}
