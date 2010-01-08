using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
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
	}
}
