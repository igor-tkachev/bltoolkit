using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class DefaultValueAttributeTest
	{
		[MapValue(Enum1.Value1, "1")]
		[MapValue(Enum1.Value3, "3")]
		[DefaultValue(Enum1.Value3)]
		public enum Enum1
		{
			Value1,
			[MapValue("2")] Value2,
			Value3
		}

		public class Object1
		{
			public Enum1 Enum1;
			[DefaultValue(Enum1.Value1)]
			public Enum1 Enum2;
		}

		[Test]
		public void TestEnum1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "Enum1", "55");
			om.SetValue(o, "Enum2", "66");

			Assert.AreEqual(Enum1.Value3, o.Enum1);
			Assert.AreEqual(Enum1.Value1, o.Enum2);

			Assert.AreEqual("3",  om.GetValue(o, "Enum1"));
			Assert.AreEqual("1",  om.GetValue(o, "Enum2"));
		}

		[MapValue(Enum2.Value1, "1")]
		[MapValue(Enum2.Value3, "3")]
		public enum Enum2
		{
			Value1,
			[MapValue("2")] Value2,
			[DefaultValue]  Value3
		}

		public class Object2
		{
			public Enum2 Enum1;
		}

		[Test]
		public void TestEnum2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = (Object2)om.CreateInstance();

			om.SetValue(o, "Enum1", "55");

			Assert.AreEqual(Enum2.Value3, o.Enum1);

			Assert.AreEqual("3",  om.GetValue(o, "Enum1"));
		}

		[DefaultValue(typeof(Enum2), Enum2.Value2)]
		public class Object3
		{
			public Enum2 Enum1;
		}

		[Test]
		public void TestEnum3()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object3));

			Object3 o = (Object3)om.CreateInstance();

			om.SetValue(o, "Enum1", "55");

			Assert.AreEqual(Enum2.Value2, o.Enum1);

			Assert.AreEqual("2",  om.GetValue(o, "Enum1"));
		}

		[DefaultValue(Enum2.Value2)]
		public class Object4
		{
			public Enum2 Enum1;
		}

		[Test]
		public void TestEnum4()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object4));

			Object4 o = (Object4)om.CreateInstance();

			om.SetValue(o, "Enum1", "55");

			Assert.AreEqual(Enum2.Value2, o.Enum1);

			Assert.AreEqual("2",  om.GetValue(o, "Enum1"));
		}
	}
}
