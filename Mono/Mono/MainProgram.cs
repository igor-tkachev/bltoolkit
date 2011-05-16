using System.Reflection;
using NUnit.Core;

namespace Mono
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            CoreExtensions.Host.InitializeService();

			/*
            var runner = new SimpleTestRunner();
            var package = new TestPackage("IdlTest");
            var loc = Assembly.GetExecutingAssembly().Location;
            package.Assemblies.Add( loc );
            if( runner.Load(package) )
            {
                var result = runner.Run( new NullListener() );
            }
            */
			
			var runner = new IdlTest();
			runner.TestJoin1();
        }
    }
}
