using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DataSetTable
	{
		public abstract class TestAcessor : DataAccessor
		{
			[SprocName("Person_SelectAll"), /*[a]*/DataSetTable/*[/a]*/("First")]
			public abstract void SelectFirstTable  ([Destination] DataSet ds);

			[SprocName("Person_SelectAll"), /*[a]*/DataSetTable/*[/a]*/("Second")]
			public abstract void SelectSecondTable ([Destination] DataSet ds);

			[SprocName("Person_SelectAll"), /*[a]*/DataSetTable/*[/a]*/(0)]
			public abstract void SelectFirstTable2 ([Destination] DataSet ds);

			[SprocName("Person_SelectAll"), /*[a]*/DataSetTable/*[/a]*/(1)]
			public abstract void SelectSecondTable2([Destination] DataSet ds);
		}

		[Test]
		public void Test()
		{
			TestAcessor ta = DataAccessor.CreateInstance<TestAcessor>();

			DataSet ds = new DataSet();

			ta.SelectFirstTable  (ds);
			ta.SelectSecondTable (ds);
			ta.SelectFirstTable2 (ds);
			ta.SelectSecondTable2(ds);

			Assert.IsTrue (ds.Tables.Contains("First"),  "Table 'First'  not found");
			Assert.IsTrue (ds.Tables.Contains("Second"), "Table 'Second' not found");
			Assert.IsFalse(ds.Tables.Contains("Table"),  "Table 'Table'  was found");
		}
	}
}
