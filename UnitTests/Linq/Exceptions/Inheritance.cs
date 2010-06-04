using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.Linq;

namespace Data.Exceptions
{
	using Linq;

	[TestFixture]
	public class Inheritance : TestBase
	{
		[Test, ExpectedException(typeof(LinqException))]
		public void Test1()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.ParentInheritance2 select p;
				q.ToList();
			});
		}
	}
}
