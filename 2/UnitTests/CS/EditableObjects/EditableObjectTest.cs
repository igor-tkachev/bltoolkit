using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace EditableObjects
{
	[TestFixture]
	public class EditableObjectTest
	{
		public EditableObjectTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public class Source
		{
			public int    ID   = 10;
			public string Name = "20";
		}

		public abstract class Dest: EditableObject
		{
			public string ChangedPropertyName;

			public abstract int    ID   { get; set; }
			public abstract string Name { get; set; }

			protected override void OnPropertyChanged(string propertyName)
			{
				ChangedPropertyName = propertyName;
			}
		}

		[Test]
		public void Notification()
		{
			Dest o = (Dest)Map.ObjectToObject(new Source(), typeof(Dest));

			Assert.AreEqual("", o.ChangedPropertyName);

			o.ID = 1;

			Assert.AreEqual("ID", o.ChangedPropertyName);
		}

		public abstract class Object1 : EditableObject
		{
			[MapField("ObjectId")]
			public abstract int       ID      { get;  }
			public abstract short     Field1  { get; set; }

			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public abstract bool      Field2  { get; set; }

			public abstract DateTime  Field3  { get; set; }
			public abstract long      Field4  { get; set; }
			public abstract byte      Field5  { get; set; }
			public abstract char      Field6  { get; set; }
			public abstract ushort    Field7  { get; set; }
			public abstract uint      Field8  { get; set; }
			public abstract ulong     Field9  { get; set; }
			public abstract sbyte     Field10 { get; set; }
			public abstract float     Field11 { get; set; }
			public abstract double    Field12 { get; set; }
			public abstract decimal   Field13 { get; set; }
			public abstract string    Field14 { get; set; }
			public abstract Guid      Field15 { get; set; }
			public abstract DayOfWeek Field16 { get; set; }
#if FW2
			public abstract ulong?    Field17 { get; set; }
#endif

			public static Object1 CreateInstance()
			{
				return (Object1)Map.CreateInstance(typeof(Object1));
			}
		}

		[Test]
		public void TestCreate()
		{
			Object1 o = Object1.CreateInstance();

			Assert.IsFalse(o.IsDirty);

			TypeAccessor.GetAccessor(typeof(Object1))["ID"].SetValue(o, 1);

			Assert.AreEqual(1, o.ID);
			Assert.IsTrue  (o.IsDirty);
			Assert.IsTrue  (o.IsDirtyMember("ID"));
			o.AcceptChanges();
			Assert.IsFalse (o.IsDirty);

			o.Field16 = DayOfWeek.Saturday;

			Assert.AreEqual(DayOfWeek.Saturday, o.Field16);
			Assert.IsTrue  (o.IsDirty);
			Assert.IsTrue  (o.IsDirtyMember("Field16"));
			o.AcceptChanges();
			Assert.IsFalse (o.IsDirty);

#if FW2
			o.Field17 = 5;

			Assert.AreEqual(5, o.Field17);
			Assert.IsTrue  (o.IsDirty);
			Assert.IsTrue  (o.IsDirtyMember("Field17"));
			o.AcceptChanges();
			Assert.IsFalse (o.IsDirty);
#endif
		}
	}
}
