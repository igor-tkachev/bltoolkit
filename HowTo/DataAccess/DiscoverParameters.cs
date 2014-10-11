using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class DiscoverParameters
	{
		public abstract class PersonAccessor : DataAccessor
		{
			[/*[a]*/DiscoverParameters/*[/a]*/]
			public abstract Person SelectByName(string anyParameterName, string otherParameterName);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();
			Person         p  = pa.SelectByName("Tester", "Testerson");

			Assert.AreEqual(2, p.ID);
		}
	}
}
