using System;
using System.Collections.Generic;
using System.Reflection;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	public class TestBase
	{
		static TestBase()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
			{
				if (args.Name.IndexOf("Sybase.AdoNet2.AseClient") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Sybase\Sybase.AdoNet2.AseClient.dll");
				if (args.Name.IndexOf("Oracle.DataAccess.dll") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Oracle\Oracle.DataAccess.dll");

				return null;
			};
		}

		static List<string> _configurations = new List<string>
		{
			//"MySql",
			"Sql2008",
			//"Sql2005",
			//"Oracle",
			//"Sybase",
		};

		protected void ForEachProvider(Action<TestDbManager> func)
		{
			for (int i = 0; i < _configurations.Count; i++)
			{
				var reThrow = false;

				try
				{
					using (var db = new TestDbManager(_configurations[i]))
					{
						var conn = db.Connection;

						reThrow = true;
						func(db);
					}
				}
				catch
				{
					if (reThrow) throw;

					_configurations.RemoveAt(i);
					i--;
				}
			}
		}
	}
}
