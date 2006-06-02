using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace DataAccess
{
	[TestFixture]
	public class EnumTest
	{
		public enum Gender
		{
			[MapValue("F")] E_Female,
			[MapValue("M")] E_Male,
			[MapValue("U")] E_Unknown,
			[MapValue("O")] E_Other
		}

		public enum RefEnum
		{
			[MapValue("A")]  E_A,
			[MapValue("B")]  E_B,
			[MapValue("BB")] E_BB
		}

		public abstract class TestAccessor : DataAccessor
		{
			public abstract int Person_Insert(
				string @FirstName, string @LastName, string @MiddleName, Gender @Gender);

			public abstract void Person_Delete(int @personID);

			public abstract void OutRefEnumTest(
				string @str, out RefEnum @outputStr, ref RefEnum @inputOutputStr);

		}

		[Test]
		public void Test()
		{
			TestAccessor ta = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			int id = ta.Person_Insert("Crazy", "Frog", null, Gender.E_Unknown);

			Assert.IsTrue(id > 0);
			ta.Person_Delete(id);
		}

		[Test]
		public void RefTest()
		{
			TestAccessor ta = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			RefEnum a;
			RefEnum b = RefEnum.E_B;

			ta.OutRefEnumTest("B", out a, ref b);

			Assert.AreEqual(RefEnum.E_B,  a);
			Assert.AreEqual(RefEnum.E_BB, b);
		}
	}
}
