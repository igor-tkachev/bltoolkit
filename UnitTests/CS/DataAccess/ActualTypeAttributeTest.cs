using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace DataAccess
{
	[TestFixture]
	public class ActualTypeAttributeTest
	{
		public interface IName
		{
			string Name { get; }
		}

		public class NameBase : IName
		{
			private string _name;
			public  string  Name { get { return _name; } set { _name = value; } }
		}

		public class Name1 : NameBase {}
		public class Name2 : NameBase {}

		public interface IValue
		{
			string Value { get; }
		}

		public class MyValue : IValue
		{
			private string _value;
			public  string  Value { get { return _value; } set { _value = value; } }
		}

		[ActualType(typeof(IName),  typeof(Name1))]
		[ActualType(typeof(IValue), typeof(MyValue))]
		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT 'John' as Name")]
			public abstract IName GetName();

			[SqlQuery("SELECT 'John' as Name"), ObjectType(typeof(Name2))]
			public abstract IName GetName2();

			[SqlQuery("SELECT 'John' as Name")]
			public abstract IList<IName> GetNameList();

			[SqlQuery("SELECT 'John' as Name"), ObjectType(typeof(Name2))]
			public abstract IList<IName> GetName2List();

			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID")]
			public abstract IDictionary<int, IName> GetNameDictionary();

			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID"), ObjectType(typeof(Name2))]
			public abstract IDictionary<int, IName> GetName2Dictionary();

			[SqlQuery("SELECT 'John' as Value")]
			public abstract IValue GetValue();
		}

		TestAccessor Accessor
		{
			get { return TypeAccessor.CreateInstance<TestAccessor>(); }
		}

		[Test]
		public void TestName()
		{
			IName name = Accessor.GetName();
			Assert.IsTrue(name is Name1);
		}

		[Test]
		public void TestName2()
		{
			IName name = Accessor.GetName2();
			Assert.IsTrue(name is Name2);
		}

		[Test]
		public void TestNameList()
		{
			IList<IName> list = Accessor.GetNameList();
			Assert.IsTrue(list[0] is Name1);
		}

		[Test]
		public void TestName2List()
		{
			IList<IName> list = Accessor.GetName2List();
			Assert.IsTrue(list[0] is Name2);
		}

		[Test]
		public void TestNameDictionary()
		{
			IDictionary<int, IName> dic = Accessor.GetNameDictionary();
			Assert.IsTrue(dic[1] is Name1);
		}

		[Test]
		public void TestName2Dictionary()
		{
			IDictionary<int, IName> dic = Accessor.GetName2Dictionary();
			Assert.IsTrue(dic[1] is Name2);
		}

		[Test]
		public void TestValue()
		{
			IValue value = Accessor.GetValue();
			Assert.AreEqual(value.Value, "John");
		}
	}
}
