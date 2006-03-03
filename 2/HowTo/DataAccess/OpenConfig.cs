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
			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			Person person = da./*[b]*/SelectByKeySql<Person>(1)/*[/b]*/;

			Assert.IsNotNull(person);
		}

		// /*[i]*/DataAccessor/*[/i]*/ takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor(db)/*[/b]*/;

				Person person = da./*[b]*/SelectByKeySql<Person>(1)/*[/b]*/;

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
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				Person person = da./*[b]*/SelectByKeySql<Person>(db, 1)/*[/b]*/;

				Assert.IsNotNull(person);
			}
		}

		// Generic /*[i]*/DataAccessor/*[/i]*/.
		//
		[Test]
		public void Test4()
		{
			/*[b]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/b]*/;

			Person person = da./*[b]*/SelectByKeySql(1)/*[/b]*/;

			Assert.IsNotNull(person);
		}
	}
}

