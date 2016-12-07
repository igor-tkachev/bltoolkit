using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.EditableObjects;
using BLToolkit.Mapping;
using BLToolkit.Reflection.MetadataProvider;
using BLToolkit.Reflection;

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
			[MapField("ParentId"), PrimaryKey]
			public abstract int         Id       { get; set; }
			[Relation(typeof(Child))]
			public abstract List<Child> Children { get; set; }
		}

		[MapField("ParentId", "Parent.Id")]
		public abstract class Child : EditableObject
		{
			[MapField("ChildId"), PrimaryKey]
			public abstract int    Id     { get; set; }
			[Relation]
			public abstract Parent Parent { get; set; }
		}

#if !FIREBIRD
		[Test]
#endif
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

		[Test]
		public void RelationAttributeTest1()
		{
			bool isSet;
			List<MapRelationBase> relations
				 = Map.DefaultSchema.MetadataProvider.GetRelations(Map.DefaultSchema, 
							Map.DefaultSchema.Extensions, typeof(Parent), typeof(Child), out isSet);

			Assert.That(isSet);
			Assert.That(relations.Count == 1);

			Assert.That(relations[0].ContainerName == "Children");
			Assert.That(relations[0].SlaveIndex.Fields[0].Name == "ParentId");

			relations
				 = Map.DefaultSchema.MetadataProvider.GetRelations(Map.DefaultSchema, 
							Map.DefaultSchema.Extensions, typeof(Child), typeof(Parent), out isSet);

			Assert.That(isSet);
			Assert.That(relations.Count == 1);

			Assert.That(relations[0].ContainerName == "Parent");
			Assert.That(relations[0].SlaveIndex.Fields[0].Name == "ParentId");
		}
	
		public abstract class Master
		{
			[PrimaryKey, Nullable]
			public abstract int          MasterId {get; set;}
			[Relation(typeof(Detail))]
			public abstract List<Detail> Details  {get; set;}
			public abstract string       Name     {get; set;}
		}

		[MapField("MasterId", "Master.MasterId")]
		public abstract class Detail
		{
			[PrimaryKey, MapField("Id"), Nullable]
			public abstract int             DetailId   {get; set;}

			[Relation]
			public abstract Master          Master     {get; set;}

			[Relation(typeof(SubDetail), "DetailId", "Id")]
			public abstract List<SubDetail> SubDetails {get; set;}
		}

		[MapField("DetailId", "Master.DetailId")]
		public abstract class SubDetail
		{
			[PrimaryKey]
			public abstract int             SubDetailId {get; set;}

			[Relation("Id", "DetailId"), Nullable]
			public abstract Detail          Master      {get; set;}
		}

		[Test]
		public void RelationAttributeTest2()
		{
			MappingSchema        ms = Map.DefaultSchema;
			MetadataProviderBase mp = ms.MetadataProvider;
			bool                 isSet;

			List<MapRelationBase> relations = mp.GetRelations(ms, ms.Extensions, typeof(Master), typeof(Detail), out isSet);
			
			//sets[0] = new MapResultSet(typeof(Master),    masters);
			//sets[1] = new MapResultSet(typeof(Detail),    details);
			//sets[2] = new MapResultSet(typeof(SubDetail), subdetails);

			//sets[0].AddRelation(sets[1], "MasterId", "MasterId", "Details");
			//sets[1].AddRelation(sets[0], "MasterId", "MasterId", "Master");
			//sets[1].AddRelation(sets[2], "DetailId",       "Id", "SubDetails");
			//sets[2].AddRelation(sets[1], "Id",       "DetailId", "Master");


			Assert.That(isSet);
			Assert.That(relations.Count == 1);
			Assert.AreEqual("MasterId", relations[0].MasterIndex.Fields[0].Name);
			Assert.AreEqual("MasterId", relations[0].SlaveIndex.Fields[0].Name);
			Assert.AreEqual("Details",  relations[0].ContainerName);

			relations = mp.GetRelations(ms, ms.Extensions, typeof(Detail), typeof(Master), out isSet);
			
			Assert.That(isSet);
			Assert.That(relations.Count == 1);
			Assert.AreEqual("MasterId", relations[0].MasterIndex.Fields[0].Name);
			Assert.AreEqual("MasterId", relations[0].SlaveIndex.Fields[0].Name);
			Assert.AreEqual("Master",   relations[0].ContainerName);

			relations = mp.GetRelations(ms, ms.Extensions, typeof(Detail), typeof(SubDetail), out isSet);
			
			Assert.That(isSet);
			Assert.That(relations.Count == 1);
			Assert.AreEqual("Id",         relations[0].MasterIndex.Fields[0].Name);
			Assert.AreEqual("DetailId",   relations[0].SlaveIndex.Fields[0].Name);
			Assert.AreEqual("SubDetails", relations[0].ContainerName );

			relations = mp.GetRelations(ms, ms.Extensions, typeof(SubDetail), typeof(Detail), out isSet);
			
			Assert.That(isSet);
			Assert.That(relations.Count == 1);
			Assert.AreEqual("DetailId", relations[0].MasterIndex.Fields[0].Name);
			Assert.AreEqual("Id",       relations[0].SlaveIndex.Fields[0].Name);
			Assert.AreEqual("Master",   relations[0].ContainerName);
		}
		
		[Test]
		public void RelationAttributeTest3()
		{
			MappingSchema        ms = Map.DefaultSchema;
			MetadataProviderBase mp = ms.MetadataProvider;
			bool                 isSet;

			List<MapRelationBase> relations = mp.GetRelations(ms, ms.Extensions, typeof(Detail), null, out isSet);

			Assert.That(relations.Count == 2);

		}

		[Test]
		public void NullKeyTest()
		{
			Master m = TypeAccessor.CreateInstance<Master>();
			Detail d = TypeAccessor.CreateInstance<Detail>();
			
			List<Master> masters = new List<Master>();
			List<Detail> details = new List<Detail>();

			masters.Add(m);
			details.Add(d);

			Map.ResultSets(new MapResultSet[] { new MapResultSet(typeof(Master), masters), 
												new MapResultSet(typeof(Detail), details) });

			Assert.IsFalse (object.ReferenceEquals(d.Master, m));
			Assert.AreEqual(0, m.Details.Count);

			m.MasterId = 1;
			d.DetailId = 1;
			d.Master.MasterId = 1;

			Map.ResultSets(new MapResultSet[] { new MapResultSet(typeof(Master), masters), 
												new MapResultSet(typeof(Detail), details) });

			
			Assert.IsTrue  (object.ReferenceEquals(d.Master, m));
			Assert.AreEqual(1, m.Details.Count);
		}
	}
}
