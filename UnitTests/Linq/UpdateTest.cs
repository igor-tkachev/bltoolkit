using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Mapping.MemberMappers;
using NUnit.Framework;

using Data.Linq;
using Data.Linq.Model;

#region ReSharper disable
// ReSharper disable ConvertToConstant.Local
#endregion

namespace Update
{
	[TestFixture]
	public class UpdateTest : TestBase
	{
		[Test]
		public void Update1()
		{
			ForEachProvider(db =>
			{
				try
				{
					var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

					db.Parent.Delete(p => p.ParentID > 1000);
					db.Insert(parent);

					Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Update(p => p.ParentID == parent.ParentID, p => new Parent { ParentID = p.ParentID + 1 }));
					Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update2()
		{
			ForEachProvider(db =>
			{
				try
				{
					var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

					db.Parent.Delete(p => p.ParentID > 1000);
					db.Insert(parent);

					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));
					Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == parent.ParentID).Update(p => new Parent { ParentID = p.ParentID + 1 }));
					Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update3()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1, db.Child.Where(c => c.ChildID == id && c.Parent.Value1 == 1).Update(c => new Child { ChildID = c.ChildID + 1 }));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update4()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == id && c.Parent.Value1 == 1)
								.Set(c => c.ChildID, c => c.ChildID + 1)
							.Update());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update5()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1,
						db.Child
							.Where(c => c.ChildID == id && c.Parent.Value1 == 1)
								.Set(c => c.ChildID, () => id + 1)
							.Update());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update6()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Parent4.Delete(p => p.ParentID > 1000);
					db.Insert(new Parent4 { ParentID = id, Value1 = TypeValue.Value1 });

					Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value1));
					Assert.AreEqual(1,
						db.Parent4
							.Where(p => p.ParentID == id)
								.Set(p => p.Value1, () => TypeValue.Value2)
							.Update());
					Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value2));
				}
				finally
				{
					db.Parent4.Delete(p => p.ParentID > 1000);
				}
			});
		}

		[Test]
		public void Update7()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				try
				{
					var id = 1001;

					db.Parent4.Delete(p => p.ParentID > 1000);
					db.Insert(new Parent4 { ParentID = id, Value1 = TypeValue.Value1 });

					Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value1));
					Assert.AreEqual(1,
						db.Parent4
							.Where(p => p.ParentID == id)
								.Set(p => p.Value1, TypeValue.Value2)
							.Update());
					Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value2));

					Assert.AreEqual(1,
						db.Parent4
							.Where(p => p.ParentID == id)
								.Set(p => p.Value1, TypeValue.Value3)
							.Update());
					Assert.AreEqual(1, db.Parent4.Count(p => p.ParentID == id && p.Value1 == TypeValue.Value3));
				}
				finally
				{
					db.Parent4.Delete(p => p.ParentID > 1000);
				}
			});
		}

		[Test]
		public void Update8()
		{
			ForEachProvider(db =>
			{
				try
				{
					var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

					db.Parent.Delete(p => p.ParentID > 1000);
					db.Insert(parent);

					parent.Value1++;

					db.Update(parent);

					Assert.AreEqual(1002, db.Parent.Single(p => p.ParentID == parent.ParentID).Value1);
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update9()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.SqlCe, ProviderName.DB2, ProviderName.Firebird, "Oracle", "DevartOracle", ProviderName.PostgreSQL, ProviderName.MySql, ProviderName.SQLite, ProviderName.Access }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					var q =
						from c in db.Child
						join p in db.Parent on c.ParentID equals p.ParentID
						where c.ChildID == id && c.Parent.Value1 == 1
						select new { c, p };

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1, q.Update(db.Child, _ => new Child { ChildID = _.c.ChildID + 1, ParentID = _.p.ParentID }));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		[Test]
		public void Update10()
		{
			ForEachProvider(new[] { ProviderName.Informix, ProviderName.SqlCe, ProviderName.DB2, ProviderName.Firebird, "Oracle", "DevartOracle", ProviderName.PostgreSQL, ProviderName.MySql, ProviderName.SQLite, ProviderName.Access }, db =>
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					var q =
						from p in db.Parent
						join c in db.Child on p.ParentID equals c.ParentID
						where c.ChildID == id && c.Parent.Value1 == 1
						select new { c, p };

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));
					Assert.AreEqual(1, q.Update(db.Child, _ => new Child { ChildID = _.c.ChildID + 1, ParentID = _.p.ParentID }));
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			});
		}

		//[Test]
		public void Update11()
		{
			ForEachProvider(db =>
			{
				var q = db.GetTable<LinqDataTypes2>().Union(db.GetTable<LinqDataTypes2>());

				//db.GetTable<LinqDataTypes2>().Update(_ => q.Contains(_), _ => new LinqDataTypes2 { GuidValue = _.GuidValue });

				q.Update(_ => new LinqDataTypes2 { GuidValue = _.GuidValue });
			});
		}

		[Test]
		public void Update12()
		{
			ForEachProvider(db =>
			{
				var parent3 = db.GetTable<Parent3>();
				try
				{
					var id = 1001;

					parent3.Delete(_ => _.ParentID2 > 1000);
					parent3.Insert(() => new Parent3() { ParentID2 = id, Value = id});

					Assert.AreEqual(1, parent3.Where(_ => _.ParentID2 == id).Set(_ => _.ParentID2, id+1).Set(_ => _.Value, _ => _.ParentID2).Update());

					var obj = parent3.FirstOrDefault(_ => _.ParentID2 == id + 1);
					Assert.IsNotNull(obj);

					db.Update(obj);

				}
				finally
				{
					parent3.Delete(_ => _.ParentID2 > 1000);
				}
			});
		}


		[Test]
		public void UpdateAssociation1([DataContexts(ProviderName.Sybase, ProviderName.Informix)] string context)
		{
			using (var db = GetDataContext(context))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				try
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);

					db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
					db.Child. Insert(() => new Child  { ChildID = childId, ParentID = parentId });

					var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

					Assert.AreEqual(1, parents.Update(db.Parent, x => new Parent { Value1 = 5 }));
				}
				finally
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);
				}
			}
		}

		[Test]
		public void UpdateAssociation2([DataContexts(ProviderName.Sybase, ProviderName.Informix)] string context)
		{
			using (var db = GetDataContext(context))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				try
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);

					db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
					db.Child. Insert(() => new Child  { ChildID = childId, ParentID = parentId });

					var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

					Assert.AreEqual(1, parents.Update(x => new Parent { Value1 = 5 }));
				}
				finally
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);
				}
			}
		}

		[Test]
		public void UpdateAssociation3([DataContexts(ProviderName.Sybase, ProviderName.Informix)] string context)
		{
			using (var db = GetDataContext(context))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				try
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);

					db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
					db.Child. Insert(() => new Child  { ChildID = childId, ParentID = parentId });

					var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

					Assert.AreEqual(1, parents.Update(x => x.ParentID > 0, x => new Parent { Value1 = 5 }));
				}
				finally
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);
				}
			}
		}

		[Test]
		public void UpdateAssociation4([DataContexts(ProviderName.Sybase, ProviderName.Informix)] string context)
		{
			using (var db = GetDataContext(context))
			{
				const int childId  = 10000;
				const int parentId = 20000;

				try
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);

					db.Parent.Insert(() => new Parent { ParentID = parentId, Value1 = parentId });
					db.Child. Insert(() => new Child  { ChildID = childId, ParentID = parentId });

					var parents =
						from child in db.Child
						where child.ChildID == childId
						select child.Parent;

					Assert.AreEqual(1, parents.Set(x => x.Value1, 5).Update());
				}
				finally
				{
					db.Child. Delete(x => x.ChildID  == childId);
					db.Parent.Delete(x => x.ParentID == parentId);
				}
			}
		}

		static readonly Func<TestDbManager,int,string,int> _updateQuery =
			CompiledQuery.Compile   <TestDbManager,int,string,int>((ctx,key,value) =>
				ctx.Person
					.Where(_ => _.ID == key)
					.Set(_ => _.FirstName, value)
					.Update());

		[Test]
		public void CompiledUpdate()
		{
			using (var ctx = new TestDbManager())
			{
				_updateQuery(ctx, 12345, "54321");
			}
		}

		[TableName("LinqDataTypes")]
		class Table1
		{
			public int  ID;
			public bool BoolValue;

			[Association(ThisKey = "ID", OtherKey = "ParentID", CanBeNull = false)]
			public List<Table2> Tables2;
		}

		[TableName("Parent")]
		class Table2
		{
			public int  ParentID;
			public bool Value1;

			[Association(ThisKey = "ParentID", OtherKey = "ID", CanBeNull = false)]
			public Table1 Table1;
		}

		[Test]
		public void UpdateAssociation5([DataContexts(
			ProviderName.Access, ProviderName.DB2, ProviderName.Firebird, ProviderName.Informix, "Oracle", ProviderName.PostgreSQL, ProviderName.SqlCe, ProviderName.SQLite,
			ExcludeLinqService=true)] string context)
		{
			using (var db = new DbManager(context))
			{
				var ids = new[] { 10000, 20000 };

				db.GetTable<Table2>()
					.Where (x => ids.Contains(x.ParentID))
					.Select(x => x.Table1)
					.Distinct()
					.Set(y => y.BoolValue, y => y.Tables2.All(x => x.Value1))
					.Update();

				var idx = db.LastQuery.IndexOf("INNER JOIN");

				Assert.That(idx, Is.Not.EqualTo(-1));

				idx = db.LastQuery.IndexOf("INNER JOIN", idx + 1);

				Assert.That(idx, Is.EqualTo(-1));
			}
		}

		[Test]
		public void AsUpdatableTest([DataContexts(ProviderName.Informix)] string context)
		{
			using (var db = GetDataContext(context))
			{
				try
				{
					var id = 1001;

					db.Child.Delete(c => c.ChildID > 1000);
					db.Child.Insert(() => new Child { ParentID = 1, ChildID = id});

					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id));

					var q  = db.Child.Where(c => c.ChildID == id && c.Parent.Value1 == 1);
					var uq = q.AsUpdatable();

					uq = uq.Set(c => c.ChildID, c => c.ChildID + 1);

					Assert.AreEqual(1, uq.Update());
					Assert.AreEqual(1, db.Child.Count(c => c.ChildID == id + 1));
				}
				finally
				{
					db.Child.Delete(c => c.ChildID > 1000);
				}
			}
		}

		[TableName("GrandChild")]
		class Table3
		{
			[PrimaryKey(1)] public int? ParentID;
			[PrimaryKey(2)] public int? ChildID;
			                public int? GrandChildID;
		}

		[Test]
		public void UpdateNullablePrimaryKey([DataContexts] string context)
		{
			using (var db = GetDataContext(context))
			{
				db.Update(new Table3 { ParentID = 10000, ChildID = null, GrandChildID = 1000 });

				if (db is DbManager)
					Assert.IsTrue(((DbManager)db).LastQuery.Contains("IS NULL"));

				db.Update(new Table3 { ParentID = 10000, ChildID = 111, GrandChildID = 1000 });

				if (db is DbManager)
					Assert.IsFalse(((DbManager)db).LastQuery.Contains("IS NULL"));
			}
		}

		public class TestObject
		{
			public int Value { get; set; }
		}

		[TableName("DataTypeTest")]
		public class Table4 
		{
			[PrimaryKey, MapField("DataTypeID"), Identity]
			public int Id;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			[MapField("String_"), DbType(DbType.String)]
			public TestObject Object;
		}

		[Test]
		public void UpdateComplexField()
		{
			ForEachProvider(db =>
			{
				var table = db.GetTable<Table4>();
				int id = 0;
				try
				{

					var obj = new Table4();
					obj.Object = new TestObject() {Value = 101};
					obj.Id = id = Convert.ToInt32(db.InsertWithIdentity(obj));

					var obj2 = table.First(_ => _.Id == id);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);

					obj.Object.Value = 999;
					db.Update(obj);

					obj2 = table.First(_ => _.Id == id);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);				

					obj.Object.Value = 666;
					table
						.Where(_ => _.Id == id)
						.Set(_ => _.Object, _ => obj.Object)
						.Update();

					obj2 = table.First(_ => _.Id == id);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);				

					obj.Object.Value = 777;
					table
						.Where(_ => _.Id == id)
						.Set(_ => _.Object, obj.Object)
						.Update();

					obj2 = table.First(_ => _.Id == id);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);

					var id2 = Convert.ToInt32(table.InsertWithIdentity(() => new Table4
					{
						Object = new TestObject() {Value = 300}
					}));

					obj2 = table.First(_ => _.Id == id2);
					Assert.AreEqual(300, obj2.Object.Value);

					var id3 = Convert.ToInt32(table.Value(_ => _.Object, () => obj.Object)
						.InsertWithIdentity());
					
					obj2 = table.First(_ => _.Id == id3);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);

					var id4 = Convert.ToInt32(table.Value(_ => _.Object, obj.Object)
						.InsertWithIdentity());
					
					obj2 = table.First(_ => _.Id == id4);
					Assert.AreEqual(obj.Object.Value, obj2.Object.Value);
				}
				finally
				{
					table.Delete(_ => _.Id >= id);
				}
			});
		}
	}
}
