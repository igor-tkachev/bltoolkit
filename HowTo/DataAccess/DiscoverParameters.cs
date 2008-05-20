using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DiscoverParameters
	{
		public abstract class PersonAcessor : DataAccessor
		{
			[/*[a]*/DiscoverParameters/*[/a]*/]
			public abstract Person SelectByName(string anyParameterName, string otherParameterName);
		}

		[Test]
		public void Test()
		{
			PersonAcessor pa = DataAccessor.CreateInstance<PersonAcessor>();
			Person        p  = pa.SelectByName("John", "Pupkin");

			Assert.AreEqual(1, p.ID);
		}
	}
}
