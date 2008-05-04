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
		public void FW2Configuration()
		{
			// <connectionString> section configuration supported in FW 2.0+.
			//
			using (DbManager db = new DbManager(/*[a]*/"DemoConnection"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void DefaultConfiguration()
		{
			// Default configuration and default data provider.
			//
			using (DbManager db = new DbManager/*[a]*/()/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void DevelopmentConfiguration()
		{
			// Development configuration and default data provider.
			//
			using (DbManager db = new DbManager(/*[a]*/"Development"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void ProductionConfiguration()
		{
			// Production configuration and default data provider.
			//
			using (DbManager db = new DbManager(/*[a]*/"Production"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDefaultConfiguration()
		{
			// Default configuration and OleDb data provider.
			//
			using (DbManager db = new DbManager(/*[a]*/"OleDb"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbDevelopmentConfiguration()
		{
			// Development configuration and OleDb data provider.
			//
			using (DbManager db = new DbManager(/*[a]*/"OleDb"/*[/a]*/, /*[a]*/"Development"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}

		[Test]
		public void OleDbProductionConfiguration()
		{
			// Production configuration and OleDb data provider.
			//
			using (DbManager db = new DbManager(/*[a]*/"OleDb"/*[/a]*/, /*[a]*/"Production"/*[/a]*/))
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
