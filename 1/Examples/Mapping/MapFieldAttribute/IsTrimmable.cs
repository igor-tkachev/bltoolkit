/// example:
/// mapfield IsTrimmable
/// comment:
/// The following example demonstrates how to use the <b>MapFieldAttribute.IsTrimmable</b> property.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapFieldAttribute
{
	[TestFixture]
	public class IsTrimmable
	{
		public class Source
		{
			public string String1 = "test1    ";
			public string String2 = "test2    ";
		}

		public class Target
		{
			public string String1;

			[MapField(IsTrimmable=false)]
			public string String2;
		}

		[Test]
		public void Test()
		{
			Target t = (Target)Map.ToObject(new Source(), typeof(Target));

			Assert.AreEqual("test1",     t.String1);
			Assert.AreEqual("test2    ", t.String2);
		}
	}
}
