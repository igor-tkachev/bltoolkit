using System;
using System.IO;
using System.Xml;
using BLToolkit.Reflection;
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
			[ActionName("Scalar_DataReader")]
			public abstract int Scalar_Regression(DbManager db);
			
			[ScalarSource(ScalarSourceType.DataReader)]
			public abstract int Scalar_DataReader(DbManager db);

			[ActionName("Scalar_DataReader")]
			[ScalarSource(ScalarSourceType.DataReader, 1)]
			public abstract string Scalar_DataReader2(DbManager db);

			[ActionName("Scalar_DataReader")]
			[ScalarSource(ScalarSourceType.DataReader, "stringField")]
			public abstract string Scalar_DataReader3(DbManager db);

			[ActionName("Scalar_OutputParameter")]
			public abstract void Scalar_OutputParameterRegression(
				DbManager db, out int outputInt, out string outputString);

			[ScalarSource(ScalarSourceType.OutputParameter)]
			public abstract int Scalar_OutputParameter(DbManager db);

			[ActionName("Scalar_OutputParameter")]
			[ScalarSource(ScalarSourceType.OutputParameter, 1)]
			public abstract string Scalar_OutputParameter2(DbManager db);

			[ActionName("Scalar_OutputParameter")]
			[ScalarSource(ScalarSourceType.OutputParameter, "outputString")]
			public abstract string Scalar_OutputParameter3(DbManager db);

			[ScalarSource(ScalarSourceType.ReturnValue)]
			public abstract int Scalar_ReturnParameter(DbManager db);

			[ActionName("Scalar_ReturnParameter")]
			[ScalarSource(ScalarSourceType.AffectedRows)]
			public abstract int Scalar_AffectedRows(DbManager db);

			[SqlQuery("SELECT Stream_ FROM DataTypeTest WHERE DataTypeID = @id")]
			public abstract Stream GetStream(DbManager db, int id);

			[SqlQuery("SELECT Xml_ FROM DataTypeTest WHERE DataTypeID = @id")]
			public abstract XmlReader GetXml(DbManager db, int id);

			public static TestAccessor CreateInstance()
			{
				return (TestAccessor)CreateInstance(typeof(TestAccessor));
			}
		}

		[Test]
		public void RegressionTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int actualValue = ta.Scalar_Regression(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReaderTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int actualValue = ta.Scalar_DataReader(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReader2Test()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				string expectedValue = "54321";
				string actualValue = ta.Scalar_DataReader2(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void DataReader3Test()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				string expectedValue = "54321";
				string actualValue = ta.Scalar_DataReader3(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameterRegressionTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedIntValue = 12345;
				int actualIntValue;
				string expectedStringValue = "54321";
				string actualStringValue;

				ta.Scalar_OutputParameterRegression(db,
					out actualIntValue, out actualStringValue);

				Assert.AreEqual(expectedIntValue, actualIntValue);
				Assert.AreEqual(expectedStringValue, actualStringValue);
			}
		}

		[Test]
		public void OutputParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int actualValue = ta.Scalar_OutputParameter(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter2Test()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				string expectedValue = "54321";
				string actualValue = ta.Scalar_OutputParameter2(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void OutputParameter3Test()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				string expectedValue = "54321";
				string actualValue = ta.Scalar_OutputParameter3(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void ReturnParameterTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = 12345;
				int actualValue = ta.Scalar_ReturnParameter(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void AffectedRowsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				int expectedValue = -1;
				int actualValue = ta.Scalar_AffectedRows(db);

				Assert.AreEqual(expectedValue, actualValue);
			}
		}

		[Test]
		public void StreamTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				Stream s = ta.GetStream(db, 2);
				Byte[] bytes = new byte[16];

				Assert.IsNotNull(s);
				Assert.AreEqual(s.Length, bytes.Length);

				Assert.AreEqual(s.Read(bytes, 0, bytes.Length), bytes.Length);
				TypeAccessor.WriteConsole(bytes);
			}
		}

		[Test]
		public void XmlTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				XmlReader xml = ta.GetXml(db, 2);
				xml.MoveToContent();
				Assert.IsTrue(xml.ReadToDescendant("element"));
				Console.WriteLine("{0}", xml.GetAttribute("strattr"));
			}
		}
	}
}
