using System;

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
	}
}
