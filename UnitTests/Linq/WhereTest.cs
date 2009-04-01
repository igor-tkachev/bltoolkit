using System;
using System.Linq;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class WhereTest : TestBase
	{
		[Test]
		public void MakeSubQuery()
		{
			TestOneJohn(db => 
				from p in db.Person
				select new { ID = p.PersonID + 1, p.FirstName } into p
				where p.ID == 2
				select new Person(p.ID - 1) { FirstName = p.FirstName });
		}

		[Test]
		public void DoNotMakeSubQuery()
		{
			TestOneJohn(db => 
				from p in db.Person
				select new { p.PersonID, Name = p.FirstName + "\r\r\r" } into p
				where p.PersonID == 1
				select new Person(p.PersonID) { FirstName = p.Name.TrimEnd('\r') });
		}

		[Test]
		public void EqualsConst()
		{
			TestOneJohn(db => from p in db.Person where p.PersonID == 1 select p);
		}

		[Test]
		public void EqualsParam()
		{
			var id = 1;
			TestOneJohn(db => from p in db.Person where p.PersonID == id select p);
		}
	}
}
