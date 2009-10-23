using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class Association : TestBase
	{
		[Test]
		public void Test1()
		{
			var expected = from p in Parent select p.Children;
			ForEachProvider(db => AreEqual(expected, from p in db.Parent select p.Children));
		}

		[Test]
		public void Test2()
		{
			var expected = from p in Parent select p.Children.Select(c => c.ChildID);
			ForEachProvider(db => AreEqual(expected, from p in db.Parent select p.Children.Select(c => c.ChildID)));
		}

		[Test]
		public void Test3()
		{
			var expected = from ch in Child where ch.ParentID == 1 select new { ch, ch.Parent };
			ForEachProvider(db => AreEqual(expected, from ch in db.Child where ch.ParentID == 1 select new { ch, ch.Parent }));
		}

		[Test]
		public void Test4()
		{
			var expected =
				from ch in Child
				orderby ch.ChildID
				select Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p);

			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					orderby ch.ChildID
					select db.Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p);

				var list  = q.ToList();
				var elist = expected.ToList();

				Assert.AreEqual(elist.Count(), list.Count);

				for (var i = 0; i < list.Count; i++)
					AreEqual(elist[i], list[i]);
			});
		}
	}
}
