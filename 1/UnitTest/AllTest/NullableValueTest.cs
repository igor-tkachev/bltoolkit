using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace AllTest
{
	[TestFixture]
	public class NullableValueTest
	{
		public class Src
		{
			public int    Field1 = 123;
			public object Field2 = null;
			public object Field3 = DBNull.Value;
		}

		public class Dest
		{
			public int?   Field1;
			public byte?  Field2;
			public short? Field3;
		}

		[Test]
		public void Test()
		{
			Dest dest = (Dest)Map.ToObject(new Src(), typeof(Dest));

			Assert.AreEqual(123,   dest.Field1.Value);
			Assert.AreEqual(false, dest.Field2.HasValue);
			Assert.AreEqual(false, dest.Field3.HasValue);
		}
	}
}
