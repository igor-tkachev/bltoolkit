using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Xml.Linq;
using BLToolkit;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class Issue369 : TestBase
	{
		[Test]
		public void Test1([IncludeDataContextsAttribute("Sql2012")] string config)
		{
			TestMethod(
				(db, data) =>
					db.SetCommand("insert into person (LastName, FirstName, Gender) values ('Issue369', @FirstName, @Gender)")
						.ExecuteForEach((ICollection) data),
				config);
		}

		[Test]
		public void Test2([IncludeDataContextsAttribute("Sql2012")] string config)
		{
			TestMethod(
				(db, data) => db.InsertBatch(data),
				config);
		}

		[Test]
		public void Test3([IncludeDataContextsAttribute("Sql2012")] string config)
		{
			TestMethod(
				(db, data) =>
					db.SetSpCommand("Person_Insert")
						.ExecuteForEach((ICollection)data),
				config);
		}

		public void TestMethod(Action<DbManager, Person[]> insert, string config)
		{
			using (var db = new TestDbManager(config))
			{
				db.Person.Where(_ => _.LastName == "Issue369").Delete();

				var data = new []
				{
					new Person() {LastName = "Issue369", FirstName = ""},
					new Person() {LastName = "Issue369", FirstName = "my"},
					new Person() {LastName = "Issue369", FirstName = "My first name"},

				};

				insert(db, data);
				var data2 = db.Person.Where(_ => _.LastName == "Issue369").ToArray();

				for (var i = 0; i < data.Length; i ++)
				{
					Assert.AreEqual(data[i].FirstName, data2[i].FirstName);
				}

				db.Person.Where(_ => _.LastName == "Issue369").Delete();
			}

		}

		[TableName("DataTypeTest")]
		public class SmallDataTypeTest
		{
			[Identity]
			public int DataTypeID;
			public byte? Byte_;
			public DateTime? DateTime_;
			public decimal? Decimal_;
			public double? Double_;
			public Guid? Guid_;
			public short? Int16_;
			public int? Int32_;
			public long? Int64_;
			public decimal? Money_;
			public byte? SByte_;
			public float? Single_;
			public string String_;
			public short? UInt16_;
			public int? UInt32_;
			public long? UInt64_;
		}

		[TableName("DataTypeTest")]
		public class SmallDataTypeTest2
		{
			[Identity]
			public int DataTypeID;
			[Nullable]public byte Byte_;
			[Nullable]public DateTime DateTime_;
			[Nullable]public decimal Decimal_;
			[Nullable]public double Double_;
			[Nullable]public Guid Guid_;
			[Nullable]public short Int16_;
			[Nullable]public int Int32_;
			[Nullable]public long Int64_;
			[Nullable]public decimal Money_;
			[Nullable]public byte SByte_;
			[Nullable]public float Single_;
			[Nullable]public string String_;
			[Nullable]public short UInt16_;
			[Nullable]public int UInt32_;
			[Nullable]public long UInt64_;
		}

		[Test]
		public void NullableTest1([DataContextsAttribute(ProviderName.DB2, ProviderName.Firebird,
			ProviderName.Informix, ProviderName.MySql, ProviderName.Sybase, ProviderName.PostgreSQL,
			ExcludeLinqService = true)] string config)
		{
			NullableTest(
				(db, data) =>
				{
					var res = Convert.ToInt32(db.InsertWithIdentity(data));

					db.InsertBatch(new[] {data, data});

					return res;
				},
				config
				);
		}

		[Test]
		public void NullableTest2([DataContextsAttribute(ProviderName.DB2, ProviderName.Firebird,
			ProviderName.Informix, ProviderName.MySql, ProviderName.Sybase, ProviderName.PostgreSQL,
			"Oracle",  ProviderName.OracleManaged,
			ExcludeLinqService = true)]string config)
		{
			NullableTest4(
				(db, data) =>
				{
					new SqlQuery<SmallDataTypeTest2>().Insert(db, data);
					return db.GetTable<SmallDataTypeTest2>().Max(_ => _.DataTypeID);
				},
				config);
		}

		[Test]
		public void NullableTest1_2([DataContextsAttribute(ProviderName.DB2, ProviderName.Firebird,
			ProviderName.Informix, ProviderName.MySql, ProviderName.Sybase, ProviderName.PostgreSQL, ProviderName.Access,
			ExcludeLinqService = true)] string config)
		{
			NullableTest4(
				(db, data) =>
				{
					var res = Convert.ToInt32(db.InsertWithIdentity(data));

					db.InsertBatch(new[] { data, data });

					return res;
				},
				config
				);
		}

		[Test]
		public void NullableTest2_2([DataContextsAttribute(ProviderName.DB2, ProviderName.Firebird,
			ProviderName.Informix, ProviderName.MySql, ProviderName.Sybase, ProviderName.PostgreSQL,
			"Oracle",  ProviderName.OracleManaged,
			ExcludeLinqService = true)] string config)
		{
			NullableTest(
				(db, data) =>
				{
					new SqlQuery<SmallDataTypeTest>().Insert(db, data);
					return db.GetTable<SmallDataTypeTest>().Max(_ => _.DataTypeID);
				},
				config);
		}


		[Test]
		public void NullableTest3([IncludeDataContexts("Sql2012")] string config)
		{
			NullableTest2(
				(db, data) =>
				{
					db.SetSpCommand("DataTypeTest_Insert", db.CreateParameters(data))
						.ExecuteNonQuery();
					var id = db.GetTable<SmallDataTypeTest>().Max(_ => _.DataTypeID);

					db.SetSpCommand("DataTypeTest_Insert")
						.ExecuteForEach((ICollection) new[] {data, data});

					return id;
				},
				config);
		}

		[Test]
		public void NullableTest3_2([IncludeDataContexts("Sql2012")] string config)
		{
			NullableTest3(
				(db, data) =>
				{
					db.SetSpCommand("DataTypeTest_Insert", db.CreateParameters(data))
						.ExecuteNonQuery();
					var id = db.GetTable<SmallDataTypeTest>().Max(_ => _.DataTypeID);

					db.SetSpCommand("DataTypeTest_Insert")
						.ExecuteForEach((ICollection) new[] {data, data});

					return id;
				},
				config);
		}


		public void NullableTest2(Func<DbManager, DataTypeTest, int> insert, string config)
		{
			using (var db = new TestDbManager(config))
			{

				var data = new DataTypeTest()
				{
					Byte_ = 0,
					Int16_ = 0,
					Int32_ = 0,
					Int64_ = 0,
					UInt16_ = 0,
					UInt32_ = 0,
					UInt64_ = 0,
					Decimal_ = 0,
					Double_ = 0,
					Money_ = 0,
					Single_ = 0,
					SByte_ = 0,
				};

				var id = insert(db, data);

				var list = db.GetTable<DataTypeTest>().Where(_ => _.DataTypeID >= id).ToList();
				var i = 0;
				foreach (var d2 in list)
				{
					Console.WriteLine("Iteration: " + i++);
					Assert.IsNotNull(d2.Byte_);
					Assert.IsNotNull(d2.Decimal_);
					Assert.IsNotNull(d2.Double_);
					Assert.IsNotNull(d2.Int16_);
					Assert.IsNotNull(d2.Int32_);
					Assert.IsNotNull(d2.Int64_);
					Assert.IsNotNull(d2.Money_);
					Assert.IsNotNull(d2.Single_);

					Assert.IsNotNull(d2.SByte_);
					Assert.IsNotNull(d2.UInt16_);
					Assert.IsNotNull(d2.UInt32_);
					Assert.IsNotNull(d2.UInt64_);

				}
				db.GetTable<SmallDataTypeTest>().Delete(_ => _.DataTypeID > 2);
			}
		}

		public void NullableTest3(Func<DbManager, DataTypeTest2, int> insert, string config)
		{
			using (var db = new TestDbManager(config))
			{

				var data = new DataTypeTest2()
				{
					Byte_ = 0,
					Int16_ = 0,
					Int32_ = 0,
					Int64_ = 0,
					UInt16_ = 0,
					UInt32_ = 0,
					UInt64_ = 0,
					Decimal_ = 0,
					Double_ = 0,
					Money_ = 0,
					Single_ = 0,
					SByte_ = 0,
				};

				var id = insert(db, data);

				var list = db.GetTable<DataTypeTest>().Where(_ => _.DataTypeID >= id).ToList();
				var i = 0;
				foreach (var d2 in list)
				{
					Console.WriteLine("Iteration: " + i++);
					Assert.IsNull(d2.Byte_);
					Assert.IsNull(d2.Decimal_);
					Assert.IsNull(d2.Double_);
					Assert.IsNull(d2.Int16_);
					Assert.IsNull(d2.Int32_);
					Assert.IsNull(d2.Int64_);
					Assert.IsNull(d2.Money_);
					Assert.IsNull(d2.Single_);

					Assert.IsNull(d2.SByte_);
					Assert.IsNull(d2.UInt16_);
					Assert.IsNull(d2.UInt32_);
					Assert.IsNull(d2.UInt64_);

				}
				db.GetTable<SmallDataTypeTest>().Delete(_ => _.DataTypeID > 2);
			}
		}

		public void NullableTest(Func<DbManager, SmallDataTypeTest, int> insert, string config)
		{
			using (var db = new TestDbManager(config))
			{

				var data = new SmallDataTypeTest()
				{
					Byte_ = 0,
					Int16_ = 0,
					Int32_ = 0,
					Int64_ = 0,
					UInt16_ = 0,
					UInt32_ = 0,
					UInt64_ = 0,
					Decimal_ = 0,
					Double_ = 0,
					Money_ = 0,
					Single_ = 0,
					SByte_ = 0,
				};

				var id = insert(db, data);

				var list = db.GetTable<SmallDataTypeTest>().Where(_ => _.DataTypeID >= id).ToList();
				var i = 0;
				foreach (var d2 in list)
				{
					Console.WriteLine("Iteration: " + i++);
					Assert.IsNotNull(d2.Byte_);
					Assert.IsNotNull(d2.Decimal_);
					Assert.IsNotNull(d2.Double_);
					Assert.IsNotNull(d2.Int16_);
					Assert.IsNotNull(d2.Int32_);
					Assert.IsNotNull(d2.Int64_);
					Assert.IsNotNull(d2.Money_);
					Assert.IsNotNull(d2.Single_);

					Assert.IsNotNull(d2.SByte_);
					Assert.IsNotNull(d2.UInt16_);
					Assert.IsNotNull(d2.UInt32_);
					Assert.IsNotNull(d2.UInt64_);

				}
				db.GetTable<SmallDataTypeTest>().Delete(_ => _.DataTypeID > 2);
			}
		}
		public void NullableTest4(Func<DbManager, SmallDataTypeTest2, int> insert, string config)
		{
			using (var db = new TestDbManager(config))
			{

				var data = new SmallDataTypeTest2()
				{
					Byte_ = 0,
					Int16_ = 0,
					Int32_ = 0,
					Int64_ = 0,
					UInt16_ = 0,
					UInt32_ = 0,
					UInt64_ = 0,
					Decimal_ = 0,
					Double_ = 0,
					Money_ = 0,
					Single_ = 0,
					SByte_ = 0,
				};

				var id = insert(db, data);

				var list = db.GetTable<SmallDataTypeTest>().Where(_ => _.DataTypeID >= id).ToList();
				var i = 0;
				foreach (var d2 in list)
				{
					Console.WriteLine("Iteration: " + i++);
					Assert.IsNull(d2.Byte_);
					Assert.IsNull(d2.Decimal_);
					Assert.IsNull(d2.Double_);
					Assert.IsNull(d2.Int16_);
					Assert.IsNull(d2.Int32_);
					Assert.IsNull(d2.Int64_);
					Assert.IsNull(d2.Money_);
					Assert.IsNull(d2.Single_);

					Assert.IsNull(d2.SByte_);
					Assert.IsNull(d2.UInt16_);
					Assert.IsNull(d2.UInt32_);
					Assert.IsNull(d2.UInt64_);

				}
				db.GetTable<SmallDataTypeTest>().Delete(_ => _.DataTypeID > 2);
			}
		}
	}
}
