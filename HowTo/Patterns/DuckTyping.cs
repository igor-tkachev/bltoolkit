using System;

using NUnit.Framework;

using BLToolkit.Patterns;

namespace HowTo.Patterns
{
	//[TestFixture]
	public class DuckTyping
	{
		// By default, all interface methods are optional.
		// 
		public interface OptionalInterface
		{
			// If the source object does not implement the [b]Method1[/b], a NotImplementedException is thrown at [i]run time[/i].
			//
			void Method1();

			// If the source object does not implement the [b]Method2[/b], a TypeBuilderException is thrown at [i]build time[/i].
			//
			/*[a]*/[MustImplement]/*[/a]*/
			void Method2();

			// If the source object does not implement the [b]Method3[/b], an empty stub is genegated.
			// The return value and all output parameters will be set to default values.
			//
			/*[a]*/[MustImplement(false, false)]/*[/a]*/
			int Method3();
		}

		// The MustImplement attribute also can control the entire interface.
		//
		/*[a]*/[MustImplement]/*[/a]*/
		public interface RequiredInterface
		{
			// If the source object does not implement the [b]Method1[/b], a TypeBuilderException is thrown at [i]build time[/i].
			//
			void Method1();

			// If the source object does not implement the [b]Method2[/b], a NotImplementedException is thrown at [i]run time[/i].
			//
			/*[a]*/[MustImplement(false)]/*[/a]*/
			void Method2();

			// If the source object does not implement the [b]Method3[/b], an empty stub is genegated.
			// The return value and all output parameters will be set to default values.
			//
			/*[a]*/[MustImplement(false, false)]/*[/a]*/
			int Method3();
		}

		public class TestClass
		{
			public int Method3()
			{
				return 3;
			}
		}

		// Two or more interfaces can be mixed together.
		//
		public interface InterfaceMix : RequiredInterface, OptionalInterface
		{
		}

		//[Test]
		public void Test()
		{
			InterfaceMix      duck = /*[a]*/BLToolkit.Patterns.DuckTyping.Implement/*[/a]*/<InterfaceMix>(new TestClass());
			RequiredInterface duck1 = duck;
			OptionalInterface duck2 = duck;

			duck1.Method1();
			duck2.Method1();
		}
	}
}
