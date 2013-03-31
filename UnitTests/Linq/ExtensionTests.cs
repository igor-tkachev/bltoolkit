using System;
using System.Linq;

using BLToolkit.Data.Linq;

using NUnit.Framework;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class ExtensionTests : TestBase
	{
		public class ParenTable
		{
			public int  ParentID;
			public int? Value1;
		}

		[Test]
		public void TableName([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
				db.GetTable<ParenTable>().TableName("Parent").ToList();
		}

		[Test]
		public void DatabaseName([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
				db.GetTable<Parent>().DatabaseName("BLToolkitData").ToList();
		}

		[Test]
		public void OwnerName([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
				db.GetTable<Parent>().OwnerName("dbo").ToList();
		}

		[Test]
		public void AllNames([Sql2008DataContext]string context)
		{
			using (var db = (TestDbManager)GetDataContext(context))
				db.GetTable<ParenTable>()
					.DatabaseName("BLToolkitData")
					.OwnerName("dbo")
					.TableName("Parent")
					.ToList();
		}
	}
}
