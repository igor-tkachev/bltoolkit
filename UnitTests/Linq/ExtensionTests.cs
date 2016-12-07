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
		public void TableName([IncludeDataContexts("Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
				db.GetTable<ParenTable>().TableName("Parent").ToList();
		}

		[Test]
		public void DatabaseName([IncludeDataContexts("Sql2008")] string context)
		{
			using (var db = new TestDbManager(context))
				db.GetTable<Parent>().DatabaseName("TestData").ToList();
		}

		[Test]
		public void OwnerName([IncludeDataContexts("Sql2008", "Sql2012")] string context)
		{
			using (var db = new TestDbManager(context))
				db.GetTable<Parent>().OwnerName("dbo").ToList();
		}

		[Test]
		public void AllNames([IncludeDataContexts("Sql2008")] string context)
		{
			using (var db = new TestDbManager(context))
				db.GetTable<ParenTable>()
					.DatabaseName("TestData")
					.OwnerName("dbo")
					.TableName("Parent")
					.ToList();
		}
	}
}
