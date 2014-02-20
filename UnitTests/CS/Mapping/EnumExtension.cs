using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Mapping
{
	[TestFixture]
	public class EnumExtension
	{
		public enum CountryCodeEnum
		{
			AF,
			AL,
			DZ,
			AS,
			AD
		}

		public enum OtherEnum
		{
			EnumValue1,
			EnumValue2
		}

		[Test]
		public void Test1()
		{
			Map.Extensions = TypeExtension.GetExtensions("Map.xml");

			Assert.AreEqual("AL", Map.EnumToValue(CountryCodeEnum.AL));
			Assert.AreEqual(101,  Map.EnumToValue(OtherEnum.EnumValue2));
		}

		public class TestObj
		{
			[MapField("country_code")] public CountryCodeEnum1 Country { get; set; }
			[MapField("other")]        public OtherEnum1       Other   { get; set; }
		}

		public enum CountryCodeEnum1
		{
			[MapValue("AFA")] AF,
			[MapValue("ALA")] AL,
			[MapValue("DZA")] DZ,
			[MapValue("ASA")] AS,
			[MapValue("ADA")] AD
		}

		public enum OtherEnum1
		{
			[MapValue("v1")] EnumValue1,
			[MapValue("v2")] EnumValue2
		}

		[Test]
		public void EnumToValueTest()
		{
			Map.Extensions = TypeExtension.GetExtensions("Map.xml");

			Assert.AreEqual("AL", Map.EnumToValue(CountryCodeEnum1.AL));
			Assert.AreEqual(101,  Map.EnumToValue(OtherEnum1.EnumValue2));
		}

		[Test]
		public void ObjToDicTest()
		{
			var obj  = new TestObj { Country = CountryCodeEnum1.DZ, Other = OtherEnum1.EnumValue2 };
			var sh   = new MappingSchema { Extensions = TypeExtension.GetExtensions("Map.xml") };
			var pars = sh.MapObjectToDictionary(obj);

			Assert.AreEqual("DZ", pars["country_code"]);
			Assert.AreEqual(101,  pars["other"]);
		}
	}
}
