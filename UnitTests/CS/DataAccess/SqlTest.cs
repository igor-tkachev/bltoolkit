using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.Data.DataProvider;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using System.Linq;

namespace DataAccess
{
	[TestFixture]
	public class SqlTest
	{
		public class Name
		{
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public class Person
		{
			[MapField("PersonID"), PrimaryKey]
			public int  ID;
			[MapField(Format="{0}")]
			public Name Name = new Name();

			[SqlIgnore]
			public string Gender;
		}

		[Test]
		public void Test()
		{
			var da = new SqlQuery();
			var p  = (Person)da.SelectByKey(typeof(Person), 1);

			Assert.AreEqual("Pupkin", p.Name.LastName);
		}

		[Test]
		public void GetFieldListTest()
		{
			SqlQuery     da = new SqlQuery();
			SqlQueryInfo info;

			using (var db = new DbManager())
			{
				info = da.GetSqlQueryInfo(db, typeof (Person),        "SelectAll");

				Console.WriteLine(info.QueryText);
				Assert.That(info.QueryText.Contains("\t" + db.DataProvider.Convert("PersonID", ConvertType.NameToQueryField)));
				Assert.That(info.QueryText.Contains("\t" + db.DataProvider.Convert("LastName", ConvertType.NameToQueryField)));
				Assert.That(info.QueryText, Is.Not.Contains("\t" + db.DataProvider.Convert("Name", ConvertType.NameToQueryField)));
			}
		}

		[MapField("InnerId", "InnerObject.Id")]
		public class TestObject
		{
			public int        Id;
			public TestObject InnerObject;
		}

		[Test]
		public void RecursiveTest()
		{
			var query = new SqlQuery<TestObject>();
			var info  = query.GetSqlQueryInfo(new DbManager(), "SelectAll");

			Console.WriteLine(info.QueryText);
			Assert.That(info.QueryText.Contains("InnerId"));
			Assert.That(info.QueryText, Is.Not.Contains("InnerObject"));
		}

		[Test]
		public void SqlIgnoreAttributeTest()
		{
			var da = new SqlQuery();
			var p  = (Person)da.SelectByKey(typeof(Person), 1);

			Assert.IsNull(p.Gender);
		}

		public class TestCategory
		{
			[PrimaryKey, NonUpdatable]
			public int    Id;
			public string Name;
		}

		[MapField("CategoryId", "Category.Id")]
		public class TestObject2
		{
			[PrimaryKey, NonUpdatable]
			public int          Id;
			public TestCategory Category;
		}

		[Test]
		public void NonUpdatableTest()
		{
			var da = new SqlQuery();

			using (var db = new DbManager())
			{
				var update = da.GetSqlQueryInfo<TestCategory>(db, "Update");
				var insert = da.GetSqlQueryInfo<TestCategory>(db, "Insert");

				Assert.That(update.QueryText, Is.Not.Contains(
					"\t" + db.DataProvider.Convert("Id", ConvertType.NameToQueryField) + " = " + db.DataProvider.Convert("Id", db.GetConvertTypeToParameter()) + "\n"),
					"Update");
				Assert.That(insert.QueryText, Is.Not.Contains("Id"), "Insert");
			}
		}

		[Test]
		public void ComplexMapperNonUpdatableTest()
		{
			var da = new SqlQuery();

			using (var db = new DbManager())
			{
				var update = da.GetSqlQueryInfo<TestObject2>(db, "Update");
				var insert = da.GetSqlQueryInfo<TestObject2>(db, "Insert");

				Assert.That(update.QueryText.Contains("CategoryId"), "Update");
				Assert.That(insert.QueryText.Contains("CategoryId"), "Insert");
			}
		}


		[TableName("DataTypeTest")]
		class UpdateTest
		{
			[PrimaryKey] public Guid Guid_;

			public UpdateTest(Guid guid)
			{
				Guid_ = guid;
			}
		}

		[Test, ExpectedException(typeof(DataAccessException))]
		public void UpdateGuid()
		{
			new SqlQuery<UpdateTest>().Update(new UpdateTest(Guid.NewGuid()));
		}

		[TableName("Person")]
		public class Person1
		{
			[Identity]                     public int    PersonID;
			                               public string FirstName;
			                               public string LastName;
			[NonUpdatable(OnInsert=false)] public string MiddleName;
			                               public char   Gender;
		}

		[Test]
		public void NonUpdatableOnInsert()
		{
			using (var db = new DbManager())
			{
				db.BeginTransaction();

				new SqlQuery<Person1>().Insert(db, new Person1
				{
					FirstName  = "TestOnInsert",
					LastName   = "",
					MiddleName = "1",
					Gender     = 'M'
				});

				var p = db.GetTable<Person1>().Single(_ => _.FirstName == "TestOnInsert");

				Assert.AreNotEqual("1", p.MiddleName);

				db.RollbackTransaction();
			}
		}
	}
}
