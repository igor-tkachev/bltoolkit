using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class SubQuery : TestBase
	{
		[Test]
		public void Test1()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID select ch.ChildID).Max();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID select ch.ChildID).Max()));
		}

		[Test]
		public void Test2()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID > 1 select ch.ChildID).Max();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID > 1 select ch.ChildID).Max()));
		}

		[Test]
		public void Test3()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).SingleOrDefault();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).SingleOrDefault()));
		}

		[Test]
		public void Test4()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).FirstOrDefault();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).FirstOrDefault()));
		}

		static int _testValue = 3;

		[Test]
		public void Test5()
		{
			IEnumerable<int> ids = new[] { 1, 2 };

			var eids = Parent
				.Where(p => ids.Contains(p.ParentID))
				.Select(p => p.Value1 == null ? p.ParentID : p.ParentID + 1)
				.Distinct();

			var expected = eids.Select(id =>
				new 
				{
					id,
					Count1 = Child.Where(p => p.ParentID == id).Count(),
					Count2 = Child.Where(p => p.ParentID == id && p.ParentID == _testValue).Count(),
				});

			ForEachProvider(db =>
			{
				var rids   = db.Parent
					.Where(p => ids.Contains(p.ParentID))
					.Select(p => p.Value1 == null ? p.ParentID : p.ParentID + 1)
					.Distinct();

				var result = rids.Select(id =>
					new
					{
						id,
						Count1 = db.Child.Where(p => p.ParentID == id).Count(),
						Count2 = db.Child.Where(p => p.ParentID == id && p.ParentID == _testValue).Count(),
					});

				AreEqual(expected, result);
			});
		}

		[Test]
		public void Test6()
		{
			var  id = 2;
			bool b  = false;

			var q = Child.Where(c => c.ParentID == id).OrderBy(c => c.ChildID);
			q = b
				? q.OrderBy(m => m.ParentID)
				: q.OrderByDescending(m => m.ParentID);

			var gc = GrandChild;
			var expected = q.Select(c => new
			{
				ID     = c.ChildID,
				c.ParentID,
				Sum    = gc.Where(g => g.ChildID == c.ChildID && g.GrandChildID > 0).Sum(g => (int)g.ChildID * g.GrandChildID),
				Count1 = gc.Count(g => g.ChildID == c.ChildID && g.GrandChildID > 0)
			});

			ForEachProvider(db =>
			{
				var r = db.Child.Where(c => c.ParentID == id).OrderBy(c => c.ChildID);
				r = b
					? r.OrderBy(m => m.ParentID)
					: r.OrderByDescending(m => m.ParentID);

				var rgc = db.GrandChild;
				var result = r.Select(c => new
				{
					ID     = c.ChildID,
					c.ParentID,
					Sum    = rgc.Where(g => g.ChildID == c.ChildID && g.GrandChildID > 0).Sum(g => (int)g.ChildID * g.GrandChildID),
					Count1 = rgc.Count(g => g.ChildID == c.ChildID && g.GrandChildID > 0),
				});

				AreEqual(expected, result);
			});
		}

		[Test]
		public void Test7()
		{
			ForEachProvider(db => AreEqual(
				from c in Child select new
				{
					Count = GrandChild.Where(g => g.ChildID == c.ChildID).Count(),
				},
				from c in db.Child select new
				{
					Count = db.GrandChild.Where(g => g.ChildID == c.ChildID).Count(),
				}));
		}

		[Test]
		public void ObjectCompare()
		{
			var expected =
				from p in Parent
				from c in
					from c in
						from c in Child select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1, Parent = c.Parent }
					where c.ChildID > 0
					select c
				where p == c.Parent
				select new { p.ParentID, c.ChildID };

			ForEachProvider(new[] { ProviderName.Access }, db => AreEqual(expected,
				from p in db.Parent
				from c in
					from c in
						from c in db.Child select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1, Parent = c.Parent }
					where c.ChildID > 0
					select c
				where p == c.Parent
				select new { p.ParentID, c.ChildID }));
		}

		[Test]
		public void TestCount()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				where p.ParentID != 5
				select new { p.ParentID, Count = p.Children.Where(c => c.ParentID == p.ParentID && c.ChildID != 0m).Count() },
				from p in db.Parent
				where p.ParentID != 5
				select new { p.ParentID, Count = p.Children.Where(c => c.ParentID == p.ParentID && c.ChildID != 0m).Count() }));
		}

		[Test]
		public void TestCount2()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				where p.ParentID != 5
				select new { Count = p.Value1 == null ? p.Children.Count : p.Children.Count(c => c.ParentID == p.ParentID) },
				from p in db.Parent
				where p.ParentID != 5
				select new { Count = p.Value1 == null ? p.Children.Count : p.Children.Count(c => c.ParentID == p.ParentID) }));
		}

		[Test]
		public void TestCount3()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				where p.ParentID != 5
				select new { Count = p.Value1 == null ? p.Children.Count() : p.Children.Count(c => c.ParentID == p.ParentID) },
				from p in db.Parent
				where p.ParentID != 5
				select new { Count = p.Value1 == null ? p.Children.Count() : p.Children.Count(c => c.ParentID == p.ParentID) }));
		}

		[Test]
		public void TestCount4()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Count =    Parent.Count(p1 => p1.ParentID == p.ParentID) },
				from p in db.Parent select new { Count = db.Parent.Count(p1 => p1.ParentID == p.ParentID) }));
		}

		[Test]
		public void TestCount5()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Count =    Parent.Where(p1 => p1.ParentID == p.ParentID).Count() },
				from p in db.Parent select new { Count = db.Parent.Where(p1 => p1.ParentID == p.ParentID).Count() }));
		}

		//[Test]
		public void TestCount6()
		{
			ForEachProvider(db => AreEqual(
				   Parent.Take(5).OrderByDescending(p => p.ParentID).Select(p => p.Children.Count()),
				db.Parent.Take(5).OrderByDescending(p => p.ParentID).Select(p => p.Children.Count())));
		}

		[Test]
		public void TestCount7()
		{
			ForEachProvider(
				new[] { ProviderName.SqlCe }, // Fix It
				db => AreEqual(
					from p in    Parent select    Child.Count(c => c.Parent == p),
					from p in db.Parent select db.Child.Count(c => c.Parent == p)));
		}

		//[Test]
		public void TestMaxCount()
		{
			ForEachProvider(db => Assert.AreEqual(
				   Parent.Max(p =>    Child.Count(c => c.Parent.ParentID == p.ParentID)),
				db.Parent.Max(p => db.Child.Count(c => c.Parent.ParentID == p.ParentID))));
		}
	}
}
