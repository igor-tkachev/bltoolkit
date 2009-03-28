using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Data;
using NUnit.Framework.SyntaxHelpers;

namespace Data
{
	[TestFixture]
	public class ComplexMappingTest
	{
		private const string _parentChildquery = @"
			SELECT       1 AS ParentId
			UNION SELECT 2 AS ParentId

			SELECT       1 AS ChildId, 1 AS ParentId
			UNION SELECT 2 AS ChildId, 1 AS ParentId
			UNION SELECT 3 AS ChildId, 2 AS ParentId
			UNION SELECT 4 AS ChildId, 2 AS ParentId";

		public abstract class Parent : EditableObject
		{
			[MapField("ParentId")]
			public abstract int         Id       { get; set; }
			public abstract List<Child> Children { get; set; }
		}

		[MapField("ParentId", "Parent.Id")]
		public abstract class Child : EditableObject
		{
			[MapField("ChildId")]
			public abstract int    Id     { get; set; }
			public abstract Parent Parent { get; set; }
		}

		[Test]
		public void Test1()
		{
			List<Parent> parents = new List<Parent>();
			MapResultSet[] sets = new MapResultSet[2];

			sets[0] = new MapResultSet(typeof(Parent), parents);
			sets[1] = new MapResultSet(typeof(Child));

			sets[0].AddRelation(sets[1], "ParentID", "ParentID", "Children");
			sets[1].AddRelation(sets[0], "ParentID", "ParentID", "Parent");

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(_parentChildquery)
					.ExecuteResultSet(sets);
			}

			foreach (Parent p in parents)
			{
				Assert.That(p.IsDirty == false);

				foreach (Child c in p.Children)
					Assert.That(c.IsDirty == false);
			}
		}
	}
}
