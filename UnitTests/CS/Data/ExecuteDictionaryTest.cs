using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class ExecuteDictionaryTest
	{
		public enum Gender
		{
			[MapValue("F")]
			Female,
			[MapValue("M")]
			Male,
			[MapValue("U")]
			Unknown,
			[MapValue("O")]
			Other
		}

		public class Person
		{
			[MapField("PersonID")]
			public int ID;
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
					.SetSpCommand("Person_SelectAll")
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
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(table, "@PersonID", typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[1];
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
					.SetSpCommand("Person_SelectAll")
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
					.SetSpCommand("Person_SelectAll")
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
					.SetSpCommand("Person_SelectAll")
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
				.SetSpCommand("Person_SelectAll")
				.ExecuteDictionary(table,
					new MapIndex("@PersonID", 2, 3), typeof(Person));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);

				Person actualValue = (Person)table[new CompoundValue(1, "", "Pupkin")];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

#if FW2
		[Test]
		public void FW2DictionaryTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, Person> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary<int, Person>("ID");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void FW2DictionaryTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, Person> dic = new Dictionary<int, Person>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(dic, "@PersonID");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void FW2DictionaryTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, Person> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary<int, Person>(0);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[1];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void FW2DictionaryMapIndexTest()
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
		public void FW2DictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, Person> dic = new Dictionary<CompoundValue, Person>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(dic, new MapIndex(0));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[new CompoundValue(1)]; ;
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
			}
		}

		[Test]
		public void FW2DictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, Person> dic = new Dictionary<CompoundValue, Person>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(dic, new MapIndex("@PersonID", 2, 3));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);

				Person actualValue = dic[new CompoundValue(1, "", "Pupkin")];
				Assert.IsNotNull(actualValue);
				Assert.AreEqual("John", actualValue.FirstName);
				
			}
		}
#endif
	}
}
