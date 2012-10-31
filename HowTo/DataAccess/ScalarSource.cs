using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ScalarSource
	{
		public abstract class TestAccessor : DataAccessor
		{
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.DataReader)]
			public abstract int Scalar_DataReader();

			[ActionName("Scalar_DataReader")]
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.DataReader, 1)]
			public abstract string Scalar_DataReader2();

			[ActionName("Scalar_DataReader")]
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.DataReader, "stringField")]
			public abstract string Scalar_DataReader3();

			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.OutputParameter)]
			public abstract int Scalar_OutputParameter();

			[ActionName("Scalar_OutputParameter")]
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.OutputParameter, 1)]
			public abstract string Scalar_OutputParameter2();

			[ActionName("Scalar_OutputParameter")]
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.OutputParameter, "outputString")]
			public abstract string Scalar_OutputParameter3();

			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.ReturnValue)]
			public abstract int Scalar_ReturnParameter();

			[ActionName("Scalar_DataReader")]
			[/*[a]*/ScalarSource/*[/a]*/(ScalarSourceType.AffectedRows)]
			public abstract int Scalar_AffectedRows();

			public static TestAccessor CreateInstance()
			{
				return (TestAccessor)CreateInstance(typeof(TestAccessor));
			}
		}

		[Test]
		public void DataReaderTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int expectedValue = 12345;
			int actualValue   = ta.Scalar_DataReader();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void DataReader2Test()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			string expectedValue = "54321";
			string actualValue   = ta.Scalar_DataReader2();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void DataReader3Test()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			string expectedValue = "54321";
			string actualValue   = ta.Scalar_DataReader3();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void OutputParameterTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int expectedValue = 12345;
			int actualValue   = ta.Scalar_OutputParameter();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void OutputParameter2Test()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			string expectedValue = "54321";
			string actualValue   = ta.Scalar_OutputParameter2();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void OutputParameter3Test()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			string expectedValue = "54321";
			string actualValue   = ta.Scalar_OutputParameter3();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void ReturnParameterTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int expectedValue = 12345;
			int actualValue   = ta.Scalar_ReturnParameter();

			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void AffectedRowsTest()
		{
			TestAccessor ta = TestAccessor.CreateInstance();

			int expectedValue = -1;
			int actualValue   = ta.Scalar_AffectedRows();

			Assert.AreEqual(expectedValue, actualValue);
		}
	}
}
