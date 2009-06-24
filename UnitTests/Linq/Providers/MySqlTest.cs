using System;
using System.Linq;
using System.Transactions;

using NUnit.Framework;

using BLToolkit.Data.DataProvider;

namespace Data.Providers
{
	using Linq;

	//[TestFixture]
	public class MySqlTest : TestBase
	{
		//[Test]
		public void TranScopeSproc()
		{
			using (new TransactionScope())
			{
				using (var db = new TestDbManager(ProviderName.MySql))
				{
					db.SetSpCommand("Person_SelectAll", false);
				}
			}
		}
	}
}
