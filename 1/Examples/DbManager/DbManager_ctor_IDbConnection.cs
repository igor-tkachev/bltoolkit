/// example:
/// db ctor(IDbConnection)
/// comment:
/// The following example creates the <see cref="System.Data.SqlClient.SqlConnection"/>, opens it, 
/// and creates the <see cref="DbManager"/>.
using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Rsdn.Framework.Data;

namespace Example
{
	class Test
	{
		static void Main()
		{
			string connectionString =
				"Server=.;Database=NorthwindDev;Integrated Security=SSPI";

			using (SqlConnection con = new SqlConnection(connectionString))
			{
				con.Open();
                
				using (DbManager db = new DbManager(con))
				{
					// ...
				}
			}
		}
	}
} 
