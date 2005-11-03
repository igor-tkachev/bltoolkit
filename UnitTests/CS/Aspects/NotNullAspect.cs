using System;
using System.Collections;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace Aspects
{
	[TestFixture]
	public class NotNullAspect
	{
		public NotNullAspect()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class Object1
		{
			public virtual void Foo(string str1, [NotNull] string str2, string str3)
			{
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException), "Value cannot be null.\r\nParameter name: str2")]
		public void Test()
		{
			Object1 o = (Object1)TypeAccessor.GetAccessor(typeof(Object1)).CreateInstance();

			o.Foo("str1", null, "str3");
		}
	}
}
