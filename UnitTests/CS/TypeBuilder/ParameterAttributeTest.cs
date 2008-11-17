using System;
using System.Collections;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace TypeBuilder
{
	[TestFixture]
	public class ParameterAttributeTest
	{
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
			public TestParameterAttribute() : base(new TestField())
			{
			}
		}

		public abstract class TestObject1
		{
			[Parameter(10)]     public abstract ArrayList   List        { get; set; }
			[Parameter("t")]    public abstract string      Str         { get; set; }
			[Parameter(20)]     public abstract string      this[int i] { get; set; }
			[Parameter(20, 30)] public abstract TestField   Field1      { get; set; }
			[TestParameter]     public abstract TestField   Field2      { get; set; }
			[Parameter(55)]     public abstract InnerObject InnerObject { get; set; }
			[Parameter(54)]     public abstract int?        Int1        { get; set; }
			[Parameter(2,2,2)]  public abstract DateTime    Date        { get; set; }
			[Parameter(222L)]   public abstract Decimal     Decimal1    { get; set; }
			[Parameter(1, 0, 0, true, (byte)2)]
			                    public abstract Decimal?    Decimal2    { get; set; }
			[Parameter(new int[]{2, 0, 0, 0})]
			                    public abstract Decimal     Decimal3    { get; set; }
			[Parameter(22.05)]  public abstract Decimal     Decimal4    { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			Assert.That(o.List.Capacity,     Is.EqualTo(10));
			Assert.That(o.Str,               Is.EqualTo("t"));
			Assert.That(o.Field1.Value,      Is.EqualTo(50));
			Assert.That(o.Field2.Value,      Is.EqualTo(77));
			Assert.That(o.InnerObject.Field, Is.EqualTo(55));
			Assert.That(o.Int1,              Is.EqualTo(54));

			Assert.That(o.Date,              Is.EqualTo(new DateTime(2,2,2)));
			Assert.That(o.Decimal1,          Is.EqualTo(222m));
			Assert.That(o.Decimal2,          Is.EqualTo(-0.01m));
			Assert.That(o.Decimal3,          Is.EqualTo(2m));
			Assert.That(o.Decimal4,          Is.EqualTo(22.05m));
		}
	}
}
