using System;
using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using NUnit.Framework;

using Data.Linq;
using Data.Linq.Model;

#region ReSharper disable
// ReSharper disable ConvertToConstant.Local
// ReSharper disable CheckNamespace
#endregion

namespace Update
{
	[TestFixture]
	public class DeleteTest : TestBase
	{
		[Test]
		public void Delete1()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Delete(parent);

				try
				{
					db.Insert(parent);

					Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Delete(p => p.ParentID == parent.ParentID));
					Assert.AreEqual(0, db.Parent.Count (p => p.ParentID == parent.ParentID));
				}
				finally
				{
					db.Delete(parent);
				}
			});
		}

		[Test]
		public void Delete2()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Delete(parent);

				try
				{
					db.Insert(parent);

					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == parent.ParentID).Delete());
					Assert.AreEqual(0, db.Parent.Count(p => p.ParentID == parent.ParentID));
				}
				finally
				{
					db.Delete(parent);
				}
			});
		}

		[Test]
		public void Delete3()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				db.Child.Delete(c => new[] { 1001, 1002 }.Contains(c.ChildID));

				try
				{
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = 1001 });
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = 1002 });

					Assert.AreEqual(3, db.Child.Count(c => c.ParentID == 1));
					Assert.AreEqual(2, db.Child.Where(c => c.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(c.ChildID)).Delete());
					Assert.AreEqual(1, db.Child.Count(c => c.ParentID == 1));
				}
				finally
				{
					db.Child.Delete(c => new[] { 1001, 1002 }.Contains(c.ChildID));
				}
			});
		}

		[Test]
		public void Delete4()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				db.GrandChild1.Delete(gc => new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value));

				try
				{
					db.GrandChild.Insert(() => new GrandChild { ParentID = 1, ChildID = 1, GrandChildID = 1001 });
					db.GrandChild.Insert(() => new GrandChild { ParentID = 1, ChildID = 2, GrandChildID = 1002 });

					Assert.AreEqual(3, db.GrandChild1.Count(gc => gc.ParentID == 1));
					Assert.AreEqual(2, db.GrandChild1.Where(gc => gc.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value)).Delete());
					Assert.AreEqual(1, db.GrandChild1.Count(gc => gc.ParentID == 1));
				}
				finally
				{
					db.GrandChild1.Delete(gc => new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value));
				}
			});
		}

		[Test]
		public void Delete5()
		{
			ForEachProvider(db =>
			{
				var values = new[] { 1001, 1002 };

				db.Parent.Delete(_ => _.ParentID > 1000);

				try
				{
					db.Parent.Insert(() => new Parent { ParentID = values[0], Value1 = 1 });
					db.Parent.Insert(() => new Parent { ParentID = values[1], Value1 = 1 });

					Assert.AreEqual(2, db.Parent.Count(_ => _.ParentID > 1000));
					Assert.AreEqual(2, db.Parent.Delete(_ => values.Contains(_.ParentID)));
					Assert.AreEqual(0, db.Parent.Count(_ => _.ParentID > 1000));
				}
				finally
				{
					db.Parent.Delete(_ => _.ParentID > 1000);
				}
			});
		}

		[Test]
		public void AlterDelete([DataContexts(ProviderName.Informix, ExcludeLinqService=true)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q =
					from p in db.Parent
						join ch in db.Child on p.ParentID equals ch.ParentID into lj1
						from ch in lj1.DefaultIfEmpty()
					where ch != null && ch.ParentID == -1 || ch == null && p.ParentID == -1
					select p;

				q.Delete();

				var sql = ((TestDbManager)db).LastQuery;

				if (sql.Contains("EXISTS"))
					Assert.That(sql.IndexOf("(("), Is.GreaterThan(0));
			}
		}

		[Test]
		public void DeleteMany1([DataContexts(
			ProviderName.Access, ProviderName.DB2, ProviderName.Informix, "Oracle",
			ProviderName.PostgreSQL, ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Firebird
			)] string context)
		{
			using (var db = GetDataContext(context))
			{
				db.Parent.Insert(() => new Parent { ParentID = 1001 });
				db.Child. Insert(() => new Child  { ParentID = 1001, ChildID = 1 });
				db.Child. Insert(() => new Child  { ParentID = 1001, ChildID = 2 });

				try
				{
					var q =
						from p in db.Parent
						where p.ParentID >= 1000
						select p;

					var n = q.SelectMany(p => p.Children).Delete();

					Assert.That(n, Is.GreaterThanOrEqualTo(2));
				}
				finally
				{
					db.Child. Delete(c => c.ParentID >= 1000);
					db.Parent.Delete(c => c.ParentID >= 1000);
				}
			}
		}

		[Test]
		public void DeleteMany2([DataContexts(
			ProviderName.Access, ProviderName.DB2, ProviderName.Informix, "Oracle",
			ProviderName.PostgreSQL, ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Firebird
			)] string context)
		{
			using (var db = GetDataContext(context))
			{
				db.Parent.    Insert(() => new Parent     { ParentID = 1001 });
				db.Child.     Insert(() => new Child      { ParentID = 1001, ChildID = 1 });
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 1, GrandChildID = 1});
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 1, GrandChildID = 2});
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 1, GrandChildID = 3});
				db.Child.     Insert(() => new Child      { ParentID = 1001, ChildID = 2 });
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 2, GrandChildID = 1});
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 2, GrandChildID = 2});
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 2, GrandChildID = 3});

				try
				{
					var q =
						from p in db.Parent
						where p.ParentID >= 1000
						select p;

					var n1 = q.SelectMany(p => p.Children.SelectMany(c => c.GrandChildren)).Delete();
					var n2 = q.SelectMany(p => p.Children).                                 Delete();

					Assert.That(n1, Is.EqualTo(6));
					Assert.That(n2, Is.EqualTo(2));
				}
				finally
				{
					db.GrandChild.Delete(c => c.ParentID >= 1000);
					db.Child.     Delete(c => c.ParentID >= 1000);
					db.Parent.    Delete(c => c.ParentID >= 1000);
				}
			}
		}

		[Test]
		public void DeleteMany3([DataContexts(
			ProviderName.Access, ProviderName.DB2, ProviderName.Informix, "Oracle",
			ProviderName.PostgreSQL, ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Firebird
			)] string context)
		{
			var ids = new[] { 1001 };

			using (var db = GetDataContext(context))
			{
				db.GrandChild.Delete(c => c.ParentID >= 1000);
				db.Child.     Delete(c => c.ParentID >= 1000);
				db.Parent.    Delete(c => c.ParentID >= 1000);

				db.Parent.    Insert(() => new Parent     { ParentID = 1001 });
				db.Child.     Insert(() => new Child      { ParentID = 1001, ChildID = 1 });
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 1, GrandChildID = 1});
				db.GrandChild.Insert(() => new GrandChild { ParentID = 1001, ChildID = 1, GrandChildID = 2});

				try
				{
					var q =
						from p in db.Parent
						where ids.Contains(p.ParentID)
						select p;

					var n1 = q.SelectMany(p => p.Children).SelectMany(gc => gc.GrandChildren).Delete();

					Assert.That(n1, Is.EqualTo(2));
				}
				finally
				{
					db.GrandChild.Delete(c => c.ParentID >= 1000);
					db.Child.     Delete(c => c.ParentID >= 1000);
					db.Parent.    Delete(c => c.ParentID >= 1000);
				}
			}
		}
	}
}
