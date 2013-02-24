using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Mapping;
using BLToolkit.Mapping.MemberMappers;

using NUnit.Framework;

namespace Mapping
{
	[TestFixture]
	public class MemberMapperDefaultMappersTest
	{
		public class Object1
		{
			[MemberMapper(typeof(XMLSerialisationMapper))]
			public List<object> Lst1;
			[MemberMapper(typeof(BinarySerialisationMapper))]
			public List<object> Lst2;
			[MemberMapper(typeof(BinarySerialisationToBase64StringMapper))]
			public List<object> Lst3;
			[MemberMapper(typeof(JSONSerialisationMapper))]
			public List<object> Lst4;
		}

		[Test]
		public void DefaultMemberMapperTest()
		{
			ObjectMapper om = Map.GetObjectMapper(typeof(Object1));

			Object1 o = new Object1();

			o.Lst1 = new List<object>() { "Hello", "this", "is", "a", "test", 1, 2, 3 };
			o.Lst2 = o.Lst1;
			o.Lst3 = o.Lst1;
			o.Lst4 = o.Lst1;

			var xml = om.GetValue(o, "Lst1");
			var bytearr = om.GetValue(o, "Lst2");
			var base64 = om.GetValue(o, "Lst3");
			var json = om.GetValue(o, "Lst4");

			var o2 = new Object1();
			om.SetValue(o2, "Lst1", xml);
			om.SetValue(o2, "Lst2", bytearr);
			om.SetValue(o2, "Lst3", base64);
			om.SetValue(o2, "Lst4", json);

			Assert.IsTrue(o.Lst1.SequenceEqual(o2.Lst1));
			Assert.IsTrue(o.Lst2.SequenceEqual(o2.Lst2));
			Assert.IsTrue(o.Lst3.SequenceEqual(o2.Lst3));
			Assert.IsTrue(o.Lst4.SequenceEqual(o2.Lst4));   
		}
	}
}
