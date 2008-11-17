using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class PersonAccessorTest
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

		public abstract class PersonAccessor : DataAccessor<Person>
		{
			[SprocName("Person_SelectByKey")]
			public abstract Person  SelectByID  (int id);

			public abstract Person  SelectByName(Person person);
			public abstract Person  SelectByName(string firstName, string lastName);

			public abstract int     Insert      (Person person);

			[ActionName("SelectByKey")]
			public abstract Person  SelectByID  (DbManager db, int id);

			public abstract Person  SelectByName(DbManager db, Person person);
			public abstract Person  SelectByName(DbManager db, string firstName, string lastName);

			public abstract int     Insert      (DbManager db, Person person);

			public readonly SprocQuery<Person> Query = new SprocQuery<Person>();
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// Insert and get id.
			//
			Person person = Person.CreateInstance();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			int id = pa.Insert(person);

			person = pa.SelectByID(id);

			TypeAccessor.WriteConsole(person);
			Assert.IsNotNull(person);

			// Update.
			//
			person.Gender = Gender.Other;

			pa.Query.Update(person);

			person = pa.SelectByID(person.ID);

			TypeAccessor.WriteConsole(person);
			Assert.AreEqual(Gender.Other, person.Gender);

			// Delete.
			//
			pa.Query.Delete(person);

			person = pa.SelectByID(person.ID);

			Assert.IsNull(person);

			// Get All.
			//
			List<Person> list = pa.Query.SelectAll();

			foreach (Person p in list)
				TypeAccessor.WriteConsole(p);
		}

		[Test]
		public void TransactionTest()
		{
			using (DbManager db = new DbManager())
			{
				PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

				db.BeginTransaction();

				// Insert and get id.
				//
				Person person = Person.CreateInstance();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Unknown;

				int id = pa.Insert(db, person);

				person = pa.SelectByID(db, id);

				TypeAccessor.WriteConsole(person);
				Assert.IsNotNull(person);

				// Update.
				//
				person.Gender = Gender.Other;

				pa.Query.Update(db, person);

				person = pa.SelectByID(db, person.ID);

				TypeAccessor.WriteConsole(person);
				Assert.AreEqual(Gender.Other, person.Gender);

				// Delete.
				//
				pa.Query.Delete(db, person);

				person = pa.SelectByID(db, person.ID);

				Assert.IsNull(person);

				db.CommitTransaction();

				// Get All.
				//
				List<Person> list = pa.Query.SelectAll(db);

				foreach (Person p in list)
					TypeAccessor.WriteConsole(p);
			}
		}
	}
}

