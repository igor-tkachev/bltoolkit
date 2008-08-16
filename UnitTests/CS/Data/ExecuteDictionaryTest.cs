using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class ExecuteDictionaryTest
	{

#if ORACLE
		private const decimal _id = 1m;
#elif SQLITE
		private const long    _id = 1;
#else
		private const int     _id = 1;
#endif

		public enum Gender
		{
			[MapValue("F")] Female,
			[MapValue("M")] Male,
			[MapValue("U")] Unknown,
			[MapValue("O")] Other
		}

		public class Person
		{
			[MapField("PersonID")]
			public int    ID;
			public string FirstName;
			public string MiddleName;
			public string LastName;
			public Gender Gender;
		}
		
		[Test]
		public void DictionaryTest()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
#if SQLITE || SQLCE
					.SetCommand("SELECT * FROM Person")
#else
					.SetSpCommand("Person_SelectAll")
#endif
					.ExecuteDictionary("ID", typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void DictionaryTest2()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(table, "@PersonID", typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[_id];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void DictionaryTest3()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(0, typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void DictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(table, new MapIndex("ID"), typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[new CompoundValue(1)];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void DictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(new MapIndex(0), typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[new CompoundValue(1)];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}


		[Test]
		public void DictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(table,
					new MapIndex("@PersonID", 2, 3), typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[new CompoundValue(_id, "", "Pupkin")];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, Person> dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary<int, Person>("ID");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryTest2()
		{
			using (DbManager db = new DbManager())
			{
#if ORACLE
				Dictionary<decimal, Person> dic = new Dictionary<decimal, Person>();
#elif SQLITE
				Dictionary<long, Person> dic = new Dictionary<long, Person>();
#else
				Dictionary<int, Person> dic = new Dictionary<int, Person>();
#endif
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(dic, "@PersonID");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[_id];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, Person> dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary<int, Person>(0);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, Person> dic = db
					.SetCommand("SELECT * FROM Person WHERE PersonID < 3")
					.ExecuteDictionary<Person>(new MapIndex("LastName"));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[new CompoundValue("Pupkin")];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, Person> dic = new Dictionary<CompoundValue, Person>();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(dic, new MapIndex(0));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[new CompoundValue(1)]; ;
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void GenericsDictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, Person> dic = new Dictionary<CompoundValue, Person>();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteDictionary(dic, new MapIndex("@PersonID", 2, 3));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[new CompoundValue(_id, "", "Pupkin")];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
				
			}
		}
	}
}
