using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class NonUpdatable
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
			[MapField("PersonID"), PrimaryKey, /*[a]*/NonUpdatable/*[/a]*/]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
			public Gender Gender;
		}

		[Test]
		public void Test()
		{
			SqlQuery<Person> query = new SqlQuery<Person>();

			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Other;

			query./*[a]*/Insert(person)/*[/a]*/;
		}
	}
}
