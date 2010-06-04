using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class OpenConfig
	{
		public class Person
		{
			[MapField("PersonID"), PrimaryKey, NonUpdatable]
			public int    ID;

			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public abstract class TestAccessor : DataAccessor
		{
			public abstract Person SelectByKey(int id);
			public abstract Person SelectByKey(/*[a]*/DbManager/*[/a]*/ db, int id);
		}

		// /*[i]*/DbManager/*[/i]*/ is created by /*[i]*/DataAccessor/*[/i]*/.
		//
		[Test]
		public void Test1()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>/*[a]*/()/*[/a]*/;

			Person person = ta.SelectByKey(1);

			Assert.IsNotNull(person);
		}

		// /*[i]*/DataAccessor/*[/i]*/ takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>/*[a]*/(db)/*[/a]*/;

				Person person = ta.SelectByKey(1);

				Assert.IsNotNull(person);
			}
		}

		// /*[i]*/DataAccessor/*[/i]*/ method takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test3()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>/*[a]*/()/*[/a]*/;

				Person person = ta.SelectByKey(/*[a]*/db/*[/a]*/, 1);

				Assert.IsNotNull(person);
			}
		}
	}
}

