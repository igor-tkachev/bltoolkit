using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class CompileTest : TestBase
	{
		[Test]
		public void CompiledTest1()
		{
			var query = CompiledQuery.Compile((TestDbManager db, string n1, int n2) => n1 + n2);

			ForEachProvider(db =>
			{
				Assert.AreEqual("11", query(db, "1", 1));
				Assert.AreEqual("22", query(db, "2", 2));
			});
		}

		[Test]
		public void CompiledTest2()
		{
			var query = CompiledQuery.Compile((TestDbManager db, int n) => db.Child.Where(c => c.ParentID == n).Take(n));

			ForEachProvider(db =>
			{
				Assert.AreEqual(1, query(db, 1).ToList().Count());
				Assert.AreEqual(2, query(db, 2).ToList().Count());
			});
		}

		[Test]
		public void CompiledTest3()
		{
			var query = CompiledQuery.Compile((TestDbManager db, int n) => db.GetTable<Child>().Where(c => c.ParentID == n).Take(n));

			ForEachProvider(db =>
			{
				Assert.AreEqual(1, query(db, 1).ToList().Count());
				Assert.AreEqual(2, query(db, 2).ToList().Count());
			});
		}
	}
}
