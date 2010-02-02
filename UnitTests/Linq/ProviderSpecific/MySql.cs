using System;
using System.Transactions;

using BLToolkit.Data.DataProvider;
using Data.Linq.Model;
using NUnit.Framework;

namespace Data.ProviderSpecific
{
	using Linq;

	[TestFixture]
	public class MySqlTest : TestBase
	{
		[Test]
		public void ParameterPrefixTest()
		{
			var oldPrefix = MySqlDataProvider.ParameterPrefix;
			MySqlDataProvider.ParameterPrefix = "_";

			try
			{
				using (var db = new TestDbManager(ProviderName.MySql))
				{
					var person = db.SetSpCommand("GetPersonById", db.Parameter("?ID", 1)).ExecuteObject<Person>();
					Assert.IsNotNull(person);

					var person2 = db.SetSpCommand("GetPersonByName", db.CreateParameters(person)).ExecuteObject<Person>();
					Assert.IsNotNull(person2);
					Assert.AreEqual(person.LastName, person2.LastName);
				}
			}
			finally
			{
				MySqlDataProvider.ParameterPrefix = oldPrefix;
			}
		}

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
