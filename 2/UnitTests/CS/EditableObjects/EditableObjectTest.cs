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
	public class EditableObjectTest
	{
		public EditableObjectTest()
		{
			TypeFactory.SaveTypes = true;
		}

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
			}
		}

		[Test]
		public void Notification()
		{
			Dest o = (Dest)Map.ObjectToObject(new Source(), typeof(Dest));

			Assert.AreEqual("", o.ChangedPropertyName);

			o.ID = 1;
			Assert.AreEqual("ID", o.ChangedPropertyName);

			o.Xml.DocumentElement.AppendChild(o.Xml.CreateElement("el"));
			Assert.AreEqual("Xml", o.ChangedPropertyName);
		}

		public abstract class Object1 : EditableObject
		{
			[MapField("ObjectId")]
			public abstract int         ID      { get;  }
			public abstract short       Field1  { get; set; }

			[MapValue(true,  "Y")]
			[MapValue(false, "N")]
			public abstract bool        Field2  { get; set; }

			public abstract DateTime    Field3  { get; set; }
			public abstract long        Field4  { get; set; }
			public abstract byte        Field5  { get; set; }
			public abstract char        Field6  { get; set; }
			public abstract ushort      Field7  { get; set; }
			public abstract uint        Field8  { get; set; }
			public abstract ulong       Field9  { get; set; }
			public abstract sbyte       Field10 { get; set; }
			public abstract float       Field11 { get; set; }
			public abstract double      Field12 { get; set; }
			public abstract decimal     Field13 { get; set; }
			public abstract string      Field14 { get; set; }
			public abstract Guid        Field15 { get; set; }
			public abstract DayOfWeek   Field16 { get; set; }
#if FW2
			public abstract ulong?      Field17 { get; set; }
#endif
			public abstract XmlDocument XmlField { get; set; }

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

			o.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(object_PropertyChanged);

			Console.WriteLine("Changing 3 fields");

			o.Field1 = 10;
			o.Field2 = !o.Field2;
			o.Field3 = DateTime.Now;
			o.XmlField.LoadXml("<root/>");

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

		public struct EO1
		{
			public int    ID;
			public string Str;
		}

		public abstract class EO2 : EditableObject<EO2>
		{
			public abstract int ID { get; set; }

			//Uncomment this to make test below work.
			//public override bool Equals(object obj)
			//{
			//    if (!ReferenceEquals(this, obj))
			//        return false;
			//    return base.Equals(obj);
			//}
		}

		[Test]
		public void EqualsTest()
		{
			EO1 eo1Inst1 = new EO1();
			EO1 eo1Inst2 = new EO1();

			EO2 eo2Inst1 = EO2.CreateInstance();
			EO2 eo2Inst2 = EO2.CreateInstance();
			EO2 eo2Inst3 = eo2Inst1;

			eo1Inst1.ID = eo1Inst2.ID = 1;
			eo2Inst1.ID = eo2Inst2.ID = 1;
			EO1 eo1Inst3 = eo1Inst1;

			Console.WriteLine("eo1Inst1 equals eo1Inst2: " + Equals(eo1Inst1, eo1Inst2));
			Console.WriteLine("eo2Inst1 equals eo2Inst2: " + Equals(eo2Inst1, eo2Inst2));
			Assert.IsTrue(Equals(eo1Inst1, eo1Inst2) == Equals(eo2Inst1, eo2Inst2));

			Console.WriteLine("eo1Inst1 equals eo1Inst3: " + Equals(eo1Inst1, eo1Inst3));
			Console.WriteLine("eo2Inst1 equals eo2Inst3: " + Equals(eo2Inst1, eo2Inst3));
			Assert.IsTrue(Equals(eo1Inst1, eo1Inst3) == Equals(eo2Inst1, eo2Inst3));

			Console.WriteLine("eo1Inst2 equals eo1Inst3: " + Equals(eo1Inst2, eo1Inst3));
			Console.WriteLine("eo2Inst2 equals eo2Inst3: " + Equals(eo2Inst2, eo2Inst3));
		}

		[Test]
		public void EqualsSpeedTest()
		{
			EO1 eo1Inst1 = new EO1();
			EO1 eo1Inst3 = eo1Inst1;

			EO2 eo2Inst1 = EO2.CreateInstance();
			EO2 eo2Inst3 = eo2Inst1;

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
			EO1 eo1Inst1 = new EO1();
			EO2 eo2Inst1 = EO2.CreateInstance();

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
