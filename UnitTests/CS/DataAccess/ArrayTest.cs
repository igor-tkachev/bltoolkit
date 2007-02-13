using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
#if ORACLE

	[TestFixture]
	public class ArrayTest
	{
		public ArrayTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class Entity
		{
			public int[]    intArray            = new int[]    {1,2,3,4,5};
			public int[]    outputIntArray;
			public int[]    inputOutputIntArray = new int[]    {5,6,7,8,9};
			public string[] strArray            = new string[] {"5","4","3","2","1"};
			public string[] outputStrArray;
			public string[] inputOutputStrArray = new string[] {"9","8","7","6","5"};
		}

		public abstract class TestAccessor : DataAccessor
		{
			public abstract void ArrayTest(
				int[]    @intArray, out int[]    @outputIntArray, ref int[]    @inputOutputIntArray,
				string[] @strArray, out string[] @outputStrArray, ref string[] @inputOutputStrArray);

			[ScalarSource(ScalarSourceType.OutputParameter)]
			public abstract int[] ScalarArray();
		}

		[Test]
		public void Test1()
		{
			int[]    @outputIntArray;
			int[]    @inputOutputIntArray = new int[]    {1,2,3,4,5};
			string[] @outputStrArray;
			string[] @inputOutputStrArray = new string[] {"9","8","7","6","5"};

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).ArrayTest(
				new int[]    {1,2,3,4,5},           out @outputIntArray, ref @inputOutputIntArray,
				new string[] {"5","4","3","2","1"}, out @outputStrArray, ref @inputOutputStrArray);

			Assert.AreEqual(new int[] {1,2,3,4,5},                    @outputIntArray);
			Assert.AreEqual(new int[] {2,4,6,8,10},                   @inputOutputIntArray);
			Assert.AreEqual(new string[] { "5","4","3","2","1"},      @outputStrArray);
			Assert.AreEqual(new string[] { "95","84","73","62","51"}, @inputOutputStrArray);
		}

		[Test]
		public void ScalarTest()
		{
			int[] @outputIntArray = ((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).ScalarArray();

			Assert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, @outputIntArray);
		}
	}

#endif
}
