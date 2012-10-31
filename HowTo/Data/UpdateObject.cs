using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.Data
{
	[TestFixture]
	public class UpdateObject
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

		int InsertPerson(Person person)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand(@"
						INSERT INTO Person
							( LastName,  FirstName,  MiddleName,  Gender)
						VALUES
							(@LastName, @FirstName, @MiddleName, @Gender)

						SELECT Cast(SCOPE_IDENTITY() as int)",
						db.CreateParameters(person))
					.ExecuteScalar<int>();
			}
		}

		Person GetPersonByID(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT * FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					.ExecuteObject<Person>();
			}
		}

		void UpdatePerson(Person person)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(@"
						UPDATE
							Person
						SET
							LastName   = @LastName,
							FirstName  = @FirstName,
							MiddleName = @MiddleName,
							Gender     = @Gender
						WHERE
							PersonID = @PersonID",
						db.CreateParameters(person))
					.ExecuteNonQuery();
			}
		}

		void DeletePerson(int id)
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand("DELETE FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					.ExecuteNonQuery();
			}
		}

		[Test]
		public void Test()
		{
			// Insert.
			//
			Person person = TypeAccessor<Person>.CreateInstanceEx();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			int id = InsertPerson(person);

			person = GetPersonByID(id);

			TypeAccessor.WriteConsole(person);
			Assert.IsNotNull(person);

			// Update.
			//
			person.Gender = Gender.Other;

			UpdatePerson(person);

			person = GetPersonByID(id);

			TypeAccessor.WriteConsole(person);
			Assert.AreEqual(Gender.Other, person.Gender);

			// Delete.
			//
			DeletePerson(id);

			person = GetPersonByID(id);

			Assert.IsNull(person);
		}
	}
}

