using System;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.DataAccess;
using BLToolkit.Data;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class CreateEntityTest
	{
		public abstract class Entity
		{
			public abstract int    Id   { get; set; }
			public abstract String Name { get; set; }

			public static Entity CreateInstance() { return TypeAccessor<Entity>.CreateInstanceEx(); }
		}

		public abstract class EntityAccessor : DataAccessor<Entity>
		{
#if SQLITE || SQLCE
			[SqlQuery("SELECT * FROM Person")]
#else
			[SprocName("Person_SelectAll")]
#endif
			public abstract int Insert(Entity entity);

			public static EntityAccessor CreateInstance(DbManager dbManager)
			{
				return DataAccessor.CreateInstance<EntityAccessor>(dbManager);
			}
		}

		[Test]
		public void Test()
		{
			TypeFactory.SaveTypes = true;
			TypeFactory.SetGlobalAssembly("CreateEntityTest.dll");

			DbManager db = new DbManager();

			try
			{
				Entity entity = Entity.CreateInstance();
				EntityAccessor accessor = EntityAccessor.CreateInstance(db);

				accessor.Insert(entity);
			}
			finally
			{
				db.Dispose();
				TypeFactory.SaveGlobalAssembly();
			}
		}
	}
}
