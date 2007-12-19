using System;
using System.Collections;
using System.Data;
using System.Data.SqlTypes;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class MapTest : TestFixtureBase
	{
		#region Enum To Int

		public class EnumClass
		{
			public enum Enum
			{
				Value1 = 1,
				Value2 = 2
			}

			public Enum Value = Enum.Value1;
		}

		public class IntClass
		{
			public int Value;
		}

		[Test]
		public void EnumToInt()
		{
			ArrayList list = new ArrayList();

			list.Add(new EnumClass());

			list = Map.ListToList(list, typeof(IntClass));

			Assert.AreEqual(1, ((IntClass)list[0]).Value);
		}

		#endregion

		#region ToEnum, FromEnum

		[DefaultValue(Enum1.Value3)]
		[MapValue(Enum1.Value2, "2")]
		public enum Enum1
		{
			[MapValue("1")] Value1,
			[NullValue]     Value2,
			[MapValue("3")] Value3
		}

		[Test]
		public void ToEnum()
		{
			Assert.AreEqual(Enum1.Value1, Map.ValueToEnum("1",    typeof(Enum1)));
			Assert.AreEqual(Enum1.Value2, Map.ValueToEnum(null,   typeof(Enum1)));
			Assert.AreEqual(Enum1.Value3, Map.ValueToEnum("2727", typeof(Enum1)));
		}

		[Test]
		public void FromEnum()
		{
			Assert.AreEqual("1", Map.EnumToValue(Enum1.Value1));
			Assert.IsNull  (     Map.EnumToValue(Enum1.Value2));
			Assert.AreEqual("3", Map.EnumToValue(Enum1.Value3));
		}

		public enum Enum2 : short
		{
			Value1 = 1,
			[NullValue] Value2,
			[DefaultValue]Value3
		}

		[Test]
		public void ToShortEnum()
		{
			Assert.AreEqual(Enum2.Value1, Map.ValueToEnum(1,    typeof(Enum2)));
			Assert.AreEqual(Enum2.Value2, Map.ValueToEnum(null, typeof(Enum2)));
			Assert.AreEqual(Enum2.Value3, Map.ValueToEnum(3,    typeof(Enum2)));
			Assert.AreEqual(Enum2.Value3, Map.ValueToEnum(2727, typeof(Enum2)));

			// DateTime.Now can't converted to Enum2.
			// Expected result in this case is default value.
			//
			Assert.AreEqual(Enum2.Value3, Map.ValueToEnum(DateTime.Now, typeof(Enum2)));
		}

		public enum Enum3 : long
		{
			Value1 = 1,
		}

		[Test, ExpectedException(typeof(InvalidCastException))]
		public void ToEnumwWithIncompatibleValue()
		{
			// An object can't be converted to Enum3.
			// Expected result is an exception, since Enum3 does not have
			// a default value defined.
			//
			Map.ValueToEnum(new object(), typeof(Enum3));
		}

		[Test]
		public void ToEnumwWithCompatibleValue()
		{
			// 123 is not defined for this enum, but can be converted to.
			// Enum3 does not have a default value defined, so just cast 123 to Enum3
			//
			Assert.AreEqual((Enum3)123, Map.ValueToEnum(123, typeof(Enum3)));

			// DateTime.Now also can be converted to Enum3 (as ticks).
			//
			Map.ValueToEnum(DateTime.Now, typeof(Enum3));
		}
		#endregion

		#region ObjectToObject

		public class SourceObject
		{
			public int    Field1 = 10;
			public string Field2 = "20";
			public double Field3 = 30.0;
		}

		public class Object1
		{
			public int Field1;
			public int Field2;
			public int Field3;
		}

		[Test]
		public void ObjectToObjectOO()
		{
			SourceObject so = new SourceObject();
			Object1      o  = new Object1();
 
			Map.ObjectToObject(so, o);

			Assert.AreEqual(10, o.Field1);
			Assert.AreEqual(20, o.Field2);
			Assert.AreEqual(30, o.Field3);
		}

		[Test]
		public void ObjectToObjectOT()
		{
			SourceObject so = new SourceObject();
			Object1      o  = (Object1)Map.ObjectToObject(so, typeof(Object1));

			Assert.AreEqual(10, o.Field1);
			Assert.AreEqual(20, o.Field2);
			Assert.AreEqual(30, o.Field3);
		}

#if FW2
		[Test]
		public void ObjectToObjectTO()
		{
			SourceObject so = new SourceObject();
			Object1      o  = Map.ObjectToObject<Object1>(so);

			Assert.AreEqual(10, o.Field1);
			Assert.AreEqual(20, o.Field2);
			Assert.AreEqual(30, o.Field3);
		}
#endif

		public class DefaultNullType
		{
			[NullValue(-1)]
			public int NullableInt;
		}

		#endregion

		#region DataRowToObject

		[Test]
		public void DataRowToObjectD()
		{
			DataTable table = new DataTable();

			table.Columns.Add("NullableInt", typeof(int));

			table.Rows.Add(new object[] { DBNull.Value });
			table.Rows.Add(new object[] { 1 });
			table.AcceptChanges();

			DefaultNullType dn = (DefaultNullType)Map.DataRowToObject(table.Rows[0], typeof(DefaultNullType));

			Assert.AreEqual(-1, dn.NullableInt);

			Map.DataRowToObject(table.Rows[1], DataRowVersion.Current, dn);

			Assert.AreEqual(1, dn.NullableInt);
		}

		#endregion

		#region ObjectToDictionary

		[Test]
		public void ObjectToDictionary()
		{
			SourceObject so = new SourceObject();
			Hashtable    ht = new Hashtable();
 
			Map.ObjectToDictionary(so, ht);

			Assert.AreEqual(10,   ht["Field1"]);
			Assert.AreEqual("20", ht["Field2"]);
			Assert.AreEqual(30,   ht["Field3"]);
		}

		#endregion

		#region DictionaryToObject

		[Test]
		public void DictionaryToObject()
		{
			SourceObject so = new SourceObject();
			Hashtable    ht = Map.ObjectToDictionary(so);
			Object1      o1 = (Object1)Map.DictionaryToObject(ht, typeof(Object1));

			Assert.AreEqual(10, o1.Field1);
			Assert.AreEqual(20, o1.Field2);
			Assert.AreEqual(30, o1.Field3);
		}

		#endregion

		#region DataRowToDictionary

		[Test]
		public void DataRowToDictionary()
		{
			DataTable table = GetDataTable();
			Hashtable hash  = Map.DataRowToDictionary(table.Rows[0]);

			Assert.AreEqual(table.Rows[0]["ID"],   hash["ID"]);
			Assert.AreEqual(table.Rows[0]["Name"], hash["Name"]);
			Assert.AreEqual(table.Rows[0]["Date"], hash["Date"]);
		}

		#endregion

		#region DictionaryToDataRow

		[Test]
		public void DictionaryToDataRow()
		{
			DataTable table1 = GetDataTable();
			Hashtable hash   = Map.DataRowToDictionary(table1.Rows[0]);
			DataTable table2 = new DataTable();

			Map.DictionaryToDataRow(hash, table2);

			Assert.AreEqual(table1.Rows[0]["ID"],   table2.Rows[0]["ID"]);
			Assert.AreEqual(table1.Rows[0]["Name"], table2.Rows[0]["Name"]);
			Assert.AreEqual(table1.Rows[0]["Date"], table2.Rows[0]["Date"]);
		}

		#endregion

		#region SqlTypes

		public class SqlTypeTypes
		{
			public class SourceObject
			{
				public string    s1 = "123";
				public SqlString s2 = "1234";
				public int       i1 = 123;
				public SqlInt32  i2 = 1234;

				public DateTime    d1 = DateTime.Now;
				public SqlDateTime d2 = DateTime.Now;
			}

			public class Object1
			{
				public SqlString s1;
				public string    s2;
				public SqlInt32  i1;
				public int       i2;

				public SqlDateTime d1;
				public DateTime    d2;
			}
		}

		[Test]
		public void SqlTypes()
		{
			SqlTypeTypes.SourceObject so = new SqlTypeTypes.SourceObject();
			SqlTypeTypes.Object1      o  = (SqlTypeTypes.Object1)Map.ObjectToObject(so, typeof(SqlTypeTypes.Object1));

			Console.WriteLine(o.s1); Assert.AreEqual("123",  o.s1.Value);
			Console.WriteLine(o.s2); Assert.AreEqual("1234", o.s2);

			Console.WriteLine(o.i1); Assert.IsTrue(o.i1.Value == 123);
			Console.WriteLine(o.i2); Assert.IsTrue(o.i2 == 1234);

			Console.WriteLine("{0} - {1}", so.d2, o.d2); Assert.AreEqual(o.d2, so.d2.Value);
			//Console.WriteLine("{0} - {1}", s.d1, d.d1); Assert.IsTrue(d.d1.Value == s.d1);
		}

		#endregion

		#region Arrays

		public class ArrayTypes
		{
			public class SourceObject
			{
				public int[,]   DimArray = new int[2, 2] { {1,2}, {3,4} };
				public string[] StrArray = new string[]  {"5","4","3","2","1"};
				public int[]    IntArray = new int[]     {1,2,3,4,5};
				public Enum1[]  EnmArray = new Enum1[]   {Enum1.Value1,Enum1.Value2,Enum1.Value3};

				public byte[][,][][,] ComplexArray = InitComplexArray();
				public static byte[][,][][,] InitComplexArray()
				{
					byte[][,][][,] ret = new byte[1][,][][,];
					ret[0]             = new byte[1,1][][,];
					ret[0][0,0]        = new byte[1][,];
					ret[0][0,0][0]     = new byte[,] { {1,2}, {3,4} };

					return ret;
				}
			}

			public class DestObject
			{
				public float[,] DimArray;
				public string[] StrArray;
				public string[] IntArray;
				public string[] EnmArray;
				public sbyte[][,][][,] ComplexArray;
			}

			public class IncompatibleObject
			{
				public int[][] DimArray;
			}
		}

		[Test]
		public void ArrayTypesTest()
		{
			ArrayTypes.SourceObject so = new ArrayTypes.SourceObject();
			ArrayTypes.DestObject o = (ArrayTypes.DestObject)Map.ObjectToObject(so, typeof(ArrayTypes.DestObject));

			Console.WriteLine(o.DimArray); Assert.AreEqual(so.DimArray[0,0], (int)o.DimArray[0,0]);
			Console.WriteLine(o.StrArray); Assert.AreEqual(so.StrArray, o.StrArray);
			Console.WriteLine(o.IntArray); Assert.AreEqual(so.IntArray[0].ToString(), o.IntArray[0]);

			Console.WriteLine(o.ComplexArray); Assert.IsTrue(o.ComplexArray[0][0,0][0][1,1] == 4);
		}

		[Test, ExpectedException(typeof(InvalidCastException))]
		public void IncompatibleArrayTypesTest()
		{
			ArrayTypes.SourceObject so = new ArrayTypes.SourceObject();
			Map.ObjectToObject(so, typeof(ArrayTypes.IncompatibleObject));
		}
		
		#endregion

		#region SourceListToDestList

		[Test]
		public void ListToList()
		{
			DataTable table = GetDataTable();
			ArrayList list1 = Map.DataTableToList(table, typeof(TestObject));
			ArrayList list2 = new ArrayList();

			Map.ListToList(list1, list2, typeof(TestObject));

			CompareLists(table, list2);
		}

		[Test]
		public void TableToList()
		{
			DataTable table = GetDataTable();
			ArrayList list  = Map.DataTableToList(table, typeof(TestObject));

			CompareLists(table, list);
		}

		[Test]
		public void ListToTable1()
		{
			DataTable table1 = GetDataTable();
			ArrayList list   = Map.DataTableToList(table1, typeof(TestObject));
			DataTable table2 = Map.ListToDataTable(list);

			table2.AcceptChanges();

			CompareLists(table1, table2);
		}

		[Test]
		public void ListToTable2()
		{
			DataTable table1 = GetDataTable();
			ArrayList list   = Map.DataTableToList(table1, typeof(TestObject));
			DataTable table2 = table1.Clone();

			Map.ListToDataTable(list, table2);

			table2.AcceptChanges();

			CompareLists(table1, table2);
		}

		[Test]
		public void TableToTable1()
		{
			DataTable table1 = GetDataTable();
			DataTable table2 = Map.DataTableToDataTable(table1);

			table2.AcceptChanges();

			CompareLists(table1, table2);
		}

		[Test]
		public void TableToTable2()
		{
			DataTable table1 = GetDataTable();
			DataTable table2 = new DataTable();
				
			Map.DataTableToDataTable(table1, table2);

			table2.AcceptChanges();

			CompareLists(table1, table2);
		}

		[Test]
		public void TableToDictionary()
		{
			DataTable   table = GetDataTable();
			IDictionary dic   = Map.DataTableToDictionary(table, new SortedList(), "ID", typeof(TestObject));

			CompareLists(table, Map.ListToList(dic.Values, typeof(TestObject)));
		}

		[Test]
		public void ListToDictionary()
		{
			DataTable   table = GetDataTable();
			ArrayList   list  = Map.DataTableToList     (table, typeof(TestObject));
			IDictionary dic   = Map.ListToDictionary(list, new SortedList(), "ID", typeof(TestObject));

			CompareLists(table, Map.ListToList(dic.Values, typeof(TestObject)));
		}

		[Test]
		public void DictionaryToDictionary()
		{
			DataTable   table = GetDataTable();
			ArrayList   list  = Map.DataTableToList           (table, typeof(TestObject));
			IDictionary dic1  = Map.ListToDictionary      (list, new SortedList(), "ID",  typeof(TestObject));
			IDictionary dic2  = Map.DictionaryToDictionary(dic1, new SortedList(), "@ID", typeof(TestObject));

			CompareLists(table, Map.ListToList(dic2.Values, typeof(TestObject)));
		}

		#endregion

		#region ObjectToDataRow

		public class DataRowTestType
		{
			public Int32      Int32Column  = 12345;
			public String    StringColumn  = "string";
			public Byte[] ByteArrayColumn  = new Byte[]{1,2,3,4,5};
			//public Byte[] ByteArrayColumn2 = null;
		}

		[Test]
		public void ObjectToDataRowTest()
		{
			DataTable       table = new DataTable();
			DataRowTestType obj   = new DataRowTestType();

			Map.ObjectToDataRow(obj, table);

			Assert.IsNotEmpty(table.Rows);
			DataRow dr      = table.Rows[0];

			Assert.AreEqual(table.Columns["Int32Column"]     .DataType, typeof(Int32));
			Assert.AreEqual(table.Columns["StringColumn"]    .DataType, typeof(String));
			Assert.AreEqual(table.Columns["ByteArrayColumn"] .DataType, typeof(Byte[]));
			//Assert.AreEqual(table.Columns["ByteArrayColumn2"].DataType, typeof(Byte[]));

			Assert.AreEqual(obj.Int32Column,      dr["Int32Column"]);
			Assert.AreEqual(obj.StringColumn,     dr["StringColumn"]);
			Assert.AreEqual(obj.ByteArrayColumn,  dr["ByteArrayColumn"]);
			//Assert.AreEqual(obj.ByteArrayColumn2, dr["ByteArrayColumn2"]);
		}

		#endregion
	}
}
