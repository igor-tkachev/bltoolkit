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
		public void PersonToList()
		{
			TestJohn(db => db.Person);
		}

		[Test]
		public void PersonSelect()
		{
			TestJohn(db => from p in db.Person select p);
		}

		[Test]
		public void PersonDoubleSelect()
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
		public void NewPerson1()
		{
			TestJohn(db => from p in db.Person select new Person { PersonID = p.PersonID, FirstName = p.FirstName });
		}

		[Test]
		public void NewPerson2()
		{
			TestJohn(db => from p in db.Person select new Person(p.PersonID, p.FirstName));
		}

		[Test]
		public void NewPerson3()
		{
			TestJohn(db => from p in db.Person select new Person(p.PersonID) { FirstName = p.FirstName });
		}

		[Test]
		public void NewPerson4()
		{
			TestJohn(db => from p in db.Person select new Person(p.PersonID) { FirstName = (p.FirstName + "\r\r\r").TrimEnd('\r') });
		}

		[Test]
		public void MultipleSelect1()
		{
			TestJohn(db => db.Person.Select(p => new { ID = p.PersonID, Name = p.FirstName }).Select(p => new Person(p.ID) { FirstName = p.Name }));
		}

		[Test]
		public void MultipleSelect2()
		{
			TestJohn(db => db.Person
				.Select(p => new        { ID       = p.PersonID, Name      = p.FirstName })
				.Select(p => new Person { PersonID = p.ID,       FirstName = p.Name      })
				.Select(p => new        { ID       = p.PersonID, Name      = p.FirstName })
				.Select(p => new Person { PersonID = p.ID,       FirstName = p.Name      }));
		}

		[Test]
		public void MultipleSelect3()
		{
			TestJohn(db => db.Person
				.Select(p => new        { p })
				.Select(p => new Person { PersonID = p.p.PersonID, FirstName = p.p.FirstName }));
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
			//Foo((e) => Expression;
		}

		[Test]
		public void Test___()
		{
			Bar();
		}
	}
}
