using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	[TestFixture]
	public class DistinctTest : TestBase
	{
		[Test]
		public void Distinct()
		{
			var expected = (from ch in Child select ch.ParentID).Distinct();
			ForEachProvider(db => AreEqual(expected, (from ch in db.Child select ch.ParentID).Distinct()));
		}

		[Test]
		public void DistinctCount()
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

				Assert.AreEqual(expected.Distinct().Count(), result.Distinct().Count());
			});
		}

		[Test]
		public void DistinctMax()
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

				Assert.AreEqual(expected.Distinct().Max(p => p.ParentID), result.Distinct().Max(p => p.ParentID));
			});
		}

		[Test]
		public void TakeDistinct()
		{
			var expected = (from ch in Child select ch.ParentID).Take(4).Distinct();
			ForEachProvider(new[] { ProviderName.SqlCe, ProviderName.Sybase, ProviderName.SQLite },
				db => AreEqual(expected, (from ch in db.Child select ch.ParentID).Take(4).Distinct()));
		}
	}
}
