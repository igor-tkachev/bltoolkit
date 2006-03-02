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
			void SayHello(string toWhom);
		}

		[Test]
		public void Test()
		{
			EmitHelper emit = new AssemblyBuilderHelper("HelloWorld.dll")
				.DefineType  ("Hello", typeof(object), typeof(IHello))
				.DefineMethod(typeof(IHello).GetMethod("SayHello"))
				.Emitter;

			/*[b]*/emit
				// string.Format("Hello, {0}!", toWhom)
				//
				.ldstr   ("Hello, {0}!")
				.ldarg_1
				.call    (typeof(string), "Format", typeof(string), typeof(object))

				// Console.WriteLine("Hello, World!");
				//
				.call    (typeof(Console), "WriteLine", typeof(string))
				.ret()
				;/*[/b]*/

			Type type = emit.Method.Type.Create();

			IHello hello = (IHello)TypeAccessor.CreateInstance(type);

			hello.SayHello("World");
		}
	}
}
