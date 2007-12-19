using System;
using System.Collections.Generic;
using System.Data;
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
#if ORACLE
			[SqlQuery("SELECT * FROM Person WHERE LastName = :lastName")]
#else
			[SqlQuery("SELECT * FROM Person WHERE LastName = @lastName")]
#endif
			public abstract List<Person> SelectByLastName(string lastName);

#if ORACLE
			[SqlQuery("SELECT * FROM Person WHERE {0} = :value")]
#else
			[SqlQuery("SELECT * FROM Person WHERE {0} = @value")]
#endif
			public abstract List<Person> SelectBy([Format] string fieldName, string value);

#if ORACLE
			[SqlQuery("SELECT * FROM Person WHERE LastName = :lastName AND rownum <= {0}")]
#elif FIREBIRD
			[SqlQuery("SELECT FIRST {0} * FROM Person WHERE LastName = @lastName")]
#else
			[SqlQuery("SELECT TOP {0} * FROM Person WHERE LastName = @lastName")]
#endif
			public abstract List<Person> SelectByLastName(string lastName, [Format(0)] int top);

#if ORACLE
			[SqlQuery("SELECT :id ID FROM Dual")]
#elif FIREBIRD
			[SqlQuery("SELECT CAST(@id AS INTEGER) ID FROM Dual")]
#else
			[SqlQuery("SELECT @id as ID")]
#endif
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
