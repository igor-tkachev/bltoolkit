using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;
using BLToolkit.TypeBuilder.Builders;

namespace TypeBuilder
{
	[TestFixture]
	public class GetSetValueAttributeTest
	{
		public GetSetValueAttributeTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public class Value
		{
			public Value(int value)
			{
				IntValue = value;
			}

			[SetValue, GetValue] public int IntValue;

			public Value(float value)
			{
				FloatValue = value;
			}

			[SetValue, GetValue] public float FloatValue;

			public Value(string value)
			{
				StrValue = value;
			}

			object _value;

			[SetValue, GetValue]
			public string StrValue
			{
				get { return (string)_value; }
				set { _value = value; }
			}

			public Value(DayOfWeek value)
			{
				DayValue = value;
			}

			[SetValue, GetValue]
			public DayOfWeek DayValue
			{
				get { return (DayOfWeek)_value; }
				set { _value = value; }
			}
		}

		public abstract class TestObject1
		{
			[InstanceType(typeof(Value), 55)]        public abstract int    IntValue   { get; set; }
			[InstanceType(typeof(Value), (float)16)] public abstract float  FloatValue { get; set; }
			[InstanceType(typeof(Value), "test1")]   public abstract string StrValue   { get; set; }

			[InstanceType(typeof(Value), DayOfWeek.Saturday)]
			public abstract DayOfWeek DayValue { get; set; }
		}

		[Test]
		public void Test()
		{
			TestObject1 o = (TestObject1)TypeAccessor.CreateInstance(typeof(TestObject1));

			Assert.AreEqual(55, o.IntValue);
			o.IntValue += 1;
			Assert.AreEqual(56, o.IntValue);

			Assert.AreEqual(16, o.FloatValue);
			o.FloatValue += 1;
			Assert.AreEqual(17, o.FloatValue);

			Assert.AreEqual("test1", o.StrValue);
			o.StrValue = "test2";
			Assert.AreEqual("test2", o.StrValue);

			Assert.AreEqual(DayOfWeek.Sunday, o.DayValue);
			o.DayValue = DayOfWeek.Thursday;
			Assert.AreEqual(DayOfWeek.Thursday, o.DayValue);
		}
	}
}
