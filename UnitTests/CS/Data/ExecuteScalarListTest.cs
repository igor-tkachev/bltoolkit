using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Xml;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteScalarListTest
	{
		//[TestFixtureSetUp]
		public void SetUp()
		{
			using (var db = new DbManager())
			{
				var query = "INSERT INTO Person(FirstName, LastName, Gender) SELECT FirstName, LastName, Gender FROM Person";

				db.SetCommand(query).ExecuteNonQuery(); // 4
				db.SetCommand(query).ExecuteNonQuery(); // 8
				db.SetCommand(query).ExecuteNonQuery(); // 16
				db.SetCommand(query).ExecuteNonQuery(); // 32
				db.SetCommand(query).ExecuteNonQuery(); // 64
				db.SetCommand(query).ExecuteNonQuery(); // 128
				db.SetCommand(query).ExecuteNonQuery(); // 256
				db.SetCommand(query).ExecuteNonQuery(); // 512
				db.SetCommand(query).ExecuteNonQuery(); // 1024
				db.SetCommand(query).ExecuteNonQuery(); // 2048
				db.SetCommand(query).ExecuteNonQuery(); // 4096
				db.SetCommand(query).ExecuteNonQuery(); // 8192
				db.SetCommand(query).ExecuteNonQuery(); // 16384
				db.SetCommand(query).ExecuteNonQuery(); // 32768
				db.SetCommand(query).ExecuteNonQuery(); // 65536
				db.SetCommand(query).ExecuteNonQuery(); // 128k
				db.SetCommand(query).ExecuteNonQuery(); // 256k
				db.SetCommand(query).ExecuteNonQuery(); // 512k
				db.SetCommand(query).ExecuteNonQuery(); // 1m
			}
		}

		//[TestFixtureTearDown]
		public void TearDown()
		{
			using (var db = new DbManager())
			{
				db.SetCommand("DELETE FROM Person WHERE PersonID > 2").ExecuteNonQuery();
			}
		}

		[Test]
		public void ScalarListTest()
		{
			using (var db = new DbManager())
			{
				var array = db
#if SQLITE || SQLCE || SQLCE
					.SetCommand("SELECT * FROM Person")
#else
					.SetSpCommand("Person_SelectAll")
#endif
					.ExecuteScalarList(typeof(int));

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest2()
		{
			using (var db = new DbManager())
			{
				var array = new ArrayList();

				db.SetCommand("SELECT * FROM Person").ExecuteScalarList(array, typeof(int));

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest3()
		{
			using (var db = new DbManager())
			{
				var array = db.SetCommand("SELECT * FROM Person").ExecuteScalarList(typeof(string),1);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void ScalarListTest4()
		{
			using (var db = new DbManager())
			{
				var array = new ArrayList();

				db.SetCommand("SELECT * FROM Person").ExecuteScalarList(array, typeof(string), "LastName");

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarListTest()
		{
			var cmd = "SELECT PersonID FROM Person UNION ALL SELECT NULL";
#if !MSSQL && !SQLCE
			cmd += " FROM dual";
#endif

			using (var db = new DbManager())
			{
				var array = db.SetCommand(cmd).ExecuteScalarList<int?>();

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsNull(array[array.Count - 1]);
			}
		}

		[Test]
		public void GenericsScalarListTest2()
		{
			var cmd = "SELECT PersonID FROM Person UNION ALL SELECT NULL";
#if !MSSQL && !SQLCE
			cmd += " FROM dual";
#endif
			using (var db = new DbManager())
			{
				var array = new List<int>();

				db.SetCommand(cmd).ExecuteScalarList(array);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Console.WriteLine("Records processed: {0}", array.Count);
			}
		}

		[Test]
		public void GenericsScalarListTest3()
		{
			using (var db = new DbManager())
			{
				var array = db.SetCommand("SELECT * FROM Person").ExecuteScalarList<string>(1);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarListTest4()
		{
			using (var db = new DbManager())
			{
				var array = new List<string>();

				db.SetCommand("SELECT * FROM Person").ExecuteScalarList(array, "LastName");

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		[Test]
		public void GenericsScalarListTest5()
		{
			using (var db = new DbManager())
			{
				var cmd = "SELECT PersonID FROM Person UNION ALL SELECT NULL";
#if !MSSQL && !SQLCE
				cmd += " FROM dual";
#endif
				var array = db.SetCommand(cmd).ExecuteScalarList<uint?>();

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
				Assert.IsNull(array[array.Count - 1]);
			}
		}

		[Test]
		public void GenericsScalarListTest6()
		{
			using (var db = new DbManager())
			{
				var array = new List<uint>();

				db.SetCommand("SELECT * FROM Person").ExecuteScalarList(array);

				Assert.IsNotNull(array);
				Assert.IsTrue(array.Count > 0);
			}
		}

		private List<T> TestType<T>(DbManager db, string columnName) where T : class
		{
			var array = db
				.SetCommand(string.Format("SELECT {0} FROM DataTypeTest ORDER BY DataTypeID", columnName))
				.ExecuteScalarList<T>();

			Assert.IsNotNull(array);
			Assert.IsTrue   (array.Count > 1);
			Assert.IsNotNull(array[1]);

			return array;
		}

		private List<T?> TestNullableType<T>(DbManager db, string columnName) where T : struct
		{
			List<T?> array = db
				.SetCommand(string.Format("SELECT {0} FROM DataTypeTest ORDER BY DataTypeID", columnName))
				.ExecuteScalarList<T?>();

			Assert.IsNotNull(array);
			Assert.IsTrue   (array.Count > 1);
			Assert.IsNull   (array[0]);
			Assert.IsTrue   (array[1].HasValue);

			return array;
		}

		private List<T?> TestINullableType<T>(DbManager db, string columnName) where T : struct, INullable
		{
			var array = db
				.SetCommand(string.Format("SELECT {0} FROM DataTypeTest ORDER BY DataTypeID", columnName))
				.ExecuteScalarList<T?>();

			Assert.IsNotNull(array);
			Assert.IsTrue(array.Count > 1);
			Assert.IsTrue(array[0].HasValue);
			Assert.IsTrue(array[0].Value.IsNull);
			Assert.IsTrue(array[1].HasValue);

			return array;
		}

		[Test]
		public void GenericsScalarListDataTypesTest()
		{
			using (var db = new DbManager())
			{
				// Base types
				//
				TestNullableType<Boolean>   (db, "Boolean_");
				TestNullableType<Byte>      (db, "Byte_");
				TestNullableType<Char>      (db, "Char_");
				TestNullableType<DateTime>  (db, "DateTime_");
				TestNullableType<Decimal>   (db, "Decimal_");
				TestNullableType<Double>    (db, "Double_");
				TestNullableType<Guid>      (db, "Guid_");
				TestNullableType<Int16>     (db, "Int16_");
				TestNullableType<Int32>     (db, "Int32_");
				TestNullableType<Int64>     (db, "Int64_");
				TestNullableType<SByte>     (db, "SByte_");
				TestNullableType<Single>    (db, "Single_");
				TestType<String>            (db, "String_");
				TestNullableType<UInt16>    (db, "UInt16_");
				TestNullableType<UInt32>    (db, "UInt32_");
				TestNullableType<UInt64>    (db, "UInt64_");

#if !ORACLE
				// Sql types
				//
				TestINullableType<SqlBinary>  (db, "Binary_");
				TestINullableType<SqlBoolean> (db, "Boolean_");
				TestINullableType<SqlByte>    (db, "Byte_");
				TestType<SqlBytes>            (db, "Bytes_");
				TestType<SqlChars>            (db, "String_");
				TestINullableType<SqlDateTime>(db, "DateTime_");
				TestINullableType<SqlDecimal> (db, "Decimal_");
				TestINullableType<SqlDouble>  (db, "Double_");
#if !ACCESS
				TestINullableType<SqlGuid>    (db, "Bytes_");
#endif
				TestINullableType<SqlGuid>    (db, "Guid_");
				TestINullableType<SqlInt16>   (db, "Int16_");
				TestINullableType<SqlInt32>   (db, "Int32_");
				TestINullableType<SqlInt64>   (db, "Int64_");
				TestINullableType<SqlMoney>   (db, "Money_");
				TestINullableType<SqlString>  (db, "String_");
				TestINullableType<SqlSingle>  (db, "Single_");
				TestType<SqlXml>              (db, "Xml_");
#endif
				// BLToolkit extension
				var arrays  = TestType<Byte[]>(db, "Binary_");
				Console.WriteLine("{0}", arrays[1][0]);

				var streams = TestType<Stream>(db, "Bytes_");
				Console.WriteLine("{0}", streams[1].ReadByte());

				var symbols = TestType<Char[]>(db, "String_");
				Assert.AreEqual(symbols[1][0], 's');

				var xmlReaders = TestType<XmlReader>(db, "Xml_");
				xmlReaders[1].MoveToContent();
				Assert.IsTrue(xmlReaders[1].ReadToDescendant("element"));
				Console.WriteLine("{0}", xmlReaders[1].GetAttribute("strattr"));

				var xmlDocs = TestType<XmlDocument>(db, "Xml_");
				Assert.IsNotNull(xmlDocs[1]);
				Assert.IsNotNull(xmlDocs[1].DocumentElement);
				Console.WriteLine("{0}", xmlDocs[1].DocumentElement.GetAttribute("strattr"));
			}
		}
	}
}
