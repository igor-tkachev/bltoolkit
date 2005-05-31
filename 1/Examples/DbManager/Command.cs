/// example:
/// db Command
/// comment:
/// The following example demonstrates how to use the property.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class Command
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				Console.WriteLine(db.Command.CommandTimeout);
			}
		}
	}
}
