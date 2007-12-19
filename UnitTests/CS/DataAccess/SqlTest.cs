using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.DataAccess;

namespace DataAccess
{
	[TestFixture]
	public class SqlTest
	{
		public class Name
		{
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[MapField("LastName",   "Name.LastName")]
		[MapField("FirstName",  "Name.FirstName")]
		[MapField("MiddleName", "Name.MiddleName")]
		public class Person
		{
			[MapField("PersonID"), PrimaryKey]
			public int    ID;
			public Name   Name = new Name();
		}

		[Test]
		public void Test()
		{
			SqlQuery da = new SqlQuery();

			Person p = (Person)da.SelectByKey(typeof(Person), 1);

			Assert.AreEqual("Pupkin", p.Name.LastName);
		}
	}
}
