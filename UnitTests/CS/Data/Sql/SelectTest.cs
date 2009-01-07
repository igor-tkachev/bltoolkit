using System;
using NUnit.Framework;
using BLToolkit.Data.Sql;

namespace Data.Sql
{
	[TestFixture]
	public class SelectTest
	{
		static class Tables
		{
			public static Table Order = new Table
			{
				Name   = "Order",
				Fields = { new Field { Name = "ID" }, new Field { Name = "Number" } }
			};
		}

		[Test]
		public void Test1()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Tables.Order["ID"])
					.Field(Tables.Order["Number"])
				.From
					.Table(Tables.Order)
				;

			Assert.AreEqual(2, sb.Select.Columns.Count);
		}

		[Test]
		public void Test2()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Tables.Order["ID"])
					.Field(Tables.Order["Number"])
					.Field(Tables.Order["ID"])
				;

			Assert.AreEqual(2, sb.Select.Columns.Count);
		}

		[Test]
		public void Test3()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Tables.Order["ID"], "id1")
					.Field(Tables.Order["ID"])
					.Expr("{0} + {1}", Tables.Order["Number"], Tables.Order["Number"])
					.Expr(new SqlExpression("{0} + {1}", Tables.Order["Number"], Tables.Order["Number"]))
				;

			Assert.AreEqual(3, sb.Select.Columns.Count);
		}
	}
}
