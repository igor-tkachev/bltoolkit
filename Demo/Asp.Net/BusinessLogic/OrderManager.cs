using System;
using System.Collections.Generic;
using System.Transactions;

using BLToolkit.Data;

namespace PetShop.BusinessLogic
{
	using DataAccess;
	using ObjectModel;

	public class OrderManager
	{
		#region Order

		/// <summary>
		/// Inserts the order and updates the inventory stock within a transaction.
		/// </summary>
		/// <param name="order">All information about the order</param>
		public void InsertOrder(Order order)
		{
			using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
			{
				OrderAccessor accessor = Accessor;

				using (DbManager db = accessor.GetDbManager())
				{
					order.Courier = "UPS";
					order.Locale  = "US_en";
					order.ID      = accessor.Query.InsertAndGetIdentity(db, order);

					accessor.InsertStatus(db, order.ID);

					for (int i = 0; i < order.Lines.Length; i++)
					{
						OrderLineItem line = order.Lines[i];

						line.OrderID = order.ID;

						accessor.Query.Insert(line);
					}
				}

				InventoryAccessor inv = InventoryAccessor.CreateInstance();

				using (DbManager db = inv.GetDbManager())
					foreach (OrderLineItem line in order.Lines)
						inv.TakeInventory(db, line.Quantity, line.ItemID);

				ts.Complete();
			}
		}

		public List<Order> GetAllOrderList()
		{
			return Accessor.GetAllOrderList();
		}

		#endregion

		#region Accessor

		OrderAccessor Accessor
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return OrderAccessor.CreateInstance(); }
		}

		#endregion
	}
}
