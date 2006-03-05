using System;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class OpenConfig3
	{
		[Test]
		public void DbConnectionConfiguration()
		{
			string connectionString =
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI";

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
			string connectionString =
				"Server=.;Database=BLToolkitData;Integrated Security=SSPI";

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
	}
}
