using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Mapping.MemberMappers;
using BLToolkit.Reflection;
using NUnit.Framework;

namespace Mapping
{
	[TestFixture]
	public class MemberMapperDefaultMappersTest
	{
		[XmlType(AnonymousType = true)]
		[Serializable]
		public abstract class Entity : EditableObject<Entity>
		{
			public abstract int    IntValue    { get; set; }
			public abstract string StringValue { get; set; }

			public override bool Equals(object obj)
			{
				var b = obj as Entity;
				if (b == null)
					return false;

				return IntValue == b.IntValue && StringValue == b.StringValue;
			}

			public override int GetHashCode()
			{
				return StringValue.GetHashCode() ^ IntValue;
			}
		}

		public class Object1
		{
			[MemberMapper(typeof(XMLSerialisationMapper))]
			public List<object> Lst1;
			[MemberMapper(typeof(BinarySerialisationMapper))]
			public List<object> Lst2;
			[MemberMapper(typeof(BinarySerialisationToBase64StringMapper))]
			public List<object> Lst3;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			public List<object> Lst4;
		}

		[XmlIncludeAbstract(typeof(Entity))]
		public class Object2
		{
			[MemberMapper(typeof(XMLSerialisationMapper))]
			public List<Entity> Lst1;
			[MemberMapper(typeof(BinarySerialisationMapper))]
			public List<Entity> Lst2;
			[MemberMapper(typeof(BinarySerialisationToBase64StringMapper))]
			public List<Entity> Lst3;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			public List<Entity> Lst4;
			[MemberMapper(typeof(XMLSerialisationMapper))]
			public Entity Entity1;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			public Entity Entity2;
		}

		[Test]
		public void DefaultMemberMapperTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			o.Lst1 = new List<object>() { "Hello", "this", "is", "a", "test", 1, 2, 3 };
			o.Lst2 = o.Lst1;
			o.Lst3 = o.Lst1;
			o.Lst4 = o.Lst1;

			var xml = om.GetValue(o, "Lst1");
			var bytearr = om.GetValue(o, "Lst2");
			var base64 = om.GetValue(o, "Lst3");
			var json = om.GetValue(o, "Lst4");

			var o2 = new Object1();
			om.SetValue(o2, "Lst1", xml);
			om.SetValue(o2, "Lst2", bytearr);
			om.SetValue(o2, "Lst3", base64);
			om.SetValue(o2, "Lst4", json);

			Assert.IsTrue(o.Lst1.SequenceEqual(o2.Lst1));
			Assert.IsTrue(o.Lst2.SequenceEqual(o2.Lst2));
			Assert.IsTrue(o.Lst3.SequenceEqual(o2.Lst3));
			Assert.IsTrue(o.Lst4.SequenceEqual(o2.Lst4));   
		}

		[Test]
		public void DefaultMemberMapperNullTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			var xml     = om.GetValue(o, "Lst1");
			var bytearr = om.GetValue(o, "Lst2");
			var base64  = om.GetValue(o, "Lst3");
			var json    = om.GetValue(o, "Lst4");

			var o2 = new Object1();
			om.SetValue(o2, "Lst1", xml);
			om.SetValue(o2, "Lst2", bytearr);
			om.SetValue(o2, "Lst3", base64);
			om.SetValue(o2, "Lst4", json);

			Assert.IsNull(o2.Lst1);
			Assert.IsNull(o2.Lst2);
			Assert.IsNull(o2.Lst3);
			Assert.IsNull(o2.Lst4);
		}

		[Test]
		public void DefaultMemberMapperTest2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			var o = new Object2();

			var entity         = Entity.CreateInstance();
			entity.IntValue    = 10;
			entity.StringValue = "string";

			o.Lst1    = new List<Entity>() { entity };
			o.Lst2    = o.Lst1;
			o.Lst3    = o.Lst1;
			o.Lst4    = o.Lst1;
			o.Entity1 = entity;
			o.Entity2 = entity;

			var xml     = om.GetValue(o, "Lst1");
			var bytearr = om.GetValue(o, "Lst2");
			var base64  = om.GetValue(o, "Lst3");
			var json    = om.GetValue(o, "Lst4");
			var xml2    = om.GetValue(o, "Entity1");
			var json2   = om.GetValue(o, "Entity2");

			var o2 = new Object2();
			om.SetValue(o2, "Lst1",    xml);
			om.SetValue(o2, "Lst2",    bytearr);
			om.SetValue(o2, "Lst3",    base64);
			om.SetValue(o2, "Lst4",    json);
			om.SetValue(o2, "Entity1", xml2);
			om.SetValue(o2, "Entity2", json2);

			Assert.IsTrue(o.Lst1.SequenceEqual(o2.Lst1));
			Assert.IsTrue(o.Lst2.SequenceEqual(o2.Lst2));
			Assert.IsTrue(o.Lst3.SequenceEqual(o2.Lst3));
			Assert.IsTrue(o.Lst4.SequenceEqual(o2.Lst4));

			Assert.AreEqual(o.Entity1, o2.Entity1);
			Assert.AreEqual(o.Entity2, o2.Entity2);
		}
	}
}
