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
			var expected = (from ch in Child select ch.ParentID).Distinct();

			ForEachProvider(db =>
			{
				var result = (from ch in db.Child select ch.ParentID).Distinct();
				Assert.AreEqual(0, result.ToList().Except(expected).Count());
			});
		}
	}
}
