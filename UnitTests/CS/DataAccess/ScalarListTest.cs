using System;
using System.Collections;
using System.Collections.Generic;

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
			public abstract ArrayList SelectIDs       (DbManager db);

			[SprocName("Person_SelectAll")]
			[ObjectType(typeof(string)), ScalarFieldName(1)]
			public abstract ArrayList SelectFirstNames(DbManager db);

			[SprocName("Person_SelectAll")]
			[ObjectType(typeof(string)), ScalarFieldName("LastName")]
			public abstract ArrayList SelectLastNames (DbManager db);

			[SprocName("Person_SelectAll")]
			public abstract List<int>          FW2SelectIDs              (DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName(1)]
			public abstract List<string>       FW2SelectFirstNames       (DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName("LastName")]
			public abstract List<string>       FW2SelectLastNames        (DbManager db);

			[SprocName("Person_SelectAll"), ObjectType(typeof(int))]
			public abstract List<IConvertible> FW2SelectIDsAsIConvertible(DbManager db);

			[SprocName("Person_SelectAll")]
			public abstract void               FW2SelectIDsReturnVoid    (DbManager db, [Destination] List<int> list);

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
				ArrayList list = ta.SelectIDs(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
				Assert.IsTrue(list[0] is int);
			}
		}

		[Test]
		public void SelectFirstNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				ArrayList list = ta.SelectFirstNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
				Assert.IsTrue(list[0] is string);
			}
		}

		[Test]
		public void SelectLastNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				ArrayList list = ta.SelectLastNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
				Assert.IsTrue(list[0] is string);
			}
		}

		[Test]
		public void FW2SelectIDsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<int> list = ta.FW2SelectIDs(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void FW2SelectIDsAsIConvertibleTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<IConvertible> list = ta.FW2SelectIDsAsIConvertible(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void FW2SelectIDsReturnVoidTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<int> list = new List<int>();
				ta.FW2SelectIDsReturnVoid(db, list);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void FW2SelectFirstNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> list = ta.FW2SelectFirstNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void FW2SelectLastNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> list = ta.FW2SelectLastNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}
	}
}
