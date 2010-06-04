using System;
using System.ComponentModel;

using NUnit.Framework;

using BLToolkit.Reflection;

namespace Reflection
{
	[TestFixture]
	public class ExtendedPropertyDescriptorTest
	{
		public class RootType
		{
			private Type1 _type1 = new Type1();
			public  Type1  Type1 { get { return _type1; } }
		}

		public class Type1
		{
			private Type2 _type2 = new Type2();
			public  Type2  Type2 { get { return _type2; } }
		}

		public class Type2
		{
			public string Name
			{
				get { return "test";  }
			}
		}

		[Test]
		public void Test()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(RootType));

			PropertyDescriptorCollection col  = ta.CreateExtendedPropertyDescriptors(null, null);
			PropertyDescriptor           prop = col["Type1+Type2+Name"];

			RootType obj = new RootType();

			object value = prop.GetValue(obj);

			Assert.AreEqual("test", value);
		}
	}
}
