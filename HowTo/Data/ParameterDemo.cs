using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
	using DataAccess;

	[TestFixture]
	public class ParameterDemo
	{
		[Test]
		public void AssignParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				int n = db
					.SetCommand("SELECT @par1 + @par2",
						db./*[a]*/Parameter/*[/a]*/("@par1", 2),
						db./*[a]*/Parameter/*[/a]*/("@par2", 2))
					.ExecuteScalar<int>();

				Assert.AreEqual(4, n);
			}
		}

		[Test]
		public void SetValueTest()
		{
			using (DbManager db = new DbManager())
			{
				db.SetCommand("SELECT @par * 2",
					db./*[a]*/Parameter/*[/a]*/("@par", DbType.Int32));

				db./*[a]*/Parameter("@par").Value/*[/a]*/ = 2;

				Assert.AreEqual(4, db.ExecuteScalar<int>());
			}
		}

		[Test]
		public void ReturnValueTest()
		{
			using (DbManager db = new DbManager())
			{
				/*
				 * CREATE Function Scalar_ReturnParameter()
				 * RETURNS int
				 * AS
				 * BEGIN
				 *     RETURN 12345
				 * END
				 */
				db
					.SetSpCommand("Scalar_ReturnParameter")
					.ExecuteNonQuery();

				int n = (int)db./*[a]*/Parameter("@RETURN_VALUE").Value/*[/a]*/;

				Assert.AreEqual(12345, n);
			}
		}

		[Test]
		public void ReturnValueTest2()
		{
			using (DbManager db = new DbManager())
			{
				int n = db
					.SetSpCommand("Scalar_ReturnParameter")
					.ExecuteScalar<int>(ScalarSourceType.ReturnValue);

				Assert.AreEqual(12345, n);
			}
		}

		[Test]
		public void OutputParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				/*
				 * CREATE Procedure Scalar_OutputParameter
				 *     @outputInt    int         = 0  output,
				 *     @outputString varchar(50) = '' output
				 * AS
				 * BEGIN
				 *     SET @outputInt = 12345
				 *     SET @outputString = '54321'
				 * END
				 */

				db
					.SetSpCommand("Scalar_OutputParameter",
						db./*[a]*/OutputParameter/*[/a]*/("@outputInt",    1),
						db./*[a]*/OutputParameter/*[/a]*/("@outputString", "1"))
					.ExecuteNonQuery();

				Assert.AreEqual(12345,   (int)   db./*[a]*/Parameter("@outputInt").   Value/*[/a]*/);
				Assert.AreEqual("54321", (string)db./*[a]*/Parameter("@outputString").Value/*[/a]*/);
			}
		}

		[Test]
		public void OutputParameterAsReturnValueTest()
		{
			using (DbManager db = new DbManager())
			{
				string returnValue = db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar<string>(/*[a]*/ScalarSourceType.OutputParameter/*[/a]*/, /*[a]*/"outputString"/*[/a]*/);

				Assert.AreEqual("54321", returnValue);
			}
		}

		[Test]
		public void CreateParametersTest()
		{
			Person person = new Person();

			person.FirstName = "John";
			person.LastName  = "Smith";
			person.Gender    = Gender.Male;

			using (DbManager db = new DbManager())
			{
				db.BeginTransaction();

				// Prepare command.
				//
				int id = db
					.SetSpCommand("Person_Insert",
						db./*[a]*/CreateParameters/*[/a]*/(person))
					.ExecuteScalar<int>();

				// Check the result.
				//
				person = db
					.SetCommand(
						"SELECT * FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					.ExecuteObject<Person>();

				Assert.IsNotNull(person);

				// Cleanup.
				//
				db
					.SetCommand(
						"DELETE FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					.ExecuteNonQuery();

				db.CommitTransaction();
			}
		}
	}
}
