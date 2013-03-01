using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Fluent.Test.MockDataBase;
using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using BLToolkit.Reflection.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BLToolkit.Fluent.Test
{
	/// <summary>
	/// Тестирование FluentMap
	/// </summary>
	[TestClass]
	public class FluentMapTest
	{
		[TestInitialize]
		public void Initialize()
		{
			DbManager.AddDataProvider(typeof(MockDataProvider));
		}

		/// <summary>
		/// TableName mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapTableName()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<TableNameDbo>()
					.TableName("TableNameDboT1")
					.MapTo(db);

				// when
				db.GetTable<TableNameDbo>().ToArray();

				// then
				conn.Commands[0]
					.Assert().AreTable("TableNameDboT1", "Fail mapping");
			}
		}

		/// <summary>
		/// TableName mapping fro child
		/// </summary>
		[TestMethod]
		public void ShouldMapTableNameForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<TableNameDbo>()
					.TableName("TableNameDboT1")
					.MapTo(db);

				// when
				db.GetTable<TableNameChildDbo>().ToArray();

				// then
				conn.Commands[0]
					.Assert().AreTable("TableNameDboT1", "Fail mapping");
			}
		}

		/// <summary>
		/// MapField mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapField()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapFieldDbo>()
					.MapField(_ => _.Field1, "f1")
					.MapTo(db);

				// when
				db.GetTable<MapFieldDbo>().Insert(() => new MapFieldDbo { Field1 = 1 });

				// then
				conn.Commands[0]
					.Assert().AreField("f1", "Fail mapping");
			}
		}

		/// <summary>
		/// MapField mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapFieldforChild()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapFieldDbo>()
					.MapField(_ => _.Field1, "f1")
					.MapTo(db);

				// when
				db.GetTable<MapFieldChild1Dbo>().Insert(() => new MapFieldChild1Dbo { Field1 = 1 });

				// then
				conn.Commands[0]
					.Assert().AreField("f1", "Fail mapping");
			}
		}

		/// <summary>
		/// MapField mapping for child override
		/// </summary>
		[TestMethod]
		public void ShouldMapFieldforChildOverride()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapFieldDbo>()
					.MapField(_ => _.Field1, "f1")
					.MapTo(db);
				new FluentMap<MapFieldChild2Dbo>()
					.MapField(_ => _.Field1, "fc1")
					.MapTo(db);

				// when
				db.GetTable<MapFieldChild2Dbo>().Insert(() => new MapFieldChild2Dbo { Field1 = 1 });

				// then
				conn.Commands[0]
					.Assert().AreField("fc1", "Fail mapping");
			}
		}

		/// <summary>
		/// PrimaryKey mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapPrimaryKey()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(1, 2);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<PrimaryKeyDbo>()
					.PrimaryKey(_ => _.Field2)
					.MapTo(db);

				// when
				AssertExceptionEx.AreNotException<Exception>(() => new SqlQuery<PrimaryKeyDbo>(db).SelectByKey(1)
					, "Fail query");

				// then
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
				conn.Commands[0]
					.Assert().AreField("Field2", 2, "Not where part");
			}
		}

		/// <summary>
		/// PrimaryKey mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapPrimaryKeyForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(1, 2);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<PrimaryKeyDbo>()
					.PrimaryKey(_ => _.Field2)
					.MapTo(db);

				// when
				AssertExceptionEx.AreNotException<Exception>(() => new SqlQuery<PrimaryKeyChildDbo>(db).SelectByKey(1)
					, "Fail query");

				// then
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
				conn.Commands[0]
					.Assert().AreField("Field2", 2, "Not where part");
			}
		}

		/// <summary>
		/// NonUpdatable use
		/// </summary>
		[TestMethod]
		public void ShouldMapNonUpdatable()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<NonUpdatableDbo>()
					.NonUpdatable(_ => _.Field1)
					.MapTo(db);

				// when
				new SqlQuery<NonUpdatableDbo>(db).Insert(new NonUpdatableDbo { Field1 = 10, Field2 = 1 });

				// then
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
			}
		}

		/// <summary>
		/// NonUpdatable use for child
		/// </summary>
		[TestMethod]
		public void ShouldMapNonUpdatableForChild()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<NonUpdatableDbo>()
					.NonUpdatable(_ => _.Field1)
					.MapTo(db);

				// when
				new SqlQuery<NonUpdatableChildDbo>(db).Insert(new NonUpdatableChildDbo { Field1 = 10, Field2 = 1 });

				// then
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
			}
		}

		/// <summary>
		/// SqlIgnore mapping on insert
		/// </summary>
		[TestMethod]
		public void ShouldMapSqlIgnoreInsert()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<SqlIgnoreInsertDbo>()
					.SqlIgnore(_ => _.Field2)
					.MapTo(db);

				// when / then
				new SqlQuery<SqlIgnoreInsertDbo>(db).Insert(new SqlIgnoreInsertDbo { Field1 = 20, Field2 = 2 });
				AssertExceptionEx.AreException<LinqException>(
					() => db.GetTable<SqlIgnoreInsertDbo>().Insert(() => new SqlIgnoreInsertDbo { Field1 = 10, Field2 = 1 })
					, "Fail for linq");

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
			}
		}

		/// <summary>
		/// SqlIgnore mapping on select
		/// </summary>
		[TestMethod]
		public void ShouldMapSqlIgnoreSelect()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(10);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<SqlIgnoreSelectDbo>()
					.SqlIgnore(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<SqlIgnoreSelectDbo>();

				// when
				(from t in table where t.Field1 == 10 select t).First();

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
			}
		}

		/// <summary>
		/// SqlIgnore mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapSqlIgnoreForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(10);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<SqlIgnoreSelectDbo>()
					.SqlIgnore(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<SqlIgnoreChildDbo>();

				// when
				(from t in table where t.Field1 == 10 select t).First();

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
			}
		}

		/// <summary>
		/// MapIgnore mapping on insert
		/// </summary>
		[TestMethod]
		public void ShouldMapIgnoreInsert()
		{
			// db config
			var conn = new MockDb()
				.NewNonQuery();

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapIgnoreInsertDbo>()
					.MapIgnore(_ => _.Field2)
					.MapTo(db);

				// when / then
				new SqlQuery<MapIgnoreInsertDbo>(db).Insert(new MapIgnoreInsertDbo { Field1 = 20, Field2 = 2 });

				AssertExceptionEx.AreException<LinqException>(
					() => db.GetTable<MapIgnoreInsertDbo>().Insert(() => new MapIgnoreInsertDbo { Field1 = 10, Field2 = 1 })
					, "Fail for linq");

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
				Assert.AreEqual(1, conn.Commands[0].Parameters.Count, "Fail params");
			}
		}

		/// <summary>
		/// MapIgnore mapping on select
		/// </summary>
		[TestMethod]
		public void ShouldMapIgnoreSelect()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(10, 1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapIgnoreSelectDbo>()
					.MapIgnore(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<MapIgnoreSelectDbo>();

				// when
				(from t in table where t.Field1 == 10 select t).First();

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
			}
		}

		/// <summary>
		/// MapIgnore mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapIgnoreForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow(10, 1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<MapIgnoreSelectDbo>()
					.MapIgnore(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<MapIgnoreChildDbo>();

				// when
				(from t in table where t.Field1 == 10 select t).First();

				// then
				conn.Commands[0]
					.Assert().AreNotField("Field2", "Field exists");
			}
		}

		/// <summary>
		/// Trimmable mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapTrimmable()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow("test     ");

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<TrimmableDbo>()
					.Trimmable(_ => _.Field1)
					.MapTo(db);

				var table = db.GetTable<TrimmableDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual("test", dbo.Field1, "Not trimmable");
			}
		}

		/// <summary>
		/// Trimmable mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapTrimmableForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1")
					.NewRow("test     ");

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<TrimmableDbo>()
					.Trimmable(_ => _.Field1)
					.MapTo(db);

				var table = db.GetTable<TrimmableChildDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual("test", dbo.Field1, "Not trimmable");
			}
		}

		/// <summary>
		/// MapValue mapping for member
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForMember()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(true, false);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueMemberDbo>()
					.MapField(_ => _.Field1)
						.MapValue("result true", true)
						.MapValue("result false", false)
					.MapField(_ => _.Field2)
						.MapValue("value true", true)
						.MapValue("value false", false)
					.MapTo(db);

				var table = db.GetTable<MapValueMemberDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual("result true", dbo.Field1, "Not map from value1");
				Assert.AreEqual("value false", dbo.Field2, "Not map from value2");
			}
		}

		/// <summary>
		/// MapValue mapping for member for child
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForMemberForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(true, false);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueMemberDbo>()
					.MapField(_ => _.Field1)
						.MapValue("result true", true)
						.MapValue("result false", false)
					.MapField(_ => _.Field2)
						.MapValue("value true", true)
						.MapValue("value false", false)
					.MapTo(db);

				var table = db.GetTable<MapValueMemberChildDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual("result true", dbo.Field1, "Not map from value1");
				Assert.AreEqual("value false", dbo.Field2, "Not map from value2");
			}
		}

		/// <summary>
		/// MapValue mapping for enum
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForEnum()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2", "Field3", "Field4")
					.NewRow("ok", "super", "yes", 10);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueEnumDbo>()
					.MapValue(MapValueEnum.Value1, "ok", "yes")
					.MapValue(MapValueEnum.Value2, "super")
					.MapTo(db);

				var table = db.GetTable<MapValueEnumDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual(MapValueEnum.Value1, dbo.Field1, "Not map from value1");
				Assert.AreEqual(MapValueEnum.Value2, dbo.Field2, "Not map from value2");
				Assert.AreEqual(MapValueEnum.Value1, dbo.Field3, "Not map from value3");
			}
		}

		/// <summary>
		/// MapValue mapping for enum for child
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForEnumForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2", "Field3", "Field4")
					.NewRow("ok", "super", "yes", 10);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueEnumDbo>()
					.MapValue(MapValueEnum.Value1, "ok", "yes")
					.MapValue(MapValueEnum.Value2, "super")
					.MapTo(db);

				var table = db.GetTable<MapValueEnumChildDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual(MapValueEnum.Value1, dbo.Field1, "Not map from value1");
				Assert.AreEqual(MapValueEnum.Value2, dbo.Field2, "Not map from value2");
				Assert.AreEqual(MapValueEnum.Value1, dbo.Field3, "Not map from value3");
			}
		}

		/// <summary>
		/// MapValue mapping for type
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForType()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2", "Field3")
					.NewRow("one", "two", true);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for property any different types
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueTypeDbo>()
					.MapValue(1, "one", "1")
					.MapValue(2, "two")
					.MapValue(3, true)
					.MapTo(db);

				var table = db.GetTable<MapValueTypeDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual(1, dbo.Field1, "Not map from value1");
				Assert.AreEqual(2, dbo.Field2, "Not map from value2");
				Assert.AreEqual(3, dbo.Field3, "Not map from value3");
			}
		}

		/// <summary>
		/// MapValue mapping for type for child
		/// </summary>
		[TestMethod]
		public void ShouldMapValueForTypeForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2", "Field3")
					.NewRow("one", "two", true);

			using (conn)
			using (var db = new DbManager(conn))
			{
#warning bug for property any different types
#warning bug for maping TO db
				// fluent config
				new FluentMap<MapValueTypeDbo>()
					.MapValue(1, "one", "1")
					.MapValue(2, "two")
					.MapValue(3, true)
					.MapTo(db);

				var table = db.GetTable<MapValueTypeChildDbo>();

				// when
				var dbo = (from t in table select t).First();

				// then
				Assert.AreEqual(1, dbo.Field1, "Not map from value1");
				Assert.AreEqual(2, dbo.Field2, "Not map from value2");
				Assert.AreEqual(3, dbo.Field3, "Not map from value3");
			}
		}

		/// <summary>
		/// InheritanceMapping mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapInheritanceMapping()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(1, 1)
					.NewRow(2, 2);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<InheritanceMappingDbo>()
					.InheritanceMapping<InheritanceMappingDbo1>(1)
					.InheritanceMapping<InheritanceMappingDbo2>(2)
					.InheritanceField(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<InheritanceMappingDbo>();

				// when
				var dbos = (from t in table select t).ToArray();

				// then
				Assert.IsInstanceOfType(dbos[0], typeof(InheritanceMappingDbo1), "Invalid type1");
				Assert.IsInstanceOfType(dbos[1], typeof(InheritanceMappingDbo2), "Invalid type2");
			}
		}

		/// <summary>
		/// InheritanceMapping mapping select only child
		/// </summary>
		[TestMethod]
		public void ShouldMapInheritanceMappingSelectChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field1", "Field2")
					.NewRow(1, 1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<InheritanceMappingChDbo>()
					.TableName("tt")
					.InheritanceMapping<InheritanceMappingChDbo1>(11111)
					.InheritanceMapping<InheritanceMappingChDbo2>(22222)
					.InheritanceField(_ => _.Field2)
					.MapTo(db);

				var table = db.GetTable<InheritanceMappingChDbo1>();

				// when
				var dbos = (from t in table select t).ToArray();

				// then
				Assert.IsInstanceOfType(dbos[0], typeof(InheritanceMappingChDbo1), "Invalid type");
				Assert.IsTrue(conn.Commands[0].CommandText.ToLower().Contains("where"), "Not condition");
				Assert.IsTrue(conn.Commands[0].CommandText.ToLower().Contains("11111"), "Fail condition value");
				conn.Commands[0]
					.Assert().AreField("Field2", 2, "Not in where part");
			}
		}

		/// <summary>
		/// Association mapping to many
		/// </summary>
		[TestMethod]
		public void ShouldMapAssociationToMany()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field4")
					.NewRow("TestMany");

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<AssociationThis1Dbo>()
					.MapField(_ => _.FieldThis1, "ThisId")
					.MapField(_ => _.FieldThis2)
						.Association(_ => _.FieldThis1).ToMany((AssociationOtherDbo _) => _.FieldOther2)
					.MapTo(db);

				var table = db.GetTable<AssociationThis1Dbo>();

				// when
				var dbo = (from t in table select t.FieldThis2.First().FieldOther4).First();

				// then
				Assert.AreEqual("TestMany", dbo, "Fail result for many");
				conn.Commands[0]
					.Assert().AreField("ThisId", "Fail this key");
				conn.Commands[0]
					.Assert().AreField("FieldOther2", "Fail other key");
				conn.Commands[0]
					.Assert().AreField("FieldOther4", "Fail other result");
				Assert.AreEqual(3, conn.Commands[0].Fields.Count, "More fields");
			}
		}

		/// <summary>
		/// Association mapping to one
		/// </summary>
		[TestMethod]
		public void ShouldMapAssociationToOne()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field4")
					.NewRow("TestOne");

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<AssociationThis2Dbo>()
					.MapField(_ => _.FieldThis1, "ThisId")
					.MapField(_ => _.FieldThis3)
						.Association(_ => _.FieldThis1).ToOne(_ => _.FieldOther3)
					.MapTo(db);

				var table = db.GetTable<AssociationThis2Dbo>();

				// when
				var dbo = (from t in table select t.FieldThis3.FieldOther4).First();

				// then
				Assert.AreEqual("TestOne", dbo, "Fail result for many");
				conn.Commands[0]
					.Assert().AreField("ThisId", "Fail this key");
				conn.Commands[0]
					.Assert().AreField("FieldOther3", "Fail other key");
				conn.Commands[0]
					.Assert().AreField("FieldOther4", "Fail other result");
				Assert.AreEqual(3, conn.Commands[0].Fields.Count, "More fields");
			}
		}

		/// <summary>
		/// Association mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapAssociationForChild()
		{
			// db config
			var conn = new MockDb()
				.NewReader("Field4")
					.NewRow("TestOne");

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<AssociationThis2Dbo>()
					.MapField(_ => _.FieldThis1, "ThisId")
					.MapField(_ => _.FieldThis3)
						.Association(_ => _.FieldThis1).ToOne(_ => _.FieldOther3)
					.MapTo(db);

				var table = db.GetTable<AssociationThis2ChildDbo>();

				// when
				var dbo = (from t in table select t.FieldThis3.FieldOther4).First();

				// then
				Assert.AreEqual("TestOne", dbo, "Fail result for many");
				conn.Commands[0]
					.Assert().AreField("ThisId", "Fail this key");
				conn.Commands[0]
					.Assert().AreField("FieldOther3", "Fail other key");
				conn.Commands[0]
					.Assert().AreField("FieldOther4", "Fail other result");
				Assert.AreEqual(3, conn.Commands[0].Fields.Count, "More fields");
			}
		}

		/// <summary>
		/// Relation mapping
		/// </summary>
		[TestMethod]
		public void ShouldMapRelation()
		{
			// given
			List<Parent> parents = new List<Parent>();
			MapResultSet[] sets = new MapResultSet[3];

			sets[0] = new MapResultSet(typeof(Parent), parents);
			sets[1] = new MapResultSet(typeof(Child));
			sets[2] = new MapResultSet(typeof(Grandchild));

			// db config
			var conn = new MockDb()
				.NewReader("ParentID")
					.NewRow(1)
					.NewRow(2)
					.NextResult("ChildID", "ParentID")
					.NewRow(4, 1)
					.NewRow(5, 2)
					.NewRow(6, 2)
					.NewRow(7, 1)
					.NextResult("GrandchildID", "ChildID")
					.NewRow(1, 4)
					.NewRow(2, 4)
					.NewRow(3, 5)
					.NewRow(4, 5)
					.NewRow(5, 6)
					.NewRow(6, 6)
					.NewRow(7, 7)
					.NewRow(8, 7);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<Parent>()
					.MapField(_ => _.ID, "ParentID").PrimaryKey()
					.MapField(_ => _.Children).Relation()
					.MapTo(db);
				new FluentMap<Child>()
					.MapField(_ => _.Parent.ID, "ParentID")
					.MapField(_ => _.ID, "ChildID").PrimaryKey()
					.MapField(_ => _.Parent).Relation()
					.MapField(_ => _.Grandchildren).Relation()
					.MapTo(db);
				new FluentMap<Grandchild>()
					.MapField(_ => _.Child.ID, "ChildID")
					.MapField(_ => _.ID, "GrandchildID").PrimaryKey()
					.MapField(_ => _.Child).Relation()
					.MapTo(db);

				// when
				db.SetCommand("select *").ExecuteResultSet(sets);
			}

			// then
			Assert.IsTrue(parents.Any());

			foreach (Parent parent in parents)
			{
				Assert.IsNotNull(parent);
				Assert.IsTrue(parent.Children.Any());

				foreach (Child child in parent.Children)
				{
					Assert.AreEqual(parent, child.Parent);
					Assert.IsTrue(child.Grandchildren.Any());

					foreach (Grandchild grandchild in child.Grandchildren)
					{
						Assert.AreEqual(child, grandchild.Child);
						Assert.AreEqual(parent, grandchild.Child.Parent);
					}
				}
			}
		}

		/// <summary>
		/// Relation mapping for child
		/// </summary>
		[TestMethod]
		public void ShouldMapRelationForChild()
		{
			// given
			List<Parent2Child> parents = new List<Parent2Child>();
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Parent2Child), parents);
			sets[1] = new MapResultSet(typeof(Child2));

			// db config
			var conn = new MockDb()
				.NewReader("ParentID")
					.NewRow(1)
					.NewRow(2)
					.NextResult("ChildID", "ParentID")
					.NewRow(4, 1)
					.NewRow(5, 2)
					.NewRow(6, 2)
					.NewRow(7, 1);

			using (conn)
			using (var db = new DbManager(conn))
			{
				// fluent config
				new FluentMap<Parent2>()
					.MapField(_ => _.ID, "ParentID").PrimaryKey()
					.MapField(_ => _.Children).Relation()
					.MapTo(db);
				new FluentMap<Child2>()
					.MapField(_ => _.Parent.ID, "ParentID")
					.MapField(_ => _.ID, "ChildID").PrimaryKey()
					.MapField(_ => _.Parent).Relation()
					.MapTo(db);

				// when
				db.SetCommand("select *").ExecuteResultSet(sets);
			}

			// then
			Assert.IsTrue(parents.Any());

			foreach (Parent2Child parent in parents)
			{
				Assert.IsNotNull(parent);
				Assert.IsTrue(parent.Children.Any());

				foreach (Child2 child in parent.Children)
				{
					Assert.AreEqual(parent, child.Parent);
				}
			}
		}

		#region Dbo
		public class TableNameDbo
		{
			public int Field1 { get; set; }
		}
		public class TableNameChildDbo : TableNameDbo
		{
		}
		public class MapFieldDbo
		{
			public int Field1 { get; set; }
		}
		public class MapFieldChild1Dbo : MapFieldDbo
		{
		}
		public class MapFieldChild2Dbo : MapFieldDbo
		{
		}
		public class PrimaryKeyDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class PrimaryKeyChildDbo : PrimaryKeyDbo
		{
		}
		public class NonUpdatableDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class NonUpdatableChildDbo : NonUpdatableDbo
		{
		}
		public class SqlIgnoreInsertDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class SqlIgnoreSelectDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class SqlIgnoreChildDbo : SqlIgnoreSelectDbo
		{
		}
		public class MapIgnoreInsertDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class MapIgnoreSelectDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class MapIgnoreChildDbo : MapIgnoreSelectDbo
		{
		}
		public class TrimmableDbo
		{
			public string Field1 { get; set; }
		}
		public class TrimmableChildDbo : TrimmableDbo
		{
		}
		public class MapValueMemberDbo
		{
			public string Field1 { get; set; }
			public string Field2 { get; set; }
		}
		public class MapValueMemberChildDbo : MapValueMemberDbo
		{
		}
		public class MapValueEnumDbo
		{
			public MapValueEnum Field1 { get; set; }
			public MapValueEnum Field2 { get; set; }
			public MapValueEnum Field3 { get; set; }
			public int Field4 { get; set; }
		}
		public class MapValueEnumChildDbo : MapValueEnumDbo
		{
		}
		public enum MapValueEnum
		{
			Value1,
			Value2
		}
		public class MapValueTypeDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
			public int Field3 { get; set; }
		}
		public class MapValueTypeChildDbo : MapValueTypeDbo
		{
		}
		public abstract class InheritanceMappingDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class InheritanceMappingDbo1 : InheritanceMappingDbo
		{
		}
		public class InheritanceMappingDbo2 : InheritanceMappingDbo
		{
		}
		public abstract class InheritanceMappingChDbo
		{
			public int Field1 { get; set; }
			public int Field2 { get; set; }
		}
		public class InheritanceMappingChDbo1 : InheritanceMappingChDbo
		{
		}
		public class InheritanceMappingChDbo2 : InheritanceMappingChDbo
		{
		}
		public class AssociationThis1Dbo
		{
			public int FieldThis1 { get; set; }
			public List<AssociationOtherDbo> FieldThis2 { get; set; }
		}
		public class AssociationThis2Dbo
		{
			public int FieldThis1 { get; set; }
			public AssociationOtherDbo FieldThis3 { get; set; }
		}
		public class AssociationThis2ChildDbo : AssociationThis2Dbo
		{
		}
		public class AssociationOtherDbo
		{
			public int FieldOther1 { get; set; }
			public int FieldOther2 { get; set; }
			public int FieldOther3 { get; set; }
			public string FieldOther4 { get; set; }
		}
		public class Parent
		{
			public int ID;
			public List<Child> Children = new List<Child>();
		}
		public class Child
		{
			public int ID;
			public Parent Parent = new Parent();
			public List<Grandchild> Grandchildren = new List<Grandchild>();
		}
		public class Grandchild
		{
			public int ID;
			public Child Child = new Child();
		}
		public class Parent2
		{
			public int ID;
			public List<Child2> Children = new List<Child2>();
		}
		public class Parent2Child : Parent2
		{
		}
		public class Child2
		{
			public int ID;
			public Parent2Child Parent = new Parent2Child();
		}
		#endregion
	}
}