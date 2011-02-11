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
			var db = new DbManager();

			/*[a]*/try/*[/a]*/
			{
				// ...
			}
			/*[a]*/finally/*[/a]*/
			{
				if (db != null)
					db./*[a]*/Close/*[/a]*/();
			}
		}

		// Consider using the C# [b][i]using[/i][/b] statement instead.
		//
		[Test]
		public void Test2()
		{
			/*[a]*/using (var db = new DbManager())/*[/a]*/
			{
				// ...
			}
		}
	}
}
