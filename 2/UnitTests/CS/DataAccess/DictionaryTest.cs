using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;
using BLToolkit.Mapping;

namespace DataAccess
{
	[TestFixture]
	public class DictionaryTest
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey]
			public int ID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public class Derived : Person
		{
			[MapIgnore]
			public string Ignore;
			public DateTime Date;
		}

		// Same as Person, but does not have any primary keys.
		public class PersonNoPK
		{
			[MapField("PersonID")]
			public int ID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		// Same as Person, but has a number of primary keys.
		public class PersonMultiPK
		{
			[MapField("PersonID"), PrimaryKey]
			public int ID;
			[PrimaryKey(22)]
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}
		
		[ObjectType(typeof(Person))]
		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT * FROM Person")]
			[Index("ID")]
			public abstract Hashtable   SelectAll1();

			[NoInstance]
			public abstract Hashtable Persons
			{
				[SqlQuery("SELECT * FROM Person")]
				[Index("ID")]
				get;
			}

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID", "LastName")]
			public abstract Hashtable   SelectAll2();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID")]
			[ScalarFieldName("FirstName")]
			[ObjectType(typeof(string))]
			public abstract Hashtable   SelectAll3();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName("FirstName")]
			[ObjectType(typeof(string))]
			public abstract Hashtable   SelectAll4();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName(1)]
			[ObjectType(typeof(string))]
			public abstract Hashtable   SelectAll5();

			// Primary Key(s) => scalar filed. This will fail, since
			// we can not figure out both key type and scalar object type.
			// Note that version with generics works just fine.
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName("FirstName"), ObjectType(typeof(string))]
			public abstract Hashtable   ScalarDictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(PersonNoPK))]
			public abstract Hashtable   Keyless();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(PersonMultiPK))]
			public abstract Hashtable   MultiPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(PersonMultiPK))]
			public abstract void        MultiPKReturnVoid([Destination] Hashtable dictionary);

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			public abstract Hashtable   DictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID")]
			public abstract Hashtable   DictionaryByIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0, 1)]
			public abstract Hashtable   DictionaryByMapIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0, 1)]
			public abstract IDictionary DictionaryByMapIndexWithDestination([Destination] Hashtable dictionary);

#if FW2
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0), ScalarFieldName(1)]
			public abstract void                              FW2ScalarDictionaryByIndexReturnVoid1([Destination] IDictionary<int, string> dictionary);

			[SqlQuery("SELECT * FROM Person")]
			[Index(new string[] { "ID" })]
			public abstract Dictionary<int, Person>           SelectAllT1();

			[SqlQuery("SELECT * FROM Person")]
			[Index("@PersonID", "LastName")]
			public abstract Dictionary<CompoundValue, Person> SelectAllT2();

			[SqlQuery("SELECT * FROM Person")]
			[Index("@PersonID")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<int, string>           SelectAllT3();

			[SqlQuery("SELECT * FROM Person")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<CompoundValue, string> SelectAllT4();

			[SqlQuery("SELECT * FROM Person")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName(1)]
			public abstract Dictionary<CompoundValue, string> SelectAllT5();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<object, string>        FW2ScalarDictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName(1)] // Index from Db table Person, not type Person!
			public abstract Dictionary<int, string>           FW2ScalarDictionaryByPK2();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0), ScalarFieldName(1)]
			public abstract Dictionary<int, string>           FW2ScalarDictionaryByIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0), ScalarFieldName(1)]
			public abstract void                              FW2ScalarDictionaryByIndexReturnVoid([Destination] IDictionary<int, string> dictionary);

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0, 1), ScalarFieldName(1)]
			public abstract Dictionary<CompoundValue, string> FW2ScalarDictionaryByMapIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0)]
			public abstract Dictionary<int, PersonNoPK>       FW2ScalarDictionaryByPKWithCustomType();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(Person))]
			public abstract Dictionary<int, object>           Fw2ScalarDictionaryByPKWithObjectType();

			[ActionName("SelectAll")]
			[Index("ID")]
			public abstract Dictionary<uint, Person> SelectAllT7();

			[ActionName("SelectAll")]
			public abstract Dictionary<long, Person> SelectAllT8();

			[SprocName("Person_SelectAll")]
			public abstract Dictionary<long, Derived> SelectAllDerived();

			[SprocName("Person_SelectAll")]
			public abstract Dictionary<CompoundValue, PersonMultiPK> SelectAllT9();

#endif
		}

		private TestAccessor _da;

#if ORACLE
		private const decimal _id = 1m;
#else
		private const int     _id = 1;
#endif

		public DictionaryTest()
		{
			TypeFactory.SaveTypes = true;

			_da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));
		}

		[Test]
		public void Test()
		{
			Hashtable dic1 = _da.SelectAll1();
			Assert.AreEqual("John", ((Person)dic1[1]).FirstName);

			Hashtable dic2 = _da.SelectAll2();
			Assert.AreEqual("John", ((Person)dic2[new CompoundValue(_id, "Pupkin")]).FirstName);

			Hashtable dic3 = _da.SelectAll3();
			Assert.AreEqual("John", dic3[_id]);

			Hashtable dic4 = _da.SelectAll4();
			Assert.AreEqual("John", dic4[new CompoundValue(_id, "Pupkin")]);

			Hashtable dic5 = _da.SelectAll5();
			Assert.AreEqual("John", dic5[new CompoundValue(_id, "Pupkin")]);
			
#if FW2
			Dictionary<int, Person> dict1 = _da.SelectAllT1();
			Assert.AreEqual("John", dict1[1].FirstName);

			Dictionary<CompoundValue, Person> dict2 = _da.SelectAllT2();
			Assert.AreEqual("John", dict2[new CompoundValue(_id, "Pupkin")].FirstName);

			Dictionary<int, string> dict3 = _da.SelectAllT3();
			Assert.AreEqual("John", dict3[1]);

			Dictionary<CompoundValue, string> dict4 = _da.SelectAllT4();
			Assert.AreEqual("John", dict4[new CompoundValue(_id, "Pupkin")]);

			Dictionary<CompoundValue, string> dict5 = _da.SelectAllT5();
			Assert.AreEqual("John", dict5[new CompoundValue(_id, "Pupkin")]);
#endif
		}

		[Test]
		public void AbstractGetterTest()
		{
			Hashtable dic = _da.Persons;
			Assert.AreEqual("John", ((Person)dic[1]).FirstName);
		}

		[Test, ExpectedException(typeof(DataAccessException))]
		public void KeylessTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			// Exception here:
			// Index is not defined for the method 'TestAccessor.Keyless'
			da.Keyless();
		}

		[Test]
		public void DictionaryByPKTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			Hashtable persons = da.DictionaryByPK();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[-1]);

			Person actualValue = (Person)persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void DictionaryByIndexTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			Hashtable persons = da.DictionaryByIndex();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[-1]);

			Person actualValue = (Person)persons[_id];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void DictionaryByMapIndexTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			Hashtable persons = da.DictionaryByMapIndex();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[-1]);

			Person actualValue = (Person)persons[new CompoundValue(1, "Pupkin")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void DictionaryByMapIndexTestWithDestionation()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			IDictionary persons = da.DictionaryByMapIndexWithDestination(new Hashtable());

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[-1]);

			Person actualValue = (Person)persons[new CompoundValue(1, "Pupkin")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test, ExpectedException(typeof(DataAccessException))]
		public void ScalarDictionaryByPKTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			// Exception here:
			// Index is not defined for the method 'TestAccessor.ScalarDictionaryByPK'
			da.ScalarDictionaryByPK();
		}

		[Test]
		public void MultiPKTest()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			Hashtable persons =  da.MultiPK();
			
			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[new CompoundValue(-1, "NoSuchPerson")]);

			PersonMultiPK actualValue = (PersonMultiPK)persons[new CompoundValue(1, "Pupkin")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}
		
		[Test]
		public void MultiPKTestReturnVoid()
		{
			TestAccessor da = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			Hashtable persons = new Hashtable();
			da.MultiPKReturnVoid(persons);
			
			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);
			Assert.IsNull(persons[new CompoundValue(-1, "NoSuchPerson")]);

			PersonMultiPK actualValue = (PersonMultiPK)persons[new CompoundValue(1, "Pupkin")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}
		
#if FW2
		[Test]
		public void FW2ScalarDictionaryByPKTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();

			Dictionary<object, string> persons = da.FW2ScalarDictionaryByPK();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[_id];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}

		[Test]
		public void FW2ScalarDictionaryByPKTest2()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();

			Dictionary<int, string> persons = da.FW2ScalarDictionaryByPK2();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}

		[Test]
		public void FW2ScalarDictionaryByIndexTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();

			Dictionary<int, string> persons = da.FW2ScalarDictionaryByIndex();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}

		[Test]
		public void FW2ScalarDictionaryByIndexReturnVoidTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();

			IDictionary<int, string> persons = new Dictionary<int, string>();
			da.FW2ScalarDictionaryByIndexReturnVoid(persons);

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}

		[Test]
		public void FW2ScalarDictionaryByMapIndexTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<CompoundValue, string> persons = da.FW2ScalarDictionaryByMapIndex();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[new CompoundValue(_id, "John")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}

		[Test]
		public void FW2ScalarDictionaryByPKWithCustomTypeTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<int, PersonNoPK> persons = da.FW2ScalarDictionaryByPKWithCustomType();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			PersonNoPK actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void FW2ScalarDictionaryByPKWithObjectTypeTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<int, object> persons = da.Fw2ScalarDictionaryByPKWithObjectType();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			Person actualValue = (Person) persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void FW2DictionaryMismatchKeyTypeTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<uint, Person> persons = da.SelectAllT7();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			Person actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void FW2DictionaryMismatchKeyTypeTest2()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<long, Person> persons = da.SelectAllT8();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			Person actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void FW2DictionaryMismatchKeyTypeWithHierarchyTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<long, Derived> persons = da.SelectAllDerived();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			Person actualValue = persons[1];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}

		[Test]
		public void FW2DictionaryMismatchKeyTypeCompoundValueTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<CompoundValue, PersonMultiPK> persons = da.SelectAllT9();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			PersonMultiPK actualValue = persons[new CompoundValue(1, "Pupkin")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue.FirstName);
		}
#endif
	}
}
