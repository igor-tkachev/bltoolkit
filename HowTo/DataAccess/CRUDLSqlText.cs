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
		public void Test()
		{
			DataAccessor da = new DataAccessor();

			// Insert.
			//
			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da.InsertSql(person);

			using (DbManager db = new DbManager())
			{
				person = db
					.SetSpCommand("Person_SelectByName",
						db.CreateParameters(person))
					.ExecuteObject<Person>();
			}

			TypeAccessor.WriteConsole(person);
			Assert.IsNotNull(person);

			// Update.
			//
			person.Gender = Gender.Other;

			da.UpdateSql(person);

			person = da.SelectByKeySql<Person>(person.ID);

			TypeAccessor.WriteConsole(person);
			Assert.AreEqual(Gender.Other, person.Gender);

			// Delete.
			//
			da.DeleteSql(person);

			person = da.SelectByKeySql<Person>(person.ID);

			Assert.IsNull(person);

			// Get All.
			//
			List<Person> list = da.SelectAllSql<Person>();

			foreach (Person p in list)
				TypeAccessor.WriteConsole(p);
		}

		[Test]
		public void TransactionTest()
		{
			using (DbManager db = new DbManager())
			{
				DataAccessor<Person> da = new DataAccessor<Person>(db);

				db.BeginTransaction();

				// Insert.
				//
				Person person = new Person();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Unknown;

				da.InsertSql(person);

				person = db
					.SetSpCommand("Person_SelectByName",
						db.CreateParameters(person))
					.ExecuteObject<Person>();

				TypeAccessor.WriteConsole(person);
				Assert.IsNotNull(person);

				// Update.
				//
				person.Gender = Gender.Other;

				da.UpdateSql(person);

				person = da.SelectByKeySql(person.ID);

				TypeAccessor.WriteConsole(person);
				Assert.AreEqual(Gender.Other, person.Gender);

				// Delete.
				//
				da.DeleteSql(person);

				person = da.SelectByKeySql(person.ID);

				Assert.IsNull(person);

				db.CommitTransaction();

				// Get All.
				//
				List<Person> list = da.SelectAllSql();

				foreach (Person p in list)
					TypeAccessor.WriteConsole(p);
			}
		}
	}
}

