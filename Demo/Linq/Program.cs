using System;
using System.Diagnostics;
using System.Linq;

using BLToolkit.Data;

namespace Linq.Demo
{
	using DataModel;

	static class Program
	{
		static void Main()
		{
			DbManager.TraceSwitch = new TraceSwitch("DbManager", "DbManager trace switch", "Info");

			FirstTest();
			CountTest();
			SingleTableTest();
			SelectManyTest();
			InnerJoinTest();
			LeftJoinTest();
			AssociationInnerJoinTest();
			AssociationCountTest();
			AssociationObjectTest();
			MultiLevelAssociationTest();
			CompareAssociationTest();
			GroupByAssociationTest();
		}

		static void FirstTest()
		{
			using (var db = new NorthwindDB())
			{
				var query = db.Employee;

				foreach (var employee in query)
				{
					Console.WriteLine("{0} {1}", employee.EmployeeID, employee.FirstName);
				}
			}
		}

		static void CountTest()
		{
			using (var db = new NorthwindDB())
			{
				int count = db.Employee.Count();

				Console.WriteLine(count);
			}
		}

		static void SingleTableTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from e in db.Employee
					where e.EmployeeID > 5
					orderby e.LastName, e.FirstName
					select e;

				foreach (var employee in query)
				{
					Console.WriteLine("{0} {1}, {2}", employee.EmployeeID, employee.LastName, employee.FirstName);
				}
			}
		}

		static void SelectManyTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Category
					from p in db.Product
					where p.CategoryID == c.CategoryID
					select new
					{
						c.CategoryName,
						p.ProductName
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void InnerJoinTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					join c in db.Category on p.CategoryID equals c.CategoryID
					select new
					{
						c.CategoryName,
						p.ProductName
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void LeftJoinTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					join c in db.Category on p.CategoryID equals c.CategoryID into g
					from c in g.DefaultIfEmpty()
					select new
					{
						c.CategoryName,
						p.ProductName
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void AssociationInnerJoinTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					select new
					{
						p.Category.CategoryName,
						p.ProductName
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void AssociationCountTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					select new
					{
						p.OrderDetails.Count,
						p.ProductName
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void AssociationObjectTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					select new Northwind.Order
					{
						OrderID  = o.OrderID,
						Customer = o.Customer
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void MultiLevelAssociationTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.OrderDetail
					select new
					{
						o.Product.ProductName,
						o.Order.OrderID,
						o.Order.Employee.ReportsToEmployee.Region
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void CompareAssociationTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					from t in db.EmployeeTerritory
					where o.Employee == t.Employee
					select new
					{
						o.OrderID,
						o.EmployeeID,
						t.TerritoryID
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void GroupByAssociationTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					group p by p.Category into g
					where g.Count() == 12
					select g.Key.CategoryID;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}
	}
}
