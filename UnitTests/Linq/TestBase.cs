using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

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
			"MySql",
			"Sql2008",
			"Sql2005",
			"Oracle",
			"Sybase",
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

		protected void Not0ForEachProvider(Func<TestDbManager, int> func)
		{
			ForEachProvider(db => Assert.Less(0, func(db)));
		}

		protected void TestJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(db =>
			{
				var person = func(db).ToList().Where(p => p.PersonID == 1).First();

				Assert.AreEqual(1,      person.PersonID);
				Assert.AreEqual("John", person.FirstName);
			});
		}
	}
}
