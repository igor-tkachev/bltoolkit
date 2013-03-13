using System;
using System.Collections.Generic;

using BLToolkit.ComponentModel;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

using NUnit.Framework;

namespace Reflection
{
	[TestFixture]
	public class TypeAccessorTest3
	{
		class TestObject
		{
			private int _field = 15;
			public  int  Field
			{
				get { return _field; }
			}
		}

		[Test]
		public void Test()
		{
			var o  = TypeAccessor<TestObject>.CreateInstance();
			var ma = ExprMemberAccessor.GetMemberAccessor(TypeAccessor<TestObject>.Instance, "_field");

			ma.SetInt32(o, 5);

			Assert.AreEqual(5, o.Field);
		}

		[Test]
		public void TestAnonymous()
		{
			var o = new { Field1 = 1 };
			var a = TypeAccessor.GetAccessor(o);

			Assert.AreEqual(1, a["Field1"].GetInt32(o));
		}

		[MapField("MasterId", "Master.MasterId")]
		public interface IMaster2
		{
			[Relation]
			Master2 Master
			{
				get;
				set;
			}
		}

		[TableName("Master")]
		public abstract class Master2
		{
			[PrimaryKey]
			public abstract int           MasterId  { get; set; }
			//[Relation(Destination=typeof(Detail2))]
			public abstract List<Detail2> Details   { get; set; }
			public abstract string        Name      { get; set; }
		}

		[TableName("Detail")]
		public abstract class Detail2 : IMaster2
		{
			[PrimaryKey, MapField("Id")]
			public abstract int           DetailId  { get; set; }
			public abstract Master2       Master    { get; set; }
		}

		[Test]
		public void InterfaceTest()
		{
			var ta  = TypeAccessor<IMaster2>.Instance;
			var ta2 = TypeAccessor<Detail2>.Instance;
			var d2  = TypeAccessor<Detail2>.CreateInstance();

			var m = ta2["Master"].GetValue(d2); // OK
			Assert.IsNotNull(m);

			m = ta["Master"].GetValue(d2); // Exception 
			Assert.IsNotNull(m);
		}

		[Test]
		public void TestITypeDescriptionProviderGetAttributes()
		{
			var ta = TypeAccessor<Detail2>.Instance;
			var typeDescriptionProvider = ta as ITypeDescriptionProvider;
			Assert.IsNotNull(typeDescriptionProvider);

			typeDescriptionProvider.GetAttributes();
		}

		[Test]
		public void ListTest()
		{
			var t = TypeAccessor.CreateInstanceEx<List<int>>();
		}
	}
}
