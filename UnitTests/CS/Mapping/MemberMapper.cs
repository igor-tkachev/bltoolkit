using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class MemberMapper
	{
		public class Object1
		{
			public int   Int32;
			public float Float;
		}

		[Test]
		public void PrimitiveMemberTest()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			om.SetValue(o, "Int32", 123.56);
			om.SetValue(o, "Float", "123.57");

			Assert.AreEqual(124,    o.Int32);
			Assert.AreEqual(123.57, o.Float);
		}

#if FW2
		public class Object2
		{
			public short? Int16;
			public int?   Int32;
			public long?  Int64;
			public float? Float;
			public Guid?  Guid;
		}

		[Test]
		public void NullableMemberTest()
		{
			IObjectMapper om = Map<Object2>.Mapper;

			Object2 o = new Object2();

			short? s = 125;
			Guid   g = Guid.NewGuid();

			om.SetValue(o, "Int16", s);
			om.SetValue(o, "Int32", 123.56);
			om.SetValue(o, "Int64", null);
			om.SetValue(o, "Float", "123.57");
			om.SetValue(o, "Guid",  g);

			Assert.AreEqual(125, o.Int16);
			Assert.AreEqual(124, o.Int32);
			Assert.IsNull (o.Int64);
			Assert.AreEqual(123.57, o.Float);
			Assert.AreEqual(g,   o.Guid);
		}
#endif
	}
}
