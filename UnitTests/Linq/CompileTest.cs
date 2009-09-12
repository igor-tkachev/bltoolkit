using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Linq
{
	[TestFixture]
	public class CompileTest : TestBase
	{
		[Test]
		public void CompiledTest1()
		{
			var query = CompiledQuery.Compile((TestDbManager db, int n) => db.Child.Where(c => c.ParentID == n).Take(n));

			ForEachProvider(db =>
			{
				Assert.AreEqual(1, query(db, 1).ToList().Count());
				Assert.AreEqual(2, query(db, 2).ToList().Count());
			});
		}
	}
}
