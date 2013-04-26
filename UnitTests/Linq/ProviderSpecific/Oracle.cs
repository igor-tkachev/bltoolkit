﻿using System;
using System.Data.Linq.Mapping;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

using UnitTests.Linq.Interface.Model;

namespace Data.Linq.ProviderSpecific
{
	using Model;

    [TestFixture, Category("Oracle")]
	public class Oracle : TestBase
	{
		#region Sequence

		[Test]
		public void SequenceInsert([IncludeDataContexts("Oracle")] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();
				db.Insert(new OracleSpecific.SequenceTest { Value = "SeqValue" });

				var id = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}

		[Test]
		public void SequenceInsertWithIdentity([IncludeDataContexts("Oracle")] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.Value == "SeqValue").Delete();

				var id1 = Convert.ToInt32(db.InsertWithIdentity(new OracleSpecific.SequenceTest { Value = "SeqValue" }));
				var id2 = db.GetTable<OracleSpecific.SequenceTest>().Single(_ => _.Value == "SeqValue").ID;

				Assert.AreEqual(id1, id2);

				db.GetTable<OracleSpecific.SequenceTest>().Where(_ => _.ID == id1).Delete();

				Assert.AreEqual(0, db.GetTable<OracleSpecific.SequenceTest>().Count(_ => _.Value == "SeqValue"));
			}
		}

		#endregion

		#region InsertBatch

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
		public void InsertBatch1([IncludeDataContexts("Oracle")] string context)
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

			using (var db = new TestDbManager(context))
			{
				db.InsertBatch(5, data);
			}
		}

		[Test]
		public void InsertBatch2([IncludeDataContexts("Oracle")] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.Types2.Delete(_ => _.ID > 1000);

				db.InsertBatch(2, new[]
				{
					new LinqDataTypes2 { ID = 1003, MoneyValue = 0m, DateTimeValue = null,         BoolValue = true,  GuidValue = new Guid("ef129165-6ffe-4df9-bb6b-bb16e413c883"), SmallIntValue = null, IntValue = null },
					new LinqDataTypes2 { ID = 1004, MoneyValue = 0m, DateTimeValue = DateTime.Now, BoolValue = false, GuidValue = null,                                             SmallIntValue = 2,    IntValue = 1532334 },
					new LinqDataTypes2 { ID = 1005, MoneyValue = 1m, DateTimeValue = DateTime.Now, BoolValue = false, GuidValue = null,                                             SmallIntValue = 5,    IntValue = null },
					new LinqDataTypes2 { ID = 1006, MoneyValue = 2m, DateTimeValue = DateTime.Now, BoolValue = false, GuidValue = null,                                             SmallIntValue = 6,    IntValue = 153     },
				});

				db.Types2.Delete(_ => _.ID > 1000);
			}
		}

		#endregion

		#region Transaction

		[TableName("demo_product_info")]
		public new class Product
		{
			[MapField("PRODUCT_ID"), PrimaryKey, NonUpdatable]
			public int Id;

			[MapField("PRODUCT_NAME")]
			public string Name;

			[MapField("PRODUCT_DESCRIPTION")]
			public string Description;
		}

		public abstract class ProductEntityAccesor : DataAccessor<Product>
		{
			public abstract int Insert(Product product);
			public abstract void Delete(int id);
			public abstract Product SelectByKey(int id);
			public abstract Product SelectByKey(DbManager db, int id);
		}

		//[Test]
		public void CanInsertProductWithAccessorTest([IncludeDataContexts("Oracle")] string context)
		{
			using (var dbManager = new TestDbManager(context))
			{
				var productEntityAccesor = DataAccessor.CreateInstance<ProductEntityAccesor>(dbManager);

				productEntityAccesor.BeginTransaction();

				var id = productEntityAccesor.Insert(new Product { Name = "product name test", Description = "product description test" });
				//This assert fails bacause id == 0 and it does not insert until the CommitTransaction is called.
				Assert.AreNotEqual(0, id);

				Product product = productEntityAccesor.SelectByKey(id);
				Assert.IsNotNull(product);

				productEntityAccesor.Delete(id);

				productEntityAccesor.CommitTransaction();
			}
		}

		#endregion
	}
}
