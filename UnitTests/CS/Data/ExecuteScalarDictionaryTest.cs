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
			var da = new SqlQuery();

			foreach (Person p in da.SelectAll(typeof(Person)))
				if (p.ID > 10 || p.FirstName == "Crazy")
					da.DeleteByKey(typeof(Person), p.ID);
		}

		[Test]
		public void ScalarDictionaryTest()
		{
			using (var db = new DbManager())
			{
				var table = db
#if SQLITE || SQLCE
					.SetCommand("SELECT * FROM Person")
#else
					.SetSpCommand("Person_SelectAll")
#endif
					.ExecuteScalarDictionary("PersonID", typeof(int),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest2()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(table,
						"PersonID", typeof(int), "FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest3()
		{
			using (var db = new DbManager())
			{
				var table = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(0, typeof(int), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryTest4()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(table,0, typeof(int), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest()
		{
			using (var db = new DbManager())
			{
				var table = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(new MapIndex("PersonID"),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest2()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest3()
		{
			using (var db = new DbManager())
			{
				var table = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(new MapIndex(0),
						"FirstName", typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest4()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest5()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
				.ExecuteScalarDictionary(table,
					new MapIndex(0, 1, 2), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest6()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID", "FirstName", "LastName"), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void ScalarDictionaryMapIndexTest7()
		{
			using (var db = new DbManager())
			{
				var table = new Hashtable();
				db
					.SetCommand("SELECT * FROM Person")
				.ExecuteScalarDictionary(table,
					new MapIndex("PersonID", 2, 3), 1, typeof(string));

				Assert.IsNotNull(table);
				Assert.IsTrue(table.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest()
		{
			using (var db = new DbManager())
			{
				var dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary<int, string>("PersonID", "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest2()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<int, string>();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, "PersonID", "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest3()
		{
			using (var db = new DbManager())
			{
				var dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary<int, string>(0, 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryTest4()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<int, string>();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, 0, 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest()
		{
			using (var db = new DbManager())
			{
				var dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary<string>(new MapIndex("LastName"), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest2()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<CompoundValue, string>();
					db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, new MapIndex("LastName"), 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest3()
		{
			using (var db = new DbManager())
			{
				var dic = db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary<string>(new MapIndex(2), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest4()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<CompoundValue, string>();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, new MapIndex(0), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest5()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<CompoundValue, string>();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, new MapIndex(0, 1, 2), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest6()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<CompoundValue, string>();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, new MapIndex("PersonID", "FirstName", "LastName"), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarDictionaryMapIndexTest7()
		{
			using (var db = new DbManager())
			{
				var dic = new Dictionary<CompoundValue, string>();
				db
					.SetCommand("SELECT * FROM Person")
					.ExecuteScalarDictionary(dic, new MapIndex("PersonID", 2, 3), "LastName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}
	}
}
