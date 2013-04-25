using System;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

using UnitTests.Linq.Interface.Model;

namespace Data.Linq.ProviderSpecific
{
    [TestFixture, Category("PostgreSQL")]
	public class PostgreSQL : TestBase
	{
		[TableName(Owner="public", Name="entity")]
		public class Entity
		{
			[MapField("the_name") ] public string TheName { get; set; }
		}

		[Test]
		public void SqlTest1([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.BeginTransaction();

				db
					.SetSpCommand("add_if_not_exists", db.Parameter("p_name", "one"))
					.ExecuteNonQuery();

				db.Insert(new Entity { TheName = "two" });
			}
		}

		[Test]
		public void SequenceInsert1([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest1>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new PostgreSQLSpecific.SequenceTest1 { Value = "SeqValue" });

				var id = db.GetTable<PostgreSQLSpecific.SequenceTest1>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<PostgreSQLSpecific.SequenceTest1>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest1>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsert2([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest2>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new PostgreSQLSpecific.SequenceTest2 { Value = "SeqValue" });

				var id = db.GetTable<PostgreSQLSpecific.SequenceTest2>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<PostgreSQLSpecific.SequenceTest2>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest2>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsert3([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest3>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new PostgreSQLSpecific.SequenceTest3 { Value = "SeqValue" });

				var id = db.GetTable<PostgreSQLSpecific.SequenceTest3>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<PostgreSQLSpecific.SequenceTest3>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest3>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsertWithIdentity1([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest1>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new PostgreSQLSpecific.SequenceTest1 { Value = "SeqValue" }));
				var id2 = db.GetTable<PostgreSQLSpecific.SequenceTest1>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<PostgreSQLSpecific.SequenceTest1>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest1>().Count(_ => _.Value == "SeqValue"));
			}
		}

			[Test]
		public void SequenceInsertWithIdentity2([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest2>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new PostgreSQLSpecific.SequenceTest2 { Value = "SeqValue" }));
				var id2 = db.GetTable<PostgreSQLSpecific.SequenceTest2>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<PostgreSQLSpecific.SequenceTest2>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest2>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsertWithIdentity3([IncludeDataContexts(ProviderName.PostgreSQL)] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<PostgreSQLSpecific.SequenceTest3>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new PostgreSQLSpecific.SequenceTest3 { Value = "SeqValue" }));
				var id2 = db.GetTable<PostgreSQLSpecific.SequenceTest3>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<PostgreSQLSpecific.SequenceTest3>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<PostgreSQLSpecific.SequenceTest3>().Count(_ => _.Value == "SeqValue"));
			}
		}
	}
}
