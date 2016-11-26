using System;
using System.Linq;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using NUnit.Framework;

namespace Data.Linq.ProviderSpecific
{
	[TestFixture]
	public class MsSql2008 : TestBase
	{
		[Test]
		public void SqlTest([IncludeDataContexts("Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
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
		public void SqlTypeTest([IncludeDataContexts("Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
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

		[TableName("Person")]
		public class Person375
		{
			[SqlFunction("getdate()", ServerSideOnly = true)]
			[MapIgnore]
			public DateTime GetDate { get; set; }

			public int PersonId;
		}

		[Test]
		public void Issue375([IncludeDataContexts("Sql2005", "Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
			{
				Assert.IsNotEmpty(db.GetTable<Person375>().ToList());
				Assert.IsNotEmpty(new SqlQuery<Person375>(db).SelectAll());

				var time = db.GetTable<Person375>().Select(_ => _.GetDate);

				Assert.AreNotEqual(default(DateTime), time);
			}
			
		}
	}
}
