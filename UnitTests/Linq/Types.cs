using System;
using System.Data.Linq;
using System.Globalization;
using System.Linq;
using System.Threading;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

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

		[Test]
		public void GuidNew()
		{
			ForEachProvider(db => AreEqual(
				from p in    Types where p.GuidValue != Guid.NewGuid() select p.GuidValue,
				from p in db.Types where p.GuidValue != Guid.NewGuid() select p.GuidValue));
		}

		[Test]
		public void Guid1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Types where p.GuidValue == new Guid("D2F970C0-35AC-4987-9CD5-5BADB1757436") select p.GuidValue,
				from p in db.Types where p.GuidValue == new Guid("D2F970C0-35AC-4987-9CD5-5BADB1757436") select p.GuidValue));
		}

		[Test]
		public void ContainsGuid()
		{
			var ids = new [] { new Guid("D2F970C0-35AC-4987-9CD5-5BADB1757436") };

			ForEachProvider(db => AreEqual(
				from p in    Types where ids.Contains(p.GuidValue) select p.GuidValue,
				from p in db.Types where ids.Contains(p.GuidValue) select p.GuidValue));
		}

		[Test]
		public void UpdateBinary1()
		{
			ForEachProvider(db =>
			{
				db.Types
					.Where(t => t.ID == 1)
					.Set(t => t.BinaryValue, new Binary(new byte[] { 1, 2, 3, 4, 5 }))
					.Update();

				var g = from t in db.Types where t.ID == 1 select t.BinaryValue;

				foreach (var binary in g)
				{
				}
			});
		}

		[Test]
		public void UpdateBinary2()
		{
			ForEachProvider(new[] { ProviderName.SqlCe }, db =>
			{
				var ints     = new[] { 1, 2 };
				var binaries = new[] { new byte[] { 1, 2, 3, 4, 5 }, new byte[] { 5, 4, 3, 2, 1 } };

				for (var i = 1; i <= 2; i++)
				{
					db.Types
						.Where(t => t.ID == ints[i - 1])
						.Set(t => t.BinaryValue, binaries[i - 1])
						.Update();
				}

				var g = from t in db.Types where new[] { 1, 2 }.Contains(t.ID) select t;

				foreach (var binary in g)
					Assert.AreEqual(binaries[binary.ID - 1], binary.BinaryValue.ToArray());
			});
		}

		[Test]
		public void DateTime1()
		{
			var dt = Types2[3].DateTimeValue;

			ForEachProvider(db => AreEqual(
				from t in    Types2 where t.DateTimeValue.Value.Date > dt.Value.Date select t,
				from t in db.Types2 where t.DateTimeValue.Value.Date > dt.Value.Date select t));
		}

		[Test]
		public void Nullable()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Value = p.Value1.GetValueOrDefault() },
				from p in db.Parent select new { Value = p.Value1.GetValueOrDefault() }));
		}

		[Test]
		public void Unicode()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.Firebird, ProviderName.Sybase }, db =>
			{
				try
				{
					db.Person.Delete(p => p.ID > 2);

					var id =
						db.Person
							.InsertWithIdentity(() => new Person
							{
								FirstName = "擊敗奴隸",
								LastName  = "Юникодкин",
								Gender    = Gender.Male
							});

					Assert.NotNull(id);

					var person = db.Person.Single(p => p.FirstName == "擊敗奴隸" && p.LastName == "Юникодкин");

					Assert.NotNull (person);
					Assert.AreEqual(id, person.ID);
					Assert.AreEqual("擊敗奴隸", person.FirstName);
					Assert.AreEqual("Юникодкин", person.LastName);
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void TestCultureInfo()
		{
			var current = Thread.CurrentThread.CurrentCulture;

			Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

			ForEachProvider(db => AreEqual(
				from t in    Types where t.MoneyValue > 0.5m select t,
				from t in db.Types where t.MoneyValue > 0.5m select t));

			Thread.CurrentThread.CurrentCulture = current;
		}
	}
}
