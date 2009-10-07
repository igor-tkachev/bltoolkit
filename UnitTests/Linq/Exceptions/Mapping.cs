using System;
using System.Linq;
using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Exceptions
{
	using Linq;

	[TestFixture]
	public class Mapping : TestBase
	{
		[Test, ExpectedException(typeof(LinqException))]
		public void MapIgnore()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.Name == "123" select p;
				q.ToList();
			});
		}
	}
}
