using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class MapValue2
	{
		public enum Gender1
		{
			[/*[a]*/MapValue("F")/*[/a]*/] Female,
			[/*[a]*/MapValue("M")/*[/a]*/] Male,
			[/*[a]*/MapValue("U")/*[/a]*/] Unknown,
			[/*[a]*/MapValue("O")/*[/a]*/] Other
		}

		[Test]
		public void Test1()
		{
			object value = Map.EnumToValue(Gender1.Male);

			Assert.AreEqual("M", value);
		}

		[/*[a]*/MapValue(Gender2.Female,  1)/*[/a]*/]
		[/*[a]*/MapValue(Gender2.Male,    2)/*[/a]*/]
		[/*[a]*/MapValue(Gender2.Unknown, 3)/*[/a]*/]
		[/*[a]*/MapValue(Gender2.Other,   4)/*[/a]*/]
		public enum Gender2
		{
			Female,
			Male,
			Unknown,
			Other
		}

		[Test]
		public void Test2()
		{
			Gender2 g = Map.ToEnum<Gender2>(2);

			Assert.AreEqual(Gender2.Male, g);
		}
	}
}
