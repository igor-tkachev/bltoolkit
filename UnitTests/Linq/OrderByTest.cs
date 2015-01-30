using System;
using System.Linq;
using BLToolkit.Data.Linq;
using NUnit.Framework;

using BLToolkit.Data.DataProvider;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace Data.Linq
{
	[TestFixture]
	public class OrderByTest : TestBase
	{
		[Test]
		public void OrderBy1()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID descending, ch.ChildID ascending
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ParentID descending, ch.ChildID ascending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy2()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID descending, ch.ChildID ascending
				select ch;

			ForEachProvider(db =>
			{
				var result = 
					from ch in db.Child
					orderby ch.ParentID descending, ch.ChildID ascending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy3()
		{
			var expected =
				from ch in
					from ch in Child
					orderby ch.ParentID descending
					select ch
				orderby ch.ParentID descending , ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ParentID descending , ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy4()
		{
			var expected =
				from ch in
					from ch in Child
					orderby ch.ParentID descending
					select ch
				orderby ch.ParentID descending, ch.ChildID, ch.ParentID + 1 descending
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in
						from ch in db.Child
						orderby ch.ParentID descending
						select ch
					orderby ch.ParentID descending, ch.ChildID, ch.ParentID + 1 descending
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy5()
		{
			var expected =
				from ch in Child
				orderby ch.ChildID % 2, ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ChildID % 2, ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBy6([DataContexts(ExcludeLinqService = true)] string context)
		{
			using (var dataContext = GetDataContext(context))
			{
				if (!(dataContext is TestDbManager)) return;
				var db = (TestDbManager)dataContext;

				var q =
					from person in db.Person
					join patient in db.Patient on person.ID equals patient.PersonID into g
					from patient in g.DefaultIfEmpty()
					orderby person.MiddleName // if comment this line then "Diagnosis" is not selected.
					select new { person.ID, PatientID = patient != null ? (int?)patient.PersonID : null };

				q.ToList();

				Assert.IsFalse(db.LastQuery.Contains("Diagnosis"), "Why do we select Patient.Diagnosis??");

			};
		}

		[Test]
		public void ConditionOrderBy()
		{
			var expected =
				from ch in Child
				orderby ch.ParentID > 0 && ch.ChildID != ch.ParentID descending, ch.ChildID
				select ch;

			ForEachProvider(db =>
			{
				var result =
					from ch in db.Child
					orderby ch.ParentID > 0 && ch.ChildID != ch.ParentID descending, ch.ChildID
					select ch;

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelf1()
		{
			var expected = from p in Parent orderby p select p;

			ForEachProvider(db =>
			{
				var result = from p in db.Parent orderby p select p;
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelf2()
		{
			var expected = from p in Parent1 orderby p select p;

			ForEachProvider(db =>
			{
				var result = from p in db.Parent1 orderby p select p;
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany1()
		{
			var expected =
				from p in Parent.OrderBy(p => p.ParentID)
				from c in Child. OrderBy(c => c.ChildID)
				where p == c.Parent
				select new { p.ParentID, c.ChildID };

			ForEachProvider(new[] { ProviderName.Access }, db =>
			{
				var result =
					from p in db.Parent.OrderBy(p => p.ParentID)
					from c in db.Child. OrderBy(c => c.ChildID)
					where p == c.Parent
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany2()
		{
			var expected =
				from p in Parent1.OrderBy(p => p.ParentID)
				from c in Child.  OrderBy(c => c.ChildID)
				where p.ParentID == c.Parent1.ParentID
				select new { p.ParentID, c.ChildID };

			ForEachProvider(db =>
			{
				var result =
					from p in db.Parent1.OrderBy(p => p.ParentID)
					from c in db.Child.  OrderBy(c => c.ChildID)
					where p == c.Parent1
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderBySelectMany3()
		{
			var expected =
				from p in Parent.OrderBy(p => p.ParentID)
				from c in Child. OrderBy(c => c.ChildID)
				where c.Parent == p
				select new { p.ParentID, c.ChildID };

			ForEachProvider(new[] { ProviderName.Access }, db =>
			{
				var result =
					from p in db.Parent.OrderBy(p => p.ParentID)
					from c in db.Child. OrderBy(c => c.ChildID)
					where c.Parent == p
					select new { p.ParentID, c.ChildID };

				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void OrderAscDesc()
		{
			var expected = Parent.OrderBy(p => p.ParentID).OrderByDescending(p => p.ParentID);

			ForEachProvider(db =>
			{
				var result = db.Parent.OrderBy(p => p.ParentID).OrderByDescending(p => p.ParentID);
				Assert.IsTrue(result.ToList().SequenceEqual(expected));
			});
		}

		[Test]
		public void Count1()
		{
			ForEachProvider(db => Assert.AreEqual(
				   Parent.OrderBy(p => p.ParentID).Count(),
				db.Parent.OrderBy(p => p.ParentID).Count()));
		}

		[Test]
		public void Count2()
		{
			ForEachProvider(new[] { ProviderName.Sybase }, db => Assert.AreEqual(
				   Parent.OrderBy(p => p.ParentID).Take(3).Count(),
				db.Parent.OrderBy(p => p.ParentID).Take(3).Count()));
		}

		[Test]
		public void Min1()
		{
			ForEachProvider(db => Assert.AreEqual(
				   Parent.OrderBy(p => p.ParentID).Min(p => p.ParentID),
				db.Parent.OrderBy(p => p.ParentID).Min(p => p.ParentID)));
		}

		[Test]
		public void Min2()
		{
			ForEachProvider(new[] { ProviderName.Sybase }, db => Assert.AreEqual(
				   Parent.OrderBy(p => p.ParentID).Take(3).Min(p => p.ParentID),
				db.Parent.OrderBy(p => p.ParentID).Take(3).Min(p => p.ParentID)));
		}

		[Test]
		public void Min3()
		{
			ForEachProvider(new[] { ProviderName.Sybase, ProviderName.Informix }, db => Assert.AreEqual(
				   Parent.OrderBy(p => p.Value1).Take(3).Min(p => p.ParentID),
				db.Parent.OrderBy(p => p.Value1).Take(3).Min(p => p.ParentID)));
		}

		[Test]
		public void Distinct()
		{
			ForEachProvider(db => AreEqual(
				(from p in Parent
				join c in Child on p.ParentID equals c.ParentID
				join g in GrandChild on c.ChildID equals  g.ChildID
				select p).Distinct().OrderBy(p => p.ParentID),
				(from p in db.Parent
				join c in db.Child on p.ParentID equals c.ParentID
				join g in db.GrandChild on c.ChildID equals  g.ChildID
				select p).Distinct().OrderBy(p => p.ParentID)));
		}

		[Test]
		public void Take()
		{
			ForEachProvider(db =>
			{
				var q =
					(from p in db.Parent
					 join c in db.Child on p.ParentID equals c.ParentID
					 join g in db.GrandChild on c.ChildID equals g.ChildID
					 select p).Take(3).OrderBy(p => p.ParentID);

				Assert.AreEqual(3, q.AsEnumerable().Count());
			});
		}

		[Test]
		public void Issue_309()
		{
			var expected1 = Child.OrderBy(_ => _.ParentID).ThenBy(_ => _.ChildID).ToList();
			var expected2 = Child.OrderBy(_ => _.ParentID).ThenByDescending(_ => _.ChildID).ToList();

			ForEachProvider(db =>
			{
				var qry = from ss in db.Child
						  orderby ss.ParentID
						  select new { ss };

				var result = qry
					.ThenBy(_ => _.ss.ChildID)
					.Select(_ => _.ss)
					.ToList();

				Assert.IsTrue(result.SequenceEqual(expected1));
				AreEqual(expected1, result);

				var result2 = db.Child
					.ThenBy(_ => _.ParentID)
					.ThenBy(_ => _.ChildID)
					.ToList();

				Assert.IsTrue(result2.SequenceEqual(expected1));
				AreEqual(expected1, result2);

				var result3 = qry
					.ThenByDescending(_ => _.ss.ChildID)
					.Select(_ => _.ss)
					.ToList();

				Assert.IsTrue(result3.SequenceEqual(expected2));
				AreEqual(expected2, result3);
				
				var result4 = db.Child
					.ThenBy(_ => _.ParentID)
					.ThenByDescending(_ => _.ChildID)
					.ToList();
				
				Assert.IsTrue(result3.SequenceEqual(expected2));
				AreEqual(expected2, result4);

				AreEqual(
					   Child.OrderBy(_ => _.ChildID),
					db.Child.ThenBy(_ => _.ChildID));

				AreEqual(
					   Child.OrderByDescending(_ => _.ChildID),
					db.Child.ThenByDescending(_ => _.ParentID));

				var qry2 = from ss in db.Child
						  select new { ss };

				AreEqual(
					     Child.OrderBy(_ => _.ChildID),
					qry2.ThenBy(_ => _.ss.ChildID).Select(_ => _.ss));
				
				AreEqual(
					     Child.OrderByDescending(_ => _.ChildID),
					qry2.ThenByDescending(_ => _.ss.ChildID).Select(_ => _.ss));

			});
		}
	}
}
