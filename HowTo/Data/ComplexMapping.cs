using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Mapping;
using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Reflection.Extension;

namespace HowTo.Data
{
	[TestFixture]
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
			[MapField("ParentID"), /*[a]*/PrimaryKey/*[/a]*/]
			public int ID;

			/*[a]*/[Relation(typeof(Child))]/*[/a]*/
			public List<Child> Children = new List<Child>();
		}

		[MapField("ParentID", "Parent.ID")]
		public class Child
		{
			[MapField("ChildID"), /*[a]*/PrimaryKey/*[/a]*/]
			public int ID;

			/*[a]*/[Relation]/*[/a]*/
			public Parent Parent = new Parent();

			/*[a]*/[Relation(typeof(Grandchild))]/*[/a]*/
			public List<Grandchild> Grandchildren = new List<Grandchild>();
		}

		[MapField("ChildID", "Child.ID")]
		public class Grandchild
		{
			[MapField("GrandchildID"), /*[a]*/PrimaryKey/*[/a]*/]
			public int ID;

			/*[a]*/[Relation]/*[/a]*/
			public Child Child = new Child();
		}

		[Test]
		public void Test()
		{
			List<Parent>   parents = new List<Parent>();
			/*[a]*/MapResultSet/*[/a]*/[] sets    = new MapResultSet[3];

			sets[0] = new MapResultSet(typeof(Parent), parents);
			sets[1] = new MapResultSet(typeof(Child));
			sets[2] = new MapResultSet(typeof(Grandchild));

			sets[0].AddRelation(sets[1], "ParentID", "ParentID", "Children");
			sets[1].AddRelation(sets[0], "ParentID", "ParentID", "Parent");

			sets[1].AddRelation(sets[2], "ChildID", "ChildID", "Grandchildren");
			sets[2].AddRelation(sets[1], "ChildID", "ChildID", "Child");

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand      (TestQuery)
					./*[a]*/ExecuteResultSet/*[/a]*/(sets);
			}

			Assert.IsNotEmpty(parents);

			foreach (Parent parent in parents)
			{
				Assert.IsNotNull(parent);
				Assert.IsNotEmpty(parent.Children);

				foreach (Child child in parent.Children)
				{
					Assert.AreEqual(parent, child.Parent);
					Assert.IsNotEmpty(child.Grandchildren);

					foreach (Grandchild grandchild in child.Grandchildren)
					{
						Assert.AreEqual(child,  grandchild.Child);
						Assert.AreEqual(parent, grandchild.Child.Parent);
					}
				}
			}
		}
		
		[Test]
		public void Test2()
		{
			List<Parent> parents = new List<Parent>();
			/*[a]*/MapResultSet/*[/a]*/[] sets = new MapResultSet[3];

			sets[0] = new MapResultSet(typeof(Parent), parents);
			sets[1] = new MapResultSet(typeof(Child));
			sets[2] = new MapResultSet(typeof(Grandchild));

			using (DbManager db = new DbManager())
			{
				db
					.SetCommand(TestQuery)
					./*[a]*/ExecuteResultSet/*[/a]*/(sets);
			}

			Assert.IsNotEmpty(parents);

			foreach (Parent parent in parents)
			{
				Assert.IsNotNull(parent);
				Assert.IsNotEmpty(parent.Children);

				foreach (Child child in parent.Children)
				{
					Assert.AreEqual(parent, child.Parent);
					Assert.IsNotEmpty(child.Grandchildren);

					foreach (Grandchild grandchild in child.Grandchildren)
					{
						Assert.AreEqual(child, grandchild.Child);
						Assert.AreEqual(parent, grandchild.Child.Parent);
					}
				}
			}
		}

		public class ParentEx
		{
			public int           ID;
			public List<ChildEx> Children = new List<ChildEx>();
		}

		public class ChildEx
		{
			public int                ID;
			public ParentEx           Parent        = new ParentEx();
			public List<GrandchildEx> Grandchildren = new List<GrandchildEx>();
		}

		public class GrandchildEx
		{
			public int     ID;
			public ChildEx Child = new ChildEx();
		}

		static readonly MappingSchema _mappingSchema = new MappingSchema
		{
			Extensions = TypeExtension.GetExtensions("RelationExtension.xml")
		};

		[Test]
		public void Test3()
		{
			var parents = new List<ParentEx>();
			var sets    = new /*[a]*/MapResultSet/*[/a]*/[3];

			sets[0] = new MapResultSet(typeof(ParentEx), parents);
			sets[1] = new MapResultSet(typeof(ChildEx));
			sets[2] = new MapResultSet(typeof(GrandchildEx));

			using (var db = new DbManager())
			{
				db.MappingSchema = _mappingSchema;

				db
					.SetCommand(TestQuery)
					./*[a]*/ExecuteResultSet/*[/a]*/(sets);
			}

			Assert.IsNotEmpty(parents);

			foreach (ParentEx parent in parents)
			{
				Assert.IsNotNull(parent);
				Assert.IsNotEmpty(parent.Children);

				foreach (ChildEx child in parent.Children)
				{
					Assert.AreEqual(parent, child.Parent);
					Assert.IsNotEmpty(child.Grandchildren);

					foreach (GrandchildEx grandchild in child.Grandchildren)
					{
						Assert.AreEqual(child, grandchild.Child);
						Assert.AreEqual(parent, grandchild.Child.Parent);
					}
				}
			}
		}
	}
}
