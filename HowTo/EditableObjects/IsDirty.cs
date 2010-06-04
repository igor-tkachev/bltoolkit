using System;
using NUnit.Framework;
using BLToolkit.EditableObjects;

namespace HowTo.EditableObjects
{
	[TestFixture]
	public class IsDirty
	{
		public /*[a]*/abstract/*[/a]*/ class TestObject : /*[a]*/EditableObject/*[/a]*/<TestObject>
		{
			public /*[a]*/abstract/*[/a]*/ string FirstName { get; set; }
			public /*[a]*/abstract/*[/a]*/ string LastName  { get; set; }
		}

		[Test]
		public void Test()
		{
			TestObject obj = TestObject./*[a]*/CreateInstance/*[/a]*/();

			Assert./*[a]*/IsFalse/*[/a]*/(obj./*[a]*/IsDirty/*[/a]*/);

			obj.FirstName = "Tester";
			obj.LastName  = "Testerson";

			Assert./*[a]*/IsTrue/*[/a]*/(obj./*[a]*/IsDirty/*[/a]*/);

			obj.AcceptChanges();

			Assert./*[a]*/IsFalse/*[/a]*/(obj./*[a]*/IsDirty/*[/a]*/);
		}
	}
}
