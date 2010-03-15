using System;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Data.Linq.Model
{
	public class NorthwindDB : DbManager
	{
		public NorthwindDB() : base("Northwind")
		{
		}

		public Table<Northwind.Category>            Category            { get { return GetTable<Northwind.Category>();            } }
		public Table<Northwind.Customer>            Customer            { get { return GetTable<Northwind.Customer>();            } }
		public Table<Northwind.Employee>            Employee            { get { return GetTable<Northwind.Employee>();            } }
		public Table<Northwind.EmployeeTerritory>   EmployeeTerritory   { get { return GetTable<Northwind.EmployeeTerritory>();   } }
		public Table<Northwind.OrderDetail>         OrderDetail         { get { return GetTable<Northwind.OrderDetail>();         } }
		public Table<Northwind.Order>               Order               { get { return GetTable<Northwind.Order>();               } }
		public Table<Northwind.Product>             Product             { get { return GetTable<Northwind.Product>();             } }
		public Table<Northwind.ActiveProduct>       ActiveProduct       { get { return GetTable<Northwind.ActiveProduct>();       } }
		public Table<Northwind.DiscontinuedProduct> DiscontinuedProduct { get { return GetTable<Northwind.DiscontinuedProduct>(); } }
		public Table<Northwind.Region>              Region              { get { return GetTable<Northwind.Region>();              } }
		public Table<Northwind.Shipper>             Shipper             { get { return GetTable<Northwind.Shipper>();             } }
		public Table<Northwind.Supplier>            Supplier            { get { return GetTable<Northwind.Supplier>();            } }
		public Table<Northwind.Territory>           Territory           { get { return GetTable<Northwind.Territory>();           } }
	}
}
