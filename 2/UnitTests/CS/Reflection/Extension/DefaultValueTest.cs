using System;
using System.IO;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;

namespace Reflection.Extension
{
	[TestFixture]
	public class DefaultValueTest
	{
		[SetUp]
		public void SetUp()
		{
			Map.DefaultSchema = new DefaultMappingSchema();

			using (StreamWriter sw = File.CreateText("Mapping.xml"))
			{
				sw.WriteLine(@"<?xml version='1.0' encoding='utf-8' ?>
<Types xmlns='urn:schemas-bltoolkit-net:typeext'>
    <Type Name='Enum3'>
        <Member Name='Value1' MapValue='1' MapValue-Type='System.String' />
        <Member Name='Value2' MapValue='3' MapValue-Type='System.String' DefaultValue=''/>
    </Type>
    <Type Name='Enum4'>
        <Member Name='Value1' MapValue='1' MapValue-Type='System.String' />
        <Member Name='Value2' MapValue='3' MapValue-Type='System.String' DefaultValue=''/>
    </Type>
    <Type Name='Dest'>
        <Member Name='Field2'>
            <MapValue Value='1' OrigValue='Value1' />
            <MapValue Value='2' OrigValue='Value2' />
            <DefaultValue Value='Value1' />
        </Member>
        <Member Name='Field3' DefaultValue='Value1'>
            <MapValue Value='1' OrigValue='Value1' />
            <MapValue Value='2' OrigValue='Value2' />
        </Member>
    </Type>
</Types>");
			}
		}

		[TearDown]
		public void TearDown()
		{
			File.Delete("Mapping.xml");
		}

		public enum Enum1
		{
			[MapValue("1")]               Value1 = 11,
			[MapValue("2"), DefaultValue] Value2 = 12
		}

		[MapValue(Enum2.Value1, "1")]
		[MapValue(Enum2.Value2, "2")]
		[DefaultValue(Enum2.Value2)]
		public enum Enum2
		{
			Value1,
			Value2
		}

		public enum Enum3
		{
			Value1,
			Value2
		}

		public enum Enum4
		{
			[MapValue("1")]               Value1,
			[MapValue("2"), DefaultValue] Value2
		}

		public class Source
		{
			public string Field1 = "11";
			public string Field2 = "22";
			public string Field3 = "33";
		}

		public class Dest
		{
			public Enum1 Field1;
			public Enum1 Field2;
			public Enum2 Field3;
		}

		[Test]
		public void Test1()
		{
			Map.Extensions = TypeExtension.GetExtenstions("Mapping.xml");

			Enum1 e1 = (Enum1)Map.ValueToEnum("3", typeof(Enum1));
			Assert.AreEqual(Enum1.Value2, e1);

			Enum2 e2 = (Enum2)Map.ValueToEnum("3", typeof(Enum2));
			Assert.AreEqual(Enum2.Value2, e2);

			Enum3 e3 = (Enum3)Map.ValueToEnum("4", typeof(Enum3));
			Assert.AreEqual(Enum3.Value2, e3);

			Enum4 e4 = (Enum4)Map.ValueToEnum("4", typeof(Enum4));
			Assert.AreEqual(Enum4.Value2, e4);

			Dest o = (Dest)Map.ObjectToObject(new Source(), typeof(Dest));

			Assert.AreEqual(Enum1.Value2, o.Field1);
			Assert.AreEqual(Enum1.Value1, o.Field2);
			Assert.AreEqual(Enum2.Value1, o.Field3);
		}
	}
}
