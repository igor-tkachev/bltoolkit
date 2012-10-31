using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class ValueToEnum
	{
		public enum Gender1
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		[Test]
		public void Test1()
		{
			Gender1 g = Map./*[a]*/ToEnum<Gender1>/*[/a]*/("M");

			Assert.AreEqual(Gender1.Male, g);
		}

		public enum Gender2
		{
			[MapValue(1)] Female,
			[MapValue(2)] Male,
			[MapValue(3)] Unknown,
			[MapValue(4)] Other
		}

		[Test]
		public void Test2()
		{
			Gender2 g = Map./*[a]*/ToEnum<Gender2>/*[/a]*/(2);

			Assert.AreEqual(Gender2.Male, g);
		}

		public enum Gender3
		{
			Female  = 1,
			Male    = 2,
			Unknown = 3,
			Other   = 4
		}

		[Test]
		public void Test3()
		{
			Gender3 g = Map./*[a]*/ToEnum<Gender3>/*[/a]*/(2);

			Assert.AreEqual(Gender3.Male, g);
		}
	}
}
