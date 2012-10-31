using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class OutRefTest
	{
		public class Entity
		{
			public int    ID             = 5;
			public int    outputID;
			public int    inputOutputID  = 10;
			public string str            = "5";
			public string outputStr;
			public string inputOutputStr = "10";
		}

		public abstract class TestAccessor : DataAccessor
		{
			public abstract void OutRefTest(
				int    @ID,  out int    @outputID,  ref int    @inputOutputID,
				string @str, out string @outputStr, ref string @inputOutputStr);

			[SprocName("OutRefTest")]
			public abstract void OutRefTest2(
				[ParamNullValue(-1)]      int               ID,
				[ParamNullValue(-2)]  out int         outputID,
				[ParamNullValue(-3)]  ref int    inputOutputID,
				[ParamNullValue("A")]     string            str,
				[ParamNullValue("B")] out string      outputStr,
				[ParamNullValue("C")] ref string inputOutputStr);

			public abstract void OutRefTest(out int outputID,
				[Direction.InputOutput("inputOutputID", "inputOutputStr"), Direction.Output("outputStr", "outputID")] Entity entity,
				ref string inputOutputStr);
		}

		[Test]
		public void Test1()
		{
			int    @outputID;
			int    @inputOutputID = 10;
			string @outputStr;
			string @inputOutputStr = "10";

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor)))
				.OutRefTest(5,  out @outputID,  ref @inputOutputID,
				           "5", out @outputStr, ref @inputOutputStr);

			Assert.AreEqual(5,     @outputID);
			Assert.AreEqual(15,    @inputOutputID);
			Assert.AreEqual("5",   @outputStr);
			Assert.AreEqual("510", @inputOutputStr);
		}

		[Test]
		public void Test2()
		{
			Entity e = new Entity();
			int    @outputID;
			string @inputOutputStr = "20";

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor)))
				.OutRefTest(out @outputID, e, ref @inputOutputStr);

			Assert.AreEqual(5,     e.outputID);
			Assert.AreEqual(5,     @outputID);
			Assert.AreEqual(15,    e.inputOutputID);
			Assert.AreEqual("5",   e.outputStr);
			Assert.AreEqual("510", e.inputOutputStr);
			Assert.AreEqual("510", @inputOutputStr);
		}

		[Test] 
		public void NullValueTest()
		{
			int    @outputID;
			int    @inputOutputID = 10;
			string @outputStr;
			string @inputOutputStr = "C";

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor)))
				.OutRefTest2(-1, out @outputID,  ref @inputOutputID,
				            "A", out @outputStr, ref @inputOutputStr);

			Assert.AreEqual(-2,        @outputID);
			Assert.AreEqual(-3,   @inputOutputID);
			Assert.AreEqual("B",      @outputStr);
			Assert.AreEqual("C", @inputOutputStr);
		}

		public class NullableEntity
		{
			public int?   ID             = null;
			public int?   outputID;
			public int?   inputOutputID  = 20;
			public string str            = null;
			public string outputStr;
			public string inputOutputStr = "40";
		}

		public abstract class TestNullableAccessor : DataAccessor
		{
			public abstract void OutRefTest(
				int?   @ID,  out int?   @outputID,  ref int?   @inputOutputID,
				string @str, out string @outputStr, ref string @inputOutputStr);

			public abstract void OutRefTest(
				out int? @outputID,
				[Direction.InputOutput("inputOutputID", "inputOutputStr"), Direction.Output("outputStr", "outputID")] NullableEntity entity,
				ref string @inputOutputStr);

		}

		[Test]
		public void TestNullable1()
		{
			int?   @outputID;
			int?   @inputOutputID = 10;
			string @outputStr;
			string @inputOutputStr = "";

			DataAccessor.CreateInstance<TestNullableAccessor>()
				.OutRefTest(null, out @outputID,  ref @inputOutputID,
				            null, out @outputStr, ref @inputOutputStr);

			Assert.IsNull (@outputID);
			Assert.IsNull (@inputOutputID);
			Assert.IsEmpty(@outputStr);
			Assert.IsEmpty(@inputOutputStr);
		}

		[Test]
		public void TestNullable2()
		{
			NullableEntity e = new NullableEntity();
			int?   @outputID;
			string @inputOutputStr = "20";
			e.str = "20";

			DataAccessor.CreateInstance<TestNullableAccessor>()
				.OutRefTest(out @outputID, e, ref @inputOutputStr);

			Assert.IsNull  (@outputID);
			Assert.AreEqual("20", e.outputStr);
			Assert.IsNull  (e.inputOutputID);
			Assert.AreEqual("2040", @inputOutputStr);
		}
	}
}
