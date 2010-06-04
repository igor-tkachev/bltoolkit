using System;
using System.Collections.Generic;
using System.Text;

namespace PetShop.BusinessLogic
{
	using DataAccess;
	using ObjectModel;

	public class ProductManager
	{
		#region Product

		public Product GetProduct(string id)
		{
			return Accessor.Query.SelectByKey<Product>(id) ?? new Product();
		}

		public IList<Product> GetProductListByCategoryID(string id)
		{
			return Accessor.GetProductListByCategoryID(id);
		}

		public IList<Product> SearchProducts(string[] keyWords)
		{
			return Accessor.Query.SelectByKeyWords<Product>(keyWords);
		}

		#endregion

		#region Item

		public IList<Item> GetItemListByProductID(string id)
		{
			return Accessor.GetItemListByProductID(id);
		}

		public Item GetItem(string itemId)
		{
			return Accessor.GetItem(itemId);
		}

		public List<Item> GetAllItemList()
		{
			return Accessor.GetAllItemList();
		}

		#endregion

		#region Category

		public IList<Category> GetCategoryList()
		{
			return Accessor.Query.SelectAll<Category>();
		}

		public Category GetCategory(string id)
		{
			return Accessor.GetCategory(id);
		}

		public List<Category> SearchCategories(string[] keyWords)
		{
			return Accessor.Query.SelectByKeyWords<Category>(keyWords);
		}

		#endregion

		#region Accessor

		ProductAccessor Accessor
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return ProductAccessor.CreateInstance(); }
		}

		#endregion
	}
}
