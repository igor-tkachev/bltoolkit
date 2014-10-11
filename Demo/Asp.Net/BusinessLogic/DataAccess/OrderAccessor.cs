using System;
using System.Collections.Generic;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace PetShop.BusinessLogic.DataAccess
{
	using ObjectModel;

	public abstract class OrderAccessor : AccessorBase<OrderAccessor.DB, OrderAccessor>
	{
		public class DB : DbManager { public DB() : base("OrderDB") {} }

		[SqlQuery(@"INSERT INTO OrderStatus (OrderId, LineNum, Timestamp, Status) VALUES (@id, @id, GetDate(), 'P')")]
		public abstract void InsertStatus(DbManager db, int @id);

		[SqlQuery(@"
			SELECT
				o.*,
				os.Status
			FROM
				Orders o
					JOIN OrderStatus os ON os.OrderId = o.OrderId
			ORDER BY
				o.OrderId DESC")]
		public abstract List<Order> GetAllOrderList();
	}
}
