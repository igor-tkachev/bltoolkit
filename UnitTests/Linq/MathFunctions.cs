using System;
using System.Linq;
using BLToolkit.Data.Linq;
using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	[TestFixture]
	public class MathFunctions : TestBase
	{
		[Test]
		public void Abs()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Abs(p.MoneyValue) where t > 0 select t,
				from t in from p in db.Types select Math.Abs(p.MoneyValue) where t > 0 select t));
		}

		[Test]
		public void Acos()
		{
			ForEachProvider(new[] { ProviderName.Access }, db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Acos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Acos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Asin()
		{
			ForEachProvider(new[] { ProviderName.Access }, db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Asin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Asin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Atan()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Atan((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Atan((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Atan2()
		{
			ForEachProvider(new[] { ProviderName.Access }, db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Atan2((double)p.MoneyValue / 15, 0) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Atan2((double)p.MoneyValue / 15, 0) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Ceiling1()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Ceiling(-(p.MoneyValue + 1)) where t != 0 select t,
				from t in from p in db.Types select Math.Ceiling(-(p.MoneyValue + 1)) where t != 0 select t));
		}

		[Test]
		public void Ceiling2()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Ceiling(p.MoneyValue) where t != 0 select t,
				from t in from p in db.Types select Math.Ceiling(p.MoneyValue) where t != 0 select t));
		}

		[Test]
		public void Cos()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Cos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Cos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Cosh()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Cosh((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Cosh((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}

		[Test]
		public void Cot()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Sql.Cot((double)p.MoneyValue / 15).Value * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Sql.Cot((double)p.MoneyValue / 15).Value * 15) where t != 0.1 select t));
		}

		[Test]
		public void Deegrees1()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Sql.Degrees(p.MoneyValue).Value) where t != 0.1m select t,
				from t in from p in db.Types select Math.Floor(Sql.Degrees(p.MoneyValue).Value) where t != 0.1m select t));
		}

		[Test]
		public void Deegrees2()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Sql.Degrees((double)p.MoneyValue).Value where t != 0.1 select Math.Floor(t),
				from t in from p in db.Types select Sql.Degrees((double)p.MoneyValue).Value where t != 0.1 select Math.Floor(t)));
		}

		[Test]
		public void Deegrees3()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Sql.Degrees((int)p.MoneyValue).Value where t != 0.1 select t,
				from t in from p in db.Types select Sql.Degrees((int)p.MoneyValue).Value where t != 0.1 select t));
		}

		[Test]
		public void Exp()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Exp((double)p.MoneyValue)) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Exp((double)p.MoneyValue)) where t != 0.1 select t));
		}

		[Test]
		public void Floor()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(-(p.MoneyValue + 1)) where t != 0 select t,
				from t in from p in db.Types select Math.Floor(-(p.MoneyValue + 1)) where t != 0 select t));
		}

		[Test]
		public void Log()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Log((double)p.MoneyValue)) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Log((double)p.MoneyValue)) where t != 0.1 select t));
		}

		[Test]
		public void Log2()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Log((double)p.MoneyValue, 2)) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Log((double)p.MoneyValue, 2)) where t != 0.1 select t));
		}

		[Test]
		public void Log10()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Log10((double)p.MoneyValue)) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Log10((double)p.MoneyValue)) where t != 0.1 select t));
		}

		[Test]
		public void Max()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Max(p.MoneyValue, 5) where t != 0 select t,
				from t in from p in db.Types select Math.Max(p.MoneyValue, 5) where t != 0 select t));
		}

		[Test]
		public void Min()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Min(p.MoneyValue, 5) where t != 0 select t,
				from t in from p in db.Types select Math.Min(p.MoneyValue, 5) where t != 0 select t));
		}

		[Test]
		public void Pow()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Pow((double)p.MoneyValue, 3)) where t != 0 select t,
				from t in from p in db.Types select Math.Floor(Math.Pow((double)p.MoneyValue, 3)) where t != 0 select t));
		}

		[Test]
		public void Sin()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Sin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Sin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}
	}
}
