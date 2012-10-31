using System;

using NUnit.Framework;

using BLToolkit.Data.Sql;
using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Data.Sql
{
	[TestFixture]
	public class DefinitionTest
	{
		#region Table definition

		static class Tables
		{
			public static readonly SqlTable Order = new SqlTable
			{
				Name         = "Order",
				PhysicalName = "Order",
				Fields       =
				{
					new SqlField { Name = "ID", PhysicalName = "OrderID" }
				},
				Joins  =
				{
					new Join
					{
						TableName = "OrderItem",
						JoinOns   = { new JoinOn { Field = "ID", OtherField = "OrderID" } }
					}
				}
			};

			public static readonly SqlTable OrderItem = new SqlTable
			{
				Name         = "OrderItem",
				PhysicalName = "OrderItem",
				Alias        = "oi",
				Fields       =
				{
					new SqlField { Name = "OrderID" },
					new SqlField { Name = "ID", PhysicalName = "OrderItemID" }
				}
			};
		}

		static class Xml
		{
			static readonly ExtensionList _ext = TypeExtension.GetExtensions("DefinitionData.xml");

			public static readonly SqlTable Order     = new SqlTable(_ext, "Order");
			public static readonly SqlTable Order2    = new SqlTable(_ext, "Order2") { Name = "Order", PhysicalName = "Order" };
			public static readonly SqlTable Order3    = new SqlTable(_ext, "Order3") { Name = "Order", PhysicalName = "Order" };
			public static readonly SqlTable OrderItem = new SqlTable(_ext, "OrderItem");
		}

		public class Types
		{
			public class Order
			{
				[MapField("OrderID")] public int ID;
			}

			public class OrderItem
			{
				                          public int OrderID;
				[MapField("OrderItemID")] public int ID;
			}

			public static SqlTable OrderTable = new SqlTable<Order>
			{
				Joins =
				{
					new Join
					{
						TableName = "OrderItem",
						JoinOns   = { new JoinOn { Field = "ID", OtherField = "OrderID" } }
					}
				}
			};
			public static SqlTable OrderItemTable = new SqlTable<OrderItem> { Alias = "oi" };
		}

		#endregion

		static void CompareTables(SqlTable t1, SqlTable t2)
		{
			Assert.AreEqual(t1.Name,         t2.Name);
			Assert.AreEqual(t1.PhysicalName, t2.PhysicalName);
			Assert.AreEqual(t1.Alias,        t2.Alias);
			Assert.AreEqual(t1.Database,     t2.Database);
			Assert.AreEqual(t1.Fields.Count, t2.Fields.Count);
			Assert.AreEqual(t1.Joins. Count, t2.Joins. Count);

			foreach (var field in t1.Fields.Values)
			{
				Assert.AreEqual(field.PhysicalName, t2[field.Name].PhysicalName);
			}

			for (var i = 0; i < t1.Joins.Count; i++)
			{
				var j1 = t1.Joins[i];
				var j2 = t2.Joins[i];

				Assert.AreEqual(j1.TableName,     j2.TableName);
				Assert.AreEqual(j1.Alias,         j2.Alias);
				Assert.AreEqual(j1.JoinOns.Count, j2.JoinOns.Count);

				for (var j = 0; j < j1.JoinOns.Count; j++)
				{
					Assert.AreEqual(j1.JoinOns[j].Field,      j2.JoinOns[j].Field);
					Assert.AreEqual(j1.JoinOns[j].OtherField, j2.JoinOns[j].OtherField);
					Assert.AreEqual(j1.JoinOns[j].Expression, j2.JoinOns[j].Expression);
				}
			}
		}

		[Test]
		public void Test()
		{
			CompareTables(Tables.Order,     Xml.Order);
			CompareTables(Tables.Order,     Xml.Order2);
			CompareTables(Tables.Order,     Xml.Order3);
			CompareTables(Tables.OrderItem, Xml.OrderItem);
			CompareTables(Tables.Order,     Types.OrderTable);
			CompareTables(Tables.OrderItem, Types.OrderItemTable);
		}
	}
}
