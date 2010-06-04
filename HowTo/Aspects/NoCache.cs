using System;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace HowTo.Aspects
{
	[/*[a]*/Cache/*[/a]*/]
	public /*[a]*/abstract/*[/a]*/ class NoCacheTestClass
	{
		public static int Value;

		public /*[a]*/virtual/*[/a]*/ int CachedMethod(int p1, int p2)
		{
			return Value;
		}

		[/*[a]*/NoCache/*[/a]*/]
		public /*[a]*/virtual/*[/a]*/ int NoCacheMethod(int p1, int p2)
		{
			return Value;
		}

		public static NoCacheTestClass CreateInstance()
		{
			// Use TypeAccessor to create an instance of an abstract class.
			//
			return /*[a]*/TypeAccessor/*[/a]*/<NoCacheTestClass>.CreateInstance();
		}
	}

	[TestFixture]
	public class NoCacheAttributeTest
	{
		[Test]
		public void Test()
		{
			NoCacheTestClass t = TypeAccessor<NoCacheTestClass>.CreateInstance();

			NoCacheTestClass.Value = 1; Assert.AreEqual(/*[a]*/1/*[/a]*/, t.CachedMethod(1, 1));
			NoCacheTestClass.Value = 2; Assert.AreEqual(/*[a]*/1/*[/a]*/, t.CachedMethod(1, 1)); // no change

			NoCacheTestClass.Value = 3; Assert.AreEqual(/*[a]*/3/*[/a]*/, t.NoCacheMethod(2, 1));
			NoCacheTestClass.Value = 4; Assert.AreEqual(/*[a]*/4/*[/a]*/, t.NoCacheMethod(2, 1));
		}
	}
}
