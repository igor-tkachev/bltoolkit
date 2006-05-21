using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection.Extension;
using BLToolkit.Data;

namespace DataAccess
{
	[TestFixture]
	public class XmlExtension
	{
		public class Person1
		{
			[MapField("PersonID")]  public int    ID;
			[MapField("FirstName")] public string Name;
		}

		[Test]
		public void Test()
		{
			Map.Extensions = TypeExtension.GetExtenstions(@"XmlExtension.xml");

			using (DbManager db = new DbManager())
			{
				DataAccessor da = new DataAccessor(db);
				Person1       ps = da.SelectByKeySql<Person1>(1);

				Assert.IsNotNull(ps);
			}
		}
	}
}

