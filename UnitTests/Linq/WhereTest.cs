using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class WhereTest : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(db =>
			{
				var id = 1;

				var list = (from p in db.Person where p.PersonID == id select p).ToList();
				Assert.AreEqual(1, list.Count);
			});
		}
	}
}
