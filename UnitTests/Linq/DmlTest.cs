using System;
using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;

using NUnit.Framework;

using Data.Linq;
using Data.Linq.Model;

#region ReSharper disable
// ReSharper disable ConvertToConstant.Local
#endregion

namespace Update
{
	[TestFixture]
	public class DmlTest : TestBase
	{
		[TestFixtureTearDown]
		public void TearDown()
		{
			ForEachProvider(db => db.Parent.Delete(p => p.ParentID >= 1000));
		}

		[Test]
		public void Delete1()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Delete(parent);
				db.Insert(parent);

				Assert.AreEqual(1, db.Parent.Count (p => p.ParentID == parent.ParentID));
				Assert.AreEqual(1, db.Parent.Delete(p => p.ParentID == parent.ParentID));
				Assert.AreEqual(0, db.Parent.Count (p => p.ParentID == parent.ParentID));
			});
		}

		[Test]
		public void Delete2()
		{
			ForEachProvider(db =>
			{
				var parent = new Parent1 { ParentID = 1001, Value1 = 1001 };

				db.Delete(parent);
				db.Insert(parent);

				Assert.AreEqual(1, db.Parent.Count(p => p.ParentID == parent.ParentID));
				Assert.AreEqual(1, db.Parent.Where(p => p.ParentID == parent.ParentID).Delete());
				Assert.AreEqual(0, db.Parent.Count(p => p.ParentID == parent.ParentID));
			});
		}

		[Test]
		public void Delete3()
		{
			ForEachProvider(new[] { ProviderName.Informix }, db =>
			{
				db.Child.Delete(c => new[] { 1001, 1002 }.Contains(c.ChildID));

				db.Child.Insert(() => new Child { ParentID = 1, ChildID = 1001 });
				db.Child.Insert(() => new Child { ParentID = 1, ChildID = 1002 });

				Assert.AreEqual(3, db.Child.Count(c => c.ParentID == 1));
				Assert.AreEqual(2, db.Child.Where(c => c.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(c.ChildID)).Delete());
				Assert.AreEqual(1, db.Child.Count(c => c.ParentID == 1));
			});
		}

		[Test]
		public void Delete4()
		{
			ForEachProvider(db =>
			{
				db.GrandChild1.Delete(gc => new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value));

				db.Insert(new[]
				{
					new GrandChild { ParentID = 1, ChildID = 1, GrandChildID = 1001 },
					new GrandChild { ParentID = 1, ChildID = 2, GrandChildID = 1002 }
				});

				Assert.AreEqual(3, db.GrandChild1.Count(gc => gc.ParentID == 1));
				Assert.AreEqual(2, db.GrandChild1.Where(gc => gc.Parent.ParentID == 1 && new[] { 1001, 1002 }.Contains(gc.GrandChildID.Value)).Delete());
				Assert.AreEqual(1, db.GrandChild1.Count(gc => gc.ParentID == 1));
			});
		}

		[Test]
		public void Delete5()
		{
			ForEachProvider(db =>
			{
				var values = new[] { 1001, 1002 };

				db.Parent.Delete(_ => _.ParentID > 1000);

				db.Insert(new[]
				{
					new Parent { ParentID = values[0], Value1 = 1 },
					new Parent { ParentID = values[1], Value1 = 1 }
				});

				Assert.AreEqual(2, db.Parent.Count(_ => _.ParentID > 1000));
				Assert.AreEqual(2, db.Parent.Delete(_ => values.Contains(_.ParentID)));
				Assert.AreEqual(0, db.Parent.Count(_ => _.ParentID > 1000));
			});
		}

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
	}
}
