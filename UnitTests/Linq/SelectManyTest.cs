using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class SelectManyTest : TestBase
	{
		[Test]
		public void Test1()
		{
			TestJohn(db =>
			{
				var q = db.Person.Select(p => p);

				return db.Person
					.SelectMany(p1 => q/*db.Person.Select(p => p)*/, (p1, p2) => new {p1, p2})
					.Where(t => t.p1.ID == t.p2.ID && t.p1.ID == 1)
					.Select(t => new Person {ID = t.p1.ID, FirstName = t.p2.FirstName});
			});
		}

		[Test]
		public void Test2()
		{
			TestJohn(db =>
				from p1 in db.Person
				from p2 in db.Person
				from p3 in db.Person
				where p1.ID == p2.ID && p1.ID == p3.ID && p1.ID == 1
				select new Person { ID = p1.ID, FirstName = p2.FirstName, LastName = p3.LastName } );
		}



		
		void Foo(Expression<Func<IDataReader,object>> func)
		{
			/*
			ParameterExpression CS$0$0000;
			this.Foo
			(
				Expression.Lambda<Func<IDataReader, object>>
				(
					Expression.Call
					(
						Expression.Field
						(
							Expression.Constant(this, typeof(SelectManyTest)),
							fieldof(SelectManyTest._dic)
						),
						(MethodInfo) methodof
						(
							Dictionary<string, string>.get_Item,
							Dictionary<string, string>
						),
						new Expression[] { Expression.Constant("123", typeof(string)) }
					),
					new ParameterExpression[] { CS$0$0000 = Expression.Parameter(typeof(IDataReader), "rd") }
				)
			);
			*/
		}

		Dictionary<string,string> _dic = new Dictionary<string,string>();

		void Bar()
		{
			Foo(rd => _dic["123"]);
		}

		//[Test]
		public void Test___()
		{
			Bar();
		}
	}
}
