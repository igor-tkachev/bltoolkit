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
			// MapIgnore set to false
			//
			[MemberMapper(typeof(MemberMapper1))]
			public int Int;

			// MapIgnore set to DebugSwitch value
			//
			[MemberMapper(typeof(MemberMapper1), Ignore = DebugSwitch)]
			public int MapIgnore;

			// MapIgnore set to true, since MapIgnore comes first
			//
			[MapIgnore, MemberMapper(typeof(MemberMapper1))]
			public int MapIgnore2;

			// MapIgnore set to false, since MemberMapper comes first
			//
			[MemberMapper(typeof(MemberMapper1)), MapIgnore]
			public int MapNotIgnore;

			private const bool DebugSwitch = true;
		}

		[Test]
		public void Test1()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			om.SetValue(o, "Int",      456);

			Assert.AreEqual(456, o.Int);
			Assert.AreEqual(45,  om.GetValue(o, "Int"));

			Assert.IsNull(om["MapIgnore"]);
			Assert.IsNull(om["MapIgnore2"]);
			Assert.IsNotNull(om["MapNotIgnore"]);
		}

		[MemberMapper(typeof(int), typeof(MemberMapper1))]
		public class Object2
		{
			public int Int;
		}

		[Test]
		public void Test2()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object2));

			Object2 o = new Object2();

			om.SetValue(o, "Int", 456);

			Assert.AreEqual(456, o.Int);
			Assert.AreEqual(45,  om.GetValue(o, "Int"));
		}
	}
}
