using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class LazyInstanceAttributeTest
	{
		public abstract class AbstractObject
		{
			public AbstractObject(InitContext init)
			{
				if (init.MemberParameters != null && init.MemberParameters.Length == 1)
					Field = (int)init.MemberParameters[0];
				else
					Field = 77;
			}

			public int Field;
		}

		public class InnerObject
		{
			public InnerObject(InitContext init)
			{
				if (init.MemberParameters != null && init.MemberParameters.Length == 1)
					Field = (int)init.MemberParameters[0];
				else
					Field = 44;
			}

			public int Field;
		}

		public class TestField
		{
			public TestField()
			{
				Value = 10;
			}

			public TestField(int p1, float p2)
			{
				Value = p1 + (int)p2;
			}

			public TestField(TestField p1)
			{
				Value = 77;
			}

			public int Value;
		}

		public abstract class TestObject1
		{
			[LazyInstance]
			public abstract ArrayList List { get; set; }

			[LazyInstance]
			public abstract string    Str { get; set; }

			[LazyInstance]
			public abstract string this[int i] { get; set; }

			[LazyInstance]
			public abstract TestField Field { get; set; }

			[LazyInstance]
			public abstract InnerObject InnerObject { get; set; }

			[LazyInstance]
			public abstract AbstractObject AbstractObject { get; set; }
		}

		[Test]
		public void NoParamTest()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			Assert.IsNotNull(o.List);
			Assert.AreEqual("", o.Str);
			Assert.AreEqual(10, o.Field.Value);
			Assert.AreEqual(44, o.InnerObject.Field);
		}

		[AttributeUsage(AttributeTargets.Property)]
		public class TestParameterAttribute : ParameterAttribute
		{
			public TestParameterAttribute()
				: base(new TestField())
			{
			}
		}

		public abstract class TestObject2
		{
			[LazyInstance, Parameter(10)]
			public abstract ArrayList List { get; set; }

			[LazyInstance, Parameter("test")]
			public abstract string Str { get; set; }

			[LazyInstance, Parameter(20)]
			public abstract string this[int i] { get; set; }

			[LazyInstance, Parameter(20, 30)]
			public abstract TestField Field1 { get; set; }

			[LazyInstance, TestParameter]
			public abstract TestField Field2 { get; set; }

			[LazyInstance, Parameter(55)]
			public abstract InnerObject InnerObject { get; set; }

			[LazyInstance, Parameter(88)]
			public abstract AbstractObject AbstractObject { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			TestObject2 o = (TestObject2)TypeAccessor.CreateInstance(typeof(TestObject2));

			Assert.AreEqual(10,     o.List.Capacity);
			Assert.AreEqual("test", o.Str);
			Assert.AreEqual(50,     o.Field1.Value);
			Assert.AreEqual(77,     o.Field2.Value);
			Assert.AreEqual(55,     o.InnerObject.Field);
		}

		[LazyInstances]
		public abstract class TestObject3
		{
			public abstract string Str1 { get; set; }
			[LazyInstance(false), Parameter("")]
			public abstract string Str2 { get; set; }
		}

		[Test]
		public void LazyInstancesTest()
		{
			TestObject3 o = (TestObject3)TypeAccessor.CreateInstance(typeof(TestObject3));

			Assert.AreEqual("", o.Str1);
			Assert.AreEqual("", o.Str2);

			o.Str1 = null;
			o.Str2 = null;

			Assert.AreEqual("",   o.Str1);
			Assert.AreEqual(null, o.Str2);
		}

		[LazyInstances(false)]
		public abstract class TestObject4 : TestObject3
		{
		}

		[Test]
		public void LazyInstancesFalseTest()
		{
			TestObject4 o = (TestObject4)TypeAccessor.CreateInstance(typeof(TestObject4));

			Assert.AreEqual("", o.Str1);
			Assert.AreEqual("", o.Str2);

			o.Str1 = null;
			o.Str2 = null;

			Assert.AreEqual(null, o.Str1);
			Assert.AreEqual(null, o.Str2);
		}

		[LazyInstances(typeof(string))]
		public abstract class TestObject5
		{
			public abstract string    Str  { get; set; }
			public abstract ArrayList List { get; set; }
		}

		[Test]
		public void LazyInstancesTypeTest()
		{
			TestObject5 o = (TestObject5)TypeAccessor.CreateInstance(typeof(TestObject5));

			Assert.IsNotNull(o.Str);
			Assert.IsNotNull(o.List);

			o.Str  = null;
			o.List = null;

			Assert.IsNotNull(o.Str);
			Assert.AreEqual (null, o.List);
		}
	}
}
