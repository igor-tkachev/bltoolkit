using System;

using NUnit.Framework;

using BLToolkit.Patterns;

namespace Patterns
{
	[TestFixture]
	public class DuckTypingTest
	{
		public DuckTypingTest()
		{
			BLToolkit.TypeBuilder.TypeFactory.SaveTypes = true;
		}

		public interface TestInterface
		{
			int  Method(int value);
			void Method(int v1, out int v2);

			int Prop { get; }

			event EventHandler Event;

			void CallEvent();
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
#if FW2
			TestInterface duck = DuckTyping.Implement<TestInterface>(new TestClass());
#else
			TestInterface duck = (TestInterface)DuckTyping.Implement(typeof(TestInterface), new TestClass());
#endif

			int value;
			duck.Method(33, out value);

			Assert.AreEqual(33, value);
			Assert.AreEqual(42, duck.Method(40));
			Assert.AreEqual(22, duck.Prop);

			duck.Event += new EventHandler(duck_Event);

			duck.CallEvent();

			Assert.AreEqual(55, eventValue);
		}

		int eventValue;

		void duck_Event(object sender, EventArgs e)
		{
			eventValue = 55;
		}
	}
}
