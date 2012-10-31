using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

using BLToolkit.Reflection;

// typeof(TargetType).FullName + "." + TypeBuilderConsts.AssemblyNameSuffix
//
/*[a]*/[assembly: InternalsVisibleTo("HowTo.TypeBuilder.InternalTypesTest.TestObject.TypeBuilder")]/*[/a]*/
/*[a]*/[assembly: InternalsVisibleTo("HowTo.TypeBuilder.InternalTypesTest.TestObject.TypeAccessor")]/*[/a]*/

namespace HowTo.TypeBuilder
{

	[TestFixture]
	public class InternalTypesTest
	{
		/*[a]*/internal/*[/a]*/ abstract class TestObject
		{
			public abstract string Value { get; set; }
		}

		[Test]
		public void Test()
		{
			var o = TypeAccessor.CreateInstance<TestObject>();
			Assert.IsNotNull(o);
		}
	}
}