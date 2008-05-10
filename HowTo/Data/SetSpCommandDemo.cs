using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class SetSpCommandDemo
	{
		// Select a person list.
		//
		public IList<Person> GetPersonList()
		{
			using (DbManager db = new DbManager())
			{
				return db
					./*[a]*/SetSpCommand/*[/a]*/("Person_SelectAll")
					.ExecuteList<Person>();
			}
		}

		[Test]
		public void Test1()
		{
			IList<Person> list = GetPersonList();

			Assert.AreNotEqual(0, list.Count);
		}

		// Select a person.
		//
		public Person GetPersonByID1(int id)
		{
			using (DbManager db = new DbManager())
			{
				// Pass a parameter using the [b]Parameter[/b] method.
				//
				return db
					./*[a]*/SetSpCommand/*[/a]*/("Person_SelectByKey",
						db./*[a]*/Parameter/*[/a]*/("@id", id))
					.ExecuteObject<Person>();
			}
		}

		public Person GetPersonByID2(int id)
		{
			using (DbManager db = new DbManager())
			{
				// Pass a parameter using the [b]params[/b] parameter of the SetSpCommand method.
				//
				return db
					./*[a]*/SetSpCommand/*[/a]*/("Person_SelectByKey", /*[a]*/id/*[/a]*/)
					.ExecuteObject<Person>();
			}
		}

		[Test]
		public void Test2()
		{
			Person person = GetPersonByID1(1);
			Assert.IsNotNull(person);

			person = GetPersonByID2(1);
			Assert.IsNotNull(person);
		}

		// Insert, Update, and Delete a person.
		//
		public Person GetPersonByID(DbManager db, int id)
		{
			return db
				./*[a]*/SetSpCommand/*[/a]*/("Person_SelectByKey", /*[a]*/id/*[/a]*/)
				.ExecuteObject<Person>();
		}

		public Person CreatePerson(DbManager db)
		{
			int id = db
				./*[a]*/SetSpCommand/*[/a]*/("Person_Insert",
					db./*[a]*/Parameter/*[/a]*/("@LastName",   "Frog"),
					db./*[a]*/Parameter/*[/a]*/("@MiddleName", null),
					db./*[a]*/Parameter/*[/a]*/("@FirstName",  "Crazy"),
					db./*[a]*/Parameter/*[/a]*/("@Gender",     Map.EnumToValue(Gender.Male)))
				.ExecuteScalar<int>();

			return GetPersonByID(db, id);
		}

		public Person UpdatePerson(DbManager db, Person person)
		{
			db
				./*[a]*/SetSpCommand/*[/a]*/("Person_Update", db./*[a]*/CreateParameters/*[/a]*/(person))
				.ExecuteNonQuery();

			return GetPersonByID(db, person.ID);
		}

		public Person DeletePerson(DbManager db, Person person)
		{
			db
				./*[a]*/SetSpCommand/*[/a]*/("Person_Delete", /*[a]*/person.ID/*[/a]*/)
				.ExecuteNonQuery();

			return GetPersonByID(db, person.ID);
		}

		[Test]
		public void Test3()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				// Insert.
				//
				Person person = CreatePerson(db);

				Assert.IsNotNull(person);

				// Update.
				//
				Assert.AreEqual(Gender.Male, person.Gender);

				person.Gender = Gender.Female;

				person = UpdatePerson(db, person);

				Assert.AreEqual(Gender.Female, person.Gender);

				// Delete.
				//
				person = DeletePerson(db, person);

				Assert.IsNull(person);

				db.CommitTransaction();
			}
		}
	}
}
