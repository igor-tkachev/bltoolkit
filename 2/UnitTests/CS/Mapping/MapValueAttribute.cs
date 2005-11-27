using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class MapValueAttributeTest
	{
		public class Object1
		{
			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public bool Bool1;

			[MapValue(true,  "Y", "Yes")]
			[MapValue(false, "N", "No")]
			public bool Bool2;
		}

		[Test]
		public void BoolTest()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "Bool1", "Y");
			om.SetValue(o, "Bool2", "Yes");

			Assert.AreEqual(true, o.Bool1);
			Assert.AreEqual(true, o.Bool2);

			Assert.AreEqual("Y", om.GetValue(o, "Bool1"));
			Assert.AreEqual("Y", om.GetValue(o, "Bool2"));
		}

		[MapValue(Enum1.Value1, "1")]
		[MapValue(Enum1.Value3, "3")]
		public enum Enum1
		{
			Value1,
			[MapValue("2")] Value2,
			Value3
		}

		public class Object2
		{
			public Enum1 Enum1;
			public Enum1 Enum2;

			[MapValue(Enum1.Value1, "10")]
			[MapValue(Enum1.Value2, "20")]
			[MapValue(Enum1.Value3, "30")]
			public Enum1 Enum3;
		}

		[Test]
		public void BoolEnum1()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = (Object2)om.CreateInstance();

			om.SetValue(o, "Enum1", "1");
			om.SetValue(o, "Enum2", "2");
			om.SetValue(o, "Enum3", "30");

			Assert.AreEqual(Enum1.Value1, o.Enum1);
			Assert.AreEqual(Enum1.Value2, o.Enum2);
			Assert.AreEqual(Enum1.Value3, o.Enum3);

			Assert.AreEqual("1",  om.GetValue(o, "Enum1"));
			Assert.AreEqual("2",  om.GetValue(o, "Enum2"));
			Assert.AreEqual("30", om.GetValue(o, "Enum3"));
		}
	}
}
