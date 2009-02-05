using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class LinqTest : TestBase
	{
		[Test]
		public void Test()
		{
			ForEachProvider(db =>
			{
				var list = db.Person.ToList();
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
		public void Test2()
		{
			Foo(0);
			Foo(1);
		}
	}
}
