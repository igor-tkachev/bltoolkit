using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Reflection;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Destination
	{
		public abstract class PersonAcessor : DataAccessor
		{
			public abstract void SelectAll([/*[a]*/Destination/*[/a]*/] IList<Person> list);
		}

		[Test]
		public void Test()
		{
			PersonAcessor pa = TypeAccessor<PersonAcessor>.CreateInstance();

			List<Person> list = new List<Person>();

			pa.SelectAll(list);

			Assert.AreNotEqual(0, list.Count);
		}
	}
}
