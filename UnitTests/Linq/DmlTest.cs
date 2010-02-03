using System;
using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Data.Linq;

using NUnit.Framework;

using Data.Linq;
using Data.Linq.Model;

namespace Update
{
	[TestFixture]
	public class DmlTest : TestBase
	{
		[Test]
		public void Delete1()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				new SqlQuery().Delete(db, parent);
				new SqlQuery().Insert(db, parent);

				Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID));
				Assert.AreEqual(1, db.Parent.Delete(p => p.ParentID == parent.ParentID));
				Assert.AreEqual(0, db.Parent.Count (p => p.ParentID == parent.ParentID));
			});
		}

		[Test]
		public void Delete2()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				new SqlQuery().Delete(db, parent);
				new SqlQuery().Insert(db, parent);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));
				Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == parent.ParentID).Delete());
				Assert.AreEqual(0, db.Parent.Count(p => p.ParentID == parent.ParentID));
			});
		}

		[Test]
		public void Delete3()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				db.Child.Delete(c => new[] { 1001, 1002 }.Contains(c.ChildID));

				new SqlQuery().Insert(db, new Child { ParentID = 1, ChildID = 1001 });
				new SqlQuery().Insert(db, new Child { ParentID = 1, ChildID = 1002 });

				Assert.AreEqual(3, db.Child.Count(c => c.ParentID == 1));
				Assert.AreEqual(2, db.Child.Where(c => c.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(c.ChildID)).Delete());
				Assert.AreEqual(1, db.Child.Count(c => c.ParentID == 1));
			});
		}

		[Test]
		public void Delete4()
		{
			ForEachProvider(db =>
			{
				db.GrandChild1.Delete(gc => new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value));

				new SqlQuery().Insert(db, new GrandChild { ParentID = 1, ChildID = 1, GrandChildID = 1001 });
				new SqlQuery().Insert(db, new GrandChild { ParentID = 1, ChildID = 2, GrandChildID = 1002 });

				Assert.AreEqual(3, db.GrandChild1.Count(gc => gc.ParentID == 1));
				Assert.AreEqual(2, db.GrandChild1.Where(gc => gc.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value)).Delete());
				Assert.AreEqual(1, db.GrandChild1.Count(gc => gc.ParentID == 1));
			});
		}

		[Test]
		public void Update1()
		{
			ForEachProvider(db =>
			{
				try
				{
					var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

					db.Parent.Delete(p => p.ParentID > 1000);
					new SqlQuery().Insert(db, parent);

					Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Update(p => p.ParentID == parent.ParentID, p => new Parent { ParentID = p.ParentID + 1 }));
					Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update2()
		{
			ForEachProvider(db =>
			{
				try
				{
					var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

					db.Parent.Delete(p => p.ParentID > 1000);
					new SqlQuery().Insert(db, parent);

					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == parent.ParentID).Update(p => new Parent { ParentID = p.ParentID + 1 }));
					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update3()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					new SqlQuery().Insert(db, new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1, db.Child.Where(c => c.ChildID == id && c.Parent.Value1 == 1).Update(c => new Child { ChildID = c.ChildID + 1 }));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update4()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					new SqlQuery().Insert(db, new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == id && c.Parent.Value1 == 1)
							.Set(c => c.ChildID, c => c.ChildID + 1)
							.Update());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}
	}
}
