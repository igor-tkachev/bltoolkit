using System;
using System.ComponentModel;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.ComponentModel;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	[TestFixture]
	public class MixinAspectTest
	{
		[Mixin(typeof(ICustomTypeDescriptor), "_typeDescriptor")]
		public abstract class TestClass
		{
			public TestClass()
			{
				_typeDescriptor = new CustomTypeDescriptorImpl(GetType());
			}

			protected object _typeDescriptor;

			public string Code = "code";
		}

		[Test]
		public void Test()
		{
			TestClass             tc = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));
			ICustomTypeDescriptor td = (ICustomTypeDescriptor)tc;

			PropertyDescriptorCollection col = td.GetProperties();

			Assert.AreNotEqual(0, col.Count);
		}
	}
}
