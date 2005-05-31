/// example:
/// maptype ctor(Type)
/// comment:
/// The following example demonstrates how to use the <b>MapTypeAttribute</b> attribute.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapTypeAttribute
{
	[TestFixture]
	public class ctor_Type
	{
		public class MyInt1
		{
			public int Value { get { return 1; } }
		}

		public class MyInt2
		{
			public int Value { get { return 2; } }
		}

		public class MyInt3
		{
			public int Value { get { return 3; } }
		}

		[MapType(typeof(int), typeof(MyInt2))]
		public abstract class BaseEntity
		{
		}

		public interface IEntity
		{
			[MapType(typeof(MyInt3))]
			int Property3 { get; }
		}

		public abstract class TestEntity : BaseEntity, IEntity
		{
			// Implementation property type is set explicitly.
			//
			[MapType(typeof(MyInt1))]
			public abstract int Property1 { get; }

			// Implementation type is taken from BaseEntity.
			// MyInt2 is propagated to all current and child classes properties of int type.
			//
			public abstract int Property2 { get; }
		
			// Implementation type is taken from IEntity.
			// This way has higher priority than previous one.
			//
			public abstract int Property3 { get; }
		}

		[Test]
		public void Test()
		{
			TestEntity te = (TestEntity)Map.Descriptor(typeof(TestEntity)).CreateInstance();

			Assert.AreEqual(1, te.Property1);
			Assert.AreEqual(2, te.Property2);
			Assert.AreEqual(3, te.Property3);
		}
	}
}
