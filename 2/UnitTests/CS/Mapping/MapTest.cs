using System;
using System.Data;
using System.Data.SqlTypes;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class MapTest
	{
		#region ToEnum, FromEnum

		[DefaultValue(Enum1.Value3)]
		public enum Enum1
		{
			[MapValue("1")] Value1,
			[NullValue]     Value2,
			[MapValue("3")] Value3
		}

		[Test]
		public void ToEnum()
		{
			Assert.AreEqual(Enum1.Value1, Map.ToEnum("1",         typeof(Enum1)));
			Assert.AreEqual(Enum1.Value2, Map.ToEnum(null,        typeof(Enum1)));
			Assert.AreEqual(Enum1.Value3, Map.ToEnum((Enum1)2727, typeof(Enum1)));
		}

		[Test]
		public void FromEnum()
		{
			Assert.AreEqual("1", Map.FromEnum(Enum1.Value1));
			Assert.IsNull  (     Map.FromEnum(Enum1.Value2));
			Assert.AreEqual("3", Map.FromEnum(Enum1.Value3));
		}

		#endregion

		#region ToObject

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
		public void ToObjectOO()
		{
			SourceObject so = new SourceObject();
			Object1      o  = new Object1();
 
			Map.ToObject(so, o);

			Assert.AreEqual(10, o.Field1);
			Assert.AreEqual(20, o.Field2);
			Assert.AreEqual(30, o.Field3);
		}

		[Test]
		public void ToObjectOT()
		{
			SourceObject so = new SourceObject();
			Object1      o  = (Object1)Map.ToObject(so, typeof(Object1));

			Assert.AreEqual(10, o.Field1);
			Assert.AreEqual(20, o.Field2);
			Assert.AreEqual(30, o.Field3);
		}

#if FW2
		[Test]
		public void ToObjectTO()
		{
			SourceObject so = new SourceObject();
			Object1      o  = Map.ToObject<Object1>(so);

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

		[Test]
		public void ToObjectD()
		{
			DataTable table = new DataTable();

			table.Columns.Add("NullableInt", typeof(int));

			table.Rows.Add(new object[] { DBNull.Value });
			table.Rows.Add(new object[] { 1 });
			table.AcceptChanges();

			DefaultNullType dn = (DefaultNullType)Map.ToObject(table, typeof(DefaultNullType));

			Assert.AreEqual(-1, dn.NullableInt);

			Map.ToObject(table.Rows[1], DataRowVersion.Current, dn);

			Assert.AreEqual(1, dn.NullableInt);
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
			SqlTypeTypes.Object1      o  = (SqlTypeTypes.Object1)Map.ToObject(so, typeof(SqlTypeTypes.Object1));

			Console.WriteLine(o.s1); Assert.AreEqual("123",  o.s1.Value);
			Console.WriteLine(o.s2); Assert.AreEqual("1234", o.s2);

			Console.WriteLine(o.i1); Assert.IsTrue(o.i1.Value == 123);
			Console.WriteLine(o.i2); Assert.IsTrue(o.i2 == 1234);

			Console.WriteLine("{0} - {1}", so.d2, o.d2); Assert.AreEqual(o.d2, so.d2.Value);
			//Console.WriteLine("{0} - {1}", s.d1, d.d1); Assert.IsTrue(d.d1.Value == s.d1);
		}

		#endregion
	}
}
