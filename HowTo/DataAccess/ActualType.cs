using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ActualType
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

		[/*[a]*/ActualType/*[/a]*/(typeof(IName), typeof(/*[a]*/Name1/*[/a]*/))]
		public abstract class TestAccessor : DataAccessor
		{
			[SqlQuery("SELECT 'John' as Name")]
			public abstract IName GetName1();

			[SqlQuery("SELECT 'John' as Name"), /*[a]*/ObjectType/*[/a]*/(typeof(/*[a]*/Name2/*[/a]*/))]
			public abstract IName GetName2();

			[SqlQuery("SELECT 'John' as Name")]
			public abstract IList<IName> GetName1List();

			[SqlQuery("SELECT 'John' as Name"), /*[a]*/ObjectType/*[/a]*/(typeof(/*[a]*/Name2/*[/a]*/))]
			public abstract IList<IName> GetName2List();

			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID")]
			public abstract IDictionary<int, IName> GetName1Dictionary();

			[SqlQuery("SELECT 1 as ID, 'John' as Name"), Index("@ID"), /*[a]*/ObjectType/*[/a]*/(typeof(/*[a]*/Name2/*[/a]*/))]
			public abstract IDictionary<int, IName> GetName2Dictionary();
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();

			Assert.IsTrue(ta.GetName1()              is Name1);
			Assert.IsTrue(ta.GetName2()              is Name2);
			Assert.IsTrue(ta.GetName1List()[0]       is Name1);
			Assert.IsTrue(ta.GetName2List()[0]       is Name2);
			Assert.IsTrue(ta.GetName1Dictionary()[1] is Name1);
			Assert.IsTrue(ta.GetName2Dictionary()[1] is Name2);
		}
	}
}
