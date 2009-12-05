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
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartQuarter()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartMonth()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Month, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Month, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartDayOfYear()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.DayOfYear, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.DayOfYear, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartDay()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartWeek()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Week, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Week, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartWeekDay()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.WeekDay, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.WeekDay, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartHour()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Hour, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Hour, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartMinute()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Minute, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Minute, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartSecond()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Second, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Second, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void DatePartMillisecond()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select Sql.DatePart(Sql.DateParts.Millisecond, t.DateTimeValue) where d >= 0 select d,
				from d in from t in db.Types select Sql.DatePart(Sql.DateParts.Millisecond, t.DateTimeValue) where d >= 0 select d));
		}

		[Test]
		public void Year()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Year where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Year where d >= 0 select d));
		}

		[Test]
		public void Month()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Month where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Month where d >= 0 select d));
		}

		[Test]
		public void DayOfYear()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.DayOfYear where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.DayOfYear where d >= 0 select d));
		}

		[Test]
		public void Day()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Day where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Day where d >= 0 select d));
		}

		[Test]
		public void DayOfWeek()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.DayOfWeek where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.DayOfWeek where d >= 0 select d));
		}

		[Test]
		public void Hour()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Hour where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Hour where d >= 0 select d));
		}

		[Test]
		public void Minute()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Minute where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Minute where d >= 0 select d));
		}

		[Test]
		public void Second()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Second where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Second where d >= 0 select d));
		}

		[Test]
		public void Millisecond()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select t.DateTimeValue.Millisecond where d >= 0 select d,
				from d in from t in db.Types select t.DateTimeValue.Millisecond where d >= 0 select d));
		}

		#endregion

		[Test]
		public void DateAddDays()
		{
			var expected =
				from t in Types
				where Sql.DateAdd(Sql.DateParts.Day, 1, t.DateTimeValue).Day > 0
				select t.DateTimeValue.Day;

			ForEachProvider(db => AreSame(expected,
				from t in db.Types
				where Sql.DateAdd(Sql.DateParts.Day, 1, t.DateTimeValue).Day > 0
				select t.DateTimeValue.Day));
		}
	}
}
