using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ExistsTest : TestBase
	{
		[Test]
		public void SingleOrDefaultWhere()
		{
			ForEachProvider(db =>
				AreEqual(
					   Parent.Where(p => db.Child.Where(c => c.ParentID == p.ParentID).Any(c => c.ParentID > 3)),
					db.Parent.Where(p => db.Child.Where(c => c.ParentID == p.ParentID).Any(c => c.ParentID > 3))));
		}
	}
}
