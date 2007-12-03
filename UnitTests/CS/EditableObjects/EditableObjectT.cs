using System;
using BLToolkit.Common;
using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.TypeBuilder;

namespace EditableObjects
{
	[TestFixture]
	public class EditableObjectT
	{
		public EditableObjectT()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject: EditableObject<TestObject>
		{
			public abstract int         ID    { get; set; }
			public abstract string      Name  { get; set; }
			public abstract InnerObject Inner { get; set; }

			public abstract class InnerObject: EditableObject<InnerObject>
			{
				public abstract int     Some  { get; set; }
			}
		}

		[Test]
		public void CloneTest()
		{
			TestObject o = TestObject.CreateInstance();

			o.ID   = 1;
			o.Name = "str";
			o.Inner.Some = 2;

			TestObject clone = o.Clone();

			// Make sure this one is cloned, not copied.
			//
			o.Inner.Some = 3;

			Assert.AreEqual(o.ID, clone.ID);
			Assert.AreEqual(o.Name, clone.Name);

			Assert.AreNotEqual(o.Inner.Some, clone.Inner.Some);
			Assert.IsFalse(o.Equals(clone));

			// Now make it the same as original value.
			//
			clone.Inner = o.Inner.Clone();

			Assert.AreEqual(o.Inner.Some, clone.Inner.Some);
			Configuration.EditableObjectUsesMemberwiseEquals = true;
			Assert.IsTrue(o.Equals(clone));
			Configuration.EditableObjectUsesMemberwiseEquals = false;
		}

		[Test]
		public void IsDirtyTest()
		{
			TestObject o = TestObject.CreateInstance();

			o.ID   = 1;
			o.Name = "str";
			o.Inner.Some = 2;
			o.AcceptChanges();

			TestObject clone = o.Clone();

			Assert.IsFalse(o.IsDirty);
			Assert.IsFalse(clone.IsDirty);
		}
	}
}
