using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SelectAllSql
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

			List<Person> list = da./*[b]*/SelectAllSql<Person>()/*[/b]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[b]*/DataAccessor da = new DataAccessor()/*[/b]*/;

				List<Person> list = da./*[b]*/SelectAllSql<Person>(db)/*[/b]*/;
			}
		}

		[Test]
		public void Test3()
		{
			/*[b]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/b]*/;

			List<Person> list = da./*[b]*/SelectAllSql()/*[/b]*/;
		}
	}
}

