/// example:
/// db Close()
/// comment:
/// The following example creates and opens a <see cref="DbManager"/>, then closes it.
using System;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class Close
	{
		[Test]
		public void Test1()
		{
			DbManager db = new DbManager();
            
			try
			{
				// ...
			}
			finally
			{
				if (db != null)
					db.Close();
			}
		}

		// Consider using the C# <b>using</b> statement instead.
		// The following example shows use of this method.

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				// ...
			}
		}
	}
}
