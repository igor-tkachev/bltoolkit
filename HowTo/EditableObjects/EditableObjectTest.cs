using System;
using NUnit.Framework;
using BLToolkit.EditableObjects;

namespace HowTo.EditableObjects
{
	[TestFixture]
	public class EditableObjectTest
	{
		public /*[a]*/abstract/*[/a]*/ class TestObject : /*[a]*/EditableObject/*[/a]*/<TestObject>
		{
			public /*[a]*/abstract/*[/a]*/ string FirstName { get; set; }
			public /*[a]*/abstract/*[/a]*/ string LastName  { get; set; }

			public string FullName
			{
				get { return string.Format("{0} {1}", FirstName, LastName); }
			}
		}

		[Test]
		public void Test()
		{
			TestObject obj = TestObject./*[a]*/CreateInstance/*[/a]*/();

			obj.FirstName = "Tester";
			obj.LastName  = "Testerson";

			Assert.IsTrue(obj.IsDirty);

			obj.AcceptChanges();

			Assert.IsFalse(obj.IsDirty);
		}
	}
}
