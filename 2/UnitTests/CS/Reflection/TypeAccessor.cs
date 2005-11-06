using System;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessorTest
	{
		public TypeAccessorTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public class TestObject1
		{
			public int    IntField = 10;
			public string StrField = "10";

			public int    IntProperty { get { return IntField * 2;   } }
			public string StrProperty { get { return StrField + "2"; } }

			public int    SetProperty { set {} }
		}

		[Test]
		public void HasGetter()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject1));

			Assert.IsTrue(ta["IntField"].   HasGetter);
			Assert.IsTrue(ta["IntProperty"].HasGetter);
			Assert.IsTrue(ta["StrField"].   HasGetter);
			Assert.IsTrue(ta["StrProperty"].HasGetter);
			
			Assert.IsFalse(ta["SetProperty"].HasGetter);
		}

		[Test]
		public void GetValue()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject1));
			TestObject1  o  = (TestObject1)ta.CreateInstance();

			Assert.AreEqual(10,    ta["IntField"].   GetValue(o));
			Assert.AreEqual(20,    ta["IntProperty"].GetValue(o));
			Assert.AreEqual("10",  ta["StrField"].   GetValue(o));
			Assert.AreEqual("102", ta["StrProperty"].GetValue(o));
		}

		public abstract class TestObject2
		{
			public int IntField = 10;

			public abstract int    IntProperty { get; set; }
			public abstract string StrProperty { get; set; }
			public abstract int    SetProperty {      set; }
			public abstract int    GetProperty { get;      }

			protected int          ProtField;
			protected int          ProtProperty1 { get { return 0; } }
			protected abstract int ProtProperty2 { get; set; }
		}

		[Test]
		public void HasAbstractGetter()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject2));

			Assert.IsTrue(ta["IntField"].     HasGetter);
			Assert.IsTrue(ta["IntProperty"].  HasGetter);
			Assert.IsTrue(ta["StrProperty"].  HasGetter);
			Assert.IsTrue(ta["SetProperty"].  HasGetter);
			Assert.IsTrue(ta["ProtProperty2"].HasGetter);
		}

		[Test]
		public void GetAbstractValue()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject2));
			TestObject2  o  = (TestObject2)ta.CreateInstance();

			o.IntProperty = 20;
			o.StrProperty = "10";
			o.SetProperty = 30;

			Assert.AreEqual(20,   ta["IntProperty"].GetValue(o));
			Assert.AreEqual("10", ta["StrProperty"].GetValue(o));
			Assert.AreEqual(30,   ta["SetProperty"].GetValue(o));
		}

		[Test]
		public void HasAbstractSetter()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject2));

			Assert.IsTrue(ta["IntField"].     HasSetter);
			Assert.IsTrue(ta["IntProperty"].  HasSetter);
			Assert.IsTrue(ta["StrProperty"].  HasSetter);
			Assert.IsTrue(ta["GetProperty"].  HasSetter);
			Assert.IsTrue(ta["ProtProperty2"].HasSetter);
		}

		[Test]
		public void SetAbstractValue()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject2));
			TestObject2  o  = (TestObject2)ta.CreateInstance();

			ta["IntField"].     SetValue(o, 10);
			ta["IntProperty"].  SetValue(o, 20);
			ta["StrProperty"].  SetValue(o, "30");
			ta["GetProperty"].  SetValue(o, 40);
			ta["ProtProperty2"].SetValue(o, 50);

			Assert.AreEqual(10,   ta["IntField"].     GetValue(o));
			Assert.AreEqual(20,   ta["IntProperty"].  GetValue(o));
			Assert.AreEqual("30", ta["StrProperty"].  GetValue(o));
			Assert.AreEqual(40,   ta["GetProperty"].  GetValue(o));
			Assert.AreEqual(50,   ta["ProtProperty2"].GetValue(o));
		}

		[Test]
		public void ProtectedMembers()
		{
			TypeAccessor ta = TypeAccessor.GetAccessor(typeof(TestObject2));

			Assert.IsNull   (ta["ProtField"]);
			Assert.IsNull   (ta["ProtProperty1"]);
			Assert.IsNotNull(ta["ProtProperty2"]);
		}
	}
}
