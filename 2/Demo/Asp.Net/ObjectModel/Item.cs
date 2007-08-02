using System;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace PetShop.ObjectModel
{
	public class Item : EntityBase
	{
		[MapField("ItemId"), PrimaryKey]        public string     ID;

		[MapField("ProductId")]                 public string     ProductID;
		[MapField("Supplier")]                  public int?       SupplierID;

		[MapField("ListPrice")]                 public decimal?   Price;
		                                        public decimal?   UnitCost;
		                                        public ItemStatus Status;
		                                        public string     Name;
		                                        public string     Image;

		[MapField("ProductName"), NonUpdatable] public string     ProductName;
		[MapField("Qty"),         NonUpdatable] public int        Quantity;
		[MapField("CategoryId"),  NonUpdatable] public string     CategoryID;
	}
}
