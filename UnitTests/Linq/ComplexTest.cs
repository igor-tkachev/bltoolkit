using System;
using System.Linq;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Linq
{
	[TestFixture]
	public class ComplexTest : TestBase
	{
		[Test]
		public void Contains1()
		{
			var q1 =
				from gc in GrandChild
					join max in
						from gch in GrandChild
						group gch by gch.ChildID into g
						select g.Max(c => c.GrandChildID)
					on gc.GrandChildID equals max
				select gc;

			var expected =
				from ch in Child
					join p  in Parent on ch.ParentID equals p.ParentID
					join gc in q1     on p.ParentID  equals gc.ParentID into g
					from gc in g.DefaultIfEmpty()
				where gc == null || !new[] { 111, 222 }.Contains(gc.GrandChildID.Value)
				select new { p.ParentID, gc };

			ForEachProvider(new [] { ProviderName.Firebird, ProviderName.Access }, db =>
			{
				var q2 =
					from gc in db.GrandChild
						join max in
							from gch in db.GrandChild
							group gch by gch.ChildID into g
							select g.Max(c => c.GrandChildID)
						on gc.GrandChildID equals max
					select gc;

				var result =
					from ch in db.Child
						join p  in db.Parent on ch.ParentID equals p.ParentID
						join gc in q2     on p.ParentID  equals gc.ParentID into g
						from gc in g.DefaultIfEmpty()
				where gc == null || !new[] { 111, 222 }.Contains(gc.GrandChildID.Value)
				select new { p.ParentID, gc };

				AreEqual(expected, result);
			});
		}

		[Test]
		public void Contains2()
		{
			var q1 =
				from gc in GrandChild
					join max in
						from gch in GrandChild
						group gch by gch.ChildID into g
						select g.Max(c => c.GrandChildID)
					on gc.GrandChildID equals max
				select gc;

			var expected =
				from ch in Child
					join p  in Parent on ch.ParentID equals p.ParentID
					join gc in q1     on p.ParentID  equals gc.ParentID into g
					from gc in g.DefaultIfEmpty()
				where gc == null || gc.GrandChildID != 111 && gc.GrandChildID != 222
				select new
				{
					Parent       = p,
					GrandChildID = gc,
					Value        = GetValue(gc != null ? gc.ChildID : int.MaxValue)
				};

			ForEachProvider(new [] { ProviderName.Firebird, ProviderName.Access }, db =>
			{
				var q2 =
					from gc in db.GrandChild
						join max in
							from gch in db.GrandChild
							group gch by gch.ChildID into g
							select g.Max(c => c.GrandChildID)
						on gc.GrandChildID equals max
					select gc;

				var result =
					from ch in db.Child
						join p  in db.Parent on ch.ParentID equals p.ParentID
						join gc in q2     on p.ParentID  equals gc.ParentID into g
						from gc in g.DefaultIfEmpty()
				where gc == null || gc.GrandChildID != 111 && gc.GrandChildID != 222
				select new
				{
					Parent       = p,
					GrandChildID = gc,
					Value        = GetValue(gc != null ? gc.ChildID : int.MaxValue)
				};

				AreEqual(expected, result);
			});
		}

		static int GetValue(int? value)
		{
			return value ?? 777;
		}

		[Test]
		public void Contains3()
		{
			var q1 =
				from gc in GrandChild1
					join max in
						from gch in GrandChild1
						group gch by gch.ChildID into g
						select g.Max(c => c.GrandChildID)
					on gc.GrandChildID equals max
				select gc;

			var expected =
				from ch in Child
					join p  in Parent on ch.ParentID equals p.ParentID
					join gc in q1     on p.ParentID  equals gc.ParentID into g
					from gc in g.DefaultIfEmpty()
				where gc == null || !new[] { 111, 222 }.Contains(gc.GrandChildID.Value)
				select new { p.ParentID, gc };

			ForEachProvider(new [] { ProviderName.SQLite, ProviderName.Access }, db =>
			{
				var q2 =
					from gc in db.GrandChild1
						join max in
							from gch in db.GrandChild1
							group gch by gch.ChildID into g
							select g.Max(c => c.GrandChildID)
						on gc.GrandChildID equals max
					select gc;

				var result =
					from ch in db.Child
						join p  in db.Parent on ch.ParentID equals p.ParentID
						join gc in q2     on p.ParentID  equals gc.ParentID into g
						from gc in g.DefaultIfEmpty()
				where gc == null || !new[] { 111, 222 }.Contains(gc.GrandChildID.Value)
				select new { p.ParentID, gc };

				AreEqual(expected, result);
			});
		}

	}
}
