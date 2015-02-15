using System;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Xml.Linq;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Mapping.MemberMappers;
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

		[TableName("TestIdentity")]
		public class Table
		{
			[Identity, MapField("ID")]
			public int    Id;
			public int?   IntValue;
			public string StringValue;
		}

		public class TestObject
		{
			public int Value { get; set; }
		}

		[TableName("TestIdentity")]
		public class Table2
		{
			[Identity, MapField("ID")]
			public int    Id;
			public int?   IntValue;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			[MapField("StringValue"), DbType(DbType.String)]
			public TestObject Object;
		}

		[Test]
		public void TransactionWithIdentity1([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				try
				{
					var list = new[]
					{
						new Table {IntValue = 1111, StringValue = "1111"},
						new Table {IntValue = 2111, StringValue = "2111"},
						new Table {IntValue = 3111, StringValue = "3111"},
						new Table {IntValue = 4111, StringValue = "4111"},
					};

					db.GetTable<Table>().Delete(_ => _.Id > 2);
					var c1 = db.GetTable<Table>().Count();
					
					db.BeginTransaction();
					db.InsertBatch(list);
					db.CommitTransaction();

					var c2 = db.GetTable<Table>().Count();

					Assert.AreEqual(c1+4, c2);
				}
				finally
				{
					db.GetTable<Table>().Delete(_ => _.Id > 2);
				}
			}
		}

		[Test]
		public void NoTransactionWithIdentity1([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				try
				{
					var list = new[]
					{
						new Table {IntValue = 1111, StringValue = "1111"},
						new Table {IntValue = 2111, StringValue = "2111"},
						new Table {IntValue = 3111, StringValue = "3111"},
						new Table {IntValue = 4111, StringValue = "4111"},
					};

					db.GetTable<Table>().Delete(_ => _.Id > 2);
					var c1 = db.GetTable<Table>().Count();
					db.InsertBatch(list);
					var c2 = db.GetTable<Table>().Count();

					Assert.AreEqual(c1+4, c2);
				}
				finally
				{
					db.GetTable<Table>().Delete(_ => _.Id > 2);
				}
			}
		}

		[Test]
		public void TransactionWithIdentity2([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				try
				{
					var list = new[]
					{
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
					};

					db.GetTable<Table2>().Delete(_ => _.IntValue == 1111);
					var c1 = db.GetTable<Table2>().Count();
					
					db.BeginTransaction();
					db.InsertBatch(list);
					db.CommitTransaction();

					var c2 = db.GetTable<Table2>().Count();

					Assert.AreEqual(c1+4, c2);

					var result = db.GetTable<Table2>().Where(_ => _.IntValue == 1111).ToList();
					foreach (var e in result)
					{
						Assert.AreEqual(1111, e.Object.Value);
					}
				}
				finally
				{
					db.GetTable<Table2>().Delete(_ => _.IntValue == 1111);
				}
			}
		}

		[Test]
		public void NoTransactionWithIdentity2([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				try
				{
					var list = new[]
					{
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
						new Table2 {IntValue = 1111, Object = new TestObject{Value = 1111}},
					};

					db.GetTable<Table2>().Delete(_ => _.IntValue == 1111);
					var c1 = db.GetTable<Table2>().Count();
					db.InsertBatch(list);
					var c2 = db.GetTable<Table2>().Count();

					Assert.AreEqual(c1+4, c2);

					var result = db.GetTable<Table2>().Where(_ => _.IntValue == 1111).ToList();
					foreach (var e in result)
					{
						Assert.AreEqual(1111, e.Object.Value);
					}
				}
				finally
				{
					db.GetTable<Table2>().Delete(_ => _.IntValue == 1111);
				}
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
