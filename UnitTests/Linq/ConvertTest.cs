using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	[TestFixture]
	public class ConvertTest : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(new[] { ProviderName.SQLite },
				db => Assert.AreEqual(3, (from t in db.Types where t.MoneyValue * t.ID == 9.99m  select t).Single().ID));
		}

		[Test]
		public void ToInt1()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select              Sql.ConvertTo<int>.From(t.MoneyValue),
				from t in db.Types select Sql.OnServer(Sql.ConvertTo<int>.From(t.MoneyValue))));
		}

		[Test]
		public void ToInt2()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select              Sql.Convert<int,decimal>(t.MoneyValue),
				from t in db.Types select Sql.OnServer(Sql.Convert<int,decimal>(t.MoneyValue))));
		}

		[Test]
		public void ToBigInt()
		{
			ForEachProvider(new[] { ProviderName.MySql }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.BigInt, t.MoneyValue),
				from t in db.Types select Sql.Convert(Sql.BigInt, t.MoneyValue)));
		}

		[Test]
		public void ToInt64()
		{
			ForEachProvider(new[] { ProviderName.MySql }, db => AreEqual(
				from p in from t in    Types select (Int64)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (Int64)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToInt64()
		{
			ForEachProvider(new[] { ProviderName.MySql }, db => AreEqual(
				from p in from t in    Types select Convert.ToInt64(t.MoneyValue) where p > 0 select p,
				from p in from t in db.Types select Convert.ToInt64(t.MoneyValue) where p > 0 select p));
		}

		[Test]
		public void ToInt()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.Int, t.MoneyValue),
				from t in db.Types select Sql.Convert(Sql.Int, t.MoneyValue)));
		}

		[Test]
		public void ToInt32()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (Int32)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (Int32)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToInt32()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToInt32(t.MoneyValue) where p > 0 select p,
				from p in from t in db.Types select Convert.ToInt32(t.MoneyValue) where p > 0 select p));
		}

		[Test]
		public void ToSmallInt()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.SmallInt, t.MoneyValue),
				from t in db.Types select Sql.Convert(Sql.SmallInt, t.MoneyValue)));
		}

		[Test]
		public void ToInt16()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (Int16)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (Int16)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToInt16()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToInt16(t.MoneyValue) where p > 0 select p,
				from p in from t in db.Types select Convert.ToInt16(t.MoneyValue) where p > 0 select p));
		}

		[Test]
		public void ToTinyInt()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.TinyInt, t.MoneyValue),
				from t in db.Types select Sql.Convert(Sql.TinyInt, t.MoneyValue)));
		}

		[Test]
		public void ToByte()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (byte)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (byte)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToByte()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToByte(t.MoneyValue) where p > 0 select p,
				from p in from t in db.Types select Convert.ToByte(t.MoneyValue) where p > 0 select p));
		}

		[Test]
		public void ToDefaultDecimal()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (int)Sql.Convert(Sql.DefaultDecimal, t.MoneyValue),
				from t in db.Types select (int)Sql.Convert(Sql.DefaultDecimal, t.MoneyValue)));
		}

		[Test]
		public void ToDecimal1()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (int)Sql.Convert(Sql.Decimal(10), t.MoneyValue),
				from t in db.Types select (int)Sql.Convert(Sql.Decimal(10), t.MoneyValue)));
		}

		[Test]
		public void ToDecimal2()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.Decimal(10,4), t.MoneyValue),
				from t in db.Types select Sql.Convert(Sql.Decimal(10,4), t.MoneyValue)));
		}

		[Test]
		public void ToDecimal3()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (Decimal)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (Decimal)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToDecimal()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToDecimal(t.MoneyValue) where p > 0 select p,
				from p in from t in db.Types select Convert.ToDecimal(t.MoneyValue) where p > 0 select p));
		}

		[Test]
		public void ToMoney()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (int)Sql.Convert(Sql.Money, t.MoneyValue),
				from t in db.Types select (int)Sql.Convert(Sql.Money, t.MoneyValue)));
		}

		[Test]
		public void ToSmallMoney()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (decimal)Sql.Convert(Sql.SmallMoney, t.MoneyValue),
				from t in db.Types select (decimal)Sql.Convert(Sql.SmallMoney, t.MoneyValue)));
		}

		[Test]
		public void ToSqlFloat()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (int)Sql.Convert(Sql.Float, t.MoneyValue),
				from t in db.Types select (int)Sql.Convert(Sql.Float, t.MoneyValue)));
		}

		[Test]
		public void ToDouble()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (int)(Double)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (int)(Double)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToDouble()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToDouble(t.MoneyValue) where p > 0 select (int)p,
				from p in from t in db.Types select Convert.ToDouble(t.MoneyValue) where p > 0 select (int)p));
		}

		[Test]
		public void ToSqlReal()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select (int)Sql.Convert(Sql.Real, t.MoneyValue),
				from t in db.Types select (int)Sql.Convert(Sql.Real, t.MoneyValue)));
		}

		[Test]
		public void ToSingle()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select (Single)t.MoneyValue where p > 0 select p,
				from p in from t in db.Types select (Single)t.MoneyValue where p > 0 select p));
		}

		[Test]
		public void ConvertToSingle()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToSingle(t.MoneyValue) where p > 0 select (int)p,
				from p in from t in db.Types select Convert.ToSingle(t.MoneyValue) where p > 0 select (int)p));
		}

		[Test]
		public void ToSqlDateTime()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DateTime, t.DateTimeValue.Year + "-01-01 00:20:00"),
				from t in db.Types select Sql.Convert(Sql.DateTime, t.DateTimeValue.Year + "-01-01 00:20:00")));
		}

		[Test]
		public void ToSqlDateTime2()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DateTime2, t.DateTimeValue.Year + "-01-01 00:20:00"),
				from t in db.Types select Sql.Convert(Sql.DateTime2, t.DateTimeValue.Year + "-01-01 00:20:00")));
		}

		[Test]
		public void ToSqlSmallDateTime()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.SmallDateTime, t.DateTimeValue.Year + "-01-01 00:20:00"),
				from t in db.Types select Sql.Convert(Sql.SmallDateTime, t.DateTimeValue.Year + "-01-01 00:20:00")));
		}

		[Test]
		public void ToSqlDate()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.Date, t.DateTimeValue.Year + "-01-01"),
				from t in db.Types select Sql.Convert(Sql.Date, t.DateTimeValue.Year + "-01-01")));
		}

		[Test]
		public void ToSqlTime()
		{
			ForEachProvider(new[] { ProviderName.SQLite }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.Time, t.DateTimeValue.Hour + ":01:01"),
				from t in db.Types select Sql.Convert(Sql.Time, t.DateTimeValue.Hour + ":01:01")));
		}

		DateTime ToDateTime(DateTimeOffset dto)
		{
			return new DateTime(dto.Year, dto.Month, dto.Day, dto.Hour, dto.Minute, dto.Second);
		}

		[Test]
		public void ToSqlDateTimeOffset()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select ToDateTime(Sql.Convert(Sql.DateTimeOffset, t.DateTimeValue.Year + "-01-01 00:20:00")),
				from t in db.Types select ToDateTime(Sql.Convert(Sql.DateTimeOffset, t.DateTimeValue.Year + "-01-01 00:20:00"))));
		}

		[Test]
		public void ToDateTime()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select DateTime.Parse(t.DateTimeValue.Year + "-01-01 00:00:00") where p.Day > 0 select p,
				from p in from t in db.Types select DateTime.Parse(t.DateTimeValue.Year + "-01-01 00:00:00") where p.Day > 0 select p));
		}

		[Test]
		public void ConvertToDateTime()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select Convert.ToDateTime(t.DateTimeValue.Year + "-01-01 00:00:00") where p.Day > 0 select p,
				from p in from t in db.Types select Convert.ToDateTime(t.DateTimeValue.Year + "-01-01 00:00:00") where p.Day > 0 select p));
		}

		[Test]
		public void ToChar()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.Char(20), t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.Char(20), t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToDefaultChar()
		{
			ForEachProvider(new[] { "Oracle", ProviderName.Firebird, ProviderName.PostgreSQL }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DefaultChar, t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.DefaultChar, t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToVarChar()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.VarChar(20), t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.VarChar(20), t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToDefaultVarChar()
		{
			ForEachProvider(new[] { "Oracle", ProviderName.Firebird, ProviderName.PostgreSQL }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DefaultVarChar, t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.DefaultVarChar, t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToNChar()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.NChar(20), t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.NChar(20), t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToDefaultNChar()
		{
			ForEachProvider(new[] { "Oracle", ProviderName.Firebird, ProviderName.PostgreSQL }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DefaultNChar, t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.DefaultNChar, t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToNVarChar()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select Sql.Convert(Sql.NVarChar(20), t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.NVarChar(20), t.MoneyValue).Trim(' ', '0')));
		}

		[Test]
		public void ToDefaultNVarChar()
		{
			ForEachProvider(new[] { "Oracle", ProviderName.Firebird, ProviderName.PostgreSQL }, db => AreEqual(
				from t in    Types select Sql.Convert(Sql.DefaultNVarChar, t.MoneyValue).Trim(' ', '0'),
				from t in db.Types select Sql.Convert(Sql.DefaultNVarChar, t.MoneyValue).Trim(' ', '0')));
		}
	}
}
