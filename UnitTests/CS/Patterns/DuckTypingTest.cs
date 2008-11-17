using System;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Patterns;
using NUnit.Framework.SyntaxHelpers;

namespace Patterns
{
	[TestFixture]
	public class DuckTypingTest
	{
		public interface TestInterface
		{
			int  Method(int value);
			void Method(int v1, out int v2);

			int Prop { get; }

			event EventHandler Event;

			void CallEvent();
		}

		public interface TestInterface2
		{
			int  Method(int value);
			void I2Method(int v1, out int v2);
		}

		public class TestClass
		{
			public static int Field;

			public int Method(int value)
			{
				return value + 2;
			}

			public void Method(int v1, out int v2)
			{
				v2 = v1;
			}

			public void I2Method(int v1, out int v2)
			{
				v2 = v1;
			}

			public int Prop
			{
				get { return 22; }
			}

			public event EventHandler Event;

			public void CallEvent()
			{
				Event(this, EventArgs.Empty);
			}
		}

		[Test]
		public void Test()
		{
			TestInterface   duck = DuckTyping.Implement<TestInterface> (new TestClass());
			TestInterface   same = DuckTyping.Implement<TestInterface> (duck);
			TestInterface2 duck2 = DuckTyping.Implement<TestInterface2>(same);

			Assert.AreSame(duck, same);

			int value;
			duck.Method(33, out value);

			Assert.AreEqual(33, value);
			Assert.AreEqual(42, duck.Method(40));
			Assert.AreEqual(22, duck.Prop);

			duck.Event += duck_Event;

			duck.CallEvent();

			Assert.AreEqual(55, eventValue);

			duck2.I2Method(33, out value);

			Assert.AreEqual(33, value);
			Assert.AreEqual(42, duck2.Method(40));
		}

		int eventValue;

		void duck_Event(object sender, EventArgs e)
		{
			eventValue = 55;
		}

		public class Child1 : TestClass
		{
			public new int Method(int value)
			{
				return value + 5;
			}
		}

		public class Child2 : TestClass
		{
			public new int Method(int value)
			{
				return value + 10;
			}
		}

		[Test]
		public void BulkTest()
		{
			TestInterface[] ducks  = DuckTyping.Implement<TestInterface, TestClass> (new Child1(), new Child2());

			Assert.IsNotEmpty(ducks);
			Assert.AreEqual(42, ducks[0].Method(40));
			Assert.AreEqual(42, ducks[1].Method(40));
		}

		[Test]
		public void BulkTest2()
		{
			TestInterface[] ducks = DuckTyping.Implement<TestInterface>(new Child1(), new Child2());

			Assert.IsNotEmpty(ducks);
			Assert.AreEqual(45, ducks[0].Method(40));
			Assert.AreEqual(50, ducks[1].Method(40));
		}
		
		[Test]
		public void InheritanceTest()
		{
			TestInterface duck1 = DuckTyping.Implement<TestInterface> (new Child1());
			TestInterface duck2 = DuckTyping.Implement<TestInterface> (new Child2());

			Assert.AreNotSame(duck1, duck2);
			Assert.AreEqual(45, duck1.Method(40));
			Assert.AreEqual(50, duck2.Method(40));

		}

		[Test]
		public void InheritanceTest2()
		{
			TestInterface duck1 = DuckTyping.Implement<TestInterface, TestClass> (new Child1());
			TestInterface duck2 = DuckTyping.Implement<TestInterface, TestClass> (new Child2());

			Assert.AreNotSame(duck1, duck2);
			Assert.AreEqual(42, duck1.Method(40));
			Assert.AreEqual(42, duck2.Method(40));
		}

		public class StaticClass
		{
			public static int Method(int value)
			{
				return value + 3;
			}
		}

		[Test]
		public void StaticTest()
		{
			DuckTyping.AllowStaticMembers = true;
			TestInterface duck = DuckTyping.Implement<TestInterface, StaticClass> (new StaticClass());

			Assert.AreEqual(43, duck.Method(40));
		}

		public struct TestStruct
		{
			public int Method(int value)
			{
				return value + 3;
			}
		}

		[Test]
		public void StructTest()
		{
			DuckTyping.AllowStaticMembers = true;
			TestInterface duck = DuckTyping.Implement<TestInterface> (new TestStruct());

			Assert.AreEqual(43, duck.Method(40));
		}


		public interface GenericInterface<T>
		{
			T Method(T value);
		}

		public class GenericClass<T>
		{
			public T Method(T value)
			{
				return value;
			}

			public void I2Method(int v1, out int v2)
			{
				v2 = v1 + 2;
			}
		}

		[Test]
		public void GenericInterfaceTest()
		{
			GenericClass<int> o = new GenericClass<int>();
			GenericInterface<int> duck  = DuckTyping.Implement<GenericInterface<int>> (o);
			TestInterface2        duck2 = DuckTyping.Implement<TestInterface2> (o);

			Assert.AreEqual(40, duck .Method(40));
			Assert.AreEqual(40, duck2.Method(40));

			int value;
			duck2.I2Method (33, out value);
			Assert.AreEqual(35, value);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void InvalidArgTest1()
		{
			TestInterface o = null;

			TestInterface duck1 = DuckTyping.Implement<TestInterface>(o);
			TestInterface duck2 = (TestInterface)DuckTyping.Implement(typeof(TestInterface), o);
		}

		interface NonPublicInterface
		{
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void InvalidArgTest2()
		{
			NonPublicInterface duck  = (NonPublicInterface) DuckTyping.Implement(typeof(NonPublicInterface), new TestClass());
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void InvalidArgTest3()
		{
			Child1 duck  = (Child1)DuckTyping.Implement(typeof(Child1), new Child2());
		}

		[Test, ExpectedException(typeof(ArgumentException))]
		public void InvalidArgTest4()
		{
			TestInterface duck  = (TestInterface)DuckTyping.Implement(typeof(TestInterface), typeof(TestInterface), new TestClass());
		}

		#region Aggregate

		public interface IAggregatable
		{
			int Method1();
			int Method2();
		}

		public interface IClass1
		{
			int Method1();
			int Method3();
		}

		public interface IClass2
		{
			int Method2();
		}

		public class AggregateClass1: IClass1
		{
			public int Method1() { return 1; }
			public int Method3() { return 3; }
		}

		public class AggregateClass2: IClass2
		{
			public int Method2() { return 2; }
		}

		[Test]
		public void AggregateTest()
		{
			IAggregatable duck  = DuckTyping.Aggregate<IAggregatable>(new AggregateClass1(), new AggregateClass2());
			Assert.IsNotNull(duck);

			Assert.AreEqual(1, duck.Method1());
			Assert.AreEqual(2, duck.Method2());

			// It is still possible to get access
			// to an interface of an underlying object.
			//
			IClass2 cls2 = DuckTyping.Implement<IClass2>(duck);
			Assert.IsNotNull(cls2);
			Assert.AreEqual(2, cls2.Method2());

			// Even to switch from one aggregated object to another
			//
			IClass1 cls1 = DuckTyping.Implement<IClass1>(cls2);
			Assert.IsNotNull(cls1);
			Assert.AreEqual(1, cls1.Method1());
			Assert.AreEqual(3, cls1.Method3());
		}

		[Test]
		public void MissedMethodsAggregateTest()
		{
			IClass1 duck = DuckTyping.Aggregate<IClass1>(new Version(1,0), Guid.NewGuid());

			Assert.That(duck, Is.Not.Null);

			// Neither System.Guid nor System.Version will ever have Method1.
			//
			Assert.That(duck.Method1(), Is.EqualTo(0));
		}

		#endregion
	}
}
