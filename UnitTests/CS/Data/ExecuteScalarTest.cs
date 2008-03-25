using System;

using NUnit.Framework;

using BLToolkit.Data;

namespace Data
{
	[TestFixture]
	public class ExecuteScalarTest
	{
		[Test]
		public void RegressionTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE
						.SetCommand("SELECT 12345 FROM Dual")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar());

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReaderTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE
						.SetCommand("SELECT 12345 FROM Dual")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar(ScalarSourceType.DataReader));
				
				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReader2Test()
		{
			using (DbManager db = new DbManager())
			{
				string expectedValue = "54321";
				string actualValue = db.MappingSchema.ConvertToString(
					db
#if SQLITE
						.SetCommand("SELECT 12345, '54321' FROM Dual")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar(ScalarSourceType.DataReader, 1));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReader3Test()
		{
			using (DbManager db = new DbManager())
			{
				string expectedValue = "54321";
				string actualValue = db.MappingSchema.ConvertToString(
					db
#if SQLITE
						.SetCommand("SELECT 12345 intField, '54321' stringField FROM Dual")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar(ScalarSourceType.DataReader, "stringField"));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
		
#if !ACCESS && !SQLITE
		[Test]
		public void OutputParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db.MappingSchema.ConvertToInt32(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter2Test()
		{
			using (DbManager db = new DbManager())
			{
				string expectedValue = "54321";
				string actualValue = db.MappingSchema.ConvertToString(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter, 1));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter3Test()
		{
			using (DbManager db = new DbManager())
			{
				string expectedValue = "54321";
				string actualValue = db.MappingSchema.ConvertToString(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter, "outputString"));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
		
		[Test]
		public void ReturnParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db.MappingSchema.ConvertToInt32(db
					.SetSpCommand("Scalar_ReturnParameter")
					.ExecuteScalar(ScalarSourceType.ReturnValue));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
#endif

		[Test]
		public void AffectedRowsTest()
		{
			using (DbManager db = new DbManager())
			{
#if ACCESS || SQLITE
				int expectedValue = 0;
#else
 				int expectedValue = -1;
#endif
				int actualValue = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE
						.SetCommand("SELECT 12345 FROM Dual")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar(ScalarSourceType.AffectedRows));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void GenericsRegressionTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db
#if SQLITE
					.SetCommand("SELECT 12345 FROM Dual")
#else
					.SetSpCommand("Scalar_DataReader")
#endif
					.ExecuteScalar<int>();

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void GenericsDataReaderTest()
		{
			using (DbManager db = new DbManager())
			{
				int expectedValue = 12345;
				int actualValue = db
#if SQLITE
					.SetCommand("SELECT 12345 FROM Dual")
#else
					.SetSpCommand("Scalar_DataReader")
#endif
					.ExecuteScalar<int>(ScalarSourceType.DataReader);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void GenericsDataReader2Test()
		{
			using (DbManager db = new DbManager())
			{
				string expectedValue = "54321";
				string actualValue = db
#if SQLITE
					.SetCommand("SELECT 12345, '54321' FROM Dual")
#else
					.SetSpCommand("Scalar_DataReader")
#endif
					.ExecuteScalar<string>(ScalarSourceType.DataReader, 1);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
	}
}
