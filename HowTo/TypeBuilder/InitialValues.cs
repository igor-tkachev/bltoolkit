using System;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace HowTo.TypeBuilder
{
	[TestFixture]
	public class InitialValueTest
	{
		[AttributeUsage(AttributeTargets.Property)]
		public class NewGuidParameterAttribute : /*[a]*/ParameterAttribute/*[/a]*/
		{
			public NewGuidParameterAttribute() : base(/*[a]*/Guid.NewGuid().ToByteArray()/*[/a]*/)
			{
			}
		}

		public abstract class TestObject1
		{
			/*[a]*/[Parameter("t")]/*[/a]*/   public abstract string   Str         { get; set; }
			/*[a]*/[Parameter(20)]/*[/a]*/    public abstract string   this[int i] { get; set; }
			/*[a]*/[Parameter(54)]/*[/a]*/    public abstract int      Int         { get; set; }
			/*[a]*/[Parameter(2,2,2)]/*[/a]*/ public abstract DateTime Date        { get; set; }
			/*[a]*/[Parameter(222L)]/*[/a]*/  public abstract Decimal  Decimal1    { get; set; }
			/*[a]*/[Parameter(-2.05)]/*[/a]*/ public abstract Decimal  Decimal2    { get; set; }
			/*[a]*/[NewGuidParameter]/*[/a]*/ public abstract Guid     Guid        { get; set; }
		}

		[Test]
		public void Test()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			Assert.That(o.Str,      Is.EqualTo("t"));
			Assert.That(o.Int,      Is.EqualTo(54));
			Assert.That(o.Date,     Is.EqualTo(new DateTime(2,2,2)));
			Assert.That(o.Decimal1, Is.EqualTo(222m));
			Assert.That(o.Decimal2, Is.EqualTo(-2.05m));
			Assert.That(o.Guid,     Is.Not.EqualTo(Guid.Empty));
		}
	}
}