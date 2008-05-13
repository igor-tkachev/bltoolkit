using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ExecuteForEach
	{
		[Test]
		public void Test()
		{
			List<Person> list = new List<Person>
			{
				new Person { FirstName = "John", LastName = "Smith", Gender = Gender.Male   },
				new Person { FirstName = "Jane", LastName = "Smith", Gender = Gender.Female }
			};

			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				// Execute.
				//
				db
					.SetSpCommand("Person_Insert")
					./*[a]*/ExecuteForEach/*[/a]*/<Person>(list);

				// Check the result.
				//
				list = db
					.SetCommand(
						"SELECT * FROM Person WHERE LastName = @lastName",
						db.Parameter("@lastName", "Smith"))
					.ExecuteList<Person>();

				Assert.GreaterOrEqual(2, list.Count);

				// Cleanup.
				//
				db
					.SetCommand(
						"DELETE FROM Person WHERE LastName = @lastName",
						db.Parameter("@lastName", "Smith"))
					.ExecuteNonQuery();

				db.CommitTransaction();
			}
		}
	}
}
