using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using NUnit.Framework;

namespace Examples.Reflection.Emit
{
	[TestFixture]
	public class HelloWorldNormal
	{
		public interface IHello
		{
			void SayHello(string toWhom);
		}

		[Test]
		public void Test()
		{
			AssemblyName asmName = new AssemblyName();

			asmName.Name = "HelloWorld";

			AssemblyBuilder asmBuilder =
				Thread.GetDomain().DefineDynamicAssembly(asmName, AssemblyBuilderAccess.RunAndSave);

			ModuleBuilder   modBuilder  = asmBuilder.DefineDynamicModule("HelloWorld");

			TypeBuilder     typeBuilder = modBuilder.DefineType(
				"Hello",
				TypeAttributes.Public,
				typeof(object),
				new Type[] { typeof(IHello) });

			MethodBuilder   methodBuilder = typeBuilder.DefineMethod("SayHello", 
									MethodAttributes.Private | MethodAttributes.Virtual,
									typeof(void), 
									new Type[] { typeof(string) });

			typeBuilder.DefineMethodOverride(methodBuilder, typeof(IHello).GetMethod("SayHello"));

			ILGenerator il = methodBuilder.GetILGenerator();

			// string.Format("Hello, {0} World!", toWhom)
			//
			/*[a]*/il.Emit/*[/a]*/(OpCodes.Ldstr, "Hello, {0} World!");
			/*[a]*/il.Emit/*[/a]*/(OpCodes.Ldarg_1);
			/*[a]*/il.Emit/*[/a]*/(OpCodes.Call, typeof(string).GetMethod("Format", new Type[] { typeof(string), typeof(object) }));

			// Console.WriteLine("Hello, World!");
			//
			/*[a]*/il.Emit/*[/a]*/(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));
			/*[a]*/il.Emit/*[/a]*/(OpCodes.Ret);

			Type   type  = typeBuilder.CreateType();

			IHello hello = (IHello)Activator.CreateInstance(type);

			hello.SayHello("Emit");
		}
	}
}
