using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class SetCommand
	{
		// Select a person list.
		//
		public IList<Person> GetPersonList()
		{
			using (DbManager db = new DbManager())
			{
				return db
					./*[a]*/SetCommand/*[/a]*/("SELECT * FROM Person")
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
		public Person GetPersonByID(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					./*[a]*/SetCommand/*[/a]*/("SELECT * FROM Person WHERE PersonID = @id",
						db./*[a]*/Parameter/*[/a]*/("@id", id))
					.ExecuteObject<Person>();
			}
		}

		[Test]
		public void Test2()
		{
			Person person = GetPersonByID(1);

			Assert.IsNotNull(person);
		}

		// Insert, Update, and Delete a person.
		//
		public Person GetPersonByID(DbManager db, int id)
		{
			return db
				./*[a]*/SetCommand/*[/a]*/("SELECT * FROM Person WHERE PersonID = @id",
					db./*[a]*/Parameter/*[/a]*/("@id", id))
				.ExecuteObject<Person>();
		}

		public Person CreatePerson(DbManager db)
		{
			int id = db
				./*[a]*/SetCommand/*[/a]*/(@"
					INSERT INTO Person ( LastName,  FirstName,  Gender)
					VALUES             (@LastName, @FirstName, @Gender)

					SELECT Cast(SCOPE_IDENTITY() as int) PersonID",
					db./*[a]*/Parameter/*[/a]*/("@LastName",  "Frog"),
					db./*[a]*/Parameter/*[/a]*/("@FirstName", "Crazy"),
					db./*[a]*/Parameter/*[/a]*/("@Gender",    Map.EnumToValue(Gender.Male)))
				.ExecuteScalar<int>();

			return GetPersonByID(db, id);
		}

		public Person UpdatePerson(DbManager db, Person person)
		{
			db
				./*[a]*/SetCommand/*[/a]*/(@"
					UPDATE
						Person
					SET
						LastName   = @LastName,
						FirstName  = @FirstName,
						Gender     = @Gender
					WHERE
						PersonID = @PersonID",
					db./*[a]*/CreateParameters/*[/a]*/(person))
				.ExecuteNonQuery();

			return GetPersonByID(db, person.ID);
		}

		public Person DeletePerson(DbManager db, Person person)
		{
			db
				./*[a]*/SetCommand/*[/a]*/("DELETE FROM Person WHERE PersonID = @id",
					db./*[a]*/Parameter/*[/a]*/("@id", person.ID))
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
