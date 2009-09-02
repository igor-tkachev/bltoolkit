using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class DistinctTest : TestBase
	{
		[Test]
		public void Distinct()
		{
			ForEachProvider(db =>
			{
				var q = (from ch in db.Child select ch.ParentID).Distinct();
				Assert.AreEqual(4, q.ToList().Count);
			});
		}
	}
}
