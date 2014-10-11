using System;
using NUnit.Framework;
using BLToolkit.Data;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ExecuteNonQuery
	{
		[Test]
		public void Test()
		{
			Person person = new Person();

			person.FirstName = "John";
			person.LastName  = "Smith";
			person.Gender    = Gender.Male;

			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				// Execute.
				//
				db
					.SetSpCommand("Person_Insert", db.CreateParameters(person))
					./*[a]*/ExecuteNonQuery/*[/a]*/();

				// Check the result.
				//
				person = db
					.SetCommand(
						"SELECT * FROM Person WHERE LastName = @lastName",
						db.Parameter("@lastName", "Smith"))
					.ExecuteObject<Person>();

				Assert.IsNotNull(person);

				// Cleanup.
				//
				db
					.SetCommand(
						"DELETE FROM Person WHERE LastName = @lastName",
						db.Parameter("@lastName", "Smith"))
					./*[a]*/ExecuteNonQuery/*[/a]*/();

				db.CommitTransaction();
			}
		}
	}
}
