using System;
using System.IO;
using System.Reflection;
using System.Data;
using System.Data.SqlTypes;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace CS
{
	[TestFixture]
	public class MapDataTest
	{
		#region ToValue

		[MapValue(StateNullable.Pending, "P")]
			[MapNullValue(StateNullable.Null)]
			[MapDefaultValue(StateNullable.Unknown)]
			public enum StateNullable
		{
			Unknown,
			Null,
			[MapValue("A")]               Active,
			[MapValue(MappedValue = "I")] Inactive,
			Pending
		}

		private void CheckState(object value, StateNullable state)
		{
			string message;
			
			if (value != null)
			{
				message = string.Format(
					"'{0}' value of '{1}' type does not map to '{2}'", 
					value.ToString(),
					value.GetType(),
					state);
			}
			else
			{
				message = string.Format("'null' does not map to '{0}'", state);
			}

			Assert.IsTrue(
				(StateNullable)Map.ToEnum(value, typeof(StateNullable)) == state, 
				message);
		}

		[Test]
		public void ToValue()
		{
			CheckState("A",          StateNullable.Active);
			CheckState("I",          StateNullable.Inactive);
			CheckState("P",          StateNullable.Pending);
			CheckState(DBNull.Value, StateNullable.Null);
			CheckState("X",          StateNullable.Unknown);
			CheckState(null,         StateNullable.Null);
		}
		#endregion

		#region ToValue_Exception

		[MapValue(State.Active,   "A")]
			[MapValue(State.Inactive, "I")]
			[MapValue(TypeValue = State.Pending, MappedValue = "P")]
			public enum State
		{
			Active,
			Inactive,
			Pending
		}

		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void ToValue_Exception1()
		{
			Map.ToEnum(DBNull.Value, typeof(State));
		}

		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void ToValue_Exception2()
		{
			Map.ToEnum("X", typeof(State));
		}
	
		[Test]
		[ExpectedException(typeof(RsdnMapException))]
		public void ToValue_Exception3()
		{
			Map.ToEnum(null, typeof(State));
		}
		#endregion

		#region DefaultNull

		public class DefaultNullType
		{
			[MapNullValue(-1)]
			public int NullableInt;
		}

		[Test]
		public void MapNullValue_DefaultNull()
		{
			DataTable table = new DataTable();
            
			table.Columns.Add("NullableInt", typeof(int));
            
			table.Rows.Add(new object[] { DBNull.Value });
			table.Rows.Add(new object[] { 1 });
			table.AcceptChanges();   
            
			DefaultNullType dn = (DefaultNullType)Map.ToObject(
				table.Rows[0], typeof(DefaultNullType));

			Console.WriteLine("NullableInt: {0}", dn.NullableInt);

			Assert.AreEqual(dn.NullableInt, -1);

			dn = (DefaultNullType)Map.ToObject(
				table.Rows[1], typeof(DefaultNullType));

			Console.WriteLine("NullableInt: {0}", dn.NullableInt);

			Assert.AreEqual(dn.NullableInt, 1);
		}
		#endregion

		#region XmlTest

		public class x1
		{
			public class Src
			{
				public int    i1 = 1;
				public int    i2 = 2;
				public object i3 = null;
				public object n1 = null;
				public object n2 = null;
			}

			public abstract class Dest1
			{
				public abstract int ix1 { get; set; }
				public abstract int i2  { get; set; }
			}

			public class Dest2
			{
				public int ix1;
				public int i2;
				public int i3;
			}

			public class Dest3
			{
				[MapNullValue(-1)]
				public int n1;

				[MapNullValue(-1)]
				public int n2;
			}
		}

		[Test]
		public void TestXml()
		{
			MapDescriptor.SetMappingSchema("CS.Map.xml");

			x1.Src   s = new x1.Src();
			x1.Dest1 d = (x1.Dest1)Map.ToObject(s, typeof(x1.Dest1));

			Assert.AreEqual(s.i1, d.ix1, "i1 -> ix1");
		}

		[Test]
		public void TestXmlNamespace()
		{
			MapDescriptor.SetMappingSchema("CS.MapNamespace.xml");

			x1.Src   s = new x1.Src();
			x1.Dest2 d = (x1.Dest2)Map.ToObject(s, typeof(x1.Dest2));

			Assert.AreEqual(s.i1, d.ix1, "i1 -> ix1");
			Assert.AreEqual(3,    d.i2,  "i2 -> i2");
			Assert.AreEqual(111,  d.i3,  "i3 -> i3");
		}

		[Test]
		public void TestXmlResource()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			Stream   stream   = assembly.GetManifestResourceStream("CS.MapNamespace.xml");

			MapDescriptor.SetMappingSchema(stream);

			x1.Src   s = new x1.Src();
			x1.Dest2 d = (x1.Dest2)Map.ToObject(s, typeof(x1.Dest2));

			Assert.AreEqual(s.i1, d.ix1, "i1 -> ix1");
			Assert.AreEqual(3,    d.i2,  "i2 -> i2");
		}

		[Test]
		public void TestXmlOverride()
		{
			MapDescriptor.SetMappingSchema("CS.MapNamespace.xml");

			x1.Src   s = new x1.Src();
			x1.Dest3 d = (x1.Dest3)Map.ToObject(s, typeof(x1.Dest3));

			Assert.AreEqual(-2, d.n1, "xml");
			Assert.AreEqual(-1, d.n2, "attribute");
		}
		#endregion

		#region XmlAttributeTest

		public class x2
		{
			public class Src
			{
				public int i1 = 1;
				public int i2 = 2;
			}

			[MapXml("DestAttr")]
				public class DestAttr
			{
				public int ix1;
				public int i2;
			}

			[MapXml("CS.MapAttribute.xml", "DestAttr")]
				public class DestAttr2
			{
				public int ixx1;
				public int i2;
			}

			[MapXml(FileName="CS.Map.xml", XPath="my_tag/my_tag_type[@my_name=\"dest_attr\"]")]
				public class DestAttr3
			{
				public int ixx1;
				public int i2;
			}
		}

		[Test]
		public void TestXmlAttribute()
		{
			MapDescriptor.SetMappingSchema("CS.MapNamespace.xml");

			x2.Src      s = new x2.Src();
			x2.DestAttr d = Map.ToObject(s, typeof(x2.DestAttr)) as x2.DestAttr;

			Assert.AreEqual(s.i1, d.ix1, "i1 -> ix1");
		}

		[Test]
		public void TestXmlAttribute2()
		{
			MapDescriptor.SetMappingSchema("CS.MapNamespace.xml");

			x2.Src       s = new x2.Src();
			x2.DestAttr2 d = Map.ToObject(s, typeof(x2.DestAttr2)) as x2.DestAttr2;

			Assert.AreEqual(s.i1, d.ixx1, "i1 -> ixx1");
		}

		[Test]
		public void TestXmlAttribute3()
		{
			x2.Src       s = new x2.Src();
			x2.DestAttr3 d = Map.ToObject(s, typeof(x2.DestAttr3)) as x2.DestAttr3;

			Assert.AreEqual(s.i1, d.ixx1, "i1 -> ixx1");
		}

		#endregion

		#region GuidTest
		public class t1
		{
			public class Src
			{
				public Guid   g1 = Guid.NewGuid();
				public string g2 = Guid.NewGuid().ToString();
			}

			public class Dest
			{
				public Guid g1 = Guid.Empty;
				public Guid g2 = Guid.Empty;
			}
		}

		[Test]
		public void GuidTest()
		{
			t1.Src  s = new t1.Src();
			t1.Dest d = (t1.Dest)Map.ToObject(s, typeof(t1.Dest));

			Console.WriteLine(d.g1);
			Console.WriteLine(d.g2);
			Assert.IsTrue(d.g1 == s.g1);
			Assert.IsTrue(d.g2 == new Guid(s.g2));
		}
		#endregion

		#region SqlTypes
		public class t3
		{
			public class Src
			{
				public string    s1 = "123";
				public SqlString s2 = "1234";
				public int       i1 = 123;
				public SqlInt32  i2 = 1234;

				public DateTime    d1 = DateTime.Now;
				public SqlDateTime d2 = DateTime.Now;
			}

			public class Dest
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
		public void SqlTypesTest()
		{
			t3.Src  s = new t3.Src();
			t3.Dest d = (t3.Dest)Map.ToObject(s, typeof(t3.Dest));

			Console.WriteLine(d.s1); Assert.IsTrue(d.s1.Value == "123");
			Console.WriteLine(d.s2); Assert.IsTrue(d.s2 == "1234");

			Console.WriteLine(d.i1); Assert.IsTrue(d.i1.Value == 123);
			Console.WriteLine(d.i2); Assert.IsTrue(d.i2 == 1234);

			Console.WriteLine("{0} - {1}", s.d2, d.d2); Assert.AreEqual(d.d2, s.d2.Value);
			//Console.WriteLine("{0} - {1}", s.d1, d.d1); Assert.IsTrue(d.d1.Value == s.d1);
		}
		#endregion

		#region SqlGuidNull
		public class t4
		{
			public class Src
			{
				public object g1 = DBNull.Value;
				public object g2 = null;
				public object g3 = DBNull.Value;
				public object g4 = null;
			}

			public class Dest
			{
				public SqlGuid g1;
				public SqlGuid g2;
				public SqlGuid g3 { get { return _g3; } set { _g3 = value; } } SqlGuid _g3;
				public SqlGuid g4 { get { return _g4; } set { _g4 = value; } } SqlGuid _g4;
			}
		}

		[Test]
		public void SqlGuidNullTest()
		{
			t4.Src  s = new t4.Src();
			t4.Dest d = (t4.Dest)Map.ToObject(s, typeof(t4.Dest));

			Console.WriteLine(d.g1); Assert.IsTrue(d.g1.IsNull);
			Console.WriteLine(d.g2); Assert.IsTrue(d.g2.IsNull);
			Console.WriteLine(d.g3); Assert.IsTrue(d.g3.IsNull);
			Console.WriteLine(d.g4); Assert.IsTrue(d.g4.IsNull);
		}
		#endregion

		#region SqlGuidNullAbstract
		public class t5
		{
			public class TestType
			{
				SqlGuid m_value;
				public SqlGuid Value
				{
					get { return m_value;  }
					set { m_value = value; }
				}
			}

			public class Src
			{
				public object g1 = DBNull.Value;
				public object g2 = null;
			}

			public abstract class Dest
			{
				[MapType(typeof(TestType))] public abstract SqlGuid g1 { get; set; }
				[MapType(typeof(TestType))] public abstract SqlGuid g2 { get; set; }
			}
		}

		[Test]
		public void SqlGuidNullAbstract()
		{
			t5.Src  s = new t5.Src();
			t5.Dest d = (t5.Dest)Map.ToObject(s, typeof(t5.Dest));

			Console.WriteLine(d.g1); Assert.IsTrue(d.g1.IsNull);
			Console.WriteLine(d.g2); Assert.IsTrue(d.g2.IsNull);
		}
		#endregion

		#region TestInnerClass
		public class t6
		{
			public class TestType
			{
				SqlGuid m_id;
				public SqlGuid ID
				{
					get { return m_id;  }
					set { m_id = value; }
				}
			}

			public class Src
			{
				public object  TestID1 = DBNull.Value;
				public object  TestID2 = null;
				public object  TestID3 = Guid.NewGuid();
				public SqlGuid TestID4 = Guid.NewGuid();
			}

			[MapField("TestID1", "Test1.ID")]
				[MapField("TestID2", "Test2.ID")]
				[MapField("TestID3", "Test3.ID")]
				[MapField("TestID4", "Test4.ID")]
				public abstract class Dest
			{
				public abstract TestType Test1 { get; set; }
				public abstract TestType Test3 { get; set; }
				public abstract TestType Test4 { get; set; }
			}
		}

		[Test]
		public void TestInnerClass()
		{
			t6.Src  s = new t6.Src();
			t6.Dest d = (t6.Dest)Map.ToObject(s, typeof(t6.Dest));

			Assert.IsTrue  (d.Test1.ID.IsNull);
			Assert.AreEqual(d.Test3.ID, new SqlGuid((Guid)s.TestID3));
			Assert.AreEqual(d.Test4.ID, s.TestID4);
		}

		[Test]
		public void TestInnerClass2()
		{
			t6.Src  s = new t6.Src();
			t6.Dest d = (t6.Dest)Map.ToObject(s, typeof(t6.Dest));
			t6.Src s1 = (t6.Src)Map.ToObject(d, typeof(t6.Src));

			Assert.AreEqual(s1.TestID1, null);
			Assert.AreEqual(new SqlGuid((Guid)s1.TestID3), new SqlGuid((Guid)s.TestID3));
			Assert.AreEqual(s1.TestID4, s.TestID4);
		}
		#endregion

		#region TestParameters
		public class t7
		{
			public class Source
			{
				public int    ID   = 100;
				public string Name = "1000";
			}

			public abstract class InnerTarget
			{
				public InnerTarget(MapInitializingData data)
				{
					Param0 = data.Parameters[0];
					Param1 = data.Parameters[1];
				}

				public object Param0;
				public object Param1;
			}

			public abstract class Target
			{
				public Target(MapInitializingData data)
				{
					Param0 = data.Parameters[0];
					Param1 = data.Parameters[1];
				}

				public object Param0;
				public object Param1;

				public abstract InnerTarget InnerTarget { get; }
			}
		}

		[Test]
		public void TestParameters()
		{
			t7.Target t = (t7.Target)Map.ToObject(new t7.Source(), typeof(t7.Target), 123, "+456+");

			Assert.AreEqual(123,     t.Param0);
			Assert.AreEqual("+456+", t.Param1);
			Assert.AreEqual(123,     t.InnerTarget.Param0);
			Assert.AreEqual("+456+", t.InnerTarget.Param1);
		}

		public class t72
		{
			public abstract class InnerTarget
			{
				public InnerTarget(MapInitializingData data)
				{
					Param0 = data.MemberParameters[0];
					Param1 = data.MemberParameters[1];
				}

				public object Param0;
				public object Param1;
			}

			public abstract class Target
			{
				[MapParameter(1234, "+5678+")]
				public abstract InnerTarget InnerTarget { get; }
			}
		}

		[Test]
		public void TestParameters2()
		{
			t72.Target t = (t72.Target)Map.ToObject(new t7.Source(), typeof(t72.Target), 123, "+456+");

			Assert.AreEqual(1234,     t.InnerTarget.Param0);
			Assert.AreEqual("+5678+", t.InnerTarget.Param1);
		}

		#endregion

		#region TestIgnoreAttribute

		public class t8
		{
			public class Src
			{
				public int i1 = 1;
				[MapIgnore]
				public int i2 = 2;
				public int i3 = 3;
			}

			public class Dest
			{
				public int i1 = 10;
				public int i2 = 20;
				[MapIgnore]
				public int i3 = 30;
			}
		}

		[Test]
		public void TestIgnoreAttribute()
		{
			t8.Src  s = new t8.Src();
			t8.Dest d = (t8.Dest)Map.ToObject(s, typeof(t8.Dest));

			Assert.AreEqual(s.i1,             d.i1, "i1");
			Assert.AreEqual(new t8.Dest().i2, d.i2, "i2");
			Assert.AreEqual(new t8.Dest().i3, d.i3, "i3");
		}
		
		#endregion

		#region TestChangeType

		public class t9
		{
			public class Src
			{
				public string f1 = "123   ";
			}

			public class Dest
			{
				private int _f1;
				[MapField(SourceName = "f1", IsTrimmable = true)]
				public int Field1
				{
					get { return _f1;  }
					set { _f1 = value; }
				}
			}
		}

		[Test]
		public void TestChangeType()
		{
			t9.Src  s = new t9.Src();
			t9.Dest d = (t9.Dest)Map.ToObject(s, typeof(t9.Dest));

			Assert.AreEqual(123, d.Field1, "f1");
		}
		
		#endregion

		#region FromEnum

		public enum OperationDirection
		{
			[MapValue(0)] Income,
			[MapValue(1)] Outcome,
			[MapValue(2)] Discard,
			[MapValue(3)] Request
		} ;

		[Test]
		public void FromEnumTest()
		{
			object o = Map.FromEnum(OperationDirection.Income);

			Assert.AreEqual(0, o);
		}

		#endregion

		#region Test Onion

		public interface I1
		{
		}

		public abstract class O1 : I1
		{
			private        int _id = int.MinValue;
			public virtual int ID
			{
				get { return _id;  }
				set { _id = value; }
			}
		}

		public interface I2
		{
		}

		public abstract class O2 : O1, I2
		{
		}

		[MapField("SourceID", "ID")]
		public class O3 : O2, I2
		{
		}

		public class O4 : O3
		{
		}

		public class Source
		{
			public int SourceID = 10;
		}

		[Test]
		public void TestOnion()
		{
			O4 o = (O4)Map.ToObject(new Source(), typeof(O4));

			Assert.AreEqual(10, o.ID);
		}

		#endregion
	}
}
