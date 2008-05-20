using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Format
	{
		public abstract class PersonAcessor : DataAccessor
		{
			[SqlQuery("SELECT TOP {0} * FROM Person")]
			public abstract List<Person> GetPersonList([/*[a]*/Format/*[/a]*/] int top);
		}

		[Test]
		public void Test()
		{
			PersonAcessor pa   = DataAccessor.CreateInstance<PersonAcessor>();
			List<Person>  list = pa.GetPersonList(2);

			Assert.LessOrEqual(list.Count, 2);
		}
	}
}
