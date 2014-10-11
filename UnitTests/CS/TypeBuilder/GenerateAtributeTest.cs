using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class GenerateAttributeTest
	{
		[AttributeUsage(AttributeTargets.All, Inherited = false)]
		public class NonInheritedAttribute: Attribute
		{
			public NonInheritedAttribute()
			{
			}

			public NonInheritedAttribute(Type type, string str)
			{
				_type = type;
			}

			public NonInheritedAttribute(Type type, string str, int i, AttributeTargets e):
				this(type, str)
			{
			}

			private string _namedArgument;
			public  string  NamedArgument
			{
				get { return _namedArgument;  }
				set { _namedArgument = value; }
			}

			private Type _type;
			public  Type  Type
			{
				get { return _type;  }
				set { _type = value; }
			}
		}

		public abstract class TestObject
		{
			[NonInherited]
			public abstract void Method1();

			[GenerateAttribute(typeof (NonInheritedAttribute))]
			public abstract void Method2();

			[GenerateAttribute(typeof (NonInheritedAttribute), typeof(TestObject), "str", 123, AttributeTargets.Field)]
			public abstract void Method3();

			[GenerateAttribute(typeof (NonInheritedAttribute))]
			public virtual void Method4(){}

			[GenerateAttribute(typeof(NonInheritedAttribute),
				NamedArgumentNames  = new string[] { "NamedArgument"},
				NamedArgumentValues = new object[] { "SomeValue"})]
			public virtual void Method5() { }

			public abstract int Prop1
			{
				[GenerateAttribute(typeof(NonInheritedAttribute))] get;
				[GenerateAttribute(typeof(NonInheritedAttribute))] set;
			}

			private        int _prop2;
			public virtual int  Prop2
			{
				[GenerateAttribute(typeof(NonInheritedAttribute))] get { return _prop2; }
				[GenerateAttribute(typeof(NonInheritedAttribute))] set { _prop2 = value; }
			}

			// This affects the underlying field, not the property itself.
			//
			[GenerateAttribute(typeof(NonInheritedAttribute))]
			public abstract int Prop3 { get; set; }
		}

		[Test]
		public void MainTest()
		{
			TestObject o = (TestObject)TypeAccessor.CreateInstance(typeof(TestObject));
			Type    type = o.GetType();

			Assert.IsNull   (Attribute.GetCustomAttribute(type.GetMethod("Method1"), typeof(NonInheritedAttribute)));

			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetMethod("Method2"), typeof(NonInheritedAttribute)));
			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetMethod("Method3"), typeof(NonInheritedAttribute)));
			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetMethod("Method4"), typeof(NonInheritedAttribute)));

			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetProperty("Prop1").GetGetMethod(), typeof(NonInheritedAttribute)));
			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetProperty("Prop1").GetSetMethod(), typeof(NonInheritedAttribute)));
			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetProperty("Prop2").GetGetMethod(), typeof(NonInheritedAttribute)));
			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetProperty("Prop2").GetSetMethod(), typeof(NonInheritedAttribute)));

			Assert.IsNotNull(Attribute.GetCustomAttribute(type.GetField("_prop3", BindingFlags.Instance | BindingFlags.NonPublic), typeof(NonInheritedAttribute)));

			NonInheritedAttribute attribute = (NonInheritedAttribute)
				Attribute.GetCustomAttribute(type.GetMethod("Method5"), typeof(NonInheritedAttribute));

			Assert.IsNotNull(attribute);
			Assert.AreEqual("SomeValue", attribute.NamedArgument);
		}

		public abstract class BadObject
		{
			[GenerateAttribute(typeof (NonInheritedAttribute), "str")]
			public abstract void Method();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void MismatchedAgsTest()
		{
			TypeAccessor.CreateInstance(typeof(BadObject));
		}

		// In FW1.1 an attribute argument may not be null, as stated in 17.2:
		// "An attribute argument must be a constant expression,
		// typeof expression or one-dimensional array creation expression."

		public abstract class NullArgObject
		{
			[GenerateAttribute(typeof(NonInheritedAttribute), null, null)]
			public abstract void Method();
		}

		[Test]
		public void NullArgTest()
		{
			TypeAccessor.CreateInstance(typeof(NullArgObject));
		}

		public class CustomGenerateAttribute: GenerateAttributeAttribute
		{
			public CustomGenerateAttribute(): base(typeof(NonInheritedAttribute))
			{
				this["NamedArgument"] = "NamedValue";
				this["Type"]          = Type.GetType("System.Int32");
			}
		}

		[CustomGenerate]
		public abstract class CustomObject
		{
		}

		[Test]
		public void CustomGenerateTest()
		{
			CustomObject o = TypeAccessor<CustomObject>.CreateInstanceEx();
			Type    type = o.GetType();

			NonInheritedAttribute attr = (NonInheritedAttribute)
				Attribute.GetCustomAttribute(type, typeof(NonInheritedAttribute));

			Assert.That(attr,               Is.Not.Null);
			Assert.That(attr.Type,          Is.EqualTo(typeof(int)));
			Assert.That(attr.NamedArgument, Is.EqualTo("NamedValue"));
		}
	}
}
