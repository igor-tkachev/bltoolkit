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
	public class ExecuteScalarDictionaryTest
	{
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

#if FW2
		[Test]
		public void FW2ScalarDictionaryTest()
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
		public void FW2ScalarDictionaryTest2()
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
		public void FW2ScalarDictionaryTest3()
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
		public void FW2ScalarDictionaryTest4()
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
		public void FW2ScalarDictionaryMapIndexTest()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<string>(new MapIndex("LastName"), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest2()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = new Dictionary<IndexValue, string>();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary(dic, new MapIndex("LastName"), 1);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest3()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarDictionary<string>(new MapIndex(2), "FirstName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest4()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = new Dictionary<IndexValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex(0), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest5()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = new Dictionary<IndexValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex(0, 1, 2), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest6()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = new Dictionary<IndexValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex("PersonID", "FirstName", "LastName"), 2);

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarDictionaryMapIndexTest7()
		{
			using (DbManager db = new DbManager())
			{
				Dictionary<IndexValue, string> dic = new Dictionary<IndexValue, string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarDictionary(dic, new MapIndex("PersonID", 2, 3), "LastName");

				Assert.IsNotNull(dic);
				Assert.IsTrue(dic.Count > 0);
			}
		}
#endif
	}
}
