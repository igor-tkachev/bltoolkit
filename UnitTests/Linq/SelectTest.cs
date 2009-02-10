using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class SelectTest : TestBase
	{
		[Test]
		public void Test1()
		{
			ForEachProvider(db =>
			{
				var list = db.Person.ToList();
				Assert.AreNotEqual(0, list.Count);
			});
		}

		[Test]
		public void Test2()
		{
			ForEachProvider(db =>
			{
				var list = (from p in db.Person select p).ToList();
				Assert.AreNotEqual(0, list.Count);
			});
		}

		Func<int, int> f;

		void Func(System.Linq.Expressions.Expression<Func<int, int>> func, int n)
		{
			if (f == null)
				f = func.Compile();

			n = f(n);

			Console.WriteLine(n);
		}

		void Foo(int i)
		{
			Func((n) => n + 2, i);
		}

		[Test]
		public void Test___()
		{
			Foo(0);
			Foo(1);
		}
	}
}
