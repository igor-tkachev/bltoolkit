//@ example:
//@ emit Emit
#include "stdafx.h"

using namespace System;

using namespace NUnit::Framework;

using namespace BLToolkit::Reflection;
using namespace BLToolkit::Reflection::Emit;

namespace Examples {
namespace Reflection {
namespace Emit
{
	[TestFixture]
	public ref class HelloWorld
	{
	public:

		interface class IHello
		{
			void SayHello(String ^toWhom);
		};

		[Test]
		void Test()
		{
			AssemblyBuilderHelper ^assembly = gcnew AssemblyBuilderHelper("HelloWorld.dll");
			
			EmitHelper ^emit = assembly
				->DefineType  ("Hello", Object::typeid, IHello::typeid)
				->DefineMethod(IHello::typeid->GetMethod("SayHello"))
				->Emitter;

			emit
				// string.Format("Hello, {0} World!", toWhom)
				//
				->ldstr   ("Hello, {0} World!")
				->ldarg_1
				->call    (String::typeid, "Format", String::typeid, Object::typeid)

				// Console.WriteLine("Hello, World!");
				//
				->call    (Console::typeid, "WriteLine", String::typeid)
				->ret()
				;

			Type ^type = emit->Method->Type->Create();

			IHello ^hello = (IHello^)TypeAccessor::CreateInstance(type);

			hello->SayHello("C++");
		}
	};
}}}

