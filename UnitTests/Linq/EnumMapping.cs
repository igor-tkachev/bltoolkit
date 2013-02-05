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
            [MapValue(11L)]
            Value1 = 3,
            [MapValue(12L)]
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

        [TableName("LinqDataTypes")]
        class TestTable1
        {
            [PrimaryKey, MapField("ID")]
            public int Id;

            [MapField("BigIntValue")]
            public TestEnum1 TestField;
        }

        [MapValue(TestEnum2.Value2, 12L)]
        [TableName("LinqDataTypes")]
        class TestTable2
        {
            [PrimaryKey, MapField("ID")]
            public int Id;

            [MapValue(TestEnum2.Value1, 11L)]
            [MapField("BigIntValue")]
            public TestEnum2 TestField;

            [MapField("IntValue")]
            public TestEnum3 Int32Field;
        }

        [TableName("LinqDataTypes")]
        class NullableTestTable1
        {
            [PrimaryKey, MapField("ID")]
            public int? Id;

            [MapField("BigIntValue")]
            public TestEnum1? TestField;
        }

        [MapValue(TestEnum2.Value2, 12L)]
        [TableName("LinqDataTypes")]
        class NullableTestTable2
        {
            [PrimaryKey, MapField("ID")]
            public int? Id;

            [MapValue(TestEnum2.Value1, 11L)]
            [MapField("BigIntValue")]
            public TestEnum2? TestField;

            [MapField("IntValue")]
            public TestEnum3? Int32Field;
        }

        [TableName("LinqDataTypes")]
        class RawTable
        {
            [PrimaryKey, MapField("ID")]
            public int Id;

            [MapField("BigIntValue")]
            public long TestField;

            [MapField("IntValue")]
            public int Int32Field;
        }

        class Cleaner : IDisposable
        {
            ITestDataContext _db;
            public Cleaner(ITestDataContext db)
            {
                _db = db;
                Clean();
            }

            private void Clean()
            {
                _db.GetTable<RawTable>().Where(r => r.Id == RID).Delete();
            }

            public void Dispose()
            {
                try
                {
                    // rollback emulation for WCF
                    Clean();
                }
                catch (Exception)
                {
                }
            }
        }

        const long VAL2 = 12;
        const long VAL1 = 11;
        const int RID = 101;

        [Test]
        public void EnumMapInsert1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<TestTable1>().Insert(() => new TestTable1
                    {
                        Id = RID,
                        TestField = TestEnum1.Value2
                    });

                    Assert.AreEqual(1, db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Count());
                }
            });
        }
        [Test]
        public void EnumMapInsert2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<TestTable2>().Insert(() => new TestTable2
                    {
                        Id = RID,
                        TestField = TestEnum2.Value2
                    });

                    Assert.AreEqual(1, db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Count());
                }
            });
        }
        [Test]
        public void EnumMapInsert3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<NullableTestTable1>().Insert(() => new NullableTestTable1
                    {
                        Id = RID,
                        TestField = TestEnum1.Value2
                    });

                    Assert.AreEqual(1, db.GetTable<RawTable>()
                        .Where(r => r.Id == RID && r.TestField == VAL2).Count());
                }
            });
        }
        [Test]
        public void EnumMapInsert4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<NullableTestTable2>().Insert(() => new NullableTestTable2
                    {
                        Id = RID,
                        TestField = TestEnum2.Value2
                    });

                    Assert.AreEqual(1, db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Count());
                }
            });
        }

        [Test]
        public void EnumMapWhere1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<TestTable1>().Where(r => r.Id == RID && r.TestField == TestEnum1.Value2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == TestEnum1.Value2);
                }
            });
        }
        [Test]
        public void EnumMapWhere2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<TestTable2>().Where(r => r.Id == RID && r.TestField == TestEnum2.Value2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == TestEnum2.Value2);
                }
            });
        }
        [Test]
        public void EnumMapWhere3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value2)
                        .Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == TestEnum1.Value2);
                }
            });
        }
        [Test]
        public void EnumMapWhere4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value2)
                        .Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == TestEnum2.Value2);
                }
            });
        }

        [Test]
        public void EnumMapUpdate1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<TestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value1)
                        .Update(r => new TestTable1 { TestField = TestEnum1.Value2 });

                    var result = db.GetTable<RawTable>()
                        .Where(r => r.Id == RID && r.TestField == VAL2)
                        .Select(r => r.TestField)
                        .FirstOrDefault();

                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapUpdate2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<TestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value1)
                        .Update(r => new TestTable2 { TestField = TestEnum2.Value2 });

                    var result = db.GetTable<RawTable>()
                        .Where(r => r.Id == RID && r.TestField == VAL2)
                        .Select(r => r.TestField)
                        .FirstOrDefault();

                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapUpdate3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value1)
                        .Update(r => new NullableTestTable1 { TestField = TestEnum1.Value2 });

                    var result = db.GetTable<RawTable>()
                        .Where(r => r.Id == RID && r.TestField == VAL2)
                        .Select(r => r.TestField)
                        .FirstOrDefault();

                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapUpdate4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value1)
                        .Update(r => new NullableTestTable2 { TestField = TestEnum2.Value2 });

                    var result = db.GetTable<RawTable>()
                        .Where(r => r.Id == RID && r.TestField == VAL2)
                        .Select(r => r.TestField)
                        .FirstOrDefault();

                    Assert.True(result == VAL2);
                }
            });
        }

        [Test]
        public void EnumMapSelectAnon1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<TestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value2)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == TestEnum1.Value2);
                }
            });
        }
        [Test]
        public void EnumMapSelectAnon2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<TestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value2)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == TestEnum2.Value2);
                }
            });
        }
        [Test]
        public void EnumMapSelectAnon3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value2)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == TestEnum1.Value2);
                }
            });
        }
        [Test]
        public void EnumMapSelectAnon4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    var result = db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value2)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == TestEnum2.Value2);
                }
            });
        }

        [Test]
        public void EnumMapDelete1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<TestTable1>().Delete(r => r.Id == RID && r.TestField == TestEnum1.Value2));
                }
            });
        }
        [Test]
        public void EnumMapDelete2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<TestTable2>().Delete(r => r.Id == RID && r.TestField == TestEnum2.Value2));
                }
            });
        }
        [Test]
        public void EnumMapDelete3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<NullableTestTable1>()
                        .Delete(r => r.Id == RID && r.TestField == TestEnum1.Value2));
                }
            });
        }
        [Test]
        public void EnumMapDelete4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<NullableTestTable2>()
                        .Delete(r => r.Id == RID && r.TestField == TestEnum2.Value2));
                }
            });
        }

        [Test]
        public void EnumMapSet1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<TestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value1)
                        .Set(r => r.TestField, TestEnum1.Value2).Update();
                    var result = db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapSet2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<TestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value1)
                        .Set(r => r.TestField, TestEnum2.Value2).Update();
                    var result = db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapSet3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        Int32Field = 3
                    });

                    db.GetTable<TestTable2>()
                        .Where(r => r.Id == RID && r.Int32Field == TestEnum3.Value1)
                        .Set(r => r.Int32Field, () => TestEnum3.Value2).Update();
                    Assert.True(1 == db.GetTable<RawTable>().Where(r => r.Id == RID && r.Int32Field == 4).Count());
                }
            });
        }
        [Test]
        public void EnumMapSet4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum1.Value1)
                        .Set(r => r.TestField, TestEnum1.Value2).Update();
                    var result = db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapSet5()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL1
                    });

                    db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.TestField == TestEnum2.Value1)
                        .Set(r => r.TestField, TestEnum2.Value2).Update();
                    var result = db.GetTable<RawTable>().Where(r => r.Id == RID && r.TestField == VAL2).Select(r => r.TestField).FirstOrDefault();
                    Assert.True(result == VAL2);
                }
            });
        }
        [Test]
        public void EnumMapSet6()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        Int32Field = 3
                    });

                    db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.Int32Field == TestEnum3.Value1)
                        .Set(r => r.Int32Field, () => TestEnum3.Value2).Update();
                    Assert.True(1 == db.GetTable<RawTable>().Where(r => r.Id == RID && r.Int32Field == 4).Count());
                }
            });
        }

        [Test]
        public void EnumMapContains1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<TestTable1>()
                        .Where(r => r.Id == RID && new[] { TestEnum1.Value2 }.Contains(r.TestField)).Count());
                }
            });
        }
        [Test]
        public void EnumMapContains2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<TestTable2>().Where(r => r.Id == RID && new[] { TestEnum2.Value2 }.Contains(r.TestField)).Count());
                }
            });
        }
        [Test]
        public void EnumMapContains3()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && new[] { (TestEnum1?)TestEnum1.Value2 }.Contains(r.TestField)).Count());
                }
            });
        }
        [Test]
        public void EnumMapContains4()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID,
                        TestField = VAL2
                    });

                    Assert.True(1 == db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && new[] { (TestEnum2?)TestEnum2.Value2 }.Contains(r.TestField)).Count());
                }
            });
        }

        [Test]
        public void EnumMapSelectNull1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID
                    });

                    var result = db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == null);
                }
            });
        }
        [Test]
        public void EnumMapSelectNull2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID
                    });

                    var result = db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID)
                        .Select(r => new { r.TestField })
                        .FirstOrDefault();

                    Assert.NotNull(result);
                    Assert.True(result.TestField == null);
                }
            });
        }

        [Test]
        public void EnumMapWhereNull1()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID
                    });

                    var result = db.GetTable<NullableTestTable1>()
                        .Where(r => r.Id == RID && r.TestField == null)
                        .Select(r => new { r.TestField }).FirstOrDefault();
                    Assert.NotNull(result);
                    Assert.Null(result.TestField);
                }
            });
        }
        [Test]
        public void EnumMapWhereNull2()
        {
            ForEachProvider(db =>
            {
                using (new Cleaner(db))
                {
                    db.GetTable<RawTable>().Insert(() => new RawTable()
                    {
                        Id = RID
                    });

                    var result = db.GetTable<NullableTestTable2>()
                        .Where(r => r.Id == RID && r.TestField == null)
                        .Select(r => new { r.TestField }).FirstOrDefault();
                    Assert.NotNull(result);
                    Assert.Null(result.TestField);
                }
            });
        }
        
    }
}
