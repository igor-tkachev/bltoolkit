using System;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.Linq;
using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class TableFunctionTest : TestBase
	{
		[Test]
		public void Func1()
		{
			using (var db = new TestDbManager())
			{
				var q =
					from p in new Model.Functions(db).GetParentByID(1)
					select p;

				q.ToList();
			}
		}

		[Test]
		public void Func2()
		{
			using (var db = new TestDbManager())
			{
				var q =
					from c in db.Child
					from p in db.GetParentByID(2)
					select p;

				q.ToList();
			}
		}

		readonly Func<DbManager,int,IQueryable<Parent>> _f1 = CompiledQuery.Compile(
			(DbManager db, int id) => from p in new Model.Functions(db).GetParentByID(id) select p);

		[Test]
		public void CompiledFunc1()
		{
			using (var db = new TestDbManager())
			{
				var q = _f1(db, 1);
				q.ToList();
			}
		}

		readonly Func<TestDbManager,int,IQueryable<Parent>> _f2 = CompiledQuery.Compile(
			(TestDbManager db, int id) => from c in db.Child from p in db.GetParentByID(id) select p);

		[Test]
		public void CompiledFunc2()
		{
			using (var db = new TestDbManager())
			{
				var q = _f2(db, 1);
				q.ToList();
			}
		}

		[Test]
		public void WithTabLock()
		{
			using (var db = new TestDbManager())
			{
				var q =
					from p in new Model.Functions(db).WithTabLock<Parent>()
					select p;

				q.ToList();
			}
		}
	}
}
