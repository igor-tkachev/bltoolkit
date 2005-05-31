/// example:
/// db AddConnectionString(string,string)
/// comment:
/// The following example uses the <b>AddConnectionString</b> method to add a new database connection string
/// for the "Test" configuration. 
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class AddConnectionString_string_string
	{
		[Test]
		public void Test()
		{
			DbManager.AddConnectionString(
				"Test", "Server=.;Database=Northwind;Integrated Security=SSPI");

			using (DbManager db = new DbManager("Test"))
			{
				// ...
			}
		}
	}
}
