using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

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
		public void MakeSubQueryWithParam()
		{
			var n = 1;

			TestOneJohn(db => 
				from p in db.Person
				select new { ID = p.PersonID + n, p.FirstName } into p
				where p.ID == 2
				select new Person(p.ID - 1) { FirstName = p.FirstName });
		}

		[Test]
		public void DoNotMakeSubQuery()
		{
			TestOneJohn(db => 
				from p1 in db.Person
				select new { p1.PersonID, Name = p1.FirstName + "\r\r\r" } into p2
				where p2.PersonID == 1
				select new Person(p2.PersonID) { FirstName = p2.Name.TrimEnd('\r') });
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

		int TestMethod()
		{
			return 1;
		}

		[Test]
		public void MethodParam()
		{
			TestOneJohn(db => from p in db.Person where p.PersonID == TestMethod() select p);
		}

		static int StaticTestMethod()
		{
			return 1;
		}

		[Test]
		public void StaticMethodParam()
		{
			TestOneJohn(db => from p in db.Person where p.PersonID == StaticTestMethod() select p);
		}

		class TestMethodClas
		{
			private readonly int _n;

			public TestMethodClas(int n)
			{
				_n = n;
			}

			public int TestMethod()
			{
				return _n;
			}
		}

		public void MethodParam(int n)
		{
			var t = new TestMethodClas(n);

			ForEachProvider(db =>
			{
				var id = (from p in db.Person where p.PersonID == t.TestMethod() select new { p.PersonID }).ToList().First();
				Assert.AreEqual(n, id.PersonID);
			});
		}

		[Test]
		public void MethodParam2()
		{
			MethodParam(1);
			MethodParam(2);
		}
	}
}
