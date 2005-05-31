/// example:
/// db AddConnectionString(string)
/// comment:
/// The following example uses the <b>AddConnectionString</b> method to add a new database connection string. 
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class AddConnectionString_string
	{
		[Test]
		public void Test()
		{
			DbManager.AddConnectionString(
				"Server=.;Database=Northwind;Integrated Security=SSPI");
            
			using (DbManager db = new DbManager())
			{
				// ...
			}
		}
	}
}

