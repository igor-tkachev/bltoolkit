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
	public class CRUDLSproc
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
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public abstract int    ID         { get; }

			public abstract string LastName   { get; set; }
			public abstract string FirstName  { get; set; }
			public abstract string MiddleName { get; set; }
			public abstract Gender Gender     { get; set; }

			public static Person CreateInstance()
			{
				return TypeAccessor<Person>.CreateInstanceEx();
			}
		}

		[Test]
		public void Test()
		{
			DataAccessor da = new DataAccessor();

			// Insert.
			//
			Person person = Person.CreateInstance();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da.Insert(person);

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

			da.Update(person);

			person = da.SelectByKey<Person>(person.ID);

			TypeAccessor.WriteConsole(person);
			Assert.AreEqual(Gender.Other, person.Gender);

			// Delete.
			//
			da.Delete(person);

			person = da.SelectByKey<Person>(person.ID);

			Assert.IsNull(person);

			// Get All.
			//
			List<Person> list = da.SelectAll<Person>();

			foreach (Person p in list)
				TypeAccessor.WriteConsole(p);
		}

		[Test]
		public void TransactionTest()
		{
			using (DbManager db = new DbManager())
			{
				DataAccessor<Person> da = new DataAccessor<Person>();

				db.BeginTransaction();

				// Insert.
				//
				Person person = Person.CreateInstance();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Unknown;

				da.Insert(db, person);

				person = db
					.SetSpCommand("Person_SelectByName",
						db.CreateParameters(person))
					.ExecuteObject<Person>();

				TypeAccessor.WriteConsole(person);
				Assert.IsNotNull(person);

				// Update.
				//
				person.Gender = Gender.Other;

				da.Update(db, person);

				person = da.SelectByKey(db, person.ID);

				TypeAccessor.WriteConsole(person);
				Assert.AreEqual(Gender.Other, person.Gender);

				// Delete.
				//
				da.Delete(db, person);

				person = da.SelectByKey(db, person.ID);

				Assert.IsNull(person);

				db.CommitTransaction();

				// Get All.
				//
				List<Person> list = da.SelectAll(db);

				foreach (Person p in list)
					TypeAccessor.WriteConsole(p);
			}
		}
	}
}

