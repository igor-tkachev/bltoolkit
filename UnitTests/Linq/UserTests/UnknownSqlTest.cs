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
			Text = 1,
		}

		class ColumnInfo
		{
			public ColumnDataType DataType { get; set; }
		}

		class CustomTableColumn
		{
			public int? DataTypeID { get; set; }
		}

		[Test]
		public void Test([DataContexts] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q = db.GetTable<CustomTableColumn>()
					.Select(
						x => new ColumnInfo
						{
							DataType = x.DataTypeID == null ? ColumnDataType.Unknown : (ColumnDataType)x.DataTypeID,
						});

				var sql = q.ToString();

				Assert.That(sql, Is.Not.Contains("Unknown"));
			}
		}
	}
}
