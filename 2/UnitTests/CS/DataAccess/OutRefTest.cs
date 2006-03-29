using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace A.DataAccess
{
	[TestFixture]
	public class OutRefTest
	{
		public OutRefTest()
		{
			TypeFactory.SaveTypes = true;
		}

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

			public abstract void OutRefTest(
				out int @outputID, Entity entity, ref string @inputOutputStr);

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

			Assert.AreEqual(5,     @outputID);
			Assert.AreEqual(10,    e.inputOutputID);
			Assert.AreEqual(null,  e.outputStr);
			Assert.AreEqual("10", @inputOutputStr);
		}
	}
}
