using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class OpenConfigQuery
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		// /*[i]*/DbManager/*[/i]*/ is created by /*[i]*/SqlQuery/*[/i]*/.
		//
		[Test]
		public void Test1()
		{
			SqlQuery<Person> query = new SqlQuery<Person>/*[a]*/()/*[/a]*/;

			Person person = query.SelectByKey(1);

			Assert.IsNotNull(person);
		}

		// /*[i]*/SqlQuery/*[/i]*/ takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery<Person> query = new SqlQuery<Person>/*[a]*/(db)/*[/a]*/;

				Person person = query.SelectByKey(1);

				Assert.IsNotNull(person);
			}
		}

		// /*[i]*/SqlQuery/*[/i]*/ method takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test3()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery<Person> query = new SqlQuery<Person>/*[a]*/()/*[/a]*/;

				Person person = query.SelectByKey(/*[a]*/db/*[/a]*/, 1);

				Assert.IsNotNull(person);
			}
		}
	}
}

