using System;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

using Data.Linq;
using Data.Linq.Model;

#region ReSharper disable
// ReSharper disable ConvertToConstant.Local
#endregion

namespace Update
{
	[TestFixture]
	public class InsertTest : TestBase
	{
		[Test]
		public void DistinctInsert1()
		{
			ForEachProvider(new[] { ProviderName.DB2, ProviderName.Informix, ProviderName.PostgreSQL, ProviderName.SQLite, ProviderName.Access }, db =>
			{
				try
				{
					db.Types.Delete(c => c.ID > 1000);

					Assert.AreEqual(
						Types.Select(_ => _.ID / 3).Distinct().Count(),
						db
							.Types
							.Select(_ => Math.Floor(_.ID / 3.0))
							.Distinct()
							.Insert(db.Types, _ => new LinqDataTypes
							{
								ID        = (int)(_ + 1001),
								GuidValue = Sql.NewGuid(),
								BoolValue = true
							}));
				}
				finally
				{
					db.Types.Delete(c => c.ID > 1000);
				}
			});
		}

		[Test]
		public void DistinctInsert2()
		{
			ForEachProvider(new[] { ProviderName.DB2, ProviderName.Informix, ProviderName.PostgreSQL, ProviderName.SQLite, ProviderName.Access }, db =>
			{
				try
				{
					db.Types.Delete(c => c.ID > 1000);

					Assert.AreEqual(
						Types.Select(_ => _.ID / 3).Distinct().Count(),
						db.Types
							.Select(_ => Math.Floor(_.ID / 3.0))
							.Distinct()
							.Into(db.Types)
								.Value(t => t.ID,        t => (int)(t + 1001))
								.Value(t => t.GuidValue, t => Sql.NewGuid())
								.Value(t => t.BoolValue, t => true)
							.Insert());
				}
				finally
				{
					db.Types.Delete(c => c.ID > 1000);
				}
			});
		}

		[Test]
		public void Insert1()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db.Child
						.Insert(() => new Child
						{
							ParentID = 1,
							ChildID  = id
						}));

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert2()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db
							.Into(db.Child)
								.Value(c => c.ParentID, () => 1)
								.Value(c => c.ChildID,  () => id)
							.Insert());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert3()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == 11)
							.Insert(db.Child, c => new Child
							{
								ParentID = c.ParentID,
								ChildID  = id
							}));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert31()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == 11)
							.Select(c => new Child
							{
								ParentID = c.ParentID,
								ChildID  = id
							})
							.Insert(db.Child, c => c));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert4()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == 11)
							.Into(db.Child)
								.Value(c => c.ParentID, c  => c.ParentID)
								.Value(c => c.ChildID,  () => id)
							.Insert());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert5()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == 11)
							.Into(db.Child)
								.Value(c => c.ParentID, c => c.ParentID)
								.Value(c => c.ChildID,  id)
							.Insert());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

        [Test]
        public void Insert65()
        {
            ForEachProvider(db =>
            {
                try
                {
                    db.Person.Where(p => p.Gender == Gender.Male)
                        .Into(db.Doctor)
                        .Value(d => d.PersonID, p => p.ID + 10)
                        .Value(d => d.Taxonomy, "VALERIU")
                        .Insert();
                }
                finally
                {
                }
            });
        }

		[Test]
		public void Insert6()
		{
			ForEachProvider(db =>
			{
				try
				{
					db.Parent.Delete(p => p.Value1 == 11);

					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == 11)
							.Into(db.Parent)
								.Value(p => p.ParentID, c => c.ParentID)
								.Value(p => p.Value1,   c => (int?)c.ChildID)
							.Insert());
					Assert.AreEqual(1, db.Parent.Count(p => p.Value1 == 11));
				}
				finally
				{
					db.Parent.Delete(p => p.Value1 == 11);
				}
			});
		}

		[Test]
		public void Insert7()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db
							.Child
								.Value(c => c.ChildID,  () => id)
								.Value(c => c.ParentID, 1)
							.Insert());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert8()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);

					Assert.AreEqual(1,
						db
							.Child
								.Value(c => c.ParentID, 1)
								.Value(c => c.ChildID,  () => id)
							.Insert());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Insert9()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Child. Delete(c => c.ParentID > 1000);
					db.Parent.Delete(p => p.ParentID > 1000);

					db.Insert(new Parent { ParentID = id, Value1 = id });
		
					Assert.AreEqual(1,
						db.Parent
							.Where(p => p.ParentID == id)
							.Insert(db.Child, p => new Child
							{
								ParentID = p.ParentID,
								ChildID  = p.ParentID,
							}));
					Assert.AreEqual(1, db.Child.Count(c => c.ParentID == id));
				}
				finally
				{
					db.Child. Delete(c => c.ParentID > 1000);
					db.Parent.Delete(p => p.ParentID > 1000);
				}
			});
		}

		[TableName("LinqDataTypes")]
		public class LinqDataTypesArrayTest
		{
			public int      ID;
			public decimal  MoneyValue;
			public DateTime DateTimeValue;
			public bool     BoolValue;
			public Guid     GuidValue;
			public byte[]   BinaryValue;
			public short    SmallIntValue;
		}

		[Test]
		public void InsertArray1()
		{
			ForEachProvider(db =>
			{
				try
				{
					var types = db.GetTable<LinqDataTypesArrayTest>();

					types.Delete(t => t.ID > 1000);
					types.Insert(() => new LinqDataTypesArrayTest { ID = 1001, BoolValue = true, BinaryValue = null });

					Assert.IsNull(types.Single(t => t.ID == 1001).BinaryValue);
				}
				finally
				{
					db.GetTable<LinqDataTypesArrayTest>().Delete(t => t.ID > 1000);
				}
			});
		}

		[Test]
		public void InsertArray2()
		{
			ForEachProvider(db =>
			{
				try
				{
					var types = db.GetTable<LinqDataTypesArrayTest>();

					types.Delete(t => t.ID > 1000);

					byte[] arr = null;

					types.Insert(() => new LinqDataTypesArrayTest { ID = 1001, BoolValue = true, BinaryValue = arr });

					var res = types.Single(t => t.ID == 1001).BinaryValue;

					Assert.IsNull(res);
				}
				finally
				{
					db.GetTable<LinqDataTypesArrayTest>().Delete(t => t.ID > 1000);
				}
			});
		}

		[Test]
		public void InsertUnion1()
		{
			Child.Count();

			ForEachProvider(
				db =>
				{
					db.Parent.Delete(p => p.ParentID > 1000);

					try
					{
						var q =
							db.Child.     Select(c => new Parent { ParentID = c.ParentID,      Value1 = (int) Math.Floor(c.ChildID / 10.0) }).Union(
							db.GrandChild.Select(c => new Parent { ParentID = c.ParentID ?? 0, Value1 = (int?)Math.Floor((c.GrandChildID ?? 0) / 100.0) }));

						q.Insert(db.Parent, p => new Parent
						{
							ParentID = p.ParentID + 1000,
							Value1   = p.Value1
						});

						Assert.AreEqual(
							Child.     Select(c => new { ParentID = c.ParentID      }).Union(
							GrandChild.Select(c => new { ParentID = c.ParentID ?? 0 })).Count(),
							db.Parent.Count(c => c.ParentID > 1000));
					}
					finally
					{
						db.Parent.Delete(p => p.ParentID > 1000);
					}
				});
		}

		[Test]
		public void InsertEnum1()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Parent4.Delete(_ => _.ParentID > 1000);

					var p = new Parent4
					{
						ParentID = id,
						Value1   = TypeValue.Value2
					};

					Assert.AreEqual(1,
						db.Parent4
						.Insert(() => new Parent4
						{
							ParentID = 1001,
							Value1   = p.Value1
						}));

					Assert.AreEqual(1, db.Parent4.Count(_ => _.ParentID == id && _.Value1 == p.Value1));
				}
				finally
				{
					db.Parent4.Delete(_ => _.ParentID > 1000);
				}
			});
		}

		[Test]
		public void InsertEnum2()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Parent4.Delete(_ => _.ParentID > 1000);

					Assert.AreEqual(1,
						db.Parent4
							.Value(_ => _.ParentID, id)
							.Value(_ => _.Value1,   TypeValue.Value1)
						.Insert());

					Assert.AreEqual(1, db.Parent4.Count(_ => _.ParentID == id));
				}
				finally
				{
					db.Parent4.Delete(_ => _.ParentID > 1000);
				}
			});
		}

		[Test]
		public void InsertEnum3()
		{
			ForEachProvider(db =>
			{
				try
				{
					var id = 1001;

					db.Parent4.Delete(_ => _.ParentID > 1000);

					Assert.AreEqual(1,
						db.Parent4
							.Value(_ => _.ParentID, id)
							.Value(_ => _.Value1,   () => TypeValue.Value1)
						.Insert());

					Assert.AreEqual(1, db.Parent4.Count(_ => _.ParentID == id));
				}
				finally
				{
					db.Parent4.Delete(_ => _.ParentID > 1000);
				}
			});
		}

		[Test]
		public void InsertNull()
		{
			ForEachProvider(db =>
			{
				try
				{
					db.Parent.Delete(p => p.ParentID == 1001);

					Assert.AreEqual(1,
						db
							.Into(db.Parent)
								.Value(p => p.ParentID, 1001)
								.Value(p => p.Value1,   (int?)null)
							.Insert());
					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == 1001));
				}
				finally
				{
					db.Parent.Delete(p => p.Value1 == 1001);
				}
			});
		}

		[Test]
		public void InsertWithIdentity1()
		{
			ForEachProvider(db =>
			{
				try
				{
					db.Person.Delete(p => p.ID > 2);

					var id =
						db.Person
							.InsertWithIdentity(() => new Person
							{
								FirstName = "John",
								LastName  = "Shepard",
								Gender    = Gender.Male
							});

					Assert.NotNull(id);

					var john = db.Person.Single(p => p.FirstName == "John" && p.LastName == "Shepard");

					Assert.NotNull (john);
					Assert.AreEqual(id, john.ID);
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void InsertWithIdentity2()
		{
			ForEachProvider(db =>
			{
				try
				{
					db.Person.Delete(p => p.ID > 2);

					var id = db
						.Into(db.Person)
							.Value(p => p.FirstName, () => "John")
							.Value(p => p.LastName,  () => "Shepard")
							.Value(p => p.Gender,    () => Gender.Male)
						.InsertWithIdentity();

					Assert.NotNull(id);

					var john = db.Person.Single(p => p.FirstName == "John" && p.LastName == "Shepard");

					Assert.NotNull (john);
					Assert.AreEqual(id, john.ID);
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void InsertWithIdentity3()
		{
			ForEachProvider(db =>
			{
				try
				{
					db.Person.Delete(p => p.ID > 2);

					var id = db
						.Into(db.Person)
							.Value(p => p.FirstName, "John")
							.Value(p => p.LastName,  "Shepard")
							.Value(p => p.Gender,    Gender.Male)
						.InsertWithIdentity();

					Assert.NotNull(id);

					var john = db.Person.Single(p => p.FirstName == "John" && p.LastName == "Shepard");

					Assert.NotNull (john);
					Assert.AreEqual(id, john.ID);
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void InsertWithIdentity4()
		{
			ForEachProvider(db =>
			{
				try
				{
					for (var i = 0; i < 2; i++)
					{
						db.Person.Delete(p => p.ID > 2);

						var id = db.InsertWithIdentity(
							new Person
							{
								FirstName = "John" + i,
								LastName  = "Shepard",
								Gender    = Gender.Male
							});

						Assert.NotNull(id);

						var john = db.Person.Single(p => p.FirstName == "John" + i && p.LastName == "Shepard");

						Assert.NotNull (john);
						Assert.AreEqual(id, john.ID);
					}
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void InsertWithIdentity5()
		{
			ForEachProvider(db =>
			{
				try
				{
					for (var i = 0; i < 2; i++)
					{
						db.Person.Delete(p => p.ID > 2);

						var person = new Person
						{
							FirstName = "John" + i,
							LastName  = "Shepard",
							Gender    = Gender.Male
						};

						var id = db.InsertWithIdentity(person);

						Assert.NotNull(id);

						var john = db.Person.Single(p => p.FirstName == "John" + i && p.LastName == "Shepard");

						Assert.NotNull (john);
						Assert.AreEqual(id, john.ID);
					}
				}
				finally
				{
					db.Person.Delete(p => p.ID > 2);
				}
			});
		}

		[Test]
		public void InsertOrUpdate1()
		{
			ForEachProvider(db =>
			{
				var id = 0;

				try
				{
					id = Convert.ToInt32(db.Person.InsertWithIdentity(() => new Person
					{
						FirstName = "John",
						LastName  = "Shepard",
						Gender    = Gender.Male
					}));

					for (var i = 0; i < 3; i++)
					{
						db.Patient.InsertOrUpdate(
							() => new Patient
							{
								PersonID  = id,
								Diagnosis = "abc",
							},
							p => new Patient
							{
								Diagnosis = (p.Diagnosis.Length + i).ToString(),
							});
					}

					Assert.AreEqual("3", db.Patient.Single(p => p.PersonID == id).Diagnosis);
				}
				finally
				{
					db.Patient.Delete(p => p.PersonID == id);
					db.Person. Delete(p => p.ID       == id);
				}
			});
		}

		[Test]
		public void InsertOrReplace1()
		{
			ForEachProvider(db =>
			{
				var id = 0;

				try
				{
					id = Convert.ToInt32(db.Person.InsertWithIdentity(() => new Person
					{
						FirstName = "John",
						LastName  = "Shepard",
						Gender    = Gender.Male
					}));

					for (var i = 0; i < 3; i++)
					{
						db.InsertOrReplace(new Patient
						{
							PersonID  = id,
							Diagnosis = ("abc" + i).ToString(),
						});
					}

					Assert.AreEqual("abc2", db.Patient.Single(p => p.PersonID == id).Diagnosis);
				}
				finally
				{
					db.Patient.Delete(p => p.PersonID == id);
					db.Person. Delete(p => p.ID       == id);
				}
			});
		}

		[Test]
		public void InsertOrUpdate3()
		{
			ForEachProvider(db =>
			{
				var id = 0;

				try
				{
					id = Convert.ToInt32(db.Person.InsertWithIdentity(() => new Person
					{
						FirstName = "John",
						LastName  = "Shepard",
						Gender    = Gender.Male
					}));

					var diagnosis = "abc";

					for (var i = 0; i < 3; i++)
					{
						db.Patient.InsertOrUpdate(
							() => new Patient
							{
								PersonID  = id,
								Diagnosis = "abc",
							},
							p => new Patient
							{
								Diagnosis = (p.Diagnosis.Length + i).ToString(),
							},
							() => new Patient
							{
								PersonID  = id,
								//Diagnosis = diagnosis,
							});

						diagnosis = (diagnosis.Length + i).ToString();
					}

					Assert.AreEqual("3", db.Patient.Single(p => p.PersonID == id).Diagnosis);
				}
				finally
				{
					db.Patient.Delete(p => p.PersonID == id);
					db.Person. Delete(p => p.ID       == id);
				}
			});
		}

		[Test]
		public void InsertBatch1()
		{
			ForEachProvider(new[] { ProviderName.PostgreSQL }, db =>
			{
				if (db is DbManager && ((DbManager)db).ConfigurationString == "Oracle")
				{
					db.Types2.Delete(_ => _.ID > 1000);

					((DbManager)db).InsertBatch(1, new[]
					{
						new LinqDataTypes2 { ID = 1003, MoneyValue = 0m, DateTimeValue = null,         BoolValue = true,  GuidValue = new Guid("ef129165-6ffe-4df9-bb6b-bb16e413c883"), SmallIntValue =  null, IntValue = null },
						new LinqDataTypes2 { ID = 1004, MoneyValue = 0m, DateTimeValue = DateTime.Now, BoolValue = false, GuidValue = null,                                             SmallIntValue =  2,    IntValue = 1532334 },
					});

					db.Types2.Delete(_ => _.ID > 1000);
				}
			});
		}

		[Test]
		public void InsertBatch2([IncludeDataContexts("Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
			{
				db.Types2.Delete(_ => _.ID > 1000);

				db.InsertBatch(100, new[]
				{
					new LinqDataTypes2 { ID = 1003, MoneyValue = 0m, DateTimeValue = null,         BoolValue = true,  GuidValue = new Guid("ef129165-6ffe-4df9-bb6b-bb16e413c883"), SmallIntValue =  null, IntValue = null },
					new LinqDataTypes2 { ID = 1004, MoneyValue = 0m, DateTimeValue = DateTime.Now, BoolValue = false, GuidValue = null,                                             SmallIntValue =  2,    IntValue = 1532334 },
				});

				db.Types2.Delete(_ => _.ID > 1000);
			}
		}

		[TableName("Parent")]
		public class  NullableFieldTestObject
		{
			public int ParentID;
			[BLToolkit.Mapping.Nullable] public int Value1;
		}

		[Test]
		public void NullableFieldTest()
		{
			ForEachProvider(db =>
			{
				db.Parent.Delete(p => p.ParentID == 1100);

				db.Insert(new NullableFieldTestObject { ParentID = 1100 });

				var parent = db.Parent.Single(p => p.ParentID == 1100);

				Assert.IsNull(parent.Value1);
			});
		}

		public class FullName
		{
			           public string FirstName     { get; set; }
			           public string LastName;
			[Nullable] public string MiddleName;
		}

		[TableName("Person")]
		[MapField("FirstName",  "Name.FirstName")]
		[MapField("LastName",   "Name.LastName")]
		[MapField("MiddleName", "Name.MiddleName")]
		public class TestPerson1
		{
			[Identity, PrimaryKey]
			//[SequenceName("PostgreSQL", "Seq")]
			[SequenceName("Firebird", "PersonID")]
			[MapField("PersonID")]
			public int ID;

			public string Gender;

			public FullName Name;
		}

		[Test]
		public void Insert11()
		{
			var p = new TestPerson1 { Name = new FullName { FirstName = "fn", LastName = "ln" }, Gender = "M" };

			ForEachProvider(db => db.Insert(p));
		}

		[Test]
		public void Insert12()
		{
			ForEachProvider(db => db
				.Into(db.GetTable<TestPerson1>())
					.Value(_ => _.Name.FirstName, "FirstName")
					.Value(_ => _.Name.LastName, () => "LastName")
					.Value(_ => _.Gender,         "F")
				.Insert());

		}

		[Test]
		public void Insert13()
		{
			ForEachProvider(db => db.GetTable<TestPerson1>()
				.Insert(() => new TestPerson1
				{
					Name = new FullName
					{
						FirstName = "FirstName",
						LastName  = "LastName"
					},
					Gender = "M",
				}));
		}

		[Test]
		public void Insert14()
		{
			ForEachProvider(
				new [] { ProviderName.SqlCe, ProviderName.Access, "Sql2000", "Sql2005", ProviderName.Sybase },
				db =>
				{
					try
					{
						db.Person.Delete(p => p.FirstName.StartsWith("Insert14"));

						Assert.AreEqual(1,
							db.Person
							.Insert(() => new Person
							{
								FirstName = "Insert14" + db.Person.Where(p => p.ID == 1).Select(p => p.FirstName).FirstOrDefault(),
								LastName  = "Shepard",
								Gender = Gender.Male
							}));

						Assert.AreEqual(1, db.Person.Count(p => p.FirstName.StartsWith("Insert14")));
					}
					finally
					{
						db.Person.Delete(p => p.FirstName.StartsWith("Insert14"));
					}
				});
		}

		[Test]
		public void InsertSingleIdentity()
		{
			ForEachProvider(
				new [] { ProviderName.Informix, ProviderName.SqlCe },
				db =>
				{
					try
					{
						db.TestIdentity.Delete();

						var id = db.TestIdentity.InsertWithIdentity(() => new TestIdentity {});

						Assert.NotNull(id);
					}
					finally
					{
						db.TestIdentity.Delete();
					}
				});
		}
	}
}
