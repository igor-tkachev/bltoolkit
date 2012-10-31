using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Aspects;
using BLToolkit.Reflection;

namespace TypeBuilder
{
	[TestFixture]
	public class GenericMethodTest
	{
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

			// Throws ArgumentNullException
			//
			t.GetValue<string>(null);
		}

		public abstract class TestClass<T>
		{
			public abstract L SelectAll<L>() where L : IList<T>, new();
		}

		// Works only with Mono.
		// See https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=282829
		//
		//[Test]
		public void GenericMixTest()
		{
			TestClass<int> t = TypeAccessor.CreateInstance<TestClass<int>>();
			Assert.That(t, Is.Not.Null);
		}
	}
}
