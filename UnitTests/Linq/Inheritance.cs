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
			var expected = from p in ParentInheritance where p is ParentInheritanceBase select p;
			ForEachProvider(db => AreEqual(expected, from p in db.ParentInheritance where p is ParentInheritanceBase select p));
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
			var expected = ParentInheritance.OfType<ParentInheritanceNull>();
			ForEachProvider(db => AreEqual(expected, db.ParentInheritance.OfType<ParentInheritanceNull>()));
		}

		[Test]
		public void Test10()
		{
			var expected = ParentInheritance.OfType<ParentInheritanceValue>();
			ForEachProvider(db => AreEqual(expected, db.ParentInheritance.OfType<ParentInheritanceValue>()));
		}
	}
}
