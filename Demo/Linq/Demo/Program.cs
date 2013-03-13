using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

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
			InheritanceTest1();
			InheritanceTest2();
			StringLengthTest();
			StringCompareTest();
			MathRoundCompare1Test();
			MathRoundCompare2Test();
			MathRoundCompare3Test();
			SimpleSelectTest();
			InsertTest1();
			InsertTest2();
			MultipleInsertTest1();
			MultipleInsertTest2();
			InsertWithIdentityTest1();
			InsertWithIdentityTest2();
			UpdateTest1();
			UpdateTest2();
			UpdateTest3();
			SelfUpdateTest();
			DeleteTest1();
			DeleteTest2();
			SqlLengthTest();
			SqlRoundCompare1Test();
			SqlSelectLengthTest();
			SqlSelectLengthAsSqlTest();
			RoundToEvenTest();
			MethodExpressionTest();
			CompiledQueryTest();
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
					select new Order
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
					select g.Key.CategoryName;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void InheritanceTest1()
		{
			using (var db = new NorthwindDB())
			{
				var query = from p in db.DiscontinuedProduct select p;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void InheritanceTest2()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from p in db.Product
					where p is DiscontinuedProduct
					select p;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void StringLengthTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					where c.ContactName.Length > 5
					select c.ContactName;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void StringCompareTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					where c.ContactName.CompareTo("John") > 0
					select c.ContactName;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void MathRoundCompare1Test()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					where Math.Round(o.Freight.Value) >= 10
					select o.Freight;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void MathRoundCompare2Test()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					where Math.Round(o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)) >= 10
					select o.Freight;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void MathRoundCompare3Test()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					let sum = o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
					where Math.Round(sum) >= 10
					select o.Freight;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void SimpleSelectTest()
		{
			using (var db = new NorthwindDB())
			{
				var value = db.Select(() => Sql.CurrentTimestamp);

				Console.WriteLine(value);
			}
		}

		static void InsertTest1()
		{
			using (var db = new NorthwindDB())
			{
				var value = db.Employee.Insert(() => new Employee
				{
					FirstName = "John",
					LastName  = "Shepard",
					Title     = "Spectre",
					HireDate  = Sql.CurrentTimestamp
				});

				Console.WriteLine(value);

				db.Employee.Delete(e => e.LastName == "Shepard");
			}
		}

		static void InsertTest2()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db
						.Into(db.Employee)
							.Value(e => e.FirstName, "John")
							.Value(e => e.LastName,  "Shepard")
							.Value(e => e.Title,     "Spectre")
							.Value(e => e.HireDate,  () => Sql.CurrentTimestamp)
						.Insert();

				Console.WriteLine(value);

				db.Employee.Delete(e => e.LastName == "Shepard");
			}
		}

		static void MultipleInsertTest1()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Region
						.Where(r => r.RegionID > 2)
						.Insert(db.Region, r => new Region()
						{
							RegionID          = r.RegionID + 100,
							RegionDescription = "Copy Of " + r.RegionDescription
						});

				Console.WriteLine(value);

				db.Region.Delete(r => r.RegionDescription.StartsWith("Copy Of "));
			}
		}

		static void MultipleInsertTest2()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Region
						.Where(r => r.RegionID > 2)
						.Into(db.Region)
							.Value(r => r.RegionID,          r => r.RegionID + 100)
							.Value(r => r.RegionDescription, r => "Copy Of " + r.RegionDescription)
						.Insert();

				Console.WriteLine(value);

				db.Region.Delete(r => r.RegionDescription.StartsWith("Copy Of "));
			}
		}

		static void InsertWithIdentityTest1()
		{
			using (var db = new NorthwindDB())
			{
				var value = db.Employee.InsertWithIdentity(() => new Employee
				{
					FirstName = "John",
					LastName  = "Shepard",
					Title     = "Spectre",
					HireDate  = Sql.CurrentTimestamp
				});

				Console.WriteLine(value);

				db.Employee.Delete(e => e.EmployeeID == Convert.ToInt32(value));
			}
		}

		static void InsertWithIdentityTest2()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db
						.Into(db.Employee)
							.Value(e => e.FirstName, "John")
							.Value(e => e.LastName,  "Shepard")
							.Value(e => e.Title,     () => "Spectre")
							.Value(e => e.HireDate,  () => Sql.CurrentTimestamp)
						.InsertWithIdentity();

				Console.WriteLine(value);

				db.Employee.Delete(e => e.EmployeeID == Convert.ToInt32(value));
			}
		}

		static void UpdateTest1()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Employee
						.Update(
							e => e.Title == "Spectre",
							e => new Employee
							{
								Title = "Commander"
							});

				Console.WriteLine(value);
			}
		}

		static void UpdateTest2()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Employee
						.Where(e => e.Title == "Spectre")
						.Update(e => new Employee
						{
							Title = "Commander"
						});

				Console.WriteLine(value);
			}
		}

		static void UpdateTest3()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Employee
						.Where(e => e.Title == "Spectre")
						.Set(e => e.Title, "Commander")
						.Update();

				Console.WriteLine(value);
			}
		}

		static void SelfUpdateTest()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Employee
						.Where(e => e.Title == "Spectre")
						.Set(e => e.HireDate, e => e.HireDate.Value.AddDays(10))
						.Update();

				Console.WriteLine(value);
			}
		}

		static void DeleteTest1()
		{
			using (var db = new NorthwindDB())
			{
				var value = db.Employee.Delete(e => e.Title == "Spectre");

				Console.WriteLine(value);
			}
		}

		static void DeleteTest2()
		{
			using (var db = new NorthwindDB())
			{
				var value =
					db.Employee
						.Where(e => e.Title == "Spectre")
						.Delete();

				Console.WriteLine(value);
			}
		}

		static void SqlLengthTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					where Sql.Length(c.ContactName) > 5
					select c.ContactName;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void SqlRoundCompare1Test()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					where Sql.Round(o.Freight) >= 10
					select o.Freight;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void SqlSelectLengthTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					select Sql.Length(c.ContactName);

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static void SqlSelectLengthAsSqlTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					select Sql.AsSql(Sql.Length(c.ContactName));

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static decimal? RoundToEven(decimal? value)
		{
			return
				value - Sql.Floor(value) == 0.5m && Sql.Floor(value) % 2 == 0?
					Sql.Floor(value) :
					Sql.Round(value);
		}

		static void RoundToEvenTest()
		{
			Expressions.MapMember<decimal?,decimal?>(
				value => RoundToEven(value),
				value =>
					value - Sql.Floor(value) == 0.5m && Sql.Floor(value) % 2 == 0?
						Sql.Floor(value) :
						Sql.Round(value));

			using (var db = new NorthwindDB())
			{
				var query =
					from o in db.Order
					let sum = o.OrderDetails.Sum(d => d.Quantity * d.UnitPrice)
					where RoundToEven(sum) >= 10
					select o.Freight;

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		[MethodExpression("OrderCountExpression")]
		static int OrderCount(Customer customer, string region)
		{
			throw new InvalidOperationException();
		}

		static Expression<Func<Customer,string,int>> OrderCountExpression()
		{
			return (customer, region) => customer.Orders.Count(o => o.ShipRegion == region);
		}

		static void MethodExpressionTest()
		{
			using (var db = new NorthwindDB())
			{
				var query =
					from c in db.Customer
					select new
					{
						sum1 = OrderCount(c, "SP"),
						sum2 = OrderCount(c, "NM")
					};

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}

		static Func<NorthwindDB,int,IEnumerable<Employee>> _query =
			CompiledQuery.Compile<NorthwindDB,int,IEnumerable<Employee>>((db, n) =>
				from e in db.Employee
				where e.EmployeeID > n
				orderby e.LastName, e.FirstName
				select e
			);

		static void CompiledQueryTest()
		{
			using (var db = new NorthwindDB())
			{
				var query = _query(db, 5);

				foreach (var item in query)
				{
					Console.WriteLine(item);
				}
			}
		}
	}
}

