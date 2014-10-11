using System;
using NUnit.Framework;
using BLToolkit.EditableObjects;

namespace HowTo.EditableObjects
{
	[TestFixture]
	public class AcceptRejectChanges
	{
		public /*[a]*/abstract/*[/a]*/ class TestObject : /*[a]*/EditableObject/*[/a]*/<TestObject>
		{
			public /*[a]*/abstract/*[/a]*/ string FirstName { get; set; }
			public /*[a]*/abstract/*[/a]*/ string LastName  { get; set; }
		}

		[Test]
		public void Test()
		{
			// Create an instance.
			//
			TestObject obj = TestObject./*[a]*/CreateInstance/*[/a]*/();

			// Accept changes.
			//
			obj.FirstName = "Tester";
			obj.LastName  = "Testerson";

			Assert.IsTrue(obj.IsDirty);

			obj./*[a]*/AcceptChanges/*[/a]*/();

			Assert.AreEqual("Tester", obj.FirstName);
			Assert.IsFalse(obj.IsDirty);

			// Reject changes.
			//
			obj.FirstName = "Developer";

			Assert.IsTrue(obj.IsDirty);

			obj./*[a]*/RejectChanges/*[/a]*/();

			Assert.AreEqual("Tester", obj.FirstName);
			Assert.IsFalse(obj.IsDirty);
		}
	}
}
