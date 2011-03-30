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
SELECT
    Max([t1].[ChildID]) as [c1],
    Count([t2].[ParentID]) as [c2],
    Count([t3].[ParentID1]) as [c3],
    Count([t4].[ParentID2]) as [c4]
FROM
    [Child] [t11]
        outer apply (
            SELECT
                [keyParam].[ParentID] as [ParentID3],
                [keyParam].[ChildID]
            FROM
                [Child] [keyParam]
            GROUP BY
                [keyParam].[ParentID]
        ) t1
        outer apply (
            SELECT
                [keyParam1].[ParentID]
            FROM
                [Child] [keyParam1]
            WHERE
                [keyParam1].[ChildID] > 20
            GROUP BY
                [keyParam1].[ParentID]
        ) [t2]
        outer apply (
            SELECT
                [keyParam2].[ParentID] as [ParentID1]
            FROM
                [Child] [keyParam2]
            WHERE
                [keyParam2].[ChildID] > 20
            GROUP BY
                [keyParam2].[ParentID]
        ) [t3] 
        outer apply (
            SELECT
                [keyParam3].[ParentID] as [ParentID2]
            FROM
                [Child] [keyParam3]
            WHERE
                [keyParam3].[ChildID] > 10
            GROUP BY
                [keyParam3].[ParentID]
        ) [t4] 
WHERE
[t11].[ParentID] = [t1].[ParentID3]
and  [t11].[ParentID] = [t2].[ParentID]
and [t11].[ParentID] = [t4].[ParentID2]
and [t11].[ParentID] = [t3].[ParentID1]
GROUP BY
    [t11].[ParentID]
")
						.ExecuteScalar();

					list = db
						.SetCommand(@"SELECT @@IDENTITY")
						.ExecuteScalar();
				}
			}
		}
	}
}
