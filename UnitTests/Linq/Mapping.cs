using System;
using System.Linq;
using BLToolkit.Mapping;
using NUnit.Framework;

using BLToolkit.DataAccess;

namespace Data.Linq
{
	using Model;

	[TestFixture]
	public class Mapping : TestBase
	{
		[Test]
		public void Enum1()
		{
			var expected = from p in Person where new[] { Gender.Male }.Contains(p.Gender) select p;
			ForEachProvider(db => AreEqual(expected, from p in db.Person where new[] { Gender.Male }.Contains(p.Gender) select p));
		}

		[Test]
		public void Enum2()
		{
			var expected = from p in Person where p.Gender == Gender.Male select p;
			ForEachProvider(db => AreEqual(expected, from p in db.Person where p.Gender == Gender.Male select p));
		}

		[Test]
		public void Enum3()
		{
			var fm = Gender.Female;

			var expected = from p in Person where p.Gender != fm select p;
			ForEachProvider(db => AreEqual(expected, from p in db.Person where p.Gender != fm select p));
		}

		[Test]
		public void EditableObject()
		{
			ForEachProvider(db =>
			{
				var e = (from p in db.GetTable<EditableParent>() where p.ParentID == 1 select p).First();
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value1);
			});
		}

		[TableName("Parent")]
		[MapField("Value1", "Value.Value1")]
		public class ParentObject
		{
			public int   ParentID;
			public Inner Value = new Inner();

			public class Inner
			{
				public int? Value1;
			}
		}

		[Test]
		public void Inner1()
		{
			ForEachProvider(db =>
			{
				var e = db.GetTable<ParentObject>().First(p => p.ParentID == 1);
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value.Value1);
			});
		}

		[Test]
		public void Inner2()
		{
			ForEachProvider(db =>
			{
				var e = db.GetTable<ParentObject>().First(p => p.ParentID == 1 && p.Value.Value1 == 1);
				Assert.AreEqual(1, e.ParentID);
				Assert.AreEqual(1, e.Value.Value1);
			});
		}
	}
}
