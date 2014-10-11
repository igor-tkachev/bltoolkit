using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SqlQuery
	{
		public abstract class TestAccessor : DataAccessor
		{
			[/*[a]*/SqlQuery/*[/a]*/(@"
				SELECT
					*
				FROM
					Person
				WHERE
					FirstName like @firstName AND
					LastName  like @lastName")]
			public abstract List<Person> GetPersonListByName(string @firstName, string @lastName);
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();

			List<Person> list = ta.GetPersonListByName("John", "P%");

			Assert.AreNotEqual(0, list.Count);
		}
	}
}
