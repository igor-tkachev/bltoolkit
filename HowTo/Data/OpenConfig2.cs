using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig2
	{
		const string sqlConnectionString =
			"Server=.;Database=BLToolkitData;Integrated Security=SSPI";
		const string oleDbConnectionString =
			"Provider=SQLOLEDB;Data Source=.;Integrated Security=SSPI;Initial Catalog=BLToolkitData";

		[Test]
		public void Test1()
		{
			string defaultConfiguration = DbManager.DefaultConfiguration;
			DbManager.DefaultConfiguration = ""; //to reset possible previous changes

			try
			{
				/*[a]*/DbManager.AddConnectionString/*[/a]*/(
					sqlConnectionString);   // connection string

				using (DbManager db = /*[a]*/new DbManager()/*[/a]*/)
				{
					Assert.AreEqual(ConnectionState.Open, db.Connection.State);
				}
			}
			finally
			{
				DbManager.DefaultConfiguration = defaultConfiguration; // to restore previous settings
			}

		}

		[Test]
		public void Test2()
		{
			/*[a]*/DbManager.AddConnectionString/*[/a]*/(
				"NewConfig",            // configuration string
				sqlConnectionString);   // connection string

			using (DbManager db = /*[a]*/new DbManager("NewConfig")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void Test3()
		{
			/*[a]*/DbManager.AddConnectionString/*[/a]*/(
				"OleDb",                // provider name
				"NewConfig",            // configuration string
				oleDbConnectionString); // connection string

			using (DbManager db = /*[a]*/new DbManager("OleDb", "NewConfig")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
