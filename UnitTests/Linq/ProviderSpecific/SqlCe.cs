using System;
using System.Data;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class SqlCe : TestBase
	{
		[TableName("LinqDataTypes")]
		class Test
		{
			public int ID;
			[MapField("DateTimeValue"), DbType(DbType.DateTime2)]
			public DateTime? Data;
		}

		[Test]
		public void DateTime2Test([IncludeDataContexts(ProviderName.SqlCe)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				try
				{
					new SqlQuery<Test>().Insert(db, new Test { ID = 100001, Data = DateTime.Now });
				}
				finally
				{
					db.GetTable<Test>().Delete(t => t.ID > 10000);
				}
			}
		}
	}
}
