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
		public void InsertTest()
		{
			// DataAccessor creates /*[i]*/DbManager/*[/i]*/ itself.
			//
			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			Person person = Person.CreateInstance();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da./*[b]*/Insert/*[/b]*/(person);
		}

		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				// Get object.
				//
				Person person = db
					.SetSpCommand("Person_SelectByName", "Crazy", "Frog")
					.ExecuteObject<Person>();

				TypeAccessor.WriteConsole(person);
				Assert.IsNotNull(person);

				// DataAccessor takes /*[i]*/DbManager/*[/i]*/ as a parameter.
				// /*[i]*/DbManager/*[/i]*/ controls the transaction.
				//
				/*[b]*/DataAccessor da = new DataAccessor(db)/*[/b]*/;

				// Update.
				//
				person.Gender = Gender.Other;

				da./*[b]*/Update/*[/b]*/(person);

				person = da./*[b]*/SelectByKey<Person>(person.ID)/*[/b]*/;

				TypeAccessor.WriteConsole(person);
				Assert.AreEqual(Gender.Other, person.Gender);

				// Delete.
				//
				da./*[b]*/Delete/*[/b]*/(person);

				person = da./*[b]*/SelectByKey<Person>(person.ID)/*[/b]*/;

				Assert.IsNull(person);

				db.CommitTransaction();
			}
		}

		[Test]
		public void SelectAll()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor(db)/*[/b]*/;

				// Select All.
				//
				List<Person> list = da./*[b]*/SelectAll<Person>()/*[/b]*/;

				foreach (Person p in list)
					TypeAccessor.WriteConsole(p);
			}
		}
	}
}

