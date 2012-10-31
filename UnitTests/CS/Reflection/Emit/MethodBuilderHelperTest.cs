using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Reflection.Emit;

namespace Reflection.Emit
{
	[TestFixture]
	public class MethodBuilderHelperTest
	{
		public abstract class TestObject
		{
			public    abstract int Property { get; }
			protected abstract int Method1(float f);
			public    abstract int Method2(float f);

			public int Method3(float f) { return Method1(f); }
		}

		[Test]
		public void Test()
		{
			TypeBuilderHelper typeBuilder = 
				new AssemblyBuilderHelper("HelloWorld.dll").DefineType("Test", typeof(TestObject));

			// Property
			//
			PropertyInfo        propertyInfo  = typeof(TestObject).GetProperty("Property");
			MethodBuilderHelper methodBuilder = typeBuilder.DefineMethod(propertyInfo.GetGetMethod());
			EmitHelper          emit          = methodBuilder.Emitter;

			emit
				.ldc_i4(10)
				.ret()
				;

			// Method1
			//
			MethodInfo methodInfo = typeof(TestObject).GetMethod(
				"Method1", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			methodBuilder = typeBuilder.DefineMethod(methodInfo);
			emit          = methodBuilder.Emitter;

			emit
				.ldc_i4(10)
				.ret()
				;

			// Method2
			//
			methodInfo = typeof(TestObject).GetMethod("Method2", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			methodBuilder = typeBuilder.DefineMethod(
				"Method2",
				MethodAttributes.Virtual |
				MethodAttributes.Public |
				MethodAttributes.HideBySig	|
				MethodAttributes.PrivateScope |
				MethodAttributes.VtableLayoutMask,
				typeof(int),
				new Type[] { typeof(float) });

			typeBuilder.TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);

			emit = methodBuilder.Emitter;

			emit
				.ldc_i4(10)
				.ret()
				;

			// Create type.
			//
			Type type = typeBuilder.Create();

			TestObject obj = (TestObject)Activator.CreateInstance(type);

			Assert.AreEqual(10, obj.Property);
			Assert.AreEqual(10, obj.Method3(0));
			Assert.AreEqual(10, obj.Method2(0));
		}
	}
}
