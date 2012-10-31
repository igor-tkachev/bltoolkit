using System;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace PetShop.BusinessLogic.DataAccess
{
	using ObjectModel;

	public abstract class InventoryAccessor : AccessorBase<InventoryAccessor.DB, InventoryAccessor>
	{
		public class DB : DbManager { public DB() : base("InventoryDB") {} }

		[SqlQuery(@"UPDATE Inventory SET Qty = Qty - @qty WHERE ItemId = @itemId")]
		public abstract void TakeInventory(DbManager db, int @qty, string @itemId);
	}
}
