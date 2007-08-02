using System;
using BLToolkit.EditableObjects;
using BLToolkit.Reflection;
using NUnit.Framework;

using BLToolkit.Validation;
using BLToolkit.Mapping;

namespace UnitTests.CS.Validation
{
	[TestFixture]
	public class NullValue
	{
		public class Entity : EditableObject
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

		public abstract class PersonDoc : EditableObject
		{
			[MapField("Series_PersonDoc"), MaxLength(50), Required]
			public abstract string Series_PersonDoc { get; set; }

			[MapField("BegDate_PersonDoc"), Required]
			[NullDateTime()]
			public abstract DateTime BegDate_PersonDoc { get; set; }
		}


		[Test, ExpectedException(typeof(ValidationException))]
		public void Test2()
		{
#if FW2
			PersonDoc doc = TypeAccessor<PersonDoc>.CreateInstance();
#else
			PersonDoc doc = (PersonDoc)TypeAccessor.CreateInstance(typeof(PersonDoc));
#endif
			doc.Series_PersonDoc = "aaa";
			doc.Validate();
		}
	}
}
