using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Mapping
{
	[TestFixture]
	public class XmlMap
	{
		public class Person
		{
			[PrimaryKey]
			[MapField("PersonID")]  public int    ID;
			[MapField("FirstName")] public string FName;
			[MapField("LastName")]  public string LName;
		}

		[Test]
		public void Test()
		{
			Person p = (Person)new SqlQuery().SelectByKey(typeof(Person), 1);

			Assert.AreEqual(1,        p.ID);
			Assert.AreEqual("John",   p.FName);
			Assert.AreEqual("Pupkin", p.LName);

			MappingSchema map = new MappingSchema();

			map.Extensions = TypeExtension.GetExtensions("XmlMap.xml");

			DataRow dr = map.MapObjectToDataRow(p, new DataTable());

			Assert.AreEqual(1,        dr["PERSON_ID"]);
			Assert.AreEqual("John",   dr["FIRST_NAME"]);
			Assert.AreEqual("Pupkin", dr["LAST_NAME"]);
		}
	}
}
