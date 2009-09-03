using System;
using System.IO;
using BLToolkit.DataAccess;
using Data.Linq.Model;
using NUnit.Framework;

using BLToolkit.Data.DataProvider;

using Data.Linq;

namespace Create
{
	[TestFixture]
	public class CreateData : TestBase
	{
		static void RunScript(Action<string> execute, TestDbManager db, string divider, string name)
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

			new SqlQuery<Parent>().Insert(db, new[]
			{
				new Parent { ParentID = 1, Value1 = 1    },
				new Parent { ParentID = 2, Value1 = null },
				new Parent { ParentID = 3, Value1 = 3    },
				new Parent { ParentID = 4, Value1 = null },
				new Parent { ParentID = 5, Value1 = 5    },
			});

			new SqlQuery<Child>().Insert(db, new[]
			{
				new Child { ParentID = 1, ChildID = 11 },
				new Child { ParentID = 2, ChildID = 21 },
				new Child { ParentID = 2, ChildID = 22 },
				new Child { ParentID = 3, ChildID = 31 },
				new Child { ParentID = 3, ChildID = 32 },
				new Child { ParentID = 3, ChildID = 33 },
				new Child { ParentID = 4, ChildID = 41 },
				new Child { ParentID = 4, ChildID = 42 },
				new Child { ParentID = 4, ChildID = 43 },
				new Child { ParentID = 4, ChildID = 44 },
			});

			new SqlQuery<GrandChild>().Insert(db, new[]
			{
				new GrandChild { ParentID = 1, ChildID = 11, GrandChildID = 111 },
				new GrandChild { ParentID = 2, ChildID = 21, GrandChildID = 211 },
				new GrandChild { ParentID = 2, ChildID = 21, GrandChildID = 212 },
				new GrandChild { ParentID = 2, ChildID = 22, GrandChildID = 221 },
				new GrandChild { ParentID = 2, ChildID = 22, GrandChildID = 222 },
				new GrandChild { ParentID = 3, ChildID = 31, GrandChildID = 311 },
				new GrandChild { ParentID = 3, ChildID = 31, GrandChildID = 312 },
				new GrandChild { ParentID = 3, ChildID = 31, GrandChildID = 313 },
				new GrandChild { ParentID = 3, ChildID = 32, GrandChildID = 321 },
				new GrandChild { ParentID = 3, ChildID = 32, GrandChildID = 322 },
				new GrandChild { ParentID = 3, ChildID = 32, GrandChildID = 323 },
				new GrandChild { ParentID = 3, ChildID = 33, GrandChildID = 331 },
				new GrandChild { ParentID = 3, ChildID = 33, GrandChildID = 332 },
				new GrandChild { ParentID = 3, ChildID = 33, GrandChildID = 333 },
				new GrandChild { ParentID = 4, ChildID = 41, GrandChildID = 411 },
				new GrandChild { ParentID = 4, ChildID = 41, GrandChildID = 412 },
				new GrandChild { ParentID = 4, ChildID = 41, GrandChildID = 413 },
				new GrandChild { ParentID = 4, ChildID = 41, GrandChildID = 414 },
				new GrandChild { ParentID = 4, ChildID = 42, GrandChildID = 421 },
				new GrandChild { ParentID = 4, ChildID = 42, GrandChildID = 422 },
				new GrandChild { ParentID = 4, ChildID = 42, GrandChildID = 423 },
				new GrandChild { ParentID = 4, ChildID = 42, GrandChildID = 424 },
			});
		}

		static void RunScript(TestDbManager db, string divider, string name)
		{
			RunScript(cmd => db.SetCommand(cmd).ExecuteNonQuery(), db, divider, name);
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
