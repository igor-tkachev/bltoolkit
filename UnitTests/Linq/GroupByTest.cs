using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class GroupByTest : TestBase
	{
		[Test]
		public void GroupBy1()
		{
			ForEachProvider(db =>
			{
				var q =
					from ch in db.Child
					group ch by ch.ParentID;

				var list = q.ToList().OrderBy(n => n).ToList();

				Assert.AreEqual(4, list.Count);
				for (var i = 0; i < list.Count; i++) Assert.AreEqual(i + 1, list[i]);
			});
		}

		[Test]
		public void GroupBy2()
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
		public void GroupBy3()
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
	}
}
