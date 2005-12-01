using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder
{
	[TestFixture]
	public class InstanceTypeAttributeTest
	{
		public InstanceTypeAttributeTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public class TestClass
		{
			public class IntFieldInstance
			{
				public int Value;
			}

			public class IntPropertyInstance
			{
				private int _value;
				public  int  Value
				{
					get { return _value * 2; }
					set { _value = value; }
				}
			}

			public class ObjFieldInstance
			{
				public object Value;
			}

			public class ObjPropertyInstance
			{
				private object _value;
				public  object  Value
				{
					get { return _value is int? (int)_value * 3: _value; }
					set { _value = value; }
				}
			}

			public abstract class TestObject1
			{
				[InstanceType(typeof(IntFieldInstance))]    public abstract int       IntField  { get; set; }
				[InstanceType(typeof(IntPropertyInstance))] public abstract int       IntProp   { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract int       ObjField  { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract int       ObjProp   { get; set; }
				[InstanceType(typeof(IntFieldInstance))]    public abstract DayOfWeek DowField  { get; set; }
				[InstanceType(typeof(IntPropertyInstance))] public abstract DayOfWeek DowProp   { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract DateTime  DateField { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract DateTime  DateProp  { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract ArrayList ArrField  { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract ArrayList ArrProp   { get; set; }
			}

			public static void Test()
			{
				TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

				o.IntField = 10;
				o.IntProp  = 11;
				o.ObjField = 12;
				o.ObjProp  = 13;

				Assert.AreEqual(10, o.IntField);
				Assert.AreEqual(22, o.IntProp);
				Assert.AreEqual(12, o.ObjField);
				Assert.AreEqual(39, o.ObjProp);

				DateTime  testDate  = new DateTime(2000, 1, 1);

				o.DowField  = DayOfWeek.Monday;
				o.DowProp   = DayOfWeek.Sunday;
				o.DateField = testDate;
				o.DateProp  = testDate;

				Assert.AreEqual(DayOfWeek.Monday, o.DowField);
				Assert.AreEqual(DayOfWeek.Sunday, o.DowProp);
				Assert.AreEqual(testDate,         o.DateField);
				Assert.AreEqual(testDate,         o.DateProp);

				o.ArrField = new ArrayList(17);
				o.ArrProp  = new ArrayList(21);

				Assert.AreEqual(17, o.ArrField.Capacity);
				Assert.AreEqual(21, o.ArrProp. Capacity);
			}
		}

		[Test]
		public void InstanceClassTest()
		{
			TestClass.Test();
		}

		public class TestStruct
		{
			public struct IntFieldInstance
			{
				public int Value;
			}

			public struct IntPropertyInstance
			{
				private int _value;
				public  int  Value
				{
					get { return _value * 2; }
					set { _value = value; }
				}
			}

			public struct ObjFieldInstance
			{
				public object Value;
			}

			public struct ObjPropertyInstance
			{
				private object _value;
				public  object  Value
				{
					get { return _value is int? (int)_value * 3: _value; }
					set { _value = value; }
				}
			}

			public abstract class TestObject1
			{
				[InstanceType(typeof(IntFieldInstance))]    public abstract int       IntField  { get; set; }
				[InstanceType(typeof(IntPropertyInstance))] public abstract int       IntProp   { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract int       ObjField  { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract int       ObjProp   { get; set; }
				[InstanceType(typeof(IntFieldInstance))]    public abstract DayOfWeek DowField  { get; set; }
				[InstanceType(typeof(IntPropertyInstance))] public abstract DayOfWeek DowProp   { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract DateTime  DateField { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract DateTime  DateProp  { get; set; }
				[InstanceType(typeof(ObjFieldInstance))]    public abstract ArrayList ArrField  { get; set; }
				[InstanceType(typeof(ObjPropertyInstance))] public abstract ArrayList ArrProp   { get; set; }
			}

			public static void Test()
			{
				TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

				o.IntField = 10;
				o.IntProp  = 11;
				o.ObjField = 12;
				o.ObjProp  = 13;

				Assert.AreEqual(10, o.IntField);
				Assert.AreEqual(22, o.IntProp);
				Assert.AreEqual(12, o.ObjField);
				Assert.AreEqual(39, o.ObjProp);

				DateTime  testDate  = new DateTime(2000, 1, 1);

				o.DowField  = DayOfWeek.Monday;
				o.DowProp   = DayOfWeek.Sunday;
				o.DateField = testDate;
				o.DateProp  = testDate;

				Assert.AreEqual(DayOfWeek.Monday, o.DowField);
				Assert.AreEqual(DayOfWeek.Sunday, o.DowProp);
				Assert.AreEqual(testDate,         o.DateField);
				Assert.AreEqual(testDate,         o.DateProp);

				o.ArrField = new ArrayList(17);
				o.ArrProp  = new ArrayList(21);

				Assert.AreEqual(17, o.ArrField.Capacity);
				Assert.AreEqual(21, o.ArrProp. Capacity);
			}
		}

		[Test]
		public void InstanceStructTest()
		{
			TestStruct.Test();
		}

		public struct IntParamInstance
		{
			public IntParamInstance(int value)
			{
				Value = value;
			}

			public int Value;
		}

		public abstract class TestObject1
		{
			[InstanceType(typeof(IntParamInstance), 58)] public abstract int IntField { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			Assert.AreEqual(58, o.IntField);
		}

		public class Instance2
		{
			public Instance2(int n)
			{
				_n = n;
			}

			private int _n;

			private int _value;
			public  int  Value
			{
				get { return _value * _n; }
				set { _value = value; }
			}
		}

		[GlobalInstanceType(typeof(int), typeof(Instance2), 3)]
		public abstract class Object2
		{
			[InstanceType(typeof(Instance2), 5)] 
			public abstract int   Int1   { get; set; }
			public abstract int   Int2   { get; set; }
			public abstract short Short1 { get; set; }
		}

		[Test]
		public void GlobalParamTest()
		{
			Object2 o = (Object2)TypeAccessor.CreateInstance(typeof(Object2));

			o.Int1   = 5;
			o.Int2   = 5;
			o.Short1 = 10;

			Assert.AreEqual(25, o.Int1);
			Assert.AreEqual(15, o.Int2);
			Assert.AreEqual(10, o.Short1);
		}
	}
}
