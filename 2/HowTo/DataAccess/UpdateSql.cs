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
			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			Person person = da.SelectByKeySql<Person>(_id);

			person.Gender = Gender.Other;

			da./*[b]*/UpdateSql(person)/*[/b]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				Person person = da.SelectByKeySql<Person>(db, _id);

				person.Gender = Gender.Other;

				da./*[b]*/UpdateSql(db, person)/*[/b]*/;
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

