using System;
using System.IO;
using System.Xml;
using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Reflection;
using NUnit.Framework.SyntaxHelpers;

namespace DataAccess
{
	[TestFixture]
	public class ScalarTest
	{
		public enum TestEnum
		{
			Null  = 0,
			Value = 12345,
		}

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

			[ActionName("Scalar_DataReader")]
			[ScalarSource(ScalarSourceType.AffectedRows)]
			public abstract int Scalar_AffectedRows(DbManager db);

			[TestQuery(
				SqlText    = "SELECT Stream_ FROM DataTypeTest WHERE DataTypeID = @id",
				OracleText = "SELECT Stream_ FROM DataTypeTest WHERE DataTypeID = :id")]
			public abstract Stream GetStream(DbManager db, int id);

			[TestQuery(
				SqlText    = "SELECT Xml_ FROM DataTypeTest WHERE DataTypeID = @id",
				OracleText = "SELECT Xml_ FROM DataTypeTest WHERE DataTypeID = :id")]
			public abstract XmlReader GetXml(DbManager db, int id);

			[TestQuery(
				SqlText    = "SELECT Xml_ FROM DataTypeTest WHERE DataTypeID = @id",
				OracleText = "SELECT Xml_ FROM DataTypeTest WHERE DataTypeID = :id")]
			public abstract XmlDocument GetXmlDoc(DbManager db, int id);

			[SprocName("Scalar_DataReader")]
			public abstract int    ScalarDestination1([Destination] out int id);

			[SprocName("Scalar_DataReader")]
			public abstract void   ScalarDestination2([Destination] out int id);

			[SprocName("Scalar_DataReader")]
			public abstract object ScalarDestination3([Destination] out int id);

			[SprocName("Scalar_DataReader")]
			public abstract int    ScalarDestination4([Destination] ref int id);

			[SprocName("Scalar_DataReader")]
			public abstract void   ScalarDestination5([Destination] ref int id);

			[SprocName("Scalar_DataReader")]
			public abstract object ScalarDestination6([Destination] ref int id);

			[SprocName("Scalar_DataReader")]
			public abstract int? ScalarNullableDestination([Destination] ref int? id);

			[SprocName("Scalar_DataReader")]
			public abstract TestEnum ScalarEnumDestination([Destination] ref TestEnum value);

			[SprocName("Scalar_DataReader")]
			public abstract TestEnum? ScalarNullableEnumDestination([Destination] ref TestEnum? value);

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

#if !ACCESS && !SQLITE
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
#endif

		[Test]
		public void AffectedRowsTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

#if ACCESS
				int expectedValue = 0;
#else
 				int expectedValue = -1;
#endif
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
		public void ScalarDestinationTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int id1;
			int id2;

			id1 = ta.ScalarDestination1(out id2);
			Assert.AreEqual(id1, 12345);
			Assert.AreEqual(id2, 12345);

			ta.ScalarDestination2(out id2);
			Assert.AreEqual(id2, 12345);

			id1 = (int)ta.ScalarDestination3(out id2);
			Assert.AreEqual(id1, 12345);
			Assert.AreEqual(id2, 12345);

			id2 = 0;
			id1 = ta.ScalarDestination4(ref id2);
			Assert.AreEqual(id1, 12345);
			Assert.AreEqual(id2, 12345);

			id2 = 0;
			ta.ScalarDestination5(ref id2);
			Assert.AreEqual(id2, 12345);

			id2 = 0;
			id1 = (int)ta.ScalarDestination6(ref id2);
			Assert.AreEqual(id1, 12345);
			Assert.AreEqual(id2, 12345);
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
				Assert.AreEqual("strvalue", xml.GetAttribute("strattr"));
			}
		}

		public void XmlDocTest()
		{
			using (DbManager db = new DbManager())
			{
				TestAccessor ta = TestAccessor.CreateInstance();

				XmlDocument xmlDocument= ta.GetXmlDoc(db, 2);
				Assert.IsNotNull(xmlDocument);
				Assert.IsNotNull(xmlDocument.DocumentElement);
				Assert.AreEqual("strvalue", xmlDocument.DocumentElement.GetAttribute("strattr"));
			}
		}

		[Test]
		public void ScalarNullableDestinationTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int? id1;
			int? id2 = -1;
			id1 = ta.ScalarNullableDestination(ref id2);
			Assert.AreEqual(id1, id2);
		}

		[Test]
		public void ScalarEnumDestinationTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			TestEnum refVal = TestEnum.Null;
			TestEnum outVal = ta.ScalarEnumDestination(ref refVal);
			Assert.That(outVal, Is.EqualTo(TestEnum.Value));
			Assert.That(refVal, Is.EqualTo(TestEnum.Value));
		}

		[Test]
		public void ScalarNullableEnumDestinationTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			TestEnum? refVal = null;
			TestEnum? outVal = ta.ScalarNullableEnumDestination(ref refVal);
			Assert.That(outVal, Is.EqualTo(TestEnum.Value));
			Assert.That(refVal, Is.EqualTo(TestEnum.Value));
		}
	}
}
