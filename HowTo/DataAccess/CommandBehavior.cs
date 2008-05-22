using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class CommandBehaviorDemo
	{
		public abstract class TestAccessor : DataAccessor
		{
			[SprocName("Person_SelectAll"), /*[a]*/CommandBehavior/*[/a]*/(CommandBehavior.SchemaOnly)]
			public abstract IDataReader SelectAllIDataReaderSchemaOnly(DbManager db);
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();

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
