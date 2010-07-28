using System;

using BLToolkit.Data;
using BLToolkit.Data.Linq;

namespace Data.Linq
{
	using Model;

	public class TestDbManager : DbManager, ITestDataContext
	{
		public TestDbManager(string configString)
			: base(configString)
		{
		}

		public TestDbManager()
			: base("Sql2008")
		{
		}

		public Table<Person>                 Person             { get { return GetTable<Person>();                 } }
		public Table<Patient>                Patient            { get { return GetTable<Patient>();                } }
		public Table<Parent>                 Parent             { get { return GetTable<Parent>();                 } }
		public Table<Parent1>                Parent1            { get { return GetTable<Parent1>();                } }
		public Table<IParent>                Parent2            { get { return GetTable<IParent>();                } }
		public Table<Parent4>                Parent4            { get { return GetTable<Parent4>();                } }
		public Table<Parent5>                Parent5            { get { return GetTable<Parent5>();                } }
		public Table<ParentInheritanceBase>  ParentInheritance  { get { return GetTable<ParentInheritanceBase>();  } }
		public Table<ParentInheritanceBase2> ParentInheritance2 { get { return GetTable<ParentInheritanceBase2>(); } }
		public Table<ParentInheritanceBase3> ParentInheritance3 { get { return GetTable<ParentInheritanceBase3>(); } }
		public Table<Child>                  Child              { get { return GetTable<Child>();                  } }
		public Table<GrandChild>             GrandChild         { get { return GetTable<GrandChild>();             } }
		public Table<GrandChild1>            GrandChild1        { get { return GetTable<GrandChild1>();            } }
		public Table<LinqDataTypes>          Types              { get { return GetTable<LinqDataTypes>();          } }
		public Table<LinqDataTypes2>         Types2             { get { return GetTable<LinqDataTypes2>();         } }
	}
}
