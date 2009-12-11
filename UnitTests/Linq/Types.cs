using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class Types : TestBase
	{
		[Test]
		public void Bool1()
		{
			var value = true;

			ForEachProvider(db => AreEqual(
				from p in    Parent where p.ParentID > 2 && value && true && !false select p,
				from p in db.Parent where p.ParentID > 2 && value && true && !false select p));
		}

		[Test]
		public void Bool2()
		{
			var value = true;

			ForEachProvider(db => AreEqual(
				from p in    Parent where p.ParentID > 2 && value || true && !false select p,
				from p in db.Parent where p.ParentID > 2 && value || true && !false select p));
		}

		[Test]
		public void Bool3()
		{
			var values = new int[0];

			ForEachProvider(db => AreEqual(
				from p in    Parent where values.Contains(p.ParentID) && !false || p.ParentID > 2 select p,
				from p in db.Parent where values.Contains(p.ParentID) && !false || p.ParentID > 2 select p));
		}

		[Test]
		public void BoolField1()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types where t.BoolValue select t.MoneyValue,
				from t in db.Types where t.BoolValue select t.MoneyValue));
		}

		[Test]
		public void BoolField2()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types where !t.BoolValue select t.MoneyValue,
				from t in db.Types where !t.BoolValue select t.MoneyValue));
		}

		[Test]
		public void BoolField3()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types where t.BoolValue == true select t.MoneyValue,
				from t in db.Types where t.BoolValue == true select t.MoneyValue));
		}

		[Test]
		public void BoolField4()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types where t.BoolValue == false select t.MoneyValue,
				from t in db.Types where t.BoolValue == false select t.MoneyValue));
		}

		[Test]
		public void BoolField5()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select new { t.MoneyValue, b = !t.BoolValue } where p.b == false select p.MoneyValue,
				from p in from t in db.Types select new { t.MoneyValue, b = !t.BoolValue } where p.b == false select p.MoneyValue));
		}

		[Test]
		public void BoolField6()
		{
			ForEachProvider(db => AreEqual(
				from p in from t in    Types select new { t.MoneyValue, b = !t.BoolValue } where p.b select p.MoneyValue,
				from p in from t in db.Types select new { t.MoneyValue, b = !t.BoolValue } where p.b select p.MoneyValue));
		}
	}
}
