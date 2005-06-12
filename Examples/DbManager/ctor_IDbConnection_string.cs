/// example:
/// db ctor(IDbConnection,string)
/// comment:
/// <para>
/// The following example creates the <see cref="System.Data.SqlClient.SqlConnection"/>
/// and then creates the <see cref="DbManager"/> for the "Development" configuration.
/// </para>
/// <para>
/// App.config:
/// </para>
/// <code>
/// &lt;configuration&gt;
///     &lt;appSettings&gt;
///         &lt;add 
///             key   = "ConnectionString.<b>Development</b>" 
///             va<i/>lue = "Server=(local);Database=NorthwindDev;Integrated Security=SSPI" /&gt;
///         &lt;add 
///             key   = "ConnectionString.<b>Production</b>" 
///             va<i/>lue = "Server=(local);Database=Northwind;Integrated Security=SSPI" /&gt;
///     &lt;/appSettings&gt;
/// &lt;configuration&gt;
/// </code>
/// <para>
/// Test.cs:
/// </para>
using System;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class ctor_IDbConnection_string
	{
		[Test]
		public void Test()
		{
			using (SqlConnection con = new SqlConnection())
			{
				using (DbManager db = new DbManager(con, "Development"))
				{
					Assert.AreEqual(ConnectionState.Open, con.State);
				}
			}
		}
	}
} 
