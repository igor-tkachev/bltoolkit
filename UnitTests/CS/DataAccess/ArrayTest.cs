#if ORACLE
using BLToolkit.Data;
#else
using System.Collections.Generic;
#endif
using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.TypeBuilder;

namespace DataAccess
{
	[TestFixture]
	public class ArrayTest
	{
		public ArrayTest()
		{
			TypeFactory.SaveTypes = true;
		}

#if ORACLE
		public abstract class TestAccessor : DataAccessor
		{
			public abstract void ArrayTest(
				int[]    @intArray, out int[]    @outputIntArray, ref int[]    @inputOutputIntArray,
				string[] @strArray, out string[] @outputStrArray, ref string[] @inputOutputStrArray);

			[ScalarSource(ScalarSourceType.OutputParameter)]
			public abstract int[] ScalarArray();

			[SqlQuery(@"
DECLARE
	intArray DBMS_UTILITY.NUMBER_ARRAY;
BEGIN
	intArray := :intArray;
	FOR i IN intArray.FIRST..intArray.LAST LOOP
		:inputOutputIntArray(i) := intArray(i);
	END LOOP;
END;"
			)]
			public abstract void Query(int[] @intArray, ref int[] @inputOutputIntArray);

			public abstract void ArrayTest(
				int?[]   @intArray, out int?[]   @outputIntArray, ref int?[]   @inputOutputIntArray,
				string[] @strArray, out string[] @outputStrArray, ref string[] @inputOutputStrArray);

			[ScalarSource(ScalarSourceType.OutputParameter), SprocName("ScalarArray")]
			public abstract int?[] NullableScalarArray();
		}

		[Test]
		public void Test()
		{
			int[]    @outputIntArray;
			int[]    @inputOutputIntArray = new int[]    {1,2,3,4,5};
			string[] @outputStrArray;
			string[] @inputOutputStrArray = new string[] {"9","8","7","6","5"};

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).ArrayTest(
				new int[]    {1,2,3,4,5},            out @outputIntArray, ref @inputOutputIntArray,
				new string[] {"5","4","3","2","1"},  out @outputStrArray, ref @inputOutputStrArray);

			Assert.AreEqual(new int[] {1,2,3,4,5},                @outputIntArray);
			Assert.AreEqual(new int[] {2,4,6,8,10},               @inputOutputIntArray);
			Assert.AreEqual(new string[] { "5","4","3","2","1"},      @outputStrArray);
			Assert.AreEqual(new string[] { "95","84","73","62","51"}, @inputOutputStrArray);
		}

		[Test]
		public void ScalarTest()
		{
			int[] @outputIntArray = ((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).ScalarArray();

			Assert.AreEqual(new int[] { 1, 2, 3, 4, 5 }, @outputIntArray);
		}

		[Test]
		public void NullableTest()
		{
			int?[]   @outputIntArray;
			int?[]   @inputOutputIntArray = new int?[]   {1,2,3,4,5};
			string[] @outputStrArray;
			string[] @inputOutputStrArray = new string[] {"9","8","7","6","5"};

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).ArrayTest(
				new int?[]    {1,null,3,4,5},        out @outputIntArray, ref @inputOutputIntArray,
				new string[] {"5","4","3","2","1"},  out @outputStrArray, ref @inputOutputStrArray);

			Assert.AreEqual(new int?[] {1,0,3,4,5},                   @outputIntArray);
			Assert.AreEqual(new int?[] {2,0,6,8,10},                  @inputOutputIntArray);
			Assert.AreEqual(new string[] { "5","4","3","2","1"},      @outputStrArray);
			Assert.AreEqual(new string[] { "95","84","73","62","51"}, @inputOutputStrArray);
		}

		[Test]
		public void NullableScalarTest()
		{
			int?[] @outputIntArray = ((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).NullableScalarArray();

			Assert.AreEqual(new int?[] { 1, 2, 3, 4, 5 }, @outputIntArray);
		}

		[Test]
		public void QueryTest()
		{
			int[] intArray            = new int[]{1,2,3,4,5};
			int[] inputOutputIntArray = new int[5];

			((TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor))).Query(
				intArray, ref inputOutputIntArray);

			Assert.AreEqual(intArray, inputOutputIntArray);
		}


#elif MSSQL

		public abstract class TestAccessor : DataAccessor
		{
			[SprocName("ArrayTest")]
			public abstract IList<int> SprocTest(int[] @inputIntArray);
			[SqlQuery("SELECT Num * 3 FROM @inputIntArray")]
			public abstract IList<int> QueryTest([ParamTypeName("IntArray")] int[] @inputIntArray);
		}

		[Test]
		public void Test()
		{
			int[] intArray = { 1, 2, 3, 4, 5 };

			TestAccessor a = TypeFactory.CreateInstance<TestAccessor>();
			IList<int> list;

			list = a.SprocTest(intArray);
			for (int i = 0; i < list.Count; i++)
				Assert.That(list[i], Is.EqualTo(intArray[i]*2));
			list = a.QueryTest(intArray);
			for (int i = 0; i < list.Count; i++)
				Assert.That(list[i], Is.EqualTo(intArray[i]*3));
		}
#endif
	}
}
