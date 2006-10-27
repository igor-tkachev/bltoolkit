using System;
using System.Data.SqlTypes;
using System.Globalization;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class MemberMapperTest
	{
		public class Object1
		{
			public int       Int32;
			public float     Float;
			public DayOfWeek Dow1;
			public DayOfWeek Dow2;
		}

		[Test]
		public void PrimitiveMemberTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			DayOfWeek de = DayOfWeek.Thursday;
			short     di = (short)de;

			om.SetValue(o, "Int32", 123.56);
			om.SetValue(o, "Float", 123.57.ToString());
			om.SetValue(o, "Dow1",  de);
			om.SetValue(o, "Dow2",  di);

			Assert.AreEqual(124,    om.GetValue(o, "Int32"));
			Assert.AreEqual(123.57, om.GetValue(o, "Float"));
			Assert.AreEqual(de,     om.GetValue(o, "Dow1"));
			Assert.AreEqual(de,     om.GetValue(o, "Dow2"));

			Assert.IsNull(om.GetValue(o, "blah-blah-blah"));
		}

#if FW2
		public class Object2
		{
			public short?     Int16;
			public int?       Int32;
			public long?      Int64;
			public float?     Float;
			public Guid?      Guid;
			public DayOfWeek? Dow1;
			public DayOfWeek? Dow2;
		}

		[Test]
		public void NullableMemberTest()
		{
			Object2 o = new Object2();

			short?    s  = 125;
			Guid      g  = Guid.NewGuid();
			DayOfWeek de = DayOfWeek.Thursday;
			int       di = (int)de;

			ObjectMapper<Object2>.SetValue(o, "Int16", s);
			ObjectMapper<Object2>.SetValue(o, "Int32", 123.56);
			ObjectMapper<Object2>.SetValue(o, "Int64", null);
			ObjectMapper<Object2>.SetValue(o, "Float", 123.57.ToString());
			ObjectMapper<Object2>.SetValue(o, "Guid",  (Guid?)g);
			ObjectMapper<Object2>.SetValue(o, "Guid",  g);
			ObjectMapper<Object2>.SetValue(o, "Dow1",  de);
			ObjectMapper<Object2>.SetValue(o, "Dow1",  (DayOfWeek?)de);
			ObjectMapper<Object2>.SetValue(o, "Dow2",  di);

			Assert.AreEqual(125,    o.Int16);
			Assert.AreEqual(124,    o.Int32);
			Assert.IsNull  (        o.Int64);
			Assert.AreEqual(123.57, o.Float);
			Assert.AreEqual(g,      o.Guid);
			Assert.AreEqual(de,     o.Dow1);
			Assert.AreEqual(de,     o.Dow2);

			Assert.AreEqual(125,    ObjectMapper<Object2>.GetValue(o, "Int16"));
			Assert.AreEqual(124,    ObjectMapper<Object2>.GetValue(o, "Int32"));
			Assert.IsNull  (        ObjectMapper<Object2>.GetValue(o, "Int64"));
			Assert.AreEqual(123.57, ObjectMapper<Object2>.GetValue(o, "Float"));
			Assert.AreEqual(g,      ObjectMapper<Object2>.GetValue(o, "Guid"));
			Assert.AreEqual(de,     ObjectMapper<Object2>.GetValue(o, "Dow1"));
			Assert.AreEqual(de,     ObjectMapper<Object2>.GetValue(o, "Dow2"));
		}
#endif

		public class Object3
		{
			public SqlInt32  Int32;
			public SqlSingle Single;
		}

		[Test]
		public void SqlTypeMemberTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object3));

			Object3 o = new Object3();

			om.SetValue(o, "Int32",  123.56);
			om.SetValue(o, "Single", 123.57.ToString(CultureInfo.InvariantCulture));

			Assert.AreEqual(124,    o.Int32. Value);
			Assert.AreEqual(123.57f, o.Single.Value);

			Assert.AreEqual(124,    om.GetValue(o, "Int32"));
			Assert.AreEqual(123.57f, om.GetValue(o, "Single"));
		}

		public interface IClassInterface
		{
			IClassInterface classInterface { get; set;}
		}

		public class ClassInterface : IClassInterface
		{
			private IClassInterface _ici;

			[MapIgnore(false)]
			public IClassInterface classInterface
			{
				get { return _ici; }
				set { _ici = value; }
			}
		}

		[Test]
		public void DerivedTypeTest()
		{
			IClassInterface ici = new ClassInterface();
			ObjectMapper om = Map.GetObjectMapper(ici.GetType());
			MemberMapper mm = om["classInterface"];
			mm.SetValue(ici, new ClassInterface());
		}
	}
}
