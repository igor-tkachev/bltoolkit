using System;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace A.EditableObjects
{
	[TestFixture]
	public class NestedObjectTest
	{
		public abstract class NestedObject : EditableObject
		{
			public NestedObject()
			{

			}
			//public abstract int ID { get; set; }
		}

		public abstract class MasterObject : EditableObject
		{
			//public abstract int          ID           { get; set; }
			public abstract NestedObject NestedObject { get; set; }
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;

			MasterObject obj = (MasterObject)TypeAccessor.CreateInstance(typeof(MasterObject));

			//Assert.IsNotNull(obj.NestedObject);
		}
	}
}
