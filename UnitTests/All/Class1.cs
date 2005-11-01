using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.TypeBuilder.Builders;

namespace UnitTests.All
{
	public class Base
	{
		public virtual void Foo (string str1, string str2, string str3)
		{
		}
	}

	[TestFixture]
	public class Class1 : Base
	{
		public override void Foo (string str1, string str2, string str3)
		{
			if (str2 == null)
				throw new ArgumentNullException("str2");

			base.Foo(str1, str2, str3);
		}

		//[Test]
		public void Test()
		{
			Foo("str1", null, "str3");
		}
	}
}
