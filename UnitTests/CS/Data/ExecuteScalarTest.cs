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
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE || SQLCE
						.SetCommand("SELECT 12345")
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
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE || SQLCE
						.SetCommand("SELECT 12345")
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
			using (var db = new DbManager())
			{
				var expectedValue = "54321";
				var actualValue   = db.MappingSchema.ConvertToString(
					db
#if SQLITE || SQLCE
						.SetCommand("SELECT 12345, '54321'")
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
			using (var db = new DbManager())
			{
				var expectedValue = "54321";
				var actualValue   = db.MappingSchema.ConvertToString(
					db
#if SQLITE || SQLCE
						.SetCommand("SELECT 12345 intField, '54321' stringField")
#else
						.SetSpCommand("Scalar_DataReader")
#endif
						.ExecuteScalar(ScalarSourceType.DataReader, "stringField"));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
		
#if !ACCESS && !SQLITE && !SQLCE
		[Test]
		public void OutputParameterTest()
		{
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db.MappingSchema.ConvertToInt32(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter2Test()
		{
			using (var db = new DbManager())
			{
				var expectedValue = "54321";
				var actualValue   = db.MappingSchema.ConvertToString(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter, 1));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter3Test()
		{
			using (var db = new DbManager())
			{
				var expectedValue = "54321";
				var actualValue   = db.MappingSchema.ConvertToString(db
					.SetSpCommand("Scalar_OutputParameter")
					.ExecuteScalar(ScalarSourceType.OutputParameter, "outputString"));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
		
		[Test]
		public void ReturnParameterTest()
		{
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db.MappingSchema.ConvertToInt32(db
					.SetSpCommand("Scalar_ReturnParameter")
					.ExecuteScalar(ScalarSourceType.ReturnValue));

				Assert.AreEqual(expectedValue, actualValue);
			}
		}
#endif

		[Test]
		public void AffectedRowsTest()
		{
			using (var db = new DbManager())
			{
#if ACCESS || SQLITE
				var expectedValue = 0;
#else
				var expectedValue = -1;
#endif
				var actualValue   = db.MappingSchema.ConvertToInt32(
					db
#if SQLITE || SQLCE
						.SetCommand("SELECT 12345")
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
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db
#if SQLITE || SQLCE
					.SetCommand("SELECT 12345")
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
			using (var db = new DbManager())
			{
				var expectedValue = 12345;
				var actualValue   = db
#if SQLITE || SQLCE
					.SetCommand("SELECT 12345")
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
			using (var db = new DbManager())
			{
				var expectedValue = "54321";
				var actualValue   = db
#if SQLITE || SQLCE
					.SetCommand("SELECT 12345, '54321'")
#else
					.SetSpCommand("Scalar_DataReader")
#endif
					.ExecuteScalar<string>(ScalarSourceType.DataReader, 1);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void NoResult()
		{
			using (var db = new DbManager())
			{
				var actualValue = db.SetCommand("SELECT FirstName FROM Person WHERE PersonID = -1").ExecuteScalar<string>();
				Assert.AreEqual("", actualValue);
			}
		}
	}
}
