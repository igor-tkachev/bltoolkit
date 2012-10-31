using System;
using System.Reflection;

using NUnit.Core.Extensibility;

namespace NUnitAddin
{
	[NUnitAddin]
	public class AssemblyResolverAddin : IAddin
	{
		public bool Install(IExtensionHost host)
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				var dir = Environment.CurrentDirectory;

				if (args.Name.IndexOf("Sybase.AdoNet2.AseClient") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Sybase\Sybase.AdoNet2.AseClient.dll");
				if (args.Name.IndexOf("Oracle.DataAccess") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Oracle\Oracle.DataAccess.dll");
				if (args.Name.IndexOf("IBM.Data.DB2") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\IBM\IBM.Data.DB2.dll");
				if (args.Name.IndexOf("Mono.Security") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\PostgreSql\Mono.Security.dll");

				return null;
			};

			return true;
		}
	}
}
