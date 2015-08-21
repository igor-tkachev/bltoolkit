using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLToolkit.Aspects;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Aspects
{
	[TestFixture]
	public class CacheAspectTest
	{
		[Log]
		public abstract class TestClass
		{
			public static int Value;

			[Cache(500, false)]
			public virtual int Test(int i1, int i2)
			{
				return Value;
			}

			[InstanceCache]
			public virtual int InstanceTest(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void Test()
		{
			TestClass t = TypeAccessor.CreateInstance<TestClass>();

			DateTime begin = DateTime.Now;

			for (TestClass.Value = 777; t.Test(2, 2) == 777; TestClass.Value++)
				continue;

			Assert.IsTrue((DateTime.Now - begin).TotalMilliseconds >= 500);

			TestClass.Value = 1; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, t.Test(1, 1));
			TestClass.Value = 3; Assert.AreEqual(3, t.Test(2, 1));

			CacheAspect.ClearCache(typeof(TestClass), "Test", typeof(int), typeof(int));
			TestClass.Value = 4; Assert.AreEqual(4, t.Test(2, 1));

			CacheAspect.ClearCache(t.GetType(), "Test", typeof(int), typeof(int));
			TestClass.Value = 5; Assert.AreEqual(5, t.Test(2, 1));

			CacheAspect.ClearCache();
			TestClass.Value = 6; Assert.AreEqual(6, t.Test(2, 1));
		}

		[Test]
		public void InstanceTest()
		{
			TestClass t = TypeAccessor.CreateInstance<TestClass>();

			TestClass.Value = 1; Assert.AreEqual(1, t.InstanceTest(1, 1));
			TestClass.Value = 2; Assert.AreEqual(1, t.InstanceTest(1, 1));
			TestClass.Value = 3; Assert.AreEqual(3, t.InstanceTest(2, 1));
			TestClass.Value = 4;

			t = TypeAccessor.CreateInstance<TestClass>();

			Assert.AreNotEqual(1, t.InstanceTest(1, 1));
			Assert.AreNotEqual(3, t.InstanceTest(2, 1));
		}

		public class CustomCacheAspect : CacheAspect
		{
			private static IDictionary _methodcache = new Hashtable();
			public  static IDictionary  MethodCache
			{
				get { return _methodcache; }
			}

			protected override IDictionary CreateCache()
			{
				return MethodCache;
			}
		}

		[Log]
		public abstract class CustomTestClass
		{
			public static int Value;

			[Cache(typeof(CustomCacheAspect), MaxCacheTime = 500)]
			public virtual int Test(int i1, int i2)
			{
				return Value;
			}
		}

		[Test]
		public void CustomCacheAspectTest()
		{
			CustomTestClass t = (CustomTestClass)TypeAccessor.CreateInstance(typeof(CustomTestClass));

			CustomTestClass.Value = 1; Assert.AreEqual(1, t.Test(1, 1));
			CustomTestClass.Value = 2; Assert.AreEqual(1, t.Test(1, 1));
			CustomTestClass.Value = 3; Assert.AreEqual(3, t.Test(2, 1));

			CustomCacheAspect.MethodCache.Clear();
			CustomTestClass.Value = 4; Assert.AreEqual(4, t.Test(2, 1));
		}

		public abstract class GenericClass<T>
			where T : new()
		{
			[Cache]
			public virtual T GetT(int i)
			{
				return new T();
			}
		}

		[Test]
		public void GenericClassTest()
		{
			TypeAccessor<GenericClass<int>>.     CreateInstance().GetT(0);
			TypeAccessor<GenericClass<DateTime>>.CreateInstance().GetT(0);
		}

		public abstract class TestClass1
		{
			[Cache]
			public virtual T Get<T>()
				where T : new()
			{
				return new T();
			}

			[Cache]
			public virtual T Get<T>(int i)
			{
				if (typeof(T) == typeof(int)) return (T)(object)10;

				return (T)(object)new DateTime();
			}
		}

		//[Test]
		public void GenericMethodTest()
		{
			var i = TypeAccessor<TestClass1>.CreateInstance().Get<int>     ();
			var d = TypeAccessor<TestClass1>.CreateInstance().Get<DateTime>();

			i = TypeAccessor<TestClass1>.CreateInstance().Get<int>     (0);
			d = TypeAccessor<TestClass1>.CreateInstance().Get<DateTime>(0);
		}


		public abstract class MultiThread
		{
			public static int Value;

			[Cache]
			public virtual int GetValue()
			{
				return Value++;
			}

			[Cache]
			public virtual int Throw()
			{
				Value++;
				throw new Exception("Dou!");
			}

			[ClearCache]
			public virtual void Clear()
			{
				Value = 0;
			}

		}

		[Test]
		public void OneThreadTest()
		{
			var o = TypeAccessor.CreateInstanceEx<MultiThread>();
			o.Clear();

			for (int i = 0; i < 10; i++)
				o.GetValue();

			Assert.AreEqual(1, MultiThread.Value);
		}


		[Test]
		public void MultiThreadTest()
		{
			var o = TypeAccessor.CreateInstanceEx<MultiThread>();
			o.Clear();

			Action<object> increment = (j) =>
			{
				Console.WriteLine("Start increment {0}", j);
				o.GetValue();
				Thread.Sleep(100);
				Console.WriteLine("End increment {0}", j);
			};

			var tasks = new List<Task>(10);

			for(int i = 0; i < 10; i++)
				tasks.Add(Task.Factory.StartNew(increment, i));

			Task.WaitAll(tasks.ToArray());

			Assert.AreEqual(1, MultiThread.Value);

		}

		[Test]
		public void MultiThreadTest10()
		{
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("Iteration {0}", i+1);
				MultiThread.Value = 0;
				Console.WriteLine("ClearCache");
				CacheAspect.ClearCache();
				Console.WriteLine("ClearCache - complete");
				MultiThreadTest();
			}
		}

		[Test]
		public void MultiThreadThrow()
		{
			MultiThread.Value = 0;
			var o = TypeAccessor.CreateInstanceEx<MultiThread>();
			Action<object> action = (n) =>
			{
				try
				{
					Console.WriteLine("Thread {1} Begin Throw {0}", n, Thread.CurrentThread.ManagedThreadId);
					o.Throw();
				}
				catch
				{
					Thread.Sleep(50);
					Console.WriteLine("Thread {1} End Throw {0}", n, Thread.CurrentThread.ManagedThreadId);
				}

			};

			var tasks = new List<Task>(10);

			for(int i = 0; i < 10; i++)
				tasks.Add(Task.Factory.StartNew(action, i));

			Task.WaitAll(tasks.ToArray());

			Assert.AreEqual(10, MultiThread.Value);

		}


		[Test]
		public void MultiThreadCacheClear()
		{
			var o = TypeAccessor.CreateInstanceEx<MultiThread>();
			o.Clear();

			Action<object> action = (j) =>
			{
				Console.WriteLine("Thread {1} Begin increment {0}", j, Thread.CurrentThread.ManagedThreadId);
				o.GetValue();
				Thread.Sleep(50);
				Console.WriteLine("Thread {1} End increment {0}", j, Thread.CurrentThread.ManagedThreadId);
			};

			Action<Object> clear = (k) =>
			{
				Console.WriteLine("Thread {1} Begin clear {0}", k, Thread.CurrentThread.ManagedThreadId);
				o.Clear();
				Thread.Sleep(50);
				Console.WriteLine("Thread {1} End clear {0}", k, Thread.CurrentThread.ManagedThreadId);
			};

			var tasks = new List<Task>(20);

			for(int i = 0; i < 20; i++)
				tasks.Add(i%2 == 0 ? Task.Factory.StartNew(action, i) : Task.Factory.StartNew(clear, i));

			var ok = Task.WaitAll(tasks.ToArray(), 25*50);
			Task.WaitAll(tasks.ToArray());
			Assert.True(ok);


		}
	}
}
