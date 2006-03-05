using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Delete
	{
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

			/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

			da./*[a]*/DeleteByKey<Person>(id)/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			int id = Insert();

			/*[a]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/a]*/;

			da./*[a]*/DeleteByKey(id)/*[/a]*/;
		}

		[Test]
		public void Test3()
		{
			int id = Insert();

			using (DbManager db = new DbManager())
			{
				/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

				Person person = da.SelectByKeySql<Person>(db, id);

				da./*[a]*/Delete(db, person)/*[/a]*/;
			}
		}
	}
}

