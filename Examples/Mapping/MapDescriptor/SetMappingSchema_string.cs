/// example:
/// desc SetMappingSchema(string)
/// comment:
/// The following example demonstrates how to use the <b>MapFieldAttribute.IsTrimmable</b> property.
using System;
using System.IO;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_MapDescriptor
{
	[TestFixture]
	public class SetMappingSchema_string
	{
		[SetUp]
		public void SetUp()
		{
			using (StreamWriter sw = File.CreateText("Mapping.xml"))
			{
				sw.WriteLine(@"<?xml version='1.0' encoding='utf-8' ?>
<mapping xmlns='http://www.rsdn.ru/mapping.xsd'>
	<type name='Dest'>
	</type>
    <value_type name='TriState'>
        <value target='Yes'           source='yes'   source_type='System.String' />
        <value target='No'            source='no'    source_type='System.String' />
        <value target='NotApplicable' source='(n/a)' source_type='System.String' />
        <null_value    target='NotApplicable' />
        <default_value target='NotApplicable' />
    </value_type>
</mapping>");
			}
		}

		[MapXml("TriState")]
		public enum TriState { Yes, No, NotApplicable };

		public class Source
		{
			public string Field1 = "no";
		}

		[MapXml("Dest")]
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
			MapDescriptor.SetMappingSchema("Mapping.xml");

			Source  s = new Source();
			Dest    d = (Dest)Map.ToObject(s, typeof(Dest));
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete("Mapping.xml");
		}
	}
}
