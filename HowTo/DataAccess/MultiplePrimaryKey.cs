using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class MultiplePrimaryKey
	{
		[TableName("Person")]
		public class Person
		{
			[MapField("PersonID"), NonUpdatable]
			public int    ID;

			// These fields are not real primary key of the table.
			// They are made primary key for demonstration purpose only.
			//
			[/*[a]*/PrimaryKey(1)/*[/a]*/] public string FirstName;
			[/*[a]*/PrimaryKey(2)/*[/a]*/] public string LastName;

			public string MiddleName;
		}

		[Test]
		public void Test()
		{
			SqlQuery<Person> query = new SqlQuery<Person>();

			Person person = query./*[a]*/SelectByKey("John", "Pupkin")/*[/a]*/;

			Assert.IsNotNull(person);
		}
	}
}

