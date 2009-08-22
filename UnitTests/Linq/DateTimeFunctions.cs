using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Linq
{
	[TestFixture]
	public class DateTimeFunctions : TestBase
	{
		[Test]
		public void GetDate()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select new { Now = Sql.GetDate() };
				Assert.AreEqual(DateTime.Now.Year, q.ToList().First().Now.Year);
			});
		}

		[Test]
		public void Now()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select new { Now = DateTime.Now };
				Assert.AreEqual(DateTime.Now.Year, q.ToList().First().Now.Year);
			});
		}
	}
}
