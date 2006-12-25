using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Insert
	{
		[Test]
		public void Test1()
		{
			/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			query./*[a]*/Insert(person)/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/SprocQuery<Person> query = new SprocQuery<Person>()/*[/a]*/;

				Person person = new Person();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Other;

				query./*[a]*/Insert(db, person)/*[/a]*/;
			}
		}
	}
}

