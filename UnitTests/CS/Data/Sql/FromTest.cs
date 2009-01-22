using System;
using NUnit.Framework;
using BLToolkit.Data.Sql;

namespace Data.Sql
{
	[TestFixture]
	public class FromTest
	{
		public static Table Order = new Table
		{
			Name   = "Order",
			Fields = { new Field { Name = "ID" }, new Field { Name = "Number" } }
		};

		public static Table OrderItem = new Table
		{
			Name   = "OrderItem",
			Fields = { new Field { Name = "ID" }, new Field { Name = "OrderID" } }
		};

		[Test]
		public void Test1()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Order["ID"])
					.Field(Order["Number"])
				.From
					.Table(Order)
				;

			Assert.AreEqual(1, sb.From.Tables.Count);
			Assert.AreEqual(0, sb.From.Tables[0].Joins.Count);
		}

		[Test]
		public void Test2()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Order["ID"])
					.Field(OrderItem["ID"])
				.From
					.Table(Order)
						.InnerJoin(OrderItem) //, new SqlExpression("{0} = {1}", Order["ID"], OrderItem["OrderID"])))
				;

			Assert.AreEqual(1, sb.From.Tables.Count);
			Assert.AreEqual(1, sb.From.Tables[0].Joins.Count);
		}
	}
}
