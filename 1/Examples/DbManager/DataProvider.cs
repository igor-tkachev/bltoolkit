/// example:
/// db DataProvider
/// comment:
/// The following example demonstrates how to use the property.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class DataProvider
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Assert.AreEqual("Sql", db.DataProvider.Name);
			}
		}
	}
}
