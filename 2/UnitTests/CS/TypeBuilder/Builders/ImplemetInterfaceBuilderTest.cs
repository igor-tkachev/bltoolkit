using System;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.TypeBuilder;
using BLToolkit.Reflection;

namespace TypeBuilder.Builders
{
	namespace Test1
	{
		public interface IInterface1
		{
			void DoAction();
		}
	}

	namespace Test2
	{
		public interface IInterface1
		{
			void DoAction();
		}
	}

	[TestFixture]
	public class ImplemetInterfaceBuilderTest
	{

		public interface IInterface2
		{
			void DoAction();
			void DoAction(int p1, [Parent] EntityBase entity);
			bool Property { get; }
		}

		public interface ISetInfo
		{
			void SetInfo(int i, [PropertyInfo] PropertyInfo info, byte b);
		}

		public struct MyInt : IInterface2
		{
			public int Value;

			public void DoAction()
			{
				Value = 123;
			}
			
			public void DoAction(int p1, EntityBase entity)
			{
				Value = p1;
				entity.CallCounter++;
			}

			public bool Property 
			{
				get { return true; } 
			}
		}

		public struct MyString : Test2.IInterface1, Test1.IInterface1, ISetInfo
		{
			public string Value;

			public void DoAction()
			{
				Value = "Test1.IAction1.DoAction";
			}

			void Test2.IInterface1.DoAction()
			{
				Value = "Test2.IAction1.DoAction";
			}
		
			public void SetInfo(int i, PropertyInfo info, byte b)
			{
				Value = info.Name;
			}
		}

		public class MyDateTime : IInterface2
		{
			public DateTime Value;

			public static DateTime TestDate1 = new DateTime(2000, 10, 10);
			public static DateTime TestDate2 = new DateTime(2001, 11, 11);

			public void DoAction()
			{
				Value = TestDate1;
			}
			
			void IInterface2.DoAction(int p1, EntityBase entity)
			{
				Value = TestDate2;
				entity.CallCounter++;
			}

			bool IInterface2.Property 
			{
				get { return true; } 
			}
		}

		[ImplementInterface(typeof(Test2.IInterface1))]
		[ImplementInterface(typeof(Test1.IInterface1))]
		[GlobalInstanceType(typeof(int),    typeof(MyInt))]
		[GlobalInstanceType(typeof(string), typeof(MyString))]
		public abstract class EntityBase
		{
			public int CallCounter = 0;
		}

		[ImplementInterface(typeof(IInterface2))]
		[ImplementInterface(typeof(ISetInfo))]
		[GlobalInstanceType(typeof(DateTime), typeof(MyDateTime))]
		public abstract class Entity : EntityBase
		{
			public abstract int      Int  { get; set; }
			[LazyInstance]
			public abstract DateTime Date { get; set; }
			public abstract string   Str  { get; set; }
		}

		[Test]
		public void Test()
		{
			Entity e = (Entity)TypeAccessor.CreateInstance(typeof(Entity));

			Test2.IInterface1 t2a = (Test2.IInterface1)e;
			t2a.DoAction();
			// because of boxing  :(
			Assert.AreEqual(null /*"Test2.IAction1.DoAction"*/, e.Str); 

			Test1.IInterface1 t1a = (Test1.IInterface1)e;
			t1a.DoAction();
			Assert.AreEqual("Test1.IAction1.DoAction", e.Str); 

			IInterface2 a2 = (IInterface2)e;
			a2.DoAction();
			Assert.AreEqual(123,                  e.Int); 
			Assert.AreEqual(MyDateTime.TestDate1, e.Date);

			a2.DoAction(456, null);
			Assert.AreEqual(456,                  e.Int); 
			Assert.AreEqual(MyDateTime.TestDate2, e.Date);
			Assert.AreEqual(2,                    e.CallCounter);

			ISetInfo si = (ISetInfo)e;
			si.SetInfo(1, null, 2);
			Assert.AreEqual("Str", e.Str); 
		}

		public interface IReturnTrueInterface
		{
			bool IsDirty { [return: ReturnIfTrue] get; }
		}

		public interface IReturnFalseInterface
		{
			[return: ReturnIfFalse] bool DoFalse();
		}

		public interface IReturnNullInterface
		{
			[return: ReturnIfNull] object DoNull();
		}

		public struct Value1 : IReturnTrueInterface, IReturnFalseInterface, IReturnNullInterface
		{
			public int Value;

			public bool IsDirty
			{
				get { Value = 100; return false; } 
			}

			public bool DoFalse()
			{
				Value = 200;
				return true;
			}

			public object DoNull()
			{
				Value = 300;
				return null;
			}
		}

		public class Value2 : IReturnTrueInterface, IReturnFalseInterface, IReturnNullInterface
		{
			public decimal Value;

			public bool IsDirty 
			{
				get { Value = 100; return true; } 
			}

			public bool DoFalse()
			{
				Value = 200;
				return true;
			}

			public object DoNull()
			{
				Value = 300;
				return null;
			}
		}

		public class Value3 : IReturnTrueInterface, IReturnFalseInterface, IReturnNullInterface
		{
			public string Value;

			public bool IsDirty 
			{
				get { Value = "100"; return true; } 
			}

			public bool DoFalse()
			{
				Value = "200";
				return false;
			}
		
			public object DoNull()
			{
				Value = "300";
				return null;
			}
		}

		[ImplementInterface(typeof(IReturnTrueInterface))]
		[ImplementInterface(typeof(IReturnFalseInterface))]
		[ImplementInterface(typeof(IReturnNullInterface))]
		public abstract class ReturnEntity
		{
			[InstanceType(typeof(Value1))] public abstract int     Value1 { get; set; }
			[InstanceType(typeof(Value2))] public abstract decimal Value2 { get; set; }
			[InstanceType(typeof(Value3))] public abstract string  Value3 { get; set; }
		}

		[Test]
		public void TestTrueReturn()
		{
			ReturnEntity e = (ReturnEntity)TypeAccessor.CreateInstance(typeof(ReturnEntity));

			bool b = ((IReturnTrueInterface)e).IsDirty;

			Assert.AreEqual(100,  e.Value1);
			Assert.AreEqual(100M, e.Value2);
			Assert.AreEqual(null, e.Value3);
		}

		[Test]
		public void TestFalseReturn()
		{
			ReturnEntity e = (ReturnEntity)TypeAccessor.CreateInstance(typeof(ReturnEntity));

			((IReturnFalseInterface)e).DoFalse();

			Assert.AreEqual(200,   e.Value1);
			Assert.AreEqual(200M,  e.Value2);
			Assert.AreEqual("200", e.Value3);
		}

		[Test]
		public void TestNullReturn()
		{
			ReturnEntity e = (ReturnEntity)TypeAccessor.CreateInstance(typeof(ReturnEntity));

			((IReturnNullInterface)e).DoNull();

			Assert.AreEqual(300,  e.Value1);
			Assert.AreEqual(0M,   e.Value2);
			Assert.AreEqual(null, e.Value3);
		}

		[ImplementInterface(typeof(IReturnNullInterface))]
		public abstract class NullValue
		{
			public abstract Value2 Value { get; set; }
		}

		[Test]
		public void TestNull()
		{
			NullValue v = (NullValue)TypeAccessor.CreateInstance(typeof(NullValue));

			v.Value = null;

			((IReturnNullInterface)v).DoNull();
		}
	}
}

