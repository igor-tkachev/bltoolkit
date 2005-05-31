/// example:
/// map ToObject(object,object)
/// comment:
/// The following example demonstrates how to use the method.
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_Map
{
	[TestFixture]
	public class ToObject_object_object
	{
		public class BizEntity
		{
			public int    ID;
			public string Name;
		}

		[Test]
		public void Test()
		{
			BizEntity entity = new BizEntity();

			entity.ID   = 1;
			entity.Name = "Example";

			DataTable table = new DataTable();

			table.Columns.Add("ID",   typeof(int));
			table.Columns.Add("Name", typeof(string));

			Map.ToObject(entity, table);

			Assert.AreEqual(entity.ID,   table.Rows[0]["ID"]);
			Assert.AreEqual(entity.Name, table.Rows[0]["Name"]);
		}
	}
}
