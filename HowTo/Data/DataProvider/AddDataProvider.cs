using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace HowTo.Data.DataProvider
{
	[TestFixture]
	public class AddDataProvider
	{
		const string connectionString = 
			"Provider=ASEOLEDB;Data Source=server;Catalog=database;User Id=user;Password=pwd;";

		[Test]
		public void Test()
		{
			// 3rd party data provider registration.
			//
			DbManager./*[a]*/AddDataProvider/*[/a]*/(new /*[a]*/SybaseAdoDataProvider/*[/a]*/());

			// It can be configured by App.config.
			// We use this way for the demo purpose only.
			//
			DbManager.AddConnectionString(
				"SybaseAdo",       // Provider name
				"Default",         // Configuration
				connectionString); // Connection string

			using (DbManager db = new DbManager("SybaseAdo", "Default"))
			{
			}
		}
	}
}
