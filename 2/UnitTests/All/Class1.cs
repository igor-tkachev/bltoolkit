using System;

using NUnit.Framework;

namespace UnitTests.All
{
	[TestFixture]
	public class GetValueTest
	{
		static byte[]   _arr1 = new byte[0];
		static byte[][] _arr2 = new byte[0][];
		static string[,]  _arr3 = new string[0,0];
		[Test]
		public void Test()
		{
			Arr1 = _arr1;
			Arr2 = _arr2;
			Arr3 = _arr3;
		}

		public byte[]   Arr1;
		public byte[][] Arr2;
		public string[,]  Arr3;

		public void Foo(string toWhom)
		{
			Console.WriteLine(string.Format("Hello, {0}!", toWhom));
		}
	}

	public abstract class AA
	{
		public abstract int[,] IntArr { get; set; }
	}

	public class BB : AA
	{
		public override int[,] IntArr
		{
			get { return null; }
			set { }
		}
	}
}
