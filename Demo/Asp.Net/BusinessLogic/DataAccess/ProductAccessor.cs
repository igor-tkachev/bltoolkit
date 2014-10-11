using System;
using System.Collections.Generic;

using BLToolkit.Aspects;
using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace PetShop.BusinessLogic.DataAccess
{
	using ObjectModel;

	public abstract class ProductAccessor : AccessorBase<ProductAccessor.DB, ProductAccessor>
	{
		public class DB : DbManager { public DB() : base("ProductDB") {} }

		#region Item

		const string _itemQuery = @"
			SELECT
				i.*,
				n.Qty,
				p.Name as ProductName,
				p.CategoryId
			FROM
				Item i
					JOIN Product   p ON p.ProductId = i.ProductId
					JOIN Inventory n ON n.ItemId    = i.ItemId";

		[SqlQuery(_itemQuery + @" WHERE i.ProductId = @id")]
		public abstract IList<Item> GetItemListByProductID(string @id);

		[SqlQuery(_itemQuery + @" WHERE i.ItemId = @itemID")]
		public abstract Item GetItem(string @itemID);

		[SqlQuery(_itemQuery + @" ORDER BY i.Name")]
		public abstract List<Item> GetAllItemList();

		#endregion

		#region Product

		[Cache(MaxMinutes = 60)]
		[SqlQuery(@"SELECT * FROM Product WHERE CategoryId = @id")]
		public abstract IList<Product> GetProductListByCategoryID(string @id);

		#endregion

		#region Categoty

		// This method needs to be cached, so we can't call Query.SelectByKey directly from the manager class.
		//
		[Cache(MaxMinutes=60)]
		public virtual Category GetCategory(string id)
		{
			return Query.SelectByKey<Category>(id);
		}

		#endregion

		#region Inventory

		[SqlQuery(@"SELECT Qty FROM Inventory WHERE ItemId = @itemId")]
		public abstract int CurrentQtyInStock(string @itemId);

		#endregion
	}
}
