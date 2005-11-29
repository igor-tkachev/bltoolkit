using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class MapTest
	{
		[DefaultValue(Enum1.Value3)]
		public enum Enum1
		{
			[MapValue("1")] Value1,
			[NullValue]     Value2,
			[MapValue("3")] Value3
		}

		[Test]
		public void ToEnum()
		{
			Assert.AreEqual(Enum1.Value1, Map.ToEnum("1",         typeof(Enum1)));
			Assert.AreEqual(Enum1.Value2, Map.ToEnum(null,        typeof(Enum1)));
			Assert.AreEqual(Enum1.Value3, Map.ToEnum((Enum1)2727, typeof(Enum1)));
		}

		[Test]
		public void FromEnum()
		{
			Assert.AreEqual("1", Map.FromEnum(Enum1.Value1));
			Assert.IsNull  (     Map.FromEnum(Enum1.Value2));
			Assert.AreEqual("3", Map.FromEnum(Enum1.Value3));
		}
	}
}
