using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Common;
using BLToolkit.Data.Linq;
using BLToolkit.Data.Sql;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Common : TestBase
	{
		[Test]
		public void AsQueryable()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in    Child               select p,
				from p in db.Parent from ch in db.Child.AsQueryable() select p));
		}

		[Test]
		public void Convert()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent from ch in                         Child                select p,
				from p in db.Parent from ch in ((IEnumerable<Child>)db.Child).AsQueryable() select p));
		}

		[Test]
		public void NewCondition()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Value = p.Value1 != null ? p.Value1 : 100 },
				from p in db.Parent select new { Value = p.Value1 != null ? p.Value1 : 100 }));
		}

		[Test]
		public void NewCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select new { Value = p.Value1 ?? 100 },
				from p in db.Parent select new { Value = p.Value1 ?? 100 }));
		}

		[Test]
		public void CoalesceNew()
		{
			Child ch = null;

			ForEachProvider(db => AreEqual(
				from p in    Parent select ch ?? new Child { ParentID = p.ParentID },
				from p in db.Parent select ch ?? new Child { ParentID = p.ParentID }));
		}

		[Test]
		public void ScalarCondition()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 != null ? p.Value1 : 100,
				from p in db.Parent select p.Value1 != null ? p.Value1 : 100));
		}

		[Test]
		public void ScalarCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? 100,
				from p in db.Parent select p.Value1 ?? 100));
		}

		[Test]
		public void ExprCoalesce()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select (p.Value1 ?? 100) + 50,
				from p in db.Parent select (p.Value1 ?? 100) + 50));
		}

		static int GetDefault1()
		{
			return 100;
		}

		[Test]
		public void ClientCoalesce1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? GetDefault1(),
				from p in db.Parent select p.Value1 ?? GetDefault1()));
		}

		static int GetDefault2(int n)
		{
			return n;
		}

		[Test]
		public void ClientCoalesce2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Value1 ?? GetDefault2(p.ParentID),
				from p in db.Parent select p.Value1 ?? GetDefault2(p.ParentID)));
		}

		[Test]
		public void PreferServerFunc1()
		{
			ForEachProvider(db => AreEqual(
				from p in    Person select p.FirstName.Length,
				from p in db.Person select p.FirstName.Length));
		}

		[Test]
		public void PreferServerFunc2()
		{
			ForEachProvider(db => AreEqual(
				from p in    Person select p.FirstName.Length + "".Length,
				from p in db.Person select p.FirstName.Length + "".Length));
		}

		class Test
		{
			class Entity
			{
				public Test TestField = null;
			}

			public Test TestClosure(TestDbManager db)
			{
				return db.Person.Select(_ => new Entity { TestField = this }).First().TestField;
			}
		}

		[Test]
		public void ClosureTest()
		{
			ForEachProvider(db => Assert.AreNotEqual(
				new Test().TestClosure(db),
				new Test().TestClosure(db)));
		}

		class MyDbManager : TestDbManager
		{
			public MyDbManager() : base("Sql2008") {}

			protected override SqlQuery ProcessQuery(SqlQuery sqlQuery)
			{
				if (sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.Into.Name == "Parent")
				{
					var expr =
						new QueryVisitor().Find(sqlQuery.Set, e =>
						{
							if (e.ElementType == QueryElementType.SetExpression)
							{
								var se = (SqlQuery.SetExpression)e;
								return ((SqlField)se.Column).Name == "ParentID";
							}

							return false;
						}) as SqlQuery.SetExpression;

					if (expr != null)
					{
						var value = ConvertTo<int>.From(((IValueContainer)expr.Expression).Value);

						if (value == 555)
						{
							var tableName = "Parent1";
							var dic       = new Dictionary<IQueryElement,IQueryElement>();

							sqlQuery = new QueryVisitor().Convert(sqlQuery, e =>
							{
								if (e.ElementType == QueryElementType.SqlTable)
								{
									var oldTable = (SqlTable)e;
									var newTable = new SqlTable(oldTable) { Name = tableName };

									foreach (var field in oldTable.Fields.Values)
										dic.Add(field, newTable.Fields[field.Name]);

									return newTable;
								}

								IQueryElement ex;
								return dic.TryGetValue(e, out ex) ? ex : null;
							});
						}
					}
				}

				return sqlQuery;
			}
		}

		[Test]
		public void QueryTest()
		{
			using (var db = new MyDbManager())
			{
				var n = 555;

				db.Parent.Insert(() => new Parent
				{
					ParentID = n,
					Value1   = n
				});
			}
		}
	}
}
