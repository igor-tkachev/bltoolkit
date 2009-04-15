using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

using BLToolkit.Mapping;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class SelectTest : TestBase
	{
		[Test]
		public void SimpleDirect()
		{
			TestJohn(db => db.Person);
		}

		[Test]
		public void Simple()
		{
			TestJohn(db => from p in db.Person select p);
		}

		[Test]
		public void SimpleDouble()
		{
			TestJohn(db => db.Person.Select(p => p).Select(p => p));
		}

		[Test]
		public void New()
		{
			ForEachProvider(db =>
			{
				var person = (

					from p in db.Person select new { p.PersonID, p.FirstName }

				).ToList().Where(p => p.PersonID == 1).First();
				Assert.AreEqual(1,      person.PersonID);
				Assert.AreEqual("John", person.FirstName);
			});
		}

		static void NewParam(IQueryable<Person> table, int i)
		{
			var person = (

				from p in table select new { i, p.PersonID, p.FirstName }

			).ToList().Where(p => p.PersonID == 1).First();

			Assert.AreEqual(i,      person.i);
			Assert.AreEqual(1,      person.PersonID);
			Assert.AreEqual("John", person.FirstName);
		}

		[Test]
		public void NewParam()
		{
			ForEachProvider(db => { for (int i = 0; i < 5; i++) NewParam(db.Person, i); });
		}

		[Test]
		public void InitObject()
		{
			TestJohn(db => from p in db.Person select new Person { PersonID = p.PersonID, FirstName = p.FirstName });
		}

		[Test]
		public void NewObject()
		{
			TestJohn(db => from p in db.Person select new Person(p.PersonID, p.FirstName));
		}

		[Test]
		public void NewInitObject()
		{
			TestJohn(db => from p in db.Person select new Person(p.PersonID) { FirstName = p.FirstName });
		}

		[Test]
		public void NewWithExpr()
		{
			TestPerson(1, "John1", db => from p in db.Person select new Person(p.PersonID) { FirstName = (p.FirstName + "1\r\r\r").TrimEnd('\r') });
		}

		[Test]
		public void MultipleSelect1()
		{
			TestJohn(db => db.Person
				.Select(p => new { ID = p.PersonID, Name = p.FirstName })
				.Select(p => new Person(p.ID) { FirstName = p.Name }));
		}

		[Test]
		public void MultipleSelect2()
		{
			TestJohn(db => 
				from p in db.Person
				select new { ID = p.PersonID, Name = p.FirstName } into pp
				select new Person(pp.ID) { FirstName = pp.Name });
		}

		[Test]
		public void MultipleSelect3()
		{
			TestJohn(db => db.Person
				.Select(p => new        { ID       = p.PersonID, Name      = p.FirstName })
				.Select(p => new Person { PersonID = p.ID,       FirstName = p.Name      })
				.Select(p => new        { ID       = p.PersonID, Name      = p.FirstName })
				.Select(p => new Person { PersonID = p.ID,       FirstName = p.Name      }));
		}

		[Test]
		public void MultipleSelect4()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new        { p2 })
				.Select(p3 => new Person { PersonID = p3.p2.p1.PersonID, FirstName = p3.p2.p1.FirstName }));
		}

		[Test]
		public void MultipleSelect5()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new Person { PersonID = p2.p1.PersonID, FirstName = p2.p1.FirstName })
				.Select(p3 => new        { p3 })
				.Select(p4 => new Person { PersonID = p4.p3.PersonID, FirstName = p4.p3.FirstName }));
		}

		[Test]
		public void MultipleSelect6()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new Person { PersonID = p2.p1.PersonID, FirstName = p2.p1.FirstName })
				.Select(p3 => p3)
				.Select(p4 => new Person { PersonID = p4.PersonID,    FirstName = p4.FirstName }));
		}

		[Test]
		public void MultipleSelect7()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { PersonID = p1.PersonID + 1, p1.FirstName })
				.Select(p2 => new Person { PersonID = p2.PersonID - 1, FirstName = p2.FirstName }));
		}

		[Test]
		public void MultipleSelect8()
		{
			ForEachProvider(db =>
			{
				var person = (

					db.Person
						.Select(p1 => new Person { PersonID = p1.PersonID * 2,           FirstName = p1.FirstName })
						.Select(p2 => new        { PersonID = p2.PersonID / "22".Length, p2.FirstName })

				).ToList().Where(p => p.PersonID == 1).First();
				Assert.AreEqual(1,      person.PersonID);
				Assert.AreEqual("John", person.FirstName);
			});
		}

		[Test]
		public void MultipleSelect9()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { PersonID = p1.PersonID - 1, p1.FirstName })
				.Select(p2 => new Person { PersonID = p2.PersonID + 1, FirstName = p2.FirstName })
				.Select(p3 => p3)
				.Select(p4 => new        { PersonID = p4.PersonID * "22".Length, p4.FirstName })
				.Select(p5 => new Person { PersonID = p5.PersonID / 2, FirstName = p5.FirstName }));
		}

		[Test]
		public void SelectScalar()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person select new { p } into p1 select p1.p
					
				).ToList().Where(p => p.PersonID == 1).First();

				Assert.AreEqual(1, q.PersonID);
			});
		}

		[Test]
		public void SelectScalar2()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person select new { p1 = p, p2 = p } into p1 where p1.p1.PersonID == 1 && p1.p2.PersonID == 1 select p1
					
				).ToList().Where(p => p.p1.PersonID == 1).First();

				Assert.AreEqual(1, q.p1.PersonID);
				Assert.AreEqual(1, q.p2.PersonID);
			});
		}

		[Test]
		public void SelectScalar11()
		{
			ForEachProvider(db =>
			{
				var n = (from p in db.Person select p.PersonID).ToList().Where(id => id == 1).First();
				Assert.AreEqual(1, n);
			});
		}

		[Test]
		public void SelectScalar21()
		{
			ForEachProvider(db =>
			{
				var n = (from p in db.Person select p.FirstName.Length).ToList().Where(len => len == 4).First();
				Assert.AreEqual(4, n);
			});
		}

		void Foo(Expression<Func<IDataReader,MappingSchema,int>> func)
		{
			/*
			ParameterExpression p0;
			Expression.Lambda
			(
				Expression.New(
					(ConstructorInfo) methodof(
						<>f__AnonymousType1<int, int, string>..ctor,
						<>f__AnonymousType1<int, int, string>),
						new Expression[]
						{
							Expression.Constant(i),
							Expression.Field
							(
								p0 = Expression.Parameter(typeof(Person), "p"),
								fieldof(Person.PersonID)
							),
							Expression.Field
							(
								p0,
								fieldof(Person.FirstName)
							)
						},
						new MethodInfo[]
						{
							(MethodInfo) methodof
							(
								<>f__AnonymousType1<int, int, string>.get_i,
								<>f__AnonymousType1<int, int, string>
							),
							(MethodInfo) methodof
							(
								<>f__AnonymousType1<int, int, string>.get_PersonID,
								<>f__AnonymousType1<int, int, string>
							),
							(MethodInfo) methodof
							(
								<>f__AnonymousType1<int, int, string>.get_FirstName,
								<>f__AnonymousType1<int, int, string>
							)
						}
					),
					new ParameterExpression[] { p0 }
				)
			)
			*/
		}

		void Bar()
		{
			//Foo(e => e);
		}

		//[Test]
		public void Test___()
		{
			Bar();
		}
	}
}
