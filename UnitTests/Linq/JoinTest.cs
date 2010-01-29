using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class JoinTest : TestBase
	{
		[Test]
		public void InnerJoin1()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in db.Person on p1.ID equals p2.ID
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName });
		}

		[Test]
		public void InnerJoin2()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in db.Person on new { p1.ID, p1.FirstName } equals new { p2.ID, p2.FirstName }
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName });
		}

		[Test]
		public void InnerJoin3()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in
						from p2 in db.Person join p3 in db.Person on new { p2.ID, p2.LastName } equals new { p3.ID, p3.LastName } select new { p2, p3 }
					on new { p1.ID, p1.FirstName } equals new { p2.p2.ID, p2.p2.FirstName }
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.p2.FirstName, LastName = p2.p3.LastName });
		}

		[Test]
		public void InnerJoin4()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in db.Person on new { p1.ID, p1.FirstName } equals new { p2.ID, p2.FirstName }
						join p3 in db.Person on new { p2.ID, p2.LastName } equals new { p3.ID, p3.LastName }
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName, LastName = p3.LastName });
		}

		[Test]
		public void InnerJoin5()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in db.Person on new { p1.ID, p1.FirstName } equals new { p2.ID, p2.FirstName }
						join p3 in db.Person on new { p1.ID, p2.LastName } equals new { p3.ID, p3.LastName }
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName, LastName = p3.LastName });
		}

		[Test]
		public void InnerJoin6()
		{
			TestJohn(db =>
				from p1 in db.Person
					join p2 in from p3 in db.Person select new { ID = p3.ID + 1, p3.FirstName } on p1.ID equals p2.ID - 1
				where p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName });
		}

		[Test]
		public void InnerJoin7()
		{
			var expected =
				from t in
					from ch in Child
						join p in Parent on ch.ParentID equals p.ParentID
					select ch.ParentID + p.ParentID
				where t > 2
				select t;

			ForEachProvider(db => AreEqual(expected,
				from t in
					from ch in db.Child
						join p in db.Parent on ch.ParentID equals p.ParentID
					select ch.ParentID + p.ParentID
				where t > 2
				select t));
		}

		[Test]
		public void InnerJoin8()
		{
			var expected =
				from t in
					from ch in Child
						join p in Parent on ch.ParentID equals p.ParentID
					select new { ID = ch.ParentID + p.ParentID }
				where t.ID > 2
				select t;

			ForEachProvider(db => AreEqual(expected,
				from t in
					from ch in db.Child
						join p in db.Parent on ch.ParentID equals p.ParentID
					select new { ID = ch.ParentID + p.ParentID }
				where t.ID > 2
				select t));
		}

		[Test]
		public void LeftJoin1()
		{
			ForEachProvider(db =>
			{
				var q = 
					from p in db.Parent
						join ch in db.Child on p.ParentID equals ch.ParentID into lj1
					where p.ParentID == 1
					select p;

				var list = q.ToList();

				Assert.AreEqual(1, list.Count);
				Assert.AreEqual(1, list[0].ParentID);
			});
		}

		[Test]
		public void LeftJoin2()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db =>
			{
				var q = 
					from p in db.Parent
						join c in db.Child on p.ParentID equals c.ParentID into lj
					where p.ParentID == 1
					select new { p, lj };

				var list = q.ToList();

				Assert.AreEqual(1, list.Count);
				Assert.AreEqual(1, list[0].p.ParentID);
				Assert.AreEqual(1, list[0].lj.Count());

				var ch = list[0].lj.ToList();

				Assert.AreEqual( 1, ch[0].ParentID);
				Assert.AreEqual(11, ch[0].ChildID);
			});
		}

		[Test]
		public void LeftJoin3()
		{
			ForEachProvider(db =>
			{
				var q = db.Parent
					.GroupJoin(
						db.Child,
						p => p.ParentID,
						ch => ch.ParentID,
						(p, lj1) => new { p, lj1 = new { lj1 } }
					)
					.Where (t => t.p.ParentID == 2)
					.Select(t => new { t.p, t.lj1 });

				var list = q.ToList();

				Assert.AreEqual(2, list.Count);
				Assert.AreEqual(2, list[0].p.ParentID);
			});
		}

		[Test]
		public void LeftJoin4()
		{
			ForEachProvider(db =>
			{
				var q = 
					from p in db.Parent
						join ch in
							from c in db.Child select new { c.ParentID, c.ChildID }
						on p.ParentID equals ch.ParentID into lj1
					where p.ParentID == 3
					select new { p, lj1 };

				var list = q.ToList();

				Assert.AreEqual(3, list.Count);
				Assert.AreEqual(3, list[0].p.ParentID);
			});
		}

		[Test]
		public void LeftJoin5()
		{
			var expected =
				from p in Parent
					join ch in Child on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				where p.ParentID >= 4
				select new { p, ch };

			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
					join ch in db.Child on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				where p.ParentID >= 4
				select new { p, ch }));
		}

		[Test]
		public void LeftJoin6()
		{
			var expected =
				from p in Parent
					join ch in Child on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				select new { p, ch };

			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
					join ch in db.Child on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				select new { p, ch }));
		}

		[Test]
		public void SubQueryJoin()
		{
			var expected =
				from p in Parent
					join ch in 
						from c in Child
						where c.ParentID > 0
						select new { c.ParentID, c.ChildID }
					on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				select p;

			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
					join ch in 
						from c in db.Child
						where c.ParentID > 0
						select new { c.ParentID, c.ChildID }
					on p.ParentID equals ch.ParentID into lj1
					from ch in lj1.DefaultIfEmpty()
				select p));
		}

		[Test]
		public void ReferenceJoin1()
		{
			ForEachProvider(db => AreEqual(
				from c in    Child join g in    GrandChild on c equals g.Child select new { c.ParentID, g.GrandChildID },
				from c in db.Child join g in db.GrandChild on c equals g.Child select new { c.ParentID, g.GrandChildID }));
		}

		[Test]
		public void ReferenceJoin2()
		{
			ForEachProvider(db => AreEqual(
				from g in    GrandChild join c in    Child on g.Child equals c select new { c.ParentID, g.GrandChildID },
				from g in db.GrandChild join c in db.Child on g.Child equals c select new { c.ParentID, g.GrandChildID }));
		}
	}
}
