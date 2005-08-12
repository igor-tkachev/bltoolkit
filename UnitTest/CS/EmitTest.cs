using System;
using System.Data;
using System.Data.SqlTypes;

#if !MSTEST
using NUnit.Framework;
#else
using Microsoft.VisualStudio.QualityTools.UnitTesting.Framework;
using TestFixture = Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestClassAttribute;
using Test        = Microsoft.VisualStudio.QualityTools.UnitTesting.Framework.TestMethodAttribute;
#endif

using Rsdn.Framework.Data;
using Rsdn.Framework.Data.Mapping;

namespace CS
{
	[TestFixture]
	public class EmitTest
	{
		public class Dest
		{
			public SqlString   SqlString;
			public SqlInt32    SqlInt32;
			public SqlDateTime SqlDateTime;
		}

		private Dest _dest = new Dest();

		private static MapDescriptor MapDest
		{
			get { return MapDescriptor.GetDescriptor(typeof(Dest)); }
		}

		[Test]
		public void SetSqlStringToString()
		{
			MapDest["SqlString"].SetValue(_dest, "123");
			Assert.IsTrue(_dest.SqlString.Value == "123");
		}

		#region Abstract Members

		public class t1
		{
			public class Prop1
			{
				private int _value;
				public  int  Value
				{
					get { return _value;  }
					set { _value = value; }
				}
			}

			public class Prop2
			{
				public int Value;
			}

			public class Src
			{
				public string f1 = "123";
				public int    p1 = 1234;
				public int    p2 = 5678;
			}

			public abstract class InnerDest
			{
				public abstract string f1 { get; }
				[MapType(typeof(Prop1))] public abstract int p1 { get; set; }
				[MapType(typeof(Prop2))] public abstract int p2 { get; set; }
			}

			public abstract class Dest
			{
				[MapIgnore]
				public int MappedObjects = 0;

				public Dest(MapInitializingData data)
				{
					object[] mo = ((IMapGenerated)this).GetCreatedMembers();

					MappedObjects = mo.Length;
				}

				                         public abstract string    f1 { get; }
				[MapType(typeof(Prop1))] public abstract int       p1 { get; set; }
				[MapType(typeof(Prop2))] public abstract int       p2 { get; set; }
				                         public abstract InnerDest id { get; }
			}
		}

		[Test]
		public void AbstractProperty()
		{
			t1.Dest d = (t1.Dest)Map.ToObject(new t1.Src(), typeof(t1.Dest));

			Assert.AreEqual("123", d.f1);
			Assert.AreEqual(1234,  d.p1);
			Assert.AreEqual(3,     d.MappedObjects);
		}

		public class a2
		{
			public class Prop
			{
				public static object param1;
				public static object param2;
				public static object param3;

				public Prop() {}

				public Prop(string p0, int p1)
				{
					param2 = p0;
					param3 = p1;
				}

				public Prop(MapInitializingData data)
				{
					if (data.MemberParameters != null && data.MemberParameters.Length == 1)
					{
						param1 = data.MemberParameters[0];
					}
				}

				private int _value;
				public  int  Value
				{
					get { return _value;  }
					set { _value = value; }
				}
			}

			public class Src
			{
				public int p1 = 1234;
				public int p2 = 4321;
			}

			public abstract class Dest
			{
				[MapType(typeof(Prop), 10)]
				//[MapParameter(10)]
				public abstract int p1 { get; set; }

				[MapType(typeof(Prop))]
				[MapParameter("20", 30)]
				public abstract int p2 { get; set; }
			}
		}

		[Test]
		public void AbstractParametrizedProperty()
		{
			a2.Dest d = (a2.Dest)Map.ToObject(new a2.Src(), typeof(a2.Dest));

			Assert.AreEqual(10,   a2.Prop.param1);
			Assert.AreEqual("20", a2.Prop.param2);
			Assert.AreEqual(30,   a2.Prop.param3);
		}

		[Test]
		public void AbstractParametrizedPropertyDef()
		{
			a2.Dest d = (a2.Dest)Map.Descriptor(typeof(a2.Dest)).CreateInstance();

			Assert.AreEqual(10,   a2.Prop.param1);
			Assert.AreEqual("20", a2.Prop.param2);
			Assert.AreEqual(30,   a2.Prop.param3);
		}

		public class p1
		{
			public class Src
			{
				public int p1 = 1234;

				public int ppppp { get { return p1; } }
			}

			public abstract class Dest
			{
				protected abstract int p1 { get; set; }
				[MapIgnore]
				public int p2 { get { return p1; } }
			}
		}

		[Test]
		public void AbstractProtectedProperty()
		{
			p1.Dest d = (p1.Dest)Map.ToObject(new p1.Src(), typeof(p1.Dest));

			Assert.AreEqual(1234, d.p2);
		}

		public class InnerStruct
		{
			public class Src
			{
				public int p = 1234;
			}

			public struct MyInt
			{
				public int Value;
			}

			public abstract class Dest
			{
				[MapType(typeof(MyInt))]
				public abstract int p { get; set; }
			}
		}

		[Test]
		public void InnerStructTest()
		{
			InnerStruct.Dest d = 
				(InnerStruct.Dest)Map.ToObject(new InnerStruct.Src(), typeof(InnerStruct.Dest));

			Assert.AreEqual(1234, d.p);
		}

		#endregion
	}
}
