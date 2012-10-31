using System;

using NUnit.Framework;

using BLToolkit.Common;

namespace Common
{
	[TestFixture]
	public class NameOrIndexParameterTest
	{
		[Test]
		public void DefaultValueTest()
		{
			int expectedValue = 0;
			NameOrIndexParameter nip = new NameOrIndexParameter();
			Assert.IsFalse(nip.ByName);
			Assert.AreEqual(nip.Index, expectedValue);
		}

		[Test]
		public void StringTest()
		{
			string expectedValue = "54321";
			NameOrIndexParameter nip = "54321";
			Assert.IsTrue(nip.ByName);
			Assert.AreEqual(nip.Name, expectedValue);
		}

		[Test]
		public void IntTest()
		{
			int expectedValue = 12345;
			NameOrIndexParameter nip = 12345;
			Assert.IsFalse(nip.ByName);
			Assert.AreEqual(nip.Index, expectedValue);
		}

		[Test]
		public void ArrayTest()
		{
			NameOrIndexParameter[] nips = new NameOrIndexParameter[]{ 12345, "54321" };
			Assert.AreEqual(nips[0].Index, 12345);
			Assert.AreEqual(nips[1].Name, "54321");
		}

		[Test]
		public void StringArrayTest()
		{
			NameOrIndexParameter[] nips = NameOrIndexParameter.FromStringArray(new string[] { "98765", "54321" });
			Assert.AreEqual(nips[0].Name, "98765");
			Assert.AreEqual(nips[1].Name, "54321");
		}

		[Test]
		public void IntArrayTest()
		{
			NameOrIndexParameter[] nips = NameOrIndexParameter.FromIndexArray(new int[] { 12345, 56789 });
			Assert.AreEqual(nips[0].Index, 12345);
			Assert.AreEqual(nips[1].Index, 56789);
		}
		
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void IllegalStringTest()
		{
			NameOrIndexParameter nip = null;
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void IllegalStringTest2()
		{
			NameOrIndexParameter nip = string.Empty;
		}
		
		[Test, ExpectedException(typeof(ArgumentException))]
		public void IllegalIntTest()
		{
			NameOrIndexParameter nip = -12345;
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void IllegalAccessTest()
		{
			// Init by index
			NameOrIndexParameter nip = 12345;
			Assert.IsFalse(nip.ByName);

			// Exception here
			string value = nip.Name;
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void IllegalAccessTest2()
		{
			// Init by name
			NameOrIndexParameter nip = "54321";
			Assert.IsTrue(nip.ByName);

			// Exception here
			int value = nip.Index;
		}

		public static object SomeFunc(NameOrIndexParameter nip)
		{
			if (nip.ByName)
			{
				return nip.Name;
			}
			else
			{
				return nip.Index;
			}
		}

		[Test]
		public void FunctionTest()
		{
			int expectedValue = 12345;
			object o = SomeFunc(12345);
			Assert.AreEqual(o, expectedValue);
		}
		
		[Test]
		public void FunctionTest2()
		{
			string expectedValue = "54321";
			object o = SomeFunc("54321");
			Assert.AreEqual(o, expectedValue);
		}
	}
}
