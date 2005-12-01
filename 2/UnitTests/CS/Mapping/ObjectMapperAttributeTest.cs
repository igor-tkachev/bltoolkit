using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture, Category("Mapping")]
	public class ObjectMapperAttributeTest
	{
		class TestMapper : ObjectMapper
		{
		}

		[ObjectMapper(typeof(TestMapper))]
		public class TestObject
		{
		}

		[Test]
		public void Test()
		{
			IObjectMapper om = Map.GetObjectMapper(typeof(TestObject));

			Assert.AreEqual(typeof(TestMapper), om.GetType());
		}
	}
}
