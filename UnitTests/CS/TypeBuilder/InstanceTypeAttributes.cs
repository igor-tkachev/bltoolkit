using System;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace UnitTests.CS.TypeBuilder
{
	[TestFixture]
	public class InstanceTypeAttributes
	{
		public InstanceTypeAttributes()
		{
			TypeFactory.SaveTypes = true;
		}

		public class IntFieldInstance
		{
			public int Value;
		}

		public abstract class Object1
		{
			[InstanceType(typeof(IntFieldInstance))]
			public abstract int Int { get; set; }
		}

		[Test]
		public void Test()
		{
			BuildContext context = TypeFactory.GetType(typeof(Object1));

			Console.WriteLine(context.Type.Type);

			Object1 o = (Object1)Activator.CreateInstance(context.Type);

			o.Int = 10;

			Assert.AreEqual(10, o.Int);
		}
	}
}
