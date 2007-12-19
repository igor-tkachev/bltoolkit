using System;
using BLToolkit.Aspects;
using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.TypeBuilder;

namespace TypeBuilder
{
	[TestFixture]
	public class GenericMethodTest
	{
		public GenericMethodTest()
		{
			TypeFactory.SaveTypes = true;
		}

		public abstract class TestObject
		{
			public virtual T GetValue<T>([NotNull] T value) where T : class, ICloneable
			{
				return value;
			}

			public abstract T Abstract<T>(T value) where T : struct, IFormattable;
			public abstract T Abstract2<T>(T value) where T : new();
			public abstract T Abstract3<T>(T value);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void Test()
		{
			// If you got an 'Invalid executable format' exception here
			// you need to install .Net Framework 2.0 SP1 or later.
			//
			TestObject t = TypeAccessor<TestObject>.CreateInstance();
			Assert.AreEqual("123", t.GetValue("123"));
			Assert.AreEqual(0, t.Abstract(123));
			Assert.AreEqual(0, t.Abstract2(123));
			Assert.AreEqual(0, t.Abstract3(123));

			t.GetValue<string>(null);
		}
	}
}
