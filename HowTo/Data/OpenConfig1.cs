using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig1
	{
		[Test]
		public void DefaultConfiguration()
		{
			// Default configuration and default data provider.
			//
			using (DbManager db = /*[a]*/new DbManager()/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void DevelopmentConfiguration()
		{
			// Development configuration and default data provider.
			//
			using (DbManager db = /*[a]*/new DbManager("Development")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void ProductionConfiguration()
		{
			// Production configuration and default data provider.
			//
			using (DbManager db = /*[a]*/new DbManager("Production")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDefaultConfiguration()
		{
			// Default configuration and OleDb data provider.
			//
			using (DbManager db = /*[a]*/new DbManager("OleDb")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDevelopmentConfiguration()
		{
			// Development configuration and OleDb data provider.
			//
			using (DbManager db = /*[a]*/new DbManager("OleDb", "Development")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbProductionConfiguration()
		{
			// Production configuration and OleDb data provider.
			//
			using (DbManager db = /*[a]*/new DbManager("OleDb", "Production")/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
