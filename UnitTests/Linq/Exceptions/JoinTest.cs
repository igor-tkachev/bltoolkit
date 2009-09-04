using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Exceptions
{
	using Linq.Model;
	using Linq;

	[TestFixture]
	public class JoinTest : TestBase
	{
		[Test, ExpectedException(typeof(NotSupportedException))]
		public void InnerJoin()
		{
			ForEachProvider(db =>
			{
				var q =
					from p1 in db.Person
						join p2 in db.Person on new Person { ID = p1.ID } equals new Person { ID = p2.ID }
					where p1.ID == 1
					select new Person { ID = p1.ID, FirstName = p2.FirstName };
				q.ToList();
			});
		}
	}
}
