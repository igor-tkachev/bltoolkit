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
			using (DbManager db = /*[b]*/new DbManager()/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void DevelopmentConfiguration()
		{
			// Development configuration and default data provider.
			//
			using (DbManager db = /*[b]*/new DbManager("Development")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void ProductionConfiguration()
		{
			// Production configuration and default data provider.
			//
			using (DbManager db = /*[b]*/new DbManager("Production")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDefaultConfiguration()
		{
			// Default configuration and OleDb data provider.
			//
			using (DbManager db = /*[b]*/new DbManager("OleDb")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDevelopmentConfiguration()
		{
			// Development configuration and OleDb data provider.
			//
			using (DbManager db = /*[b]*/new DbManager("OleDb", "Development")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbProductionConfiguration()
		{
			// Production configuration and OleDb data provider.
			//
			using (DbManager db = /*[b]*/new DbManager("OleDb", "Production")/*[/b]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
