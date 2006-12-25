using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SelectByKeySql
	{
		[Test]
		public void Test1()
		{
			/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

			Person person = query./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

				Person person = query./*[a]*/SelectByKey(db, 1)/*[/a]*/;

				Assert.IsNotNull(person);
			}
		}
	}
}

