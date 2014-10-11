using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ParamName
	{
		public abstract class TestAccessor : DataAccessor
		{
			public abstract Person SelectByName(
				[ParamName("FirstName")] string name1,
				[ParamName("@LastName")] string name2);
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();
			Person       p  = ta.SelectByName("Tester", "Testerson");

			Assert.AreEqual(2, p.ID);
		}
	}
}
