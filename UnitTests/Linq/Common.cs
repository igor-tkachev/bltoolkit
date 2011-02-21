using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Common : TestBase
	{
		[Test]
		public void AsQueryable()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in    Child               select p,
				from p in db.Parent from ch in db.Child.AsQueryable() select p));
		}

		[Test]
		public void Convert()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in                         Child                select p,
				from p in db.Parent from ch in ((IEnumerable<Child>)db.Child).AsQueryable() select p));
		}

		[Test]
		public void NewCondition()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Value = p.Value1 != null ? p.Value1 : 100 },
				from p in db.Parent select new { Value = p.Value1 != null ? p.Value1 : 100 }));
		}

		[Test]
		public void NewCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Value = p.Value1 ?? 100 },
				from p in db.Parent select new { Value = p.Value1 ?? 100 }));
		}

		[Test]
		public void CoalesceNew()
		{
			Child ch = null;

			ForEachProvider(db => AreEqual(
				from p in    Parent select ch ?? new Child { ParentID = p.ParentID },
				from p in db.Parent select ch ?? new Child { ParentID = p.ParentID }));
		}

		[Test]
		public void ScalarCondition()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 != null ? p.Value1 : 100,
				from p in db.Parent select p.Value1 != null ? p.Value1 : 100));
		}

		[Test]
		public void ScalarCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? 100,
				from p in db.Parent select p.Value1 ?? 100));
		}

		[Test]
		public void ExprCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select (p.Value1 ?? 100) + 50,
				from p in db.Parent select (p.Value1 ?? 100) + 50));
		}

		static int GetDefault1()
		{
			return 100;
		}

		[Test]
		public void ClientCoalesce1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? GetDefault1(),
				from p in db.Parent select p.Value1 ?? GetDefault1()));
		}

		static int GetDefault2(int n)
		{
			return n;
		}

		[Test]
		public void ClientCoalesce2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? GetDefault2(p.ParentID),
				from p in db.Parent select p.Value1 ?? GetDefault2(p.ParentID)));
		}

		[Test]
		public void PreferServerFunc1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Person select p.FirstName.Length,
				from p in db.Person select p.FirstName.Length));
		}

		[Test]
		public void PreferServerFunc2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Person select p.FirstName.Length + "".Length,
				from p in db.Person select p.FirstName.Length + "".Length));
		}

		class Test
		{
			class Entity
			{
				public Test TestField = null;
			}

			public Test TestClosure(ITestDataContext db)
			{
				return db.Person.Select(_ => new Entity { TestField = this }).First().TestField;
			}
		}

		[Test]
		public void ClosureTest()
		{
			ForEachProvider(db => Assert.AreNotEqual(
				new Test().TestClosure(db),
				new Test().TestClosure(db)));
		}

		[Test]
		public void ExecuteTest()
		{
			using (var db = new NorthwindDB())
			{
				var emp = db.Employee;

				Expression<Func<int>> m = () => emp.Count();

				var exp = Expression.Call(((MethodCallExpression)m.Body).Method, emp.Expression);

				var results = (int)((IQueryable)emp).Provider.Execute(exp);
			}
		}

		class MyClass
		{
			public int ID;
		}

		[Test]
		public void NewObjectTest()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				select new { ID = new MyClass { ID = p.ParentID } } into p1
				where p1.ID.ID == 1
				select new { ID = new MyClass { ID = p1.ID.ID } },
				from p in db.Parent
				select new { ID = new MyClass { ID = p.ParentID } } into p1
				where p1.ID.ID == 1
				select new { ID = new MyClass { ID = p1.ID.ID } }));
		}
	}
}
