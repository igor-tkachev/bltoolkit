using System;

using NUnit.Framework;

using BLToolkit.Reflection;

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
		public class Object : Base, IObject2, IObject1, IBase2
		{
			public new void Method2() {}
		}

		[Test]
		public void GetAttributes()
		{
			object[] attrs = new TypeHelper(typeof(Object)).GetAttributes();

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
			object[] attrs = new TypeHelper(typeof(Object)).GetAttributes(typeof(Attribute2));

			foreach (object attr in attrs)
				Console.WriteLine("{0} {1}", attr, attr.GetHashCode());

			Assert.AreEqual(typeof(Attribute5), attrs[0].GetType());
			Assert.AreEqual(typeof(Attribute5), attrs[1].GetType());

			Assert.IsTrue(
				typeof(Attribute2) == attrs[2].GetType() && typeof(Attribute5) == attrs[3].GetType() ||
				typeof(Attribute2) == attrs[3].GetType() && typeof(Attribute5) == attrs[2].GetType());
		}
	}
}
