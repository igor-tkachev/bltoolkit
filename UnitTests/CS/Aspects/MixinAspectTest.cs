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
		public interface ITestInterface
		{
			int    Test1(ref int    value);
			object Test2(ref object value);
		}

		public class TestInterfaceImpl : ITestInterface
		{
			int    ITestInterface.Test1(ref int    value) { return value; }
			object ITestInterface.Test2(ref object value) { return value; }
		}

		[Mixin(typeof(ICustomTypeDescriptor), "_typeDescriptor")]
		[Mixin(typeof(ITestInterface),        "_testInterface")]
		public abstract class TestClass
		{
			public TestClass()
			{
				_typeDescriptor = new CustomTypeDescriptorImpl(GetType());
			}

			protected object _typeDescriptor;
			protected object _testInterface = new TestInterfaceImpl();

			public string Code = "code";
		}

		[Test]
		public void Test1()
		{
			TestClass             tc = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));
			ICustomTypeDescriptor td = (ICustomTypeDescriptor)tc;

			PropertyDescriptorCollection col = td.GetProperties();

			Assert.AreNotEqual(0, col.Count);
		}

		[Test]
		public void Test2()
		{
			TestClass      tc = (TestClass)TypeAccessor.CreateInstance(typeof(TestClass));
			ITestInterface ti = (ITestInterface)tc;

			int    n = 10;
			object o = new object();

			Assert.AreEqual(10, ti.Test1(ref n));
			Assert.AreSame (o,  ti.Test2(ref o));
		}
	}
}
