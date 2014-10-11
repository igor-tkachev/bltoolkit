using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLToolkit.Mapping;
using BLToolkit.Mapping.Fluent;
using NUnit.Framework;

namespace UnitTests.CS.Mapping
{
	[TestFixture]
	public class TestFluentNullable
	{
		public class TestObject
		{
			public string NullableString { get; set; }
		}

		public class TestObjectMap : FluentMap<TestObject>
		{
			public TestObjectMap ()
			{
				NullValue (x => x.NullableString, null).Nullable ();
			} 
		}

		[Test]
		public void TestFluentNullValue ()
		{
			var schema = new MappingSchema ();
			FluentConfig.Configure (schema).MapingFromType<TestObjectMap> ();
			var om = schema.GetObjectMapper (typeof (TestObject));
			var instance = new TestObject () { NullableString = "SOMEVALUE" };
			om.SetValue (instance, "NullableString", null);
			Assert.AreEqual (null, instance.NullableString);
		}


		

	}
}
