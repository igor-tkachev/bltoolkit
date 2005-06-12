/// example:
/// map FromValue(object)
/// comment:
/// The following example demonstrates how to use the <b>FromValue</b> method.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_Map
{
	[TestFixture]
	public class FromValue_object
	{
		[MapValue(Status.Active,   "A")]
		[MapValue(Status.Inactive, "I")]
		[MapValue(Status.Pending,  "P")]
		[MapNullValue(Status.Null)]
		public enum Status
		{
			Active,
			Inactive,
			Pending,
			Null
		}

		[Test]
		public void Test()
		{
			Assert.AreEqual("A",  Map.FromEnum(Status.Active));
			Assert.AreEqual("I",  Map.FromEnum(Status.Inactive));
			Assert.AreEqual("P",  Map.FromEnum(Status.Pending));
			Assert.AreEqual(null, Map.FromEnum(Status.Null));
		}
	}
}
