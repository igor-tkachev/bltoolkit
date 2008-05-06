using System;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig3
	{
		const string connectionString =
			"Server=.;Database=BLToolkitData;Integrated Security=SSPI";

		[Test]
		public void DbConnectionConfiguration()
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				using (DbManager db = /*[a]*/new DbManager(con)/*[/a]*/)
				{
					Assert.AreEqual(ConnectionState.Open, db.Connection.State);
				}
			}
		}

		[Test]
		public void DbTransactionConfiguration()
		{
			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				SqlTransaction tran = con.BeginTransaction();

				using (DbManager db = /*[a]*/new DbManager(tran)/*[/a]*/)
				{
					Assert.AreEqual(ConnectionState.Open, db.Connection.State);
				}

				tran.Commit();
			}
		}

		[Test]
		public void DataProviderConfiguration()
		{
			SqlDataProvider dp = new SqlDataProvider();

			using (DbManager db = /*[a]*/new DbManager(dp, connectionString)/*[/a]*/)
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
