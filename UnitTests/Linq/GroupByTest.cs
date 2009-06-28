using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class GroupByTest : TestBase
	{
		[Test]
		public void GroupBy1()
		{
			TestJohn(db =>
				from p in db.Person
				where p.ID == 1
				group p by p.FirstName into g
				select new Person { ID = g.Min(p => p.ID), FirstName = g.Key });
		}
	}
}
