using System;
using System.Collections.Generic;

using NUnit.Framework;

using BLToolkit.Data;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;
using BLToolkit.Reflection;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class AbstractAccessor
	{
		public /*[a]*/abstract/*[/a]*/ class /*[a]*/PersonAccessor : DataAccessor<Person>/*[/a]*/
		{
			public /*[a]*/abstract/*[/a]*/ Person  /*[a]*/SelectByName/*[/a]*/(Person person);
			public /*[a]*/abstract/*[/a]*/ Person  /*[a]*/SelectByName/*[/a]*/(string firstName, string lastName);

			/*[a]*/new/*[/a]*/ public /*[a]*/abstract/*[/a]*/ int /*[a]*/Insert/*[/a]*/      (Person person);
		}

		[Test]
		public void Test()
		{
			BLToolkit.TypeBuilder.TypeFactory.SaveTypes = true;

			using (DbManager db = new DbManager())
			{
				PersonAccessor pa = /*[a]*/DataAccessor.CreateInstance<PersonAccessor>(db)/*[/a]*/;

				pa.BeginTransaction();

				// Insert and get id.
				//
				Person person = new Person();

				person.FirstName = "Crazy";
				person.LastName  = "Frog";
				person.Gender    = Gender.Unknown;

				int id = pa./*[a]*/Insert(person)/*[/a]*/;

				// SelectByName.
				//
				person = pa./*[a]*/SelectByName("Crazy", "Frog")/*[/a]*/;

				Assert.IsNotNull(person);

				// Delete.
				//
				pa.Delete(person);

				pa.CommitTransaction();
			}
		}
	}
}

