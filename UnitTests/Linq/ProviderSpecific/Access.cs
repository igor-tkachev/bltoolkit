using System;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class Access : TestBase
	{
		[Test]
		public void SqlTest()
		{
			using (var db = new TestDbManager(ProviderName.Access))
			{
				var list = db
					.SetCommand(@"
						SELECT
							DateValue([t].[DateTimeValue])
						FROM
							[LinqDataTypes] [t]")
					.ExecuteScalarList<DateTime>();
			}
		}
	}
}
