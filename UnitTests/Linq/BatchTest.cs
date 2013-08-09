using System;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using Data.Linq;
using Data.Linq.Model;

using NUnit.Framework;

namespace Update
{
	[TestFixture]
	public class BatchTest : TestBase
	{
		[Test]
		public void Transaction([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				var list = new[]
				{
					new Parent { ParentID = 1111, Value1 = 1111 },
					new Parent { ParentID = 2111, Value1 = 2111 },
					new Parent { ParentID = 3111, Value1 = 3111 },
					new Parent { ParentID = 4111, Value1 = 4111 },
				};

				foreach (var parent in list)
					db.Parent.Delete(p => p.ParentID == parent.ParentID);

				db.BeginTransaction();
				db.InsertBatch(list);
				db.CommitTransaction();

				foreach (var parent in list)
					db.Parent.Delete(p => p.ParentID == parent.ParentID);
			}
		}

		[Test]
		public void NoTransaction([DataContexts(ExcludeLinqService=true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				var list = new[]
				{
					new Parent { ParentID = 1111, Value1 = 1111 },
					new Parent { ParentID = 2111, Value1 = 2111 },
					new Parent { ParentID = 3111, Value1 = 3111 },
					new Parent { ParentID = 4111, Value1 = 4111 },
				};

				foreach (var parent in list)
					db.Parent.Delete(p => p.ParentID == parent.ParentID);

				db.InsertBatch(list);

				foreach (var parent in list)
					db.Parent.Delete(p => p.ParentID == parent.ParentID);
			}
		}

		[TableName(Database="KanoonIr", Name="Area")]
		public class Area
		{
			[          PrimaryKey(1)] public int    AreaCode  { get; set; }
			                          public string AreaName  { get; set; }
			                          public int    StateCode { get; set; }
			[          PrimaryKey(2)] public int    CityCode  { get; set; }
			                          public string Address   { get; set; }
			                          public string Tels      { get; set; }
			[Nullable               ] public string WebSite   { get; set; }
			                          public bool   IsActive  { get; set; }
		}

		[Test, ExpectedException(typeof(InvalidOperationException), ExpectedMessage="Cannot access destination table '[KanoonIr]..[Area]'.")]
		public void Issue260([IncludeDataContexts("Sql2005")] string context)
		{
			using (var db = GetDataContext(context))
			{
				((DbManager)db).InsertBatch(new[] { new Area { AreaCode = 1 } });
			}
		}
	}
}
