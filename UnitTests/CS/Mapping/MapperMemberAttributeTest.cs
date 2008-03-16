using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.MetadataProvider;

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
			[MapIgnore(DebugSwitch), MemberMapper(typeof(MemberMapper1))]
			public int MapIgnore;

			// MapIgnore set to true
			//
			[MemberMapper(typeof(MemberMapper1)), MapIgnore]
			public int MapIgnore2;

			[MapIgnore(false), MemberMapper(typeof(MemberMapper1))]
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

		[MemberMapper(typeof(CustomNum.Mapper))]
		public class CustomNum
		{
			public int Num;

			public CustomNum(int num)
			{
				Num = num;
			}

			public CustomNum()
			{
			}

			public class Mapper : MemberMapper
			{
				public override object GetValue(object o)
				{
					object value = MemberAccessor.GetValue(o);
					return value is CustomNum? ((CustomNum)value).Num: 0;
				}

				public override void SetValue(object o, object value)
				{
					value = (value is int)? new CustomNum((int)value) : new CustomNum();
					MemberAccessor.SetValue(o, value);
				}
			}
		}

		public class Object3
		{
			public CustomNum Num;
		}

		[Test]
		public void TestNumberString()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object3));
			Object3      o  = new Object3();

			om.SetValue(o, "Num", 123);

			Assert.AreEqual(123, o.Num.Num);
		}

		#region Custom mapping schema test

		public class CustomString
		{
			public string Str;

			public CustomString(string str)
			{
				Str = str;
			}

			public CustomString()
			{
				Str = string.Empty;
			}

			public class Mapper : MemberMapper
			{
				public override object GetValue(object o)
				{
					object value = MemberAccessor.GetValue(o);
					return value is CustomString? ((CustomString)value).Str: null;
				}

				public override void SetValue(object o, object value)
				{
					value = (value is string)? new CustomString((string)value) : new CustomString();
					MemberAccessor.SetValue(o, value);
				}
			}
		}

		public class Object4
		{
			public CustomString Str;
		}

		public class TestMappingSchema : MappingSchema
		{
			private class TestMapMetadataProvider : MetadataProviderBase
			{
				public override bool GetIgnore(ObjectMapper mapper, MemberAccessor member, out bool isSet)
				{
					if (member.Type == typeof(CustomString))
					{
						isSet = true;
						return false;
					}

					return base.GetIgnore(mapper, member, out isSet);
				}
			}

			private class TestObjectMapper : ObjectMapper
			{
				protected override  MemberMapper CreateMemberMapper(MapMemberInfo mapMemberInfo)
				{
					if (mapMemberInfo.Type == typeof(CustomString))
					{
						MemberMapper mm = new CustomString.Mapper();
						mm.Init(mapMemberInfo);
						return mm;
					}

					return base.CreateMemberMapper(mapMemberInfo);
				}

				protected override MetadataProviderBase CreateMetadataProvider()
				{
					MetadataProviderBase provider = base.CreateMetadataProvider();
					provider.AddProvider(new TestMapMetadataProvider());
					return provider;
				}

			}

			protected override ObjectMapper CreateObjectMapperInstance(Type type)
			{
				return new TestObjectMapper();
			}
		}

		[Test]
		public void TestCustomString()
		{
			MappingSchema save = Map.DefaultSchema;

			Map.DefaultSchema = new TestMappingSchema();
			
			ObjectMapper om = Map.GetObjectMapper(typeof(Object4));

			Object4 o = new Object4();

			om.SetValue(o, "Str", "Test");

			Assert.AreEqual("Test", o.Str.Str);
			
			Map.DefaultSchema = save;
		}

		#endregion
	}
}
