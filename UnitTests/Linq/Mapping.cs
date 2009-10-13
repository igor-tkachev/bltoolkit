using System;
using System.Linq;

using NUnit.Framework;

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
	}
}
