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
			DataAccessor da = new DataAccessor();

			MyPersonObject person = da./*[a]*/SelectByKeySql<MyPersonObject>(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			DataAccessor da = new DataAccessor();

			MyPersonObject person = da./*[a]*/SelectByKey<MyPersonObject>(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}
	}
}

