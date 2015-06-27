using System;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Exceptions
{
	using Linq;

	[TestFixture]
	public class Mapping : TestBase
	{
		[Test, ExpectedException(typeof (LinqException))]
		public void MapIgnore1()
		{
			ForEachProvider(typeof (LinqException),
				new[] {"Northwind"},
				db =>
				{
					var q = from p in db.Person where p.Name == "123" select p;
					q.ToList();
				});
		}

		[TableName("Person")]
		public class TestPerson1
		{
			            public int    PersonID;
			[MapIgnore] public string FirstName;
		}

		[Test, ExpectedException(typeof (LinqException))]
		public void MapIgnore2()
		{
			ForEachProvider(typeof (LinqException),
				new[] {"Northwind"},
				db => db.GetTable<TestPerson1>().FirstOrDefault(_ => _.FirstName == null));
		}
	}
}
