using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using NUnit.Framework;

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

					from p in db.Person select new { p.ID, p.FirstName }

				).ToList().Where(p => p.ID == 1).First();
				Assert.AreEqual(1,      person.ID);
				Assert.AreEqual("John", person.FirstName);
			});
		}

		static void NewParam(IQueryable<Person> table, int i)
		{
			var person = (

				from p in table select new { i, p.ID, p.FirstName }

			).ToList().Where(p => p.ID == 1).First();

			Assert.AreEqual(i,      person.i);
			Assert.AreEqual(1,      person.ID);
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
			TestJohn(db => from p in db.Person select new Person { ID = p.ID, FirstName = p.FirstName });
		}

		[Test]
		public void NewObject()
		{
			TestJohn(db => from p in db.Person select new Person(p.ID, p.FirstName));
		}

		[Test]
		public void NewInitObject()
		{
			TestJohn(db => from p in db.Person select new Person(p.ID) { FirstName = p.FirstName });
		}

		[Test]
		public void NewWithExpr()
		{
			TestPerson(1, "John1", db => from p in db.Person select new Person(p.ID) { FirstName = (p.FirstName + "1\r\r\r").TrimEnd('\r') });
		}

		[Test]
		public void MultipleSelect1()
		{
			TestJohn(db => db.Person
				.Select(p => new { PersonID = p.ID, Name = p.FirstName })
				.Select(p => new Person(p.PersonID) { FirstName = p.Name }));
		}

		[Test]
		public void MultipleSelect2()
		{
			TestJohn(db => 
				from p in db.Person
				select new { PersonID = p.ID, Name = p.FirstName } into pp
				select new Person(pp.PersonID) { FirstName = pp.Name });
		}

		[Test]
		public void MultipleSelect3()
		{
			TestJohn(db => db.Person
				.Select(p => new        { PersonID = p.ID,       Name      = p.FirstName })
				.Select(p => new Person { ID       = p.PersonID, FirstName = p.Name      })
				.Select(p => new        { PersonID = p.ID,       Name      = p.FirstName })
				.Select(p => new Person { ID       = p.PersonID, FirstName = p.Name      }));
		}

		[Test]
		public void MultipleSelect4()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new        { p2 })
				.Select(p3 => new Person { ID = p3.p2.p1.ID, FirstName = p3.p2.p1.FirstName }));
		}

		[Test]
		public void MultipleSelect5()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new Person { ID = p2.p1.ID, FirstName = p2.p1.FirstName })
				.Select(p3 => new        { p3 })
				.Select(p4 => new Person { ID = p4.p3.ID, FirstName = p4.p3.FirstName }));
		}

		[Test]
		public void MultipleSelect6()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1 })
				.Select(p2 => new Person { ID = p2.p1.ID, FirstName = p2.p1.FirstName })
				.Select(p3 => p3)
				.Select(p4 => new Person { ID = p4.ID,    FirstName = p4.FirstName }));
		}

		[Test]
		public void MultipleSelect7()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { ID = p1.ID + 1, p1.FirstName })
				.Select(p2 => new Person { ID = p2.ID - 1, FirstName = p2.FirstName }));
		}

		[Test]
		public void MultipleSelect8()
		{
			ForEachProvider(db =>
			{
				var person = (

					db.Person
						.Select(p1 => new Person { ID = p1.ID * 2,           FirstName = p1.FirstName })
						.Select(p2 => new        { ID = p2.ID / "22".Length, p2.FirstName })

				).ToList().Where(p => p.ID == 1).First();
				Assert.AreEqual(1,      person.ID);
				Assert.AreEqual("John", person.FirstName);
			});
		}

		[Test]
		public void MultipleSelect9()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { ID = p1.ID - 1, p1.FirstName })
				.Select(p2 => new Person { ID = p2.ID + 1, FirstName = p2.FirstName })
				.Select(p3 => p3)
				.Select(p4 => new        { ID = p4.ID * "22".Length, p4.FirstName })
				.Select(p5 => new Person { ID = p5.ID / 2, FirstName = p5.FirstName }));
		}

		[Test]
		public void MultipleSelect10()
		{
			TestJohn(db => db.Person
				.Select(p1 => new        { p1.ID, p1 })
				.Select(p2 => new        { p2.ID, p2.p1, p2 })
				.Select(p3 => new        { p3.ID, p3.p1.FirstName, p11 = p3.p2.p1, p3 })
				.Select(p4 => new Person { ID = p4.p11.ID, FirstName = p4.p3.p1.FirstName }));
		}

		[Test]
		public void SelectScalar()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person select new { p } into p1 select p1.p

				).ToList().Where(p => p.ID == 1).First();

				Assert.AreEqual(1, q.ID);
			});
		}

		[Test]
		public void SelectScalar11()
		{
			ForEachProvider(db =>
			{
				var n = (from p in db.Person select p.ID).ToList().Where(id => id == 1).First();
				Assert.AreEqual(1, n);
			});
		}

		[Test]
		public void SelectScalar2()
		{
			ForEachProvider(db =>
			{
				var q = (from p in db.Person select new { p }).ToList().Where(p => p.p.ID == 1).First();
				Assert.AreEqual(1, q.p.ID);
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

		[Test]
		public void SelectScalar22()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person select new { p1 = p, p2 = p } into p1 where p1.p1.ID == 1 && p1.p2.ID == 1 select p1

				).ToList().First();

				Assert.AreEqual(1, q.p1.ID);
				Assert.AreEqual(1, q.p2.ID);
			});
		}

		[Test]
		public void SelectScalar23()
		{
			ForEachProvider(db =>
			{
				var q = (from p in db.Person select p.ID into p1 where p1 == 1 select new { p1 }).ToList().First();
				Assert.AreEqual(1, q.p1);
			});
		}

		void Foo(Expression<Func<IDataReader,object>> func)
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

		protected object MapDataReaderToObject(Type destObjectType, IDataReader dataReader, int slotNumber, int[] index)
		{
			return null;
		}

		void Bar()
		{
			Foo(rd => MapDataReaderToObject(typeof(string), rd, 10, new[] { 0, 2, 3, 4, 5 }));
		}

		//[Test]
		public void Test___()
		{
			Bar();
		}
	}
}
