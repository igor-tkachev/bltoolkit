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
			public abstract List<int>          GenericsSelectIDs              (DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName(1)]
			public abstract List<string>       GenericsSelectFirstNames       (DbManager db);

			[SprocName("Person_SelectAll")]
			[ScalarFieldName("LastName")]
			public abstract List<string>       GenericsSelectLastNames        (DbManager db);

			[SprocName("Person_SelectAll"), ObjectType(typeof(int))]
			public abstract List<IConvertible> GenericsSelectIDsAsIConvertible(DbManager db);

			[SprocName("Person_SelectAll")]
			public abstract void               GenericsSelectIDsReturnVoid    (DbManager db, [Destination] List<int> list);

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
		public void GenericsSelectIDsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<int> list = ta.GenericsSelectIDs(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void GenericsSelectIDsAsIConvertibleTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<IConvertible> list = ta.GenericsSelectIDsAsIConvertible(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void GenericsSelectIDsReturnVoidTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<int> list = new List<int>();
				ta.GenericsSelectIDsReturnVoid(db, list);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void GenericsSelectFirstNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> list = ta.GenericsSelectFirstNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}

		[Test]
		public void GenericsSelectLastNamesTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();
				List<string> list = ta.GenericsSelectLastNames(db);

				Assert.IsNotNull (list);
				Assert.IsNotEmpty(list);
			}
		}
	}
}
