using System;
using System.Linq;

using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class UnknownSqlTest : TestBase
	{
		enum ColumnDataType
		{
			Unknown = 0,
			Text    = 1,
		}

		class CustomTableColumn
		{
			public int? DataTypeID { get; set; }
		}

		[Test]
		public void Test()
		{
			using (var db = new TestDbManager())
			{
				var q = db.GetTable<CustomTableColumn>()
					.Select(
						x => new
						{
							DataType = Sql.AsSql(ColumnDataType.Unknown),
						});

				var sql = q.ToString();

				Assert.That(sql, Is.Not.Contains("Unknown"));
			}
		}
	}
}
