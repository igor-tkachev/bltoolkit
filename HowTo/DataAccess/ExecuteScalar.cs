using System;

using NUnit.Framework;

using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ExecuteScalar
	{
		public abstract class /*[a]*/PersonAccessor/*[/a]*/ : /*[a]*/DataAccessor/*[/a]*/<Person>
		{
			[SqlQuery("SELECT Count(*) FROM Person")]
			public abstract int GetCount();

			// The Person_Insert sproc returns an id of the created record.
			//
			[SprocName("Person_Insert")]
			public abstract /*[a]*/int/*[/a]*/ Insert(Person person);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = DataAccessor.CreateInstance<PersonAccessor>();

			// ExecuteScalar.
			//
			Assert.IsTrue(pa.GetCount() > 0);

			// Insert and get id.
			//
			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			int id = pa./*[a]*/Insert(person)/*[/a]*/;

			Assert.IsFalse(id == 0);

			new SprocQuery<Person>().DeleteByKey(id);
		}
	}
}
