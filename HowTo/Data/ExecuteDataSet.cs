using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteDataSet
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				DataSet ds = db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteDataSet/*[/a]*/();

				Assert.AreNotEqual(0, ds.Tables[0].Rows.Count);
			}
		}
	}
}
