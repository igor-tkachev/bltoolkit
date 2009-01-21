using System;
using NUnit.Framework;
using BLToolkit.Data.Sql;

namespace Data.Sql
{
	[TestFixture]
	public class SelectTest
	{
		public static Table Order = new Table
		{
			Name   = "Order",
			Fields = { new Field { Name = "ID" }, new Field { Name = "Number" } }
		};

		[Test]
		public void Test1()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Order["ID"])
					.Field(Order["Number"])
				;

			Assert.AreEqual(2, sb.Select.Columns.Count);
		}

		[Test]
		public void Test2()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Order["ID"])
					.Field(Order["Number"])
					.Field(Order["ID"])
				;

			Assert.AreEqual(2, sb.Select.Columns.Count);
		}

		[Test]
		public void Test3()
		{
			var sb = new SqlBuilder();

			sb
				.Select
					.Field(Order["ID"], "id1")
					.Field(Order["ID"])
					.Expr("{0} + {1}", Order["Number"], Order["Number"])
					.Expr(new SqlExpression("{0} + {1}", Order["Number"], Order["Number"]))
				;

			Assert.AreEqual(3, sb.Select.Columns.Count);
		}
	}
}
