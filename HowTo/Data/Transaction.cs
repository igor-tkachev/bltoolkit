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
				db./*[b]*/BeginTransaction()/*[/b]*/;

				// ...

				db./*[b]*/CommitTransaction()/*[/b]*/;
			}
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction(/*[b]*/IsolationLevel.ReadCommitted/*[/b]*/);

				// ...

				db.CommitTransaction();
			}
		}
	}
}
