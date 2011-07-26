using System;
using System.Linq;

using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Inheritance : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(db => AreEqual(ParentInheritance, db.ParentInheritance));
		}

		[Test]
		public void Test2()
		{
			ForEachProvider(db => AreEqual(ParentInheritance, db.ParentInheritance.Select(p => p)));
		}

		[Test]
		public void Test3()
		{
			ForEachProvider(db => AreEqual(
				from p in    ParentInheritance where p is ParentInheritance1 select p,
				from p in db.ParentInheritance where p is ParentInheritance1 select p));
		}

		[Test]
		public void Test4()
		{
			var expected = from p in ParentInheritance where !(p is ParentInheritanceNull) select p;
			ForEachProvider(db => AreEqual(expected, from p in db.ParentInheritance where !(p is ParentInheritanceNull) select p));
		}

		[Test]
		public void Test5()
		{
			var expected = from p in ParentInheritance where p is ParentInheritanceValue select p;
			ForEachProvider(db => AreEqual(expected, from p in db.ParentInheritance where p is ParentInheritanceValue select p));
		}

		[Test]
		public void Test6()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.ParentInheritance2 where p is ParentInheritance12 select p;
				q.ToList();
			});
		}

		[Test]
		public void Test7()
		{
#pragma warning disable 183
			var expected = from p in ParentInheritance where p is ParentInheritanceBase select p;
			ForEachProvider(db => AreEqual(expected, from p in db.ParentInheritance where p is ParentInheritanceBase select p));
#pragma warning restore 183
		}

		[Test]
		public void Test8()
		{
			var expected = ParentInheritance.OfType<ParentInheritance1>();
			ForEachProvider(db => AreEqual(expected, db.ParentInheritance.OfType<ParentInheritance1>()));
		}

		[Test]
		public void Test9()
		{
			ForEachProvider(db =>
				AreEqual(
					   ParentInheritance
						.Where(p => p.ParentID == 1 || p.ParentID == 2 || p.ParentID == 4)
						.OfType<ParentInheritanceNull>(),
					db.ParentInheritance
						.Where(p => p.ParentID == 1 || p.ParentID == 2 || p.ParentID == 4)
						.OfType<ParentInheritanceNull>()));
		}

		[Test]
		public void Test10()
		{
			var expected = ParentInheritance.OfType<ParentInheritanceValue>();
			ForEachProvider(db => AreEqual(expected, db.ParentInheritance.OfType<ParentInheritanceValue>()));
		}

		[Test]
		public void Test11()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.ParentInheritance3 where p is ParentInheritance13 select p;
				q.ToList();
			});
		}

		[Test]
		public void Test12()
		{
			ForEachProvider(db => AreEqual(
				from p in    ParentInheritance1 where p.ParentID == 1 select p,
				from p in db.ParentInheritance1 where p.ParentID == 1 select p));
		}

		//[Test]
		public void Test13()
		{
			ForEachProvider(db => AreEqual(
				from p in    ParentInheritance4
				join c in    Child on p.ParentID equals c.ParentID
				select p,
				from p in db.ParentInheritance4
				join c in db.Child on p.ParentID equals c.ParentID
				select p));
		}

		[Test]
		public void TypeCastAsTest1()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					   DiscontinuedProduct.ToList()
						.Select(p => p as Northwind.Product)
						.Select(p => p == null ? "NULL" : p.ProductName),
					db.DiscontinuedProduct
						.Select(p => p as Northwind.Product)
						.Select(p => p == null ? "NULL" : p.ProductName));
		}

		[Test]
		public void TypeCastAsTest11()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					   DiscontinuedProduct.ToList()
						.Select(p => new { p = p as Northwind.Product })
						.Select(p => p.p == null ? "NULL" : p.p.ProductName),
					db.DiscontinuedProduct
						.Select(p => new { p = p as Northwind.Product })
						.Select(p => p.p == null ? "NULL" : p.p.ProductName));
		}

		[Test]
		public void TypeCastAsTest2()
		{
			using (var db = new NorthwindDB())
				AreEqual(
					   Product.ToList()
						.Select(p => p as Northwind.DiscontinuedProduct)
						.Select(p => p == null ? "NULL" : p.ProductName),
					db.Product
						.Select(p => p as Northwind.DiscontinuedProduct)
						.Select(p => p == null ? "NULL" : p.ProductName));
		}

		[Test]
		public void FirstOrDefault()
		{
			using (var db = new NorthwindDB())
				Assert.AreEqual(
					   DiscontinuedProduct.FirstOrDefault().ProductID,
					db.DiscontinuedProduct.FirstOrDefault().ProductID);
		}

		[Test]
		public void Cast1()
		{
			ForEachProvider(db => AreEqual(
				   ParentInheritance.OfType<ParentInheritance1>().Cast<ParentInheritanceBase>(),
				db.ParentInheritance.OfType<ParentInheritance1>().Cast<ParentInheritanceBase>()));
		}

		class ParentEx : Parent
		{
			[MapIgnore]
			protected bool Field1;

			public static void Test(Inheritance inheritance)
			{
			inheritance.ForEachProvider(db => inheritance.AreEqual(
				inheritance.Parent.Select(p => new ParentEx { Field1 = true, ParentID = p.ParentID, Value1 = p.Value1 }).Cast<Parent>(),
				         db.Parent.Select(p => new ParentEx { Field1 = true, ParentID = p.ParentID, Value1 = p.Value1 }).Cast<Parent>()));
			}
		}

		[Test]
		public void Cast2()
		{
			ParentEx.Test(this);
		}

		[TableName("Person")]
		class PersonEx : Person
		{
		}

		[Test]
		public void SimplTest()
		{
			using (var db = new TestDbManager())
				Assert.AreEqual(1, db.GetTable<PersonEx>().Where(_ => _.FirstName == "John").Select(_ => _.ID).Single());
		}

		[InheritanceMapping(Code = 1, Type = typeof(Parent222))]
		[TableName("Parent")]
		public class Parent111
		{
			[MapField(IsInheritanceDiscriminator = true)]
			public int ParentID;
		}

		[MapField("Value1", "Value.ID")]
		public class Parent222 : Parent111
		{
			[MapIgnore]
			public Value111 Value;
		}

		public class Value111
		{
			public int ID;
		}

		[Test]
		public void InheritanceMappingIssueTest()
		{
			using (var db = new TestDbManager())
			{
				var q1 = db.GetTable<Parent222>();
				var q  = q1.Where(_ => _.Value.ID == 1);

				var sql = ((Table<Parent222>)q).SqlText;
				Assert.IsNotEmpty(sql);
			}
		}

		[Test]
		public void ReferenceNavigation()
		{
			using (var db = new NorthwindDB())
			{
				var result =
					from od in db.OrderDetail
					where od.Product.Category.CategoryName == "Seafood"
					select new { od.Order, od.Product };
				
				var list = result.ToList();

				Assert.AreEqual(330, list.Count);

				foreach (var item in list)
				{
					Assert.IsNotNull(item);
					Assert.IsNotNull(item.Order);
					Assert.IsNotNull(item.Product);
					Assert.IsTrue(
						 item.Product.Discontinued && item.Product is Northwind.DiscontinuedProduct ||
						!item.Product.Discontinued && item.Product is Northwind.ActiveProduct);
				}
			}
		}

		[Test]
		public void TypeCastIsChildConditional1()
		{
			using (var db = new NorthwindDB())
			{
				var result   = db.Product.         Select(x => x is Northwind.DiscontinuedProduct ? x : null);
				var expected = db.Product.ToList().Select(x => x is Northwind.DiscontinuedProduct ? x : null);

				var list = result.ToList();

				Assert.Greater(list.Count, 0);
				Assert.AreEqual(expected.Count(), list.Count);
				Assert.IsTrue(list.Except(expected).Count() == 0);
				Assert.IsTrue(list.Contains(null));
			}
		}

		[Test]
		public void TypeCastIsChildConditional2()
		{
			using (var db = new NorthwindDB())
			{
				var result   = db.Product.         Select(x => x is Northwind.DiscontinuedProduct);
				var expected = db.Product.ToList().Select(x => x is Northwind.DiscontinuedProduct);

				var list = result.ToList();

				Assert.Greater(list.Count, 0);
				Assert.AreEqual(expected.Count(), list.Count);
				Assert.IsTrue(list.Except(expected).Count() == 0);
			}
		}

		[Test]
		public void TypeCastIsChild()
		{
			using (var db = new NorthwindDB())
			{
				var result    = db.Product.Where(x => x is Northwind.DiscontinuedProduct).ToList();
				var exprected =    Product.Where(x => x is Northwind.DiscontinuedProduct).ToList();

				Assert.Greater(result.Count, 0);
				Assert.AreEqual(result.Count, exprected.Count);
			}
		}
	}
}
