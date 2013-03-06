using System;
using System.Collections.Generic;
using System.Linq;

using BLToolkit.Data.DataProvider;
using BLToolkit.Data.Linq;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

using NUnit.Framework;

namespace Data.Linq.UserTests
{
	[TestFixture]
	public class SelectManyDeleteTest : TestBase
	{
		[TableName(Name = "GrandChild")]
		public new class GrandChild
		{
			public int ChildID { get; set; }
		}

		[TableName(Name = "Child")]
		public new class Child
		{
			public int ParentID { get; set; }
			public int ChildID  { get; set; }

			[Association(ThisKey = "ChildID", OtherKey = "ChildID", CanBeNull = false)]
			public List<GrandChild> GrandChildren { get; set; }
		}

		[TableName(Name = "Parent")]
		public new class Parent
		{
			[Identity, PrimaryKey(1)]
			public int ParentID { get; set; }

			[Association(ThisKey = "ParentID", OtherKey = "ParentID", CanBeNull = true)]
			public List<Child> Children { get; set; }
		}

		[Test]
		public void Test([DataContexts(
			ProviderName.Access, ProviderName.DB2, ProviderName.Informix, "Oracle",
			ProviderName.PostgreSQL, ProviderName.SqlCe, ProviderName.SQLite, ProviderName.Firebird
			)] string context)
		{
			var harnessIds = new int[2];

			using (var db = GetDataContext(context))
			{
				db.GetTable<Parent>()
					.Where     (x => harnessIds.Contains(x.ParentID))
					.SelectMany(x => x.Children)
					.SelectMany(x => x.GrandChildren)
					.Delete();
			}
		}
	}
}

