using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	using Model;

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

				var list = q.ToList().Where(n => n.Key != 6).OrderBy(n => n.Key).ToList();

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

				var list = q.ToList().Where(n => n != 6).OrderBy(n => n).ToList();

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

				var list = q.ToList().Where(n => n != 6).ToList();

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
				AreEqual(expected[0].ToList(), result[0].ToList());
			});
		}

		[Test]
		public void Simple11()
		{
			ForEachProvider(db =>
			{
				var q1 = GrandChild
					.GroupBy(ch => new { ParentID = ch.ParentID + 1, ch.ChildID }, ch => ch.ChildID);

				var q2 = db.GrandChild
					.GroupBy(ch => new { ParentID = ch.ParentID + 1, ch.ChildID }, ch => ch.ChildID);

				var list1 = q1.AsEnumerable().OrderBy(_ => _.Key.ChildID).ToList();
				var list2 = q2.AsEnumerable().OrderBy(_ => _.Key.ChildID).ToList();

				Assert.AreEqual(list1.Count,       list2.Count);
				Assert.AreEqual(list1[0].ToList(), list2[0].ToList());
			});
		}

		[Test]
		public void Simple12()
		{
			ForEachProvider(db =>
			{
				var q = db.GrandChild
					.GroupBy(ch => new { ParentID = ch.ParentID + 1, ch.ChildID }, (g,ch) => g.ChildID);

				var list = q.ToList();
				Assert.AreEqual(8, list.Count);
			});
		}

		[Test]
		public void Simple13()
		{
			ForEachProvider(db =>
			{
				var q = db.GrandChild
					.GroupBy(ch => new { ParentID = ch.ParentID + 1, ch.ChildID }, ch => ch.ChildID, (g,ch) => g.ChildID);

				var list = q.ToList();
				Assert.AreEqual(8, list.Count);
			});
		}

		[Test]
		public void Simple14()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent
				select
					from c in p.Children
					group c by c.ParentID into g
					select g.Key,
				from p in db.Parent
				select
					from c in p.Children
					group c by c.ParentID into g
					select g.Key));
		}

		[Test]
		public void MemberInit()
		{
			ForEachProvider(db => AreEqual(
				from ch in Child
				group ch by new Child { ParentID = ch.ParentID } into g
				select g.Key,
				from ch in db.Child
				group ch by new Child { ParentID = ch.ParentID } into g
				select g.Key));
		}

		[Test]
		public void SubQuery1()
		{
			var n = 1;

			var expected =
				from ch in
					from ch in Child select ch.ParentID + 1
				where ch + 1 > n
				group ch by ch into g
				select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in
					from ch in db.Child select ch.ParentID + 1
				where ch > n
				group ch by ch into g
				select g.Key));
		}

		[Test]
		public void SubQuery2()
		{
			var n = 1;

			var expected =
				from ch in Child select new { ParentID = ch.ParentID + 1 } into ch
				where ch.ParentID > n
				group ch by ch into g
				select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child select new { ParentID = ch.ParentID + 1 } into ch
				where ch.ParentID > n
				group ch by ch into g
				select g.Key));
		}

		[Test]
		public void SubQuery3()
		{
			ForEachProvider(db => AreEqual(
				from ch in
					from ch in Child
					select new { ch, n = ch.ChildID + 1 }
				group ch by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ch.ParentID)
				},
				from ch in
					from ch in db.Child
					select new { ch, n = ch.ChildID + 1 }
				group ch by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ch.ParentID)
				}));
		}

		[Test]
		public void SubQuery31()
		{
			ForEachProvider(db => AreEqual(
				from ch in
					from ch in Child
					select new { ch, n = ch.ChildID + 1 }
				group ch.ch by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ParentID)
				},
				from ch in
					from ch in db.Child
					select new { ch, n = ch.ChildID + 1 }
				group ch.ch by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ParentID)
				}));
		}

		[Test]
		public void SubQuery32()
		{
			ForEachProvider(db => AreEqual(
				from ch in
					from ch in Child
					select new { ch, n = ch.ChildID + 1 }
				group ch.ch.ParentID by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _)
				},
				from ch in
					from ch in db.Child
					select new { ch, n = ch.ChildID + 1 }
				group ch.ch.ParentID by ch.n into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _)
				}));
		}

		[Test]
		public void SubQuery4()
		{
			ForEachProvider(db => AreEqual(
				from ch in Child
				group ch by new { n = ch.ChildID + 1 } into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ParentID)
				},
				from ch in db.Child
				group ch by new { n = ch.ChildID + 1 } into g
				select new
				{
					g.Key,
					Sum = g.Sum(_ => _.ParentID)
				}));
		}

		[Test]
		public void SubQuery5()
		{
			ForEachProvider(db => AreEqual(
				from ch in Child
				join p in Parent on ch.ParentID equals p.ParentID into pg
				from p in pg.DefaultIfEmpty()
				group ch by ch.ChildID into g
				select g.Sum(_ => _.ParentID),
				from ch in db.Child
				join p in db.Parent on ch.ParentID equals p.ParentID into pg
				from p in pg.DefaultIfEmpty()
				group ch by ch.ChildID into g
				select g.Sum(_ => _.ParentID)));
		}

		[Test]
		public void SubQuery6()
		{
			var expected =
				from ch in Child select new { ParentID = ch.ParentID + 1 } into ch
				group ch.ParentID by ch into g
				select g.Key;

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child select new { ParentID = ch.ParentID + 1 } into ch
				group ch.ParentID by ch into g
				select g.Key));
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
		public void GroupBy1()
		{
			ForEachProvider(db => AreEqual(
				   Child.GroupBy(ch => ch.ParentID).GroupBy(ch => ch).GroupBy(ch => ch).Select(p => p.Key.Key.Key),
				db.Child.GroupBy(ch => ch.ParentID).GroupBy(ch => ch).GroupBy(ch => ch).Select(p => p.Key.Key.Key)));
		}

		[Test]
		public void Sum1()
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
		public void Sum2()
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
		public void GroupByMax()
		{
			ForEachProvider(db => AreEqual(
				from ch in    Child group ch.ParentID by ch.ChildID into g select new { Max = g.Max() },
				from ch in db.Child group ch.ParentID by ch.ChildID into g select new { Max = g.Max() }));
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
		public void SelectMax()
		{
			var expected =
				from ch in Child
				group ch by ch.ParentID into g
				select g.Max(c => c.ChildID);

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
				group ch by ch.ParentID into g
				select g.Max(c => c.ChildID)));
		}

		[Test]
		public void JoinMax()
		{
			var expected =
				from ch in Child
					join max in
						from ch in Child
						group ch by ch.ParentID into g
						select g.Max(c => c.ChildID)
					on ch.ChildID equals max
				select ch;

			ForEachProvider(db => AreEqual(expected,
				from ch in db.Child
					join max in
						from ch in db.Child
						group ch by ch.ParentID into g
						select g.Max(c => c.ChildID)
					on ch.ChildID equals max
				select ch));
		}

		[Test]
		public void Min1()
		{
			var expected = Child.Min(c => c.ChildID);
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Min(c => c.ChildID)));
		}

		[Test]
		public void Min2()
		{
			var expected = Child.Select(c => c.ChildID).Min();
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Select(c => c.ChildID).Min()));
		}

		[Test]
		public void Max1()
		{
			var expected = Child.Max(c => c.ChildID);
			Assert.AreNotEqual(0, expected);
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Max(c => c.ChildID)));
		}

		[Test]
		public void Max2()
		{
			var expected =
				from p in Parent
					join c in Child on p.ParentID equals c.ParentID
				where c.ChildID > 20
				select p;

			ForEachProvider(db =>
			{
				var result =
					from p in db.Parent
						join c in db.Child on p.ParentID equals c.ParentID
					where c.ChildID > 20
					select p;

				Assert.AreEqual(expected.Max(p => p.ParentID), result.Max(p => p.ParentID));
			});
		}

		[Test]
		public void Max3()
		{
			var expected = Child.Select(c => c.ChildID).Max();
			ForEachProvider(db => Assert.AreEqual(expected, db.Child.Select(c => c.ChildID).Max()));
		}

		[Test]
		public void Average1()
		{
			var expected = Child.Average(c => c.ChildID);
			Assert.AreNotEqual(0, expected);
			ForEachProvider(db => Assert.AreEqual((int)expected, (int)db.Child.Average(c => c.ChildID)));
		}

		[Test]
		public void Average2()
		{
			var expected = Child.Select(c => c.ChildID).Average();
			ForEachProvider(db => Assert.AreEqual((int)expected, (int)db.Child.Select(c => c.ChildID).Average()));
		}


		[Test]
		public void GrooupByAssociation1()
		{
			ForEachProvider(db => AreEqual(
				from ch in GrandChild1
				group ch by ch.Parent into g
				where g.Count() > 2
				select g.Key.Value1
				,
				from ch in db.GrandChild1
				group ch by ch.Parent into g
				where g.Count() > 2
				select g.Key.Value1));
		}

		[Test]
		public void GrooupByAssociation2()
		{
			ForEachProvider(db => AreEqual(
				from ch in GrandChild1
				group ch by ch.Parent into g
				where g.Count() > 2 && g.Key.ParentID != 1
				select g.Key.Value1
				,
				from ch in db.GrandChild1
				group ch by ch.Parent into g
				where g.Count() > 2 && g.Key.ParentID != 1
				select g.Key.Value1));
		}

		[Test]
		public void GrooupByAssociation3()
		{
			using (var db = new NorthwindDB())
			{
				var result = 
					from p in db.Product
					group p by p.Category into g
					where g.Count() == 12
					select g.Key.CategoryName;

				var list = result.ToList();
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public void GrooupByAssociation4()
		{
			using (var db = new NorthwindDB())
			{
				var result = 
					from p in db.Product
					group p by p.Category into g
					where g.Count() == 12
					select g.Key.CategoryID;

				var list = result.ToList();
				Assert.AreEqual(3, list.Count);
			}
		}

		[Test]
		public void GroupByAggregate1()
		{
			var expected =
				from p in Parent
				group p by p.Children.Count > 0 && p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key;

			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(expected,
				from p in db.Parent
				group p by p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key));
		}

		[Test]
		public void GroupByAggregate11()
		{
			var expected =
				from p in Parent
				where p.Children.Count > 0
				group p by p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key;

			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(expected,
				from p in db.Parent
				where p.Children.Count > 0
				group p by p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key));
		}

		[Test]
		public void GroupByAggregate12()
		{
			var expected =
				from p in Parent
				group p by p.Children.Count > 0 && p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key;

			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(expected,
				from p in db.Parent
				group p by p.Children.Count > 0 && p.Children.Average(c => c.ParentID) > 3 into g
				select g.Key));
		}

		[Test]
		public void GroupByAggregate2()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					(
						from c in Customer
						group c by c.Orders.Count > 0 && c.Orders.Average(o => o.Freight) >= 80
					).ToList().Select(k => k.Key),
					(
						from c in db.Customer
						group c by c.Orders.Average(o => o.Freight) >= 80
					).ToList().Select(k => k.Key));
		}

		[Test]
		public void GroupByAggregate3()
		{
			var expected =
				(
					from p in Parent
					group p by p.Children.Count > 0 && p.Children.Average(c => c.ParentID) > 3
				).ToList().First(g => !g.Key);

			ForEachProvider(new[] { ProviderName.SqlCe }, db => AreEqual(expected,
				(
					from p in db.Parent
					group p by p.Children.Average(c => c.ParentID) > 3
				).ToList().First(g => !g.Key)));
		}

		[Test]
		public void ByJoin()
		{
			ForEachProvider(db => AreEqual(
				from c1 in Child
				join c2 in Child on c1.ChildID equals c2.ChildID + 1
				group c2 by c1.ParentID into g
				select g.Sum(_ => _.ChildID),
				from c1 in db.Child
				join c2 in db.Child on c1.ChildID equals c2.ChildID + 1
				group c2 by c1.ParentID into g
				select g.Sum(_ => _.ChildID)));
		}

		//[Test]
		public void SelectMany()
		{
			ForEachProvider(db => AreEqual(
				   Child.GroupBy(ch => ch.ParentID).SelectMany(g => g),
				db.Child.GroupBy(ch => ch.ParentID).SelectMany(g => g)));
		}
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           