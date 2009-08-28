using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class OrderByTest : TestBase
	{
		static void ValidateResult(IEnumerable<Child> q)
		{
			var list = q.ToList();

			Assert.AreEqual(10, list.Count);
			Assert.AreEqual(4, list[0].ParentID); Assert.AreEqual(41, list[0].ChildID);
			Assert.AreEqual(4, list[1].ParentID); Assert.AreEqual(42, list[1].ChildID);
			Assert.AreEqual(4, list[2].ParentID); Assert.AreEqual(43, list[2].ChildID);
			Assert.AreEqual(4, list[3].ParentID); Assert.AreEqual(44, list[3].ChildID);
			Assert.AreEqual(3, list[4].ParentID); Assert.AreEqual(31, list[4].ChildID);
			Assert.AreEqual(3, list[5].ParentID); Assert.AreEqual(32, list[5].ChildID);
			Assert.AreEqual(3, list[6].ParentID); Assert.AreEqual(33, list[6].ChildID);
			Assert.AreEqual(2, list[7].ParentID); Assert.AreEqual(21, list[7].ChildID);
			Assert.AreEqual(2, list[8].ParentID); Assert.AreEqual(22, list[8].ChildID);
			Assert.AreEqual(1, list[9].ParentID); Assert.AreEqual(11, list[9].ChildID);
		}

		[Test]
		public void OrderBy1()
		{
			ForEachProvider(db =>
				ValidateResult(
					from ch in db.Child
					orderby ch.ParentID descending
					orderby ch.ChildID  ascending
					select ch));
		}

		[Test]
		public void OrderBy2()
		{
			ForEachProvider(db =>
				ValidateResult(
					from ch in db.Child
					orderby ch.ParentID descending, ch.ChildID ascending
					select ch));
		}

		[Test]
		public void OrderBy3()
		{
			ForEachProvider(db =>
				ValidateResult(
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ChildID
					select ch)
			);
		}

		[Test]
		public void OrderBy4()
		{
			ForEachProvider(db =>
				ValidateResult(
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ChildID, ch.ParentID + 1 descending
					select ch)
			);
		}
	}
}
