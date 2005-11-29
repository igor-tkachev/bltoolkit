using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Mapping
{
	[TestFixture]
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
			IObjectMapper om = Map.DefaultSchema.GetObjectMapper(typeof(TestObject));

			Assert.AreEqual(typeof(TestMapper), om.GetType());
		}
	}
}
