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
				var list = (from p in db.Person where p.PersonID == 1 select p).ToList();
				Assert.AreNotEqual(0, list.Count);
			});
		}

	}
}
