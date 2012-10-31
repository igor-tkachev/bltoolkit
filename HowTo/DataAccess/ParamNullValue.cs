using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ParamNullValue
	{
		public abstract class TestAccessor : DataAccessor
		{
			public abstract Person SelectByKey([/*[a]*/ParamNullValue/*[/a]*/(1)] int id);
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();

			// Parameter id == 1 will be replaced with NULL
			//
			Person p1 = ta.SelectByKey(1);
			Assert.IsNull(p1);

			// Parameter id == 2 will be send as is
			//
			Person p2 = ta.SelectByKey(2);
			Assert.IsNotNull(p2);
		}
	}
}
