using System;
using System.Collections;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;
using Rsdn.Framework.DataAccess;
using Rsdn.Framework.Validation;

#if VER2
using System.Collections.Generic;

using EmployeeDataSet = Toys.Test.EmployeeDataSet2;
#endif

namespace Toys.Test
{
	[TestFixture]
	public class DataAccess
	{
		[TableName("Employees")]
		public abstract class Employee : ValidatableObjectBase
		{
			[PrimaryKey, NonUpdatable]
			[MapField("EmployeeID")]           public abstract int      ID              { get; set; }
			[MaxLength(20), Required]          public abstract string   LastName        { get; set; }
			[MaxLength(10), Required]          public abstract string   FirstName       { get; set; }
			[MaxLength(30), MapNullValue("")]  public abstract string   Title           { get; set; }
			[MaxLength(25)]                    public abstract string   TitleOfCourtesy { get; set; }
			[MapNullDateTime]                  public abstract DateTime BirthDate       { get; set; }
			[MapNullDateTime]                  public abstract DateTime HireDate        { get; set; }
			[MaxLength(60), MapNullValue("")]  public abstract string   Address         { get; set; }
			[MaxLength(15), MapNullValue("")]  public abstract string   City            { get; set; }
			[MaxLength(15), MapNullValue("")]  public abstract string   Region          { get; set; }
			[MaxLength(10), MapNullValue("")]  public abstract string   PostalCode      { get; set; }
			[MaxLength(15), MapNullValue("")]  public abstract string   Country         { get; set; }
			[MaxLength(24), MapNullValue("")]  public abstract string   HomePhone       { get; set; }
			[MaxLength(4),  MapNullValue("")]  public abstract string   Extension       { get; set; }
			[NonUpdatable,  MapNullValue("")]  public abstract string   Notes           { get; set; }
			[MapNullValue(0)]                  public abstract int      ReportsTo       { get; set; }
			[MaxLength(255), MapNullValue("")] public abstract string   PhotoPath       { get; set; }

			public abstract ArrayList Territories { get; set; }
		}

		public abstract class EmployeeDataAccessor : DataAccessorBase
		{
			public abstract int      EmployeeSalesByCountry(DateTime Beginning_Date, DateTime Ending_Date);
			public abstract int      Employee_SelectAll();
			public abstract void     Employee_SelectAll(DbManager db);
			public abstract Employee SelectByName(string firstName, string lastName);

			[SprocName("Employee_SelectByName"), DiscoverParameters]
			public abstract Employee AnySprocName(string firstName, string lastName);

			[ActionName("SelectByName")]
			public abstract Employee AnyActionName(string firstName, string lastName);

			[ActionName("SelectByName")]
			public abstract Employee AnyParamName(
				[ParamName("FirstName")] string name1,
				[ParamName("@LastName")] string name2);

			[ActionName("SelectAll"), ObjectType(typeof(Employee))]
			public abstract ArrayList SelectAllList();

#if VER2
			[ActionName("SelectAll")]
			public abstract List<Employee> SelectAllListT();
			[SprocName("Employee_SelectAll")]
			public abstract EmployeeDataSet.EmployeesDataTable SelectAllTypedDataTable();
#endif

			[SprocName("Employee_SelectAll")] public abstract DataSet         SelectAllDataSet();
			[SprocName("Employee_SelectAll")] public abstract EmployeeDataSet SelectAllTypedDataSet();
			[SprocName("Employee_SelectAll")] public abstract DataTable       SelectAllDataTable();
		}

		public abstract class EmployeeDataAccessor2 : DataAccessorBase
		{
			[SprocName("Employee_SelectAll")] public abstract ArrayList SelectAllList();
		}

		public abstract class EmployeeDataAccessor1 : EmployeeDataAccessor
		{
			public DataSet SelectByName()
			{
				using (DbManager db = GetDbManager())
				{
					DataSet ds = new DataSet();

					db.SetSpCommand("Employee_SelectAll");

					if (ds.Tables.Count > 0)
						db.ExecuteDataSet(ds, ds.Tables[0].TableName);
					else
						db.ExecuteDataSet(ds);

					return ds;
				}
			}
		}

		public class EmployeeList : ArrayList
		{
			public new Employee this[int idx]
			{
				get { return (Employee)base[idx]; }
				set { base[idx] = value;          }
			}
		}

		private EmployeeDataAccessor _da;

		public DataAccess()
		{
			_da = (EmployeeDataAccessor)DataAccessFactory.CreateInstance(typeof(EmployeeDataAccessor));
		}

		[Test]
		public void Sql_Select()
		{
			Employee e = (Employee)_da.SelectByKeySql(typeof(Employee), 1);
		}

		[Test]
		public void Sql_SelectAll()
		{
			ArrayList list = _da.SelectAllSql(typeof(Employee));

			Console.WriteLine(list.Count);
		}

		[Test]
		public void Sql_Insert()
		{
			ArrayList list = _da.SelectAllSql(typeof(Employee));
			Hashtable tbl  = new Hashtable();

			foreach (Employee e in list)
				tbl[e.ID] = e;

			Employee em = (Employee)Map.Descriptor(typeof(Employee)).CreateInstance();

			em.FirstName = "1";
			em.LastName  = "2";

			_da.InsertSql(em);

			list = _da.SelectAllSql(typeof(Employee));

			foreach (Employee e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_da.DeleteSql(e);
		}

		[Test]
		public void Sql_Update()
		{
			Employee e = (Employee)_da.SelectByKeySql(typeof(Employee), 1);

			int n = _da.UpdateSql(e);

			Assert.AreEqual(1, n);
		}

		[Test]
		public void Sql_DeleteByKey()
		{
			ArrayList list = _da.SelectAllSql(typeof(Employee));
			Hashtable tbl = new Hashtable();

			foreach (Employee e in list)
				tbl[e.ID] = e;

			Employee em = (Employee)Map.Descriptor(typeof(Employee)).CreateInstance();

			em.FirstName = "1";
			em.LastName  = "2";

			_da.InsertSql(em);

			list = _da.SelectAllSql(typeof(Employee));

			foreach (Employee e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_da.DeleteByKeySql(typeof(Employee), e.ID);
		}

		[Test]
		public void Sproc_SelectAll()
		{
			ArrayList list = _da.SelectAll(typeof(Employee));
			Console.WriteLine(list.Count);
		}

		[Test]
		public void Gen_Employee_SelectAll()
		{
			int n = _da.Employee_SelectAll();
			Console.WriteLine(n);
		}

		[Test]
		public void Gen_Employee_SelectAll_DbManager()
		{
			using (DbManager db = _da.GetDbManager())
				_da.Employee_SelectAll(db);
		}

		[Test]
		public void Gen_SelectByName()
		{
			Employee e = _da.SelectByName("Nancy", "Davolio");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_SprocName()
		{
			Employee e = _da.AnySprocName("Nancy", "Davolio");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_ActionName()
		{
			Employee e = _da.AnyActionName("Nancy", "Davolio");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_ParamName()
		{
			Employee e = _da.AnyParamName("Nancy", "Davolio");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_SelectAllDataSet()
		{
			DataSet ds = _da.SelectAllDataSet();
			Assert.AreNotEqual(0, ds.Tables[0].Rows.Count);
		}

		[Test]
		public void Gen_SelectAllTypedDataSet()
		{
			EmployeeDataSet ds = _da.SelectAllTypedDataSet();
			Assert.AreNotEqual(0, ds.Employees.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllDataTable()
		{
			DataTable dt = _da.SelectAllDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		[ExpectedException(typeof(RsdnDataAccessException))]
		public void Gen_SelectAllListexception()
		{
			DataAccessFactory.CreateInstance(typeof(EmployeeDataAccessor2));
		}

		[Test]
		public void Gen_SelectAllList()
		{
			ArrayList list = _da.SelectAllList();
			Assert.AreNotEqual(0, list.Count);
		}

#if VER2
		[Test]
		public void Gen_SelectAllTypedDataTable()
		{
			EmployeeDataSet.EmployeesDataTable dt = _da.SelectAllTypedDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllListT()
		{
			List<Employee> list = _da.SelectAllListT();
			Assert.AreNotEqual(0, list.Count);
		}
#endif
	}
}
