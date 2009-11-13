using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	[TestFixture]
	public class OrderByTest : TestBase
	{
		[Test]
		public void OrderBy1()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID descending, ch.ChildID ascending
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ParentID descending, ch.ChildID ascending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy2()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID descending, ch.ChildID ascending
				select ch;

			ForEachProvider(db =>
			{
				var result = 
					from ch in db.Child
					orderby ch.ParentID descending, ch.ChildID ascending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy3()
		{
			var expected =
				from ch in
					from ch in Child
					orderby ch.ParentID descending
					select ch
				orderby ch.ParentID descending , ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ParentID descending , ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy4()
		{
			var expected =
				from ch in
					from ch in Child
					orderby ch.ParentID descending
					select ch
				orderby ch.ParentID descending, ch.ChildID, ch.ParentID + 1 descending
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ParentID descending, ch.ChildID, ch.ParentID + 1 descending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy5()
		{
			var expected =
				from ch in Child
				orderby ch.ChildID % 2, ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ChildID % 2, ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void ConditionOrderBy()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID > 0 && ch.ChildID != ch.ParentID descending, ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ParentID > 0 && ch.ChildID != ch.ParentID descending, ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelf1()
		{
			var expected = from p in Parent orderby p select p;

			ForEachProvider(db =>
			{
				var result = from p in db.Parent orderby p select p;
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelf2()
		{
			var expected = from p in Parent1 orderby p select p;

			ForEachProvider(db =>
			{
				var result = from p in db.Parent1 orderby p select p;
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany1()
		{
			var expected =
				from p in Parent.OrderBy(p => p.ParentID)
				from c in Child. OrderBy(c => c.ChildID)
				where p == c.Parent
				select new { p.ParentID, c.ChildID };

			ForEachProvider(new[] { ProviderName.Access }, db =>
			{
				var result =
					from p in db.Parent.OrderBy(p => p.ParentID)
					from c in db.Child. OrderBy(c => c.ChildID)
					where p == c.Parent
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany2()
		{
			var expected =
				from p in Parent1.OrderBy(p => p.ParentID)
				from c in Child.  OrderBy(c => c.ChildID)
				where p.ParentID == c.Parent1.ParentID
				select new { p.ParentID, c.ChildID };

			ForEachProvider(db =>
			{
				var result =
					from p in db.Parent1.OrderBy(p => p.ParentID)
					from c in db.Child.  OrderBy(c => c.ChildID)
					where p == c.Parent1
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany3()
		{
			var expected =
				from p in Parent.OrderBy(p => p.ParentID)
				from c in Child. OrderBy(c => c.ChildID)
				where c.Parent == p
				select new { p.ParentID, c.ChildID };

			ForEachProvider(new[] { ProviderName.Access }, db =>
			{
				var result =
					from p in db.Parent.OrderBy(p => p.ParentID)
					from c in db.Child. OrderBy(c => c.ChildID)
					where c.Parent == p
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}
	}
}
