/// example:
/// db Connection
/// comment:
/// The following example demonstrates how to use the property.
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class Connection
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Console.WriteLine(db.Connection.State);

				Assert.AreEqual(ConnectionState.Open, db.Connection.State);
			}
		}
	}
}
