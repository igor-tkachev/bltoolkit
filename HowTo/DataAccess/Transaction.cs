using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Transaction
	{
		public abstract class TestAccessor : DataAccessor<Person>
		{
			public abstract int  Insert(Person person);
			public abstract void Delete(int @PersonID);

			public abstract Person SelectByKey(int id);
			public abstract Person SelectByKey(/*[a]*/DbManager/*[/a]*/ db, int id);
		}

		// /*[i]*/DataAccessor/*[/i]*/ takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test1()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>/*[a]*/(db)/*[/a]*/;

				ta./*[a]*/BeginTransaction/*[/a]*/();

				int id = ta.Insert(new Person { FirstName = "John", LastName = "Smith" });
				Assert.AreNotEqual(0, id);

				Person person = ta.SelectByKey(id);
				Assert.IsNotNull(person);

				ta.Delete(id);

				ta./*[a]*/CommitTransaction/*[/a]*/();
			}
		}

		// /*[i]*/DataAccessor/*[/i]*/ method takes /*[i]*/DbManager/*[/i]*/ as a parameter.
		//
		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>/*[a]*/()/*[/a]*/;

				int id = ta.Insert(new Person { FirstName = "John", LastName = "Smith" });
				Assert.AreNotEqual(0, id);

				Person person = ta.SelectByKey(/*[a]*/db/*[/a]*/, id);
				Assert.IsNotNull(person);

				ta.Delete(id);

				db.CommitTransaction();
			}
		}
	}
}

