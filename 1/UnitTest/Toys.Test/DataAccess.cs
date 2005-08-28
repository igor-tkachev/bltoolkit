using System;
using System.Collections;

using NUnit.Framework;

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;
using Rsdn.Framework.DataAccess;
using Rsdn.Framework.Validation;

namespace Toys.Test
{
	[TestFixture]
	public class DataAccess
	{
		[TableName("Employees")]
		public abstract class Employee : ValidatableEntityBase
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
				[ParamNameAttribute("FirstName")] string name1,
				[ParamNameAttribute("@LastName")] string name2);
		}

		public abstract class EmployeeDataAccessor1 : EmployeeDataAccessor
		{
			public override int EmployeeSalesByCountry(DateTime Beginning_Date, DateTime Ending_Date)
			{
				using (DbManager db = GetDbManager())
				{
					return db
						.SetSpCommand(
							GetSpName((string)null, "EmployeeSalesByCountry"),
							db.Parameter("@Beginning_Date", Beginning_Date),
							db.Parameter("@Ending_Date",    Ending_Date))
						.ExecuteNonQuery();
				}
			}

			public override Employee SelectByName(string firstName, string lastName)
			{
				using (DbManager db = GetDbManager())
				{
					Type type = typeof(Employee);

					return (Employee)db
						.SetSpCommand(
							GetSpName(type, "SelectByName"),
							db.Parameter("@firstName", firstName),
							db.Parameter("@lastName",  lastName))
						.ExecuteObject(type);
				}
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
	}
}
