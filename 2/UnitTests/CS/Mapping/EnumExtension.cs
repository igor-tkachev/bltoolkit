using System;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace A.Mapping
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
		public void Test()
		{
			Map.Extensions = TypeExtension.GetExtenstions("Map.xml");

			Assert.AreEqual("AL", Map.EnumToValue(CountryCodeEnum.AL));
			Assert.AreEqual(101,  Map.EnumToValue(OtherEnum.EnumValue2));
		}
	}
}
