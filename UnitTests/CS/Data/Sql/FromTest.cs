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

		public static Table OrderItem2 = new Table(OrderItem);
		public static Table OrderItem3 = new Table(OrderItem);
		public static Table OrderItem4 = new Table(OrderItem);
		public static Table OrderItem5 = new Table(OrderItem);

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

			sb.FinalizeAndValidate();

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
