using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.ComponentModel;
using BLToolkit.Reflection;
using NUnit.Framework.SyntaxHelpers;

namespace Aspects
{
	[TestFixture]
	public class MixinAspectTest
	{
		public interface ITestInterface
		{
			int    Test1(ref int    value);
			object Test2(ref object value);

			int    Test3(string p);
			int    Test4(double p);
			int    Test5 { get; }
			int    Test6 { get; }
		}

		public class TestInterfaceImpl : ITestInterface, INullable
		{
			int    ITestInterface.Test1(ref int    value) { return value; }
			object ITestInterface.Test2(ref object value) { return value; }

			int    ITestInterface.Test3(string p) { return 10; }
			int    ITestInterface.Test4(double p) { return 20; }

			int    ITestInterface.Test5 { get { return 30; } }
			int    ITestInterface.Test6 { get { return 40; } }

			bool   INullable.IsNull     { get { return true; } }
		}

		[Mixin(typeof(ICustomTypeDescriptor), "_typeDescriptor")]
		[Mixin(typeof(ITestInterface),        "TestInterface", "'{0}.{1}' is null.")]
		[Mixin(typeof(INullable),             "TestInterface", "'{0}.{1}' is null.")]
		public abstract class TestClass
		{
			public TestClass()
			{
				_typeDescriptor = new CustomTypeDescriptorImpl(GetType());
			}

			protected object _typeDescriptor;

			private ITestInterface _testInterface;
			public  ITestInterface  TestInterface
			{
				get
				{
					if (_testInterface == null)
						_testInterface = new TestInterfaceImpl();
					return _testInterface;
				}
			}

			public string Code = "code";

			[MixinOverride]
			protected int Test3(string p) { return 15; }
			[MixinOverride(typeof(IDisposable))]
			protected int Test4(double p) { return 25; }

			protected int Test5 { [MixinOverride] get { return 35; } }
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
			INullable      tn = (INullable)tc;

			int    n = 10;
			object o = new object();

			Assert.AreEqual(10, ti.Test1(ref n));
			Assert.AreSame (o,  ti.Test2(ref o));
			Assert.AreEqual(15, ti.Test3(null));
			Assert.AreEqual(20, ti.Test4(0));
			Assert.AreEqual(35, ti.Test5);
			Assert.AreEqual(40, ti.Test6);
			Assert.That(tn.IsNull, Is.True);
		}
	}
}
