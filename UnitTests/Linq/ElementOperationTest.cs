using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ElementOperationTest : TestBase
	{
		[Test]
		public void First()
		{
			ForEachProvider(db => Assert.AreEqual(5, db.Parent.OrderByDescending(p => p.ParentID).First().ParentID));
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
	}
}
