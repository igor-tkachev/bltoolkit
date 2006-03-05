using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SelectByKey
	{
		[Test]
		public void Test1()
		{
			/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

			Person person = da./*[a]*/SelectByKey<Person>(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

				Person person = da./*[a]*/SelectByKey<Person>(db, 1)/*[/a]*/;

				Assert.IsNotNull(person);
			}
		}

		[Test]
		public void Test3()
		{
			/*[a]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/a]*/;

			Person person = da./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}
	}
}

