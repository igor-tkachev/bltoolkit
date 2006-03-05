using System;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class Insert
	{
		[Test]
		public void Test1()
		{
			/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

			Person person = new Person();

			person.FirstName = "Crazy";
			person.LastName  = "Frog";
			person.Gender    = Gender.Unknown;

			da./*[a]*/Insert(person)/*[/a]*/;
		}

		[Test]
		public void Test2()
		{
			using (DbManager db = new DbManager())
			{
				/*[a]*/DataAccessor da = new DataAccessor()/*[/a]*/;

				Person person = new Person();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Other;

				da./*[a]*/Insert(db, person)/*[/a]*/;
			}
		}
	}
}

