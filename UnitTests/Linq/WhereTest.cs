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
				select new { PersonID = p.ID + 1, p.FirstName } into p
				where p.PersonID == 2
				select new Person(p.PersonID - 1) { FirstName = p.FirstName });
		}

		[Test]
		public void MakeSubQueryWithParam()
		{
			var n = 1;

			TestOneJohn(new[] { "Fdp" }, db => 
				from p in db.Person
				select new { PersonID = p.ID + n, p.FirstName } into p
				where p.PersonID == 2
				select new Person(p.PersonID - 1) { FirstName = p.FirstName });
		}

		[Test]
		public void DoNotMakeSubQuery()
		{
			TestOneJohn(db => 
				from p1 in db.Person
				select new { p1.ID, Name = p1.FirstName + "\r\r\r" } into p2
				where p2.ID == 1
				select new Person(p2.ID) { FirstName = p2.Name.TrimEnd('\r') });
		}

		[Test]
		public void EqualsConst()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 select p);
		}

		[Test]
		public void EqualsConsts()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && p.FirstName == "John" select p);
		}

		[Test]
		public void EqualsConsts2()
		{
			TestOneJohn(db =>
				from p in db.Person
				where (p.FirstName == "John" || p.FirstName == "John's") && p.ID > 0 && p.ID < 2 && p.LastName != "123"
				select p);
		}

		[Test]
		public void EqualsParam()
		{
			var id = 1;
			TestOneJohn(db => from p in db.Person where p.ID == id select p);
		}

		[Test]
		public void EqualsParams()
		{
			var id   = 1;
			var name = "John";
			TestOneJohn(db => from p in db.Person where p.ID == id && p.FirstName == name select p);
		}

		int TestMethod()
		{
			return 1;
		}

		[Test]
		public void MethodParam()
		{
			TestOneJohn(db => from p in db.Person where p.ID == TestMethod() select p);
		}

		static int StaticTestMethod()
		{
			return 1;
		}

		[Test]
		public void StaticMethodParam()
		{
			TestOneJohn(db => from p in db.Person where p.ID == StaticTestMethod() select p);
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
				var id = (from p in db.Person where p.ID == t.TestMethod() select new { p.ID }).ToList().First();
				Assert.AreEqual(n, id.ID);
			});
		}

		[Test]
		public void MethodParam2()
		{
			MethodParam(1);
			MethodParam(2);
		}

		static IQueryable<Person> TestDirectParam(TestDbManager db, int id)
		{
			var name = "John";
			return from p in db.Person where p.ID == id && p.FirstName == name select p;
		}

		[Test]
		public void DirectParams()
		{
			TestOneJohn(db => TestDirectParam(db, 1));
		}

		[Test]
		public void BinaryAdd()
		{
			TestOneJohn(db => from p in db.Person where p.ID + 1 == 2 select p);
		}

		[Test]
		public void BinaryDivide()
		{
			TestOneJohn(db => from p in db.Person where (p.ID + 9) / 10 == 1 && p.ID == 1 select p);
		}

		[Test]
		public void BinaryModulo()
		{
			TestOneJohn(db => from p in db.Person where p.ID % 2 == 1 && p.ID == 1 select p);
		}

		[Test]
		public void BinaryMultiply()
		{
			TestOneJohn(db => from p in db.Person where p.ID * 10 - 9 == 1 select p);
		}

		[Test]
		public void BinaryXor()
		{
			TestOneJohn(new[] { "Access" }, db => from p in db.Person where (p.ID ^ 2) == 3 select p);
		}

		[Test]
		public void BinaryAnd()
		{
			TestOneJohn(new[] { "Access" }, db => from p in db.Person where (p.ID & 3) == 1 select p);
		}

		[Test]
		public void BinaryOr()
		{
			TestOneJohn(new[] { "Access" }, db => from p in db.Person where (p.ID | 2) == 3 select p);
		}

		[Test]
		public void BinarySubtract()
		{
			TestOneJohn(db => from p in db.Person where p.ID - 1 == 0 select p);
		}

		[Test]
		public void EqualsNull()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && p.MiddleName == null select p);
		}

		[Test]
		public void EqualsNull2()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && null == p.MiddleName select p);
		}

		[Test]
		public void NotEqualNull()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && p.FirstName != null select p);
		}

		[Test]
		public void NotEqualNull2()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && null != p.FirstName select p);
		}

		[Test]
		public void NotTest()
		{
			TestOneJohn(db => from p in db.Person where p.ID == 1 && !(p.MiddleName != null) select p);
		}

		[Test]
		public void NotTest2()
		{
			int n = 2;
			TestOneJohn(db => from p in db.Person where p.ID == 1 && !(p.MiddleName != null && p.ID == n) select p);
		}

		[Test]
		public void Coalesce()
		{
			TestOneJohn(db =>

				from p in db.Person
				where
					p.ID == 1 &&
					(p.MiddleName ?? "None") == "None" &&
					(p.FirstName  ?? "None") == "John"
				select p

			);
		}

		[Test]
		public void Conditional()
		{
			TestOneJohn(db =>

				from p in db.Person
				where
					p.ID == 1 &&
					(p.MiddleName == null ? 1 : 2) == 1 &&
					(p.FirstName  != null ? 1 : 2) == 1
				select p

			);
		}

		[Test]
		public void Conditional2()
		{
			TestOneJohn(db =>

				from p in db.Person
				where
					p.ID == 1 &&
					(p.MiddleName != null ? 3 : p.MiddleName == null? 1 : 2) == 1 &&
					(p.FirstName  == null ? 3 : p.FirstName  != null? 1 : 2) == 1
				select p

			);
		}

		[Test]
		public void Conditional3()
		{
			TestOneJohn(db =>

				from p in db.Person
				where
					p.ID == 1 &&
					(p.MiddleName != null ? 3 : p.ID == 2 ? 2 : p.MiddleName != null ? 0 : 1) == 1 &&
					(p.FirstName  == null ? 3 : p.ID == 2 ? 2 : p.FirstName  == null ? 0 : 1) == 1
				select p

			);
		}
	}
}
