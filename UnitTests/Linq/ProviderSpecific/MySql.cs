using System;
using System.Transactions;

using BLToolkit.Data.DataProvider;

namespace Data.ProviderSpecific
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
