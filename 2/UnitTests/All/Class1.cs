using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.TypeBuilder.Builders;

namespace UnitTests.All
{
	public class TestObject1
	{
		public int       IntField = 10;
		public string    StrField = "10";
		public DayOfWeek DowField;
	}

	public class Accessor
	{
		public virtual void SetValue1(object o, object value)
		{
			Type t = typeof(List<TestObject1>);

			//((TestObject1)o).IntField = (int)value;
		}

		public virtual void SetValue2(object o, object value)
		{
			((TestObject1)o).StrField = (string)value;
		}

		public virtual void SetValue3(object o, object value)
		{
			((TestObject1)o).DowField = (DayOfWeek)value;
		}
	}
}
