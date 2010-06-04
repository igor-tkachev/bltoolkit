using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig1FW2
	{
		[Test]
		public void FW2Configuration()
		{
			// <connectionString> section configuration supported in FW 2.0+.
			//
			using (DbManager db = new DbManager(/*[a]*/"DemoConnection"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
