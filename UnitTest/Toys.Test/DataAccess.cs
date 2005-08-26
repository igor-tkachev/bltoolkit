using System;
using System.Collections;

using NUnit.Framework;

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
			[PrimaryKey, NonUpdatableAttribute]
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
			               [MapNullValue("")]  public abstract string   Notes           { get; set; }
			[MapNullValue(0)]                  public abstract int      ReportsTo       { get; set; }
			[MaxLength(255), MapNullValue("")] public abstract string   PhotoPath       { get; set; }

			public abstract ArrayList Territories { get; set; }
		}

		public abstract class EmployeeDataAccessor : DataAccessorBase
		{
			public abstract void Test();
		}

		private EmployeeDataAccessor _da;

		public DataAccess()
		{
			_da = (EmployeeDataAccessor)DataAccessFactory.CreateInstance(typeof(EmployeeDataAccessor));
		}

		[Test]
		public void SelectSql()
		{
			Employee e = (Employee)_da.SelectByKeySql(typeof(Employee), 1);
		}

		[Test]
		public void SelectAllSql()
		{
			ArrayList list = _da.SelectAllSql(typeof(Employee));

			Console.WriteLine(list.Count);
		}

		[Test]
		public void InsertSql()
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
		public void UpdateSql()
		{
			Employee e = (Employee)_da.SelectByKeySql(typeof(Employee), 1);

			int n = _da.UpdateSql(e);

			Assert.AreEqual(1, n);
		}

		[Test]
		public void DeleteByKeySql()
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
	}
}
