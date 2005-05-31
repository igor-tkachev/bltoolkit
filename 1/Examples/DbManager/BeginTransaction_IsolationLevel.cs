/// example:
/// db BeginTransaction
/// comment:
/// The following example creates an instance of the <see cref="DbManager"/>, 
/// calls the <see cref="BeginTransaction"/> method, and commit the transaction.
using System;
using System.Data;

using NUnit.Framework;

using Rsdn.Framework.Data;

namespace Examples_DbManager
{
	[TestFixture]
	public class BeginTransaction_IsolationLevel
	{
		[Test]
		public void Test()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction(IsolationLevel.ReadCommitted);
                
				// ...
                
				db.Transaction.Commit();
			}
		}
	}
}
