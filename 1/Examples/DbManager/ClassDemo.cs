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
	public class ClassDemo
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				string name = (string)db
					.SetCommand(@"
						SELECT
							CategoryName
						FROM 
							Categories
						WHERE
							CategoryID = @id",
						db.Parameter("@id", 1))
					.ExecuteScalar();

				Console.WriteLine(name);
			}
		}
	}
}