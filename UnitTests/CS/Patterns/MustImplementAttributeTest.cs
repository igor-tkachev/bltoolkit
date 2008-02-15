using System;
using BLToolkit.TypeBuilder;
using NUnit.Framework;

using BLToolkit.Patterns;

namespace Patterns
{
	[TestFixture]
	public class MustImplementAttributeTest
	{
		public MustImplementAttributeTest()
		{
			TypeFactory.SaveTypes = true;
		}

		[MustImplement]
		public interface RequiredInterface
		{
			int RequiredMethod();
			[MustImplement(false)]
			int SameMethodName();
			[MustImplement(false)]
			int OptionalMethod();
		}

		public interface SameMethodNameInterface
		{
			[MustImplement]
			int SameMethodName();
		}

		public interface IntermediateInterface : RequiredInterface, SameMethodNameInterface
		{
			new int OptionalMethod();
		}

		public interface OptionalInterface : IntermediateInterface, SameMethodNameInterface
		{
		}

		[MustImplement(false, ThrowException = false)]
		public interface OptionalInterfaceNoException : RequiredInterface
		{
			int OtherOptionalMethod();
		}

		[MustImplement(true, ThrowException = false)]
		public interface IOtherOptionalInterface
		{
			int SameMethodName();
		}

		public struct TestClass
		{
			public int RequiredMethod()
			{
				return 1;
			}

			public int SameMethodName()
			{
				return 2;
			}
		}

		public class EmptyClass
		{
		}

		[Test]
		public void Test()
		{
			OptionalInterfaceNoException duck = DuckTyping.Implement<OptionalInterfaceNoException> (new TestClass());

			Assert.AreEqual(1, duck.RequiredMethod());
			Assert.AreEqual(0, duck.OtherOptionalMethod());
			Assert.AreEqual(2, duck.SameMethodName());
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void RuntimeExceptionTes()
		{
			OptionalInterface duck = DuckTyping.Implement<OptionalInterface>(new TestClass());

			Assert.AreEqual(1, duck.RequiredMethod());

			// Exception here.
			//
			duck.OptionalMethod();
		}

		[Test, ExpectedException(typeof(TypeBuilderException))]
		public void BuildtimeExceptionTest()
		{
			// Exception here.
			//
			OptionalInterface duck1 = DuckTyping.Implement<OptionalInterface> (string.Empty);
			OptionalInterface duck2 = (OptionalInterface)DuckTyping.Implement(typeof(OptionalInterface), string.Empty);
		}

		[Test]
		public void AsLikeBehaviourTest()
		{
			IOtherOptionalInterface duck1 = DuckTyping.Implement<IOtherOptionalInterface>(new TestClass());
			IOtherOptionalInterface duck2 = DuckTyping.Implement<IOtherOptionalInterface>(new EmptyClass());
			IOtherOptionalInterface duck3 = DuckTyping.Implement<IOtherOptionalInterface>(new EmptyClass());

			Assert.IsNotNull(duck1);
			Assert.IsNull   (duck2);
			Assert.IsNull   (duck3);
		}
	}
}
