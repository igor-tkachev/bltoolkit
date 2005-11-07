//@ example:
//@ emit Emit
using System;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.Reflection.Emit;

namespace Examples.Reflection.Emit
{
	[TestFixture]
	public class HelloWorld
	{
		public interface IHello
		{
			void SayHello();
		}

		[Test]
		public void Test()
		{
			EmitHelper emit = new AssemblyBuilderHelper("HelloWorld.dll")
				.DefineType  ("Hello", typeof(object), typeof(IHello))
				.DefineMethod(typeof(IHello).GetMethod("SayHello"))
				.Emitter;

			emit
				.ldstr("Hello, World!")
				.call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }))
				.ret()
				;

			Type type = emit.Method.Type.Create();

			IHello hello = (IHello)TypeAccessor.CreateInstance(type);

			hello.SayHello();
		}
	}
}
