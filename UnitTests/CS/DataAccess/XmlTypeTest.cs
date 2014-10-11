using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace DataAccess
{
	[TestFixture]
	public class XmlTypeTest
	{
		public class TestObject
		{
			[MapField("DataTypeID")]
			public int       ID;

			[MapField("Xml_")]
			public string    Xml;
		}

		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("Select * FROM DataTypeTest")]
			public abstract List<TestObject> LoadAll();

			[TestQuery(
				SqlText    = "SELECT * FROM DataTypeTest WHERE DataTypeID=@ID",
				OracleText = "SELECT * FROM DataTypeTest WHERE DataTypeID=:ID")]
			public abstract TestObject       LoadById(int ID); 

		}

		[Test]
		public void Test()
		{
			TestAccessor ta = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));
			TestObject   o  = ta.LoadById(2);

			Assert.IsNotNull(o);
			Assert.IsNotNull(o.Xml);

			List<TestObject> lst = ta.LoadAll();
			Assert.IsNotEmpty(lst);
			Assert.IsNotNull(lst[0].Xml);
		}
	}
}
