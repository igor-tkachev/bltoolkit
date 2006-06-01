using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace DataAccess
{
	[TestFixture]
	public class ScalarListTest
	{
		public abstract class TestAccessor : DataAccessor
		{
			[SprocName("Person_SelectAll")]
			[ObjectType(typeof(int))]
			public abstract ArrayList SelectIDs(DbManager db);

			[SprocName("Person_SelectAll")]
			[ObjectType(typeof(string)), ScalarFieldName(1)]
			public abstract ArrayList SelectFirstNames(DbManager db);

			[SprocName("Person_SelectAll")]
			[ObjectType(typeof(string)), ScalarFieldName("LastName")]
			public abstract ArrayList SelectLastNames(DbManager db);

#if FW2
			[SprocName("Person_SelectAll")]
			public abstract List<int> FW2SelectIDs(DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName(1)]
			public abstract List<string> FW2SelectFirstNames(DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName("LastName")]
			public abstract List<string> FW2SelectLastNames(DbManager db);
#endif
			public static TestAccessor CreateInstance()
			{
				return (TestAccessor)CreateInstance(typeof(TestAccessor));
			}
		}

		[Test]
		public void SelectIDsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				ArrayList array = ta.SelectIDs(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsTrue(array[0] is int);
			}
		}

		[Test]
		public void SelectFirstNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				ArrayList array = ta.SelectFirstNames(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsTrue(array[0] is string);
			}
		}

		[Test]
		public void SelectLastNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				ArrayList array = ta.SelectLastNames(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsTrue(array[0] is string);
			}
		}
#if FW2
		[Test]
		public void FW2SelectIDsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<int> array = ta.FW2SelectIDs(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void FW2SelectFirstNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> array = ta.FW2SelectFirstNames(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void FW2SelectLastNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> array = ta.FW2SelectLastNames(db);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}
#endif
	}
}
