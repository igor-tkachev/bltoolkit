using System;

using NUnit.Framework;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class MsSql2008 : TestBase
	{
		[Test]
		public void SqlTest()
		{
			using (var db = new TestDbManager("Sql2008"))
			using (var rd = db.SetCommand(@"
				SELECT
					DateAdd(Hour, 1, [t].[DateTimeValue]) - [t].[DateTimeValue]
				FROM
					[LinqDataTypes] [t]")
				.ExecuteReader())
			{
				if (rd.Read())
				{
					var value = rd.GetValue(0);
				}
			}
		}

		[Test]
		public void SqlTypeTest()
		{
			using (var db = new TestDbManager("Sql2008"))
			{
				var value = db.SetCommand(@"SELECT SmallIntValue FROM LinqDataTypes WHERE ID = 1").ExecuteScalar<short>();

				db.SetCommand(@"UPDATE LinqDataTypes SET SmallIntValue = @value WHERE ID = 1", db.Parameter("value", (ushort)value)).ExecuteNonQuery();
			}
		}
	}
}
