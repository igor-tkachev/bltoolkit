using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class SqlQueryTest
	{
		public class Person
		{
			public int    ID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		[ObjectType(typeof(Person))]
		public abstract class PersonAccessor : DataAccessor
		{
			[SqlQuery("SELECT * FROM Person WHERE LastName = @lastName")]
			public abstract ArrayList SelectByLastName(string lastName);

			[SqlQuery("SELECT * FROM Person WHERE {0} = @value")]
			public abstract ArrayList SelectBy([Format] string fieldName, string value);

			[SqlQuery("SELECT TOP {0} * FROM Person WHERE LastName = @lastName")]
			public abstract ArrayList SelectByLastName(string lastName, [Format(0)] int top);
		}

		[Test]
		public void Test1()
		{
			PersonAccessor da = (PersonAccessor)DataAccessor.CreateInstance(typeof(PersonAccessor));

			ArrayList list = da.SelectByLastName("Pupkin");

			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Test2()
		{
			PersonAccessor da = (PersonAccessor)DataAccessor.CreateInstance(typeof(PersonAccessor));

			ArrayList list = da.SelectBy("FirstName", "John");

			Assert.AreNotEqual(0, list.Count);
		}

		[Test]
		public void Test3()
		{
			PersonAccessor da = (PersonAccessor)DataAccessor.CreateInstance(typeof(PersonAccessor));

			ArrayList list = da.SelectByLastName("Pupkin", 1);

			Assert.AreNotEqual(0, list.Count);
		}
	}
}
