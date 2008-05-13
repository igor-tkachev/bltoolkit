using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class CommandBehaviorDemo
	{
		public abstract class TestAcessor : DataAccessor
		{
			[SprocName("Person_SelectAll"), /*[a]*/CommandBehavior/*[/a]*/(CommandBehavior.SchemaOnly)]
			public abstract IDataReader SelectAllIDataReaderSchemaOnly(DbManager db);
		}

		[Test]
		public void Test()
		{
			TestAcessor ta = TypeAccessor<TestAcessor>.CreateInstance();

			using (DbManager   db = ta.GetDbManager())
			using (IDataReader dr = ta.SelectAllIDataReaderSchemaOnly(db))
			{
				DataTable table = dr.GetSchemaTable();

				Assert.AreEqual("PersonID",  table.Rows[0]["ColumnName"]);
				Assert.AreEqual(typeof(int), table.Rows[0]["DataType"]);
			}
		}
	}
}
