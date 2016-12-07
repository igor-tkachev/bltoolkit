using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class ElementOperationTest : TestBase
	{
		[Test]
		public void First()
		{
			ForEachProvider(db => Assert.AreEqual(
				   Parent.OrderByDescending(p => p.ParentID).First().ParentID,
				db.Parent.OrderByDescending(p => p.ParentID).First().ParentID));
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
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.Sybase }, db => AreEqual(
				from p in    Parent select    Child.FirstOrDefault().ChildID,
				from p in db.Parent select db.Child.FirstOrDefault().ChildID));
		}

		[Test]
		public void NestedFirstOrDefaultScalar2()
		{
			ForEachProvider(
				new[] { ProviderName.Informix, "Oracle", "DevartOracle", ProviderName.Sybase }, db =>
				AreEqual(
					from p in Parent
					select new
					{
						p.ParentID,
						MaxChild =
							Child
								.Where(c => c.Parent == p)
								.OrderByDescending(c => c.ChildID * c.ParentID)
								.FirstOrDefault() == null ?
							0 :
							Child
								.Where(c => c.Parent == p)
								.OrderByDescending(c => c.ChildID * c.ParentID)
								.FirstOrDefault()
								.ChildID
					},
					from p in db.Parent
					select new
					{
						p.ParentID,
						MaxChild = db.Child
							.Where(c => c.Parent == p)
							.OrderByDescending(c => c.ChildID * c.ParentID)
							.FirstOrDefault()
							.ChildID
					}));
		}

		[Test]
		public void NestedFirstOrDefault1()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(
				db => AreEqual(
					from p in    Parent select    Child.FirstOrDefault(),
					from p in db.Parent select db.Child.FirstOrDefault()));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void NestedFirstOrDefault2()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(
				db => AreEqual(
					from p in    Parent select p.Children.FirstOrDefault(),
					from p in db.Parent select p.Children.FirstOrDefault()));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void NestedFirstOrDefault3()
		{
			ForEachProvider(
				// Can be fixed.
				new[] { ProviderName.Informix, ProviderName.Firebird },
				db => AreEqual(
					from p in    Parent select p.Children.Select(c => c.ParentID).Distinct().FirstOrDefault(),
					from p in db.Parent select p.Children.Select(c => c.ParentID).Distinct().FirstOrDefault()));
		}

		[Test]
		public void NestedFirstOrDefault4()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(
				// Can be fixed.
				new[] { ProviderName.Informix, ProviderName.Firebird, ProviderName.PostgreSQL },
				db => AreEqual(
					from p in    Parent select p.Children.Where(c => c.ParentID > 0).Distinct().FirstOrDefault(),
					from p in db.Parent select p.Children.Where(c => c.ParentID > 0).Distinct().FirstOrDefault()));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void NestedFirstOrDefault5()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(
				db => AreEqual(
					from p in    GrandChild select p.Child.Parent.Children.FirstOrDefault(),
					from p in db.GrandChild select p.Child.Parent.Children.FirstOrDefault()));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void NestedSingleOrDefault1()
		{
			ForEachProvider(
				db => AreEqual(
					from p in    Parent select p.Children.Select(c => c.ParentID).Distinct().SingleOrDefault(),
					from p in db.Parent select p.Children.Select(c => c.ParentID).Distinct().SingleOrDefault()));
		}

		[Test]
		public void FirstOrDefaultEntitySet([IncludeDataContexts("Northwind")] string context)
		{
			using (var db = new NorthwindDB())
			{
				AreEqual(
					   Customer.Select(c => c.Orders.FirstOrDefault()),
					db.Customer.Select(c => c.Orders.FirstOrDefault()));
			}
		}

		[Test]
		public void NestedSingleOrDefaultTest([IncludeDataContexts("Northwind")] string context)
		{
			using (var db = new NorthwindDB())
			{
				AreEqual(
					   Customer.Select(c => c.Orders.Take(1).SingleOrDefault()),
					db.Customer.Select(c => c.Orders.Take(1).SingleOrDefault()));
			}
		}

		[Test]
		public void MultipleQuery([IncludeDataContexts("Northwind")] string context)
		{
			using (var db = new NorthwindDB())
			{
				var q =
					from p in db.Product
					select db.Category.Select(zrp => zrp.CategoryName).FirstOrDefault();

				q.ToList();
			}
		}
	}
}
