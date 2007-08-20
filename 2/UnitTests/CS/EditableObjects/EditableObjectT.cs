using System;
using System.Reflection;
using System.Xml;
using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace EditableObjects
{
	[TestFixture]
	public class EditableObjectT
	{
		public EditableObjectT()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObjectBase: EditableObject<TestObjectBase>
		{
			public abstract int         ID    { get; set; }
			public abstract string      Name  { get; set; }
			public abstract InnerObject Inner { get; set; }

			public abstract class InnerObject: EditableObject<InnerObject>
			{
				public abstract int     Some  { get; set; }
			}
		}

		public abstract class Derived1: TestObjectBase
		{
			public abstract int         Some  { get; set; }
		}

		public abstract class Derived2: TestObjectBase
		{
			public abstract string      Some  { get; set; }
		}

		[Test]
		public void CloneTest()
		{
			TestObjectBase o = Derived1.CreateInstance();

			o.ID   = 1;
			o.Name = "str";
			o.Inner.Some = 2;

			TestObjectBase clone = o.Clone();

			// Make sure this one is cloned, not copied.
			//
			o.Inner.Some = 3;

			Assert.AreEqual(o.ID, clone.ID);
			Assert.AreEqual(o.Name, clone.Name);

			Assert.AreNotEqual(o.Inner.Some, clone.Inner.Some);
			Assert.IsFalse(((IEquatable<TestObjectBase>)o).Equals(clone));

			// Now make it the same as original value.
			//
			clone.Inner = o.Inner.Clone();

			Assert.AreEqual(o.Inner.Some, clone.Inner.Some);
			Assert.IsTrue(((IEquatable<TestObjectBase>)o).Equals(clone));
		}
	}
}
