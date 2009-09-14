using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
using NUnit.Framework;

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
	}
}
