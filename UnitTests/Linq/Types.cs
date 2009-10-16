using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class Types : TestBase
	{
		[Test]
		public void Bool1()
		{
			var value = true;

			var expected =
				from p in Parent
				where p.ParentID > 2 && value && true && !false
				select p;

			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID > 2 && value && true && !false
				select p));
		}

		[Test]
		public void Bool2()
		{
			var value = true;

			var expected =
				from p in Parent
				where p.ParentID > 2 && value || true && !false
				select p;

			ForEachProvider(db => AreEqual(expected,
				from p in db.Parent
				where p.ParentID > 2 && value || true && !false
				select p));
		}
	}
}
