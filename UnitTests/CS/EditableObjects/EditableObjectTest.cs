using System;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace EditableObjects
{
	[TestFixture]
	public class EditableObjectTest
	{
		public class Source
		{
			public int         ID   = 10;
			public string      Name = "20";
			public XmlDocument Xml;
		
			public Source()
			{
				Xml = new XmlDocument();
				Xml.LoadXml("<test/>");
			}
		}

		public abstract class Dest: EditableObject
		{
			public string ChangedPropertyName;

			public abstract int         ID   { get; set; }
			public abstract string      Name { get; set; }
			public abstract XmlDocument Xml  { get; set; }

			protected override void OnPropertyChanged(string propertyName)
			{
				ChangedPropertyName = propertyName;
				if (propertyName == "ID")
					Assert.That(ID, Is.Not.EqualTo(0));
				else if (propertyName == "Xml")
					Assert.That(Xml.InnerXml, Is.Not.EqualTo("<test />"));

			}
		}

		[Test]
		public void Notification()
		{
			Dest o = Map.ObjectToObject<Dest>(new Source());

			Assert.AreEqual("", o.ChangedPropertyName);

			o.ID = 1;
			Assert.AreEqual("ID", o.ChangedPropertyName);

			o.Xml.DocumentElement.AppendChild(o.Xml.CreateElement("el"));
			Assert.AreEqual("Xml", o.ChangedPropertyName);
		}

		public abstract class Object1: EditableObject<Object1>
		{
			[MapField("ObjectId")]
			public abstract int         ID       { get; }
			public abstract short       Field1   { get; set; }

			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public abstract bool        Field2   { get; set; }
			[Parameter(2, 2, 2)]
			public abstract DateTime?   Field3   { get; set; }
			[Parameter(2L)]
			public abstract long        Field4   { get; set; }
			public abstract byte        Field5   { get; set; }
			public abstract char        Field6   { get; set; }
			public abstract ushort      Field7   { get; set; }
			public abstract uint        Field8   { get; set; }
			public abstract ulong       Field9   { get; set; }
			public abstract sbyte       Field10  { get; set; }
			public abstract float       Field11  { get; set; }
			public abstract double      Field12  { get; set; }
			[Parameter(3.08)]
			public abstract decimal?    Field13  { get; set; }
			public abstract string      Field14  { get; set; }
			public abstract Guid        Field15  { get; set; }
			public abstract DayOfWeek   Field16  { get; set; }
			public abstract ulong?      Field17  { get; set; }
			public abstract XmlDocument XmlField { get; set; }
		}

		[Test]
		public void TestCreate()
		{
			Object1 o = Object1.CreateInstance();

			Assert.That(o.Field4,  Is.EqualTo(2L));
			Assert.That(o.Field3,  Is.EqualTo(new DateTime(2,2,2)));
			Assert.That(o.Field13, Is.EqualTo(3.08m));

			Assert.IsFalse(o.IsDirty);

			TypeAccessor<Object1>.Instance["ID"].SetValue(o, 1);

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

			o.Field17 = 5;

			Assert.AreEqual(5, o.Field17);
			Assert.IsTrue  (o.IsDirty);
			Assert.IsTrue  (o.IsDirtyMember("Field17"));
			o.AcceptChanges();
			Assert.IsFalse (o.IsDirty);

			o.XmlField.LoadXml(@"<root><element attribute=""value""/></root>");
			Assert.IsTrue  (o.IsDirty);
			o.AcceptChanges();

			o.XmlField.SelectSingleNode("/root/element/@attribute").Value = "changed";
			Assert.IsTrue  (o.IsDirty);
			Assert.IsTrue  (o.IsDirtyMember("XmlField"));
			o.AcceptChanges();
			Assert.IsFalse (o.IsDirty);

			o.XmlField.SelectSingleNode("/root/element/@attribute").Value = "once again";
			o.XmlField = new XmlDocument();
			Assert.IsTrue  (o.IsDirty);
		}


		[Test]
		public void TestRejectChangesNotification()
		{
			Object1 o = Object1.CreateInstance();

			Console.WriteLine("o is dirty: " + o.IsDirty);

			o.PropertyChanged += object_PropertyChanged;

			Console.WriteLine("Changing 3 fields");

			o.Field1 = 10;
			o.Field2 = !o.Field2;
			o.Field3 = DateTime.Now;
			o.XmlField.LoadXml("<root foo=\"bar\"/>");
			o.XmlField.DocumentElement.Attributes.RemoveAll();
			o.XmlField.DocumentElement.Attributes.Append(o.XmlField.CreateAttribute("attr"));

			Console.WriteLine("Dirty Members");

			PropertyInfo[] dirtyMembers = o.GetDirtyMembers();

			Assert.AreEqual(4, dirtyMembers.Length);

			foreach (PropertyInfo dirtyMember in dirtyMembers)
				Console.WriteLine(dirtyMember.Name);

			Console.WriteLine("Rejecting field 1");

			o.RejectMemberChanges("Field1");
			o.RejectMemberChanges("XmlField");

			Console.WriteLine("Dirty Members");

			dirtyMembers = o.GetDirtyMembers();
			Assert.AreEqual(2, dirtyMembers.Length);

			foreach (PropertyInfo dirtyMember in dirtyMembers)
				Console.WriteLine(dirtyMember.Name);

			Console.WriteLine("Rejecting all changes");

			o.RejectChanges();

			Console.WriteLine("Dirty Members");

			dirtyMembers = o.GetDirtyMembers();
			Assert.AreEqual(dirtyMembers.Length, 0);

			foreach (PropertyInfo dirtyMember in dirtyMembers)
				Console.WriteLine(dirtyMember.Name);
		}

		private static void object_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			Console.WriteLine("Property Changed: " + e.PropertyName);
		}

		[Test]
		public void IsDirtyTest()
		{
			Object1 o = Object1.CreateInstance();

			o.Field1 = 10;
			o.Field2 = !o.Field2;
			o.Field3 = DateTime.Now;
			o.AcceptChanges();

			Object1 c = (Object1)((ICloneable)o).Clone();

			Assert.IsFalse(o.IsDirty);
			Assert.IsFalse(c.IsDirty);

			o.Field1 = 100;
			c = (Object1)((ICloneable)o).Clone();

			Assert.IsTrue(o.IsDirty);
			Assert.IsTrue(c.IsDirty);

			Assert.IsTrue(o.IsDirtyMember("Field1"));
			Assert.IsTrue(c.IsDirtyMember("Field1"));

			Assert.IsFalse(o.IsDirtyMember("Field2"));
			Assert.IsFalse(c.IsDirtyMember("Field2"));
		}

		public class TestClass
		{
			public int    ID;
			public string Str;
		}

		public struct TestStruct
		{
			public int    ID;
			public string Str;
		}

		public abstract class TestEditableObject : EditableObject
		{
			public abstract int ID { get; set; }

			public static TestEditableObject CreateInstance()
			{
				return (TestEditableObject)
					TypeAccessor.CreateInstanceEx(typeof(TestEditableObject), null);
			}
		}

		[Test]
		public void EqualsTest()
		{
			TestClass classInst1 = new TestClass();
			TestClass classInst2 = new TestClass();

			TestStruct structInst1 = new TestStruct();
			TestStruct structInst2 = new TestStruct();

			TestEditableObject editableInst1 = TestEditableObject.CreateInstance();
			TestEditableObject editableInst2 = TestEditableObject.CreateInstance();
			TestEditableObject editableInst3 = editableInst1;

			classInst1   .ID = classInst2   .ID = 1;
			structInst1  .ID = structInst2  .ID = 1;
			editableInst1.ID = editableInst2.ID = 1;

			TestStruct structInst3 = structInst1;
			TestClass   classInst3 = classInst1;

			Assert.IsTrue(Equals(structInst1,   structInst2));
			Assert.IsTrue(Equals(structInst1,   structInst3));
			Assert.IsTrue(Equals(structInst2,   structInst3));

			Assert.IsFalse(Equals(classInst1,    classInst2));
			Assert.IsFalse(Equals(editableInst1, editableInst2));

			Assert.IsTrue (Equals(classInst1,    classInst3));
			Assert.IsTrue (Equals(editableInst1, editableInst3));

			Assert.IsFalse(Equals(classInst2,    classInst3));
			Assert.IsFalse(Equals(editableInst2, editableInst3));
		}

		[Test]
		public void EqualsSpeedTest()
		{
			TestClass eo1Inst1 = new TestClass();
			TestClass eo1Inst3 = eo1Inst1;

			TestEditableObject eo2Inst1 = TestEditableObject.CreateInstance();
			TestEditableObject eo2Inst3 = eo2Inst1;

			eo1Inst1.ID = 1; eo1Inst1.Equals(eo1Inst3);
			eo2Inst1.ID = 1; eo2Inst1.Equals(eo2Inst3);

			long startTicks = DateTime.Now.Ticks;
			for (int i = 0; i < 100000; i++)
				eo1Inst1.Equals(eo1Inst3);
			Console.WriteLine(".NET: {0}", DateTime.Now.Ticks - startTicks);

			startTicks = DateTime.Now.Ticks;
			for (int i = 0; i < 100000; i++)
				eo2Inst1.Equals(eo2Inst3);
			Console.WriteLine("BLT: {0}", DateTime.Now.Ticks - startTicks);
		}

		[Test]
		public void GetHashCodeSpeedTest()
		{
			TestClass eo1Inst1 = new TestClass();
			TestEditableObject eo2Inst1 = TestEditableObject.CreateInstance();

			eo1Inst1.ID = 1; eo1Inst1.GetHashCode();
			eo2Inst1.ID = 1; eo2Inst1.GetHashCode();

			long startTicks = DateTime.Now.Ticks;
			for (int i = 0; i < 100000; i++)
				eo1Inst1.GetHashCode();
			Console.WriteLine(".NET: {0}", DateTime.Now.Ticks - startTicks);

			startTicks = DateTime.Now.Ticks;
			for (int i = 0; i < 100000; i++)
				eo2Inst1.GetHashCode();
			Console.WriteLine("BLT: {0}", DateTime.Now.Ticks - startTicks);
		}

	}
}
