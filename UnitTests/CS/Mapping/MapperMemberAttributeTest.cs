using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
	public class MapperMemberAttributeTest
	{
		class MemberMapper1 : MemberMapper
		{
			public override object GetValue(object o)
			{
				return 45;
			}
		}

		public class Object1
		{
			[MemberMapper(typeof(MemberMapper1))]
			public int Int;
		}

		[Test]
		public void Test1()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			om.SetValue(o, "Int", 456);

			Assert.AreEqual(456, o.Int);
			Assert.AreEqual(45,  om.GetValue(o, "Int"));
		}

		[MemberMapper(typeof(int), typeof(MemberMapper1))]
		public class Object2
		{
			public int Int;
		}

		[Test]
		public void Test2()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = new Object2();

			om.SetValue(o, "Int", 456);

			Assert.AreEqual(456, o.Int);
			Assert.AreEqual(45,  om.GetValue(o, "Int"));
		}
	}
}
