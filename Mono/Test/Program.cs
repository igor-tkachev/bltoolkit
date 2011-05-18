using System;
using System.Linq;

using Data.Linq;

namespace Test
{
	class Program
	{
		static void Main()
		{
			using (var db = new TestDbManager("MySql"))
			{
				var src = new TestDataSource(db);
				var allParents = src.Parent;
				var allPersons = src.Person;

				var @p1 = "a";
				var query = from e in allParents
							join c in allPersons on e.Id equals c.Id
							where c.Name.StartsWith(@p1)
							orderby c.Name
							select e;

				Console.WriteLine(query);

				var result = query.ToArray();

				Console.WriteLine(result.Length);
			}
		}

		public class ClassPerson
		{
			public int Id;
			public string Name;
		}

		public class ClassParent
		{
			public int  Id;
			public int? Value1;
		}

		public class TestDataSource
		{
			private ITestDataContext m_db;

			public TestDataSource(ITestDataContext db)
			{
				m_db = db;
			}

			public IQueryable<ClassParent> Parent
			{
				get
				{
					var query = from p in m_db.Parent
								select new ClassParent
								{
									Id = p.ParentID,
									Value1 = p.Value1,
								};
					return query;
				}
			}

			public IQueryable<ClassPerson> Person
			{
				get
				{
					var query = from p in m_db.Person
								select new ClassPerson
								{
									Id = p.ID,
									Name = p.FirstName,
								};
					return query;
				}
			}
		}
	}
}
