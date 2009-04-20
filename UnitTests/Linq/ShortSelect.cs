using System;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ShortSelect : TestBase
	{
		static readonly string[] _exceptList = new[] {"Informix", "DB2"};

		[Test]
		public void Parameter1()
		{
			var p = 1;
			ForEachProvider(_exceptList, db => Assert.AreEqual(p, db.Select(() => p)));
		}

		[Test]
		public void Parameter2()
		{
			ForEachProvider(_exceptList, db =>
			{
				var p = 1;
				Assert.AreEqual(p, db.Select(() => new { p }).p);
			});
		}

		[Test]
		public void Constant1()
		{
			ForEachProvider(db => Assert.AreEqual(1, db.Select(() => 1)));
		}

		[Test]
		public void Constant2()
		{
			ForEachProvider(db =>
			{
				Assert.AreEqual(1, db.Select(() => new { p = 1 }).p);
			});
		}

		[Test]
		public void StrLen()
		{
			ForEachProvider(db => Assert.AreEqual(1, db.Select(() => "1".Length)));
		}
	}
}
