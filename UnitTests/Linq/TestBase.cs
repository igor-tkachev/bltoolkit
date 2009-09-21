using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using BLToolkit.Data.DataProvider;
using BLToolkit.Common;
using BLToolkit.Data;

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
				if (args.Name.IndexOf("Oracle.DataAccess") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\Oracle\Oracle.DataAccess.dll");
				if (args.Name.IndexOf("IBM.Data.DB2") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\IBM\IBM.Data.DB2.dll");
				if (args.Name.IndexOf("Mono.Security") >= 0)
					return Assembly.LoadFrom(@"..\..\..\..\Redist\PostgreSql\Mono.Security.dll");

				return null;
			};

			DbManager.TraceSwitch = new TraceSwitch("DbManager", "DbManager trace switch", "Info");
		}

		static readonly List<string> _configurations = new List<string>
		{
			ProviderName.DB2,
			ProviderName.Informix,
			"Oracle",
			ProviderName.Firebird,
			ProviderName.PostgreSQL,
			ProviderName.MySql,
			"Sql2008",
			"Sql2005",
			ProviderName.SqlCe,
			ProviderName.Sybase,
			ProviderName.SQLite,
			ProviderName.Access,
		};

		protected void ForEachProvider(Action<TestDbManager> func)
		{
			ForEachProvider(Array<string>.Empty, func);
		}

		protected void ForEachProvider(string[] exceptList, Action<TestDbManager> func)
		{
			for (var i = 0; i < _configurations.Count; i++)
			{
				if (exceptList.Contains(_configurations[i]))
					continue;

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
				var person = func(db).ToList().Where(p => p.ID == id).First();

				Assert.AreEqual(id,        person.ID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestPerson(1, "John", func);
		}

		protected void TestOnePerson(string[] exceptList, int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			ForEachProvider(exceptList, db =>
			{
				var list = func(db).ToList();

				Assert.AreEqual(1, list.Count);

				var person = list[0];

				Assert.AreEqual(id,        person.ID);
				Assert.AreEqual(firstName, person.FirstName);
			});
		}

		protected void TestOnePerson(int id, string firstName, Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(Array<string>.Empty, id, firstName, func);
		}

		protected void TestOneJohn(string[] exceptList, Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(exceptList, 1, "John", func);
		}

		protected void TestOneJohn(Func<TestDbManager,IQueryable<Person>> func)
		{
			TestOnePerson(Array<string>.Empty, 1, "John", func);
		}

		private   List<Person>       _person;
		protected List<Person>        Person
		{
			get
			{
				if (_person == null)
					using (var db = new TestDbManager("Sql2008"))
						_person = db.Person.ToList();
				return _person;
			}
		}

		private   List<Parent>        _parent;
		protected List<Parent>         Parent
		{
			get
			{
				if (_parent == null)
					using (var db = new TestDbManager("Sql2008"))
						_parent = db.Parent.ToList();
				return _parent;
			}
		}

		private   List<Child>         _child;
		protected List<Child>          Child
		{
			get
			{
				if (_child == null)
					using (var db = new TestDbManager("Sql2008"))
						_child = db.Child.ToList();
				return _child;
			}
		}

		private   List<GrandChild>    _grandChild;
		protected List<GrandChild>     GrandChild
		{
			get
			{
				if (_grandChild == null)
					using (var db = new TestDbManager("Sql2008"))
						_grandChild = db.GrandChild.ToList();
				return _grandChild;
			}
		}

		private   List<LinqDataTypes> _types;
		protected List<LinqDataTypes>  Types
		{
			get
			{
				if (_types == null)
					using (var db = new TestDbManager("Sql2008"))
						_types = db.Types.ToList();
				return _types;
			}
		}

		protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result)
		{
			var expectedList = expected.ToList();
			var resultList   = result.  ToList();

			Assert.AreNotEqual(0, expectedList.Count());
			Assert.AreNotEqual(0, resultList.  Count());

			Assert.AreEqual(0, resultList.  ToList().Except(expectedList).Count());
			Assert.AreEqual(0, expectedList.ToList().Except(resultList).  Count());
		}
	}
}
