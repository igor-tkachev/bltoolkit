using System;

using NUnit.Framework;

using Rsdn.Framework.Data.Mapping;

namespace CS
{
	namespace Test1
	{
		public interface IAction1
		{
			void DoAction();
		}
	}

	namespace Test2
	{
		public interface IAction1
		{
			void DoAction();
		}
	}

	[TestFixture]
	public class ActionTest
	{

		public interface IAction2
		{
			void DoAction();
			void DoAction(int p1, [MapParent] EntityBase entity);
			bool Property { get; }
		}

		public interface ISetInfo
		{
			void SetInfo(int i, [MapPropertyInfo] MapPropertyInfo info, byte b);
		}

		public struct MyInt : IAction2
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

		public struct MyString : Test2.IAction1, Test1.IAction1, ISetInfo
		{
			public string Value;

			public void DoAction()
			{
				Value = "Test1.IAction1.DoAction";
			}

			void Test2.IAction1.DoAction()
			{
				Value = "Test2.IAction1.DoAction";
			}
		
			public void SetInfo(int i, MapPropertyInfo info, byte b)
			{
				Value = info.PropertyName;
			}
		}

		public class MyDateTime : IAction2
		{
			public DateTime Value;

			public static DateTime TestDate1 = new DateTime(2000, 10, 10);
			public static DateTime TestDate2 = new DateTime(2001, 11, 11);

			public void DoAction()
			{
				Value = TestDate1;
			}
			
			void IAction2.DoAction(int p1, EntityBase entity)
			{
				Value = TestDate2;
				entity.CallCounter++;
			}

			bool IAction2.Property 
			{
				get { return true; } 
			}
		}

		[MapAction(typeof(Test2.IAction1))]
		[MapAction(typeof(Test1.IAction1))]
		[MapType(typeof(int),    typeof(MyInt))]
		[MapType(typeof(string), typeof(MyString))]
		public abstract class EntityBase
		{
			public int CallCounter = 0;
		}

		[MapAction(typeof(IAction2))]
		[MapAction(typeof(ISetInfo))]
		[MapType(typeof(DateTime), typeof(MyDateTime))]
		public abstract class Entity : EntityBase
		{
			public abstract int      Int  { get; set; }
			public abstract DateTime Date { get; set; }
			public abstract string   Str  { get; set; }
		}

		#region GenTest
		#endregion

		[Test]
		public void TestAction()
		{
			Entity e = (Entity)Map.Descriptor(typeof(Entity)).CreateInstance();

			Test2.IAction1 t2a = (Test2.IAction1)e;
			t2a.DoAction();
			// because of boxing  :(
			Assert.AreEqual(null /*"Test2.IAction1.DoAction"*/, e.Str); 

			Test1.IAction1 t1a = (Test1.IAction1)e;
			t1a.DoAction();
			Assert.AreEqual("Test1.IAction1.DoAction", e.Str); 

			IAction2 a2 = (IAction2)e;
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

		public interface IReturnTrueAction
		{
			bool IsDirty { [return: MapReturnIfTrue] get; }
		}

		public interface IReturnFalseAction
		{
			[return: MapReturnIfFalse] bool DoFalse();
		}

		public interface IReturnNullAction
		{
			[return: MapReturnIfNull] object DoNull();
		}

		public struct Value1 : IReturnTrueAction, IReturnFalseAction, IReturnNullAction
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

		public class Value2 : IReturnTrueAction, IReturnFalseAction, IReturnNullAction
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

		public class Value3 : IReturnTrueAction, IReturnFalseAction, IReturnNullAction
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

		[MapAction(typeof(IReturnTrueAction))]
		[MapAction(typeof(IReturnFalseAction))]
		[MapAction(typeof(IReturnNullAction))]
		public abstract class ReturnEntity
		{
			[MapType(typeof(Value1))] public abstract int     Value1 { get; set; }
			[MapType(typeof(Value2))] public abstract decimal Value2 { get; set; }
			[MapType(typeof(Value3))] public abstract string  Value3 { get; set; }
		}

		[Test]
		public void TestTrueReturn()
		{
			ReturnEntity e = (ReturnEntity)Map.Descriptor(typeof(ReturnEntity)).CreateInstance();

			bool b = ((IReturnTrueAction)e).IsDirty;

			Assert.AreEqual(100,  e.Value1);
			Assert.AreEqual(100M, e.Value2);
			Assert.AreEqual(null, e.Value3);
		}

		[Test]
		public void TestFalseReturn()
		{
			ReturnEntity e = (ReturnEntity)Map.Descriptor(typeof(ReturnEntity)).CreateInstance();

			((IReturnFalseAction)e).DoFalse();

			Assert.AreEqual(200,   e.Value1);
			Assert.AreEqual(200M,  e.Value2);
			Assert.AreEqual("200", e.Value3);
		}

		[Test]
		public void TestNullReturn()
		{
			ReturnEntity e = (ReturnEntity)Map.Descriptor(typeof(ReturnEntity)).CreateInstance();

			((IReturnNullAction)e).DoNull();

			Assert.AreEqual(300,  e.Value1);
			Assert.AreEqual(0M,   e.Value2);
			Assert.AreEqual(null, e.Value3);
		}
	}
}
