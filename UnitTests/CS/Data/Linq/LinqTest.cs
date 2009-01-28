using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class LinqTest
	{
		class TestManager : DbManager
		{
			public Table<Person> Person
			{
				get { return GetTable<Person>(); }
			}
		}

		[Test]
		public void Test()
		{
			using (TestManager db = new TestManager())
			{
				var query = db.Person.Select(p => p);

				var list = query.ToList();
			}
		}
	}
}
