using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Data
{
	[TestFixture]
	public class ExecuteScalarDictionaryTest
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey]
			public int    ID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[TestFixtureSetUp]
		public void SetUp()
		{
			SqlQuery da = new SqlQuery();

			foreach (Person p in da.SelectAll(typeof(Person)))
				if (p.ID > 10 || p.FirstName == "Crazy")
					da.DeleteByKey(typeof(Person), p.ID);
		}

		[Test]
		public void ScalarDictionaryTest()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary("PersonID", typeof(int),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest2()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(table,
						"PersonID", typeof(int), "FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest3()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(0, typeof(int), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest4()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(table,0, typeof(int), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(new MapIndex("PersonID"),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(new MapIndex(0),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest4()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest5()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(table,
					new MapIndex(0, 1, 2), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest6()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID", "FirstName", "LastName"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest7()
		{
			using (DbManager db = new DbManager())
			{
				Hashtable table = new Hashtable();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID", 2, 3), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<int, string>("PersonID", "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, string> dic = new Dictionary<int, string>();
				db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(dic, "PersonID", "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<int, string>(0, 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest4()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<int, string> dic = new Dictionary<int, string>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(dic, 0, 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<string>(new MapIndex("LastName"), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = new Dictionary<CompoundValue, string>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(dic, new MapIndex("LastName"), 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<string>(new MapIndex(2), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest4()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = new Dictionary<CompoundValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex(0), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest5()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = new Dictionary<CompoundValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex(0, 1, 2), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest6()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = new Dictionary<CompoundValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex("PersonID", "FirstName", "LastName"), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest7()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<CompoundValue, string> dic = new Dictionary<CompoundValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex("PersonID", 2, 3), "LastName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}
	}
}
