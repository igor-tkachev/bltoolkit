using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	[TestFixture]
	public class TakeSkipTest : TestBase
	{
		[Test]
		public void Take1()
		{
			ForEachProvider(db => Assert.AreEqual(3, (from ch in db.Child select ch).Take(3).ToList().Count));
		}

		static void TakeParam(TestDbManager db, int n)
		{
			Assert.AreEqual(n, (from ch in db.Child select ch).Take(n).ToList().Count);
		}

		[Test]
		public void Take2()
		{
			ForEachProvider(db => TakeParam(db, 1));
		}

		[Test]
		public void Take3()
		{
			ForEachProvider(db =>
				Assert.AreEqual(3, (from ch in db.Child where ch.ChildID > 3 || ch.ChildID < 4 select ch).Take(3).ToList().Count));
		}

		[Test]
		public void Take4()
		{
			ForEachProvider(db =>
				Assert.AreEqual(3, (from ch in db.Child where ch.ChildID >= 0 && ch.ChildID <= 100 select ch).Take(3).ToList().Count));
		}

		[Test]
		public void Take5()
		{
			ForEachProvider(db => Assert.AreEqual(3, db.Child.Take(3).ToList().Count));
		}

		[Test]
		public void Take6()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.OrderBy(c => c.ChildID).Take(3).ToList();
				Assert.AreEqual( 3, list.Count);
				Assert.AreEqual(11, list[0].ChildID);
				Assert.AreEqual(21, list[1].ChildID);
				Assert.AreEqual(22, list[2].ChildID);
			});
		}

		[Test]
		public void Skip1()
		{
			ForEachProvider(db => Assert.AreEqual(7, db.Child.Skip(3).ToList().Count));
		}

		[Test]
		public void Skip2()
		{
			ForEachProvider(db =>
				Assert.AreEqual(7, (from ch in db.Child where ch.ChildID > 3 || ch.ChildID < 4 select ch).Skip(3).ToList().Count));
		}

		[Test]
		public void Skip3()
		{
			ForEachProvider(db =>
				Assert.AreEqual(7, (from ch in db.Child where ch.ChildID >= 0 && ch.ChildID <= 100 select ch).Skip(3).ToList().Count));
		}

		[Test]
		public void Skip4()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.OrderByDescending(c => c.ChildID).Skip(3).ToList();
				Assert.AreEqual( 7, list.Count);
				Assert.AreEqual(41, list[0].ChildID);
				Assert.AreEqual(33, list[1].ChildID);
				Assert.AreEqual(32, list[2].ChildID);
				Assert.AreEqual(31, list[3].ChildID);
				Assert.AreEqual(22, list[4].ChildID);
				Assert.AreEqual(21, list[5].ChildID);
				Assert.AreEqual(11, list[6].ChildID);
	});
		}

		[Test]
		public void Skip5()
		{
			ForEachProvider(db => Assert.AreEqual(7, db.Child.OrderByDescending(c => c.ChildID).ThenBy(c => c.ParentID + 1).Skip(3).ToList().Count));
		}

		[Test]
		public void SkipTake1()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.OrderByDescending(c => c.ChildID).Skip(2).Take(5).ToList();
				Assert.AreEqual( 5, list.Count);
				Assert.AreEqual(42, list[0].ChildID);
				Assert.AreEqual(41, list[1].ChildID);
				Assert.AreEqual(33, list[2].ChildID);
				Assert.AreEqual(32, list[3].ChildID);
				Assert.AreEqual(31, list[4].ChildID);
			});
		}

		[Test]
		public void SkipTake2()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.OrderByDescending(c => c.ChildID).Take(7).Skip(2).ToList();
				Assert.AreEqual( 5, list.Count);
				Assert.AreEqual(42, list[0].ChildID);
				Assert.AreEqual(41, list[1].ChildID);
				Assert.AreEqual(33, list[2].ChildID);
				Assert.AreEqual(32, list[3].ChildID);
				Assert.AreEqual(31, list[4].ChildID);
			});
		}

		[Test]
		public void SkipTake3()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.OrderBy(c => c.ChildID).Skip(1).Take(7).Skip(2).ToList();
				Assert.AreEqual( 5, list.Count);
				Assert.AreEqual(31, list[0].ChildID);
				Assert.AreEqual(32, list[1].ChildID);
				Assert.AreEqual(33, list[2].ChildID);
				Assert.AreEqual(41, list[3].ChildID);
				Assert.AreEqual(42, list[4].ChildID);
			});
		}

		[Test]
		public void SkipTake4()
		{
			ForEachProvider(new[] { ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Sybase, ProviderName.Access }, db =>
			{
				var list = db.Child.OrderByDescending(c => c.ChildID).Skip(1).Take(7).OrderBy(c => c.ChildID).Skip(2).ToList();
				Assert.AreEqual( 5, list.Count);
				Assert.AreEqual(32, list[0].ChildID);
				Assert.AreEqual(33, list[1].ChildID);
				Assert.AreEqual(41, list[2].ChildID);
				Assert.AreEqual(42, list[3].ChildID);
				Assert.AreEqual(43, list[4].ChildID);
			});
		}

		[Test]
		public void SkipTake5()
		{
			ForEachProvider(db =>
			{
				var list = db.Child.Skip(2).Take(5).ToList();
				Assert.AreEqual( 5, list.Count);
			});
		}
	}
}
