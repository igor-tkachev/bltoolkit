using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class PrimaryKey
	{
		public class Person
		{
			[MapField("PersonID"), /*[a]*/PrimaryKey/*[/a]*/, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[Test]
		public void Test()
		{
			DataAccessor<Person> da = new DataAccessor<Person>();

			Person person = da./*[a]*/SelectByKeySql(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}
	}
}

