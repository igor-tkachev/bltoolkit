using System.Linq;

using BLToolkit.Data.DataProvider;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ElementOperationTest : TestBase
	{
		[Test]
		public void First()
		{
			var expected = Parent.OrderByDescending(p => p.ParentID).First().ParentID;
			ForEachProvider(db => Assert.AreEqual(expected, db.Parent.OrderByDescending(p => p.ParentID).First().ParentID));
		}

		[Test]
		public void FirstWhere()
		{
			ForEachProvider(db => Assert.AreEqual(2, db.Parent.First(p => p.ParentID == 2).ParentID));
		}

		[Test]
		public void FirstOrDefault()
		{
			ForEachProvider(db => Assert.IsNull((from p in db.Parent where p.ParentID == 100 select p).FirstOrDefault()));
		}

		[Test]
		public void FirstOrDefaultWhere()
		{
			ForEachProvider(db => Assert.AreEqual(2, db.Parent.FirstOrDefault(p => p.ParentID == 2).ParentID));
		}

		[Test]
		public void Single()
		{
			ForEachProvider(db => Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == 1).Single().ParentID));
		}

		[Test]
		public void SingleWhere()
		{
			ForEachProvider(db => Assert.AreEqual(2, db.Parent.Single(p => p.ParentID == 2).ParentID));
		}

		[Test]
		public void SingleOrDefault()
		{
			ForEachProvider(db => Assert.IsNull((from p in db.Parent where p.ParentID == 100 select p).SingleOrDefault()));
		}

		[Test]
		public void SingleOrDefaultWhere()
		{
			ForEachProvider(db => Assert.AreEqual(2, db.Parent.SingleOrDefault(p => p.ParentID == 2).ParentID));
		}

		[Test]
		public void FirstOrDefaultScalar()
		{
			ForEachProvider(db => Assert.AreEqual(
				   Parent.OrderBy(p => p.ParentID).FirstOrDefault().ParentID,
				db.Parent.OrderBy(p => p.ParentID).FirstOrDefault().ParentID));
		}

		[Test]
		public void NestedFirstOrDefaultScalar1()
		{
			ForEachProvider(new[] { ProviderName.SqlCe, ProviderName.Informix, ProviderName.Sybase }, db => AreEqual(
				from p in    Parent select    Child.FirstOrDefault().ChildID,
				from p in db.Parent select db.Child.FirstOrDefault().ChildID));
		}

		[Test]
		public void NestedFirstOrDefaultScalar2()
		{
			ForEachProvider(new[] { ProviderName.SqlCe, ProviderName.Informix, "Oracle", ProviderName.Sybase }, db =>
			{
				var result =
					from p in db.Parent
					select new
					{
						p.ParentID,
						MaxChild = db.Child.Where(c => c.Parent == p).OrderByDescending(c => c.ChildID * c.ParentID).FirstOrDefault().ChildID
					};

				var list = result.ToList();
				Assert.Greater(list.Count, 0);
			});
		}
	}
}
