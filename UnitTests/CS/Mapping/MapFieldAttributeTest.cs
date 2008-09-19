using System;
using System.Collections.Generic;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class MapFieldAttributeTest
	{
		[MapField("MapName", "Field1")]
		public class Object1
		{
			public int Field1;
			[MapField("intfld")]
			public int Field2;
		}

		[Test]
		public void Test1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "mapname", 123);
			om.SetValue(o, "intfld",  234);

			Assert.AreEqual(123, o.Field1);
			Assert.AreEqual(234, o.Field2);
		}

		[MapValue(true,  "Y")]
		[MapValue(false, "N")]
		public class Object2
		{
			public bool Field1;
			public int  Field2;
		}

		public class Object3
		{
			public Object2 Object2 = new Object2();
			public Object4 Object4;
		}

		[MapField("fld1", "Object3.Object2.Field1")]
		[MapField("fld2", "Object3.Object4.Str1")]
		public class Object4
		{
			public Object3 Object3 = new Object3();
			public string  Str1;
		}

		[Test]
		public void Test2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object4));

			Object4 o = (Object4)om.CreateInstance();

			om.SetValue(o, "fld1", "Y");
			om.SetValue(o, "Object3.Object2.Field2", 123);
			om.SetValue(o, "fld2", "str");

			Assert.AreEqual(true, o.Object3.Object2.Field1);
			Assert.AreEqual(123,  o.Object3.Object2.Field2);
			Assert.IsNull  (      o.Object3.Object4);

			Assert.AreEqual("Y", om.GetValue(o, "fld1"));
			Assert.AreEqual(123, om.GetValue(o, "Object3.Object2.Field2"));
			Assert.IsNull  (     om.GetValue(o, "fld2"));
		}

		[Test]
		public void Test3()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object4));
			MemberMapper mm = om["Object3.Object2.Field1", true];

			Assert.IsNotNull(mm);
		}

		[MapField("fld2", "Object3.Object4.Str1")]
		[TypeExtension(FileName="Map.xml")]
		public class Object5
		{
			public Object3 Object3 = new Object3();
			public string  Str1;
		}

		[Test]
		public void Test4()
		{
			ObjectMapper om  = Map.GetObjectMapper(typeof(Object5));
			MemberMapper mm1 = om["Object3.Object2.Field1", true];
			MemberMapper mm2 = om["Object3.Object4.Str1", true];

			Assert.IsNotNull(mm1);
			Assert.IsNotNull(mm2);
		}

		public class Entity
		{
			[PrimaryKey]
			public int Id;
		}

		public class Name
		{
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[MapField("PhoneId", "Id")]
		public class PhoneEntity : Entity
		{
			public string Phone;
		}

		[MapField("PhoneId",    "Phone.Id")]
		[MapField("LastName",   "Name.LastName")]
		[MapField("FirstName",  "Name.FirstName")]
		[MapField("MiddleName", "Name.MiddleName")]
		public class PersonEntity : Entity
		{
			public PhoneEntity Phone;
			public Name        Name   = new Name();
		}

		[Test]
		public void InnerTest1()
		{
			ObjectMapper om = ObjectMapper<PersonEntity>.Instance;
			List<string> list = new List<MemberMapper>(om)
				.ConvertAll<string>(delegate(MemberMapper mm) { return mm.Name; });

			Assert.That(list.Count == 5);
			Assert.That(list.Contains("PhoneId"));
			Assert.That(list.Contains("Phone"), Is.False);
			Assert.That(list.Contains("LastName"));
			Assert.That(list.Contains("Name"), Is.False);
		}

		[MapField("PhoneId", "Phone.Id")]
		public class PersonEntity2 : Entity
		{
			[MapIgnore]
			public PhoneEntity Phone = new PhoneEntity();
			[MapField(Format="{0}")]
			public Name        Name  = new Name();
		}

		[Test]
		public void InnerTest2()
		{
			ObjectMapper om = ObjectMapper<PersonEntity2>.Instance;
			List<string> list = new List<MemberMapper>(om)
				.ConvertAll<string>(delegate(MemberMapper mm) { return mm.Name; });

			Assert.That(list.Count == 5);
			Assert.That(list.Contains("PhoneId"));
			Assert.That(list.Contains("Phone"), Is.False);
			Assert.That(list.Contains("LastName"));
			Assert.That(list.Contains("Name"), Is.False);
		}

		[MapField("PhoneId", "Phone.Id")]
		[MapField("Name", Format="{0}")]
		public class PersonEntity3 : Entity
		{
			[MapIgnore]
			public PhoneEntity Phone = new PhoneEntity();
			public Name        Name  = new Name();
		}

		[Test]
		public void InnerTest3()
		{
			ObjectMapper om = ObjectMapper<PersonEntity3>.Instance;
			List<string> list = new List<MemberMapper>(om)
				.ConvertAll<string>(delegate(MemberMapper mm) { return mm.Name; });

			Assert.That(list.Count == 5);
			Assert.That(list.Contains("PhoneId"));
			Assert.That(list.Contains("Phone"), Is.False);
			Assert.That(list.Contains("LastName"));
			Assert.That(list.Contains("Name"), Is.False);
		}
	}
}
