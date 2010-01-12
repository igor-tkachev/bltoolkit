using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.DataProvider;
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
				ID = c.ChildID,
				c.ParentID,
				Sum = gc.Where(g => g.ChildID == c.ChildID && g.GrandChildID > 0).Sum(g => (int)g.ChildID * g.GrandChildID),
				Count1 = gc.Count(g => g.ChildID == c.ChildID && g.GrandChildID > 0),
                    //AgreesImpl
                        //rates
                        //  .Count(r => r.MessageID == m.ID && r.RateType == MessageRates.Agree),
                    //DisagreesImpl
                        //rates
                        //  .Count(r => r.MessageID == m.ID && r.RateType == MessageRates.DisAgree),
                    //ModeratorialsImpl
                        //db
                        //  .Moderatorials()
                        //  .Count(mod => mod.MessageID == m.ID && (m.LastModerated != null || mod.Create > m.LastModerated)),
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
					ID = c.ChildID,
					c.ParentID,
					Sum = rgc.Where(g => g.ChildID == c.ChildID && g.GrandChildID > 0).Sum(g => (int)g.ChildID * g.GrandChildID),
					Count1 = rgc.Count(g => g.ChildID == c.ChildID && g.GrandChildID > 0),
						//AgreesImpl
							//rates
							//  .Count(r => r.MessageID == m.ID && r.RateType == MessageRates.Agree),
						//DisagreesImpl
							//rates
							//  .Count(r => r.MessageID == m.ID && r.RateType == MessageRates.DisAgree),
						//ModeratorialsImpl
							//db
							//  .Moderatorials()
							//  .Count(mod => mod.MessageID == m.ID && (m.LastModerated != null || mod.Create > m.LastModerated)),
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
	}
}
