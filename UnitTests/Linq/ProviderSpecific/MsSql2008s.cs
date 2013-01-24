using System;
using System.Linq;
using BLToolkit.Data.Linq;
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
				var q =
					from p in db.Parent
					join c in db.Child on p.ParentID equals c.ParentID into g
					from c in g.DefaultIfEmpty()
					select new {p, b = Sql.AsSql((int?)c.ParentID) };

				var list = q.ToList();


				var value = db.SetCommand(@"SELECT SmallIntValue FROM LinqDataTypes WHERE ID = 1").ExecuteScalar<short>();

				db.SetCommand(@"UPDATE LinqDataTypes SET SmallIntValue = @value WHERE ID = 1", db.Parameter("value", (ushort)value)).ExecuteNonQuery();
			}
		}
	}
}
