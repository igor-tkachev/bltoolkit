using System;
using BLToolkit.Data;
using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.DataAccess;
using NUnit.Framework.SyntaxHelpers;

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

		public class Person
		{
			[MapField("PersonID"), PrimaryKey]
			public int  ID;
			[MapField(Format="{0}")]
			public Name Name = new Name();
		}

		[Test]
		public void Test()
		{
			SqlQuery da = new SqlQuery();

			Person p = (Person)da.SelectByKey(typeof(Person), 1);

			Assert.AreEqual("Pupkin", p.Name.LastName);
		}

		[Test]
		public void GetFieldListTest()
		{
			SqlQuery     da = new SqlQuery();
			SqlQueryInfo info;

			using (DbManager db = new DbManager())
			{
				info = da.GetSqlQueryInfo(db, typeof (Person),        "SelectAll");
			}

			Console.WriteLine(info.QueryText);
			Assert.That(info.QueryText.Contains("[PersonID]"));
			Assert.That(info.QueryText.Contains("[LastName]"));
			Assert.That(info.QueryText, Is.Not.Contains("[Name]"));
		}
	}
}
