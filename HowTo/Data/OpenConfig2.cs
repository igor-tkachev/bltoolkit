using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig2
	{
		[Test]
		public void NoConfigConfiguration1()
		{
			/*[a]*/DbManager.AddConnectionString(
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI")/*[/a]*/;

			using (DbManager db = /*[a]*/new DbManager()/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void NoConfigConfiguration2()
		{
			/*[a]*/DbManager.AddConnectionString(
				"NewConfig",
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI")/*[/a]*/;

			using (DbManager db = /*[a]*/new DbManager("NewConfig")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
