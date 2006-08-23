using System;
using System.Collections;
using System.Data;

using NUnit.Framework;

#if FW2
using System.Collections.Generic;
using PersonDataSet = DataAccessTest.PersonDataSet2;
#else
using PersonDataSet = DataAccessTest.PersonDataSet;
#endif

using BLToolkit.EditableObjects;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Validation;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	namespace Other
	{
		public abstract class Person : DataAccessorTest.Person
		{
			[MaxLength(256), Required]
			public abstract string Diagnosis { get; set; }
		}
	}

	[TestFixture]
	public class DataAccessorTest
	{
		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		[TableName("Person")]
		public abstract class Person : EditableObject
		{
			[Required]                     public abstract Gender Gender     { get; set; }

			[PrimaryKey, NonUpdatable]
			[MapField("PersonID")]         public abstract int    ID         { get; set; }
			[MaxLength(50), Required]      public abstract string LastName   { get; set; }
			[MaxLength(50), Required]      public abstract string FirstName  { get; set; }
			[MaxLength(50), NullValue("")] public abstract string MiddleName { get; set; }

			public abstract ArrayList Territories { get; set; }
		}

		public abstract class PersonAccessor : DataAccessor
		{
			public abstract int    Person_SelectAll();
			public abstract void   Person_SelectAll(DbManager db);
			public abstract Person SelectByName(string firstName, string lastName);

			[SprocName("Person_SelectByName"), DiscoverParameters]
			public abstract Person AnySprocName(string firstName, string lastName);

			[ActionName("SelectByName")]
			public abstract Person AnyActionName(string firstName, string lastName);

			[ActionName("SelectByName")]
			public abstract Person AnyParamName(
				[ParamName("FirstName")] string name1,
				[ParamName("@LastName")] string name2);

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract ArrayList SelectAllList();

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract IList SelectAllList([Destination] IList list);

			public abstract Person SelectByName(string firstName, string lastName, [Destination] Person p);

			public abstract Person Insert([Destination(NoMap = false)] Person e);
			
			[SprocName("Person_Insert_OutputParameter")]
			public abstract void Insert_OutputParameter([Direction.Output("PersonID")] Person e);

			[SprocName("Scalar_ReturnParameter")]
			public abstract void Insert_ReturnParameter([Direction.ReturnValue("PersonID"),
				Direction.Ignore("PersonID", "FirstName", "LastName", "MiddleName", "Gender")] Person e);
#if FW2
			[ActionName("SelectAll")]
			public abstract List<Person> SelectAllListT();
			[SprocName("Person_SelectAll")]
			public abstract PersonDataSet.PersonDataTable SelectAllTypedDataTable();
#endif

			[SprocName("Person_SelectAll")] public abstract DataSet       SelectAllDataSet();
			[SprocName("Person_SelectAll")] public abstract PersonDataSet SelectAllTypedDataSet();
			[SprocName("Person_SelectAll")] public abstract DataTable     SelectAllDataTable();

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract ArrayList SameTypeName();

			[ActionName("SelectByName")]
			public abstract Person SameTypeName(string firstName, string lastName);
		}

		public abstract class PersonDataAccessor1 : PersonAccessor
		{
			public DataSet SelectByName()
			{
				using (DbManager db = GetDbManager())
				{
					DataSet ds = new DataSet();

					db.SetSpCommand("Person_SelectAll");

					if (ds.Tables.Count > 0)
						db.ExecuteDataSet(ds, ds.Tables[0].TableName);
					else
						db.ExecuteDataSet(ds);

					return ds;
				}
			}

			[ActionName("SelectAll"), ObjectType(typeof(Other.Person))]
			public abstract ArrayList SameTypeName1();

			[ActionName("SelectByName")]
			public abstract Other.Person SameTypeName1(string firstName, string lastName);

			protected override string GetDefaultSpName(string typeName, string actionName)
			{
				return "Patient_" + actionName;
			}
		}

		public class PersonList : ArrayList
		{
			public new Person this[int idx]
			{
				get { return (Person)base[idx]; }
				set { base[idx] = value;        }
			}
		}

		private PersonAccessor _da;

		public DataAccessorTest()
		{
			TypeFactory.SaveTypes = true;

			object o = TypeAccessor.CreateInstance(typeof(Person));

			_da = (PersonAccessor)DataAccessor.CreateInstance(typeof(PersonAccessor));
		}

		[Test]
		public void Sql_Select()
		{
			Person e = (Person)_da.SelectByKeySql(typeof(Person), 1);
		}

		[Test]
		public void Sql_SelectAll()
		{
			ArrayList list = _da.SelectAllSql(typeof(Person));

			Console.WriteLine(list.Count);
		}

		[Test]
		public void Sql_Insert()
		{
			ArrayList list = _da.SelectAllSql(typeof(Person));
			Hashtable tbl  = new Hashtable();

			foreach (Person e in list)
				tbl[e.ID] = e;

			Person em = (Person)Map.CreateInstance(typeof(Person));

			em.FirstName = "1";
			em.LastName  = "2";

			_da.InsertSql(em);

			list = _da.SelectAllSql(typeof(Person));

			foreach (Person e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_da.DeleteSql(e);
		}

		[Test]
		public void Sql_Update()
		{
			Person e = (Person)_da.SelectByKeySql(typeof(Person), 1);

			int n = _da.UpdateSql(e);

			Assert.AreEqual(1, n);
		}

		[Test]
		public void Sql_DeleteByKey()
		{
			ArrayList list = _da.SelectAllSql(typeof(Person));
			Hashtable tbl = new Hashtable();

			foreach (Person e in list)
				tbl[e.ID] = e;

			Person em = (Person)Map.CreateInstance(typeof(Person));

			em.FirstName = "1";
			em.LastName  = "2";

			_da.InsertSql(em);

			list = _da.SelectAllSql(typeof(Person));

			foreach (Person e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_da.DeleteByKeySql(typeof(Person), e.ID);
		}

		[Test]
		public void Sproc_SelectAll()
		{
			ArrayList list = _da.SelectAll(typeof(Person));
			Console.WriteLine(list.Count);
		}

		[Test]
		public void Gen_Person_SelectAll()
		{
			int n = _da.Person_SelectAll();
			Console.WriteLine(n);
		}

		[Test]
		public void Gen_Person_SelectAll_DbManager()
		{
			using (DbManager db = _da.GetDbManager())
				_da.Person_SelectAll(db);
		}

		[Test]
		public void Gen_SelectByName()
		{
			Person e = _da.SelectByName("John", "Pupkin");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_SelectByNameWithDestination()
		{
			Person e = (Person)TypeAccessor.CreateInstance(typeof (Person));
			_da.SelectByName("John", "Pupkin", e);
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_InsertGetID()
		{
			Person    e = (Person)TypeAccessor.CreateInstance(typeof(Person));
			e.FirstName = "Crazy";
			e.LastName  = "Frog";
			e.Gender    = Gender.Other;

			_da.Insert(e);

			// If you got an assertion here, make sure your Person_Insert sproc looks like
			//
			// INSERT INTO Person( LastName,  FirstName,  MiddleName,  Gender)
			// VALUES            (@LastName, @FirstName, @MiddleName, @Gender)
			//
			// SELECT Cast(SCOPE_IDENTITY() as int) PersonID
			// ^==important                         ^==important
			//
			Assert.IsTrue(e.ID > 0);
			_da.Delete(e);
		}

		[Test]
		public void Gen_InsertGetIDReturnParameter()
		{
			Person    e = (Person)TypeAccessor.CreateInstance(typeof(Person));

			_da.Insert_ReturnParameter(e);

			Assert.AreEqual(12345, e.ID);
		}

		[Test]
		public void Gen_InsertGetIDOutputParameter()
		{
			Person    e = (Person)TypeAccessor.CreateInstance(typeof(Person));
			e.FirstName = "Crazy";
			e.LastName  = "Frog";
			e.Gender    = Gender.Other;

			_da.Insert_OutputParameter(e);

			Assert.IsTrue(e.ID > 0);
			_da.Delete(e);
		}

		[Test]
		public void Gen_SprocName()
		{
			Person e = _da.AnySprocName("John", "Pupkin");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_ActionName()
		{
			Person e = _da.AnyActionName("John", "Pupkin");
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Gen_ParamName()
		{
			Person e = _da.AnyParamName("John", "Pupkin");
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
			PersonDataSet ds = _da.SelectAllTypedDataSet();
			Assert.AreNotEqual(0, ds.Person.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllDataTable()
		{
			DataTable dt = _da.SelectAllDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllList()
		{
			ArrayList list = _da.SelectAllList();
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListWithDestination()
		{
			ArrayList list = new ArrayList();
			_da.SelectAllList(list);
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SameTypeName()
		{
			Person e = _da.SameTypeName("Tester", "Testerson");
			Assert.IsInstanceOfType(typeof(Person), e);
			Assert.AreEqual(2, e.ID);

			ArrayList list = _da.SameTypeName();
			Assert.AreNotEqual(0, list.Count);
			Assert.IsInstanceOfType(typeof(Person), list[0]);

			PersonDataAccessor1 da1 = (PersonDataAccessor1)DataAccessor.CreateInstance(typeof(PersonDataAccessor1));

			Other.Person e1 = da1.SameTypeName1("Tester", "Testerson");
			Assert.IsInstanceOfType(typeof(Other.Person), e1);
			Assert.IsNotEmpty(e1.Diagnosis);

			list = da1.SameTypeName1();
			Assert.AreNotEqual(0, list.Count);
			Assert.IsInstanceOfType(typeof(Other.Person), list[0]);
		}

#if FW2
		[Test]
		public void Gen_SelectAllTypedDataTable()
		{
			PersonDataSet.PersonDataTable dt = _da.SelectAllTypedDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllListT()
		{
			List<Person> list = _da.SelectAllListT();
			Assert.AreNotEqual(0, list.Count);
		}
#endif
	}
}

