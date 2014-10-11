using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class EnumToValue
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
			object value = Map./*[a]*/EnumToValue/*[/a]*/(Gender1.Male);

			Assert.AreEqual("M", value);
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
			object value = Map./*[a]*/EnumToValue/*[/a]*/(Gender2.Male);

			Assert.AreEqual(2, value);
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
			object value = (int)Map./*[a]*/EnumToValue/*[/a]*/(Gender3.Male);

			Assert.AreEqual(2, value);
		}
	}
}
