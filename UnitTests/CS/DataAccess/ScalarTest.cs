using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace DataAccess
{
	[TestFixture]
	public class ScalarTest
	{
		public abstract class TestAccessor : DataAccessor
		{
			public abstract int Scalar_Cursor(DbManager db);
			public abstract void Scalar_OutputParameter(DbManager db, out int outputValue);
			public abstract int Scalar_ReturnParameter(DbManager db);

			public static TestAccessor CreateInstance()
			{
				return (TestAccessor)CreateInstance(typeof(TestAccessor));
			}
		}

		[Test]
		public void CursorTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int cursorValue = ta.Scalar_Cursor(db);

				Assert.AreEqual(expectedValue, cursorValue);
			}
		}

		[Test]
		public void OutputParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int outputParameterValue;
				ta.Scalar_OutputParameter(db, out outputParameterValue);

				Assert.AreEqual(expectedValue, outputParameterValue);
			}
		}

		[Test]
		public void ReturnParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int returnParameterValue = ta.Scalar_ReturnParameter(db);

				Assert.AreEqual(expectedValue, returnParameterValue);
			}
		}
	}
}
