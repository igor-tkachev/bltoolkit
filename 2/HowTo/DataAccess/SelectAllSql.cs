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
		[Test]
		public void Test1()
		{
			/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

			List<Person> list = da./*[a]*/SelectAllSql<Person>()/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

				List<Person> list = da./*[a]*/SelectAllSql<Person>(db)/*[/a]*/;
			}
		}

		[Test]
		public void Test3()
		{
			/*[a]*/DataAccessor<Person> da = new DataAccessor<Person>()/*[/a]*/;

			List<Person> list = da./*[a]*/SelectAllSql()/*[/a]*/;
		}
	}
}

