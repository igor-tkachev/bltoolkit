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
		class Table1
		{
			[PrimaryKey(1)]
			[Identity] public Int64 Field1 { get; set; }
			[Nullable] public Char? Foeld2 { get; set; }
		}

		class Repository : DbManager
		{
			public Repository(string configurationString) : base(configurationString)
			{
			}

			public Table<Table1> Table1 { get { return this.GetTable<Table1>(); } }
		}

		[Test]
		public void Test([DataContexts(ExcludeLinqService=true)] string context)
		{
			using (var db = new Repository(context))
			{
				var q =
					from current  in db.Table1
					from previous in db.Table1
					where current.Foeld2 == previous.Foeld2
					select new { current.Field1, Field2 = previous.Field1 };

				var sql = q.ToString();
			}
		}
	}
}
