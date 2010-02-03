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
UPDATE
    [Child]
SET
    [ChildID] = [ChildID] + 1
WHERE EXISTS(
FROM
    [Child] [c]
        LEFT JOIN [Parent] [t1] ON [c].[ParentID] = [t1].[ParentID]
WHERE
    [c].[ChildID] = @id AND [t1].[Value1] = 1
")
						.ExecuteNonQuery();
				}
			}
		}
	}
}
