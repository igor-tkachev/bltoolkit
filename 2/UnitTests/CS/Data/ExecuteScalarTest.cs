using System;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteScalarTest
	{
		[Test]
		public void CursorTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int cursorValue = Convert.ToInt32(db.SetSpCommand("Scalar_Cursor").ExecuteScalar());
				
				Assert.AreEqual(expectedValue, cursorValue);
			}
		}

		[Test]
		public void OutputParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int outputParameterValue = Convert.ToInt32(db.SetSpCommand("Scalar_OutputParameter").ExecuteScalar());

				Assert.AreEqual(expectedValue, outputParameterValue);
			}
		}
		
		[Test]
		public void ReturnParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int returnParameterValue = Convert.ToInt32(db.SetSpCommand("Scalar_ReturnParameter").ExecuteScalar());

				Assert.AreEqual(expectedValue, returnParameterValue);
			}
		}
	}
}
