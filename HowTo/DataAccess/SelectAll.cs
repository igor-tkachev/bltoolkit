using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SelectAll
	{
		[Test]
		public void Test1()
		{
			/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

			List<Person> list = query./*[a]*/SelectAll()/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

				List<Person> list = query./*[a]*/SelectAll(db)/*[/a]*/;
			}
		}
	}
}

