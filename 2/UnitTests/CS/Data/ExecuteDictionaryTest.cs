using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

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
			}
		}

		[Test]
		public void FW2DictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, Person> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary<Person>(new MapIndex("LastName"));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2DictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, Person> dic = new Dictionary<IndexValue, Person>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(dic, new MapIndex(0));

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2DictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, Person> dic = new Dictionary<IndexValue, Person>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteDictionary(dic, new MapIndex("@PersonID", 2, 3), "LastName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}
#endif
	}
}
