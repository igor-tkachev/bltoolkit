using System;
using System.Collections;
#if FW2
using System.Collections.Generic;
#endif
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

namespace Reflection
{
	[TestFixture]
	public class TypeHellperTest
	{
		public class Attribute1 : Attribute {}
		public class Attribute2 : Attribute {}
		public class Attribute3 : Attribute {}
		public class Attribute4 : Attribute {}

		public class Attribute5 : Attribute2
		{
			public Attribute5(int n)
			{
				_n = n;
			}

			int _n;

			public override string ToString()
			{
				return base.ToString() + ":" + _n;
			}
		}

		[Attribute1]
		[Attribute5(2)]
		public interface IBase1
		{
			void Method1();
		}

		[Attribute5(1)]
		public interface IBase2
		{
			void Method2();
		}

		[Attribute2]
		[Attribute5(0)]
		public class Base : IBase1, IBase2
		{
			public void Method1() {}
			public void Method2() {}
		}

		[Attribute3]
		public interface IObject1 : IBase1
		{
		}

		[Attribute4]
		public interface IObject2 : IBase1, IBase2
		{
		}

		[Attribute5(2)]
		public class TestObject : Base, IObject2, IObject1, IBase2
		{
			public new void Method2() {}
		}

#if FW2

		public class TestObject<T> : TestObject
		{
			public T Method2(T value)    { return value;      }
			public V Method2<V>()        { return default(V); }
			
			public V Method3<V>(V value) where V : IConvertible, IFormattable, new()
			{
				 return value;
			}
		}

#endif

		[Test]
		public void GetAttributes()
		{
			object[] attrs = new TypeHelper(typeof(TestObject)).GetAttributes();

			foreach (object attr in attrs)
				Console.WriteLine("{0} {1}", attr, attr.GetHashCode());

			Assert.AreEqual(typeof(Attribute5), attrs[0].GetType());
			Assert.AreEqual(typeof(Attribute5), attrs[1].GetType());
			Assert.AreEqual(typeof(Attribute4), attrs[2].GetType());
			Assert.AreEqual(typeof(Attribute3), attrs[3].GetType());

			Assert.IsTrue(
				typeof(Attribute2) == attrs[4].GetType() && typeof(Attribute5) == attrs[5].GetType() ||
				typeof(Attribute2) == attrs[5].GetType() && typeof(Attribute5) == attrs[4].GetType());

			Assert.AreEqual(typeof(Attribute1), attrs[6].GetType());
		}

		[Test]
		public void GetAttributes_ByType()
		{
			object[] attrs = new TypeHelper(typeof(TestObject)).GetAttributes(typeof(Attribute2));

			foreach (object attr in attrs)
				Console.WriteLine("{0} {1}", attr, attr.GetHashCode());

			Assert.AreEqual(typeof(Attribute5), attrs[0].GetType());
			Assert.AreEqual(typeof(Attribute5), attrs[1].GetType());

			Assert.IsTrue(
				typeof(Attribute2) == attrs[2].GetType() && typeof(Attribute5) == attrs[3].GetType() ||
				typeof(Attribute2) == attrs[3].GetType() && typeof(Attribute5) == attrs[2].GetType());
		}

#if FW2
		[Test]
		public void UnderlyingTypeTest()
		{
			Type type;

			type = TypeHelper.GetUnderlyingType(typeof(int?));
			Assert.AreEqual(typeof(int), type);

			type = TypeHelper.GetUnderlyingType(typeof(DayOfWeek?));
			Assert.AreEqual(typeof(int), type);

			type = TypeHelper.GetUnderlyingType(typeof(IComparable<int>));
			Assert.AreEqual(typeof(IComparable<int>), type);

		}

		[Test]
		public void GenericsTest()
		{
			Type testType = typeof (TestObject<int>);
			TypeHelper helper = new TypeHelper(testType);

			Assert.IsNotNull(helper.GetMethod(true, "Method2"));  // Generic
			Assert.IsNotNull(helper.GetMethod(false, "Method2")); // Non-generic

			// TestObject<T>.Method2<V>() is a generic method
			Assert.IsNotNull(helper.GetMethod(true, "Method2", Type.EmptyTypes));
			// TestObject.Method2() is non
			Assert.IsNotNull(helper.GetMethod(false, "Method2", Type.EmptyTypes));
			// TestObject<T>.Method2(T value) is neither!
			Assert.IsNotNull(helper.GetMethod(false, "Method2", testType.GetGenericArguments()[0]));
			// typeof(int) is same as testType.GetGenericArguments()[0]
			Assert.IsNotNull(helper.GetMethod(false, "Method2", typeof(int)));
			// Get TestObject<T>.Method3<V>() with constraint type hack
			Assert.IsNotNull(helper.GetMethod(true, "Method3", typeof(int)));
			// Get TestObject<T>.Method3<V>() with constraint violation type will fail.
			Assert.IsNull(helper.GetMethod(true, "Method3", typeof(object)));
			// Get TestObject<T>.Method3<V>() with no types will fail
			Assert.IsNull(helper.GetMethod(true, "Method3", Type.EmptyTypes));

			Assert.AreEqual(helper.GetMethods().Length, 12);      // 12 total
			Assert.AreEqual(helper.GetMethods(true).Length, 2);   // 2 generic
			Assert.AreEqual(helper.GetMethods(false).Length, 10); // 10 non-generics

		}

		[Test, ExpectedException(typeof(AmbiguousMatchException))]
		public void GenericsAmbigousMatchTest()
		{
			// There are 
			new TypeHelper(typeof (TestObject<int>)).GetMethod("Method2");
		}
#endif

		class MyArrayList : ArrayList
		{
			public new TestObject this[int i]
			{
				get { return (TestObject)base[i]; }
			}
		}

		[Test]
		public void GetListItemType()
		{
			Assert.AreEqual(typeof(TestObject), TypeHelper.GetListItemType(new EditableArrayList(typeof(TestObject))));
			Assert.AreEqual(typeof(TestObject), TypeHelper.GetListItemType(new TestObject[0]));
			Assert.AreEqual(typeof(TestObject), TypeHelper.GetListItemType(new MyArrayList()));
#if FW2
			Assert.AreEqual(typeof(TestObject), TypeHelper.GetListItemType(new List<TestObject>()));
#endif
		}
	}
}

