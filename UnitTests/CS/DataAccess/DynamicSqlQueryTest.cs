using System;
using System.Runtime.CompilerServices;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace DataAccess
{
	[TestFixture]
	public class DynamicSqlQueryTest
	{
		public abstract class DynamicSqlQueryAccessor : DataAccessor
		{
			class DynamicSqlQueryAttribute : SqlQueryAttribute
			{
				public override string GetSqlText(DataAccessor accessor, DbManager dbManager)
				{
					return SqlText + 1;
				}
			}

			[DynamicSqlQuery(SqlText="SELECT ", IsDynamic=true)]
			public abstract int GetID1();

			[SqlQuery("SELECT ", ID = 2)]
			public abstract int GetID2();

			protected override string PrepareSqlQuery(
				DbManager db, int queryID, int uniqueQueryID, string sqlQuery)
			{
				switch (queryID)
				{
					case 2: return sqlQuery + queryID;
				}

				return base.PrepareSqlQuery(db, queryID, uniqueQueryID, sqlQuery);
			}
		}

		[Test]
		public void DynamicQueryTest()
		{
			DynamicSqlQueryAccessor da = TypeAccessor<DynamicSqlQueryAccessor>.CreateInstance();
			Assert.AreEqual(da.GetID1(), 1);
		}

		[Test]
		public void QueryIDTest()
		{
			DynamicSqlQueryAccessor da = TypeAccessor<DynamicSqlQueryAccessor>.CreateInstance();
			Assert.AreEqual(da.GetID2(), 2);
		}
	}
}
