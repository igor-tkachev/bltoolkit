using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class MapFieldAttributeTest
	{
		[MapField("MapName", "Field1")]
		public class Object1
		{
			public int Field1;
			[MapField("intfld")]
			public int Field2;
		}

		[Test]
		public void Test()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = (Object1)om.CreateInstance();

			om.SetValue(o, "mapname", 123);
			om.SetValue(o, "intfld",  234);

			Assert.AreEqual(123, o.Field1);
			Assert.AreEqual(234, o.Field2);
		}
	}
}
