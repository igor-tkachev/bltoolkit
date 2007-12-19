using System;
using System.Data;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;

using NUnit.Framework;

namespace Data
{
	[TestFixture]
	public class OutputParameter
	{
		[Test]
		public void DirectTest()
		{
			using (DbManager db = new DbManager())
			{
				string paramName = (string) db.DataProvider.Convert("name",      ConvertType.NameToQueryParameter);
				string fieldName = (string) db.DataProvider.Convert("FirstName", ConvertType.NameToQueryField);

				db.SetCommand(string.Format("SELECT {0} = {1} FROM Person WHERE PersonID = 1", paramName, fieldName)
						, db.OutputParameter(paramName, DbType.String, 50))
					.ExecuteNonQuery();
				Assert.AreEqual("John", db.Parameter(paramName).Value);
			}
		}

		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT {0} = {1} FROM Person WHERE PersonID = 1")]
			public abstract void SelectJohn([ParamSize(50), ParamDbType(DbType.String)] out string name, [Format] string paramName, [Format] string FieldName);
		}

		[Test]
		public void AccessorTest()
		{
			using (DbManager db = new DbManager())
			{
				string paramName = (string)db.DataProvider.Convert("name",      ConvertType.NameToQueryParameter);
				string fieldName = (string)db.DataProvider.Convert("FirstName", ConvertType.NameToQueryField);

				TestAccessor ta = (TestAccessor)TestAccessor.CreateInstance(typeof(TestAccessor), db);

				string actual;
				ta.SelectJohn(out actual, paramName, fieldName);

				Assert.AreEqual("John", actual);
			}
		}
	}
}
