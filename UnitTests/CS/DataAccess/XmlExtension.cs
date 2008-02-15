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
			using (DbManager db = new DbManager())
			{
				SqlQuery          sq = new SqlQuery(db);
				sq.Extensions = TypeExtension.GetExtenstions(@"XmlExtension.xml");
				Assert.IsNotNull(sq.Extensions["Person1"]);

				Person1           ps = (Person1)sq.SelectByKey(typeof(Person1), 1);
				Assert.IsNotNull(ps);
			}
		}

		[Test]
		public void GenericsTest()
		{
			using (DbManager db = new DbManager())
			{
				SqlQuery<Person1> sq = new SqlQuery<Person1>(db);
				sq.Extensions = TypeExtension.GetExtenstions(@"XmlExtension.xml");
				Assert.IsNotNull(sq.Extensions["Person1"]);

				Person1           ps = sq.SelectByKey(1);
				Assert.IsNotNull(ps);
			}
		}
	}
}

