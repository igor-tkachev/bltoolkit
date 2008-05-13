using System;
using NUnit.Framework;
using BLToolkit.Data;

namespace HowTo.Data
{
	[TestFixture]
	public class ExecuteScalar
	{
		string GetFirstName(int id)
		{
			using (DbManager db = new DbManager())
			{
				return db
					.SetCommand("SELECT FirstName FROM Person WHERE PersonID = @id",
						db.Parameter("@id", id))
					./*[a]*/ExecuteScalar/*[/a]*/<string>();
			}
		}

		[Test]
		public void ReaderTest()
		{
			string firstName = GetFirstName(1);

			Assert.IsNotNull(firstName);
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
				int n = db
					.SetSpCommand("Scalar_ReturnParameter")
					./*[a]*/ExecuteScalar/*[/a]*/<int>(/*[a]*/ScalarSourceType.ReturnValue/*[/a]*/);

				Assert.AreEqual(12345, n);
			}
		}

		[Test]
		public void OutputParameterAsReturnValueTest()
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
				string returnValue = db
					.SetSpCommand("Scalar_OutputParameter")
					./*[a]*/ExecuteScalar/*[/a]*/<string>(/*[a]*/ScalarSourceType.OutputParameter/*[/a]*/, /*[a]*/"outputString"/*[/a]*/);

				Assert.AreEqual("54321", returnValue);
			}
		}
	}
}
