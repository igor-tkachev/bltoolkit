using System;
using System.IO;
using NUnit.Framework;

using BLToolkit.Data.DataProvider;

using Data.Linq;

namespace Create
{
	[TestFixture]
	public class CreateData : TestBase
	{
		static void RunScript(Action<string> execute, string divider, string name)
		{
			Console.WriteLine("=== " + name + " === \n");

			var text = File.ReadAllText(@"..\..\..\..\Data\Create Scripts\" + name + ".sql");
			var cmds = text.Replace("\r", "").Replace(divider, "\x1").Split('\x1');

			Exception exception = null;

			foreach (var cmd in cmds)
			{
				var command = cmd.Trim();

				if (command.Length == 0)
					continue;

				try 
				{
					Console.WriteLine(command);
					execute(command);
					Console.WriteLine("\nOK\n");
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine("\nFAILED\n");

					if (exception == null)
						exception = ex;
				}
			}

			if (exception != null)
				throw exception;
		}

		static void RunScript(TestDbManager db, string divider, string name)
		{
			RunScript(cmd => db.SetCommand(cmd).ExecuteNonQuery(), divider, name);
		}

		[Test] public void DB2       () { using (var db = new TestDbManager(ProviderName.DB2))        RunScript(db, "\nGO\n",  "DB2");        }
		[Test] public void Informix  () { using (var db = new TestDbManager(ProviderName.Informix))   RunScript(db, "\nGO\n",  "Informix");   }
		[Test] public void Oracle    () { using (var db = new TestDbManager("Oracle"))                RunScript(db, "\n/\n",   "Oracle");     }
		[Test] public void Firebird  () { using (var db = new TestDbManager(ProviderName.Firebird))   RunScript(db, "COMMIT;", "Firebird2");  }
		[Test] public void PostgreSQL() { using (var db = new TestDbManager(ProviderName.PostgreSQL)) RunScript(db, "\nGO\n",  "PostgreSQL"); }
		[Test] public void MySql     () { using (var db = new TestDbManager(ProviderName.MySql))      RunScript(db, "\nGO\n",  "MySql");      }
		[Test] public void Sql2008   () { using (var db = new TestDbManager("Sql2008"))               RunScript(db, "\nGO\n",  "MsSql");      }
		[Test] public void Sql2005   () { using (var db = new TestDbManager("Sql2005"))               RunScript(db, "\nGO\n",  "MsSql");      }
		[Test] public void SqlCe     () { using (var db = new TestDbManager(ProviderName.SqlCe))      RunScript(db, "\nGO\n",  "SqlCe");      }
		[Test] public void Sybase    () { using (var db = new TestDbManager(ProviderName.Sybase))     RunScript(db, "\nGO\n",  "Sybase");     }
		[Test] public void SQLite    () { using (var db = new TestDbManager(ProviderName.SQLite))     RunScript(db, "\nGO\n",  "SQLite");     }
		[Test] public void Access    () { using (var db = new TestDbManager(ProviderName.Access))     RunScript(db, "\nGO\n",  "Access");     }
	}
}
