using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class ObjectToObject
	{
		public class SourceObject
		{
			public bool   Value1   = true;
			public string Value2   = "10";
			public string StrValue = "test";
		}

		public class DestObject
		{
			[MapField("Value1")] public bool   BoolValue;
			[MapField("Value2")] public int    IntValue;

			// If the source and destination field/property names are equal,
			// there is no need for using the MapField attribute.
			//
			                     public string StrValue; 
		}

		[Test]
		public void Test1()
		{
			SourceObject source = new SourceObject();
			DestObject   dest   = Map./*[a]*/ObjectToObject/*[/a]*/<DestObject>(source);

			Assert.AreEqual(true,   dest.BoolValue);
			Assert.AreEqual(10,     dest.IntValue);
			Assert.AreEqual("test", dest.StrValue);
		}
	}
}
