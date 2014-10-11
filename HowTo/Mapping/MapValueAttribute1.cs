using System;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace HowTo.Mapping
{
	[TestFixture]
	public class MapValue1
	{
		public class SourceObject
		{
			public string Value = "Y";
		}

		public class TestObject1
		{
			// The attribute is applied to a field/property.
			//
			[/*[a]*/MapValue(true,  "Y")/*[/a]*/]
			[/*[a]*/MapValue(false, "N")/*[/a]*/]
			public bool Value;
		}

		[Test]
		public void Test1()
		{
			SourceObject so = new SourceObject();
			TestObject1  to = Map.ObjectToObject<TestObject1>(so);

			Assert.AreEqual(true, to.Value);
		}

		// The attribute is applied to a class.
		//
		[/*[a]*/MapValue(true,  "Y")/*[/a]*/]
		[/*[a]*/MapValue(false, "N")/*[/a]*/]
		public class TestObject2
		{
			public bool Value;
		}

		[Test]
		public void Test2()
		{
			SourceObject so = new SourceObject();
			TestObject2  to = Map.ObjectToObject<TestObject2>(so);

			Assert.AreEqual(true, to.Value);
		}

		// The attribute is applied to a base class and affects all child classes.
		//
		[/*[a]*/MapValue(typeof(bool), true,  "Y")/*[/a]*/]
		[/*[a]*/MapValue(typeof(bool), false, "N")/*[/a]*/]
		public class ObjectBase
		{
		}

		public class TestObject3 : /*[a]*/ObjectBase/*[/a]*/
		{
			public bool Value;
		}

		[Test]
		public void Test3()
		{
			SourceObject so = new SourceObject();
			TestObject3  to = Map.ObjectToObject<TestObject3>(so);

			Assert.AreEqual(true, to.Value);
		}
	}
}
