using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	public /*[a]*/abstract/*[/a]*/ class OverloadTestObject
	{
		// This is a member we will overload.
		//
		public int /*[a]*/Test/*[/a]*/(/*[a]*/int intVal, string strVal/*[/a]*/)
		{
			return intVal;
		}

		// Overloaded methods simply calls a base method with same name
		// and has a few parameters less or more.
		//
		[/*[a]*/Overload/*[/a]*/] public abstract int /*[a]*/Test/*[/a]*/(/*[a]*/int intVal/*[/a]*/);
		[/*[a]*/Overload/*[/a]*/] public abstract int /*[a]*/Test/*[/a]*/(/*[a]*/string strVal/*[/a]*/);
		[/*[a]*/Overload/*[/a]*/] public abstract int /*[a]*/Test/*[/a]*/(int intVal, string strVal, /*[a]*/bool boolVal/*[/a]*/);
	}

	[TestFixture]
	public class OverloadAspectTest
	{
		[Test]
		public void OverloadTest()
		{
			OverloadTestObject o = /*[a]*/TypeAccessor/*[/a]*/<OverloadTestObject>.CreateInstance();

			Assert.AreEqual(1, o./*[a]*/Test/*[/a]*/(1));
			Assert.AreEqual(0, o./*[a]*/Test/*[/a]*/("str"));
		}
	}
}
