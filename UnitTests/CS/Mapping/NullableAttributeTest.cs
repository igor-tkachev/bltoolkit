using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class NullableAttributeTest
	{
		public abstract class Object1
		{
			public abstract string Str1 { get; set; }
			[Nullable]
			public abstract string Str2 { get; set; }
			[NullValue("(null)")]
			public abstract string Str3 { get; set; }
			[NullValue(typeof(DBNull))]
			public abstract string Str4 { get; set; }
		}

		[Test]
		public void TestString1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "Str1", null);
			om.SetValue(o, "Str2", null);
			om.SetValue(o, "Str3", null);
			om.SetValue(o, "Str4", null);

			Assert.AreEqual("",       o.Str1);
			Assert.AreEqual("",       o.Str2);
			Assert.AreEqual("(null)", o.Str3);
			Assert.IsNull  (o.Str4);

			Assert.IsNotNull(om.GetValue(o, "Str1"));
			Assert.IsNull   (om.GetValue(o, "Str2"));
			Assert.IsNull   (om.GetValue(o, "Str3"));
			Assert.IsNull   (om.GetValue(o, "Str4"));
		}

		[NullValue(typeof(string), "(null)")]
		[NullValue(typeof(bool),   false)]
		public abstract class Object2
		{
			[Nullable(false)]
			public abstract string Str1 { get; set; }
			[NullValue("")]
			public abstract string Str2 { get; set; }
			public abstract string Str3 { get; set; }
			[NullValue(typeof(DBNull))]
			public abstract string Str4 { get; set; }
		}

		[Test]
		public void TestString2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = (Object2)om.CreateInstance();

			om.SetValue(o, "Str1", null);
			om.SetValue(o, "Str2", null);
			om.SetValue(o, "Str3", null);
			om.SetValue(o, "Str4", null);

			Assert.AreEqual("",       o.Str1);
			Assert.AreEqual("",       o.Str2);
			Assert.AreEqual("(null)", o.Str3);
			Assert.IsNull  (o.Str4);

			Assert.IsNotNull(om.GetValue(o, "Str1"));
			Assert.IsNull   (om.GetValue(o, "Str2"));
			Assert.IsNull   (om.GetValue(o, "Str3"));
			Assert.IsNull   (om.GetValue(o, "Str4"));
		}

		[Nullable(typeof(string))]
		public abstract class Object3
		{
			[Nullable(false)]
			public abstract string Str1 { get; set; }
			public abstract string Str2 { get; set; }
			[NullValue("(null)")]
			public abstract string Str3 { get; set; }
			[NullValue(typeof(DBNull))]
			public abstract string Str4 { get; set; }
		}

		[Test]
		public void TestString3()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object3));

			Object3 o = (Object3)om.CreateInstance();

			om.SetValue(o, "Str1", null);
			om.SetValue(o, "Str2", null);
			om.SetValue(o, "Str3", null);
			om.SetValue(o, "Str4", null);

			Assert.AreEqual("",       o.Str1);
			Assert.AreEqual("",       o.Str2);
			Assert.AreEqual("(null)", o.Str3);
			Assert.IsNull  (o.Str4);

			Assert.IsNotNull(om.GetValue(o, "Str1"));
			Assert.IsNull   (om.GetValue(o, "Str2"));
			Assert.IsNull   (om.GetValue(o, "Str3"));
			Assert.IsNull   (om.GetValue(o, "Str4"));
		}

		[NullValue("(null)")]
		public abstract class Object4
		{
			[Nullable(false)]
			public abstract string Str1 { get; set; }
			[NullValue("")]
			public abstract string Str2 { get; set; }
			public abstract string Str3 { get; set; }
			[NullValue(typeof(DBNull))]
			public abstract string Str4 { get; set; }
		}

		[Test]
		public void TestString4()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = (Object2)om.CreateInstance();

			om.SetValue(o, "Str1", null);
			om.SetValue(o, "Str2", "2");
			om.SetValue(o, "Str3", null);
			om.SetValue(o, "Str4", null);

			Assert.AreEqual("",       o.Str1);
			Assert.AreEqual("2",      o.Str2);
			Assert.AreEqual("(null)", o.Str3);
			Assert.IsNull  (o.Str4);

			Assert.IsNotNull(     om.GetValue(o, "Str1"));
			Assert.AreEqual ("2", om.GetValue(o, "Str2"));
			Assert.IsNull   (     om.GetValue(o, "Str3"));
			Assert.IsNull   (     om.GetValue(o, "Str4"));
		}

		public class Object5
		{
			public int Int1;
			[Nullable]
			public int Int2;
			[NullValue(int.MinValue)]
			public int Int3;
		}

		[Test]
		public void TestPrimitive()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object5));

			Object5 o = (Object5)om.CreateInstance();

			om.SetValue(o, "Int1", null);
			om.SetValue(o, "Int2", null);
			om.SetValue(o, "Int3", null);

			Assert.AreEqual(0,            o.Int1);
			Assert.AreEqual(0,            o.Int2);
			Assert.AreEqual(int.MinValue, o.Int3);

			Assert.IsNotNull(om.GetValue(o, "Int1"));
			Assert.IsNull   (om.GetValue(o, "Int2"));
			Assert.IsNull   (om.GetValue(o, "Int3"));
		}
	}
}
