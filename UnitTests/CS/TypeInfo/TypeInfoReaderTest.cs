using System;
using System.IO;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;

namespace A.TypeInfoTest
{
	[TestFixture]
	public class TypeInfoReaderTest
	{
		[SetUp]
		public void SetUp()
		{
			using (StreamWriter sw = File.CreateText("Mapping.xml"))
			{
				sw.WriteLine(@"<?xml version='1.0' encoding='utf-8' ?>
<Types xmlns='urn:schemas-bltoolkit-net:typeinfo'>
    <Type Name='Dest'>
    </Type>
    <Type Name='TriState'>
        <Member Name='Yes'           MapValue='yes'   MapValue-Type='System.String' />
        <Member Name='No'>
            <MapValue Value='no' Type='System.String' />
            <MapValue Type='System.String'>N</MapValue>
        </Member>
        <Member Name='Maybe'         MapValue='xz'    MapValue-Type='System.String' />
        <Member Name='NotApplicable' MapValue-Type='System.String' MapValue='(n/a)' />
    </Type>
</Types>");
			}
		}

		//<null_value    target='NotApplicable' />
		//<default_value target='NotApplicable' />

		[TypeExtension("TriState")]
		public enum TriState { Yes, No, NotApplicable };

		public class Source
		{
			public string Field1 = "no";
		}

		[TypeExtension("Dest")]
		public class Dest
		{
			private TriState _f1 = TriState.NotApplicable;

			public TriState Field1
			{
				get { return _f1;  }
				set { _f1 = value; }
			}
		}

		[Test]
		public void Test()
		{
			Map.Extensions = TypeExtension.GetExtenstions("Mapping.xml");

			object o = Map.Extensions["TriState"]["Yes"]["MapValue"].Value;

			Assert.AreEqual("yes", o);

			//Source  s = new Source();
			//Dest    d = (Dest)Map.ObjectToObject(s, typeof(Dest));
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete("Mapping.xml");
		}
	}
}
