using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class CustomSqlQuery1
	{
		public class TestQueryAttribute : /*[a]*/SqlQueryAttribute/*[/a]*/
		{
			public TestQueryAttribute()
			{
				/*[a]*/IsDynamic = true/*[/a]*/;
			}

			public string OracleText { get; set; }
			public string FbText     { get; set; }
			public string SQLiteText { get; set; }

			public /*[a]*/override/*[/a]*/ string /*[a]*/GetSqlText/*[/a]*/(DataAccessor accessor, DbManager dbManager)
			{
				switch (dbManager.DataProvider.Name)
				{
					case "Sql"       :
					case "MsSql2000" :
					case "MsSql2005" :
					case "MsSql2008" :
					case "MsSql2012" :
					case "Access"    : return SqlText;
					case "Oracle"    : return OracleText ?? SqlText;
					case "Fdp"       : return FbText     ?? SqlText;
					case "SQLite"    : return SQLiteText ?? SqlText;
				}

				throw new ApplicationException(string.Format("Unknown data provider '{0}'", dbManager.DataProvider.Name));
			}
		}

		public abstract class PersonAccessor : DataAccessor
		{
			[TestQuery(
				/*[a]*/SqlText/*[/a]*/    = "SELECT * FROM Person WHERE LastName = @lastName",
				/*[a]*/OracleText/*[/a]*/ = "SELECT * FROM Person WHERE LastName = :lastName")]
			public abstract List<Person> SelectByLastName(string lastName);

			[TestQuery(
				/*[a]*/SqlText/*[/a]*/    = "SELECT * FROM Person WHERE {0} = @value",
				/*[a]*/OracleText/*[/a]*/ = "SELECT * FROM Person WHERE {0} = :value")]
			public abstract List<Person> SelectBy([Format] string fieldName, string value);

			[TestQuery(
				/*[a]*/SqlText/*[/a]*/    = "SELECT TOP {0} * FROM Person WHERE LastName = @lastName",
				/*[a]*/OracleText/*[/a]*/ = "SELECT * FROM Person WHERE LastName = :lastName AND rownum <= {0}",
				/*[a]*/FbText/*[/a]*/     = "SELECT FIRST {0} * FROM Person WHERE LastName = @lastName",
				/*[a]*/SQLiteText/*[/a]*/ = "SELECT * FROM Person WHERE LastName = @lastName LIMIT {0}")]
			public abstract List<Person> SelectByLastName(string lastName, [Format(0)] int top);

			[TestQuery(
				/*[a]*/SqlText/*[/a]*/    = "SELECT @id as PersonID",
				/*[a]*/OracleText/*[/a]*/ = "SELECT :id PersonID FROM Dual",
				/*[a]*/FbText/*[/a]*/     = "SELECT CAST(@id AS INTEGER) PersonID FROM Dual")]
			public abstract List<Person> SelectID(int @id);
		}

		[Test]
		public void Test1()
		{
			PersonAccessor da = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = da.SelectByLastName("Testerson");

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

			List<Person> list = da.SelectByLastName("Testerson", 1);

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
