using System;
using System.Linq;

using NUnit.Framework;

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
	}
}
