using System.IO;
using System.Xml.Serialization;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

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
		public abstract class MyClassB
		{
			public abstract string   ValueB        { get; set; }
			public abstract MyClassA ValueMyClassA { get; set; }
	
			public abstract EditableList<MyClassA> MyList { get; set; }
		}

		[Test]
		public void Test()
		{
			MyClassB      original = TypeAccessor<MyClassB>.CreateInstance();
			MyClassB      serialized;
			XmlSerializer sr      = new XmlSerializer(/*[a]*/TypeAccessor<MyClassB>.Type/*[/a]*/);

			original.ValueB               = "string value B";
			original.ValueMyClassA.ValueA = "string value A";

			using (MemoryStream stm = new MemoryStream())
			{
				sr.Serialize(stm, original);
				stm.Position = 0L;
				serialized = (MyClassB)sr.Deserialize(stm);
			}

			Assert.That(serialized.ValueB,               Is.EqualTo(original.ValueB));
			Assert.That(serialized.ValueMyClassA.ValueA, Is.EqualTo(original.ValueMyClassA.ValueA));
		}
	}
}