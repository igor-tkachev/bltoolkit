using System;

using NUnit.Framework;
using NullableTypes;
using Rsdn.Framework.Data.Mapping;

namespace AllTest
{
	[TestFixture]
	public class NullableTypesTest
	{
		public class Src
		{
			public string Field1 = "123";
			public int    Field2 = 123;
			public object Field3 = DBNull.Value;
		}

		public class Dest
		{
			public NullableString Field1;
			public NullableInt32  Field2;
			public NullableByte   Field3;
		}

		[Test]
		public void Test()
		{
			Dest dest = (Dest)Map.ToObject(new Src(), typeof(Dest));

			Assert.AreEqual("123", dest.Field1.Value);
			Assert.AreEqual(123,   dest.Field2.Value);
			Assert.AreEqual(true,  dest.Field3.IsNull);
		}
	}
}
