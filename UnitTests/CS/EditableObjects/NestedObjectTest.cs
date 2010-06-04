using System;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace EditableObjects
{
	[TestFixture]
	public class NestedObjectTest
	{
		public abstract class AbstractNestedObject : EditableObject
		{
			public AbstractNestedObject(InitContext ic)
			{
				if (ic.MemberParameters != null && ic.MemberParameters.Length > 0)
					ID = Convert.ToInt32(ic.MemberParameters[0]);
			}

			public abstract int ID { get; set; }
		}

		public class NestedObject : EditableObject
		{
			public NestedObject()
			{
			}

			public NestedObject(int n)
			{
				ID = n;
			}

			public int ID;
		}

		public abstract class MasterObject : EditableObject
		{
			public abstract int                  ID      { get; set; }
			public abstract AbstractNestedObject Object1 { get; set; }
			public abstract NestedObject         Object2 { get; set; }

			[Parameter(100)]
			public abstract AbstractNestedObject Object3 { get; set; }
			[Parameter(200)]
			public abstract NestedObject         Object4 { get; set; }

			public string ChangedProperty;

			protected override void OnPropertyChanged(string propertyName)
			{
				base.OnPropertyChanged(propertyName);

				ChangedProperty = propertyName;
			}
		}

		[Test]
		public void Test()
		{
			MasterObject obj = (MasterObject)TypeAccessor.CreateInstance(typeof(MasterObject));

			Assert.IsNotNull(obj.Object1);
			Assert.IsNotNull(obj.Object2);

			Assert.AreEqual(100, obj.Object3.ID);
			Assert.AreEqual(200, obj.Object4.ID);

			obj.Object1.ID = 50;

			Assert.AreEqual("Object1.ID", obj.ChangedProperty);
		}
	}
}
