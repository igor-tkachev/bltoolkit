using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SelectByKeySql
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[Test]
		public void Test1()
		{
			/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

			Person person = da./*[b]*/SelectByKeySql<Person>(1)/*[/b]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				Person person = da./*[b]*/SelectByKeySql<Person>(db, 1)/*[/b]*/;

				Assert.IsNotNull(person);
			}
		}

		[Test]
		public void Test3()
		{
			/*[b]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/b]*/;

			Person person = da./*[b]*/SelectByKeySql(1)/*[/b]*/;

			Assert.IsNotNull(person);
		}
	}
}

