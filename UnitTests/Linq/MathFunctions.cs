using System;
using System.Linq;

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

		/*
		[Test]
		public void Cot()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select SqlFunctions.Cot(Math.Cos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select SqlFunctions.Cot(Math.Cos((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}
		*/





		[Test]
		public void Sin()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(Math.Sin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t,
				from t in from p in db.Types select Math.Floor(Math.Sin((double)p.MoneyValue / 15) * 15) where t != 0.1 select t));
		}


		[Test]
		public void Floor()
		{
			ForEachProvider(db => AreSame(
				from t in from p in    Types select Math.Floor(-(p.MoneyValue + 1)) where t != 0 select t,
				from t in from p in db.Types select Math.Floor(-(p.MoneyValue + 1)) where t != 0 select t));
		}
	}
}
