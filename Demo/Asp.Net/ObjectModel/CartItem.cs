using System;

using BLToolkit.Common;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	[Serializable]
	public class CartItem : EntityBase
	{
		[MapField("ItemId")]     public string  ItemID;
		                         public string  Name;
		                         public int     Quantity;
		                         public decimal Price;
		                         public string  Type;
		[MapField("CategoryId")] public string  CategoryID;
		[MapField("ProductId")]  public string  ProductID;

		[MapIgnore]              public decimal Subtotal { get { return Price * Quantity; } }
 	}
}
