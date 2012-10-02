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
		public void BoolTest1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "Bool1", "Y");
			om.SetValue(o, "Bool2", "Yes");

			Assert.AreEqual(true, o.Bool1);
			Assert.AreEqual(true, o.Bool2);

			Assert.AreEqual("Y", om.GetValue(o, "Bool1"));
			Assert.AreEqual("Y", om.GetValue(o, "Bool2"));
		}

		[MapValue(true,  "Y")]
		[MapValue(false, "N")]
		public class Object2
		{
			public bool Bool1;

			[MapValue(true,  "Y", "Yes")]
			[MapValue(false, "N", "No")]
			public bool Bool2;
		}

		[Test]
		public void BoolTest2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = (Object2)om.CreateInstance();

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
			                Value3,
			[NullValue]     Value4
		}

		public class Object3
		{
			public Enum1 Enum1;
			public Enum1 Enum2;

			[MapValue(Enum1.Value1, "10")]
			[MapValue(Enum1.Value2, "20")]
			[MapValue(Enum1.Value3, "30")]
			[MapValue(Enum1.Value3, "32")]
			[MapValue(Enum1.Value3, "31")]
			public Enum1 Enum3;
			public Enum1 Enum4;
		}

		[Test]
		public void EnumTest1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object3));

			Object3 o = (Object3)om.CreateInstance();

			om.SetValue(o, "Enum1", "1");
			om.SetValue(o, "Enum2", "2");
			om.SetValue(o, "Enum3", "30");
			om.SetValue(o, "Enum4", null);

			Assert.AreEqual(Enum1.Value1, o.Enum1);
			Assert.AreEqual(Enum1.Value2, o.Enum2);
			Assert.AreEqual(Enum1.Value3, o.Enum3);
			Assert.AreEqual(Enum1.Value4, o.Enum4);

			om.SetValue(o, "Enum3", "31");
			Assert.AreEqual(Enum1.Value3, o.Enum3);

			om.SetValue(o, "Enum3", "32");
			Assert.AreEqual(Enum1.Value3, o.Enum3);

			Assert.AreEqual("1",                     om.GetValue(o, "Enum1"));
			Assert.AreEqual("2",                     om.GetValue(o, "Enum2"));
			Assert.Contains(om.GetValue(o, "Enum3"), new[] {"30", "31", "32", "3"});
			Assert.IsNull  (                         om.GetValue(o, "Enum4"));
		}

		[MapValue(typeof(DayOfWeek), DayOfWeek.Monday, "M")]
		[MapValue(                   DayOfWeek.Friday, "F")]
		public class Object4
		{
			public DayOfWeek Dow1;
			public DayOfWeek Dow2;
		}

		[Test]
		public void DayOfWeekTest1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object4));

			Object4 o = (Object4)om.CreateInstance();

			om.SetValue(o, "Dow1", "M");
			om.SetValue(o, "Dow2", "F");

			Assert.AreEqual(DayOfWeek.Monday, o.Dow1);
			Assert.AreEqual(DayOfWeek.Friday, o.Dow2);

			Assert.AreEqual("M", om.GetValue(o, "Dow1"));
			Assert.AreEqual("F", om.GetValue(o, "Dow2"));
		}

		// http://www.rsdn.ru/Forum/?mid=1809157
		//
		public enum StringAlignment
		{
			Far,
			Near,
			Center
		}

		public class SourceObject
		{
			public string test = "Near";
		}

		public class DestObject
		{
			[MapValue(StringAlignment.Near,   "Near")]
			[MapValue(StringAlignment.Far,    "Far")]
			[MapValue(StringAlignment.Center, "Center")]
			public StringAlignment test;
		}

		[Test]
		public void EnumTest()
		{
			SourceObject so = new SourceObject();
			DestObject o = (DestObject)Map.ObjectToObject(so, typeof(DestObject));

			Assert.AreEqual(StringAlignment.Near, o.test);
		}

		#region Nullable Enum

		public enum Enum2
		{
			[MapValue("Near")] Value1,
		}

		public class Object5
		{
			public Enum2? test;
		}

		[Test]
		public void NullableEnumTest()
		{
			SourceObject so = new SourceObject();

			Object5 b = Map.ObjectToObject<Object5>(so);
			Assert.AreEqual(Enum2.Value1, b.test);
		}

		#endregion
	}
}
