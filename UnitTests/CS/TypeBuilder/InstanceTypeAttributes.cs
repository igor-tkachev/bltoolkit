using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder
{
	[TestFixture]
	public class InstanceTypeAttributes
	{
		public InstanceTypeAttributes()
		{
			TypeFactory.SaveTypes = true;
		}

		public class Class
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

			public abstract class Object1
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
				BuildContext context = TypeFactory.GetType(typeof(Object1));

				Console.WriteLine(context.Type.Type);

				Object1 o = (Object1)Activator.CreateInstance(context.Type);

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
			Class.Test();
		}

		public class Struct
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

			public abstract class Object1
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
				BuildContext context = TypeFactory.GetType(typeof(Object1));

				Console.WriteLine(context.Type.Type);

				Object1 o = (Object1)Activator.CreateInstance(context.Type);

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
			Struct.Test();
		}

		public struct IntParamInstance
		{
			public IntParamInstance(int value)
			{
				Value = value;
			}

			public int Value;
		}

		public abstract class Object1
		{
			[InstanceType(typeof(IntParamInstance), 58)] public abstract int IntField { get; set; }
		}

		[Test]
		public void ParamTest()
		{
			BuildContext context = TypeFactory.GetType(typeof(Object1));

			Console.WriteLine(context.Type.Type);

			Object1 o = (Object1)Activator.CreateInstance(context.Type);

			Assert.AreEqual(58, o.IntField);
		}
	}
}
