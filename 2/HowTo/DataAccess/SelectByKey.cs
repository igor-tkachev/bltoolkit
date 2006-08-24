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
			/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

			Person person = query./*[a]*/SelectByKey(1)/*[/a]*/;

			Assert.IsNotNull(person);
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

				Person person = query./*[a]*/SelectByKey(db, 1)/*[/a]*/;

				Assert.IsNotNull(person);
			}
		}
	}
}

