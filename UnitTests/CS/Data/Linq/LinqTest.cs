using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class LinqTest
	{
		class TestManager : DbManager
		{
			public Table<Person> Person
			{
				get { return GetTable<Person>(); }
			}
		}

		[Test]
		public void Test()
		{
			using (TestManager db = new TestManager())
			{
				var query = db.Person.Select(p => p);

				var list = query.ToList();
			}
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
