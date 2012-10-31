using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteObject
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public abstract class Person
		{
			[MapField("PersonID")]
			public abstract int    ID         { get; }

			public abstract string LastName   { get; set; }
			public abstract string FirstName  { get; set; }
			public abstract string MiddleName { get; set; }
			public abstract Gender Gender     { get; set; }
		}

		Person GetPersonSqlText(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					./*[a]*/ExecuteObject<Person>()/*[/a]*/;
			}
		}

		[Test]
		public void SqlText()
		{
			Person person = GetPersonSqlText(1);

			TypeAccessor.WriteConsole(person);
		}

		Person GetPersonSproc1(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetSpCommand("Person_SelectByKey",
						db.Parameter("@id", id))
					./*[a]*/ExecuteObject<Person>()/*[/a]*/;
			}
		}

		[Test]
		public void Sproc1()
		{
			Person person = GetPersonSproc1(1);

			TypeAccessor.WriteConsole(person);
		}

		Person GetPersonSproc2(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetSpCommand("Person_SelectByKey", id)
					./*[a]*/ExecuteObject<Person>()/*[/a]*/;
			}
		}

		[Test]
		public void Sproc2()
		{
			Person person = GetPersonSproc2(1);

			TypeAccessor.WriteConsole(person);
		}
	}
}
