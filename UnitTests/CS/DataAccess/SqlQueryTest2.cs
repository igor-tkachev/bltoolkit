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
			[TestQuery(
				SqlText    = "SELECT * FROM Person WHERE LastName = @lastName",
				OracleText = "SELECT * FROM Person WHERE LastName = :lastName")]
			public abstract List<Person> SelectByLastName(string lastName);

			[TestQuery(
				SqlText    = "SELECT * FROM Person WHERE {0} = @value",
				OracleText = "SELECT * FROM Person WHERE {0} = :value")]
			public abstract List<Person> SelectBy([Format] string fieldName, string value);

			[TestQuery(
				SqlText    = "SELECT TOP ({0}) * FROM Person WHERE LastName = @lastName",
				AccessText = "SELECT TOP {0} * FROM Person WHERE LastName = @lastName",
				OracleText = "SELECT * FROM Person WHERE LastName = :lastName AND rownum <= {0}",
				FbText     = "SELECT FIRST {0} * FROM Person WHERE LastName = @lastName",
				SQLiteText = "SELECT * FROM Person WHERE LastName = @lastName LIMIT {0}")]
			public abstract List<Person> SelectByLastName(string lastName, [Format(0)] int top);

			[TestQuery(
				SqlText    = "SELECT @id as ID",
				OracleText = "SELECT :id ID FROM Dual",
				FbText     = "SELECT CAST(@id AS INTEGER) ID FROM Dual")]
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

#if !SQLCE // Works in Query window but not here???
		[Test]
#endif
		public void Test4()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectID(42);

			Assert.AreEqual(42, list[0].ID);
		}
	}
}
