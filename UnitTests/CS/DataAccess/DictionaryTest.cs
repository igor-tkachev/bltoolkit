using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

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
			public abstract Hashtable SelectAll1();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID", "LastName")]
			public abstract Hashtable SelectAll2();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID")]
			[ScalarFieldName("FirstName")]
			[ObjectType(typeof(string))]
			public abstract Hashtable SelectAll3();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName("FirstName")]
			[ObjectType(typeof(string))]
			public abstract Hashtable SelectAll4();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName(1)]
			[ObjectType(typeof(string))]
			public abstract Hashtable SelectAll5();

			// Primary Key(s) => scalar filed. This will fail, since
			// we can not figure out both key type and scalar object type.
			// Note that version with generics works just fine.
			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName("FirstName"), ObjectType(typeof(string))]
			public abstract Hashtable ScalarDictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(PersonNoPK))]
			public abstract Hashtable Keyless();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ObjectType(typeof(PersonMultiPK))]
			public abstract Hashtable MultiPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			public abstract Hashtable DictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index("@PersonID")]
			public abstract Hashtable DictionaryByIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0, 1)]
			public abstract Hashtable DictionaryByMapIndex();

#if FW2
			[SqlQuery("SELECT * FROM Person")]
			[Index(new string[] { "ID" })]
			public abstract Dictionary<int, Person> SelectAllT1();

			[SqlQuery("SELECT * FROM Person")]
			[Index("@PersonID", "LastName")]
			public abstract Dictionary<IndexValue, Person> SelectAllT2();

			[SqlQuery("SELECT * FROM Person")]
			[Index("@PersonID")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<int, string> SelectAllT3();

			[SqlQuery("SELECT * FROM Person")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<IndexValue, string> SelectAllT4();

			[SqlQuery("SELECT * FROM Person")]
			[Index("PersonID", "LastName")]
			[ScalarFieldName(1)]
			public abstract Dictionary<IndexValue, string> SelectAllT5();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName("FirstName")]
			public abstract Dictionary<object, string> FW2ScalarDictionaryByPK();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[ScalarFieldName(1)] // Index from Db table Person, not type Person!
			public abstract Dictionary<int, string> FW2ScalarDictionaryByPK2();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0), ScalarFieldName(1)]
			public abstract Dictionary<int, string> FW2ScalarDictionaryByIndex();

			[SqlQuery("SELECT * FROM Person WHERE PersonID < 3")]
			[Index(0, 1), ScalarFieldName(1)]
			public abstract Dictionary<IndexValue, string> FW2ScalarDictionaryByMapIndex();
#endif
		}

		private TestAccessor _da;

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
			Assert.AreEqual("John", ((Person)dic2[new IndexValue(1, "Pupkin")]).FirstName);

			Hashtable dic3 = _da.SelectAll3();
			Assert.AreEqual("John", dic3[1]);

			Hashtable dic4 = _da.SelectAll4();
			Assert.AreEqual("John", dic4[new IndexValue(1, "Pupkin")]);

			Hashtable dic5 = _da.SelectAll5();
			Assert.AreEqual("John", dic5[new IndexValue(1, "Pupkin")]);
			
#if FW2
			Dictionary<int, Person> dict1 = _da.SelectAllT1();
			Assert.AreEqual("John", dict1[1].FirstName);

			Dictionary<IndexValue, Person> dict2 = _da.SelectAllT2();
			Assert.AreEqual("John", dict2[new IndexValue(1, "Pupkin")].FirstName);

			Dictionary<int, string> dict3 = _da.SelectAllT3();
			Assert.AreEqual("John", dict3[1]);

			Dictionary<IndexValue, string> dict4 = _da.SelectAllT4();
			Assert.AreEqual("John", dict4[new IndexValue(1, "Pupkin")]);

			Dictionary<IndexValue, string> dict5 = _da.SelectAllT5();
			Assert.AreEqual("John", dict5[new IndexValue(1, "Pupkin")]);
#endif
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

			Person actualValue = (Person)persons[1];
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

			Person actualValue = (Person)persons[new IndexValue(1, "Pupkin")];
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
			Assert.IsNull(persons[new IndexValue(-1, "NoSuchPerson")]);

			PersonMultiPK actualValue = (PersonMultiPK)persons[new IndexValue(1, "Pupkin")];
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

			string actualValue = persons[1];
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
		public void FW2ScalarDictionaryByMapIndexTest()
		{
			TestAccessor da = DataAccessor.CreateInstance<TestAccessor>();
			Dictionary<IndexValue, string> persons = da.FW2ScalarDictionaryByMapIndex();

			Assert.IsNotNull(persons);
			Assert.IsTrue(persons.Count > 0);

			string actualValue = persons[new IndexValue(1, "John")];
			Assert.IsNotNull(actualValue);
			Assert.AreEqual("John", actualValue);
		}
#endif
	}
}
