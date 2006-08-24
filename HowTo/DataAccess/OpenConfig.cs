using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class OpenConfig
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		// /*[i]*/DbManager/*[/i]*/ is created by /*[i]*/DataAccessor/*[/i]*/.
		//
		[Test]
		public void Test1()
		{
			/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>()/*[/a]*/;

			Person person = query./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		// /*[i]*/DataAccessor/*[/i]*/ takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SqlQuery<Person> query = new SqlQuery<Person>(db)/*[/a]*/;

				Person person = query./*[a]*/SelectByKey(1)/*[/a]*/;

				Assert.IsNotNull(person);
			}
		}

		// Every single /*[i]*/DataAccessor/*[/i]*/ method takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test3()
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

