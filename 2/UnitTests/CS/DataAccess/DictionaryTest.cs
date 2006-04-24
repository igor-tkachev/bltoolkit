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
			[MapField("PersonID")]
			public int    ID;
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

#if FW2
			Dictionary<int, Person> dict1 = _da.SelectAllT1();
			Assert.AreEqual("John", dict1[1].FirstName);

			Dictionary<IndexValue, Person> dict2 = _da.SelectAllT2();
			Assert.AreEqual("John", dict2[new IndexValue(1, "Pupkin")].FirstName);

			Dictionary<int, string> dict3 = _da.SelectAllT3();
			Assert.AreEqual("John", dict3[1]);

			Dictionary<IndexValue, string> dict4 = _da.SelectAllT4();
			Assert.AreEqual("John", dict4[new IndexValue(1, "Pupkin")]);
#endif
		}
	}
}
