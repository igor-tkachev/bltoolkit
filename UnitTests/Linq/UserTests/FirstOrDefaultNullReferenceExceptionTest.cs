using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class FirstOrDefaultNullReferenceExceptionTest : TestBase
	{
		[TableName("GrandChild")]
		class Table1
		{
			public int ChildID;
		}

		[TableName("Child")]
		class Table2
		{
			public int ChildID;
			public int ParentID;

			[Association(ThisKey = "ChildID", OtherKey = "ChildID", CanBeNull = true)]
			public List<Table1> GrandChildren { get; set; }
		}

		[TableName("Parent")]
		class Table3
		{
			public int ParentID { get; set; }

			[Association(ThisKey = "ParentID", OtherKey = "ParentID", CanBeNull = true)]
			public List<Table2> Children { get; set; }
		}

		[Test]
		public void Test()
		{
			using (var db = new TestDbManager())
			{
				/*
				var query =
					from t3 in db.Parent
					//let t1 = t3.Children.SelectMany(x => x.GrandChildren)
					//let t2 = t3.Table2s.SelectMany(x => x.Table1s)
					select new
					{
						//c2 = t1.Count(),
						c1 = t3.Children.SelectMany(x => x.GrandChildren),
					};
				 */

				var query =
					from t3 in db.GetTable<Table3>()
					let t1 = t3.Children.SelectMany(x => x.GrandChildren)
					//let t2 = t3.Table2s.SelectMany(x => x.Table1s)
					select new
					{
						c2 = t1.Count(),
						c1 = t3.Children.SelectMany(x => x.GrandChildren).Count(),
					};

				query.FirstOrDefault(p => p.c1 > 10);
			}
		}
	}
}
