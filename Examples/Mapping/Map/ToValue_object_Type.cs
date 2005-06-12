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
			Assert.AreEqual(Status.Active,   Map.ToEnum("A",  typeof(Status)));
			Assert.AreEqual(Status.Inactive, Map.ToEnum("I",  typeof(Status)));
			Assert.AreEqual(Status.Pending,  Map.ToEnum("P",  typeof(Status)));
			Assert.AreEqual(Status.Null,     Map.ToEnum(null, typeof(Status)));
			Assert.AreEqual(Status.Unknown,  Map.ToEnum(123,  typeof(Status)));
		}
	}
}