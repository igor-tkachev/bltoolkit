using System.IO;
using System.Xml.Serialization;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;
using BLToolkit.TypeBuilder;

namespace HowTo.TypeBuilder
{
	[TestFixture]
	public class XmlSerializationTest
	{
		/*[a]*/[XmlType(AnonymousType = true)]/*[/a]*/
		public abstract class MyClassA
		{
			public abstract string ValueA { get; set; }
		}

		[XmlType(AnonymousType = true)]
		/*[a]*/[XmlIncludeAbstract(typeof(MyClassA))]/*[/a]*/
		/*[a]*/[XmlIncludeAbstract(typeof(MyClassC))]/*[/a]*/
		public abstract class MyClassB
		{
			public abstract string   ValueB        { get; set; }
			public abstract MyClassA ValueMyClassA { get; set; }
	
			public abstract EditableList<MyClassA> MyList { get; set; }
		}

		/*[a]*/[XmlType("abs:MyClassC")]/*[/a]*/
		public abstract class MyClassC : MyClassA { }

		[Test]
		public void Test()
		{
			MyClassB      original = TypeAccessor<MyClassB>.CreateInstance();
			MyClassB      serialized;
			XmlSerializer sr       = new XmlSerializer(/*[a]*/TypeAccessor<MyClassB>.Type/*[/a]*/);

			original.ValueB               = "string value B";
			original.ValueMyClassA.ValueA = "string value A";
			original.MyList.Add(TypeAccessor<MyClassA>.CreateInstance());
			original.MyList.Add(TypeAccessor<MyClassC>.CreateInstance());
			
			using (MemoryStream stm = new MemoryStream())
			{
				sr.Serialize(stm, original);
				stm.Position = 0L;
				serialized = (MyClassB)sr.Deserialize(stm);
			}

			Assert.That(serialized.ValueB,               Is.EqualTo(original.ValueB));
			Assert.That(serialized.ValueMyClassA.ValueA, Is.EqualTo(original.ValueMyClassA.ValueA));
			
			Assert.AreEqual(original.MyList.Count, serialized.MyList.Count);
			Assert.That(serialized.MyList[0] is MyClassA);
			Assert.That(serialized.MyList[1] is MyClassA);
			Assert.That(serialized.MyList[1] is MyClassC);
			
			Assert.AreEqual(serialized.MyList[0].GetType(), TypeFactory.GetType(typeof(MyClassA)));
			Assert.AreEqual(serialized.MyList[1].GetType(), TypeFactory.GetType(typeof(MyClassC)));
		}
	}
}