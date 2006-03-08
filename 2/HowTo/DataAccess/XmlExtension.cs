using System;

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
			DataAccessor da = new DataAccessor();

			/*[a]*/da.Extensions = TypeExtension.GetExtenstions("XmlExtension.xml")/*[/a]*/;

			MyPersonObject person = da.SelectByKeySql<MyPersonObject>(1);

			Assert.IsNotNull(person);
		}
	}
}

