using System;
using System.Linq;

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
			var expected = from p in ParentInheritance where p is ParentInheritance1 select p;
			ForEachProvider(db => AreEqual(expected, from p in db.ParentInheritance where p is ParentInheritance1 select p));
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
			var expected =
				ParentInheritance
					.Where(p => p.ParentID == 1 || p.ParentID == 2 || p.ParentID == 4)
					.OfType<ParentInheritanceNull>();
			ForEachProvider(db => AreEqual(expected,
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
		public void TypeCastAsTest()
		{
			var expected =
				DiscontinuedProduct.ToList()
					.Select(p => p as Northwind.Product)
					.Select(p => p == null ? "NULL" : p.ProductName);

			using (var db = new NorthwindDB()) AreEqual(expected, 
				db.DiscontinuedProduct
					.Select(p => p as Northwind.Product)
					.Select(p => p == null ? "NULL" : p.ProductName));
		}
	}
}
