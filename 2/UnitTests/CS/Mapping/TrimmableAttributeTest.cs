using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class TrimmableAttributeTest
	{
		[Trimmable]
		interface Interface1
		{
		}

		public class TestObject : Interface1
		{
			[Trimmable(false)]
			public string Str1;
			public string Str2;
		}

		[Test]
		public void Test()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(TestObject));

			TestObject o = new TestObject();

			om.SetValue(o, "Str1", "1  ");
			om.SetValue(o, "Str2", "2  ");

			Assert.AreEqual("1  ", o.Str1);
			Assert.AreEqual("2",   o.Str2);
		}
	}
}
