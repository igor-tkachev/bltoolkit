using System;
using System.Data.Linq.Mapping;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.Mapping;
using NUnit.Framework;

using UnitTests.Linq.Interface.Model;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class Oracle : TestBase
	{
		[Test]
		public void SequenceInsert()
		{
			using (var db = new TestDbManager("Oracle"))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new OracleSpecific.SequenceTest { Value = "SeqValue" });

				var id = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsertWithIdentity()
		{
			using (var db = new TestDbManager("Oracle"))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new OracleSpecific.SequenceTest { Value = "SeqValue" }));
				var id2 = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Table(Name = "stg_trade_information")]
		public class Trade
		{
			[MapField("STG_TRADE_ID")]          public int       ID             { get; set; }
			[MapField("STG_TRADE_VERSION")]     public int       Version        { get; set; }
			[MapField("INFORMATION_TYPE_ID")]   public int       TypeID         { get; set; }
			[MapField("INFORMATION_TYPE_NAME")] public string    TypeName       { get; set; }
			[MapField("value")]                 public string    Value          { get; set; }
			[MapField("value_as_integer")]      public int?      ValueAsInteger { get; set; }
			[MapField("value_as_date")]         public DateTime? ValueAsDate    { get; set; }
		}

		[Test]
		public void InsertBatch()
		{
			var data = new[]
			{
				new Trade { ID = 375, Version = 1, TypeID = 20224, TypeName = "Gas Month",     },
				new Trade { ID = 328, Version = 1, TypeID = 20224, TypeName = "Gas Month",     },
				new Trade { ID = 348, Version = 1, TypeID = 20224, TypeName = "Gas Month",     },
				new Trade { ID = 357, Version = 1, TypeID = 20224, TypeName = "Gas Month",     },
				new Trade { ID = 371, Version = 1, TypeID = 20224, TypeName = "Gas Month",     },
				new Trade { ID = 333, Version = 1, TypeID = 20224, TypeName = "Gas Month",     ValueAsInteger = 1,          ValueAsDate = new DateTime(2011, 1, 5) },
				new Trade { ID = 353, Version = 1, TypeID = 20224, TypeName = "Gas Month",     ValueAsInteger = 1000000000,                                        },
				new Trade { ID = 973, Version = 1, TypeID = 20160, TypeName = "EU Allowances", },
			};

			using (var db = new TestDbManager("Oracle"))
			{
				db.InsertBatch(5, data);
			}
		}
	}
}
