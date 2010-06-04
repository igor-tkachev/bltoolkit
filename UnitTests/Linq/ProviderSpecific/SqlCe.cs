using System;
using System.Transactions;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class SqlCe : TestBase
	{
		//[Test]
		public void SqlTest()
		{
			using (new TransactionScope())
			{
				using (var db = new TestDbManager(ProviderName.SqlCe))
				{
					var list = db
						.SetCommand(@"
INSERT INTO [Person] 
(
    [FirstName],
    [LastName],
    [Gender]
)
VALUES
(
    'John',
    'Shepard',
    'M'
)")
						.ExecuteScalar();

					list = db
						.SetCommand(@"SELECT @@IDENTITY")
						.ExecuteScalar();
				}
			}
		}
	}
}
