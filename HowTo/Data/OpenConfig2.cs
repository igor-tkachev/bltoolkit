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
			/*[b]*/DbManager.AddConnectionString(
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI")/*[/b]*/;

			using (DbManager db = /*[b]*/new DbManager()/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void NoConfigConfiguration2()
		{
			/*[b]*/DbManager.AddConnectionString(
				"NewConfig",
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI")/*[/b]*/;

			using (DbManager db = /*[b]*/new DbManager("NewConfig")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
