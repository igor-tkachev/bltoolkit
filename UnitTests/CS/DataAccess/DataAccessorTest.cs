using System;
using System.Collections;
using System.ComponentModel;
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
#if FW2
	namespace Other
	{
		public abstract class Person : DataAccessorTest.Person
		{
			[MaxLength(256), Required]
			public abstract string Diagnosis { get; set; }
		}
	}
#endif

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

		public interface IPerson
		{
			int    ID          { get; set; }
			string LastName    { get; set; }
			string FirstName   { get; set; }
			string MiddleName  { get; set; }
			IList  Territories { get; set; }
		}

		[TableName("Person")]
		public abstract class Person : EditableObject, IPerson
		{
			[Required]                     public abstract Gender Gender     { get; set; }

			[PrimaryKey, NonUpdatable]
			[MapField("PersonID")]         public abstract int    ID         { get; set; }
			[MaxLength(50), Required]      public abstract string LastName   { get; set; }
			[MaxLength(50), Required]      public abstract string FirstName  { get; set; }
			[MaxLength(50), NullValue("")] public abstract string MiddleName { get; set; }

			public abstract IList Territories { get; set; }
		}

		public abstract class PersonAccessor : DataAccessor
		{
			public abstract int    Person_SelectAll();
			public abstract void   Person_SelectAll(DbManager db);
			public abstract Person SelectByName(string firstName, string lastName);

			[SprocName("Person_SelectByName"), DiscoverParameters]
			public abstract Person AnySprocName(string anyParameterName, string otherParameterName);

			[ActionName("SelectByName")]
			public abstract Person AnyActionName(string firstName, string lastName);

			[ActionName("SelectByName")]
			public abstract Person AnyParamName(
				[ParamName("FirstName")] string name1,
				[ParamName("@LastName")] string name2);

			[ActionName("SelectByName"), ObjectType(typeof(Person))]
			public abstract IPerson SelectByNameReturnInterface(string firstName, string lastName);

			[ActionName("SelectByName"), ObjectType(typeof(Person))]
			public abstract void SelectByNameReturnVoid(string firstName, string lastName, [Destination] object p);

			public abstract IPerson SelectByName(string firstName, string lastName, [Destination] Person p);
			
			public abstract Person Insert([Destination(NoMap = false)] Person e);
			
			[SprocName("Person_Insert_OutputParameter")]
			public abstract void Insert_OutputParameter([Direction.Output("PERSONID")] Person e);

			[SprocName("Scalar_ReturnParameter")]
			public abstract void Insert_ReturnParameter([Direction.ReturnValue("@PersonID"),
				Direction.Ignore("PersonID", "FirstName", "LastName", "MiddleName", "Gender")] Person e);

			[SprocName("Scalar_ReturnParameter")]
			public abstract void Insert_ReturnParameter2([Direction.ReturnValue("ID"),
				Direction.Ignore("PersonID", "FirstName", "LastName", "MiddleName", "Gender")] Person e);

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract ArrayList SameTypeName();

			[ActionName("SelectByName")]
			public abstract Person SameTypeName(string firstName, string lastName);

			[ActionName("SelectByKey")]
			public abstract Person ParamNullID    ([ParamNullValue(1)] int id);

#if FW2
			[ActionName("SelectByKey")]
			public abstract Person ParamNullableID([ParamNullValue(1)] int? id);

			[ActionName("SelectByKey")]
			public abstract Person ParamNullableID2([ParamNullValue("1")] int? id);
#endif

			[ActionName("SelectByName")]
			public abstract Person ParamNullString([ParamNullValue("John")] string firstName, string lastName);

			public abstract Person ParamNullGuid  ([ParamNullValue("49F74716-C6DE-4b3e-A753-E40CFE6C6EA0")] Guid guid);
			public abstract Person ParamNullEnum  ([ParamNullValue(Gender.Unknown)] Gender gender);

			#region IDataReader

			[SprocName("Person_SelectAll")]
			public abstract IDataReader   SelectAllIDataReader(DbManager db);

			[SprocName("Person_SelectAll"), CommandBehavior(CommandBehavior.SchemaOnly)]
			public abstract IDataReader   SelectAllIDataReaderSchemaOnly(DbManager db);

			#endregion

			#region DataSet

			[SprocName("Person_SelectAll")]
			public abstract DataSet       SelectAllDataSet();

			[SprocName("Person_SelectAll")]
			public abstract IListSource   SelectAllDataSetWithDestination([Destination] DataSet ds);

			[SprocName("Person_SelectAll")]
			public abstract void          SelectAllDataSetReturnVoid     ([Destination] DataSet ds);

			[SprocName("Person_SelectAll")]
			public abstract PersonDataSet SelectAllTypedDataSet();

			[SprocName("Person_SelectAll")]
			public abstract object        SelectAllTypedDataSetWithDestination([Destination] PersonDataSet ds);

			[SprocName("Person_SelectAll")]
			public abstract void          SelectAllTypedDataSetReturnVoid     ([Destination] PersonDataSet ds);

			#endregion

			#region DataTable

			[SprocName("Person_SelectAll")]
			public abstract DataTable     SelectAllDataTable();

			[SprocName("Person_SelectAll")]
			public abstract DataTable     SelectAllDataTableWithDestination([Destination] DataTable dt);

			[SprocName("Person_SelectAll")]
			public abstract void          SelectAllDataTableReturnVoid     ([Destination] DataTable dt);

#if FW2
			[SprocName("Person_SelectAll")]
			public abstract PersonDataSet.PersonDataTable SelectAllTypedDataTable();

			[SprocName("Person_SelectAll")]
			public abstract void          SelectAllTypedDataTableReturnVoid     ([Destination] PersonDataSet.PersonDataTable dt);
#endif

			#endregion

			#region List

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract ArrayList SelectAllList();

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract IList SelectAllListWithDestination([Destination] IList list);

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract void SelectAllListReturnVoid      ([Destination] IList list);

#if FW2
			[ActionName("SelectAll")]
			public abstract List<Person> SelectAllListT();

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract List<IPerson> SelectAllListTWithObjectType();

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract List<IPerson> SelectAllListTWithDestination([Destination] IList list);

			[ActionName("SelectAll"), ObjectType(typeof(Person))]
			public abstract void SelectAllListTWithObjectTypeReturnVoid([Destination] IList list);

			[ActionName("SelectAll")]
			public abstract void SelectAllListTReturnVoid              ([Destination] IList<Person> list);
#endif

			#endregion
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

#if FW2
			[ActionName("SelectAll"), ObjectType(typeof(Other.Person))]
			public abstract ArrayList SameTypeName1();

			[ActionName("SelectByName")]
			public abstract Other.Person SameTypeName1(string firstName, string lastName);
#endif

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
		private SprocQuery     _sproc = new SprocQuery();
		private SqlQuery       _sql   = new SqlQuery();

		public DataAccessorTest()
		{
			TypeFactory.SaveTypes = true;

			object o = TypeAccessor.CreateInstance(typeof(Person));
			Assert.IsInstanceOfType(typeof(Person), o);

			_da = (PersonAccessor)DataAccessor.CreateInstance(typeof(PersonAccessor));
			Assert.IsInstanceOfType(typeof(PersonAccessor), _da);
		}

		[Test]
		public void Sql_Select()
		{
			Person e = (Person)_sql.SelectByKey(typeof(Person), 1);

			Assert.IsNotNull(e);
			Assert.AreEqual(1, e.ID);
		}

		[Test]
		public void Sql_SelectAll()
		{
			ArrayList list = _sql.SelectAll(typeof(Person));

			Console.WriteLine(list.Count);
		}

		[Test]
		public void Sql_Insert()
		{
			ArrayList list = _sql.SelectAll(typeof(Person));
			Hashtable tbl  = new Hashtable();

			foreach (Person e in list)
				tbl[e.ID] = e;

			Person em = (Person)Map.CreateInstance(typeof(Person));

			em.FirstName = "1";
			em.LastName  = "2";

			_sql.Insert(em);

			list = _sql.SelectAll(typeof(Person));

			foreach (Person e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_sql.Delete(e);
		}

		[Test]
		public void Sql_Update()
		{
			Person e = (Person)_sql.SelectByKey(typeof(Person), 1);

			int n = _sql.Update(e);

			Assert.AreEqual(1, n);
		}

		[Test]
		public void Sql_DeleteByKey()
		{
			ArrayList list = _sql.SelectAll(typeof(Person));
			Hashtable tbl = new Hashtable();

			foreach (Person e in list)
				tbl[e.ID] = e;

			Person em = (Person)Map.CreateInstance(typeof(Person));

			em.FirstName = "1";
			em.LastName  = "2";

			_sql.Insert(em);

			list = _sql.SelectAll(typeof(Person));

			foreach (Person e in list)
				if (tbl.ContainsKey(e.ID) == false)
					_sql.DeleteByKey(typeof(Person), e.ID);
		}

		[Test]
		public void Sproc_SelectAll()
		{
			ArrayList list = _sproc.SelectAll(typeof(Person));
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
		public void Gen_SelectByNameReturnInterface()
		{
			IPerson i = _da.SelectByNameReturnInterface("John", "Pupkin");

			Assert.IsNotNull(i);
			Assert.AreEqual (1, i.ID);
		}

		[Test]
		public void Gen_SelectByNameReturnVoid()
		{
			Person e = (Person)TypeAccessor.CreateInstance(typeof (Person));
			Assert.IsNotNull(e);

			_da.SelectByNameReturnVoid("John", "Pupkin", e);
			Assert.AreEqual (1, e.ID);
		}

		[Test]
		public void Gen_SelectByNameWithDestination()
		{
			Person e = (Person)TypeAccessor.CreateInstance(typeof (Person));
			IPerson i = _da.SelectByName("John", "Pupkin", e);

			Assert.AreSame (i, e);
			Assert.AreEqual(1, i.ID);
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
			_sproc.Delete(e);
		}
		[Test]
		public void Gen_InsertGetIDReturnParameter()
		{
			Person e = (Person)TypeAccessor.CreateInstance(typeof(Person));

			_da.Insert_ReturnParameter(e);

			Assert.AreEqual(12345, e.ID);
		}


		[Test]
		public void Gen_InsertGetIDReturnParameter2()
		{
			Person e = (Person)TypeAccessor.CreateInstance(typeof(Person));

			_da.Insert_ReturnParameter2(e);

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
			_sproc.Delete(e);
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
		public void Gen_SameTypeName()
		{
			Person e = _da.SameTypeName("Tester", "Testerson");
			Assert.IsInstanceOfType(typeof(Person), e);
			Assert.AreEqual(2, e.ID);

			ArrayList list = _da.SameTypeName();
			Assert.AreNotEqual(0, list.Count);
			Assert.IsInstanceOfType(typeof(Person), list[0]);

#if FW2
			PersonDataAccessor1 da1 = (PersonDataAccessor1)DataAccessor.CreateInstance(typeof(PersonDataAccessor1));
			Other.Person         e1 = da1.SameTypeName1("Tester", "Testerson");

			Assert.IsInstanceOfType(typeof(Other.Person), e1);
			Assert.IsNotEmpty(e1.Diagnosis);

			list = da1.SameTypeName1();
			Assert.AreNotEqual(0, list.Count);
			Assert.IsInstanceOfType(typeof(Other.Person), list[0]);
#endif
		}

		[Test]
		public void ParamNullValueIDTest()
		{
			// Parameter id == 1 will be replaced with NULL
			//
			Person e1 = _da.ParamNullID(1);
			Assert.IsNull(e1);

			// Parameter id == 2 will be send as is
			//
			Person e2 = _da.ParamNullID(2);
			Assert.IsNotNull(e2);
		}

#if FW2
		[Test]
		public void ParamNullValueNullableIDTest()
		{
			// Parameter id == 1 will be replaced with NULL
			//
			Person e1 = _da.ParamNullableID(1);
			Assert.IsNull(e1);
			e1  = _da.ParamNullableID2(1);
			Assert.IsNull(e1);

			// Parameter id == 2 will be send as is
			//
			Person e2 = _da.ParamNullableID(2);
			Assert.IsNotNull(e2);
		}
#endif

		[Test]
		public void ParamNullValueStrTest()
		{
			// Parameter firstName == 'John' will be replaced with NULL
			//
			Person e1 = _da.ParamNullString("John", "Pupkin");
			Assert.IsNull(e1);

			// Parameter firstName == 'Tester' will be send as is
			//
			Person e2 = _da.ParamNullString("Tester", "Testerson");
			Assert.IsNotNull(e2);
		}

		#region IDataReader

		[Test]
		public void Gen_SelectAllIDataReader()
		{
			// Keep connection open for IDataReader
			//
			using (DbManager db = _da.GetDbManager())
			{
				IDataReader dr = _da.SelectAllIDataReader(db);

				Assert.IsNotNull(dr);
				Assert.AreNotEqual(0, dr.FieldCount);
				Assert.IsTrue(dr.Read());
				Assert.AreNotEqual(0, dr.GetInt32(dr.GetOrdinal("PersonID")));
			}
		}

		[Test]
		public void Gen_SelectAllIDataReaderSchemaOnly()
		{
			// Keep connection open for IDataReader
			//
			using (DbManager db = _da.GetDbManager())
			{
				IDataReader dr = _da.SelectAllIDataReaderSchemaOnly(db);

				Assert.IsNotNull(dr);
				Assert.AreNotEqual(0, dr.FieldCount);
				Assert.IsFalse(dr.Read());
			}
		}

		#endregion

		#region DataSet

		[Test]
		public void Gen_SelectAllDataSet()
		{
			DataSet ds = _da.SelectAllDataSet();
			Assert.AreNotEqual(0, ds.Tables[0].Rows.Count);
		}

		[Test]
		public void Gen_SelectAllDataSetWithDestination()
		{
			IListSource ls = _da.SelectAllDataSetWithDestination(new DataSet());
			Assert.AreNotEqual(0, ls.GetList().Count);
		}

		[Test]
		public void Gen_SelectAllDataSetReturnVoid()
		{
			DataSet ds = new DataSet();
			_da.SelectAllDataSetReturnVoid(ds);
			Assert.AreNotEqual(0, ds.Tables[0].Rows.Count);
		}

		[Test]
		public void Gen_SelectAllTypedDataSet()
		{
			PersonDataSet ds = _da.SelectAllTypedDataSet();
			Assert.AreNotEqual(0, ds.Person.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllTypedDataSetWithObjectType()
		{
			object o = _da.SelectAllTypedDataSetWithDestination(new PersonDataSet());
			Assert.IsInstanceOfType(typeof(PersonDataSet), o);

			PersonDataSet ds = (PersonDataSet)o;
			Assert.AreNotEqual(0, ds.Person.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllTypedDataSetReturnVoid()
		{
			PersonDataSet ds = new PersonDataSet();
			_da.SelectAllTypedDataSetReturnVoid(ds);

			Assert.AreNotEqual(0, ds.Person.Rows.Count);
		}

		#endregion

		#region DataTable

		[Test]
		public void Gen_SelectAllDataTable()
		{
			DataTable dt = _da.SelectAllDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllDataTableWithDestination()
		{
			DataTable dt = _da.SelectAllDataTableWithDestination(new DataTable());
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllDataTableReturnVoid()
		{
			DataTable dt = new DataTable();
			_da.SelectAllDataTableReturnVoid(dt);
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

#if FW2
		[Test]
		public void Gen_SelectAllTypedDataTable()
		{
			PersonDataSet.PersonDataTable dt = _da.SelectAllTypedDataTable();
			Assert.AreNotEqual(0, dt.Rows.Count);
		}

		[Test]
		public void Gen_SelectAllTypedDataTableReturnVoid()
		{
			PersonDataSet.PersonDataTable dt = new PersonDataSet.PersonDataTable();

			_da.SelectAllTypedDataTableReturnVoid(dt);
			Assert.AreNotEqual(0, dt.Rows.Count);
		}
#endif

		#endregion

		#region List

		[Test]
		public void Gen_SelectAllList()
		{
			ArrayList list = _da.SelectAllList();
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListWithDestination()
		{
			IList list = _da.SelectAllListWithDestination(new ArrayList());
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListReturnVoid()
		{
			ArrayList list = new ArrayList();
			_da.SelectAllListReturnVoid(list);
			Assert.AreNotEqual(0, list.Count);
		}

#if FW2

		[Test]
		public void Gen_SelectAllListT()
		{
			List<Person> list = _da.SelectAllListT();
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListTWithObjectType()
		{
			List<IPerson> list = _da.SelectAllListTWithObjectType();
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListTReturnVoid()
		{
			List<Person> list = new List<Person>();
			_da.SelectAllListTReturnVoid(list);
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListTWithObjectTypeReturnVoid()
		{
			List<IPerson> list = new List<IPerson>();
			_da.SelectAllListTWithObjectTypeReturnVoid(list);
			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Gen_SelectAllListTWithDestination()
		{
			List<IPerson> list = _da.SelectAllListTWithDestination(new List<IPerson>());
			Assert.AreNotEqual(0, list.Count);
		}

#endif

		#endregion
	}
}

