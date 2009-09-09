using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	//[TestFixture]
	public class ConvertTest : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(db => Assert.AreEqual(3, (from t in db.Types where t.MoneyValue * t.ID == 9.99m  select t).Single().ID));
		}
	}
}
