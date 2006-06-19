using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif
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

		private List<T> TestType<T>(DbManager db, string columnName) where T : class
		{
			List<T> array = db
				.SetCommand(string.Format("SELECT {0} FROM DataTypeTest ORDER BY DataTypeID", columnName))
				.ExecuteScalarList<T>();

			Assert.IsNotNull(array);
			Assert.IsTrue   (array.Count > 1);
			Assert.IsNull   (array[0]);
			Assert.IsNotNull(array[1]);

			return array;
		}

		private List<Nullable<T>> TestNullableType<T>(DbManager db, string columnName) where T : struct
		{
			List<Nullable<T>> array = db
				.SetCommand(string.Format("SELECT {0} FROM DataTypeTest ORDER BY DataTypeID", columnName))
				.ExecuteScalarList<Nullable<T>>();

			Assert.IsNotNull(array);
			Assert.IsTrue   (array.Count > 1);
			Assert.IsNull   (array[0]);
			Assert.IsTrue   (array[1].HasValue);

			return array;
		}


		[Test]
		public void FW2ScalarListDataTypesTest()
		{
			using (DbManager db = new DbManager())
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

#if EXPERIMENTAL
				// Sql types
				//
				TestNullableType<SqlBinary>  (db, "Binary_");
				TestNullableType<SqlBoolean> (db, "Boolean_");
				TestNullableType<SqlByte>    (db, "Byte_");
				TestType<SqlBytes>           (db, "Bytes_");
				TestType<SqlChars>           (db, "String_");
				TestNullableType<SqlDateTime>(db, "DateTime_");
				TestNullableType<SqlDecimal> (db, "Decimal_");
				TestNullableType<SqlDouble>  (db, "Double_");
				TestNullableType<SqlGuid>    (db, "Guid_");
				TestNullableType<SqlInt16>   (db, "Int16_");
				TestNullableType<SqlInt32>   (db, "Int32_");
				TestNullableType<SqlInt64>   (db, "Int64_");
				TestNullableType<SqlMoney>   (db, "Money_");
				TestNullableType<SqlString>  (db, "String_");
				TestNullableType<SqlSingle>  (db, "Single_");
				TestType<SqlXml>             (db, "Xml_");

				// BLToolkit extension
				List<Byte[]> arrays =
					TestType<Byte[]>         (db, "Binary_");
				Console.WriteLine("{0}", arrays[1][0]);
				List<Stream> streams =
					TestType<Stream>         (db, "Bytes_");
				Console.WriteLine("{0}", streams[1].ReadByte());
				List<Char[]> symbols =
					TestType<Char[]>         (db, "String_");
				Assert.AreEqual(symbols[1][0], 'B');
				TestNullableType<SqlGuid>    (db, "Bytes_");
				List<XmlReader> xmlReaders =
					TestType<XmlReader>      (db, "Xml_");
				xmlReaders[1].MoveToContent();
				Assert.IsTrue(xmlReaders[1].ReadToDescendant("element"));
				Console.WriteLine("{0}", xmlReaders[1].GetAttribute("strattr"));
#endif
			}
		}

#endif
	}
}
