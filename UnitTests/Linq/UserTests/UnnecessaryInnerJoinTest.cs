using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class UnnecessaryInnerJoinTest : TestBase
	{
		class Table1
		{
			[PrimaryKey(1)]
			[Identity]
			public Int64 Field1 { get; set; }
			public Int64 Field2 { get; set; }
		}

		class Table2
		{
			[PrimaryKey(1)]
			[Identity]
			public Int64 Field2 { get; set; }

			[Association(ThisKey = "Field2", OtherKey = "Field2", CanBeNull = false)]
			public List<Table1> Field3 { get; set; }
		}

		[Test]
		public void Test([DataContextsAttribute(ExcludeLinqService=true)] string context)
		{
			var ids = new long[] { 1, 2, 3 };

			using (var db = new DbManager(context))
			{
				var q =
					from t1 in db.GetTable<Table2>()
					where t1.Field3.Any(x => ids.Contains(x.Field1))
					select new { t1.Field2 };

				var sql = q.ToString();

				Assert.That(sql.IndexOf("INNER JOIN"), Is.LessThan(0));
			}
		}
	}
}
