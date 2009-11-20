using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class SetTest : TestBase
	{
		[Test]
		public void Concat1()
		{
			var expected =
				(from p in Parent where p.ParentID == 1 select p).Concat(
				(from p in Parent where p.ParentID == 2 select p));

			ForEachProvider(db => AreEqual(expected, 
				(from p in db.Parent where p.ParentID == 1 select p).Concat(
				(from p in db.Parent where p.ParentID == 2 select p))));
		}

		[Test]
		public void Concat2()
		{
			var expected =
				(from p in Parent where p.ParentID == 1 select p).Concat(
				(from p in Parent where p.ParentID == 2 select p)).Concat(
				(from p in Parent where p.ParentID == 4 select p));

			ForEachProvider(db => AreEqual(expected, 
				(from p in db.Parent where p.ParentID == 1 select p).Concat(
				(from p in db.Parent where p.ParentID == 2 select p)).Concat(
				(from p in db.Parent where p.ParentID == 4 select p))));
		}

		[Test]
		public void Concat3()
		{
			var expected =
				(from p in Parent where p.ParentID == 1 select p).Concat(
				(from p in Parent where p.ParentID == 2 select p).Concat(
				(from p in Parent where p.ParentID == 4 select p)));

			ForEachProvider(db => AreEqual(expected, 
				(from p in db.Parent where p.ParentID == 1 select p).Concat(
				(from p in db.Parent where p.ParentID == 2 select p).Concat(
				(from p in db.Parent where p.ParentID == 4 select p)))));
		}

		[Test]
		public void Concat4()
		{
			var expected =
				(from c in Child where c.ParentID == 1 select c).Concat(
				(from c in Child where c.ParentID == 3 select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1000}).
				Where(c => c.ChildID != 1032));

			ForEachProvider(db => AreEqual(expected, 
				(from c in db.Child where c.ParentID == 1 select c).Concat(
				(from c in db.Child where c.ParentID == 3 select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1000})).
				Where(c => c.ChildID != 1032)));
		}

		[Test]
		public void Concat5()
		{
			var expected =
				(from c in Child where c.ParentID == 1 select c).Concat(
				(from c in Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000}).
				Where(c => c.ChildID != 1032));

			ForEachProvider(new[] { ProviderName.DB2, ProviderName.Informix }, db => AreEqual(expected, 
				(from c in db.Child where c.ParentID == 1 select c).Concat(
				(from c in db.Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000})).
				Where(c => c.ChildID != 1032)));
		}
	}
}
