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
		public void SqlTest([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
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
		public void SqlTypeTest([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
			{
				db.BeginTransaction();

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
