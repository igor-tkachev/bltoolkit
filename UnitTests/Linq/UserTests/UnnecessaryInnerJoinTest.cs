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
		[TableName(Name = "EngineeringCircuitEnd")]
		public class EngineeringCircuitEndRecord
		{
			[PrimaryKey(1)]
			[Identity]
			public Int64 EngineeringCircuitID { get; set; }

			public Int64 EngineeringConnectorID { get; set; }
		}

		[TableName(Name = "EngineeringConnector")]
		public class EngineeringConnectorRecord
		{
			[Association(ThisKey = "EngineeringConnectorID", OtherKey = "EngineeringConnectorID", CanBeNull = false)]
			public List<EngineeringCircuitEndRecord> EngineeringCircuits { get; set; }

			[PrimaryKey(1)]
			[Identity]
			public Int64 EngineeringConnectorID { get; set; }
		}

		[Test]
		public void Test([DataContextsAttribute(ExcludeLinqService=true)] string context)
		{
			var ids = new long[] { 1, 2, 3 };

			using (var db = new DbManager(context))
			{
				var q =
					from engineeringConnector in db.GetTable<EngineeringConnectorRecord>()
					where engineeringConnector.EngineeringCircuits.Any(x => ids.Contains(x.EngineeringCircuitID))
					select new { engineeringConnector.EngineeringConnectorID };

				var sql = q.ToString();

				Assert.That(sql.IndexOf("INNER JOIN"), Is.LessThan(0));
			}
		}
	}
}
