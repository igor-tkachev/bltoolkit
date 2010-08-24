using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Association : TestBase
	{
		//[Test]
		public void Test1()
		{
			var expected = from p in Parent select p.Children;
			ForEachProvider(db => AreEqual(expected, from p in db.Parent select p.Children));
		}

		//[Test]
		public void Test2()
		{
			var expected = from p in Parent select p.Children.Select(c => c.ChildID);
			ForEachProvider(db => AreEqual(expected, from p in db.Parent select p.Children.Select(c => c.ChildID)));
		}

		[Test]
		public void Test3()
		{
			ForEachProvider(db => AreEqual(
				from ch in    Child where ch.ParentID == 1 select new { ch, ch.Parent },
				from ch in db.Child where ch.ParentID == 1 select new { ch, ch.Parent }));
		}

		//[Test]
		public void Test4()
		{
			var expected =
				from ch in Child
				orderby ch.ChildID
				select Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p);

			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					orderby ch.ChildID
					select db.Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p);

				var list  = q.ToList();
				var elist = expected.ToList();

				Assert.AreEqual(elist.Count(), list.Count);

				for (var i = 0; i < list.Count; i++)
					AreEqual(elist[i], list[i]);
			});
		}

		[Test]
		public void Test5()
		{
			var expected =
				from p  in Parent
				from ch in p.Children
				where ch.ParentID < 4 || ch.ParentID >= 4
				select new { p.ParentID, ch.ChildID };

			ForEachProvider(db => AreEqual(expected,
				from p  in db.Parent
				from ch in p.Children
				where ch.ParentID < 4 || ch.ParentID >= 4
				select new { p.ParentID, ch.ChildID }));
		}

		[Test]
		public void Test6()
		{
			var expected =
				from p  in Parent
				from ch in p.Children
				where p.ParentID < 4 || p.ParentID >= 4
				select new { p.ParentID };

			ForEachProvider(db => AreEqual(expected,
				from p  in db.Parent
				from ch in p.Children
				where p.ParentID < 4 || p.ParentID >= 4
				select new { p.ParentID }));
		}

		[Test]
		public void Test7()
		{
			var expected =
				from p  in Parent
				from ch in p.Children
				where p.ParentID < 4 || p.ParentID >= 4
				select new { p.ParentID, ch.ChildID };

			ForEachProvider(db => AreEqual(expected,
				from p  in db.Parent
				from ch in p.Children
				where p.ParentID < 4 || p.ParentID >= 4
				select new { p.ParentID, ch.ChildID }));
		}

		[Test]
		public void Test8()
		{
			var expected =
				from p  in Parent
				from ch in p.Children2
				where ch.ParentID < 4 || ch.ParentID >= 4
				select new { p.ParentID, ch.ChildID };

			ForEachProvider(db => AreEqual(expected,
				from p  in db.Parent
				from ch in p.Children2
				where ch.ParentID < 4 || ch.ParentID >= 4
				select new { p.ParentID, ch.ChildID }));
		}

		[Test]
		public void SelectMany1()
		{
			ForEachProvider(db => AreEqual(
				   Parent.SelectMany(p => p.Children.Select(ch => p)),
				db.Parent.SelectMany(p => p.Children.Select(ch => p))));
		}

		[Test]
		public void SelectMany2()
		{
			ForEachProvider(db => AreEqual(
				   Parent.SelectMany(p =>    Child.Select(ch => p)),
				db.Parent.SelectMany(p => db.Child.Select(ch => p))));
		}

		//[Test]
		public void SelectMany3()
		{
			var expected =
				Child
					.GroupBy(ch => ch.Parent)
					.Where(g => g.Count() > 2)
					.SelectMany(
					g => 
						g.Select(ch => ch.Parent));

			ForEachProvider(db => AreEqual(expected,
				db.Child
					.GroupBy(ch => ch.Parent)
					.Where(g => g.Count() > 2)
					.SelectMany(g => g.Select(ch => ch.Parent))));
		}

		//[Test]
		public void SelectMany4()
		{
			var expected =
				Child
					.GroupBy(ch => ch.Parent)
					.Where(g => g.Count() > 2)
					.SelectMany(g => g.Select(ch => ch.Parent.ParentID));

			ForEachProvider(db => AreEqual(expected,
				db.Child
					.GroupBy(ch => ch.Parent)
					.Where(g => g.Count() > 2)
					.SelectMany(g => g.Select(ch => ch.Parent.ParentID))));
		}

		[Test]
		public void SelectMany5()
		{
			ForEachProvider(db => AreEqual(
				   Parent.SelectMany(p => p.Children.Select(ch => p.ParentID)),
				db.Parent.SelectMany(p => p.Children.Select(ch => p.ParentID))));
		}

		[Test]
		public void LeftJoin1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from c in p.Children.DefaultIfEmpty() where p.ParentID >= 4 select new { p, c },
				from p in db.Parent from c in p.Children.DefaultIfEmpty() where p.ParentID >= 4 select new { p, c }));
		}

		[Test]
		public void LeftJoin2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from c in p.Children.DefaultIfEmpty() where p.ParentID >= 4 select new { c, p },
				from p in db.Parent from c in p.Children.DefaultIfEmpty() where p.ParentID >= 4 select new { c, p }));
		}

		[Test]
		public void GroupBy1()
		{
			var expected = from ch in Child group ch by ch.Parent into g select g.Key;
			ForEachProvider(db => AreEqual(expected, from ch in db.Child group ch by ch.Parent into g select g.Key));
		}

		[Test]
		public void GroupBy2()
		{
			var expected = (from ch in Child group ch by ch.Parent1).ToList().Select(g => g.Key);
			ForEachProvider(db => AreEqual(expected, (from ch in db.Child group ch by ch.Parent1).ToList().Select(g => g.Key)));
		}

		[Test]
		public void GroupBy3()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent group p by p.Types.DateTimeValue.Year into g select g.Key,
				from p in db.Parent group p by p.Types.DateTimeValue.Year into g select g.Key));
		}

		[Test]
		public void GroupBy4()
		{
			ForEachProvider(db => AreEqual(
				from p in    Types group p by p.DateTimeValue.Year into g select g.Key,
				from p in db.Types group p by p.DateTimeValue.Year into g select g.Key));
		}

		[Test]
		public void Count1()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(
				from p in    Parent where p.Children.Count > 2 select p,
				from p in db.Parent where p.Children.Count > 2 select p));
		}

		[Test]
		public void EqualsNull1()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					from employee in    Employee where employee.ReportsToEmployee != null select employee.EmployeeID,
					from employee in db.Employee where employee.ReportsToEmployee != null select employee.EmployeeID);
		}

		[Test]
		public void EqualsNull2()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					from employee in    Employee where employee.ReportsToEmployee != null select employee, 
					from employee in db.Employee where employee.ReportsToEmployee != null select employee);
		}

		[Test]
		public void EqualsNull3()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					from employee in    Employee where employee.ReportsToEmployee != null select new { employee.ReportsToEmployee, employee },
					from employee in db.Employee where employee.ReportsToEmployee != null select new { employee.ReportsToEmployee, employee });
		}

		[Test]
		public void StackOverflow1()
		{
			using (var db = new NorthwindDB())
				Assert.AreEqual(
					(from employee in    Employee where employee.Employees.Count > 0 select employee).FirstOrDefault(),
					(from employee in db.Employee where employee.Employees.Count > 0 select employee).FirstOrDefault());
		}

		[Test]
		public void StackOverflow2()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(
				from p in    Parent5 where p.Children.Count != 0 select p,
				from p in db.Parent5 where p.Children.Count != 0 select p));
		}

		[Test]
		public void StackOverflow3()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(
				from p in    Parent5 where p.Children.Count() != 0 select p,
				from p in db.Parent5 where p.Children.Count() != 0 select p));
		}

		[Test]
		public void StackOverflow4()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(
				from p in    Parent5 select new { p.Children.Count },
				from p in db.Parent5 select new { p.Children.Count }));
		}
	}
}
