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
			string StringValue { get; }
		}

		public class MyValue : IValue
		{
			private string _stringValue;
			public  string  StringValue { get { return _stringValue; } set { _stringValue = value; } }
		}

		[ActualType(typeof(IName),  typeof(Name1))]
		[ActualType(typeof(IValue), typeof(MyValue))]
		public abstract class TestAccessor : DataAccessor
		{
#if FIREBIRD
			[SqlQuery("SELECT 'John' as Name FROM Dual")]
#else
			[SqlQuery("SELECT 'John' as Name")]
#endif
			public abstract IName GetName();

#if FIREBIRD
			[SqlQuery("SELECT 'John' as Name FROM Dual"), ObjectType(typeof(Name2))]
#else
			[SqlQuery("SELECT 'John' as Name"), ObjectType(typeof(Name2))]
#endif
			public abstract IName GetName2();

#if FIREBIRD
			[SqlQuery("SELECT 'John' as Name FROM Dual")]
#else
			[SqlQuery("SELECT 'John' as Name")]
#endif
			public abstract IList<IName> GetNameList();

#if FIREBIRD
			[SqlQuery("SELECT 'John' as Name FROM Dual"), ObjectType(typeof(Name2))]
#else
			[SqlQuery("SELECT 'John' as Name"), ObjectType(typeof(Name2))]
#endif
			public abstract IList<IName> GetName2List();

#if FIREBIRD
			[SqlQuery("SELECT 1 as ID, 'John' as Name FROM Dual"), Index("@ID")]
#else
			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID")]
#endif
			public abstract IDictionary<int, IName> GetNameDictionary();

#if FIREBIRD
			[SqlQuery("SELECT 1 as ID, 'John' as Name FROM Dual"), Index("@ID"), ObjectType(typeof(Name2))]
#else
			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID"), ObjectType(typeof(Name2))]
#endif
			public abstract IDictionary<int, IName> GetName2Dictionary();

#if FIREBIRD
			[SqlQuery("SELECT 'John' as StringValue FROM Dual")]
#else
			[SqlQuery("SELECT 'John' as StringValue")]
#endif
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
			Assert.AreEqual(value.StringValue, "John");
		}
	}
}
