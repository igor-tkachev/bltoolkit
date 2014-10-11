using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteDataTable
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				DataTable dt = db
					.SetCommand("SELECT * FROM Person")
					./*[a]*/ExecuteDataTable/*[/a]*/();

				Assert.AreNotEqual(0, dt.Rows.Count);
			}
		}
	}
}
