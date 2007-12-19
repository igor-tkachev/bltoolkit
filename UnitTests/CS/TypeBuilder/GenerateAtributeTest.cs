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
		public class NonInheritedAttribute : Attribute
		{
			public NonInheritedAttribute()
			{
			}

			public NonInheritedAttribute(Type t, string s)
			{
			}

			public NonInheritedAttribute(Type t, string s, int i, AttributeTargets e)
			{
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

#if FW2
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
#endif
	}
}
