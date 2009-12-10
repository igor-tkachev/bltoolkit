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
		public void Test2()
		{
			ForEachProvider(db => AreEqual(
				from t in    Types select              Sql.Convert<int,decimal>(t.MoneyValue),
				from t in db.Types select Sql.OnServer(Sql.Convert<int,decimal>(t.MoneyValue))));
		}
	}
}
