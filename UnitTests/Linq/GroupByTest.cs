using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class GroupByTest : TestBase
	{
		[Test]
		public void Simple1()
		{
			BLToolkit.Common.Configuration.Linq.PreloadGroups = true;

			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					group ch by ch.ParentID;

				var list = q.ToList().OrderBy(n => n.Key).ToList();

				Assert.AreEqual(4, list.Count);

				for (var i = 0; i < list.Count; i++)
				{
					var values = list[i].OrderBy(c => c.ChildID).ToList();

					Assert.AreEqual(i + 1, list[i].Key);
					Assert.AreEqual(i + 1, values.Count);

					for (var j = 0; j < values.Count; j++)
						Assert.AreEqual((i + 1) * 10 + j + 1, values[j].ChildID);
				}
			});
		}

		[Test]
		public void Simple2()
		{
			BLToolkit.Common.Configuration.Linq.PreloadGroups = false;

			ForEachProvider(db =>
			{
				var q =
					from ch in db.GrandChild
					group ch by new { ch.ParentID, ch.ChildID };

				var list = q.ToList();

				Assert.AreEqual   (8, list.Count);
				Assert.AreNotEqual(0, list.OrderBy(c => c.Key.ParentID).First().ToList().Count);
			});
		}

		[Test]
		public void Simple3()
		{
			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					group ch by ch.ParentID into g
					select g.Key;

				var list = q.ToList().OrderBy(n => n).ToList();

				Assert.AreEqual(4, list.Count);
				for (var i = 0; i < list.Count; i++) Assert.AreEqual(i + 1, list[i]);
			});
		}

		[Test]
		public void Simple4()
		{
			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					group ch by ch.ParentID into g
					orderby g.Key
					select g.Key;

				var list = q.ToList();

				Assert.AreEqual(4, list.Count);
				for (var i = 0; i < list.Count; i++) Assert.AreEqual(i + 1, list[i]);
			});
		}

		[Test]
		public void Simple5()
		{
			var expected =
				from ch in GrandChild
				group ch by new { ch.ParentID, ch.ChildID } into g
				group g  by new { g.Key.ParentID }          into g
				select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in db.GrandChild
				group ch by new { ch.ParentID, ch.ChildID } into g
				group g  by new { g.Key.ParentID }          into g
				select g.Key));
		}

		[Test]
		public void Simple6()
		{
			ForEachProvider(db =>
			{
				var q    = db.GrandChild.GroupBy(ch => new { ch.ParentID, ch.ChildID }, ch => ch.GrandChildID);
				var list = q.ToList();

				Assert.AreNotEqual(0, list[0].Count());
				Assert.AreEqual   (8, list.Count);
			});
		}

		[Test]
		public void Simple7()
		{
			ForEachProvider(db =>
			{
				var q = db.GrandChild
					.GroupBy(ch => new { ch.ParentID, ch.ChildID }, ch => ch.GrandChildID)
					.Select (gr => new { gr.Key.ParentID, gr.Key.ChildID });

				var list = q.ToList();
				Assert.AreEqual(8, list.Count);
			});
		}

		[Test]
		public void Simple8()
		{
			ForEachProvider(db =>
			{
				var q = db.GrandChild.GroupBy(ch => new { ch.ParentID, ch.ChildID }, (g,ch) => g.ChildID);

				var list = q.ToList();
				Assert.AreEqual(8, list.Count);
			});
		}

		[Test]
		public void Simple9()
		{
			ForEachProvider(db =>
			{
				var q    = db.GrandChild.GroupBy(ch => new { ch.ParentID, ch.ChildID }, ch => ch.GrandChildID,  (g,ch) => g.ChildID);
				var list = q.ToList();

				Assert.AreEqual(8, list.Count);
			});
		}

		[Test]
		public void Simple10()
		{
			var expected = (from ch in Child group ch by ch.ParentID into g select g).ToList().OrderBy(p => p.Key).ToList();

			ForEachProvider(db =>
			{
				var result = (from ch in db.Child group ch by ch.ParentID into g select g).ToList().OrderBy(p => p.Key).ToList();

				AreEqual(expected[0], result[0]);
				AreEqual(expected.Select(p => p.Key), result.Select(p => p.Key));
			});
		}

		[Test]
		public void SubQuery1()
		{
			var n = 1;

			var expected =
				from ch in
					from ch in Child select ch.ParentID + 1
				where ch + 1 > n group ch by ch into g select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select ch.ParentID + 1
				where ch > n group ch by ch into g select g.Key));
		}

		[Test]
		public void SubQuery2()
		{
			var n = 1;

			var expected =
				from ch in Child select new { ParentID = ch.ParentID + 1 } into ch
				where ch.ParentID > n
				group ch by ch into g select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child select new { ParentID = ch.ParentID + 1 } into ch
				where ch.ParentID > n
				group ch by ch into g select g.Key));
		}

		[Test]
		public void Calculated1()
		{
			var expected = 
				(
					from ch in Child
					group ch by ch.ParentID > 2 ? ch.ParentID > 3 ? "1" : "2" : "3"
					into g select g
				).ToList().OrderBy(p => p.Key).ToList();

			ForEachProvider(db =>
			{
				var result =
					(
						from ch in db.Child
						group ch by ch.ParentID > 2 ? ch.ParentID > 3 ? "1" : "2" : "3"
						into g select g
					).ToList().OrderBy(p => p.Key).ToList();

				AreEqual(expected[0], result[0]);
				AreEqual(expected.Select(p => p.Key), result.Select(p => p.Key));
			});
		}

		[Test]
		public void Calculated2()
		{
			var expected =
				from p in
					from ch in
						from ch in Child
						group ch by ch.ParentID > 2 ? ch.ParentID > 3 ? "1" : "2" : "3"
						into g select g
					select ch.Key + "2"
				where p == "22"
				select p;

			ForEachProvider(db => AreEqual(expected,
				from p in
					from ch in
						from ch in db.Child
						group ch by ch.ParentID > 2 ? ch.ParentID > 3 ? "1" : "2" : "3"
						into g select g
					select ch.Key + "2"
				where p == "22"
				select p));
		}

		[Test]
		public void Sum1()
		{
			var expected = Child.GroupBy(ch => ch.ParentID).GroupBy(ch => ch).GroupBy(ch => ch).Select(p => p.Key.Key.Key);

			ForEachProvider(db => AreEqual(expected, db.Child.GroupBy(ch => ch.ParentID).GroupBy(ch => ch).GroupBy(ch => ch).Select(p => p.Key.Key.Key)));
		}

		[Test]
		public void Sum2()
		{
			var expected =
				from ch in Child
				group ch by ch.ParentID into g
				select g.Sum(p => p.ChildID);

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
				group ch by ch.ParentID into g
				select g.Sum(p => p.ChildID)));
		}

		[Test]
		public void Sum3()
		{
			var expected =
				from ch in Child
				group ch by ch.ParentID into g
				select new { Sum = g.Sum(p => p.ChildID) };

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
				group ch by ch.ParentID into g
				select new { Sum = g.Sum(p => p.ChildID) }));
		}

		[Test]
		public void SumSubQuery1()
		{
			var n = 1;

			var expected =
				from ch in
					from ch in Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n group ch by ch into g
				select g.Sum(p => p.ParentID - 3);

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n group ch by ch into g
				select g.Sum(p => p.ParentID - 3)));
		}

		[Test]
		public void Aggregates()
		{
			var expected =
				from ch in Child
				group ch by ch.ParentID into g
				select new
				{
					Sum = g.Sum(c => c.ChildID),
					Min = g.Min(c => c.ChildID),
					Max = g.Max(c => c.ChildID),
					Avg = (int)g.Average(c => c.ChildID),
					Cnt = g.Count()
				};

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
				group ch by ch.ParentID into g
				select new
				{
					Sum = g.Sum(c => c.ChildID),
					Min = g.Min(c => c.ChildID),
					Max = g.Max(c => c.ChildID),
					Avg = (int)g.Average(c => c.ChildID),
					Cnt = g.Count()
				}));
		}

		[Test]
		public void Min()
		{
			var expected = Child.Min(c => c.ChildID);
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Min(c => c.ChildID)));
		}

		[Test]
		public void Max()
		{
			var expected = Child.Max(c => c.ChildID);
			Assert.AreNotEqual(0, expected);
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Max(c => c.ChildID)));
		}

		[Test]
		public void Average()
		{
			var expected = Child.Average(c => c.ChildID);
			Assert.AreNotEqual(0, expected);
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Average(c => c.ChildID)));
		}

		[Test]
		public void Count1()
		{
			var expected =
				from ch in Child
				group ch by ch.ParentID into g
				select g.Count(ch => ch.ChildID > 20);

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
				group ch by ch.ParentID into g
				select g.Count(ch => ch.ChildID > 20)));
		}

		[Test]
		public void Count21()
		{
			var n = 1;

			var expected =
				from ch in
					from ch in Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n
				group ch by ch into g
				select g.Count(p => p.ParentID < 3);

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n
				group ch by ch into g
				select g.Count(p => p.ParentID < 3)));
		}

		[Test]
		public void Count22()
		{
			var n = 1;

			var expected =
				from ch in
					from ch in Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n
				group ch by new { ch.ParentID } into g
				select g.Count(p => p.ParentID < 3);

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID + 1 > n
				group ch by new { ch.ParentID } into g
				select g.Count(p => p.ParentID < 3)));
		}

		[Test]
		public void Count3()
		{
			var expected =
				from ch in
					from ch in Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID - 1 > 0
				group ch by new { ch.ParentID } into g
				select new
				{
					g.Key.ParentID,
					ChildMin   = g.Min(p => p.ChildID),
					ChildCount = g.Count(p => p.ChildID > 25)
				};

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select new { ParentID = ch.ParentID + 1, ch.ChildID }
				where ch.ParentID - 1 > 0
				group ch by new { ch.ParentID } into g
				select new
				{
					g.Key.ParentID,
					ChildMin   = g.Min(p => p.ChildID),
					ChildCount = g.Count(p => p.ChildID > 25)
				}));
		}

		[Test]
		public void Count4()
		{
			var expected = Child.Count();

			ForEachProvider(db =>
			{
				var result = db.Child.Count();
				Assert.AreEqual(expected, result);
			});
		}

		[Test]
		public void CountWhere()
		{
			var expected = Child.Count(ch => ch.ChildID > 20);
			Assert.AreNotEqual(0, expected);

			ForEachProvider(db =>
			{
				var result = db.Child.Count(ch => ch.ChildID > 20);
				Assert.AreEqual(expected, result);
			});
		}
	}
}
