using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using BLToolkit.Common;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Sql.SqlProvider;
using BLToolkit.Mapping;
using BLToolkit.ServiceModel;
using Mono.Model;
using NUnit.Framework;

namespace Mono
{
    public class TestBase
    {
        static TestBase()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string assembly;

                if (args.Name.IndexOf("Sybase.AdoNet2.AseClient") >= 0) assembly = @"Sybase\Sybase.AdoNet2.AseClient.dll";
                else if (args.Name.IndexOf("Oracle.DataAccess") >= 0) assembly = @"Oracle\Oracle.DataAccess.dll";
                else if (args.Name.IndexOf("IBM.Data.DB2") >= 0) assembly = @"IBM\IBM.Data.DB2.dll";
                else if (args.Name.IndexOf("Npgsql") >= 0) assembly = @"PostgreSql\Npgsql.dll";
                else if (args.Name.IndexOf("Mono.Security") >= 0) assembly = @"PostgreSql\Mono.Security.dll";
                else if (args.Name.IndexOf("System.Data.SqlServerCe,") >= 0) assembly = @"SqlCe\System.Data.SqlServerCe.dll";
                else
                    return null;

                assembly = @"..\..\..\..\Redist\" + assembly;

                if (!File.Exists(assembly))
                    assembly = @"..\..\" + assembly;

                return Assembly.LoadFrom(assembly);
            };

            DbManager.TurnTraceSwitchOn();

            var path = Path.GetDirectoryName(typeof(DbManager).Assembly.CodeBase.Replace("file:///", ""));

            foreach (var info in Providers)
            {
                try
                {
                    Type type;

                    if (info.Assembly == null)
                    {
                        type = typeof(DbManager).Assembly.GetType(info.Type, true);
                    }
                    else
                    {
#if FW4
						var fileName = info.Assembly + ".4.dll";
#else
                        var fileName = info.Assembly + ".3.dll";
#endif

                        var assembly = Assembly.LoadFile(Path.Combine(Path.DirectorySeparatorChar + path, fileName));

                        type = assembly.GetType(info.Type, true);
                    }

                    DbManager.AddDataProvider(type);

                    info.Loaded = true;
                }
                catch (Exception ex)
                {
                    info.Loaded = false;
                }
            }
        }

        readonly List<ServiceHost> _hosts = new List<ServiceHost>();

        const int StartIP = 12345;

        [TestFixtureSetUp]
        public void SetUp()
        {
            LinqService.TypeResolver = str =>
            {
                switch (str)
                {
                    case "Data.Linq.Model.Gender": return typeof(Gender);
                    case "Data.Linq.Model.Person": return typeof(Person);
                    default: return null;
                }
            };

            var ip = StartIP;

            foreach (var info in Providers)
            {
                ip++;

                if (!info.Loaded)
                    continue;

                var host = new ServiceHost(new LinqService(info.Name) { AllowUpdates = true }, new Uri("net.tcp://localhost:" + ip));

                host.Description.Behaviors.Add(new ServiceMetadataBehavior());
                host.Description.Behaviors.Find<ServiceDebugBehavior>().IncludeExceptionDetailInFaults = true;
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                host.AddServiceEndpoint(
                    typeof(ILinqService),
                    new NetTcpBinding(SecurityMode.None)
                    {
                        MaxReceivedMessageSize = 10000000,
                        MaxBufferPoolSize = 10000000,
                        MaxBufferSize = 10000000,
                        CloseTimeout = new TimeSpan(00, 01, 00),
                        OpenTimeout = new TimeSpan(00, 01, 00),
                        ReceiveTimeout = new TimeSpan(00, 10, 00),
                        SendTimeout = new TimeSpan(00, 10, 00),
                    },
                    "LinqOverWCF");
				
				try {
                	host.Open();
				} catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					throw ex;
				}

                _hosts.Add(host);
            }
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            foreach (var host in _hosts)
                host.Close();
        }

        protected class ProviderInfo
        {
            public ProviderInfo(string name, string assembly, string type)
            {
                Name = name;
                Assembly = assembly;
                Type = type;
            }

            public readonly string Name;
            public readonly string Assembly;
            public readonly string Type;
            public bool Loaded;
        }

        protected static readonly List<ProviderInfo> Providers = new List<ProviderInfo>
		{
			new ProviderInfo(ProviderName.MySql,      "BLToolkit.Data.DataProvider.MySql",      "BLToolkit.Data.DataProvider.MySqlDataProvider"),
		};

        static IEnumerable<ITestDataContext> GetProviders(IEnumerable<string> exceptList)
        {
            var ip = StartIP;

            foreach (var info in Providers)
            {
                ip++;

                if (exceptList.Contains(info.Name))
                    continue;

                Debug.WriteLine(info.Name, "Provider ");

                if (!info.Loaded)
                    continue;

                yield return new TestDbManager(info.Name);

                //var dx = new TestServiceModelDataContext(ip);

                //Debug.WriteLine(((IDataContext)dx).ContextID, "Provider ");

                //yield return dx;
            }
        }

        protected void ForEachProvider(Type expectedException, string[] exceptList, Action<ITestDataContext> func)
        {
            Exception ex = null;

            foreach (var db in GetProviders(exceptList))
            {
                try
                {
                    if (db is DbManager)
                        ((DbManager)db).BeginTransaction();

                    func(db);
                }
                catch (Exception e)
                {
                    if (expectedException == null || e.GetType() != expectedException)
                        throw;

                    ex = e;
                }
                finally
                {
                    db.Dispose();
                }
            }

            if (ex != null)
                throw ex;
        }

        protected void ForEachProvider(string[] exceptList, Action<ITestDataContext> func)
        {
            ForEachProvider(null, exceptList, func);
        }

        protected void ForEachProvider(Action<ITestDataContext> func)
        {
            ForEachProvider(Array<string>.Empty, func);
        }

        protected void ForEachProvider(Type expectedException, Action<ITestDataContext> func)
        {
            ForEachProvider(expectedException, Array<string>.Empty, func);
        }

        protected void Not0ForEachProvider(Func<ITestDataContext, int> func)
        {
            ForEachProvider(db => Assert.Less(0, func(db)));
        }

        protected void TestPerson(int id, string firstName, Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            ForEachProvider(db =>
            {
                var person = func(db).ToList().Where(p => p.ID == id).First();

                Assert.AreEqual(id, person.ID);
                Assert.AreEqual(firstName, person.FirstName);
            });
        }

        protected void TestJohn(Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            TestPerson(1, "John", func);
        }

        protected void TestOnePerson(string[] exceptList, int id, string firstName, Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            ForEachProvider(exceptList, db =>
            {
                var list = func(db).ToList();

                Assert.AreEqual(1, list.Count);

                var person = list[0];

                Assert.AreEqual(id, person.ID);
                Assert.AreEqual(firstName, person.FirstName);
            });
        }

        protected void TestOnePerson(int id, string firstName, Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            TestOnePerson(Array<string>.Empty, id, firstName, func);
        }

        protected void TestOneJohn(string[] exceptList, Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            TestOnePerson(exceptList, 1, "John", func);
        }

        protected void TestOneJohn(Func<ITestDataContext, System.Linq.IQueryable<Person>> func)
        {
            TestOnePerson(Array<string>.Empty, 1, "John", func);
        }

        private List<LinqDataTypes> _types;
        protected List<LinqDataTypes> Types
        {
            get
            {
                if (_types == null)
                    using (var db = new TestDbManager("Sql2008"))
                        _types = db.Types.ToList();
                return _types;
            }
        }

        private List<LinqDataTypes2> _types2;
        protected List<LinqDataTypes2> Types2
        {
            get
            {
                if (_types2 == null)
                    using (var db = new TestDbManager("Sql2008"))
                        _types2 = db.Types2.ToList();
                return _types2;
            }
        }

        private List<Person> _person;
        protected List<Person> Person
        {
            get
            {
                if (_person == null)
                    using (var db = new TestDbManager("Sql2008"))
                        _person = db.Person.ToList();

                return _person;
            }
        }

        private List<Patient> _patient;
        protected List<Patient> Patient
        {
            get
            {
                if (_patient == null)
                {
                    using (var db = new TestDbManager("Sql2008"))
                        _patient = db.Patient.ToList();

                    foreach (var p in _patient)
                        p.Person = Person.Single(ps => ps.ID == p.PersonID);
                }

                return _patient;
            }
        }

        #region Parent/Child Model

        private List<Parent> _parent;
        protected IEnumerable<Parent> Parent
        {
            get
            {
                if (_parent == null)
                    using (var db = new TestDbManager("Sql2008"))
                    {
                        _parent = db.Parent.ToList();
                        db.Close();

                        foreach (var p in _parent)
                        {
                            p.Children = Child.Where(c => c.ParentID == p.ParentID).ToList();
                            p.GrandChildren = GrandChild.Where(c => c.ParentID == p.ParentID).ToList();
                            p.Types = Types.FirstOrDefault(t => t.ID == p.ParentID);
                        }
                    }

                foreach (var parent in _parent)
                    yield return parent;
            }
        }

        private List<Parent1> _parent1;
        protected List<Parent1> Parent1
        {
            get
            {
                return _parent1 ?? (_parent1 = Parent.Select(p => new Parent1 { ParentID = p.ParentID, Value1 = p.Value1 }).ToList());
            }
        }

        private List<Parent4> _parent4;
        protected List<Parent4> Parent4
        {
            get
            {
                return _parent4 ?? (_parent4 = Parent.Select(p => new Parent4 { ParentID = p.ParentID, Value1 = Map.ToEnum<TypeValue>(p.Value1) }).ToList());
            }
        }

        private List<Parent5> _parent5;
        protected List<Parent5> Parent5
        {
            get
            {
                if (_parent5 == null)
                {
                    _parent5 = Parent.Select(p => new Parent5 { ParentID = p.ParentID, Value1 = p.Value1 }).ToList();

                    foreach (var p in _parent5)
                        p.Children = _parent5.Where(c => c.Value1 == p.ParentID).ToList();
                }

                return _parent5;
            }
        }

        private List<ParentInheritanceBase> _parentInheritance;
        protected IEnumerable<ParentInheritanceBase> ParentInheritance
        {
            get
            {
                if (_parentInheritance == null)
                    _parentInheritance = Parent.Select(p =>
                        p.Value1 == null ? new ParentInheritanceNull { ParentID = p.ParentID } :
                        p.Value1.Value == 1 ? new ParentInheritance1 { ParentID = p.ParentID, Value1 = p.Value1.Value } :
                         (ParentInheritanceBase)new ParentInheritanceValue { ParentID = p.ParentID, Value1 = p.Value1.Value }
                    ).ToList();

                foreach (var item in _parentInheritance)
                    yield return item;
            }
        }

        private List<ParentInheritanceValue> _parentInheritanceValue;
        protected List<ParentInheritanceValue> ParentInheritanceValue
        {
            get
            {
                return _parentInheritanceValue ?? (_parentInheritanceValue =
                    ParentInheritance.Where(p => p is ParentInheritanceValue).Cast<ParentInheritanceValue>().ToList());
            }
        }

        private List<ParentInheritance1> _parentInheritance1;
        protected List<ParentInheritance1> ParentInheritance1
        {
            get
            {
                return _parentInheritance1 ?? (_parentInheritance1 =
                    ParentInheritance.Where(p => p is ParentInheritance1).Cast<ParentInheritance1>().ToList());
            }
        }

        private List<ParentInheritanceBase4> _parentInheritance4;
        protected List<ParentInheritanceBase4> ParentInheritance4
        {
            get
            {
                return _parentInheritance4 ?? (_parentInheritance4 = Parent
                    .Where(p => p.Value1.HasValue && (new[] { 1, 2 }.Contains(p.Value1.Value)))
                    .Select(p => p.Value1 == 1 ?
                        (ParentInheritanceBase4)new ParentInheritance14 { ParentID = p.ParentID } :
                        (ParentInheritanceBase4)new ParentInheritance24 { ParentID = p.ParentID }
                ).ToList());
            }
        }

        private List<Child> _child;
        protected IEnumerable<Child> Child
        {
            get
            {
                if (_child == null)
                    using (var db = new TestDbManager("Sql2008"))
                    {
                        _child = db.Child.ToList();
                        db.Clone();

                        foreach (var ch in _child)
                        {
                            ch.Parent = Parent.Single(p => p.ParentID == ch.ParentID);
                            ch.Parent1 = Parent1.Single(p => p.ParentID == ch.ParentID);
                            ch.ParentID2 = new Parent3 { ParentID2 = ch.Parent.ParentID, Value1 = ch.Parent.Value1 };
                            ch.GrandChildren = GrandChild.Where(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID).ToList();
                        }
                    }

                foreach (var child in _child)
                    yield return child;
            }
        }

        private List<GrandChild> _grandChild;
        protected IEnumerable<GrandChild> GrandChild
        {
            get
            {
                if (_grandChild == null)
                    using (var db = new TestDbManager("Sql2008"))
                    {
                        _grandChild = db.GrandChild.ToList();
                        db.Close();

                        foreach (var ch in _grandChild)
                            ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
                    }

                foreach (var grandChild in _grandChild)
                    yield return grandChild;
            }
        }

        private List<GrandChild1> _grandChild1;
        protected List<GrandChild1> GrandChild1
        {
            get
            {
                if (_grandChild1 == null)
                    using (var db = new TestDbManager("Sql2008"))
                    {
                        _grandChild1 = db.GrandChild1.ToList();

                        foreach (var ch in _grandChild1)
                        {
                            ch.Parent = Parent1.Single(p => p.ParentID == ch.ParentID);
                            ch.Child = Child.Single(c => c.ParentID == ch.ParentID && c.ChildID == ch.ChildID);
                        }
                    }

                return _grandChild1;
            }
        }

        #endregion

        protected void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> result)
        {
            var resultList = result.ToList();
            var expectedList = expected.ToList();

            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreEqual(expectedList.Count, resultList.Count, "Expected and result lists are different. Lenght: ");

            var exceptExpected = resultList.Except(expectedList).Count();
            var exceptResult = expectedList.Except(resultList).Count();

            if (exceptResult != 0 || exceptExpected != 0)
                for (var i = 0; i < resultList.Count; i++)
                    Debug.WriteLine(string.Format("{0} {1} --- {2}", Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]));

            Assert.AreEqual(0, exceptExpected);
            Assert.AreEqual(0, exceptResult);
        }

        protected void AreSame<T>(IEnumerable<T> expected, IEnumerable<T> result)
        {
            var resultList = result.ToList();
            var expectedList = expected.ToList();

            Assert.AreNotEqual(0, expectedList.Count);
            Assert.AreEqual(expectedList.Count, resultList.Count);

            var b = expectedList.SequenceEqual(resultList);

            if (!b)
                for (var i = 0; i < resultList.Count; i++)
                    Debug.WriteLine(string.Format("{0} {1} --- {2}", Equals(expectedList[i], resultList[i]) ? " " : "-", expectedList[i], resultList[i]));

            Assert.IsTrue(b);
        }

        protected void CompareSql(string result, string expected)
        {
            var ss = expected.Trim('\r', '\n').Split('\n');

            while (ss.All(_ => _.Length > 0 && _[0] == '\t'))
                for (var i = 0; i < ss.Length; i++)
                    ss[i] = ss[i].Substring(1);

            Assert.AreEqual(string.Join("\n", ss), result.Trim('\r', '\n'));
        }
    }
}
