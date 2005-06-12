/// example:
/// db ctor(IDbTransaction)
/// comment:
/// The following example creates the <see cref="System.Data.SqlClient.SqlConnection"/>, opens it, 
/// and creates the <see cref="DbManager"/>.
using System;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class ctor_IDbTransaction
	{
		[Test]
		public void Test()
		{
			string connectionString =
				"Server=(local);Database=Northwind;Integrated Security=SSPI";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();

				SqlTransaction tran = con.BeginTransaction();
                
				using (DbManager db = new DbManager(tran))
				{
					Assert.AreEqual(ConnectionState.Open, db.Connection.State);
				}

				tran.Commit();
			}
		}
	}
} 
