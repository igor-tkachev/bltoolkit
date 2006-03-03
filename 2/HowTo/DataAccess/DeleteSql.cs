using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DeleteSql
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		private int Insert()
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand(@"
						INSERT INTO Person (
							 FirstName,  LastName,  Gender
						) VALUES (
							@FirstName, @LastName, @Gender
						)
						SELECT Cast(SCOPE_IDENTITY() as int)",
						db.Parameter("@FirstName", "Crazy"),
						db.Parameter("@LastName",  "Frog"),
						db.Parameter("@Gender",    "U"))
					.ExecuteScalar<int>();
			}
		}

		[Test]
		public void Test1()
		{
			int id = Insert();

			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			da./*[b]*/DeleteByKey<Person>(id)/*[/b]*/;
		}

		[Test]
		public void Test2()
		{
			int id = Insert();

			/*[b]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/b]*/;

			da./*[b]*/DeleteByKey(id)/*[/b]*/;
		}

		[Test]
		public void Test3()
		{
			int id = Insert();

			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				Person person = da.SelectByKeySql<Person>(db, id);

				da./*[b]*/DeleteSql(db, person)/*[/b]*/;
			}
		}
	}
}

