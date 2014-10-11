using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
/*[a]*/using BLToolkit.Reflection.Extension/*[/a]*/;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class XmlExtension
	{
		public class MyPersonObject
		{
			[MapField("PersonID")]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[Test]
		public void Test()
		{
			SqlQuery<MyPersonObject> query = new SqlQuery<MyPersonObject>();

			/*[a]*/query.Extensions = TypeExtension.GetExtensions("XmlExtension.xml")/*[/a]*/;

			MyPersonObject person = query.SelectByKey(1);

			Assert.IsNotNull(person);
		}
	}
}

