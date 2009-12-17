using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
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
				var q = from p in db.Person where p.ID == 1 select new { Now = Sql.OnServer(Sql.GetDate()) };
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

		//[Test]
		public void Parse1()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select DateTime.Parse(Sql.ConvertTo<string>.From(t.DateTimeValue)) where d.Day > 0 select d.Date,
				from d in from t in db.Types select DateTime.Parse(Sql.ConvertTo<string>.From(t.DateTimeValue)) where d.Day > 0 select d.Date));
		}

		//[Test]
		public void Parse2()
		{
			ForEachProvider(db => AreSame(
				from d in from t in    Types select              DateTime.Parse("2001-10-24")  where d.Day > 0 select d,
				from d in from t in db.Types select Sql.OnServer(DateTime.Parse("2001-10-24")) where d.Day > 0 select d));
		}
	}

	#region DatePart

	[TestFixture]
	public class DatePart : TestBase
	{
		[Test]
		public void DatePartYear()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Year, t.DateTimeValue))));
		}

		[Test]
		public void DatePartQuarter()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Quarter, t.DateTimeValue))));
		}

		[Test]
		public void DatePartMonth()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Month, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Month, t.DateTimeValue))));
		}

		[Test]
		public void DatePartDayOfYear()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.DayOfYear, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.DayOfYear, t.DateTimeValue))));
		}

		[Test]
		public void DatePartDay()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Day, t.DateTimeValue))));
		}

		[Test]
		public void DatePartWeek()
		{
			ForEachProvider(db => 
				(from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Week, t.DateTimeValue))).ToList());
		}

		[Test]
		public void DatePartWeekDay()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.WeekDay, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.WeekDay, t.DateTimeValue))));
		}

		[Test]
		public void DatePartHour()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Hour, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Hour, t.DateTimeValue))));
		}

		[Test]
		public void DatePartMinute()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Minute, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Minute, t.DateTimeValue))));
		}

		[Test]
		public void DatePartSecond()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Second, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Second, t.DateTimeValue))));
		}

		[Test]
		public void DatePartMillisecond()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.MySql, ProviderName.Access }, db => AreSame(
				from t in    Types select              Sql.DatePart(Sql.DateParts.Millisecond, t.DateTimeValue),
				from t in db.Types select Sql.OnServer(Sql.DatePart(Sql.DateParts.Millisecond, t.DateTimeValue))));
		}

		[Test]
		public void Year()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Year,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Year)));
		}

		[Test]
		public void Month()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Month,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Month)));
		}

		[Test]
		public void DayOfYear()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.DayOfYear,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.DayOfYear)));
		}

		[Test]
		public void Day()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Day,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Day)));
		}

		[Test]
		public void DayOfWeek()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.DayOfWeek,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.DayOfWeek)));
		}

		[Test]
		public void Hour()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Hour,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Hour)));
		}

		[Test]
		public void Minute()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Minute,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Minute)));
		}

		[Test]
		public void Second()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.Second,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Second)));
		}

		[Test]
		public void Millisecond()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.MySql, ProviderName.Access }, db => AreSame(
				from t in    Types select              t.DateTimeValue.Millisecond,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Millisecond)));
		}
	}

	#endregion

	#region DateAdd

	[TestFixture]
	public class DateAdd : TestBase
	{
		[Test]
		public void DateAddYear()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Year, 1, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Year, 1, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddQuarter()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Quarter, -1, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Quarter, -1, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddMonth()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Month, 2, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Month, 2, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddDayOfYear()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.DayOfYear, 3, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.DayOfYear, 3, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddDay()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Day, 5, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Day, 5, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddWeek()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Week, -1, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Week, -1, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddWeekDay()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.WeekDay, 1, t.DateTimeValue). Date,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.WeekDay, 1, t.DateTimeValue)).Date));
		}

		[Test]
		public void DateAddHour()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Hour, 1, t.DateTimeValue). Hour,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Hour, 1, t.DateTimeValue)).Hour));
		}

		[Test]
		public void DateAddMinute()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Minute, 5, t.DateTimeValue). Minute,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Minute, 5, t.DateTimeValue)).Minute));
		}

		[Test]
		public void DateAddSecond()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              Sql.DateAdd(Sql.DateParts.Second, 41, t.DateTimeValue). Second,
				from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Second, 41, t.DateTimeValue)).Second));
		}

		[Test]
		public void DateAddMillisecond()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.MySql, ProviderName.Access },
				db => (from t in db.Types select Sql.OnServer(Sql.DateAdd(Sql.DateParts.Millisecond, 41, t.DateTimeValue))).ToList());
		}

		[Test]
		public void AddYears()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddYears(1). Date,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddYears(1)).Date));
		}

		[Test]
		public void AddMonths()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddMonths(-2). Date,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddMonths(-2)).Date));
		}

		[Test]
		public void AddDays()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddDays(5). Date,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddDays(5)).Date));
		}

		[Test]
		public void AddHours()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddHours(22). Hour,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddHours(22)).Hour));
		}

		[Test]
		public void AddMinutes()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddMinutes(-8). Minute,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddMinutes(-8)).Minute));
		}

		[Test]
		public void AddSeconds()
		{
			ForEachProvider(db => AreSame(
				from t in    Types select              t.DateTimeValue.AddSeconds(-35). Second,
				from t in db.Types select Sql.OnServer(t.DateTimeValue.AddSeconds(-35)).Second));
		}

		[Test]
		public void AddMilliseconds()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.MySql, ProviderName.Access },
				db => (from t in db.Types select Sql.OnServer(t.DateTimeValue.AddMilliseconds(221))).ToList());
		}

		[Test]
		public void Date()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.OnServer(t.DateTimeValue.Date),
				from t in db.Types select Sql.OnServer(t.DateTimeValue.Date)));
		}
	}

	#endregion
}
