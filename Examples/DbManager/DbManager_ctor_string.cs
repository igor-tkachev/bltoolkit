/// example:
/// db ctor(string)
/// comment:
/// <para>
/// The following example creates the <see cref="DbManager"/>
/// and opens a database connection associated with the "Development" configuration.
/// </para>
/// <para>
/// App.config:
/// </para>
/// <code>
/// &lt;configuration&gt;
///     &lt;appSettings&gt;
///         &lt;add 
///             key   = "ConnectionString.<b>Development</b>" 
///             va<i/>lue = "Server=.;Database=NorthwindDev;Integrated Security=SSPI" /&gt;
///         &lt;add 
///             key   = "ConnectionString.<b>Production</b>" 
///             va<i/>lue = "Server=.;Database=Northwind;Integrated Security=SSPI" /&gt;
///     &lt;/appSettings&gt;
/// &lt;configuration&gt;
/// </code>
/// <para>
/// Test.cs:
/// </para>
using System;
using NUnit.Framework;
using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class DbManager_ctor_string
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager("Development"))
			{
				// ...
			}
		}
	}
}