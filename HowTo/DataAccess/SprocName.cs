using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class SprocName
	{
		public abstract class TestAccessor : DataAccessor
		{
			[/*[a]*/SprocName/*[/a]*/("Person_SelectAll")]
			public abstract List<Person> GetPersonList();
		}

		[Test]
		public void Test()
		{
			TestAccessor ta = DataAccessor.CreateInstance<TestAccessor>();

			List<Person> list = ta.GetPersonList();

			Assert.AreNotEqual(0, list.Count);
		}
	}
}
