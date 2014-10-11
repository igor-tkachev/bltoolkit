using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class UpdateSql
	{
		int _id;

		[TestFixtureSetUp]
		public void Insert()
		{
			using (DbManager db = new DbManager())
			{
				_id = db
					.SetCommand(@"
						INSERT INTO Person (
							 FirstName,  LastName,  Gender
						) VALUES (
							@FirstName, @LastName, @Gender
						)
						SELECT Cast(SCOPE_IDENTITY() as int)",
						db.Parameter("@FirstName", "Crazy"),
						db.Parameter("@LastName",  "Frog"),
						db.Parameter("@Gender",    Map.EnumToValue(Gender.Unknown)))
					.ExecuteScalar<int>();
			}
		}

		[Test]
		public void Test1()
		{
			/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

			Person person = query.SelectByKey(_id);

			person.Gender = Gender.Other;

			query./*[a]*/Update(person)/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

				Person person = query.SelectByKey(db, _id);

				person.Gender = Gender.Other;

				query./*[a]*/Update(db, person)/*[/a]*/;
			}
		}

		[TestFixtureTearDown]
		public void Delete()
		{
			using (DbManager db = new DbManager())
			{
				db
					.SetCommand("DELETE FROM Person WHERE PersonID = @id",
						db.Parameter("@id", _id))
					.ExecuteNonQuery();
			}
		}
	}
}

