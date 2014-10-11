using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	[Serializable]
	[TableName("LineItem")]
	public class OrderLineItem : EntityBase
	{
		[MapField("OrderId"), PrimaryKey(1)] public int     OrderID;
		[MapField("LineNum"), PrimaryKey(2)] public int     Line;

		[MapField("ItemId")]                 public string  ItemID;
		                                     public int     Quantity;
		[MapField("UnitPrice")]              public decimal Price;

		[MapIgnore] public decimal Subtotal { get { return Quantity * Price; } }
	}
}
