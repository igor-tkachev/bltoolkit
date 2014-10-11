using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Param
	{
		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT {0} = {1} FROM Person WHERE PersonID = 1")]
			public abstract void SelectJohn(
				[/*[a]*/ParamSize/*[/a]*/(50), /*[a]*/ParamDbType/*[/a]*/(DbType.String)] out string name,
				[Format] string paramName,
				[Format] string fieldName);
		}

		[Test]
		public void AccessorTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>(db);

				string actualName;

				ta.SelectJohn(out actualName, "@name", "FirstName");

				Assert.AreEqual("John", actualName);
			}
		}
	}
}
