using System;
using NUnit.Framework;
using BLToolkit.EditableObjects;

namespace HowTo.EditableObjects
{
	[TestFixture]
	public class PropertyChanged
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

			bool proprtyName = null;

			obj./*[a]*/PropertyChanged/*[/a]*/ += (s, e) => proprtyName = e.PropertyName;

			Assert.IsNull(propertyName);

			obj.FirstName = "Tester";

			Assert.AreEqual(proprtyName, "FirstName");

			bool proprtyName = null;

			obj.AcceptChanges();
		}
	}
}
