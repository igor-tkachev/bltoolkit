using System;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Data.DataProvider;

using NUnit.Framework;

namespace Data.Linq.Exceptions
{
	using Model;

	[TestFixture]
	public class DataExceptionTest
	{
		[Test]
		public void ParameterPrefixTest()
		{
			try
			{
				using (var db = new DbManager(new MySqlDataProvider(), "Server=DBHost;Port=3306;Database=nodatabase;Uid=bltoolkit;Pwd=TestPassword;"))
				{
					db.GetTable<Person>().ToList();
				}
			}
			catch (DataException ex)
			{
				var number = ex.Number;
			}
		}
	}
}
