using System;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace A.TypeBuilder.Builders
{
	[TestFixture]
	public class ArrayBuilderTest
	{
		public ArrayBuilderTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject
		{
			public abstract int[,]   DimArray  { get; set; }
			public abstract int[]    IntArray1 { get; set; }
			[LazyInstance]
			public abstract int[]    IntArray2 { get; set; }
			public abstract byte[][] ByteArray { get; set; }
		}

		[Test]
		public void AbstractProperties()
		{
			TestObject o = (TestObject)TypeAccessor.CreateInstance(typeof(TestObject));

			Assert.IsNotNull(o.IntArray1);
			Assert.AreSame(o.IntArray1, o.IntArray2);

			Assert.IsNotNull(o.ByteArray);
		}
	}
}
