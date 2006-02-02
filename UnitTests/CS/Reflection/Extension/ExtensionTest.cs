using System;
using System.IO;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace A.Reflection.Extension
{
	[TestFixture]
	public class ExtensionTestTest
	{
		[SetUp]
		public void SetUp()
		{
			using (StreamWriter sw = File.CreateText("Mapping.xml"))
			{
				sw.WriteLine(@"<?xml version='1.0' encoding='utf-8' ?>
<Types xmlns='urn:schemas-bltoolkit-net:typeinfo'>
    <Type Name='Dest'>
        <Member Name='Field3'>
            <MapValue Value='-1-' OrigValue='1' OrigValue-Type='System.Double' />
            <MapValue Value='-2-' OrigValue='2' OrigValue-Type='System.Double' />
        </Member>
    </Type>
    <Type Name='TriState'>
        <Member Name='Yes'           MapValue='yes'   MapValue-Type='System.String' />
        <Member Name='No'>
            <MapValue Value='no' Type='System.String' />
            <MapValue Type='System.String'>N</MapValue>
        </Member>
        <Member Name='Maybe'         MapValue='xz'    MapValue-Type='System.String' />
        <Member Name='NotApplicable' MapValue-Type='System.String' MapValue='(n/a)' DefaultValue='' />
    </Type>
    <Type Name='System.Double'>
        <MapValue OrigValue='1' OrigValue-Type='System.Double' Value='One' Type='System.String' />
        <MapValue OrigValue='2' Value='Two' Type='System.String' />
        <DefaultValue Value='54' />
    </Type>
</Types>");
			}
		}

		[TypeExtension("TriState")]
		public enum TriState { Yes, No, NotApplicable };

		public class Source
		{
			public string Field1 = "no";
			public string Field2 = "One";
			public string Field3 = "-2-";
			public string Field4 = "***";
		}

		[TypeExtension("Dest")]
		public class Dest
		{
			private TriState _field1 = TriState.NotApplicable;
			public  TriState  Field1
			{
				get { return _field1;  }
				set { _field1 = value; }
			}

			public double Field2;
			public double Field3;
			public double Field4;
		}

		[Test]
		public void Test()
		{
			Map.Extensions = TypeExtension.GetExtenstions("Mapping.xml");

			object o = Map.Extensions["TriState"]["Yes"]["MapValue"].Value;

			Assert.AreEqual("yes", o);

			Source  s = new Source();
			Dest    d = (Dest)Map.ObjectToObject(s, typeof(Dest));

			Assert.AreEqual(TriState.No,            d.Field1);
			Assert.AreEqual( 1,                     d.Field2);
			Assert.AreEqual( 2,                     d.Field3);
			Assert.AreEqual(54,                     d.Field4);
			Assert.AreEqual(TriState.NotApplicable, Map.ValueToEnum("1234", typeof(TriState)));
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete("Mapping.xml");
		}
	}
}
