using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Data.Linq;
using NUnit.Framework;

namespace Data.Linq
{
    [TestFixture, Category("MapValue")]
    public class EnumMapping : TestBase
    {
        enum TestEnum1
        {
            [MapValue("VAL1")]
            Value1 = 3,
            [MapValue("VAL2")]
            Value2,
        }
        enum TestEnum2
        {
            Value1 = 3,
            Value2,
        }
        enum TestEnum3
        {
            Value1 = 3,
            Value2,
        }

        [TableName("DataTypeTest")]
        class TestTable1
        {
            [PrimaryKey, MapField("DataTypeID")]
            public int Id;

            [MapField("String_")]
            public TestEnum1 TestField;
        }

        [MapValue(TestEnum2.Value2, "VAL2")]
        [TableName("DataTypeTest")]
        class TestTable2
        {
            [PrimaryKey, MapField("DataTypeID")]
            public int Id;

            [MapValue(TestEnum2.Value1, "VAL1")]
            [MapField("String_")]
            public TestEnum2 TestField;

            [MapField("Int32_")]
            public TestEnum3 Int32Field;
        }

        [TableName("DataTypeTest")]
        class RawTable
        {
            [PrimaryKey, MapField("DataTypeID")]
            public int Id;

            [MapField("String_")]
            public string TestField;

            [MapField("Int32_")]
            public int Int32Field;
        }

        [Test]
        public void EnumMapInsert1()
        {
            ForEachProvider(db =>
            {
                db.GetTable<TestTable1>().Insert(() => new TestTable1
                {
                    TestField = TestEnum1.Value2
                });
                Assert.True(db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Count() == 1);
            });
        }

        [Test]
        public void EnumMapInsert2()
        {
            ForEachProvider(db =>
            {
                db.GetTable<TestTable2>().Insert(() => new TestTable2
                {
                    TestField = TestEnum2.Value2
                });
                Assert.True(db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Count() == 1);
            });
        }

        [Test]
        public void EnumMapWhere1()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });

                var result = db.GetTable<TestTable1>().Where(r => r.TestField == TestEnum1.Value2).Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == TestEnum1.Value2);
            });
        }

        [Test]
        public void EnumMapWhere2()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });

                var result = db.GetTable<TestTable2>().Where(r => r.TestField == TestEnum2.Value2).Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == TestEnum2.Value2);
            });
        }

        [Test]
        public void EnumMapUpdate1()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable1>()
                    .Where(r => r.TestField == TestEnum1.Value1)
                    .Update(r => new TestTable1 { TestField = TestEnum1.Value2 });
                var result = db.GetTable<TestTable1>().Where(r => r.TestField == TestEnum1.Value2).Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == TestEnum1.Value2);
            });
        }

        [Test]
        public void EnumMapUpdate2()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable2>()
                    .Where(r => r.TestField == TestEnum2.Value1)
                    .Update(r => new TestTable2 { TestField = TestEnum2.Value2 });
                var result = db.GetTable<TestTable2>().Where(r => r.TestField == TestEnum2.Value2).Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == TestEnum2.Value2);
            });
        }

        [Test]
        public void EnumMapSelectAnon1()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });

                var result = db.GetTable<TestTable1>().Where(r => r.TestField == TestEnum1.Value2).Select(r => new { r.TestField }).FirstOrDefault();
                Assert.NotNull(result);
                Assert.True(result.TestField == TestEnum1.Value2);
            });
        }

        [Test]
        public void EnumMapSelectAnon2()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });

                var result = db.GetTable<TestTable2>().Where(r => r.TestField == TestEnum2.Value2).Select(r => new { r.TestField }).FirstOrDefault();
                Assert.NotNull(result);
                Assert.True(result.TestField == TestEnum2.Value2);
            });
        }

        [Test]
        public void EnumMapUpdate3()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable1>()
                    .Update(r => new TestTable1 { TestField = TestEnum1.Value2 });
                var result = db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == "VAL2");
            });
        }

        [Test]
        public void EnumMapUpdate4()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable2>()
                    .Update(r => new TestTable2 { TestField = TestEnum2.Value2 });
                var result = db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == "VAL2");
            });
        }

        [Test]
        public void EnumMapDelete1()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });
                Assert.True(1 == db.GetTable<TestTable1>().Delete(r => r.TestField == TestEnum1.Value2));
            });
        }

        [Test]
        public void EnumMapDelete2()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL2"
                });

                Assert.True(1 == db.GetTable<TestTable2>().Delete(r => r.TestField == TestEnum2.Value2));
            });
        }

        [Test]
        public void EnumMapUpdate5()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable1>()
                    .Set(r => r.TestField, TestEnum1.Value2).Update();
                var result = db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == "VAL2");
            });
        }

        [Test]
        public void EnumMapUpdate6()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    TestField = "VAL1"
                });

                db.GetTable<TestTable2>()
                    .Set(r => r.TestField, TestEnum2.Value2).Update();
                var result = db.GetTable<RawTable>().Where(r => r.TestField == "VAL2").Select(r => r.TestField).FirstOrDefault();
                Assert.True(result == "VAL2");
            });
        }

        [Test]
        public void EnumMapUpdate7()
        {
            ForEachProvider(db =>
            {
                db.GetTable<RawTable>().Insert(() => new RawTable()
                {
                    Int32Field = 3
                });

                db.GetTable<TestTable2>()
                    .Where(r => r.Int32Field == TestEnum3.Value1)
                    .Set(r => r.Int32Field, () => TestEnum3.Value2).Update();
                Assert.True(1 == db.GetTable<RawTable>().Where(r => r.Int32Field == 4).Count());
            });
        }
    }
}
