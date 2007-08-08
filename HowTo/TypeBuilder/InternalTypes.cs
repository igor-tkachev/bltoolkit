using System;
using System.Runtime.CompilerServices;
using NUnit.Framework;

using BLToolkit.Reflection;

// typeof(TargetType).FullName + "." + TypeBuilderConsts.AssemblyNameSuffix + ".dll"
//
/*[a]*/[assembly: InternalsVisibleTo("TypeBuilder.InternalTypesTest.TestObject.TypeBuilder.dll")]/*[/a]*/

namespace TypeBuilder
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
			TestObject o = TypeAccessor.CreateInstance<TestObject>();
			Assert.IsNotNull(o);
		}
	}
}
