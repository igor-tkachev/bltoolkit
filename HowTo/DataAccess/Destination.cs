using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Destination
	{
		public abstract class PersonAccessor : DataAccessor
		{
			public abstract void SelectAll([/*[a]*/Destination/*[/a]*/] IList<Person> list);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			List<Person> list = new List<Person>();

			pa.SelectAll(list);

			Assert.AreNotEqual(0, list.Count);
		}
	}
}
