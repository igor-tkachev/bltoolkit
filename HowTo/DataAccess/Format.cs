using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.DataAccess;
using NUnit.Framework.SyntaxHelpers;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Format
	{
		public abstract class PersonAccessor : DataAccessor
		{
			[SqlQuery("SELECT TOP {0} * FROM Person")]
			public abstract List<Person> GetPersonList([/*[a]*/Format/*[/a]*/] int top);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa   = DataAccessor.CreateInstance<PersonAccessor>();
			List<Person>   list = pa.GetPersonList(2);

			Assert.That(list,       Is.Not.Null);
			Assert.That(list.Count, Is.LessThanOrEqualTo(2));
		}
	}
}
