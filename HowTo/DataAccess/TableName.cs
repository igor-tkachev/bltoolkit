using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class TableName
	{
		/*[a]*/[TableName("Person")]/*[/a]*/
		public class /*[a]*/MyPersonObject/*[/a]*/
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
			SqlQuery<MyPersonObject> query = new SqlQuery<MyPersonObject>();

			MyPersonObject person = query./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			SprocQuery<MyPersonObject> query = new SprocQuery<MyPersonObject>();

			MyPersonObject person = query./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}
	}
}

