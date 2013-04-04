using System;
using System.Linq;

using BLToolkit.Data;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class CompareNullableChars : TestBase
	{
		[TableName(Name = "EngineeringCircuitEnd")]
		public class EngineeringCircuitEndRecord
		{
			[PrimaryKey(1)]
			[Identity] public Int64 EngineeringCircuitID { get; set; }
			[Nullable] public Char? Gender               { get; set; }
		}

		public class SqlServerDataRepository : DbManager
		{
			public SqlServerDataRepository(string configurationString) : base(configurationString)
			{
			}

			public Table<EngineeringCircuitEndRecord> EngineeringCircuitEnds { get { return this.GetTable<EngineeringCircuitEndRecord>(); } }
		}

		[Test]
		public void Test([DataContexts(ExcludeLinqService=true)] string context)
		{
			using (var db = new SqlServerDataRepository(context))
			{
				var q =
					from current  in db.EngineeringCircuitEnds
					from previous in db.EngineeringCircuitEnds
					where current.Gender == previous.Gender
					select new { CurrentId = current.EngineeringCircuitID, PreviousId = previous.EngineeringCircuitID };

				var sql = q.ToString();
			}
		}
	}
}
