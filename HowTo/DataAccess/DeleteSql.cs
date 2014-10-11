using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DeleteSql
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

			/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

			query./*[a]*/DeleteByKey(id)/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			int id = Insert();

			using (DbManager db = new DbManager())
			{
				/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

				Person person = query.SelectByKey(db, id);

				query./*[a]*/Delete(db, person)/*[/a]*/;
			}
		}
	}
}

