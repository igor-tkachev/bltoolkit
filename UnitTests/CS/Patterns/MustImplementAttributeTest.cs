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
#if FW2
			OptionalInterfaceNoException duck = DuckTyping.Implement<OptionalInterfaceNoException> (new TestClass());
#else
			OptionalInterfaceNoException duck = (OptionalInterfaceNoException)DuckTyping.Implement(typeof(OptionalInterfaceNoException), new TestClass());
#endif

			Assert.AreEqual(1, duck.RequiredMethod());
			Assert.AreEqual(0, duck.OtherOptionalMethod());
			Assert.AreEqual(2, duck.SameMethodName());
		}

		[Test, ExpectedException(typeof(NotImplementedException))]
		public void RuntimeExceptionTes()
		{
#if FW2
			OptionalInterface duck = DuckTyping.Implement<OptionalInterface> (new TestClass());
#else
			OptionalInterface duck = (OptionalInterface)DuckTyping.Implement(typeof(OptionalInterface), new TestClass());
#endif

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
#if FW2
			OptionalInterface duck = DuckTyping.Implement<OptionalInterface> (string.Empty);
#else
			OptionalInterface duck = (OptionalInterface)DuckTyping.Implement(typeof(OptionalInterface), string.Empty);
#endif
		}

		[Test]
		public void AsLikeBehaviourTest()
		{
#if FW2
			IOtherOptionalInterface duck1 = DuckTyping.Implement<IOtherOptionalInterface>(new TestClass());
			IOtherOptionalInterface duck2 = DuckTyping.Implement<IOtherOptionalInterface>(new EmptyClass());
#else
			IOtherOptionalInterface duck1 = (IOtherOptionalInterface)DuckTyping.Implement(typeof(IOtherOptionalInterface), new TestClass());
			IOtherOptionalInterface duck2 = (IOtherOptionalInterface)DuckTyping.Implement(typeof(IOtherOptionalInterface), new EmptyClass());
#endif
			Assert.IsNotNull(duck1);
			Assert.IsNull   (duck2);
		}
	}
}
