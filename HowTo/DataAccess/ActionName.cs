using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ActionName
	{
		public abstract class PersonAccessor : DataAccessor<Person, PersonAccessor>
		{
			// Default action name is 'SelectByKey'.
			// Stored procedure name is 'Person_SelectByKey'.
			//
			public abstract Person SelectByKey(int @id);

			// Explicit action name is 'SelectByName'.
			// Stored procedure name is 'Person_SelectByName'.
			//
			[/*[a]*/ActionName/*[/a]*/(/*[a]*/"SelectByName"/*[/a]*/)]
			public abstract Person /*[a]*/AnyName/*[/a]*/    (string @firstName, string @lastName);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = /*[a]*/PersonAccessor.CreateInstance/*[/a]*/();

			Person person1 = pa.SelectByKey(1);

			Assert.IsNotNull(person1);

			Person person2 = pa.AnyName(person1.FirstName, person1.LastName);

			Assert.AreEqual(person1.ID, person2.ID);
		}
	}
}
