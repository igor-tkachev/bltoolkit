using System;
using System.Transactions;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class SqlCe : TestBase
	{
		[Test]
		public void SqlTest()
		{
			using (new TransactionScope())
			{
				using (var db = new TestDbManager(ProviderName.SqlCe))
				{
					var list = db
						.SetCommand(@"
							SELECT
								Cast(Floor(Cast([t].[DateTimeValue] as Float)) as DateTime)
							FROM
								[LinqDataTypes] [t]")
						.ExecuteList<DateTime>();
				}
			}
		}
	}
}
