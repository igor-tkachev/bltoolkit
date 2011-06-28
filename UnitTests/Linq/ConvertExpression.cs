using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class ConvertExpression : TestBase
	{
		[Test]
		public void ConvertSelect1()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				let children = p.Children.Where(c => c.ParentID > 1)
				select children.Sum(c => c.ChildID),
				from p in db.Parent
				let children = p.Children.Where(c => c.ParentID > 1)
				select children.Sum(c => c.ChildID)));
		}

		[Test]
		public void ConvertSelect2()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				select children2.Sum(c => c.ChildID),
				from p in db.Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				select children2.Sum(c => c.ChildID)));
		}

		[Test]
		public void ConvertSelect3()
		{
			ForEachProvider(db => AreEqual(
				Parent
					.Select(p => new { children1 = p.Children. Where(c => c.ParentID > 1)  })
					.Select(t => new { children2 = t.children1.Where(c => c.ParentID < 10) })
					.Select(t => t.children2.Sum(c => c.ChildID)),
				db.Parent
					.Select(p => new { children1 = p.Children. Where(c => c.ParentID > 1)  })
					.Select(t => new { children2 = t.children1.Where(c => c.ParentID < 10) })
					.Select(t => t.children2.Sum(c => c.ChildID))));
		}

		[Test]
		public void ConvertSelect4()
		{
			ForEachProvider(db => AreEqual(
				Parent
					.Select(p => p.Children. Where(c => c.ParentID > 1))
					.Select(t => t.Where(c => c.ParentID < 10))
					.Select(t => t.Sum(c => c.ChildID)),
				db.Parent
					.Select(p => p.Children. Where(c => c.ParentID > 1))
					.Select(t => t.Where(c => c.ParentID < 10))
					.Select(t => t.Sum(c => c.ChildID))));
		}

		[Test]
		public void ConvertWhere1()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				where children1.Any()
				select children2.Sum(c => c.ChildID),
				from p in db.Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				where children1.Any()
				select children2.Sum(c => c.ChildID)));
		}

		[Test]
		public void ConvertWhere2()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				where children1.Any()
				let children2 = children1.Where(c => c.ParentID < 10)
				select children2.Sum(c => c.ChildID),
				from p in db.Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				where children1.Any()
				let children2 = children1.Where(c => c.ParentID < 10)
				select children2.Sum(c => c.ChildID)));
		}

		[Test]
		public void ConvertWhere3()
		{
			ForEachProvider(db => AreEqual(
				from p in Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				where children2.Any()
				select children2.Sum(c => c.ChildID),
				from p in db.Parent
				let children1 = p.Children.Where(c => c.ParentID > 1)
				let children2 = children1.Where(c => c.ParentID < 10)
				where children2.Any()
				select children2.Sum(c => c.ChildID)));
		}
	}
}
