using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;

using BLToolkit.Reflection;
using BLToolkit.EditableObjects;

namespace Reflection
{
	[TestFixture]
	public class TypeHelperTest
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

		public struct TestStruct<T,V>
		{
			public T _t;
			public V _v;
		}

		public class TestObject<T> : TestObject
		{
			public T Method2(T value)
			{
				 return value;
			}

			public V Method2<V>()
			{
				 return default(V);
			}
			
			public V Method3<V>(V value)
				where V : IConvertible, IFormattable, new()
			{
				 return value;
			}

			public V Method3<V>(Nullable<V> value)
				where V : struct, IFormattable
			{
				 return value.Value;
			}

			public V Method3<V, Q>(TestStruct<Nullable<V>, Q> value)
				where V : struct
				where Q : IFormattable
			{
				return default(V);
			}

			public V Method4<V, Q>(TestStruct<V, Q> value)
			{
				return default(V);
			}
		}

		[Test]
		public void GetAttributes()
		{
			object[] attrs = new TypeHelper(typeof(TestObject)).GetAttributes();

			for (int i = 0; i < attrs.Length; i++)
			{
				object attr = attrs[i];
				Console.WriteLine("{0}: {1} {2}", i, attr, attr.GetHashCode());
			}

			Assert.AreEqual(typeof(Attribute5), attrs[0].GetType());
			Assert.AreEqual(typeof(Attribute5), attrs[1].GetType());
			Assert.AreEqual(typeof(Attribute4), attrs[2].GetType());
			Assert.AreEqual(typeof(Attribute3), attrs[3].GetType());

			Assert.IsTrue(
				typeof(Attribute2) == attrs[4].GetType() && typeof(Attribute5) == attrs[5].GetType() ||
				typeof(Attribute2) == attrs[5].GetType() && typeof(Attribute5) == attrs[4].GetType());

			Assert.IsTrue(
				typeof(Attribute1) == attrs[6].GetType() && typeof(Attribute5) == attrs[7].GetType() ||
				typeof(Attribute1) == attrs[7].GetType() && typeof(Attribute5) == attrs[6].GetType());

			Assert.AreEqual(typeof(Attribute5), attrs[8].GetType());
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

			Assert.IsNotNull(helper.GetMethod(true,  "Method2")); // Generic
			Assert.IsNotNull(helper.GetMethod(false, "Method2")); // Non-generic

			// TestObject<T>.Method2<V>() is a generic method
			Assert.IsNotNull(helper.GetMethod(true,  "Method2", Type.EmptyTypes));
			// TestObject.Method2() is not
			Assert.IsNotNull(helper.GetMethod(false, "Method2", Type.EmptyTypes));
			// TestObject<T>.Method2(T value) is neither!
			Assert.IsNotNull(helper.GetMethod(false, "Method2", testType.GetGenericArguments()[0]));
			// typeof(int) is same as testType.GetGenericArguments()[0]
			Assert.IsNotNull(helper.GetMethod(false, "Method2", typeof(int)));
			// Get TestObject<T>.Method3<V>() with constraint type hack
			Assert.IsNotNull(helper.GetMethod(true,  "Method3", typeof(int)));
			// Get TestObject<T>.Method3<V>() with constraint violation will fail.
			Assert.IsNull   (helper.GetMethod(true,  "Method3", typeof(object)));
			// Get TestObject<T>.Method3<V>() with no types will fail
			Assert.IsNull   (helper.GetMethod(true,  "Method3", Type.EmptyTypes));
			// Nullable<> violates IFormattable constraint
			Assert.IsNull   (helper.GetMethod(true,  "Method3", typeof(Nullable<>)));
			// Method4 does not define a costraint
			Assert.IsNotNull(helper.GetMethod(true,  "Method4", typeof(TestStruct<,>)));

			Assert.IsNotNull(helper.GetMethod(true,  "Method3", typeof(Nullable<int>)));
			Assert.IsNotNull(helper.GetMethod(true,  "Method3", typeof(TestStruct<Nullable<int>, int>)));
			Assert.IsNull   (helper.GetMethod(true,  "Method3", typeof(TestStruct<Nullable<int>, object>)));
			Assert.IsNull   (helper.GetMethod(true,  "Method3", typeof(TestStruct<int, int>)));

			Assert.AreEqual (helper.GetMethods().Length,     15);  // 15 total
			Assert.AreEqual (helper.GetMethods(true).Length,  5);  //  5 generic
			Assert.AreEqual (helper.GetMethods(false).Length, 10); // 10 non-generics

		}

		[Test, ExpectedException(typeof(AmbiguousMatchException))]
		public void GenericsAmbiguousMatchTest()
		{
			// There are more then one Method2 in the TestObject<T> class
			new TypeHelper(typeof (TestObject<int>)).GetMethod("Method2");
		}

		public class MyArrayList : ArrayList
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
			Assert.AreEqual(typeof(TestObject), TypeHelper.GetListItemType(new List<TestObject>()));
		}
	}
}

