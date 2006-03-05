using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class Transaction
	{
		[Test]
		public void Test1()
		{
			using (DbManager db = new DbManager())
			{
				db./*[a]*/BeginTransaction()/*[/a]*/;

				// ...

				db./*[a]*/CommitTransaction()/*[/a]*/;
			}
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction(/*[a]*/IsolationLevel.ReadCommitted/*[/a]*/);

				// ...

				db.CommitTransaction();
			}
		}
	}
}
