using System;
using System.Collections.Generic;

using NUnit.Framework;
using BLToolkit.Mapping;
using BLToolkit.Data;

namespace HowTo.Data
{
	//[TestFixture]
	public class ComplexMapping
	{
		const string TestQuery = @"
			-- Parent Data
			SELECT       1 as ParentID
			UNION SELECT 2 as ParentID

			-- Child Data
			SELECT       4 ChildID, 1 as ParentID
			UNION SELECT 5 ChildID, 2 as ParentID
			UNION SELECT 6 ChildID, 2 as ParentID
			UNION SELECT 7 ChildID, 1 as ParentID

			-- Grandchild Data
			SELECT       1 GrandchildID, 4 as ChildID
			UNION SELECT 2 GrandchildID, 4 as ChildID
			UNION SELECT 3 GrandchildID, 5 as ChildID
			UNION SELECT 4 GrandchildID, 5 as ChildID
			UNION SELECT 5 GrandchildID, 6 as ChildID
			UNION SELECT 6 GrandchildID, 6 as ChildID
			UNION SELECT 7 GrandchildID, 7 as ChildID
			UNION SELECT 8 GrandchildID, 7 as ChildID
";

		public class Parent
		{
			[MapField("ParentID")]
			public int ID;

			public List<Child> Children = new List<Child>();
		}

		[MapField("ParentID", "Parent.ID")]
		public class Child
		{
			[MapField("ChildID")]
			public int ID;

			public Parent Parent = new Parent();
		}

		[Test]
		public void Test()
		{
			List<Parent>   parents = new List<Parent>();
			MapResultSet[] sets    = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Parent), parents);
			sets[1] = new MapResultSet(typeof(Child));

			sets[0].AddRelation(sets[1], "ParentID", "ParentID", "Children");
			sets[1].AddRelation(sets[0], "ParentID", "ParentID", "Parent");

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand      (TestQuery)
					.ExecuteResultSet(sets);
			}

			Assert.AreEqual(7, parents[0].Children[1].ID);
		}
	}
}
