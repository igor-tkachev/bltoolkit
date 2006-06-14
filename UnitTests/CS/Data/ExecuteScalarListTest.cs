using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteScalarListTest
	{
		[Test]
		public void ScalarListTest()
		{
			using (DbManager db = new DbManager())
			{
				ArrayList array = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarList(typeof(int));

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest2()
		{
			using (DbManager db = new DbManager())
			{
				ArrayList array = new ArrayList();
					db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarList(array, typeof(int));

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest3()
		{
			using (DbManager db = new DbManager())
			{
				ArrayList array = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarList(typeof(string),1);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest4()
		{
			using (DbManager db = new DbManager())
			{
				ArrayList array = new ArrayList();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarList(array, typeof(string), "LastName");

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}
#if FW2
		[Test]
		public void FW2ScalarListTest()
		{
			using (DbManager db = new DbManager())
			{
				List<int?> array = db
					.SetCommand("SELECT PersonID FROM Person UNION ALL SELECT NULL")
					.ExecuteScalarList<int?>();

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsNull(array[array.Count - 1]);
			}
		}

		[Test]
		public void FW2ScalarListTest2()
		{
			using (DbManager db = new DbManager())
			{
				List<int> array = new List<int>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarList(array);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarListTest3()
		{
			using (DbManager db = new DbManager())
			{
				List<string> array = db
					.SetSpCommand("Person_SelectAll")
					.ExecuteScalarList<string>(1);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarListTest4()
		{
			using (DbManager db = new DbManager())
			{
				List<string> array = new List<string>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarList(array, "LastName");

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void FW2ScalarListTest5()
		{
			using (DbManager db = new DbManager())
			{
				List<uint?> array = db
					.SetCommand("SELECT PersonID FROM Person UNION ALL SELECT NULL")
					.ExecuteScalarList<uint?>();

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsNull(array[array.Count - 1]);
			}
		}

		[Test]
		public void FW2ScalarListTest6()
		{
			using (DbManager db = new DbManager())
			{
				List<uint> array = new List<uint>();
				db
				.SetSpCommand("Person_SelectAll")
				.ExecuteScalarList(array);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}
#endif
	}
}
