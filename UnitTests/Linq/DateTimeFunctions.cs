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
		public void CurrentTimestamp()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select new { Now = Sql.CurrentTimestamp };
				Assert.AreEqual(DateTime.Now.Year, q.ToList().First().Now.Year);
			});
		}

		[Test]
		public void Now()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select new { DateTime.Now };
				Assert.AreEqual(DateTime.Now.Year, q.ToList().First().Now.Year);
			});
		}

		#region DatePart

		[Test]
		public void DatePartYear()
		{
			ForEachProvider(db => AreEqual(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartQuarter()
		{
			ForEachProvider(db => AreEqual(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartDay()
		{
			ForEachProvider(db => AreEqual(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void Year()
		{
			ForEachProvider(db => AreEqual(
				from d in from t in    Types select t.DateTimeValue.Year where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Year where d >= 0 select d));
		}

		[Test]
		public void Day()
		{
			ForEachProvider(db => AreEqual(
				from d in from t in    Types select t.DateTimeValue.Day where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Day where d >= 0 select d));
		}

		#endregion

		[Test]
		public void DateAddDays()
		{
			var expected =
				from t in Types
				where Sql.DateAdd(Sql.DateParts.Day, 1, t.DateTimeValue).Day > 0
				select t.DateTimeValue.Day;

			ForEachProvider(db => AreEqual(expected,
				from t in db.Types
				where Sql.DateAdd(Sql.DateParts.Day, 1, t.DateTimeValue).Day > 0
				select t.DateTimeValue.Day));
		}
	}
}
