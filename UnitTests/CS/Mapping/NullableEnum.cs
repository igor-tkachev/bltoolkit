using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Mapping
{
	[TestFixture]
	public class NullableEnum
	{
		public class Source
		{
			public Source(string value)
			{
				Field = value;
			}

			public string Field;
		}

		public enum StrEnum
		{
			[MapValue("str")]  Str,
			[MapValue("str1")] Str1
		}

		public class StrDest1
		{
			public StrEnum? Field;
		}

		[Test]
		public void Test1()
		{
			StrDest1 sd = Map.ObjectToObject<StrDest1>(new Source("str"));

			Assert.AreEqual(StrEnum.Str, sd.Field);
		}

		[Test]
		public void Test2()
		{
			StrDest1 sd = Map.ObjectToObject<StrDest1>(new Source(null));

			Assert.IsNull(sd.Field);
		}

		public class StrDest2
		{
			public Mapping.EnumExtension.CountryCodeEnum? Field;
		}

		[Test]
		public void Test3()
		{
			Map.Extensions = TypeExtension.GetExtensions("Map.xml");

			StrDest2 sd = Map.ObjectToObject<StrDest2>(new Source("AL"));

			Assert.AreEqual(Mapping.EnumExtension.CountryCodeEnum.AL, sd.Field);
		}

		[Test]
		public void Test4()
		{
			Map.Extensions = TypeExtension.GetExtensions("Map.xml");

			StrDest2 sd = Map.ObjectToObject<StrDest2>(new Source(null));

			Assert.IsNull(sd.Field);
		}

		public enum IntEnum
		{
			Str  = 1,
			Str1 = 2
		}

		public class StrDest3
		{
			public IntEnum? Field;
		}

		[Test]
		public void Test5()
		{
			StrDest3 sd = Map.ObjectToObject<StrDest3>(new Source("1"));

			Assert.AreEqual(IntEnum.Str, sd.Field);
		}

		[Test]
		public void Test6()
		{
			StrDest3 sd = Map.ObjectToObject<StrDest3>(new Source(null));

			Assert.IsNull(sd.Field);
		}
	}
}
