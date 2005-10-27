using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder
{
	[TestFixture]
	public class ParameterAttributes
	{
		public ParameterAttributes()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class AbstractObject
		{
			public AbstractObject(InitContext init)
			{
				if (init.MemberParameters != null && init.MemberParameters.Length == 1)
					Field = (int)init.MemberParameters[0];
				else
					Field = 77;
			}

			public int Field;
		}

		public class InnerObject
		{
			public InnerObject(InitContext init)
			{
				if (init.MemberParameters != null && init.MemberParameters.Length == 1)
					Field = (int)init.MemberParameters[0];
				else
					Field = 44;
			}

			public int Field;
		}

		public class TestField
		{
			public TestField()
			{
				Value = 10;
			}

			public TestField(int p1, float p2)
			{
				Value = p1 + (int)p2;
			}

			public TestField(TestField p1)
			{
				Value = 77;
			}

			public int Value;
		}

		[AttributeUsage(AttributeTargets.Property)]
		public class TestParameterAttribute : ParameterAttribute
		{
			public TestParameterAttribute()
				: base(new TestField())
			{
			}
		}

		public abstract class Object1
		{
			[Parameter(10)]     public abstract ArrayList      List           { get; set; }
			[Parameter("t")]    public abstract string         Str            { get; set; }
			[Parameter(20)]     public abstract string         this[int i]    { get; set; }
			[Parameter(20, 30)] public abstract TestField      Field1         { get; set; }
			[TestParameter]     public abstract TestField      Field2         { get; set; }
			[Parameter(55)]     public abstract InnerObject    InnerObject    { get; set; }
			//[Parameter(88)]     public abstract AbstractObject AbstractObject { get; set; }
			[Parameter(54)]     public abstract int            Int1           { get; set; }
			//[Parameter(null)]   public abstract int            Int2           { get; set; }
			[Parameter(2,2,2)]  public abstract DateTime       Date           { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			BuildContext context = TypeFactory.GetType(typeof(Object1));

			Console.WriteLine(context.Type.Type);

			Object1 o = (Object1)Activator.CreateInstance(context.Type);

			Assert.AreEqual(10,  o.List.Capacity);
			Assert.AreEqual("t", o.Str);
			Assert.AreEqual(50,  o.Field1.Value);
			Assert.AreEqual(77,  o.Field2.Value);
			Assert.AreEqual(55,  o.InnerObject.Field);
			Assert.AreEqual(54,  o.Int1);

			Assert.AreEqual(new DateTime(2,2,2),  o.Date);
		}
	}
}
