using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Data;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class CRUDLSqlText
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
		public void InsertTest()
		{
			DataAccessor da = new DataAccessor();

			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da.InsertSql(person);
		}

		[Test]
		public void UpdateTest()
		{
			using (DbManager db = new DbManager())
			{
				// Get object.
				//
				Person person = db
					.SetSpCommand("Person_SelectByName", "Crazy", "Frog")
					.ExecuteObject<Person>();

				TypeAccessor.WriteConsole(person);
				Assert.IsNotNull(person);

				DataAccessor da = new DataAccessor();

				// Update.
				//
				person.Gender = Gender.Other;

				da.UpdateSql(db, person);

				person = da.SelectByKeySql<Person>(db, person.ID);

				TypeAccessor.WriteConsole(person);
				Assert.AreEqual(Gender.Other, person.Gender);

				// Delete.
				//
				da.DeleteSql(db, person);

				person = da.SelectByKeySql<Person>(db, person.ID);

				Assert.IsNull(person);

				// Get All.
				//
				List<Person> list = da.SelectAllSql<Person>(db);

				foreach (Person p in list)
					TypeAccessor.WriteConsole(p);
			}
		}
	}
}

