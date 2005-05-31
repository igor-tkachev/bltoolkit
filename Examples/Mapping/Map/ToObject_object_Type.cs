/// example:
/// map ToObject(object,Type)
/// comment:
/// The following example demonstrates how to use the method.
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace Examples_Mapping_Map
{
	[TestFixture]
	public class ToObject_object_Type
	{
		public class BizEntity
		{
			public int    ID;
			public string Name;
		}

		[Test]
		public void Test()
		{
			DataTable table = new DataTable();

			table.Columns.Add("ID",   typeof(int));
			table.Columns.Add("Name", typeof(string));

			table.Rows.Add(new object[] { 1, "Example" });

			BizEntity entity = 
				(BizEntity)Map.ToObject(table, typeof(BizEntity));

			Assert.AreEqual(table.Rows[0]["ID"],   entity.ID);
			Assert.AreEqual(table.Rows[0]["Name"], entity.Name);
		}
	}
}
