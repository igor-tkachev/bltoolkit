using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Common : TestBase
	{
		[Test]
		public void AsQueryable1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in    Child               select p,
				from p in db.Parent from ch in db.Child.AsQueryable() select p));
		}

		[Test]
		public void AsQueryable2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in                         Child                select p,
				from p in db.Parent from ch in ((IEnumerable<Child>)db.Child).AsQueryable() select p));
		}
	}
}
