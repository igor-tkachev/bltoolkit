/// example:
/// db ctor
/// comment:
/// <para>
/// The following example creates the <see cref="DbManager"/>
/// and opens a database connection associated with the default configuration.
/// </para>
/// <para>
/// App.config:
/// </para>
/// <code>
/// &lt;configuration&gt;
///     &lt;appSettings&gt;
///         &lt;add 
///             key   = "ConnectionString" 
///             va<i/>lue = "Server=.;Database=NorthwindDev;Integrated Security=SSPI" /&gt;
///     &lt;/appSettings&gt;
/// &lt;configuration&gt;
/// </code>
/// <para>
/// Test.cs:
/// </para>
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class ctor
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}