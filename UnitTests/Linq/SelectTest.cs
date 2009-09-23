using System;
using System.Linq;
using BLToolkit.Data.DataProvider;
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
			var expected = from p in Person select new {p.ID, p.FirstName};

			ForEachProvider(db =>
			{
				var result = from p in db.Person select new { p.ID, p.FirstName };
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		void NewParam(IQueryable<Person> table, int i)
		{
			var expected = from p in Person select new { i, p.ID, p.FirstName };
			var result   = from p in table  select new { i, p.ID, p.FirstName };

			Assert.IsTrue(result.ToList().SequenceEqual(expected));
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
		public void Coalesce()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person
					where p.ID == 1
					select new
					{
						p.ID,
						FirstName  = p.FirstName  ?? "None",
						MiddleName = p.MiddleName ?? "None"
					}

				).ToList().First();

				Assert.AreEqual(1,      q.ID);
				Assert.AreEqual("John", q.FirstName);
				Assert.AreEqual("None", q.MiddleName);
			});
		}

		[Test]
		public void Coalesce2()
		{
			ForEachProvider(db =>
			{
				var q = (

					from p in db.Person
					where p.ID == 1
					select new
					{
						p.ID,
						FirstName  = p.MiddleName ?? p.FirstName  ?? "None",
						LastName   = p.LastName   ?? p.FirstName  ?? "None",
						MiddleName = p.MiddleName ?? p.MiddleName ?? "None"
					}

				).ToList().First();

				Assert.AreEqual(1,        q.ID);
				Assert.AreEqual("John",   q.FirstName);
				Assert.AreEqual("Pupkin", q.LastName);
				Assert.AreEqual("None",   q.MiddleName);
			});
		}

		[Test]
		public void Concatenation()
		{
			ForEachProvider(db =>
			{
				var q = from p in db.Person where p.ID == 1 select new { p.ID, FirstName  = "123" + p.FirstName + "456" };
				var f = q.Where(p => p.FirstName == "123John456").ToList().First();
				Assert.AreEqual(1, f.ID);
			});
		}
	}
}
