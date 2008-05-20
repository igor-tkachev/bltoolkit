using System;
using NUnit.Framework;
using BLToolkit.DataAccess;

namespace HowTo.DataAccess
{
	[TestFixture]
	public class ActionSprocName
	{
		[/*[a]*/ActionSprocName/*[/a]*/("GetByName", "Person_SelectByName")]
		public class Person
		{
			public int    PersonID;
			public string LastName;
			public string FirstName;
			public string MiddleName;
		}

		public abstract class PersonAccessor : DataAccessor
		{
			// Default action name is 'SelectByKey'.
			// Stored procedure name is 'Person_SelectByKey'.
			//
			public abstract Person SelectByKey(int @id);

			// Default action name is 'GetByName'.
			// Stored procedure name is 'Person_SelectByName'
			// defined by the ActionSprocName attribute of the Person class.
			//
			public abstract Person GetByName(string @firstName, string @lastName);
		}

		[Test]
		public void Test()
		{
			PersonAccessor pa = /*[a]*/DataAccessor/*[/a]*/./*[a]*/CreateInstance/*[/a]*/<PersonAccessor>();

			Person person1 = pa.SelectByKey(1);

			Assert.IsNotNull(person1);

			Person person2 = pa.GetByName(person1.FirstName, person1.LastName);

			Assert.AreEqual(person1.PersonID, person2.PersonID);
		}
	}
}
