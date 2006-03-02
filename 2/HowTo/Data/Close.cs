using System;
using NUnit.Framework;
using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class Close
	{
		[Test]
		public void Test1()
		{
			DbManager db = new DbManager();

			/*[b]*/try/*[/b]*/
			{
				// ...
			}
			/*[b]*/finally/*[/b]*/
			{
				if (db != null)
					db./*[b]*/Close/*[/b]*/();
			}
		}

		// Consider using the C# [b][i]using[/i][/b] statement instead.
		//
		[Test]
		public void Test2()
		{
			/*[b]*/using (DbManager db = new DbManager())/*[/b]*/
			{
				// ...
			}
		}
	}
}
