using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Data;

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
				if (args.Name.IndexOf("Oracle.DataAccess") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Oracle\Oracle.DataAccess.dll");
				if (args.Name.IndexOf("IBM.Data.DB2") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\IBM\IBM.Data.DB2.dll");

				return null;
			};

			DbManager.TraceSwitch = new TraceSwitch("DbManager", "DbManager trace switch", "Info");
		}

		static readonly List<string> _configurations = new List<string>
		{
			"Informix",
			"DB2",
			"MySql",
			"Sql2008",
			"Sql2005",
			"Oracle",
			"Sybase",
		};

		protected void ForEachProvider(Action<TestDbManager> func)
		{
			for (var i = 0; i < _configurations.Count; i++)
			{
				var reThrow = false;

				try
				{
					using (var db = new TestDbManager(_configurations[i]))
					{
						//var conn = db.Connection;

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

		protected void TestPerson(int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(db =>
			{
				var person = func(db).ToList().Where(p => p.PersonID == id).First();

				Assert.AreEqual(id,        person.PersonID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestPerson(1, "John", func);
		}

		protected void TestOnePerson(int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(db =>
			{
				var list = func(db).ToList();

				Assert.AreEqual(1, list.Count);

				var person = list[0];

				Assert.AreEqual(id,        person.PersonID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestOneJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(1, "John", func);
		}
	}
}
