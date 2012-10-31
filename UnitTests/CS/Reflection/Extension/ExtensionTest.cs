using System.IO;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;
using System.Collections.Generic;
using System.Xml;

namespace Reflection.Extension
{
	[TestFixture]
	public class ExtensionTestTest
	{
		[SetUp]
		public void SetUp()
		{
			Map.DefaultSchema = new DefaultMappingSchema();

			using (StreamWriter sw = File.CreateText("Mapping.xml"))
			{
				sw.WriteLine(@"<?xml version='1.0' encoding='utf-8' ?>
<Types xmlns='urn:schemas-bltoolkit-net:typeext'>
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
    <Type Name='TestType'>
        <Member Name='SomeRelation'>
            <Relation>
                <MasterIndex Name='MasterIndex1'/>
                <MasterIndex Name='MasterIndex2'/>
                <SlaveIndex  Name='SlaveIndex1'/>
                <SlaveIndex  Name='SlaveIndex2'/>
           </Relation>
        </Member>
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
			Map.Extensions = TypeExtension.GetExtensions("Mapping.xml");

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

		public class TestType
		{
			public object SomeRelation;
		}

		[Test]
		public void MultiKeyRelationTest()
		{
			MappingSchema ms = new MappingSchema();
			ms.Extensions = TypeExtension.GetExtensions("Mapping.xml");

			bool isSet = false;

			List<MapRelationBase> relations = ms.MetadataProvider.GetRelations(ms, ms.Extensions, typeof(TestType), null, out isSet);

			Assert.IsTrue(isSet);
			Assert.AreEqual(1, relations.Count);
			Assert.AreEqual(2, relations[0].MasterIndex.Fields.Length);
			Assert.AreEqual(2, relations[0].SlaveIndex .Fields.Length);
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete("Mapping.xml");
		}
	}
}
