using System;

using NUnit.Framework;

using BLToolkit.Validation;
using BLToolkit.Mapping;

namespace Validation
{
	[TestFixture]
	public class NullValue
	{
		public class Entity : BLToolkit.EditableObjects.EditableObject
		{
			private byte timeStart;

			[Required("Time Start is required")]
			[NullValue(typeof(byte), 99)]
			public byte TimeStart
			{
				get { return timeStart;  }
				set { timeStart = value; }
			}
		}

		[Test]
		public void Test()
		{
			Entity test = new Entity();

			test.TimeStart = 0;
			test.Validate();
		}
	}
}
