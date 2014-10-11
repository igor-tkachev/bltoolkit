using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ScalarFieldName
	{
		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT PersonID, FirstName FROM Person")]
			[Index("PersonID")]
			[/*[a]*/ScalarFieldName("FirstName")/*[/a]*/]
			public abstract Dictionary<int, string> GetPersonNameDictionary();
		}

		[Test]
		public void Test()
		{
			TestAccessor pa = DataAccessor.CreateInstance<TestAccessor>();

			IDictionary<int, string> dic = pa.GetPersonNameDictionary();

			Assert.AreEqual("John", dic[1]);
		}
	}
}
