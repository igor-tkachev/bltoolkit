using System;
using BLToolkit.Data;
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
				string @FirstName, string @MiddleName, string @LastName, Gender @Gender);

#if ACCESS
			public abstract int Person_SelectByName(
				string @FirstName, string @MiddleName);
#endif
			public abstract void Person_Delete(int @personID);

			public abstract void OutRefEnumTest(
				string @str, out RefEnum @outputStr, ref RefEnum @inputOutputStr);

		}

		[Test]
		public void Test()
		{
			TestAccessor ta = (TestAccessor)DataAccessor.CreateInstance(typeof(TestAccessor));

			int id = ta.Person_Insert("Crazy", null, "Frog", Gender.E_Unknown);

#if ACCESS
			Assert.AreEqual(0, id);
			id = ta.Person_SelectByName("Crazy", "Frog");
#endif

			Assert.IsTrue(id > 0);
			ta.Person_Delete(id);
		}

#if !ACCESS
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
#endif
	}
}
