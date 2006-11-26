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
#if FW2
				SqlQuery<Person1> sq = new SqlQuery<Person1>(db);
				Person1           ps = sq.SelectByKey(1);
#else
				SqlQuery          sq = new SqlQuery(db);
				Person1           ps = (Person1)sq.SelectByKey(typeof(Person1), 1);
#endif

				Assert.IsNotNull(ps);
			}
		}
	}
}

