using System;
using System.Transactions;

using BLToolkit.Data.DataProvider;

using NUnit.Framework;

namespace Data.Linq.ProviderSpecific
{
	using Model;

	[TestFixture]
	public class MySqlTest : TestBase
	{
		[Test]
		public void ParameterPrefixTest()
		{
			var oldPrefix = MySqlDataProvider.SprocParameterPrefix;
			MySqlDataProvider.SprocParameterPrefix = "_";

			try
			{
				using (var db = new TestDbManager(ProviderName.MySql))
				{
					var person = db.SetSpCommand("GetPersonById", db.Parameter("ID", 1)).ExecuteObject<Person>();
					Assert.IsNotNull(person);

					var person2 = db.SetSpCommand("GetPersonByName", db.CreateParameters(person)).ExecuteObject<Person>();
					Assert.IsNotNull(person2);

					Assert.AreEqual(person.LastName, person2.LastName);

					person = db.SetCommand(
							"SELECT * FROM Person WHERE PersonID = ?ID", 
							db.Parameter("?ID", 1))
						.ExecuteObject<Person>();

					Assert.IsNotNull(person);
					Assert.AreEqual(1, person.ID);

					person2 = db.SetCommand(
							"SELECT * FROM Person WHERE FirstName = ?firstName AND LastName = ?lastName", 
							db.CreateParameters(person))
						.ExecuteObject<Person>();

					Assert.IsNotNull(person2);
					Assert.AreEqual(person.LastName, person2.LastName);
				}
			}
			finally
			{
				MySqlDataProvider.SprocParameterPrefix = oldPrefix;
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
