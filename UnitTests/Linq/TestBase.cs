using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using BLToolkit.Data.DataProvider;
using BLToolkit.Common;
using BLToolkit.Data;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	public class TestBase
	{
		static TestBase()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				if (args.Name.IndexOf("Sybase.AdoNet2.AseClient") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Sybase\Sybase.AdoNet2.AseClient.dll");
				if (args.Name.IndexOf("Oracle.DataAccess") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Oracle\Oracle.DataAccess.dll");
				if (args.Name.IndexOf("IBM.Data.DB2") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\IBM\IBM.Data.DB2.dll");
				if (args.Name.IndexOf("Mono.Security") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\PostgreSql\Mono.Security.dll");

				return null;
			};

			DbManager.TraceSwitch = new TraceSwitch("DbManager", "DbManager trace switch", "Info");
		}

		static readonly List<string> _configurations = new List<string>
		{
			"Sql2008",
			"Sql2005",
			ProviderName.SqlCe,
			ProviderName.DB2,
			ProviderName.Informix,
			"Oracle",
			ProviderName.Firebird,
			ProviderName.PostgreSQL,
			ProviderName.MySql,
			ProviderName.Sybase,
			ProviderName.SQLite,
			ProviderName.Access,
		};

		protected void ForEachProvider(Action<TestDbManager> func)
		{
			ForEachProvider(Array<string>.Empty, func);
		}

		protected void ForEachProvider(string[] exceptList, Action<TestDbManager> func)
		{
			for (var i = 0; i < _configurations.Count; i++)
			{
				if (exceptList.Contains(_configurations[i]))
					continue;

				Debug.WriteLine(_configurations[i], "Provider ");

				var reThrow = false;

				try
				{
					using (var db = new TestDbManager(_configurations[i]))
					{
						//var conn = db.Connection;

						reThrow = true;
						func(db);
					}
				}
				catch
				{
					if (reThrow) throw;

					_configurations.RemoveAt(i);
					i--;
				}
			}
		}

		protected void Not0ForEachProvider(Func<TestDbManager, int> func)
		{
			ForEachProvider(db => Assert.Less(0, func(db)));
		}

		protected void TestPerson(int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(db =>
			{
				var person = func(db).ToList().Where(p => p.ID == id).First();

				Assert.AreEqual(id,        person.ID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestPerson(1, "John", func);
		}

		protected void TestOnePerson(string[] exceptList, int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(exceptList, db =>
			{
				var list = func(db).ToList();

				Assert.AreEqual(1, list.Count);

				var person = list[0];

				Assert.AreEqual(id,        person.ID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestOnePerson(int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(Array<string>.Empty, id, firstName, func);
		}

		protected void TestOneJohn(string[] exceptList, Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(exceptList, 1, "John", func);
		}

		protected void TestOneJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(Array<string>.Empty, 1, "John", func);
		}

		private   List<LinqDataTypes> _types;
		protected List<LinqDataTypes>  Types
		{
			get
			{
				if (_types == null)
					using (var db = new TestDbManager("Sql2008"))
						_types = db.Types.ToList();
				return _types;
			}
		}

		private   List<Person> _person;
		protected List<Person>  Person
		{
			get
			{
				if (_person == null)
					using (var db = new TestDbManager("Sql2008"))
						_person = db.Person.ToList();

				return _person;
			}
		}

		#region Parent/Child Model

		private   List<Parent> _parent;
		protected List<Parent>  Parent
		{
			get
			{
				if (_parent == null)
					using (var db = new TestDbManager("Sql2008"))
					{
						_parent = db.Parent.ToList();
						db.Close();

						foreach (var p in _parent)
							p.Children = Child.Where(c => c.ParentID == p.ParentID).ToList();
					}

				return _parent;
			}
		}

		private   List<Parent1> _parent1;
		protected List<Parent1>  Parent1
		{
			get
			{
				return _parent1 ?? (_parent1 = Parent.Select(p => new Parent1 { ParentID = p.ParentID, Value1 = p.Value1 }).ToList());
			}
		}

		private   List<ParentInheritanceBase> _parentInheritance;
		protected List<ParentInheritanceBase>  ParentInheritance
		{
			get
			{
				return _parentInheritance ?? (_parentInheritance = Parent.Select(p =>
					p.Value1       == null ? new ParentInheritanceNull  { ParentID = p.ParentID } :
					p.Value1.Value == 1    ? new ParentInheritance1     { ParentID = p.ParentID, Value1 = p.Value1.Value } :
					 (ParentInheritanceBase) new ParentInheritanceValue { ParentID = p.ParentID, Value1 = p.Value1.Value }
				).ToList());
			}
		}

		private   List<Child> _child;
		protected List<Child>  Child
		{
			get
			{
				if (_child == null)
					using (var db = new TestDbManager("Sql2008"))
					{
						_child = db.Child.ToList();
						db.Clone();

						foreach (var ch in _child)
						{
							ch.Parent        = Parent. Single(p => p.ParentID == ch.ParentID);
							ch.Parent1       = Parent1.Single(p => p.ParentID == ch.ParentID);
							ch.ParentID2     = new Parent3 { ParentID2 = ch.Parent.ParentID, Value1 = ch.Parent.Value1 };
							ch.GrandChildren = GrandChild.Where(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID).ToList();
						}
					}

				return _child;
			}
		}

		private   List<GrandChild> _grandChild;
		protected List<GrandChild>  GrandChild
		{
			get
			{
				if (_grandChild == null)
					using (var db = new TestDbManager("Sql2008"))
					{
						_grandChild = db.GrandChild.ToList();
						db.Close();

						foreach (var ch in _grandChild)
							ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
					}

				return _grandChild;
			}
		}

		private   List<GrandChild1> _grandChild1;
		protected List<GrandChild1>  GrandChild1
		{
			get
			{
				if (_grandChild1 == null)
					using (var db = new TestDbManager("Sql2008"))
					{
						_grandChild1 = db.GrandChild1.ToList();

						foreach (var ch in _grandChild1)
							ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
					}

				return _grandChild1;
			}
		}

		#endregion

		#region Northwind

		private List<Northwind.Category> _category;
		public  List<Northwind.Category>  Category
		{
			get
			{
				if (_category == null)
					using (var db = new NorthwindDB())
						_category = db.Category.ToList();
				return _category;
			}
		}

		private List<Northwind.Customer> _customer;
		public  List<Northwind.Customer>  Customer
		{
			get
			{
				if (_customer == null)
				{
					using (var db = new NorthwindDB())
						_customer = db.Customer.ToList();

					foreach (var c in _customer)
						c.Orders = (from o in Order where o.CustomerID == c.CustomerID select o).ToList();
				}

				return _customer;
			}
		}

		private List<Northwind.Employee> _employee;
		public  List<Northwind.Employee>  Employee
		{
			get
			{
				if (_employee == null)
				{
					using (var db = new NorthwindDB())
					{
						_employee = db.Employee.ToList();

						foreach (var employee in _employee)
						{
							employee.Employees         = (from e in _employee where e.ReportsTo  == employee.EmployeeID select e).ToList();
							employee.ReportsToEmployee = (from e in _employee where e.EmployeeID == employee.ReportsTo  select e).SingleOrDefault();
						}
					}
				}

				return _employee;
			}
		}

		private List<Northwind.EmployeeTerritory> _employeeTerritory;
		public  List<Northwind.EmployeeTerritory>  EmployeeTerritory
		{
			get
			{
				if (_employeeTerritory == null)
					using (var db = new NorthwindDB())
						_employeeTerritory = db.EmployeeTerritory.ToList();
				return _employeeTerritory;
			}
		}

		private List<Northwind.OrderDetail> _orderDetail;
		public  List<Northwind.OrderDetail>  OrderDetail
		{
			get
			{
				if (_orderDetail == null)
					using (var db = new NorthwindDB())
						_orderDetail = db.OrderDetail.ToList();
				return _orderDetail;
			}
		}

		private List<Northwind.Order> _order;
		public  List<Northwind.Order>  Order
		{
			get
			{
				if (_order == null)
				{
					using (var db = new NorthwindDB())
						_order = db.Order.ToList();

					foreach (var o in _order)
					{
						o.Customer = Customer.Single(c => o.CustomerID == c.CustomerID);
						o.Employee = Employee.Single(e => o.EmployeeID == e.EmployeeID);
					}
				}

				return _order;
			}
		}

		private List<Northwind.Product> _product;
		public  List<Northwind.Product>  Product
		{
			get
			{
				if (_product == null)
					using (var db = new NorthwindDB())
						_product = db.Product.ToList();
				return _product;
			}
		}

		private List<Northwind.ActiveProduct> _activeProduct;
		public  List<Northwind.ActiveProduct>  ActiveProduct
		{
			get { return _activeProduct ?? (_activeProduct = Product.OfType<Northwind.ActiveProduct>().ToList()); }
		}

		private List<Northwind.DiscontinuedProduct> _discontinuedProduct;
		public  List<Northwind.DiscontinuedProduct>  DiscontinuedProduct
		{
			get { return _discontinuedProduct ?? (_discontinuedProduct = Product.OfType<Northwind.DiscontinuedProduct>().ToList()); }
		}

		private List<Northwind.Region> _region;
		public  List<Northwind.Region>  Region
		{
			get
			{
				if (_region == null)
					using (var db = new NorthwindDB())
						_region = db.Region.ToList();
				return _region;
			}
		}

		private List<Northwind.Shipper> _shipper;
		public  List<Northwind.Shipper>  Shipper
		{
			get
			{
				if (_shipper == null)
					using (var db = new NorthwindDB())
						_shipper = db.Shipper.ToList();
				return _shipper;
			}
		}

		private List<Northwind.Supplier> _supplier;
		public  List<Northwind.Supplier>  Supplier
		{
			get
			{
				if (_supplier == null)
					using (var db = new NorthwindDB())
						_supplier = db.Supplier.ToList();
				return _supplier;
			}
		}

		private List<Northwind.Territory> _territory;
		public  List<Northwind.Territory>  Territory
		{
			get
			{
				if (_territory == null)
					using (var db = new NorthwindDB())
						_territory = db.Territory.ToList();
				return _territory;
			}
		}

		#endregion

		protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result)
		{
			var expectedList = expected.ToList();
			var resultList   = result.  ToList();

			Assert.AreEqual(expectedList.Count(), resultList.Count());
			Assert.AreNotEqual(0, expectedList.Count());
			Assert.AreNotEqual(0, resultList.  Count());

			var exceptExpected = resultList.  Except(expectedList).Count();
			var exceptResult   = expectedList.Except(resultList).  Count();

			if (exceptResult != 0 || exceptExpected != 0)
				for (int i = 0; i < resultList.Count; i++)
					Debug.WriteLine(string.Format("{0} {1} --- {2}", Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]));

			Assert.AreEqual(0, exceptExpected);
			Assert.AreEqual(0, exceptResult);
		}

		protected void AreSame<T>(IEnumerable<T> expected, IEnumerable<T> result)
		{
			var expectedList = expected.ToList();
			var resultList   = result.  ToList();

			Assert.AreEqual(expectedList.Count(), resultList.Count());
			Assert.AreNotEqual(0, expectedList.Count());
			Assert.AreNotEqual(0, resultList.  Count());

			var b = expectedList.SequenceEqual(resultList);

			if (!b)
				for (int i = 0; i < resultList.Count; i++)
					Debug.WriteLine(string.Format("{0} {1} --- {2}", Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]));

			Assert.IsTrue(b);
		}
	}
}
