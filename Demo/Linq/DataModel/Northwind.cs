using System;
using System.Collections.Generic;
using System.Data.Linq;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Linq.Demo.DataModel
{
	public static class Northwind
	{
		[TableName("Categories")]
		public class Category
		{
			[PrimaryKey, Identity] public int    CategoryID;
			[NotNull]              public string CategoryName;
			                       public string Description;
			                       public Binary Picture;

			[Association(ThisKey="CategoryID", OtherKey="CategoryID")]
			public List<Product> Products;
		}

		[TableName("CustomerCustomerDemo")]
		public class CustomerCustomerDemo
		{
			[PrimaryKey, NotNull] public string CustomerID;
			[PrimaryKey, NotNull] public string CustomerTypeID;

			[Association(ThisKey="CustomerTypeID", OtherKey="CustomerTypeID")]
			public CustomerDemographic CustomerDemographics;

			[Association(ThisKey="CustomerID", OtherKey="CustomerID")]
			public Customer Customers;
		}

		[TableName("CustomerDemographics")]
		public class CustomerDemographic
		{
			[PrimaryKey, NotNull] public string CustomerTypeID;
			                      public string CustomerDesc;
			
			[Association(ThisKey="CustomerTypeID", OtherKey="CustomerTypeID")]
			public List<CustomerCustomerDemo> CustomerCustomerDemos;
		}

		[TableName("Customers")]
		public class Customer
		{
			[PrimaryKey] public string CustomerID;
			[NotNull]    public string CompanyName;
			             public string ContactName;
			             public string ContactTitle;
			             public string Address;
			             public string City;
			             public string Region;
			             public string PostalCode;
			             public string Country;
			             public string Phone;
			             public string Fax;

			[Association(ThisKey="CustomerID", OtherKey="CustomerID")]
			public List<CustomerCustomerDemo> CustomerCustomerDemos;

			[Association(ThisKey="CustomerID", OtherKey="CustomerID")]
			public List<Order> Orders;
		}

		[TableName("Employees")]
		public class Employee
		{
			[PrimaryKey, Identity] public int       EmployeeID;
			[NotNull]              public string    LastName;
			[NotNull]              public string    FirstName;
			                       public string    Title;
			                       public string    TitleOfCourtesy;
			                       public DateTime? BirthDate;
			                       public DateTime? HireDate;
			                       public string    Address;
			                       public string    City;
			                       public string    Region;
			                       public string    PostalCode;
			                       public string    Country;
			                       public string    HomePhone;
			                       public string    Extension;
			                       public Binary    Photo;
			                       public string    Notes;
			                       public int?      ReportsTo;
			                       public string    PhotoPath;

			[Association(ThisKey="EmployeeID", OtherKey="ReportsTo")]  public List<Employee>          Employees;
			[Association(ThisKey="EmployeeID", OtherKey="EmployeeID")] public List<EmployeeTerritory> EmployeeTerritories;
			[Association(ThisKey="EmployeeID", OtherKey="EmployeeID")] public List<Order>             Orders;
			[Association(ThisKey="ReportsTo",  OtherKey="EmployeeID")] public Employee                ReportsToEmployee;
		}

		[TableName("EmployeeTerritories")]
		public class EmployeeTerritory
		{
			[PrimaryKey]          public int    EmployeeID;
			[PrimaryKey, NotNull] public string TerritoryID;

			[Association(ThisKey="EmployeeID",  OtherKey="EmployeeID")]  public Employee  Employee;
			[Association(ThisKey="TerritoryID", OtherKey="TerritoryID")] public Territory Territory;
		}

		[TableName("Order Details")]
		public class OrderDetail
		{
			[PrimaryKey] public int     OrderID;
			[PrimaryKey] public int     ProductID;
			             public decimal UnitPrice;
			             public short   Quantity;
			             public float   Discount;

			[Association(ThisKey="OrderID",   OtherKey="OrderID",   CanBeNull=false)] public Order   Order;
			[Association(ThisKey="ProductID", OtherKey="ProductID", CanBeNull=false)] public Product Product;
		}

		[TableName("Orders")]
		public class Order
		{
			[PrimaryKey, Identity] public int       OrderID;
			                       public string    CustomerID;
			                       public int?      EmployeeID;
			                       public DateTime? OrderDate;
			                       public DateTime? RequiredDate;
			                       public DateTime? ShippedDate;
			                       public int?      ShipVia;
			                       public decimal   Freight;
			                       public string    ShipName;
			                       public string    ShipAddress;
			                       public string    ShipCity;
			                       public string    ShipRegion;
			                       public string    ShipPostalCode;
			                       public string    ShipCountry;

			[Association(ThisKey="OrderID",    OtherKey="OrderID",    CanBeNull=false)] public List<OrderDetail> OrderDetails;
			[Association(ThisKey="CustomerID", OtherKey="CustomerID", CanBeNull=false)] public Customer          Customer;
			[Association(ThisKey="EmployeeID", OtherKey="EmployeeID")]                  public Employee          Employee;
			[Association(ThisKey="ShipVia",    OtherKey="ShipperID")]                   public Shipper           Shipper;
		}

		[TableName("Products")]
		[InheritanceMapping(Code="True",  Type=typeof(DiscontinuedProduct))]
		[InheritanceMapping(Code="False", Type=typeof(ActiveProduct))]
		public abstract class Product
		{
			[PrimaryKey, Identity]                      public int      ProductID;
			[NotNull]                                   public string   ProductName;
			                                            public int?     SupplierID;
			                                            public int?     CategoryID;
			                                            public string   QuantityPerUnit;
			                                            public decimal? UnitPrice;
			                                            public short?   UnitsInStock;
			                                            public short?   UnitsOnOrder;
			                                            public short?   ReorderLevel;
			[MapField(IsInheritanceDiscriminator=true)] public bool     Discontinued;

			[Association(ThisKey="ProductID",  OtherKey="ProductID")]
			public List<OrderDetail> OrderDetails;

			[Association(ThisKey="CategoryID", OtherKey="CategoryID", CanBeNull=false)]
			public Category Category;

			[Association(ThisKey="SupplierID", OtherKey="SupplierID", CanBeNull=false)]
			public Supplier Supplier;
		}

		public class ActiveProduct       : Product {}
		public class DiscontinuedProduct : Product {}

		[TableName("Region")]
		public class Region
		{
			[PrimaryKey] public int    RegionID;
			[NotNull]    public string RegionDescription;

			[Association(ThisKey="RegionID", OtherKey="RegionID")]
			public List<Territory> Territories;
		}

		[TableName("Shippers")]
		public class Shipper
		{
			[PrimaryKey, Identity] public int    ShipperID;
			[NotNull]              public string CompanyName;
			                       public string Phone;

			[Association(ThisKey="ShipperID", OtherKey="ShipVia")]
			public List<Order> Orders;
		}

		[TableName("Suppliers")]
		public class Supplier
		{
			[PrimaryKey, Identity] public int    SupplierID;
			[NotNull]              public string CompanyName;
			                       public string ContactName;
			                       public string ContactTitle;
			                       public string Address;
			                       public string City;
			                       public string Region;
			                       public string PostalCode;
			                       public string Country;
			                       public string Phone;
			                       public string Fax;
			                       public string HomePage;

			[Association(ThisKey="SupplierID", OtherKey="SupplierID")]
			public List<Product> Products;
		}

		[TableName("Territories")]
		public class Territory
		{
			[PrimaryKey, NotNull] public string TerritoryID;
			[NotNull]             public string TerritoryDescription;
			                      public int    RegionID;

			[Association(ThisKey="TerritoryID", OtherKey="TerritoryID")]
			public List<EmployeeTerritory> EmployeeTerritories;

			[Association(ThisKey="RegionID", OtherKey="RegionID")]
			public Region Region;
		}
	}
}
