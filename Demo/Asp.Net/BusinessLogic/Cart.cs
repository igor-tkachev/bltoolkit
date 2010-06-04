using System;
using System.Collections.Generic;

namespace PetShop.BusinessLogic
{
	using ObjectModel;

	[Serializable]
	public class Cart
	{
		/// <summary>
		/// Calculate the total for all the cartItems in the Cart
		/// </summary>
		public decimal Total
		{
			get
			{
				decimal total = 0;

				foreach (CartItem item in _items.Values)
					total += item.Subtotal;

				return total;
			}
		}

		/// <summary>
		/// Update the quantity for item that exists in the cart
		/// </summary>
		/// <param name="itemId">Item Id</param>
		/// <param name="qty">Quantity</param>
		public void SetQuantity(string itemId, int qty)
		{
			_items[itemId].Quantity = qty;
		}

		/// <summary>
		/// Return the number of unique items in cart
		/// </summary>
		public int Count
		{
			get { return _items.Count; }
		}

		/// <summary>
		/// Add an item to the cart.
		/// When ItemId to be added has already existed, this method will update the quantity instead.
		/// </summary>
		/// <param name="itemId">Item Id of item to add</param>
		public void Add(string itemId)
		{
			CartItem cartItem;

			if (!_items.TryGetValue(itemId, out cartItem))
			{
				Item item = new ProductManager().GetItem(itemId);

				if (item != null)
				{
					cartItem = new CartItem();

					cartItem.ItemID     = itemId;
					cartItem.Name       = item.ProductName;
					cartItem.Price      = (decimal)item.Price;
					cartItem.Type       = item.Name;
					cartItem.CategoryID = item.CategoryID;
					cartItem.ProductID  = item.ProductID;

					_items.Add(itemId, cartItem);
				}
			}

			cartItem.Quantity++;
		}

		/// <summary>
		/// Add an item to the cart.
		/// When ItemId to be added has already existed, this method will update the quantity instead.
		/// </summary>
		/// <param name="item">Item to add</param>
		public void Add(CartItem item)
		{
			CartItem cartItem;

			if (!_items.TryGetValue(item.ItemID, out cartItem))
				_items.Add(item.ItemID, item);
			else
				cartItem.Quantity += item.Quantity;
		}

		/// <summary>
		/// Remove item from the cart based on <paramref name="itemId"/>
		/// </summary>
		/// <param name="itemId">ItemId of item to remove</param>
		public void Remove(string itemId)
		{
			_items.Remove(itemId);
		}

		// Internal storage for a cart	  
		private Dictionary<string, CartItem> _items = new Dictionary<string, CartItem>();

		/// <summary>
		/// Returns all items in the cart. Useful for looping through the cart.
		/// </summary>
		/// <returns>Collection of CartItemInfo</returns>
		public ICollection<CartItem> Items
		{
			get { return _items.Values; }
		}

		/// <summary>
		/// Method to convert all cart items to order line items
		/// </summary>
		/// <returns>A new array of order line items</returns>
		public OrderLineItem[] GetOrderLineItems()
		{
			OrderLineItem[] items = new OrderLineItem[_items.Count];

			int lineNum = 0;

			foreach (CartItem item in _items.Values)
			{
				OrderLineItem line = new OrderLineItem();

				items[lineNum] = line;

				line.ItemID   = item.ItemID;
				line.Line     = ++lineNum;
				line.Quantity = item.Quantity;
				line.Price    = item.Price;
			}

			return items;
		}

		/// <summary>
		/// Clear the cart
		/// </summary>
		public void Clear()
		{
			_items.Clear();
		}
	}
}
