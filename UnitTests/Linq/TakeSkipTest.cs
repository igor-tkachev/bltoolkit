using System;
using System.Linq;
using Data.Linq.Model;
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
			var expected = Child.OrderBy(c => c.ChildID).Take(3);

			ForEachProvider(db =>
			{
				var result = db.Child.OrderBy(c => c.ChildID).Take(3);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void Skip1()
		{
			var expected = Child.Skip(3);
			ForEachProvider(db => AreEqual(expected, db.Child.Skip(3)));
		}

		[Test]
		public void Skip2()
		{
			var expected = (from ch in Child where ch.ChildID > 3 || ch.ChildID < 4 select ch).Skip(3);
			ForEachProvider(db => AreEqual(expected, (from ch in db.Child where ch.ChildID > 3 || ch.ChildID < 4 select ch).Skip(3)));
		}

		[Test]
		public void Skip3()
		{
			var expected = (from ch in Child where ch.ChildID >= 0 && ch.ChildID <= 100 select ch).Skip(3);
			ForEachProvider(db => AreEqual(expected, (from ch in db.Child where ch.ChildID >= 0 && ch.ChildID <= 100 select ch).Skip(3)));
		}

		[Test]
		public void Skip4()
		{
			var expected = Child.OrderByDescending(c => c.ChildID).Skip(3);

			ForEachProvider(db =>
			{
				var result = db.Child.OrderByDescending(c => c.ChildID).Skip(3);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void Skip5()
		{
			var expected = Child.OrderByDescending(c => c.ChildID).ThenBy(c => c.ParentID + 1).Skip(3);
			ForEachProvider(db => AreEqual(expected, db.Child.OrderByDescending(c => c.ChildID).ThenBy(c => c.ParentID + 1).Skip(3)));
		}

		[Test]
		public void SkipTake1()
		{
			var expected = Child.OrderByDescending(c => c.ChildID).Skip(2).Take(5);
			ForEachProvider(db =>
			{
				var result = db.Child.OrderByDescending(c => c.ChildID).Skip(2).Take(5);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void SkipTake2()
		{
			var expected = Child.OrderByDescending(c => c.ChildID).Take(7).Skip(2);
			ForEachProvider(db =>
			{
				var result = db.Child.OrderByDescending(c => c.ChildID).Take(7).Skip(2);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void SkipTake3()
		{
			var expected = Child.OrderBy(c => c.ChildID).Skip(1).Take(7).Skip(2);
			ForEachProvider(db =>
			{
				var result = db.Child.OrderBy(c => c.ChildID).Skip(1).Take(7).Skip(2);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void SkipTake4()
		{
			var expected = Child.OrderByDescending(c => c.ChildID).Skip(1).Take(7).OrderBy(c => c.ChildID).Skip(2);
			ForEachProvider(new[] { ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Sybase, ProviderName.Access }, db =>
			{
				var result = db.Child.OrderByDescending(c => c.ChildID).Skip(1).Take(7).OrderBy(c => c.ChildID).Skip(2);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
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

		[Test]
		public void SkipFirst()
		{
			var expected = (from p in Parent where p.ParentID > 1 select p).Skip(1).First();

			ForEachProvider(db =>
			{
				var result = from p in db.GetTable<Parent>() select p;
				result = from p in result where p.ParentID > 1 select p;
				var b = result.Skip(1).First();

				Assert.AreEqual(expected, b);
			});
		}
	}
}
