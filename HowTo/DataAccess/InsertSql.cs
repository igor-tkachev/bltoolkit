using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class InsertSql
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
			public Gender Gender;
		}

		[Test]
		public void Test1()
		{
			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da./*[b]*/InsertSql(person)/*[/b]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				Person person = new Person();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Other;

				da./*[b]*/InsertSql(db, person)/*[/b]*/;
			}
		}
	}
}

