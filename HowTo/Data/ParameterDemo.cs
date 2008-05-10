using System;
using System.Data;

using NUnit.Framework;

using BLToolkit.Data;

namespace HowTo.Data
{
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

				db.Parameter("@par").Value = 2;

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
					.ExecuteScalar<int>();

				int n = (int)db.Parameter("@RETURN_VALUE").Value;

				Assert.AreEqual(12345, n);
			}
		}
	}
}
