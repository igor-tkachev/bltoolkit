/// example:
/// map ToValue(object,Type)
/// comment:
/// The following example demonstrates how to use the <b>ToValue</b> method.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_Map
{
	[TestFixture]
	public class ToValue_object_Type
	{
		[MapValue       (Status.Active,   "A")]
		[MapValue       (Status.Inactive, "I")]
		[MapValue       (Status.Pending,  "P")]
		[MapNullValue   (Status.Null)]
		[MapDefaultValue(Status.Unknown)]
		public enum Status
		{
			Active,
			Inactive,
			Pending,
			Unknown,
			Null
		}

		[Test]
		public void Test()
		{
			Assert.AreEqual(Status.Active,   Map.ToValue("A",  typeof(Status)));
			Assert.AreEqual(Status.Inactive, Map.ToValue("I",  typeof(Status)));
			Assert.AreEqual(Status.Pending,  Map.ToValue("P",  typeof(Status)));
			Assert.AreEqual(Status.Null,     Map.ToValue(null, typeof(Status)));
			Assert.AreEqual(Status.Unknown,  Map.ToValue(123,  typeof(Status)));
		}
	}
}