/// example:
/// db DbManager
/// comment:
/// The following example demonstrates how to create and use the <b>DbManager</b> class.
using System;
using NUnit.Framework;
using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class DbManager_Demo
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				string name = (string)db.ExecuteScalar(@"
					SELECT
						CategoryName
					FROM 
						Categories
					WHERE
						CategoryID = @id",
					db.Parameter("@id", 1));

				Console.WriteLine(name);
			}
		}
	}
}