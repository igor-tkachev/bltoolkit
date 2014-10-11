using System;
using System.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	[TestFixture]
	public class MultipleQuery : TestBase
	{
		//[Test]
		public void Test1()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Children,
				from p in db.Parent select p.Children));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		//[Test]
		public void Test2()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Children.ToList(),
				from p in db.Parent select p.Children.ToList()));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void Test3()
		{
			ForEachProvider(db => AreEqual(
				from p in    Parent select    Child,
				from p in db.Parent select db.Child));
		}

		//[Test]
		public void Test4()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(db => AreEqual(
				from p in    Parent select p.Children.Select(c => c.ChildID),
				from p in db.Parent select p.Children.Select(c => c.ChildID)));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}

		[Test]
		public void Test5()
		{
			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = true;

			ForEachProvider(db =>
				AreEqual(
					from ch in    Child
					orderby ch.ChildID
					select    Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p),
					from ch in db.Child
					orderby ch.ChildID
					select db.Parent.Where(p => p.ParentID == ch.Parent.ParentID).Select(p => p)));

			BLToolkit.Common.Configuration.Linq.AllowMultipleQuery = false;
		}
	}
}
