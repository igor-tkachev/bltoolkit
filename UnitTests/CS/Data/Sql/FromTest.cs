using System;
using NUnit.Framework;
using BLToolkit.Data.Sql;

namespace Data.Sql
{
	[TestFixture]
	public class FromTest
	{
		public static SqlTable Order = new SqlTable
		{
			Name   = "Order",
			Fields = { new SqlField { Name = "ID" }, new SqlField { Name = "Number" } }
		};

		public static SqlTable OrderItem = new SqlTable
		{
			Name   = "OrderItem",
			Fields = { new SqlField { Name = "ID" }, new SqlField { Name = "OrderID" } }
		};

		public static SqlTable OrderItem2 = new SqlTable(OrderItem);
		public static SqlTable OrderItem3 = new SqlTable(OrderItem);
		public static SqlTable OrderItem4 = new SqlTable(OrderItem);
		public static SqlTable OrderItem5 = new SqlTable(OrderItem);

		[Test]
		public void Test1()
		{
			var sb = new SqlQuery();

			sb
				.Select
					.Field(Order["ID"])
					.Field(Order["Number"])
				.From
					.Table(Order)
				;

			sb.FinalizeAndValidate();

			Assert.AreEqual(1, sb.From.Tables.Count);
			Assert.AreEqual(0, sb.From.Tables[0].Joins.Count);
		}

		[Test]
		public void Test2()
		{
			var sb = new SqlQuery();

			sb
				.Select
					.Field(Order["ID"])
					.Field(OrderItem.All)
					.Field(OrderItem4["ID"])
				.From
					.Table(Order,
						OrderItem. Join    ().Field(Order["ID"]).Equal.Field(OrderItem["OrderID"]),
						OrderItem2.WeakJoin(),
						OrderItem3.WeakJoin(
							OrderItem5.WeakLeftJoin(),
							OrderItem4.LeftJoin    ().Field(OrderItem4["ID"]).Equal.Field(OrderItem3["ID"]))
						)
				.Where
					.Not.Field(Order["ID"]).Like("1234").Or
					.Field(Order["ID"]).Equal.Value("!%")
				.GroupBy
					.Field(Order["ID"])
				.OrderBy
					.Field(Order["ID"])
				;

			Assert.AreEqual(1, sb.From.Tables.Count);
			Assert.AreEqual(3, sb.From.Tables[0].Joins.Count);
			Assert.AreEqual(2, sb.From.Tables[0].Joins[2].Table.Joins.Count);

			sb.FinalizeAndValidate();

			Assert.AreEqual(2, sb.From.Tables[0].Joins.Count);
			Assert.AreEqual(1, sb.From.Tables[0].Joins[1].Table.Joins.Count);
		}
	}
}
