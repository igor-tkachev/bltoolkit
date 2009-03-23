using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class SelectTest : TestBase
	{
		[Test]
		public void PersonToList()
		{
			Less0ForEachProvider(db => db.Person.ToList().Count);
		}

		[Test]
		public void PersonSelect()
		{
			Less0ForEachProvider(db => (from p in db.Person select p).ToList().Count);
		}

		[Test]
		public void PersonDoubleSelect()
		{
			Less0ForEachProvider(db => db.Person.Select(p => p).Select(p => p).ToList().Count);
		}

		[Test]
		public void New()
		{
			Less0ForEachProvider(db => (from p in db.Person select new { p.PersonID, p.FirstName }).ToList().Count);
		}

		[Test]
		public void NewPerson1()
		{
			Less0ForEachProvider(db => (from p in db.Person select new Person { PersonID = p.PersonID }).ToList().Count);
		}

		[Test]
		public void NewPerson2()
		{
			Less0ForEachProvider(db => (from p in db.Person select new Person(p.PersonID)).ToList().Count);
		}

		[Test]
		public void NewPerson3()
		{
			Less0ForEachProvider(db => (from p in db.Person select new Person(p.PersonID) { FirstName = p.FirstName }).ToList().Count);
		}
	}
}
