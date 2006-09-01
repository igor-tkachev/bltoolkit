using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace DataAccess
{
	[TestFixture]
	public class SqlQueryTest2
	{
		public class Person
		{
			public int    ID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public abstract class PersonAccessor : DataAccessor<Person>
		{
			[SqlQuery("SELECT * FROM Person WHERE LastName = @lastName")]
			public abstract List<Person> SelectByLastName(string lastName);

			[SqlQuery("SELECT * FROM Person WHERE {0} = @value")]
			public abstract List<Person> SelectBy([Format] string fieldName, string value);

			[SqlQuery("SELECT TOP {0} * FROM Person WHERE LastName = @lastName")]
			public abstract List<Person> SelectByLastName(string lastName, [Format(0)] int top);

			[SqlQuery("SELECT @id as ID")]
			public abstract List<Person> SelectID(int @id);
		}

		[Test]
		public void Test1()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectByLastName("Pupkin");

			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Test2()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectBy("FirstName", "John");

			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Test3()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectByLastName("Pupkin", 1);

			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Test4()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectID(42);

			Assert.AreEqual(42, list[0].ID);
		}
	}
}
