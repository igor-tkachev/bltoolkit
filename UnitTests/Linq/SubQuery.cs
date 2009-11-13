using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class SubQuery : TestBase
	{
		[Test]
		public void Test1()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID select ch.ChildID).Max();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID select ch.ChildID).Max()));
		}

		//[Test]
		public void Test2()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID > 1 select ch.ChildID).Max();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID > 1 select ch.ChildID).Max()));
		}

		[Test]
		public void Test3()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).SingleOrDefault();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).SingleOrDefault()));
		}

		[Test]
		public void Test4()
		{
			var expected =
				from p in Parent
				where p.ParentID != 5
				select (from ch in Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).FirstOrDefault();
			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID != 5
				select (from ch in db.Child where ch.ParentID == p.ParentID && ch.ChildID == ch.ParentID * 10 + 1 select ch.ChildID).FirstOrDefault()));
		}
	}
}
